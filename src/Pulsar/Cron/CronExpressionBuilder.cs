namespace Codestellation.Pulsar.Cron
{
    /// <summary>
    /// Encapsulates logic to build valid cron expression
    /// <remarks> See http://www.quartz-scheduler.org/documentation/quartz-1.x/tutorials/crontrigger for details of cron expressions</remarks>
    /// </summary>
    public class CronExpressionBuilder
    {
        /// <summary>
        /// Seconds part of Cron Expression. Possible values are: 0-59, Special characters: , - * /
        /// </summary>
        public string Seconds { get; set; }

        /// <summary>
        /// Minutes part of Cron Expression. Possible values are: 0-59, Special characters: , - * /
        /// </summary>
        public string Minutes { get; set; }

        /// <summary>
        /// Hours part of Cron Expression. Possible values are: 0-23, Special characters: , - * ? /
        /// </summary>
        public string Hours { get; set; }

        /// <summary>
        /// DayOfMonth part of Cron Expression. Possible values are: 1-31, Special characters: , - * ? / L W
        /// </summary>
        public string DayOfMonth { get; set; }

        /// <summary>
        /// Month part of Cron Expression. Possible values are: 1-12, Special characters: , - * ? /
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// DayOfWeek part of Cron Expression, Possible values are: 1-7, Special characters: , - * ? / L #
        /// </summary>
        public string DayOfWeek { get; set; }

        /// <summary>
        /// Year part of Cron Expression.  Possible values are: 1970-2099, Special characters: , - * /
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Returns generated cron expression string
        /// </summary>
        public override string ToString()
        {
            return $"{Seconds} {Minutes} {Hours} {DayOfMonth} {Month} {DayOfWeek} {Year}";
        }

        /// <summary>
        /// Creates new <see cref="CronExpression"/>
        /// </summary>
        public CronExpression ToCronExpression()
        {
            return new CronExpression(ToString());
        }
    }
}