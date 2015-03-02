using System;

namespace Codestellation.Pulsar.Triggers
{
    public class CronTrigger : ITrigger
    {
        public DateTime? LastFiredAt
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime? NextFireAt
        {
            get { throw new NotImplementedException(); }
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void SetCallback(TriggerCallback callback)
        {
            throw new NotImplementedException();
        }
    }
}