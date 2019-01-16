using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KLRT.Enums;
using OfficeOpenXml;

namespace KLRT.Services
{
    public static class StationOfLineSL
    {
        public static IEnumerable<StationOfLineDTO> Parse(string fileName, Dictionary<string, Guid> LineMap, Dictionary<string, Guid> StationMap)
        {
            using (var p = new ExcelPackage(new FileInfo(fileName)))
            {
                foreach (var row in Enumerable.Range(3, 14))
                {
                    var cells = p.Workbook.Worksheets[1].Cells;
                    yield return new StationOfLineDTO
                    {
                        LineNo = (string)cells[row, 1].Value,
                        LineID = (string)cells[row, 2].Value,
                        Sequence = (int)(double)cells[row, 3].Value,
                        StationID = (string)cells[row, 4].Value,
                        Zh_tw = (string)cells[row, 5].Value,
                        En = (string)cells[row, 6].Value,
                        Distance = (double?)cells[row, 7].Value,
                        
                        FK_MRTLine = LineMap[(string)cells[row, 2].Value],
                        FK_MRTStationBase = StationMap[(string)cells[row, 4].Value]
                    };
                }
            }
        }
        
        public static string GenerateSql(IEnumerable<StationOfLineDTO> data) => string.Join(Environment.NewLine, 
            data.Select(a => SqlSL.Insert(KLRTTable.MRTLine_MRTStationBase, new 
            {
                a.PK_MRTLine_MRTStationBase,
                KLRTConstants.FK_Provider,
                a.FK_MRTLine,
                a.FK_MRTStationBase,
                a.Distance,
                a.Sequence
            })));
    }

    public class StationOfLineDTO
    {
        public Guid PK_MRTLine_MRTStationBase = Guid.NewGuid();
        public string LineNo, LineID, StationID, Zh_tw, En;
        public int Sequence;
        public double? Distance;
        
        public Guid FK_MRTLine, FK_MRTStationBase;
    }
}