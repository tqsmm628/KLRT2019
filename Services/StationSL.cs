using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KLRT.Enums;
using OfficeOpenXml;

namespace KLRT.Services
{
    public static class StationSL
    {
        public static IEnumerable<StationDTO> Parse(string fileName)
        {
            using (var p = new ExcelPackage(new FileInfo(fileName)))
            {
                foreach (var row in Enumerable.Range(3, 14))
                {
                    var cells = p.Workbook.Worksheets[1].Cells;

                    yield return new StationDTO
                    {
                        StationID = (string)cells[row, 1].Value,
                        NameZh_tw = (string)cells[row, 2].Value,
                        NameEn_us = (string)cells[row, 3].Value,
                        Latitude = (double)cells[row, 4].Value,
                        Longitude = (double)cells[row, 5].Value
                    };
                }
            }
        }

        public static string GenerateSql(IEnumerable<StationDTO> data) => string.Join(Environment.NewLine, 
            data.Select(a => SqlSL.Insert(KLRTTable.MRTStationBase, new 
            {
                a.PK_MRTStationBase,
                KLRTConstants.FK_Provider,
                a.StationID,
                a.NameZh_tw,
                a.NameEn_us,
                a.Latitude,
                a.Longitude
            })));
    }

    public class StationDTO
    {
        public Guid PK_MRTStationBase = Guid.NewGuid();
        public string StationID, NameZh_tw, NameEn_us;
        public double Latitude, Longitude;
    }
}