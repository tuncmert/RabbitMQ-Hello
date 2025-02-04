using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory
{
    Uri = new Uri("amqps://xoagscos:E7j0IsuTnacByY-oAYIPcoQzQyabVU9W@shark.rmq.cloudamqp.com/xoagscos")
};

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();



QueueDeclareOk queueDeclareResult = await channel.QueueDeclareAsync();
string randomQueueName = queueDeclareResult.QueueName;

//Aşağıdaki şekilde kuyruk oluşturursak kuyruk kalıcı olur. Oluşturmazsak bağlantı gidince kuyrukta kaybolur
//await channel.QueueDeclareAsync(randomQueueName,true,false,false);

await channel.QueueBindAsync(randomQueueName, "logs-fanout", "");

await channel.BasicQosAsync(0, 1, false);
var consumer = new AsyncEventingBasicConsumer(channel);


await channel.BasicConsumeAsync(randomQueueName, false, consumer);


Console.WriteLine("Loglar alınıyor...");
consumer.ReceivedAsync += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Thread.Sleep(1000);
    Console.WriteLine("Gelen mesaj: " + message);

    channel.BasicAckAsync(ea.DeliveryTag, false);

    return Task.CompletedTask;
};

Console.ReadLine();
