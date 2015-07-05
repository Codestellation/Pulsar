namespace Codestellation.Pulsar.Triggers
{
    public class ManualTrigger : AbstractTrigger
    {
        public void Fire()
        {
            InvokeCallback();
        }
    }
}