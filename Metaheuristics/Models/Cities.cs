using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Globalization;

namespace Metaheuristics.Models
{
    /// <summary>
    /// Списък с градовете
    /// </summary>
    public class Cities : List<City>
    {
        /// <summary>
        /// Изчисляване на разстоянието между всеки два града.
        /// </summary>
        public void CalculateCityDistances(int numberOfCloseCities)
        {
            foreach (City city in this)
            {
                city.Distances.Clear();

                for (int i = 0; i < Count; i++)
                {
                    city.Distances.Add(Math.Sqrt(Math.Pow((double)(city.Location.X - this[i].Location.X), 2D) + Math.Pow((double)(city.Location.Y - this[i].Location.Y), 2D)));
                }
            }

            foreach (City city in this)
            {
                city.FindClosestCities(numberOfCloseCities);
            }
        }
    }
}
