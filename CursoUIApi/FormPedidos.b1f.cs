using CursoUIApi.DI;
using CursoUIAPI.DI.Model;
using CursoUIAPI.DI.Services;
using SAPbouiCOM.Framework;
using Support.Framework;
using Support.Framework.Extentions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CursoUIApi
{
    [FormAttribute("CursoFormPed", "FormPedidos.b1f")]
    class FormPedidos : UserFormBase
    {
        public static readonly string FORM_NAME = "CursoFormPed";
        public FormPedidos()
        {
        }

        #region variaveis

        private SAPbouiCOM.Form _form;
        private int _currentDocEntry;
        private FaturamentoService _faturamentoService;
        private List<DocumentoLinha> _docLinhas;

        private SAPbouiCOM.StaticText lblData;
        private SAPbouiCOM.EditText txtDataInicio;
        private SAPbouiCOM.EditText txtDataFim;
        private SAPbouiCOM.StaticText lblCardCode;
        private SAPbouiCOM.LinkedButton lkdCardCode;
        private SAPbouiCOM.EditText txtCardCode;
        private SAPbouiCOM.EditText txtCardName;
        private SAPbouiCOM.Button btnListar;

        private SAPbouiCOM.Grid GridPedidos;
        private SAPbouiCOM.Grid GridItens;

        private SAPbouiCOM.Button btnGerarNF;
        private SAPbouiCOM.Button btnCancelar;
        
        #endregion

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.lblData = ((SAPbouiCOM.StaticText)(this.GetItem("lblData").Specific));
            this.txtDataInicio = ((SAPbouiCOM.EditText)(this.GetItem("txtDtIni").Specific));
            this.txtDataFim = ((SAPbouiCOM.EditText)(this.GetItem("txtDtFim").Specific));
            this.lblCardCode = ((SAPbouiCOM.StaticText)(this.GetItem("lblCli").Specific));
            this.txtCardCode = ((SAPbouiCOM.EditText)(this.GetItem("txt002").Specific));
            this.txtCardCode.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.txtCardCode_ChooseFromListAfter);
            this.txtCardName = ((SAPbouiCOM.EditText)(this.GetItem("txt002_2").Specific));
            this.lkdCardCode = ((SAPbouiCOM.LinkedButton)(this.GetItem("lkdCli").Specific));
            this.GridPedidos = ((SAPbouiCOM.Grid)(this.GetItem("grd001").Specific));
            this.GridPedidos.ClickAfter += new SAPbouiCOM._IGridEvents_ClickAfterEventHandler(this.GridPedidos_ClickAfter);
            this.btnListar = ((SAPbouiCOM.Button)(this.GetItem("btn001").Specific));
            this.btnListar.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.btnListar_ClickAfter);
            this.GridItens = ((SAPbouiCOM.Grid)(this.GetItem("grid002").Specific));
            this.GridItens.ValidateAfter += new SAPbouiCOM._IGridEvents_ValidateAfterEventHandler(this.GridItens_ValidateAfter);
            this.GridItens.ValidateBefore += new SAPbouiCOM._IGridEvents_ValidateBeforeEventHandler(this.GridItens_ValidateBefore);
            this.btnGerarNF = ((SAPbouiCOM.Button)(this.GetItem("btn002").Specific));
            this.btnGerarNF.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.btnGerarNF_ClickAfter);
            this.btnCancelar = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ResizeAfter += new ResizeAfterHandler(this.Form_ResizeAfter);

        }

        private void OnCustomInitialize()
        {
            try
            {
                _form = (SAPbouiCOM.Form)UIAPIRawForm;

                _form.EnableMenu(MenuKeys.Data.Find, false);
                _form.EnableMenu(MenuKeys.Data.Add, false);
                _form.EnableMenu(MenuKeys.Data.CopyTable, true);

                _form.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;

                ChangeChooseCliente();
                SetDates();

                _docLinhas = new List<DocumentoLinha>();
                _faturamentoService = new FaturamentoService(UICompany.Instance);

                btnListar_ClickAfter(null, null);
                ListItens();

            }
            catch (Exception ex)
            {
                Util.ShowMessageBox(ex.Message);
            }
        }
       
        private void SetDates()
        {
            txtDataInicio.Value = DateTime.Now.Date.ToStringUI();
            txtDataFim.Value = DateTime.Now.Date.ToStringUI();
        }

        private void ChangeChooseCliente()
        {
            var cfl = _form.ChooseFromLists.Item("CFL_002");
            var conds = cfl.GetConditions();

            var cond = conds.Add();
            cond.Alias = "CardType";
            cond.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            cond.CondVal = "C";

            cfl.SetConditions(conds);
        }

        private void txtCardCode_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                SAPbouiCOM.ISBOChooseFromListEventArg chflarg = (SAPbouiCOM.ISBOChooseFromListEventArg)pVal;
                var uidChoose = chflarg.ChooseFromListUID;

                SAPbouiCOM.DataTable dt = chflarg.SelectedObjects;

                if (dt == null)
                    return;

                var udCode = _form.DataSources.UserDataSources.Item("UD_002");
                var valCode = dt.GetValue("CardCode", 0).ToString();
                udCode.ValueEx = valCode;

                txtCardName.Value = dt.GetValue("CardName", 0).ToString();
            }
            catch (Exception ex)
            {
                Util.ShowMessageError(ex.Message);
            }
        }

        private void btnListar_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                _form.Freeze(true);
                ListPedidos();

                LimparDadosEmMemoria();

            }
            catch (Exception ex)
            {
                Util.ShowMessageError(ex.Message);
            }
            finally
            {
                _form.Freeze(false);
            }
        }
        private void LimparDadosEmMemoria()
        {
            _currentDocEntry = 0;
            _docLinhas.Clear();
        }

        private void ListPedidos()
        {
            var data1 = txtDataInicio.Value;
            var data2 = txtDataFim.Value;
            var cardCode = txtCardCode.Value;

            var query = MyQuery.GetQueryFaturamento(data1, data2, cardCode);
            GridPedidos.DataTable.ExecuteQuery(query);

            GridPedidos.DisableAllColumns();
            GridPedidos.EnableSortingAllColumns();

            var colSel = (SAPbouiCOM.EditTextColumn)GridPedidos.Columns.Item("Selecionar");
            colSel.Type = SAPbouiCOM.BoGridColumnType.gct_CheckBox;
            colSel.Editable = true;
            colSel.AffectsFormMode = false;

            var colCliente = (SAPbouiCOM.EditTextColumn)GridPedidos.Columns.Item("Cliente");
            colCliente.LinkedObjectType = "2";

            var colDocSap = (SAPbouiCOM.EditTextColumn)GridPedidos.Columns.Item("DocSap");
            colDocSap.LinkedObjectType = "17";

            //GridPedidos.Columns.Item("ValorTotal").RightJustified = true;

            GridPedidos.Columns.Item("GroupNum").Visible = false;
            GridPedidos.Columns.Item("BPLId").Visible = false;

            GridPedidos.ShowColumnSum("ValorTotal");

            GridPedidos.AutoResizeColumnsIfNeeded();
        }

        private void Form_ResizeAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                GridPedidos.AutoResizeColumnsIfNeeded();
            }
            catch (Exception ex)
            {
                Util.ShowMessageError(ex.Message);
            }
        }
        private void ListItens()
        {
            var query = MyQuery.GetQueryFaturamentoItens(_currentDocEntry);
            GridItens.DataTable.ExecuteQuery(query);
            GridItens.DisableAllColumns();

            var colItem = (SAPbouiCOM.EditTextColumn)GridItens.Columns.Item("Item");
            colItem.LinkedObjectType = "4";

            var colFaturado = (SAPbouiCOM.EditTextColumn)GridItens.Columns.Item("Faturado");
            colFaturado.Type = SAPbouiCOM.BoGridColumnType.gct_CheckBox;
            colFaturado.Description = "Indicador para informar se a linha já está fechada no PV";

            GridItens.Columns.Item("Valor Total").RightJustified = true;
            GridItens.Columns.Item("Qtd Pedido").RightJustified = true;
            GridItens.Columns.Item("Quantidade").RightJustified = true;
            GridItens.Columns.Item("Quantidade").TitleObject.Caption = "Qtd em Aberto";
            GridItens.Columns.Item("Preço").RightJustified = true;

            GridItens.Columns.Item("Qtd a Faturar").RightJustified = true;
            GridItens.Columns.Item("Qtd a Faturar").Editable = true;

            GridItens.Columns.Item("Doc.Sap").Visible = false;
            GridItens.Columns.Item("LineNum").Visible = false;

            GridItens.Columns.Item("Obs").Editable = true;

            GridItens.RowHeaders.TitleObject.Caption = "#";
            for (int i = 0; i < GridItens.Rows.Count; i++)
            {
                GridItens.RowHeaders.SetText(i, GridItens.DataTable.GetValue("LineNum", i).ToString());
            }

            GridItens.AutoResizeColumnsIfNeeded();
        }

        private void GridPedidos_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                _form.Freeze(true);
                GridPedidos.SelectRow(pVal.Row);
                var rowIndex = GridPedidos.GetDataTableRowIndex(pVal.Row);

                _currentDocEntry = Convert.ToInt32(GridPedidos.DataTable.GetValue("DocSap", rowIndex).ToString());

                ListItens();

                // se existir algum dado do pedido em memoria, atualiza a grid
                foreach (var item in _docLinhas.Where(v => v.DocEntry == _currentDocEntry))
                {
                    var qtdFaturarGrid = Convert.ToDecimal(GridItens.DataTable.GetValue("Qtd a Faturar", item.LinhaGrid));

                    if (qtdFaturarGrid != item.Quantity)
                    {
                        var qtdValue = item.Quantity.ToString(CultureInfo.InvariantCulture);
                        GridItens.DataTable.SetValue("Qtd a Faturar", item.LinhaGrid, qtdValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ShowMessageError(ex.Message);
            }
            finally
            {
                _form.Freeze(false);
            }

        }
     
        private void GridItens_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (pVal.ColUID == "Qtd a Faturar")
                {
                    var dt = GridItens.DataTable;
                    var qtdEmAberto = Convert.ToDecimal(dt.GetValue("Quantidade", pVal.Row));
                    var qtdAFaturar = Convert.ToDecimal(dt.GetValue("Qtd a Faturar", pVal.Row));

                    if (qtdAFaturar > qtdEmAberto)
                    {
                        Util.ShowMessageError("A quantidade a faturar não pode ser maior que a quantidade em aberto da linha.");
                        BubbleEvent = false;
                    }
                    else if (qtdAFaturar <= 0)
                    {
                        Util.ShowMessageError("A quantidade a faturar não pode ser menor que ou igual a zero.");
                        BubbleEvent = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ShowMessageError(ex.Message);
            }
        }

        private void btnGerarNF_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                List<Documento> documentos = GetDocumentosSelecionados();
                if (!documentos.Any())
                    throw new Exception("Nenhum pedido selecionado. Verifique.");

                if (!Util.ShowMessageConfirm("Confirma o faturamento do pedido selecionado?"))
                    return;

                var oProgBar = Application.SBO_Application.StatusBar.CreateProgressBar("Faturando documentos....", documentos.Count, false);

                int docEntryPv = 0;

                try
                {
                    foreach (var documento in documentos)
                    {
                        docEntryPv = documento.DocEntryPV;
                        Documento documentoFaturado;

                        documentoFaturado = _faturamentoService.FaturaDocumento(documento);

                        if (documentoFaturado.DocEntryNF > 0)
                        {
                            oProgBar.Text = $"Pedido de Nº [{docEntryPv}] faturado. NF de Nº [{documentoFaturado.DocEntryNF}] Gerada.";
                            oProgBar.Value += 1;
                        }
                        else
                        {
                            throw new Exception($"Erro ao faturar Pedido de Nº [{docEntryPv}]. Erro: [{documentoFaturado.Erro}]");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Util.ShowMessageError($"Erro ao faturar documento de Nº [{docEntryPv}] - [${ex.Message}]");
                    oProgBar.Value += 1;
                }
                finally
                {
                    oProgBar.Stop();
                }

                if (documentos.TrueForAll(v => v.DocEntryNF > 0))
                {
                    Util.ShowMessageBox("Pedido(s) faturado(s) com sucesso!");
                    btnListar_ClickAfter(null, null);
                }
                else
                {
                    Util.ShowMessageBox("Houve erro(s) no processo. Verifique o log de mensagens.");
                }
            }
            catch (Exception ex)
            {
                Util.ShowMessageError(ex.Message);
            }
        }

        private List<Documento> GetDocumentosSelecionados()
        {
            var list = new List<Documento>();

            for (int i = 0; i < GridPedidos.Rows.Count; i++)
            {
                var rowId = GridPedidos.GetDataTableRowIndex(i);
                var selecionado = GridPedidos.DataTable.GetValue("Selecionar", rowId).ToString() == "Y";
                if (!selecionado)
                    continue;

                var doc = new Documento
                {
                    CardCode = GridPedidos.DataTable.GetValue("Cliente", rowId).ToString(),
                    DocEntryPV = GridPedidos.DataTable.GetValue("DocSap", rowId).ToString().ToIntParse(),
                    BPLId = GridPedidos.DataTable.GetValue("BPLId", rowId).ToString().ToIntParse(),
                    // var valorTitulo = Convert.ToDecimal(dataTable.GetValue("Valor do Título", i));
                };

                var linhasDoPedido = _docLinhas.Where(v => v.DocEntry == doc.DocEntryPV);
                doc.Linhas.AddRange(linhasDoPedido);

                list.Add(doc);
            }

            return list;
        }

        private void GridItens_ValidateAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                _form.Freeze(true);

                if (pVal.ColUID == "Qtd a Faturar")
                {
                    var dt = GridItens.DataTable;
                    var lineNum = dt.GetValue("LineNum", pVal.Row).ToString().ToIntParse();
                    var itemCode = dt.GetValue("Item", pVal.Row).ToString();
                    var quantidade = Convert.ToDecimal(dt.GetValue("Quantidade", pVal.Row));
                    var quantidadeFaturar = Convert.ToDecimal(dt.GetValue("Qtd a Faturar", pVal.Row));
                    var preco = Convert.ToDecimal(dt.GetValue("Preço", pVal.Row));
                    var deposito = dt.GetValue("Deposito", pVal.Row).ToString();

                    _docLinhas.RemoveAll(v => v.DocEntry == _currentDocEntry && v.LineNum == lineNum && v.ItemCode == itemCode);

                    _docLinhas.Add(new DocumentoLinha
                    {
                        DocEntry = _currentDocEntry,
                        LinhaGrid = pVal.Row,
                        LineNum = lineNum,
                        ItemCode = itemCode,
                        Quantity = quantidadeFaturar,
                        UnitPrice = preco,
                        WhsCode = deposito
                    });
                }
            }
            catch (Exception ex)
            {
                Util.ShowMessageError(ex.Message);
            }
            finally
            {
                _form.Freeze(false);
            }
        }
    }
}
