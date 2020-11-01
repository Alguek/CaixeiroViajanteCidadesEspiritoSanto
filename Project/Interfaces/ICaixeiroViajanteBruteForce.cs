using System.Collections.Generic;
using Project.Class;

namespace Project.Interfaces
{
    public interface ICaixeiroViajanteBruteForce
    {
        Resultado GerarRota(string cidadeDestino, List<CidadePartida> listaCidadePartida);
    }
}