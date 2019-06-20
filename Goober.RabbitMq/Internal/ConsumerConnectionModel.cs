﻿using EasyNetQ;
using System;

namespace Goober.RabbitMq.Internal
{
    class ConsumerConnectionModel
    {
        public IBus Bus { get; set; }

        public ISubscriptionResult SubscriptionResult { get; set; }

        public Type HandlerType { get; set; }

        public ushort MaxParallelHandlers { get; set; }

        public ushort PrefetchCount { get; set; }

        public ushort MessageProcessRetryCount { get; set; }

        public uint MessageProccessRetryDelayInMilliseconds { get; set; }
    }
}
