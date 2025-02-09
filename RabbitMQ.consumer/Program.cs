using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;
using System.Text;
using System.Text.Json;

var factory = new ConnectionFactory
{
    Uri = new Uri("amqps://xoagscos:E7j0IsuTnacByY-oAYIPcoQzQyabVU9W@shark.rmq.cloudamqp.com/xoagscos")
};

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();


await channel.BasicQosAsync(0, 1, false);
var consumer = new AsyncEventingBasicConsumer(channel);

//kuyruğu consumer tarafında oluşturduk.topic yapısında çok fazla seçenek olduğu için bu şekilde yaptık fakat senaryayo göre kuyruğun nerede oluşturulacağı değişiklik gösterebilir.
var queueDeclareResult = await channel.QueueDeclareAsync();
var queueName = queueDeclareResult.QueueName;

//sadece ortası Error olanları almak için.
var routeKey = "contextTypeTest";

channel.QueueBindAsync(queueName, "complex-type", "contextTypeTest");

await channel.BasicConsumeAsync(queueName, false, consumer);

consumer.ReceivedAsync += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var product = JsonSerializer.Deserialize<Product>(message);
    Thread.Sleep(1000);
    Console.WriteLine($"Gelen mesaj: {product.Id}  {product.Name} {product.Price} {product.Stock}");

    channel.BasicAckAsync(ea.DeliveryTag, false);

    return Task.CompletedTask;
};

Console.ReadLine();
