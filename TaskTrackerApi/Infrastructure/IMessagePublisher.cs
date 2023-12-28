using System.Collections.Generic;
using TaskTrackerApi.Models;

namespace TaskTrackerApi.Infrastructure
{
    public interface IMessagePublisher
    {
        void PublishTaskStatusChangedMessage(int userId, string currentStatus, string topic);
    }
}