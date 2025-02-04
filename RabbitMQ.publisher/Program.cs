

using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory
{
    Uri = new Uri("amqps://xoagscos:E7j0IsuTnacByY-oAYIPcoQzQyabVU9W@shark.rmq.cloudamqp.com/xoagscos")
};

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();


//await channel.QueueDeclareAsync("hello-queue", true, false, false);
//burada kuyruk oluşturmuyoruz.Senaryomuzda consumerların kendin kuyruklarını oluşturup exchange bağlandığını varsayıyoruz.

//durable fiziksel olarak kayıt edilip edilmeyeceği(true olursa uygulama restart atılsa bile exchangeler kaybolmaz)
await channel.ExchangeDeclareAsync("logs-fanout",ExchangeType.Fanout, true);


Enumerable.Range(1, 50).ToList().ForEach(async x =>
{
    string message = $"log {x}";
    var messageBody = Encoding.UTF8.GetBytes(message);

    await channel.BasicPublishAsync("logs-fanout", "", messageBody);

    Console.WriteLine($"Mesaj gönderildi: {message}");
});


Console.ReadLine();