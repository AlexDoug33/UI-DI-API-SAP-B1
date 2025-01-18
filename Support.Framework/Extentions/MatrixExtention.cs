using System;

namespace Support.Framework.Extentions
{
    public static class MatrixExtention
    {
        public static void AutoResizeColumnsIfNeeded(this SAPbouiCOM.Matrix matrix)
        {
            if (matrix == null)
                return;

            if (matrix.Columns.Count <= 0)
                return;

            try
            {
                matrix.AutoResizeColumns();
            }
            catch { }
        }

        public static void AddNewRow(this SAPbouiCOM.Matrix matrix, SAPbouiCOM.Form form, string colIndex = "#")
        {
            form.Freeze(true);

            matrix.AddRow();

            var iRow = matrix.VisualRowCount;

            matrix.ClearRowData(iRow);
            matrix.SetCellFocus(iRow, 1);

            matrix.UpdateNewRowNumber(iRow, colIndex);

            matrix.AutoResizeColumnsIfNeeded();

            form.Freeze(false);
        }

        public static void UpdateNewRowNumber(this SAPbouiCOM.Matrix matrix, int iRow, object colIndex)
        {
            string newRowNum;

            if (iRow <= 1)
                newRowNum = iRow.ToString();
            else
            {
                var previewRowNum = matrix.GetCellEditTextValue(iRow - 1, colIndex);
                newRowNum = (Convert.ToInt32(previewRowNum) + 1).ToString();
            }

            matrix.SetCellEditTextValue(iRow, colIndex, newRowNum);
        }

        public static string GetCellEditTextValue(this SAPbouiCOM.Matrix matrix, int rowNum, object colIndex)
        {
            return ((SAPbouiCOM.EditText)matrix.Columns.Item(colIndex)
                    .Cells.Item(rowNum).Specific).Value;
        }

        public static void SetCellEditTextValue(this SAPbouiCOM.Matrix matrix, int rowNum, object colIndex, string value)
        {
            ((SAPbouiCOM.EditText)matrix.Columns.Item(colIndex)
                    .Cells.Item(rowNum).Specific).Value = value;
        }

        public static void RemoveSelectedRow(this SAPbouiCOM.Matrix matrix)
        {
            if (!matrix.HasSelectedRow())
                return;

            var iRow = matrix.GetSelectedRowIndex();
            matrix.DeleteRow(iRow);
            //form.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
            //matrix.UpdateRowNumber("#");
            matrix.FlushToDataSource();            
        }

        public static bool HasSelectedRow(this SAPbouiCOM.Matrix matrix)
        {
            var selectedRowIndex = matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);
            return selectedRowIndex > -1;
        }

        /// <summary>
        ///  Returns -1 if not selected
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static int GetSelectedRowIndex(this SAPbouiCOM.Matrix matrix)
        {
            return matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);
        }
    }
}
