using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KLRT.Constants;
using KLRT.Enums;
using OfficeOpenXml;

namespace KLRT.Services
{
    public static class StationExitSL
    {
        public static IEnumerable<StationExitDTO> Parse(string fileName, Dictionary<string, Guid> StationMap)
        {
            using (var p = new ExcelPackage(new FileInfo(fileName)))
            {
                foreach (var row in Enumerable.Range(3, 14))
                {
                    var cells = p.Workbook.Worksheets[1].Cells;
                    yield return new StationExitDTO
                    {
                        StationID = (string)cells[row, 1].Value,
                        ExitID = (string)cells[row, 2].Value,
                        NameZh_tw = (string)cells[row, 3].Value,
                        NameEn_us = (string)cells[row, 4].Value,
                        LocationDescription = (string)cells[row, 5].Value,
                        Elevator = (int)(double)cells[row, 6].Value,
                        
                        FK_MRTStationBase = StationMap[(string)cells[row, 1].Value]
                    };
                }
            }
        }

        public static string GenerateSql(IEnumerable<StationExitDTO> data) => string.Join(Environment.NewLine, 
            data.Select(a => SqlSL.Insert(KLRTTable.MRTStationExit, new 
            {
                a.PK_MRTStationExit,
                KLRTConstants.FK_Provider,
                a.FK_MRTStationBase,
                ExitID = a.ExitID ?? string.Empty,
                a.NameZh_tw,
                a.NameEn_us,
                a.LocationDescription,
                a.Elevator
            })));
    }

    public class StationExitDTO
    {
        public Guid PK_MRTStationExit = Guid.NewGuid();
        public string StationID, ExitID, NameZh_tw, NameEn_us, LocationDescription;
        public int Elevator;

        public Guid FK_MRTStationBase;
    }
}