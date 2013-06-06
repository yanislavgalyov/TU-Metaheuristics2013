using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Metaheuristics.Models
{
    class Population : List<Tour>
    {
        private Tour bestTour = null;
        /// <summary>
        /// Най добрият маршрут
        /// </summary>
        public Tour BestTour
        {
            set
            {
                bestTour = value;
            }
            get
            {
                return bestTour;
            }
        }

        /// <summary>
        /// Метод за създаване на първоначалната популация
        /// </summary>
        public void CreateRandomPopulation(int populationSize, Cities cityList, Random rand, int chanceToUseCloseCity)
        {
            int firstCity, lastCity, nextCity;

            for (int tourCount = 0; tourCount < populationSize; tourCount++)
            {
                Tour tour = new Tour(cityList.Count);

                // Начален град
                firstCity = rand.Next(cityList.Count);
                lastCity = firstCity;

                for (int city = 0; city < cityList.Count - 1; city++)
                {
                    do
                    {
                        if ((rand.Next(100) < chanceToUseCloseCity) && (cityList[city].CloseCities.Count > 0))
                        {
                            nextCity = cityList[city].CloseCities[rand.Next(cityList[city].CloseCities.Count)];
                        }
                        else
                        {
                            nextCity = rand.Next(cityList.Count);
                        }
                    } while ((tour[nextCity].Connection2 != -1) || (nextCity == lastCity));

                    tour[lastCity].Connection2 = nextCity;
                    tour[nextCity].Connection1 = lastCity;
                    lastCity = nextCity;
                }

                // Свързване на двата последни града
                tour[lastCity].Connection2 = firstCity;
                tour[firstCity].Connection1 = lastCity;

                tour.DetermineFitness(cityList);

                Add(tour);

                if ((bestTour == null) || (tour.Fitness < bestTour.Fitness))
                {
                    BestTour = tour;
                }
            }
        }
    }
}
