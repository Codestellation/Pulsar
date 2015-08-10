using System;

namespace Codestellation.Pulsar.Triggers
{
    public abstract class AbstractTrigger : ITrigger
    {
        private TriggerCallback _callback;
        private bool _started;

        public void Start(TriggerCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            _callback = callback;

            OnStart();
            _started = true;
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
            if (_started)
            {
                var context = new TriggerContext(this);
                _callback(context);
            }
        }
    }
}