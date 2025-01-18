namespace Support.Framework.Extentions
{
    public static class StringExtention
    {
        /// <summary>
        /// Demonstrar em um EditTex um valor decimal amigavel
        /// </summary>
        /// <param name="value"></param>
        /// <param name="showCurrency"></param>
        /// <returns></returns>
        public static string ToUIString(this decimal value, bool showCurrency = true)
        {
            if (showCurrency)
                return "R$ " + value.ToString("#,##0.00");
            else
                return value.ToString("#,##0.00");
        }

        /// <summary>
        /// int.TryParse() without exception
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToIntParse(this string value)
        {
            int valueInt = 0;
            int.TryParse(value, out valueInt);

            return valueInt;
        }
    }
}
