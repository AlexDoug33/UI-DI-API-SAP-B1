using SAPbouiCOM.Framework;
using Support.Framework;
using Support.Framework.Extentions;
using System;

namespace CursoUIApi
{
    [FormAttribute("CursoFormGer", "Form1.b1f")]
    class Form1 : UserFormBase
    {
        public static readonly string FORM_NAME = "CursoFormGer";

        public Form1()
        {
        }

        #region variaveis

        private SAPbouiCOM.Form _form;

        private SAPbouiCOM.StaticText lblCode;
        private SAPbouiCOM.EditText txtCode;
        private SAPbouiCOM.StaticText lblName;
        private SAPbouiCOM.EditText txtName;
        private SAPbouiCOM.StaticText lblDataNascimento;
        private SAPbouiCOM.EditText txtDataNascimento;
        private SAPbouiCOM.StaticText lblSalario;
        private SAPbouiCOM.EditText txtSalario;
        private SAPbouiCOM.StaticText lblCPF;
        private SAPbouiCOM.EditText txtCPF;
        private SAPbouiCOM.StaticText lblFuncao;
        private SAPbouiCOM.ComboBox cboFuncao;
        
        private SAPbouiCOM.Matrix MatrixFuncionarios;
        private SAPbouiCOM.Button btnAdicionarLinha;
        private SAPbouiCOM.Button btnRemoverLinha;

        private SAPbouiCOM.Button btnOk;
        private SAPbouiCOM.Button btnCancelar;

        #endregion

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.lblCode = ((SAPbouiCOM.StaticText)(this.GetItem("lblCode").Specific));
            this.txtCode = ((SAPbouiCOM.EditText)(this.GetItem("txtCode").Specific));
            this.lblName = ((SAPbouiCOM.StaticText)(this.GetItem("lblName").Specific));
            this.txtName = ((SAPbouiCOM.EditText)(this.GetItem("txtName").Specific));
            this.lblDataNascimento = ((SAPbouiCOM.StaticText)(this.GetItem("lbl001").Specific));
            this.txtDataNascimento = ((SAPbouiCOM.EditText)(this.GetItem("txt001").Specific));
            this.lblSalario = ((SAPbouiCOM.StaticText)(this.GetItem("lbl002").Specific));
            this.txtSalario = ((SAPbouiCOM.EditText)(this.GetItem("txt002").Specific));
            this.lblCPF = ((SAPbouiCOM.StaticText)(this.GetItem("lbl003").Specific));
            this.txtCPF = ((SAPbouiCOM.EditText)(this.GetItem("txt003").Specific));
            this.lblFuncao = ((SAPbouiCOM.StaticText)(this.GetItem("lbl004").Specific));
            this.cboFuncao = ((SAPbouiCOM.ComboBox)(this.GetItem("cbo004").Specific));
            this.btnOk = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.btnCancelar = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.MatrixFuncionarios = ((SAPbouiCOM.Matrix)(this.GetItem("mat001").Specific));
            this.btnAdicionarLinha = ((SAPbouiCOM.Button)(this.GetItem("btnAdd").Specific));
            this.btnAdicionarLinha.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnAdicionarLinha_ClickBefore);
            this.btnRemoverLinha = ((SAPbouiCOM.Button)(this.GetItem("btnRem").Specific));
            this.btnRemoverLinha.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnRemoverLinha_ClickBefore);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private void OnCustomInitialize()
        {
            try
            {
                _form = (SAPbouiCOM.Form)UIAPIRawForm;
            }
            catch (Exception ex)
            {
                Util.ShowMessageBox(ex.Message);
            }
        }

        private void btnAdicionarLinha_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                MatrixFuncionarios.AddNewRow(_form);
            }
            catch (Exception ex)
            {
                Util.ShowMessageError(ex.Message);
            }
        }
        private void btnRemoverLinha_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                var index = MatrixFuncionarios.GetSelectedRowIndex();
                MatrixFuncionarios.RemoveSelectedRow();

                if (_form.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
                    _form.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;

                Util.ShowMessageStatus($"Linha {index} removida.");
            }
            catch (Exception ex)
            {
                Util.ShowMessageError(ex.Message);
            }
        }
    }
}