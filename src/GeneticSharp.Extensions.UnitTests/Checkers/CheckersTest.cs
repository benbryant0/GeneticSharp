using NUnit.Framework;

namespace GeneticSharp.Extensions.UnitTests.Checkers
{
    [TestFixture]
    [Category("Extensions")]
    public class CheckersTest
    {
        [SetUp]
        public void InitializeTest()
        {
            RandomizationProvider.Current = new BasicRandomization();
        }

        [Test()]
        [MaxTime(100000)]
        public void Evolve_ManyGenerations_Fast()
        {
            int movesAhead = 10;
            int boardSize = 10;
            var selection = new EliteSelection();
            var crossover = new OrderedCrossover();
            var mutation = new TworsMutation();
            var chromosome = new CheckersChromosome(movesAhead, boardSize);
            var fitness = new CheckersFitness(new CheckersBoard(boardSize));

            var population = new Population(40, 40, chromosome);

            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
            ga.GenerationRan += delegate
            {
                if (ga.Population.GenerationsNumber % 100 == 0)
                {
                    fitness.Update(ga.Population.BestChromosome as CheckersChromosome);
                }
            };

            ga.Start();
            var firstFitness = ((CheckersChromosome)ga.Population.BestChromosome).Fitness;

            ga.Termination = new GenerationNumberTermination(2001);

            ga.Start();
     
            var lastFitness = ((CheckersChromosome)ga.Population.BestChromosome).Fitness;

            Assert.LessOrEqual(firstFitness, lastFitness);
        }
    }
}