using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KLRT.Enums;

namespace KLRT.Services
{
    public static class SqlSL
    {
        public static string Insert(KLRTTable table, object pairs)
            => Insert(table, pairs.GetType().GetProperties().ToDictionary(o => o.Name, o => o.GetValue(pairs)));
        
        private static string Insert(KLRTTable table, Dictionary<string, object> pairs)
            => $"insert into {table} " +
               $"({string.Join(", ", pairs.Keys)}) " +
               "values " +
               $"({string.Join(", ", pairs.Values.Select(AsString))});";

        public static string Delete(KLRTTable table, string where = null)
            => $"delete from {table}{(where == null ? string.Empty : $" where {where}")};";

        private static string AsString(object o)
        {
            switch (o)
            {
                case null: return "null";
                
                case int _:
                case float _:
                case double _: return $"{o}";
                
                case bool b: return $"b'{(b ? 1 : 0)}'";
            }
            return $"'{o}'";
        }
    }
}