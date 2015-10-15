using System;
using Codestellation.Pulsar.Diagnostics;

namespace Codestellation.Pulsar.Triggers
{
    /// <summary>
    /// Base class for building <see cref="ITrigger"/> implmementations
    /// </summary>
    public abstract class AbstractTrigger : ITrigger
    {
        private static readonly PulsarLogger Logger = PulsarLogManager.GetLogger<AbstractTrigger>();

        private TriggerCallback _callback;
        private bool _started;

        /// <summary>
        ///  Forces trigger to callback at specified schedule
        /// </summary>
        /// <param name="callback"></param>
        public void Start(TriggerCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            _callback = callback;

            if (Logger.IsInfoEnabled)
            {
                Logger.Info($"Started");
            }

            OnStart();
            _started = true;
        }

        /// <summary>
        /// Called from <see cref="Start"/> when parameter validation succeed
        /// </summary>
        protected virtual void OnStart()
        {
        }

        /// <summary>
        ///  Stops trigger
        /// </summary>
        public void Stop()
        {
            _started = false;
            OnStop();

            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Stopped");
            }
        }

        /// <summary>
        /// Called when <see cref="Stop"/> is called
        /// </summary>
        protected virtual void OnStop()
        {
        }

        /// <summary>
        /// Invokes callback supplied in <see cref="Start"/> method if trigger is started
        /// </summary>
        protected void InvokeCallback()
        {
            if (_started)
            {
                if (Logger.IsInfoEnabled)
                {
                    Logger.Info("Trigger callback invoked");
                }

                var context = new TriggerContext(this);
                _callback(context);
            }
        }
    }
}