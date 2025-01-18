using System;

namespace Support.Framework.Extentions
{
    public static class ObjectExtention
    {
        public static void ReleaseObject(this object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
                GC.Collect(); // Release the handle to the table
            }
            catch { }
        }
    }
}
