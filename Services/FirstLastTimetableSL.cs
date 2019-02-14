using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KLRT.Constants;
using KLRT.Enums;
using OfficeOpenXml;

namespace KLRT.Services
{
    public class FirstLastTimetableSL
    {
        public static IEnumerable<FirstLastTimetableDTO> Parse(string fileName, Dictionary<string, Guid> LineMap, Dictionary<string, Guid> StationMap)
        {
            using (var p = new ExcelPackage(new FileInfo(fileName)))
            {
                foreach (var row in Enumerable.Range(3, 26))
                {
                    var cells = p.Workbook.Worksheets[1].Cells;
                    yield return new FirstLastTimetableDTO
                    {
                        LineID = (string)cells[row, 1].Value,
                        StationID = (string)cells[row, 2].Value,
                        TripHeadSign = (string)cells[row, 3].Value,
                        DestinationStationID = (string)cells[row, 4].Value,
                        Zh_tw = (string)cells[row, 5].Value,
                        En = (string)cells[row, 6].Value,
                        FirstTrainTime = $"{(DateTime)cells[row, 7].Value:HH:mm}",
                        LastTrainTime = $"{(DateTime)cells[row, 8].Value:HH:mm}",
                        FK_MRTLine = LineMap[(string)cells[row, 1].Value],
                        FK_MRTStationBase = StationMap[(string)cells[row, 2].Value],
                        FK_MRTStationBase_End = StationMap[(string)cells[row, 4].Value]
                    };
                }
            }
        }
        
        public static string GenerateSql(IEnumerable<FirstLastTimetableDTO> data) => string.Join(Environment.NewLine, 
            data.Select(a => SqlSL.Insert(KLRTTable.MRTFirstLastTimetable, new 
            {
                a.PK_MRTFirstLastTimetable,
                KLRTConstants.FK_Provider,
                a.FK_MRTLine,
                a.FK_MRTStationBase,
                a.FK_MRTStationBase_End,
                FK_MRTTrainType = KLRTConstants.FK_MRTTrainType_普通車,
                a.TripHeadSign,
                a.FirstTrainTime,
                a.LastTrainTime,
                Monday = true,
                Tuesday = true,
                Wednesday = true,
                Thursday = true,
                Friday = true,
                Saturday = true,
                Sunday = true,
                NationalHolidays = true
            })));
    }
    
    public class FirstLastTimetableDTO
    {
        public Guid PK_MRTFirstLastTimetable = Guid.NewGuid();
        public string LineID, StationID, TripHeadSign, DestinationStationID, Zh_tw, En, FirstTrainTime, LastTrainTime;
        public Guid FK_MRTLine, FK_MRTStationBase, FK_MRTStationBase_End;
    }
}