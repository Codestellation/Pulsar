using System;

namespace Codestellation.Pulsar.Cron
{
    public interface ICronField
    {
        int GetClosestTo(DateTime point);
    }
}