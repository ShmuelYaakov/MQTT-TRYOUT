using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

namespace MQTTPublisher
{
    internal class Publisher
    {
        static async Task Main(string[] args)
        {
            var mqttFactory = new MqttFactory();
            var client = mqttFactory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                //.WithTcpServer("test.mosquitto.org", 1883)
                .WithTcpServer("localhost", 707)
                .WithCleanSession()
                .Build();

            client.ConnectedAsync += (e) =>
            {
                Console.WriteLine("Connected to the broker successfully");
                return Task.CompletedTask;
            };

            client.DisconnectedAsync += (e) =>
            {
                Console.Write("Disconnected from the broker successfully");
                return Task.CompletedTask;
            };

            await client.ConnectAsync(options);

            Console.WriteLine("Please press a key to publish the message");

            var mmmm = Console.ReadLine();

            await PublishMessageAsync(client, mmmm);

            await client.DisconnectAsync();
        }

        private static async Task PublishMessageAsync(IMqttClient client, string mmmm)
        {
            string messagePayload = mmmm;
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("SYW")
                .WithPayload(messagePayload)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            if (client.IsConnected)
            {
                await client.PublishAsync(message);
            }
        }
    }
}