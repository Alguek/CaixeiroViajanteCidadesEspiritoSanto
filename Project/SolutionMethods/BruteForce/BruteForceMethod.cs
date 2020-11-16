using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq.Extensions;
using Project.Class;
using Project.Interfaces;

namespace Project.SolutionMethods.BruteForce
{
    public class BruteForceMethod : IBruteForceMethod
    {
        private List<Cidade> _listaCidadePartida;
        private List<CidadeDestino> _listaCidadeVisitadas;
        
        public Resultado GerarRota(string cidadeDestino, List<Cidade> listaCidadePartida)
        {
            _listaCidadeVisitadas = new List<CidadeDestino>();
            _listaCidadePartida = listaCidadePartida;

            AdicionaPassagemListaVisitados(cidadeDestino);

            _listaCidadePartida.Aggregate(cidadeDestino, (current, cidade) => EscolherMenorDistancia(current));

            CompletarRota(_listaCidadeVisitadas.Last().NomeCidade, _listaCidadeVisitadas.First().NomeCidade);

            var valorTotalDistancia = _listaCidadeVisitadas.Sum(s => s.DistanciaDestino);
            
            Console.WriteLine($"Distancia partindo de {_listaCidadeVisitadas.First().NomeCidade} : {valorTotalDistancia}");
            return new Resultado { Cidade = _listaCidadeVisitadas.First().NomeCidade, DistanciaPercorrida = valorTotalDistancia};
        }

        private string EscolherMenorDistancia(string stringCidadePartida)
        {
            var cidadePartida = ObterCidadePartida(stringCidadePartida);

            var destinos = cidadePartida.Destinos.Where(s =>
                !_listaCidadeVisitadas.Select(z => z.NomeCidade).Contains(s.NomeCidade))
                .ToList();

            var cidadeDestinoMenorDistancia = destinos.Where(s => s.NomeCidade != stringCidadePartida)
                .MinBy(s => s.DistanciaDestino).FirstOrDefault();

            if (cidadeDestinoMenorDistancia == null)
                return null;

            _listaCidadeVisitadas.Add(cidadeDestinoMenorDistancia);

            return cidadeDestinoMenorDistancia.NomeCidade;
        }

        private Cidade ObterCidadePartida(string stringCidadePartida)
        {
            var cidadePartida = _listaCidadePartida.FirstOrDefault(s => s.NomeCidade == stringCidadePartida);
            return cidadePartida;
        }

        private void AdicionaPassagemListaVisitados(string cidadeDestino)
        {
            var cidadePartida = ObterCidadePartida(cidadeDestino).Destinos
                .FirstOrDefault(x => x.NomeCidade == cidadeDestino);
            
            _listaCidadeVisitadas.Add(cidadePartida);
        }

        private void CompletarRota(string nomeUltimaCidadeDaLista, string cidadeDePartidaInicial)
        {
            var ultimaCidadeRota = ObterCidadePartida(nomeUltimaCidadeDaLista);
            var cidadeInicial = ultimaCidadeRota.Destinos.FirstOrDefault(x => x.NomeCidade == cidadeDePartidaInicial);
            _listaCidadeVisitadas.Add(cidadeInicial);
        }
        
    }
}