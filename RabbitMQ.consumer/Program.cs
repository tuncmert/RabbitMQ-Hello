using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory
{
    Uri = new Uri("amqps://xoagscos:E7j0IsuTnacByY-oAYIPcoQzQyabVU9W@shark.rmq.cloudamqp.com/xoagscos")
};

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

//burada tekrar kuyruk oluşturmaya gerek yok fakat eğer publisher oluşturmadıysa buradan oluşturulabilir.
await channel.QueueDeclareAsync("hello-queue", true, false, false);

//true olursa girilen sayıyı toplam mesaj sayısı olarak gönderir, false olursa her bir consumera girilen sayı kadar sırayla gönderir.
await channel.BasicQosAsync(0, 1, false);
var consumer = new AsyncEventingBasicConsumer(channel);

//mesaj geldiğinde hemen silinip silinmeyeği false-true
await channel.BasicConsumeAsync("hello-queue", false, consumer);

consumer.ReceivedAsync += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Thread.Sleep(1000);
    Console.WriteLine("Gelen mesaj: " + message);

    //artık mesajı silmesi için rabbitmqye gönderiyoruz
    channel.BasicAckAsync(ea.DeliveryTag, false);

    return Task.CompletedTask;
};

Console.ReadLine();
