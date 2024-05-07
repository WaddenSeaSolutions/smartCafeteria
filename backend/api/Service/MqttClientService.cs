using System.Text.Json;
using backend.DAL;
using backend.Model;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;

namespace backend.Service;

public class MqttClientService
{
    private readonly MqttClientDAL _mqttClientDal;

    public MqttClientService(MqttClientDAL mqttClientDal)
    {
        _mqttClientDal = mqttClientDal;
    }
    public async Task CommunicateWithBroker()
    {
        var mqttFactory = new MqttFactory();
        var mqttClient = mqttFactory.CreateMqttClient();
        
        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer("mqtt.flespi.io", 1883) // Set the server and port
            .WithCredentials("FlespiToken H2AG1ypT28ZX4gUZgktOb38QbqbQZvmIR37AFAxNhOyZUCO7u2xR140gBOjoCXSY", "") // Add credentials
            .WithProtocolVersion(MqttProtocolVersion.V500) // Use MQTT 5.0
            .Build();

        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
        Console.WriteLine("HJÆÆÆÆÆÆÆÆÆÆÆÆÆÆLP");

        var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(f => f.WithTopic("Cafeteria/CompleteOrder")).Build();

        await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
        
        Console.WriteLine("HJÆÆÆÆÆÆÆÆÆÆÆÆÆÆLP");

        mqttClient.ApplicationMessageReceivedAsync += async e =>
        {
            try
            {
                var message = e.ApplicationMessage.ConvertPayloadToString();
                Console.WriteLine("Message received:" + message);
                var messageObject = JsonSerializer.Deserialize<String>(message);
                var orderNumbers = message.Split(",").Select(int.Parse).ToList();
                var timestamp = DateTimeOffset.UtcNow;
                var userId = 1;
                OrderMqtt order = new OrderMqtt
                {
                    Id = userId,
                    Done = false,
                    Payment = false,
                    Timestamp = timestamp,
                    Options = messageObject
                };
                var pongMessage = new MqttApplicationMessageBuilder().WithTopic("response_topic")
                    .WithPayload("Message received by the backend")
                    .WithQualityOfServiceLevel(e.ApplicationMessage.QualityOfServiceLevel)
                    .WithRetainFlag(e.ApplicationMessage.Retain)
                    .Build();
                await mqttClient.PublishAsync(pongMessage, CancellationToken.None);
                
                var insertionResult = _mqttClientDal.CreateNewOrderFromMqtt(order);
                
                _mqttClientDal.AddContentToOrder(orderNumbers,insertionResult.Id);
                Console.WriteLine("HJÆÆÆÆÆÆÆÆÆÆÆÆÆÆLP");
                
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.InnerException);
                Console.WriteLine(exc.StackTrace);
            }
        };
    }
}

public class MqttClientWantsToSendMessageToBackend
{
    public string message { get; set; }
    public int sender { get; set; }
}