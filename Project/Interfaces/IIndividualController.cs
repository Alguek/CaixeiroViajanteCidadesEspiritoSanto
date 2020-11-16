using System.Collections.Generic;
using Project.Class;

namespace Project.Interfaces
{
    public interface IIndividualController
    {
        Individual CreateGnome(int possibilitiesLength);
        
        int GetIndividualAccuracy(Individual individual, List<Cidade> listaCidade);

        Individual Mate(Individual parent1, Individual parent2);

        Individual Mutate(Individual individual);

        List<string> GetCitiesFromListOfInt(Individual individual, List<Cidade> listaCidade);
    }
}