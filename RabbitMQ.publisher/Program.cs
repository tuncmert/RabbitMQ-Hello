

using RabbitMQ.Client;
using Shared;
using System.Text;
using System.Text.Json;



var factory = new ConnectionFactory
{
    Uri = new Uri("amqps://xoagscos:E7j0IsuTnacByY-oAYIPcoQzQyabVU9W@shark.rmq.cloudamqp.com/xoagscos")
};

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync("complex-type", ExchangeType.Topic,durable: true);
var product = new Product { Id = 1, Name = "Kalem", Price = 10, Stock = 10 };
var productJsonString= JsonSerializer.Serialize(product);
var routeKey = "contextTypeTest";

var messageBody = Encoding.UTF8.GetBytes(productJsonString);
    await channel.BasicPublishAsync("complex-type", routeKey, messageBody);

    Console.WriteLine($"Mesaj gönderildi");


Console.ReadLine();