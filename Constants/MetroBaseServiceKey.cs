using System;
using System.Collections.Generic;
using System.Linq;

namespace KLRT.Constants
{
    public static class MetroBaseServiceKey
    {
        public static readonly Guid Line = Guid.Parse("036cfcb7-2cf2-11e7-b355-00155d63e605");
        public static readonly Guid Station = Guid.Parse("036cfdcc-2cf2-11e7-b355-00155d63e605");
        public static readonly Guid FirstLastTimetable = Guid.Parse("ab1d4bf3-2cf2-11e7-b355-00155d63e605");
        public static readonly Guid ODFare = Guid.Parse("b3b1a53e-2cf2-11e7-b355-00155d63e605");
        public static readonly Guid StationExit = Guid.Parse("9f81ebd3-2cf2-11e7-b355-00155d63e605");
        public static readonly Guid StationOfLine = Guid.Parse("036cfe64-2cf2-11e7-b355-00155d63e605");
        
        public static string GenerateKeySql() 
            => GenerateKeySql(new [] { "Line", "Station", "FirstLastTimetable", "ODFare", "StationExit", "StationOfLine" });
        
        private static string GenerateKeySql(IEnumerable<string> urlPart) => string.Join($" union all {Environment.NewLine}", 
            urlPart.Select(u => $@"(select concat('public static readonly Guid {u} = Guid.Parse(""', PK_BaseService, '"");')" +
                                "from BaseService " +
                                $"where URL like '%/v2/Rail/Metro/{u}/%' " +
                                "order by Version desc limit 1)"));
    }
}