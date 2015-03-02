using System.Collections.Generic;

namespace Codestellation.Pulsar
{
    public interface ITask
    {
        string Name { get; }

        IEnumerable<ITrigger> Triggers { get; }

        void Run();
    }
}