using System.Threading.Tasks;

namespace Codestellation.Pulsar.Triggers
{
    /// <summary>
    /// Fires callback when called <see cref="Fire"/> only
    /// </summary>
    public class ManualTrigger : AbstractTrigger
    {
        /// <summary>
        /// Forces trigger to fire callback.
        /// <remarks>Callback will be called using thread pool thread</remarks>
        /// </summary>
        public void Fire()
        {
            Task.Run(() => InvokeCallback());
        }
    }
}