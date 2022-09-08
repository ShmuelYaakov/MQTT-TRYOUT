using MQTTnet;
using MQTTnet.Server;
using System.Text;

namespace MQTTBroker
{
    internal class Broker
    {
        static void Main(string[] args)
        {
            var options = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .WithDefaultEndpointPort(707)
                .Build();

            var mqttFactory = new MqttFactory();
            var mqttServer = mqttFactory.CreateMqttServer(options);

            mqttServer.InterceptingPublishAsync += e =>
            {
                Console.WriteLine($"Received publish request - {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                return Task.CompletedTask;
            };

            mqttServer.ClientConnectedAsync += e =>
            {
                Console.WriteLine($"ClientId = {e.ClientId}, Endpoint = {e.Endpoint}");
                return Task.CompletedTask;
            };

            mqttServer.StartAsync();

            Console.ReadLine();
        }
    }
}