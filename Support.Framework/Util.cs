using SAPbouiCOM.Framework;

namespace Support.Framework
{
    public class Util
    {
        /// <summary>
        /// SetStatusBarMessage como Erro
        /// </summary>
        /// <param name="message"></param>
        public static void ShowMessageError(string message)
        {
            Application.SBO_Application.StatusBar.SetText(message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
        }

        public static void ShowMessageStatus(string message, SAPbouiCOM.BoMessageTime time = SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType type = SAPbouiCOM.BoStatusBarMessageType.smt_Success)
        {
            Application.SBO_Application.StatusBar.SetText(message, time, type);
        }

        public static void ShowMessageBox(string message)
        {
            Application.SBO_Application.MessageBox(message);
        }

        public static bool ShowMessageConfirm(string message)
        {
            var iReturnValue = Application.SBO_Application.MessageBox(message, 1, "Sim", "Não");
            // 1 - Continue , 2 - Abort

            return iReturnValue == 1;
        }
    }
}
