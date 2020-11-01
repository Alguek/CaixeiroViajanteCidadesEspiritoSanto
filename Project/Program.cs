using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Project.Class;
using Project.Interfaces;
using Project.IoC;

namespace Project
{
    internal static class Program
    {
        private static void Main() =>
            new StartProgram().Start().GetAwaiter().GetResult();
    }

    public class StartProgram
    {
        private IServiceProvider _services;
        private readonly ServiceCollection _serviceCollections = new ServiceCollection();
        public Task Start()
        {
            ContainerConfiguration();

            var excelDataExtraction = _services.GetService<IExcelDataExtraction>();
            var listaCidadePartida = excelDataExtraction.ExtractFromExcel();

            var listaResultado = new List<Resultado>();
            foreach (var cidade in listaCidadePartida)
            {
                var caixeiroViajanteBruteForce = _services.GetService<ICaixeiroViajanteBruteForce>();
                var resultado =  caixeiroViajanteBruteForce.GerarRota(cidade.NomeCidade, listaCidadePartida);
                listaResultado.Add(resultado);
            }

            listaResultado = listaResultado.OrderBy(s => s.DistanciaPercorrida).ToList();
            
            return null;
        }

        private void ContainerConfiguration()
        {
            var servicesContainer = new ServiceDependency();
            _services = servicesContainer.Register(_serviceCollections);
        }
    }
}