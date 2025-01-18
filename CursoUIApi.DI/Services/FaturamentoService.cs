using CursoUIAPI.DI.Model;
using Support.Framework.Extentions;
using System;

namespace CursoUIAPI.DI.Services
{
    public class FaturamentoService
    {
        private SAPbobsCOM.Company _conSap;

        public FaturamentoService(SAPbobsCOM.Company conSap)
        {
            _conSap = conSap;
        }

        public Documento FaturaDocumento(Documento doc)
        {
            if (!_conSap.InTransaction)
                _conSap.StartTransaction();

            var nf = _conSap.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices) as SAPbobsCOM.Documents;
            nf.DocDate = DateTime.Today;
            nf.TaxDate = DateTime.Today;

            if (doc.BPLId > 0)
                nf.BPL_IDAssignedToInvoice = doc.BPLId;

            var indexItem = 0;
            foreach (var linha in doc.Linhas)
            {
                if (indexItem > 0)
                    nf.Lines.Add();

                nf.Lines.BaseType = (int)SAPbobsCOM.BoObjectTypes.oOrders;
                nf.Lines.BaseEntry = doc.DocEntryPV;
                nf.Lines.BaseLine = linha.LineNum;

                nf.Lines.ItemCode = linha.ItemCode;
                nf.Lines.UnitPrice = (double)linha.UnitPrice;
                nf.Lines.DiscountPercent = (double)linha.DiscPrcnt;
                nf.Lines.Quantity = (double)linha.Quantity;
                nf.Lines.WarehouseCode = linha.WhsCode;

                indexItem++;
            }

            if (nf.Add() != 0)
            {
                var msg = $"Erro ao gerar faturamento do pedido [{doc.DocEntryPV}]: [{_conSap.GetLastErrorDescription()}].";
                doc.Erro = msg;

                if (_conSap.InTransaction)
                    _conSap.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                return doc;
            }
            else
            {
                int docEntry = 0;
                int.TryParse(_conSap.GetNewObjectKey(), out docEntry);
                doc.DocEntryNF = docEntry;

                if (_conSap.InTransaction)
                    _conSap.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
            }

            nf.ReleaseObject();

            return doc;
        }
    }
}
