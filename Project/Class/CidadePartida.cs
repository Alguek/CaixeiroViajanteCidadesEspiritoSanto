using System.Collections.Generic;

namespace Project.Class
{
    public class CidadePartida
    {
        public CidadePartida()
        {
            Destinos = new List<CidadeDestino>();
        }

        public string NomeCidade { get; set; }
        public List<CidadeDestino> Destinos { get; set; }
    }
}