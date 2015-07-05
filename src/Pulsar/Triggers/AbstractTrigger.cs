using System;

namespace Codestellation.Pulsar.Triggers
{
    public abstract class AbstractTrigger : ITrigger
    {
        private TriggerCallback _callback;

        public void Start(TriggerCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            _callback = callback;

            OnStart();
        }

        protected virtual void OnStart()
        {
        }

        public void Stop()
        {
            OnStop();
        }

        protected virtual void OnStop()
        {
        }

        protected void InvokeCallback()
        {
            var context = new TriggerContext(this);
            _callback(context);
        }
    }
}