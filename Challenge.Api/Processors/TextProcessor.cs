using System.IO;
using RabbitMQ.Client;


namespace Challenge.Api.Processors
{
    public class TextProcessor
    {
        private readonly IModel _channel;

        public TextProcessor(IModel channel)
        {
            _channel = channel;
        }

        public void Process(byte[] message)
        {
            var result = System.Text.Encoding.UTF8.GetString(message);

            File.AppendAllLines("C:\\Users\\daiane.azevedo\\Documents\\\\Challenge.txt", new[] { result });
        }
    }
}
