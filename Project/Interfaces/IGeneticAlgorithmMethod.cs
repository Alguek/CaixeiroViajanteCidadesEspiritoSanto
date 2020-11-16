
using System.Collections.Generic;
using Project.Class;

namespace Project.Interfaces
{
    public interface IGeneticAlgorithmMethod
    {
        void Handler(List<Cidade> listaCidade);
        List<Individual> CriarPopulacaoInicial(int possibilitiesLength);
    }
}