namespace Support.Framework
{
    public static class MenuKeys
    {
        // https://archive.sap.com/discussions/thread/299254

        public struct Data
        {
            public static readonly string Find = "1281";
            public static readonly string Add = "1282";
            public static readonly string First = "1290";
            public static readonly string Next = "1288";
            public static readonly string Previous = "1289";
            public static readonly string Last = "1291";
            public static readonly string Refresh = "1304";

            public static readonly string Cancel = "1284";
            public static readonly string Restore = "1285";
            public static readonly string FilterTable = "4870";
            public static readonly string CopyTable = "784";
            public static readonly string Log = "4876";
            public static readonly string Remove = "1283";
            public static readonly string Duplicate = "1287";
            public static readonly string Close = "1286";

            public static readonly string AddLine = "1292";
            public static readonly string RemoveLine = "1293";
            public static readonly string CloneLine = "1299";
            public static readonly string ReopenLine = "1312";
            public static readonly string DuplicateLine = "1294";
            public static readonly string FormSetting = "5890";

            public static readonly string Cut = "771";
            public static readonly string Copy = "772";
            public static readonly string Paste = "773";
        }

        public struct CharIds
        {
            public static readonly int Tab = 9;
            public static readonly int Up = 38;
            public static readonly int Down = 40;
        }

        public static string GetColorMenu(eCompanyColor color)
        {
            switch (color)
            {
                case eCompanyColor.Combined:
                    return "5632";
                case eCompanyColor.Classic:
                    return "5633";
                case eCompanyColor.Gray:
                    return "5634";
                case eCompanyColor.Violet:
                    return "5635";
                case eCompanyColor.Blue:
                    return "5636";
                case eCompanyColor.Green:
                    return "5637";
                case eCompanyColor.Yellow:
                    return "5638";
                case eCompanyColor.Orange:
                    return "5639";
                case eCompanyColor.Red:
                    return "5640";
                case eCompanyColor.Brown:
                    return "5641";
                default:
                    return "5633";
            }
        }
    }

    public enum eCompanyColor
    {
        Combined = 0,
        Classic = 1,
        Gray = 2,
        Violet = 3,
        Blue = 4,
        Green = 5,
        Yellow = 6,
        Orange = 7,
        Red = 8,
        Brown = 9
    }
}
