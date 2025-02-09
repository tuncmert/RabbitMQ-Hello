using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory
{
    Uri = new Uri("amqps://xoagscos:E7j0IsuTnacByY-oAYIPcoQzQyabVU9W@shark.rmq.cloudamqp.com/xoagscos")
};

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();


await channel.BasicQosAsync(0, 1, false);
var consumer = new AsyncEventingBasicConsumer(channel);
var queueName = "direct-queue-Critical";


await channel.BasicConsumeAsync(queueName, false, consumer);

consumer.ReceivedAsync += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Thread.Sleep(1000);
    Console.WriteLine("Gelen mesaj: " + message);

    //logları bir txt dosyasına kayıt için 
    //File.AppendAllText("log-critical.txt", message + "\n");


    //artık mesajı silmesi için rabbitmqye gönderiyoruz
    channel.BasicAckAsync(ea.DeliveryTag, false);

    return Task.CompletedTask;
};

Console.ReadLine();
