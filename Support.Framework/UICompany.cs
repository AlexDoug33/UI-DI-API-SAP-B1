using SAPbouiCOM.Framework;
using Support.Framework.Extentions;
using System;

namespace Support.Framework
{
    public class UICompany
    {
        private static SAPbobsCOM.Company _conSap;

        // Padrao Singleton
        public static SAPbobsCOM.Company Instance
        {
            get
            {
                if (_conSap == null)
                    Connect();

                if (_conSap != null && !_conSap.Connected)
                {
                    _conSap.ReleaseObject();
                    Connect();
                }

                return _conSap;
            }
        }

        private static void Connect()
        {
            var conTmp = Application.SBO_Application.Company.GetDICompany();
            var count = 1;
            while (conTmp == null && count < 5)
            {
                conTmp = Application.SBO_Application.Company.GetDICompany();
                count++;
            }

            if (conTmp == null)
                throw new Exception("Erro em UICompany método Connect()");

            _conSap = (SAPbobsCOM.Company)conTmp;

            if (_conSap == null)
                throw new Exception("Erro _conSap em UICompany método Connect()");
        }
    }
}
