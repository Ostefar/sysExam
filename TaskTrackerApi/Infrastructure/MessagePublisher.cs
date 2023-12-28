using System;
using System.Collections.Generic;
using EasyNetQ;
using TaskTrackerApi.Models;
using SharedModels;

namespace TaskTrackerApi.Infrastructure
{
    public class MessagePublisher : IMessagePublisher, IDisposable
    {
        IBus bus;

        public MessagePublisher(string connectionString)
        {
            bus = RabbitHutch.CreateBus(connectionString);
        }

        public void Dispose()
        {
            bus.Dispose();
        }

        public void PublishTaskStatusChangedMessage(int userId, string topic)
        {
            var message = new TaskStatusChangedMessage
            {
                UserId = userId,
            };

            bus.PubSub.Publish(message, topic);
        }
    }
}