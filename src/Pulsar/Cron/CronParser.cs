using System;

namespace Codestellation.Pulsar.Cron
{
    public static class CronParser
    {
        public static int ParseNumber(string token, ref int index, int min, int max)
        {
            if (char.IsDigit(token[index]))
            {
                var result = ParseDigit(token[index]);
                index++;
                if (index < token.Length && char.IsDigit(token[index]))
                {
                    result = result * 10 + ParseDigit(token[index]);
                    index++;
                }

                if (result < min || max < result)
                {
                    throw new FormatException();
                }
                return result;
            }

            throw new FormatException();
        }

        private static int ParseDigit(char value)
        {
            return Convert.ToInt32(value) - 48;
        }
    }
}