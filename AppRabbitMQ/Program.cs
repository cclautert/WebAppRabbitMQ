using AppRabbitMQ.Domain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace AppRabbitMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "OrdemQueue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var ordem = JsonSerializer.Deserialize<Ordem>(message);

                        Console.WriteLine($" Ordem: {ordem.Numero} | {ordem.Produto} | {ordem.ValorTotal:N2}");

                        channel.BasicAck(ea.DeliveryTag, false);
                    }catch(Exception ex)
                    {
                        channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                };
                channel.BasicConsume(queue: "OrdemQueue",
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
