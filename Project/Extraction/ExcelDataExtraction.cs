using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ExcelDataReader;
using Project.Class;
using Project.Interfaces;
using Project.Tools;

namespace Project.Extraction
{
    public class ExcelDataExtraction : IExcelDataExtraction
    {
        private readonly string _filePath;
        private readonly List<Cidade> _listaCidadePartidas;

        public ExcelDataExtraction(string filePath)
        {
            _filePath = filePath;
            _listaCidadePartidas = new List<Cidade>();
        }

        public List<Cidade> ExtractFromExcel()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var stream = File.Open(_filePath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            while (reader.Read())
            {
                if (reader.IsDBNull(0))
                    continue;

                var coluna1 = reader.GetValue(0);
                var coluna2 = reader.GetValue(1);
                var coluna3 = reader.GetValue(2);
                var coluna4 = reader.GetValue(3);

                if (CriaCidadeSeTextoForOrigem(coluna1) || SeTextoForCabecalho(coluna1, coluna2))
                    continue;

                InserirCidadesDestino(_listaCidadePartidas.Last(), coluna1, coluna2, coluna3, coluna4);
            }

            return _listaCidadePartidas.OrderBy(s => s.NomeCidade).ToList();
        }

        private bool CriaCidadeSeTextoForOrigem(object coluna1)
        {
            var texto = coluna1.ToString();

            Debug.Assert(texto != null, nameof(texto) + " != null");
            if (!texto.Contains("ORIGEM:"))
                return false;

            var nomeCidadePartida =
                texto.Replace("ORIGEM:", "").LimparString();

            if (_listaCidadePartidas.LastOrDefault() != null)
                _listaCidadePartidas.Last().Destinos =
                    _listaCidadePartidas.Last().Destinos.OrderBy(s => s.NomeCidade).ToList();

            _listaCidadePartidas.Add(new Cidade {NomeCidade = nomeCidadePartida});
            return true;
        }

        private static bool SeTextoForCabecalho(object coluna1, object coluna2)
        {
            if (coluna1 == null || coluna2 == null)
                return true;

            return coluna1.ToString().RemoveAccents().ToUpper() == "DESTINO" ||
                   coluna2.ToString().RemoveAccents().ToUpper() == "DISTANCIA";
        }

        private void InserirCidadesDestino(Cidade cidade, params object[] listaObjetos)
        {
            for (var i = 0; i < 2; i++)
            {
                if (listaObjetos[0] == null || listaObjetos[3] == null
                                            || cidade.NomeCidade == listaObjetos[i * 2].ToString().LimparString())
                    continue;

                cidade.Destinos.Add(new CidadeDestino()
                {
                    NomeCidade = listaObjetos[i * 2].ToString().LimparString(),
                    DistanciaDestino = Convert.ToInt32(listaObjetos[i * 2 + 1])
                });
            }
        }
    }
}