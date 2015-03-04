using System.Collections.Generic;

namespace Codestellation.Pulsar
{
    public interface IScheduler
    {
        IEnumerable<ITask> Tasks { get; } 
        
        IScheduler Add(ITask task);

        IScheduler Remove(ITask task);
    }
}