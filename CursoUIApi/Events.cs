using SAPbouiCOM.Framework;
using Support.Framework;
using System;

namespace CursoUIApi
{
    public class Events
    {
        public Events()
        {
            Application.SBO_Application.AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);
            Application.SBO_Application.ItemEvent += SBO_Application_ItemEvent;
            Application.SBO_Application.RightClickEvent += SBO_Application_RightClickEvent;
            Application.SBO_Application.FormDataEvent += SBO_Application_FormDataEvent;
        }

        private void SBO_Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                var activeForm = Application.SBO_Application.Forms.ActiveForm;

                if (eventInfo.BeforeAction)
                {
                    if (activeForm.Type == SboDocumentForm.FormTypePedidoVenda)
                    {
                        AddRightClickEventToFormOrder(activeForm, eventInfo.ItemUID, out BubbleEvent);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ShowMessageError(ex.Message);
            }
        }
        private void AddRightClickEventToFormOrder(SAPbouiCOM.Form form, string itemUID, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (form.Type == SboDocumentForm.FormTypePedidoVenda)
            {
                SAPbouiCOM.MenuItem oMenuItem = null;
                SAPbouiCOM.Menus oMenus = null;

                try
                {
                    SAPbouiCOM.MenuCreationParams oCreationPackage = null;
                    oCreationPackage = ((SAPbouiCOM.MenuCreationParams)
                        (Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));

                    oMenuItem = Application.SBO_Application.Menus.Item("1280"); // Data'
                    oMenus = oMenuItem.SubMenus;

                    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    oCreationPackage.UniqueID = SboDocumentForm.UniqueIDs.testeRightClick;
                    oCreationPackage.String = "Emitir Mensagem de Teste";
                    oCreationPackage.Position = 1;
                    oCreationPackage.Enabled = true;
                    oMenus.AddEx(oCreationPackage);
                }
                catch (Exception)
                {
                    //Util.ShowMessageError(ex.Message);
                }
            }
            else
            {
                try
                {
                    Application.SBO_Application.Menus.RemoveEx(SboDocumentForm.UniqueIDs.testeRightClick);
                }
                catch (Exception)
                {
                    //Util.ShowMessageError(ex.Message);
                }
            }
        }

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (pVal.BeforeAction)
                    return;

                SboFormDocument_ItemEvent_BotaoTeste(pVal, out BubbleEvent);
            }
            catch (Exception ex)
            {
                Util.ShowMessageError(ex.Message);
            }
        }

        private static void SboFormDocument_ItemEvent_BotaoTeste(SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.FormType != SboDocumentForm.FormTypePedidoVenda)
                return;

            if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_UNLOAD)
                return;

            if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_ACTIVATE)
                return;

            SAPbouiCOM.Form form = null;
            try
            {
                form = Application.SBO_Application.Forms.GetFormByTypeAndCount(pVal.FormType, pVal.FormTypeCount);
            }
            catch { }

            if (form == null)
                return;

            if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED)
            {
                if (pVal.ItemUID == SboDocumentForm.UniqueIDs.btnTeste)
                    SboDocumentForm.BtnBotaoTesteClick(form);
            }
        }

        private void SBO_Application_FormDataEvent(ref SAPbouiCOM.BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (BusinessObjectInfo.BeforeAction)
                {
                    switch (BusinessObjectInfo.EventType)
                    {
                        case SAPbouiCOM.BoEventTypes.et_RIGHT_CLICK:
                            break;
                        case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD:
                            if (BusinessObjectInfo.Type == SboDocumentForm.InvoiceTypePedidoVenda)
                            {
                                var form = Application.SBO_Application.Forms.ActiveForm;
                                form.Title = form.Title + " com botão adicionado";
                                SboDocumentForm.AddButtonToFormOrder(form);

                            }
                            break;
                    }
                }
                else
                {
                    switch (BusinessObjectInfo.EventType)
                    {
                        case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST:
                            break;
                        case SAPbouiCOM.BoEventTypes.et_CLICK:
                            break;
                        case SAPbouiCOM.BoEventTypes.et_COMBO_SELECT:
                            break;
                        case SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD:
                            break;
                        case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD:
                            break;
                        case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                //Util.ShowMessageError(ex.Message);
            }
        }

        static void SBO_Application_AppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case SAPbouiCOM.BoAppEventTypes.aet_ShutDown:
                    //Exit Add-On
                    System.Windows.Forms.Application.Exit();
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_FontChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition:
                    break;
                default:
                    break;
            }
        }
    }
}
