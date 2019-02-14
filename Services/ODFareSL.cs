using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KLRT.Constants;
using KLRT.Enums;
using Microsoft.VisualBasic;
using OfficeOpenXml;

namespace KLRT.Services
{
    public static class ODFareSL
    {
        public static IEnumerable<ODFareDTO> Parse(string fileName, Dictionary<string, Guid> StationMap, Dictionary<int, Guid> TicketTypeMap, Dictionary<int, Guid> FareClassMap)
        {
            using (var p = new ExcelPackage(new FileInfo(fileName)))
            {
                foreach (var row in Enumerable.Range(3, 1176))
                {
                    var cells = p.Workbook.Worksheets[1].Cells;
                    yield return new ODFareDTO
                    {
                        OriginStationID = (string)cells[row, 1].Value,
                        Zh_tw = (string)cells[row, 2].Value,
                        DestinationStationID = (string)cells[row, 3].Value,
                        Zh_tw2 = (string)cells[row, 4].Value,
                        Ticketype = (int)(double)cells[row, 5].Value,
                        FareClass = (int)(double)cells[row, 6].Value,
                        Price = (double)cells[row, 7].Value,
                        
                        FK_MRTStationBase_Origin = StationMap[(string)cells[row, 1].Value],
                        FK_MRTStationBase_Destination = StationMap[(string)cells[row, 3].Value],
                        FK_MRTTicketType = TicketTypeMap[(int)(double)cells[row, 5].Value],
                        FK_MRTFareClass = FareClassMap[(int)(double)cells[row, 6].Value],
                        FK_MRTTrainType = KLRTConstants.FK_MRTTrainType_普通車
                    };
                }
            }
        }

        public static string GenerateSql(IEnumerable<ODFareDTO> data) => string.Join(Environment.NewLine, 
            data.Select(a => SqlSL.Insert(KLRTTable.MRTODFare, new 
            {
                a.PK_MRTODFare,
                KLRTConstants.FK_Provider,
                a.FK_MRTStationBase_Origin,
                a.FK_MRTStationBase_Destination,
                a.FK_MRTTrainType,
                a.FK_MRTTicketType,
                a.FK_MRTFareClass,
                a.Price
            })));
    }

    public class ODFareDTO
    {
        public Guid PK_MRTODFare = Guid.NewGuid();
        public string OriginStationID, Zh_tw, DestinationStationID, Zh_tw2;
        public int Ticketype, FareClass;
        public double Price;
        
        public Guid FK_MRTStationBase_Origin, FK_MRTStationBase_Destination, FK_MRTTicketType, FK_MRTFareClass;
        public Guid? FK_MRTTrainType;
    }
}