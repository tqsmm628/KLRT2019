using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KLRT.Enums;
using OfficeOpenXml;

namespace KLRT.Services
{
    public static class LineSL
    {
        public static IEnumerable<LineDTO> Parse(string fileName)
        {
            using (var p = new ExcelPackage(new FileInfo(fileName)))
            {
                foreach (var row in Enumerable.Range(3, 1))
                {
                    var cells = p.Workbook.Worksheets[1].Cells;
                    yield return new LineDTO
                    {
                        LineID = (string)cells[row, 2].Value,
                        NameZh_tw = (string)cells[row, 3].Value
                    };
                }
            }
        }

        public static string GenerateSql(IEnumerable<LineDTO> data) => string.Join(Environment.NewLine, 
            data.Select(a => SqlSL.Insert(KLRTTable.MRTLine, new 
            {
                a.PK_MRTLine,
                KLRTConstants.FK_Provider,
                FK_MRTNetwork = "",
                a.LineID,
                a.NameZh_tw,
                IsBranch = false
            })));
    }

    public class LineDTO
    {
        public Guid PK_MRTLine = Guid.NewGuid();
        public string LineID, NameZh_tw;
    }
}