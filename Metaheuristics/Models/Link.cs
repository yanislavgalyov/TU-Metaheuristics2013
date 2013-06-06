using System;
using System.Collections.Generic;
using System.Text;

namespace Metaheuristics.Models
{
    /// <summary>
    /// Връзка между два града в маршрут
    /// </summary>
    public class Link
    {
        private int connection1;
        /// <summary>
        /// Към първи град.
        /// </summary>
        public int Connection1
        {
            get
            {
                return connection1;
            }
            set
            {
                connection1 = value; ;
            }
        }

        private int connection2;
        /// <summary>
        /// Към втори град.
        /// </summary>
        public int Connection2
        {
            get
            {
                return connection2;
            }
            set
            {
                connection2 = value;
            }
        }
    }
}
