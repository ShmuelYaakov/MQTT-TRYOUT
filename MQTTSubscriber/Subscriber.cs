using MQTTnet;
using MQTTnet.Client;
using System.Text;

namespace MQTTSubscriber
{
    internal class Subscriber
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

            client.ConnectedAsync += async (e) =>
            {
                Console.WriteLine("Connected to the broker successfully");
                var topicFilter = new MqttTopicFilterBuilder()
                    .WithTopic("SYW")
                    .Build();

                await client.SubscribeAsync(topicFilter);
            };

            client.DisconnectedAsync += (e) =>
            {
                Console.Write("Disconnected from the broker successfully");
                return Task.CompletedTask;
            };

            client.ApplicationMessageReceivedAsync += e =>
            {
                Console.WriteLine($"Received message - {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                return Task.CompletedTask;
            };


            await client.ConnectAsync(options);

            Console.ReadLine();

            await client.DisconnectAsync();
        }
    }
}