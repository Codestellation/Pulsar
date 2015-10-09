using System;

namespace Codestellation.Pulsar.Triggers
{
    /// <summary>
    /// Calculates next <see cref="DateTime"/> to trigger an event
    /// </summary>
    public interface ISchedule
    {
        /// <summary>
        /// Returns next datetime if possible
        /// <remarks>Should return <see cref="DateTimeKind.Utc"/> <see cref="DateTime"/></remarks>
        /// </summary>
        DateTime? NextAt { get; }
    }
}