using System.Text.Json;
using backend.DAL;
using backend.Model;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;

namespace backend.Service;

public class MqttClientService
{
    private readonly MQTTClientDAL _mqttClientDal;

    public MqttClientService(MQTTClientDAL mqttClientDal)
    {
        _mqttClientDal = mqttClientDal;
    }
    public async Task CommunicateWithBroker()
    {
        var mqttFactory = new MqttFactory();
        var mqttClient = mqttFactory.CreateMqttClient();
        var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("localhost", 1883)
            .WithProtocolVersion(MqttProtocolVersion.V500).Build();

        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(f => f.WithTopic("Cafeteria/CompleteOrder")).Build();

        await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

        mqttClient.ApplicationMessageReceivedAsync += async e =>
        {
            try
            {
                var message = e.ApplicationMessage.ConvertPayloadToString();
                Console.WriteLine("Message received:" + message);
                var messageObject = JsonSerializer.Deserialize<MqttClientWantsToSendMessageToRoom>(message);
                var orderNumbers = message.Split(",").Select(int.Parse).ToList();
                var timestamp = DateTimeOffset.UtcNow;
                var userId = 1;
                OrderMQTT order = new OrderMQTT
                {
                    Id = userId,
                    Done = false,
                    Payment = false,
                    Timestamp = timestamp,
                    Options = messageObject.message
                };
                
                
                var insertionResult = _mqttClientDal.CreateNewOrderFromMQTT(order);
                
                _mqttClientDal.AddContentToOrder(orderNumbers,insertionResult.Id);
                
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

public class MqttClientWantsToSendMessageToRoom
{
    public string message { get; set; }
    public int sender { get; set; }
}