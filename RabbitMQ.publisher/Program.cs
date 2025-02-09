

using RabbitMQ.Client;
using RabbitMQ.publisher;
using System.Text;



var factory = new ConnectionFactory
{
    Uri = new Uri("amqps://xoagscos:E7j0IsuTnacByY-oAYIPcoQzQyabVU9W@shark.rmq.cloudamqp.com/xoagscos")
};

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync("logs-topic", ExchangeType.Topic,durable: true);
Random rnd = new Random();
Enumerable.Range(1, 50).ToList().ForEach(async x =>
{
    LogNames log1 = (LogNames)rnd.Next(1, 5);
    LogNames log2 = (LogNames)rnd.Next(1, 5);
    LogNames log3 = (LogNames)rnd.Next(1, 5);

    var routeKey = $"{log1}.{log2}.{log3}";
    string message = $"Log-type: {log1}-{log2}-{log3}";
    var messageBody = Encoding.UTF8.GetBytes(message);
    await channel.BasicPublishAsync( "logs-topic", routeKey, messageBody);

    Console.WriteLine($"Log gönderildi: {message}");
});


Console.ReadLine();