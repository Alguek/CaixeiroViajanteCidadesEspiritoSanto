using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq.Extensions;
using Project.Class;
using Project.Interfaces;

namespace Project.CaixeiroViajante
{
    public class CaixeiroViajanteBruteForce : ICaixeiroViajanteBruteForce
    {
        private List<CidadePartida> _listaCidadePartida;
        private readonly List<CidadeDestino> _listaCidadeVisitadas;

        public CaixeiroViajanteBruteForce()
        {
            _listaCidadeVisitadas = new List<CidadeDestino>();
        }

        public Resultado GerarRota(string cidadeDestino, List<CidadePartida> listaCidadePartida)
        {
            _listaCidadePartida = listaCidadePartida;

            AdicionaPassagemListaVisitados(cidadeDestino);

            _listaCidadePartida.Aggregate(cidadeDestino, (current, cidade) => EscolherMenorDistancia(current));

            CompletarRota(_listaCidadeVisitadas.Last().NomeCidadeOrigem, _listaCidadeVisitadas.First().NomeCidadeOrigem);

            var valorTotalDistancia = _listaCidadeVisitadas.Sum(s => s.DistanciaDestino);
            
            Console.WriteLine($"Distancia partindo de {_listaCidadeVisitadas.First().NomeCidadeOrigem} : {valorTotalDistancia}");
            return new Resultado { Cidade = _listaCidadeVisitadas.First().NomeCidadeOrigem, DistanciaPercorrida = valorTotalDistancia};
        }

        private string EscolherMenorDistancia(string stringCidadePartida)
        {
            var cidadePartida = ObterCidadePartida(stringCidadePartida);

            var destinos = cidadePartida.Destinos.Where(s =>
                !_listaCidadeVisitadas.Select(z => z.NomeCidadeOrigem).Contains(s.NomeCidadeOrigem)).ToList();

            var cidadeDestinoMenorDistancia = destinos.Where(s => s.NomeCidadeOrigem != stringCidadePartida)
                .MinBy(s => s.DistanciaDestino).FirstOrDefault();

            if (cidadeDestinoMenorDistancia == null)
                return null;

            _listaCidadeVisitadas.Add(cidadeDestinoMenorDistancia);

            return cidadeDestinoMenorDistancia.NomeCidadeOrigem;
        }

        private CidadePartida ObterCidadePartida(string stringCidadePartida)
        {
            var cidadePartida = _listaCidadePartida.FirstOrDefault(s => s.NomeCidade == stringCidadePartida);
            return cidadePartida;
        }

        private void AdicionaPassagemListaVisitados(string cidadeDestino)
        {
            var cidadePartida = ObterCidadePartida(cidadeDestino).Destinos
                .FirstOrDefault(x => x.NomeCidadeOrigem == cidadeDestino);
            
            _listaCidadeVisitadas.Add(cidadePartida);
        }

        private void CompletarRota(string nomeUltimaCidadeDaLista, string cidadeDePartidaInicial)
        {
            var ultimaCidadeRota = ObterCidadePartida(nomeUltimaCidadeDaLista);
            var cidadeInicial = ultimaCidadeRota.Destinos.FirstOrDefault(x => x.NomeCidadeOrigem == cidadeDePartidaInicial);
            _listaCidadeVisitadas.Add(cidadeInicial);
        }
        
    }
}