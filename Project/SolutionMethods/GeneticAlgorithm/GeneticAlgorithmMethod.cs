using System;
using System.Collections.Generic;
using System.Linq;
using Project.Class;
using Project.Interfaces;

namespace Project.SolutionMethods.GeneticAlgorithm
{
    public class GeneticAlgorithmMethod : IGeneticAlgorithmMethod
    {
        private readonly IIndividualController _individualController;
        public int Generation = 0;

        public GeneticAlgorithmMethod(IIndividualController individualController)
        {
            _individualController = individualController;
        }

        public void Handler(List<Cidade> listaCidade)
        {
            var populacao = CriarPopulacaoInicial(listaCidade[0].Destinos.Count + 1);

            foreach (var individual in populacao)
                individual.Accuracy = _individualController.GetIndividualAccuracy(individual, listaCidade);

            var generation = 0;
            var endGeneration = 10000;

            var best = new Individual();
            while (generation < endGeneration)
            {
                populacao = populacao.OrderBy(s => s.Accuracy).ToList();
            
                const int reducaoPopulacao = (10 * GenericAlgorithmConstants.PopulationSize) / 100;
                var novaGeracao = populacao.GetRange(0, reducaoPopulacao);
            
                const int recriarPopulacao = (90 * GenericAlgorithmConstants.PopulationSize) / 100;
                for (var i = 0; i < recriarPopulacao; i++)
                {
                    var r1 = new Random((int)DateTime.Now.Ticks & 0x0000FFFF).Next(0, 50);
                    var r2 = new Random((int)DateTime.Now.Ticks & 0x0000FFFF).Next(0, 50);

                    var parent1 = populacao[r1];
                    var parent2 = populacao[r2];

                    var offspring = _individualController.Mate(parent1, parent2);
                    var mutatedChance = new Random().NextDouble() * new Random((int)DateTime.Now.Ticks & 0x0000FFFF).Next(100);
                    if (mutatedChance <= GenericAlgorithmConstants.MutatedChance)
                        offspring = _individualController.Mutate(offspring);
                    
                    offspring.Accuracy = _individualController.GetIndividualAccuracy(offspring, listaCidade);

                    novaGeracao.Add(offspring);
                }
                populacao = novaGeracao.OrderBy(s => s.Accuracy).ToList();

                best = populacao[0];
                Console.WriteLine(best.Accuracy);
                generation++;
            }

            _individualController.GetCitiesFromListOfInt(best, listaCidade);
        }

        public List<Individual> CriarPopulacaoInicial(int possibilitiesLength)
        {
            var populacao = new List<Individual>();

            for (var i = 0; i < GenericAlgorithmConstants.PopulationSize; i++)
                populacao.Add(_individualController.CreateGnome(possibilitiesLength));

            return populacao;
        }
    }
}