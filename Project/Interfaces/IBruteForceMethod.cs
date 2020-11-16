using System.Collections.Generic;
using Project.Class;

namespace Project.Interfaces
{
    public interface IBruteForceMethod
    {
        Resultado GerarRota(string cidadeDestino, List<Cidade> listaCidadePartida);
    }
}