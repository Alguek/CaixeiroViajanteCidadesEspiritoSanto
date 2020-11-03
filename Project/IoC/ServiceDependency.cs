using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Extraction;
using Project.Interfaces;
using Project.SolutionMethods.BruteForce;

namespace Project.IoC
{
    public class ServiceDependency
    {
        private readonly IConfigurationRoot _configuration;

        public ServiceDependency()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Environment.CurrentDirectory).Parent?.Parent?.FullName)
                .AddJsonFile("appsettings.json", true, true);

            _configuration = configurationBuilder.Build();
        }

        public ServiceProvider Register(ServiceCollection services)
        {
            var filePath = GetDirectoryPath();
            
            services.AddScoped<IExcelDataExtraction>(provider => new
                ExcelDataExtraction(filePath));

            services.AddTransient<IBruteForceMethod, BruteForceMethod>();

            return services.BuildServiceProvider();
        }

        private string GetDirectoryPath()
        {
            var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent?.Parent?.FullName;
            var fileName = _configuration.GetSection("fileName").Value;

            var filePath = $"{projectDirectory}\\{fileName}";

            return filePath;
        }
    }
}