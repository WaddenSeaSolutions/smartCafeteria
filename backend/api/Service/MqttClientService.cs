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
    
    /// <summary>
    /// This method is used to connect and communicate with a Mqtt server.
    /// The method receives a string of numbers which is then parsed and seperated into a list.
    /// The numbers are used to create a order in the database.
    /// </summary>
    public async Task CommunicateWithBroker()
    {
        var mqttFactory = new MqttFactory();
        var mqttClient = mqttFactory.CreateMqttClient();
        
        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer("mqtt.flespi.io", 1883) 
            .WithCredentials("FlespiToken H2AG1ypT28ZX4gUZgktOb38QbqbQZvmIR37AFAxNhOyZUCO7u2xR140gBOjoCXSY", "")
            .WithProtocolVersion(MqttProtocolVersion.V500) 
            .Build();

        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(f => f.WithTopic("Cafeteria/CompleteOrder"))
            .WithTopicFilter(f => f.WithTopic("Cafeteria/GetOptions"))
            .Build();

        await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
        

        mqttClient.ApplicationMessageReceivedAsync += async e =>
        {
            try
            {
                var topic = e.ApplicationMessage.Topic;
                var message = e.ApplicationMessage.ConvertPayloadToString();
                Console.WriteLine("Message received:" + message);

                if (topic == "Cafeteria/CompleteOrder")
                {
                    HandleCompleteOrder(message, mqttClient, e);
                }else if (topic == "Cafeteria/GetOptions")
                {
                    HandleGetOrderOptions( mqttClient, e);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.InnerException);
                Console.WriteLine(exc.StackTrace);
                var errorMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("Cafeteria/Response")
                    .WithPayload("Something went wrong")
                    .WithQualityOfServiceLevel(e.ApplicationMessage.QualityOfServiceLevel)
                    .WithRetainFlag(e.ApplicationMessage.Retain)
                    .Build();
                await mqttClient.PublishAsync(errorMessage, CancellationToken.None);
            }
        };
    }
    
    private async void HandleCompleteOrder(string message, IMqttClient mqttClient, MqttApplicationMessageReceivedEventArgs e)
    {
        var orderNumbers = new List<int>();

        // Split the message
        foreach (var str in message.Split(','))
        {
            // Check if the string is not empty or whitespace
            if (!string.IsNullOrWhiteSpace(str))
            {
                // Try to parse the string as an integer
                if (int.TryParse(str, out int num))
                {
                    orderNumbers.Add(num);
                }
                else { Console.WriteLine($"Warning: Unable to parse '{str}' as an integer."); }
            }
            else { Console.WriteLine($"Warning: Ignoring empty or whitespace value in message: '{str}'"); }
        }

        // Create an OrderMqtt object
        var timestamp = DateTimeOffset.UtcNow;
        var userId = 1;
        OrderMqtt order = new OrderMqtt
        {
            Id = userId,
            Done = false,
            Payment = false,
            Timestamp = timestamp, 
            UserId = 1// Consider deserializing to a different type if needed
        };
        
        // Publishing a response message
        var pongMessage = new MqttApplicationMessageBuilder()
            .WithTopic("Cafeteria/Response")
            .WithPayload("Message received by the backend")
            .WithQualityOfServiceLevel(e.ApplicationMessage.QualityOfServiceLevel)
            .WithRetainFlag(e.ApplicationMessage.Retain)
            .Build();
        await mqttClient.PublishAsync(pongMessage, CancellationToken.None);
        
        var insertionResult = _mqttClientDal.CreateNewOrderFromMqtt(order);
        _mqttClientDal.AddContentToOrder(orderNumbers, insertionResult.Id);
    }

    private async void HandleGetOrderOptions(IMqttClient mqttClient, MqttApplicationMessageReceivedEventArgs e)
    {
        var orderOptionList = new List<string>();

        orderOptionList = _mqttClientDal.GetOrderOptions();
        
        var orderOptionsString = string.Join(",", orderOptionList);

        var pongMessage = new MqttApplicationMessageBuilder()
            .WithTopic("Cafeteria/OrderOptions")
            .WithPayload(orderOptionsString)
            .WithQualityOfServiceLevel(e.ApplicationMessage.QualityOfServiceLevel)
            .WithRetainFlag(e.ApplicationMessage.Retain)
            .Build();
        await mqttClient.PublishAsync(pongMessage, CancellationToken.None);
    }
}


public class MqttClientWantsToSendMessageToBackend
{
    public string message { get; set; }
    public int sender { get; set; }
}
