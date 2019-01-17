using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KLRT.Constants;
using KLRT.Enums;

namespace KLRT.Services
{
    public static class SqlSL
    {
        public static string Insert(string table, object pairs)
            => Insert(table, pairs.GetType().GetProperties().ToDictionary(o => o.Name, o => o.GetValue(pairs)));
        
        public static string Insert(KLRTTable table, object pairs)
            => Insert($"{table}", pairs.GetType().GetProperties().ToDictionary(o => o.Name, o => o.GetValue(pairs)));
        
        private static string Insert(string table, Dictionary<string, object> pairs)
            => $"insert into {table} " +
               $"({string.Join(", ", pairs.Keys)}) " +
               "values " +
               $"({string.Join(", ", pairs.Values.Select(AsString))});";

        private static string Delete(string table, string where = null)
            => $"delete from {table}{(where == null ? string.Empty : $" where {where}")};";
        
        private static string Select(string table, string where = null)
            => $"select * from {table}{(where == null ? string.Empty : $" where {where}")};";
        
        private static string Count(string table, string where = null)
            => $"select count(*) count from {table}{(where == null ? string.Empty : $" where {where}")}";

        private static string AsString(object o)
        {
            switch (o)
            {
                case null: return "null";
                
                case int _:
                case float _:
                case double _: return $"{o}";
                
                case bool b: return $"b'{(b ? 1 : 0)}'";
                
                case DateTime dt: return $"'{dt:yyyy-MM-dd hh:mm:ss}'";
            }
            return $"'{o}'";
        }
        
        public static string GetInsertSql()
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

        public static string GetDeleteSql() => string.Join(Environment.NewLine,
            Enum.GetValues(typeof(KLRTTable)).Cast<KLRTTable>()
                .Select(v => Delete($"{v}", $"FK_Provider = '{KLRTConstants.FK_Provider}'")));
        
        public static string GetCountSql() => string.Concat(
            @"select sum(count) from (", Environment.NewLine, 
            string.Join($" union all {Environment.NewLine}",
                Enum.GetValues(typeof(KLRTTable)).Cast<KLRTTable>()
                    .Select(v => Count($"{v}", $"FK_Provider = '{KLRTConstants.FK_Provider}'"))),
            ") x");
        

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

        public static string GenerateBaseServiceDetailInsertSql() => string.Join(Environment.NewLine, 
            ServiceDetailValues.Select(a => Insert("BaseServiceDetail", new
            {
                PK_BaseServiceDetail = Guid.NewGuid(),
                a.ID,
                a.FK_BaseService,
                FK_BaseAuthority = KLRTConstants.FK_BaseAuthority_高雄市政府捷運工程局,
                Parameter = "KLRT",
                ParamDescription = "高雄輕軌",
                a.NameZh_tw,
                DataUpdateInterval = -1,
                PublishTime = new DateTime(2019, 1, 1),
                UpdateTime = new DateTime(2019, 1, 1)
            })));

        private static (string ID, Guid FK_BaseService, string NameZh_tw)[] ServiceDetailValues { get; } = new []
        {
            ("Metro_02049", MetroBaseServiceKey.Line, "高雄輕軌路線資料服務"),
            ("Metro_02050", MetroBaseServiceKey.Station, "高雄輕軌車站資料服務"),
            ("Metro_02051", MetroBaseServiceKey.FirstLastTimetable, "高雄輕軌首末班車時刻表資料服務"),
            ("Metro_02052", MetroBaseServiceKey.ODFare, "高雄輕軌起迄站間票價資料服務"),
            ("Metro_02053", MetroBaseServiceKey.StationExit, "高雄輕軌車站出入口資料服務"),
            ("Metro_02054", MetroBaseServiceKey.StationOfLine, "高雄輕軌路線車站資料服務"),
        };
    }
}