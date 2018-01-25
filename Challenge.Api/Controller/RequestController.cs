using Challenge.SDK.RequestInputs;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace Challenge.Api.Controller 
{
    [Route("api/[controller]")]
    public class RequestController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IModel _channel;

        public RequestController(IModel channel)
        {
            _channel = channel;
        }

        [HttpPost]
        public IActionResult Post([FromBody] TextRequest request)
        {
            var messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(request.Text);

            var exchange = Request.Headers["exchange"];
            var route = Request.Headers["route-key"];

            _channel.BasicPublish(exchange, route, null, messageBodyBytes);

            return Accepted("Challenge.txt");
        }
    }
}
