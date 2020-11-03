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
        private static IServiceProvider _services;
        private static readonly ServiceCollection ServiceCollections = new ServiceCollection();

        private static void Main()
        {
            ContainerConfiguration();

            var startProgram = _services.GetService<StartProgram>();

            startProgram.Start().GetAwaiter().GetResult();
        }

        private static void ContainerConfiguration()
        {
            ServiceCollections.AddScoped<StartProgram>();
            var servicesContainer = new ServiceDependency();
            _services = servicesContainer.Register(ServiceCollections);
        }
    }

    public class StartProgram
    {
        private readonly IExcelDataExtraction _excelDataExtraction;
        private readonly IBruteForceMethod _bruteForceMethod;

        public StartProgram(IExcelDataExtraction excelDataExtraction, IBruteForceMethod bruteForceMethod)
        {
            _excelDataExtraction = excelDataExtraction;
            _bruteForceMethod = bruteForceMethod;
        }

        public Task Start()
        {
            var listaCidadePartida = _excelDataExtraction.ExtractFromExcel();

            var listaResultado = new List<Resultado>();
            foreach (var cidade in listaCidadePartida)
            {
                var resultado = _bruteForceMethod.GerarRota(cidade.NomeCidade, listaCidadePartida);
                listaResultado.Add(resultado);
            }

            listaResultado = listaResultado.OrderBy(s => s.DistanciaPercorrida).ToList();
            
            return null;
        }
    }
}