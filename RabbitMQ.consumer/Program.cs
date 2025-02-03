using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory
{
    Uri = new Uri("amqps://xoagscos:E7j0IsuTnacByY-oAYIPcoQzQyabVU9W@shark.rmq.cloudamqp.com/xoagscos")
};

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

channel.QueueDeclareAsync("hello-queue", true, false, false);

var consumer = new AsyncEventingBasicConsumer(channel);
await channel.BasicConsumeAsync("hello-queue", true, consumer);

consumer.ReceivedAsync += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine("Gelen mesaj: " + message);
    return Task.CompletedTask;
};



Console.ReadLine();
