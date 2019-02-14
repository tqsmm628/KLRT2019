using System;
using System.IO;
using KLRT.Services;

namespace KLRT
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
//            Dump(SqlSL.GetCountSql());
//            Dump(SqlSL.GetInsertSql());
//            Dump(SqlSL.GetDeleteSql());
            Dump(SqlSL.GenerateBaseServiceDetailInsertSql());
//            Dump(SqlSL.GenerateBaseServiceDetailDeleteSql());
        }

        

        private static void Dump(params string[] msg) => File.WriteAllText(
            "Output/dump.sql",
            string.Join(Environment.NewLine, msg));
    }
}
