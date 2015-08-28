using Codestellation.Pulsar.Triggers;

namespace Codestellation.Pulsar
{
    /// <summary>
    /// Encapsulates any method to run <see cref="ITask"/>
    /// </summary>
    public interface ITrigger
    {
        /// <summary>
        /// Forces trigger to call <see cref="TriggerCallback"/> on internal events.
        /// </summary>
        /// <param name="callback">A function to be called when <see cref="ITrigger"/> fires</param>
        void Start(TriggerCallback callback);

        /// <summary>
        /// Prevents trigger from calling <see cref="TriggerCallback"/>.
        /// </summary>
        void Stop();
    }
}