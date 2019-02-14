using System;

namespace KLRT.Constants
{
    public static class KLRTConstants
    {
        public static readonly Guid FK_Provider = Guid.Parse("98ac2d64-17d4-11e9-a4d1-005056c00001");
        public static readonly Guid FK_MRTTrainType_普通車 = Guid.Parse("2d201123-0d4f-11e7-bc59-00155d63e605");
        public static readonly Guid FK_BaseAuthority_高雄市政府捷運工程局 = Guid.Parse("9da9afd6-d9e3-11e5-ad02-00155d63e605");
        public static readonly DateTime UpdateTime = DateTime.Now.Date;
        public static readonly int DataVersion = 1;
    }
}