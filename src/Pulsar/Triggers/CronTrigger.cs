using System;
using Codestellation.Pulsar.Cron;

namespace Codestellation.Pulsar.Triggers
{
    public class CronTrigger : ITrigger
    {
        private TriggerCallback _callback;
        private CronExpression _cronExpression;

        public CronTrigger(string cronExpression) : this(new CronExpression(cronExpression))
        {
            
        }

        public CronTrigger(CronExpression cronExpression)
        {
            if (cronExpression == null)
            {
                throw new ArgumentNullException("cronExpression");
            }
            _cronExpression = cronExpression;
        }

        public DateTime? LastFiredAt
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime? NextFireAt
        {
            get { throw new NotImplementedException(); }
        }

        public void Start(TriggerCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            _callback = callback;
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}