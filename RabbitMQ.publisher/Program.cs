

using RabbitMQ.Client;
using RabbitMQ.publisher;
using System.Text;



var factory = new ConnectionFactory
{
    Uri = new Uri("amqps://xoagscos:E7j0IsuTnacByY-oAYIPcoQzQyabVU9W@shark.rmq.cloudamqp.com/xoagscos")
};

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync("logs-direct", ExchangeType.Direct,durable: true);
Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
{
    var routeKey = $"route-{x}";
    var queueName = $"direct-queue-{x}";
    channel.QueueDeclareAsync(queueName, true, false, false);
    channel.QueueBindAsync(queueName, "logs-direct", routeKey);
});

Enumerable.Range(1, 50).ToList().ForEach(async x =>
{
    LogNames log = (LogNames)new Random().Next(1,5);
    string message = $"Log-type: {log}";
    var messageBody = Encoding.UTF8.GetBytes(message);
    var routeKey = $"route-{log}";
    await channel.BasicPublishAsync( "logs-direct", routeKey, messageBody);

    Console.WriteLine($"Log gönderildi: {message}");
});


Console.ReadLine();