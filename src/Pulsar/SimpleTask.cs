using System;
using System.Collections.Generic;

namespace Codestellation.Pulsar
{
    public class SimpleTask : ITask
    {
        private readonly Guid _id;
        private readonly string _name;
        private readonly Action _action;
        private readonly List<ITrigger> _triggers;

        public SimpleTask(string name, Action action)
        {
            _name = name;
            _action = action;
            _id = Guid.NewGuid();
            _triggers = new List<ITrigger>();
        }

        public Guid Id
        {
            get { return _id; }
        }

        public string Name
        {
            get { return _name; }
        }

        public IEnumerable<ITrigger> Triggers
        {
            get { return _triggers; }
        }

        public void AddTrigger(ITrigger trigger)
        {
            _triggers.Add(trigger);
        }

        public void RemoveTrigger(ITrigger trigger)
        {
            _triggers.Remove(trigger);
        }

        public void Run()
        {
            _action();
        }
    }
}