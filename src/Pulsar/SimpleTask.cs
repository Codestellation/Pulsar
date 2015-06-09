using System;
using System.Collections.Generic;

namespace Codestellation.Pulsar
{
    public class SimpleTask : ITask
    {
        private readonly Guid _id;
        private readonly string _name;
        private readonly Action _action;
        private readonly HashSet<ITrigger> _triggers;

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
            _triggers = new HashSet<ITrigger>();
        }

        public virtual Guid Id
        {
            get { return _id; }
        }

        public virtual string Title
        {
            get { return _name; }
        }

        public virtual IEnumerable<ITrigger> Triggers
        {
            get { return _triggers; }
        }

        public virtual SimpleTask AddTrigger(ITrigger trigger)
        {
            if (trigger == null)
            {
                throw new ArgumentNullException("trigger");
            }
            _triggers.Add(trigger);
            return this;
        }

        public virtual SimpleTask RemoveTrigger(ITrigger trigger)
        {
            if (trigger == null)
            {
                throw new ArgumentNullException("trigger");
            }
            _triggers.Remove(trigger);
            return this;
        }

        public virtual void Run()
        {
            _action();
        }
    }
}