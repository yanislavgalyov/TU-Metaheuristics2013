using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;

namespace Metaheuristics.Models
{
    /// <summary>
    /// Обощаващ клас за задачата
    /// </summary>
    class Tsp
    {
        Random rand;

        public Cities cityList;
        public Population population;

        public Tsp()
        {
        }

        public void Iterate(int maxGenerations, int groupSize, int mutation)
        {
            bool foundNewBestTour = false;
            int generation;
            for (generation = 0; generation < maxGenerations; generation++)
            {
                foundNewBestTour = makeChildren(groupSize, mutation);
            }
        }

        /// <summary>
        /// Стартиране на алгоритъма с началните стойности
        /// </summary>
        public void Begin(int populationSize, int maxGenerations, int groupSize, int mutation, int seed, int chanceToUseCloseCity, Cities cityList)
        {
            rand = new Random(seed);

            this.cityList = cityList;

            population = new Population();
            population.CreateRandomPopulation(populationSize, cityList, rand, chanceToUseCloseCity);

            bool foundNewBestTour = false;
            int generation;
            for (generation = 0; generation < maxGenerations; generation++)
            {
                foundNewBestTour = makeChildren(groupSize, mutation);
            }
        }

        /// <summary>
        /// Генериране на детски решения
        /// </summary>
        bool makeChildren(int groupSize, int mutation)
        {
            int[] tourGroup = new int[groupSize];
            int tourCount, i, topTour, childPosition, tempTour;

            // pick random tours to be in the neighborhood city group
            // we allow for the same tour to be included twice
            for (tourCount = 0; tourCount < groupSize; tourCount++)
            {
                tourGroup[tourCount] = rand.Next(population.Count);
            }

            // bubble sort on the neighborhood city group
            for (tourCount = 0; tourCount < groupSize - 1; tourCount++)
            {
                topTour = tourCount;
                for (i = topTour + 1; i < groupSize; i++)
                {
                    if (population[tourGroup[i]].Fitness < population[tourGroup[topTour]].Fitness)
                    {
                        topTour = i;
                    }
                }

                if (topTour != tourCount)
                {
                    tempTour = tourGroup[tourCount];
                    tourGroup[tourCount] = tourGroup[topTour];
                    tourGroup[topTour] = tempTour;
                }
            }

            bool foundNewBestTour = false;

            // take the best 2 tours, do crossover, and replace the worst tour with it
            childPosition = tourGroup[groupSize - 1];
            population[childPosition] = Tour.Crossover(population[tourGroup[0]], population[tourGroup[1]], cityList, rand);
            if (rand.Next(100) < mutation)
            {
                population[childPosition].Mutate(rand);
            }
            population[childPosition].DetermineFitness(cityList);

            // now see if the first new tour has the best fitness
            if (population[childPosition].Fitness < population.BestTour.Fitness)
            {
                population.BestTour = population[childPosition];
                foundNewBestTour = true;
            }

            // take the best 2 tours (opposite order), do crossover, and replace the 2nd worst tour with it
            childPosition = tourGroup[groupSize - 2];
            population[childPosition] = Tour.Crossover(population[tourGroup[1]], population[tourGroup[0]], cityList, rand);
            if (rand.Next(100) < mutation)
            {
                population[childPosition].Mutate(rand);
            }
            population[childPosition].DetermineFitness(cityList);

            // now see if the second new tour has the best fitness
            if (population[childPosition].Fitness < population.BestTour.Fitness)
            {
                population.BestTour = population[childPosition];
                foundNewBestTour = true;
            }

            return foundNewBestTour;
        }
    }
}
