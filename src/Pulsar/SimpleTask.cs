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

        public SimpleTask(string name, Action action) : this(action)
        {
            _name = string.IsNullOrWhiteSpace(name) ? string.Format("Task {0}", _id) : name;
        }

        public SimpleTask(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
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

        public SimpleTask AddTrigger(ITrigger trigger)
        {
            if (trigger == null)
            {
                throw new ArgumentNullException("trigger");
            }
            _triggers.Add(trigger);
            return this;
        }

        public SimpleTask RemoveTrigger(ITrigger trigger)
        {
            if (trigger == null)
            {
                throw new ArgumentNullException("trigger");
            }
            _triggers.Remove(trigger);
            return this;
        }

        public void Run()
        {
            _action();
        }
    }
}