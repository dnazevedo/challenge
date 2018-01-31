# Challenge WebAPI using RabbitMQ


API that receives a request and publishes its body in a defined exchange and route in request HTTP headers, containing an SDK abstracting the publication in the created endpoint. Creates a consumer who writes in a text file the messages published using the native RabbitMQ.Client, without abstractions.
