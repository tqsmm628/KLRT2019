using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KLRT.Constants;
using KLRT.Enums;
using OfficeOpenXml;

namespace KLRT.Services
{
    public static class VersionSL
    {
        public static string GenerateSql() => SqlSL.Insert(KLRTTable.MRTVersion, new 
        {
            PK_MRTVersion = Guid.NewGuid(),
            KLRTConstants.FK_Provider,
            Network = KLRTConstants.UpdateTime,
            SrcNetwork = KLRTConstants.UpdateTime,
            NetworkVersion = KLRTConstants.DataVersion,
            Line = KLRTConstants.UpdateTime, 
            SrcLine = KLRTConstants.UpdateTime, 
            LineVersion = KLRTConstants.DataVersion, 
            LineStation = KLRTConstants.UpdateTime, 
            SrcLineStation = KLRTConstants.UpdateTime, 
            LineStationVersion = KLRTConstants.DataVersion, 
            LineTransfer = KLRTConstants.UpdateTime, 
            SrcLineTransfer = KLRTConstants.UpdateTime, 
            LineTransferVersion = KLRTConstants.DataVersion, 
            Route = KLRTConstants.UpdateTime, 
            SrcRoute = KLRTConstants.UpdateTime, 
            RouteVersion = KLRTConstants.DataVersion, 
            RouteStation = KLRTConstants.UpdateTime, 
            SrcRouteStation = KLRTConstants.UpdateTime, 
            RouteStationVersion = KLRTConstants.DataVersion, 
            Station = KLRTConstants.UpdateTime, 
            SrcStation = KLRTConstants.UpdateTime, 
            StationVersion = KLRTConstants.DataVersion, 
            StationExit = KLRTConstants.UpdateTime, 
            SrcStationExit = KLRTConstants.UpdateTime, 
            StationExitVersion = KLRTConstants.DataVersion, 
            StationFacility = KLRTConstants.UpdateTime, 
            SrcStationFacility = KLRTConstants.UpdateTime, 
            StationFacilityVersion = KLRTConstants.DataVersion, 
            FirstLastTimetable = KLRTConstants.UpdateTime, 
            SrcFirstLastTimetable = KLRTConstants.UpdateTime, 
            FirstLastTimetableVersion = KLRTConstants.DataVersion, 
            Frequency = KLRTConstants.UpdateTime, 
            SrcFrequency = KLRTConstants.UpdateTime, 
            FrequencyVersion = KLRTConstants.DataVersion, 
            S2STravelTime = KLRTConstants.UpdateTime, 
            SrcS2STravelTime = KLRTConstants.UpdateTime, 
            S2STravelTimeVersion = KLRTConstants.DataVersion, 
            ODFare = KLRTConstants.UpdateTime, 
            SrcODFare = KLRTConstants.UpdateTime, 
            ODFareVersion = KLRTConstants.DataVersion, 
            StationTimeTable = KLRTConstants.UpdateTime, 
            SrcStationTimeTable = KLRTConstants.UpdateTime, 
            StationTimeTableVersion = KLRTConstants.DataVersion, 
            StationTransfer = KLRTConstants.UpdateTime, 
            SrcStationTransfer = KLRTConstants.UpdateTime, 
            StationTransferVersion = KLRTConstants.DataVersion, 
        });
    }
}