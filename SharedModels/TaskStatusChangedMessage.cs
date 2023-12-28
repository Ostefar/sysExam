using System;
using System.Collections.Generic;

namespace SharedModels
{
    public class TaskStatusChangedMessage
    {
        public int UserId { get; set; }

        public string CurrentStatus { get; set; }

    }
}
