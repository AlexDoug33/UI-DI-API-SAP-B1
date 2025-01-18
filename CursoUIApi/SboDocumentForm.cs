using SAPbouiCOM.Framework;
using Support.Framework;
using Support.Framework.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursoUIApi
{
    class SboDocumentForm
    {
        public static readonly int FormTypePedidoVenda = 139;
        public static readonly string InvoiceTypePedidoVenda = "17"; // Cotacao = 23 | NF de Saida = 13 | Entrega = 15;

        public struct UniqueIDs
        {
            public static readonly int topDistance = 15;
            public static readonly int defaultWith = 225;

            public static readonly string btnSYSCancelar = "2";
            public static readonly string lblSYSTotalAPagar = "30";
            public static readonly string btnTeste = "btncurui01";

            public static readonly string testeRightClick = "rcbtncurui1";
        }

        public static void AddButtonToFormOrder(SAPbouiCOM.Form form)
        {
            SAPbouiCOM.Item oNewItem;
            SAPbouiCOM.Item oItem;

            oItem = form.Items.Item(UniqueIDs.btnSYSCancelar);

            // Perc. Lucro Bruto
            oNewItem = form.Items.Add(UniqueIDs.btnTeste, SAPbouiCOM.BoFormItemTypes.it_BUTTON);
            oNewItem.Width = oItem.Width;
            oNewItem.Top = oItem.Top;
            oNewItem.Left = oItem.Left + oItem.Width + UniqueIDs.topDistance;
            oNewItem.Height = oItem.Height;
            oNewItem.FromPane = 0;
            ((SAPbouiCOM.Button)(oNewItem.Specific)).Caption = "Botão Teste";
        }
        
        internal static void BtnBotaoTesteClick(SAPbouiCOM.Form form)
        {
            var dbORDR = form.DataSources.DBDataSources.Item("ORDR");
            var docEntry = dbORDR.GetValue("DocEntry", dbORDR.Offset).ToIntParse();
            var cardCode = dbORDR.GetValue("CardCode", dbORDR.Offset);

            Util.ShowMessageBox($"Você clicou no Botão de Teste: DocEntry: {docEntry} | CardCode: {cardCode}");

            Application.SBO_Application.ActivateMenuItem(MenuKeys.Data.Refresh);
        }
    }
}
