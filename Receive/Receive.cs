﻿using System;
using System.Text;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace Receive
{
  class Receive
  {
    static void Main(string[] args)
    {
      var factory = new ConnectionFactory { HostName = "localhost" };

      using (var connection = factory.CreateConnection())
      using (var channel = connection.CreateModel())
      {
        channel.QueueDeclare(queue: "hello",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, eventArgs) =>
        {
          var body = eventArgs.Body.ToArray();
          var message = Encoding.UTF8.GetString(body);
          Console.WriteLine($" [x] Received {message}");
        };

        channel.BasicConsume(queue: "hello",
                            autoAck: true,
                            consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
      }
    }
  }
}