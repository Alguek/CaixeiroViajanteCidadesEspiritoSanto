using System;
using System.Collections.Generic;
using System.Linq;
using Project.Class;
using Project.Interfaces;

namespace Project.SolutionMethods.GeneticAlgorithm
{
    public class IndividualController : IIndividualController
    {
        private int GetRandomOfPossibiliesList(List<int> possibilitiesList)
        {
            var rand = new Random((int) DateTime.Now.Ticks & 0x0000FFFF).Next(0, possibilitiesList.Count);

            return possibilitiesList[rand];
        }

        public Individual CreateGnome(int possibilitiesLength)
        {
            var possibilities = Enumerable.Range(0, possibilitiesLength).ToList();
            var gnome = new List<int>();

            for (var i = 0; i < possibilitiesLength; i++)
            {
                var mutatedGene = GetRandomOfPossibiliesList(possibilities);
                gnome.Add(mutatedGene);
                possibilities.Remove(mutatedGene);
            }

            return new Individual {Destinos = gnome};
        }

        public int GetIndividualAccuracy(Individual individual, List<Cidade> listaCidade)
        {
            var accuracy = 0;
            for (var i = 0; i < individual.Destinos.Count - 1; i++)
            {
                try
                {
                    var partida = individual.Destinos[i];
                    var destino = individual.Destinos[i + 1];

                    var deCidade = listaCidade[partida];
                    var paraCidade = deCidade.Destinos[destino];

                    accuracy += paraCidade.DistanciaDestino;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return accuracy;
        }

        public List<string> GetCitiesFromListOfInt(Individual individual, List<Cidade> listaCidade)
        {
            var listaDestinos = new List<string>();
            foreach (var cidadeIndex in individual.Destinos)
            {
                var cidade = listaCidade[cidadeIndex].NomeCidade;
                listaDestinos.Add(cidade);
            }

            return listaDestinos;
        }

        public Individual Mate(Individual parent1, Individual parent2)
        {
            var childP1 = new List<int>();
            var childP2 = new List<int>();
            var child = new List<int>();

            var listaDestinos = new List<int>();

            var possibilities = parent1.Destinos.Count;

            var geneA = new Random((int) DateTime.Now.Ticks & 0x0000FFFF).Next(0, possibilities);
            var geneB = new Random((int) DateTime.Now.Ticks & 0x0000FFFF).Next(0, possibilities);

            var startGene = Math.Min(geneA, geneB);
            var endGene = Math.Max(geneA, geneB);

            for (int i = startGene; i < endGene; i++)
                childP1.Add(parent1.Destinos[i]);

            childP2 = parent2.Destinos.Where(s => !childP1.Contains(s)).ToList();

            child = childP1.Concat(childP2).ToList();

            return new Individual {Destinos = child};
        }

        public Individual Mutate(Individual individual)
        {
            var destinos = individual.Destinos;
            var possibilitiesLength = destinos.Count;

            var mutates = new Random((int) DateTime.Now.Ticks & 0x0000FFFF).Next(0, possibilitiesLength / 2);

            for (var i = 0; i < mutates; i++)
            {
                var index1 = new Random((int) DateTime.Now.Ticks & 0x0000FFFF).Next(0, possibilitiesLength);
                var index2 = new Random((int) DateTime.Now.Ticks & 0x0000FFFF).Next(0, possibilitiesLength);
                Swap(destinos, index1, index2);
            }

            return individual;
        }

        private static void Swap<T>(IList<T> list, int index1, int index2)
        {
            var temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }
    }
}