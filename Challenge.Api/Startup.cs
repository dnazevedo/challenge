using System.ComponentModel.Design;
using System.Net;
using Challenge.Api.Processors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing.Impl;
using Swashbuckle.AspNetCore.Swagger;

namespace Challenge.Api
{
    public class Startup
    {
        private string _exchangeName = "Challenge";

        private readonly IModel _channel;

        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment host)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(host.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings{host.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            _channel = BuildConnection().CreateModel();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            ConfigureRabbit();
            services.AddSingleton(x => _channel);

            ConfigureMvc(services);
        }

        private void ConfigureRabbit()
        {
            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct);

            QueueDeclare(_exchangeName, "resolve-write-txt", "send-write-txt");

            ConfigureConsumer("resolve-write-txt");
        }

        private void ConfigureConsumer(string queueName)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body;
                var textProcessor = new TextProcessor(_channel);
                textProcessor.Process(body);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queueName, false, consumer);
        }

        private IConnection BuildConnection()
        {
            var factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                HostName = "localhost",
            };

            return factory.CreateConnection();
        }

        private void QueueDeclare(string exchangeName, string queueName, string route)
        {
            _channel.QueueDeclare(queueName, false, false, false, null);
            _channel.QueueBind(queueName, exchangeName, route, null);
        }

        public virtual void ConfigureMvc(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info()
                {
                    Description = "This document describes the operations available from Challenge API.",
                    Title = "Challenge API",
                    Version = "1",
                    Contact = new Contact
                    {
                        Email = "",
                        Name = ""
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandler(
                options =>
                {
                    options.Run(
                        async context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.ContentType = "application/json";
                            var ex = context.Features.Get<IExceptionHandlerFeature>();
                        }
                    );
                }
            );

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Challenge API");
            });
        }

    }
}