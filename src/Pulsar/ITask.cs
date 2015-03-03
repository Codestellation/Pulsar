using System;
using System.Collections.Generic;

namespace Codestellation.Pulsar
{
    public interface ITask
    {
        Guid Id { get; }

        string Name { get; }

        IEnumerable<ITrigger> Triggers { get; }

        void Run();
    }
}