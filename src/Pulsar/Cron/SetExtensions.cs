using System.Collections.Generic;

namespace Codestellation.Pulsar.Cron
{
    public static class SetExtensions
    {
        public static void AddRange<T>(this ISet<T> self, IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                self.Add(value);
            }
        }
    }
}