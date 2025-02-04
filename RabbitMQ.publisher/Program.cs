

using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory
{
    Uri = new Uri("amqps://xoagscos:E7j0IsuTnacByY-oAYIPcoQzQyabVU9W@shark.rmq.cloudamqp.com/xoagscos")
};

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync("hello-queue", true, false, false);

Enumerable.Range(1, 50).ToList().ForEach(async x =>
{
    string message = $"Message {x}";
    var messageBody = Encoding.UTF8.GetBytes(message);

    await channel.BasicPublishAsync(string.Empty, "hello-queue", messageBody);

    Console.WriteLine($"Mesaj gönderildi: {message}");
});


Console.ReadLine();