using System.Collections.Generic;
using Project.Class;

namespace Project.Interfaces
{
    public interface IExcelDataExtraction
    {
        List<CidadePartida> ExtractFromExcel();
    }
}