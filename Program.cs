using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KLRT.Enums;
using KLRT.Services;

namespace KLRT
{
    class Program
    {
        static void Main(string[] args)
        {
            Dump(GetInsertSql());
        }

        private static string GetInsertSql()
        {
            string f(string name) => $"./Assets/Source/{name}.xlsx";
            var TicketTypeMap = GetTicketTypeMap();
            var FareClassMap = GetFareClassMap();
            
            var Lines = LineSL.Parse(f("Line")).ToList();
            var LineMap = Lines.ToDictionary(o => o.LineID, o => o.PK_MRTLine);
            var Stations = StationSL.Parse(f("Station")).ToList();
            var StationMap = Stations.ToDictionary(o => o.StationID, o => o.PK_MRTStationBase);
            var FirstLastTimetables = FirstLastTimetableSL.Parse(f("FirstLastTimetable"), LineMap, StationMap).ToList();
            var ODFares = ODFareSL.Parse(f("ODFare"), StationMap, TicketTypeMap, FareClassMap).ToList();
            var StationExits = StationExitSL.Parse(f("StationExitList"), StationMap).ToList();
            var StationOfLines = StationOfLineSL.Parse(f("StationOfLine"), LineMap, StationMap).ToList();

            
            var LineSql = LineSL.GenerateSql(Lines);
            var StationSql = StationSL.GenerateSql(Stations);
            var FirstLastTimetableSql = FirstLastTimetableSL.GenerateSql(FirstLastTimetables);
            var ODFareSql = ODFareSL.GenerateSql(ODFares);
            var StationExitSql = StationExitSL.GenerateSql(StationExits);
            var StationOfLineSql = StationOfLineSL.GenerateSql(StationOfLines);

            return string.Join(Environment.NewLine + Environment.NewLine, new string[]
            {
                LineSql,
                StationSql,
                FirstLastTimetableSql,
                ODFareSql,
                StationExitSql,
                StationOfLineSql
            });
        }

        private static string GetDeleteSql() => string.Join(Environment.NewLine,
            Enum.GetValues(typeof(KLRTTable)).Cast<KLRTTable>()
                .Select(v => SqlSL.Delete(v, $"FK_Provider = '{KLRTConstants.FK_Provider}'")));
        

        private static Dictionary<int, Guid> GetTicketTypeMap()
            => new Dictionary<int, Guid>
            {
                {1, Guid.Parse("735b2f2f-28ff-11e7-b355-00155d63e605")},
                {2, Guid.Parse("735b3232-28ff-11e7-b355-00155d63e605")},
                {3, Guid.Parse("735b3332-28ff-11e7-b355-00155d63e605")},
                {4, Guid.Parse("735b3399-28ff-11e7-b355-00155d63e605")},
                {5, Guid.Parse("735b3400-28ff-11e7-b355-00155d63e605")},
                {6, Guid.Parse("735b345d-28ff-11e7-b355-00155d63e605")},
            };
        
        private static Dictionary<int, Guid> GetFareClassMap()
            => new Dictionary<int, Guid>
            {
                {1, Guid.Parse("e7f7ecf5-28ff-11e7-b355-00155d63e605")},
                {2, Guid.Parse("e7f7f076-28ff-11e7-b355-00155d63e605")},
                {3, Guid.Parse("e7f7f1a8-28ff-11e7-b355-00155d63e605")},
                {4, Guid.Parse("e7f7f25a-28ff-11e7-b355-00155d63e605")},
                {5, Guid.Parse("e7f7f2ef-28ff-11e7-b355-00155d63e605")},
                {6, Guid.Parse("e7f7f389-28ff-11e7-b355-00155d63e605")},
                {7, Guid.Parse("e7f7f43d-28ff-11e7-b355-00155d63e605")},
                {8, Guid.Parse("e7f7f4d9-28ff-11e7-b355-00155d63e605")},
            };

        private static void Dump(params string[] msg) => File.WriteAllText(
            "Output/dump.sql",
            string.Join(Environment.NewLine, msg));
    }
    
    public static class KLRTConstants
    {
        public static readonly Guid FK_Provider = Guid.Parse("98ac2d64-17d4-11e9-a4d1-005056c00001");
        public static readonly Guid FK_TrainType_普通車 = Guid.Parse("2d201123-0d4f-11e7-bc59-00155d63e605");
    }
}
