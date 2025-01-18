using SAPbouiCOM;

namespace Support.Framework.Extentions
{
    public static class GridExtention
    {
        public static void DisableAllColumns(this Grid grid)
        {
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                var col = grid.Columns.Item(i);
                col.Editable = false;
            }
        }

        /// <summary>
        /// Habilida ordenação via 2 cliques nas colunas da grid
        /// </summary>
        /// <param name="grid"></param>
        public static void EnableSortingAllColumns(this Grid grid)
        {
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                var col = grid.Columns.Item(i);
                col.TitleObject.Sortable = true;
            }
        }

        public static void AutoResizeColumnsIfNeeded(this Grid grid)
        {
            if (grid.Columns.Count > 0)
                grid.AutoResizeColumns();
        }

        /// <summary>
        /// Alinha o texto da coluna a direita e mostra o somatório no rodapé
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="columnName"></param>
        public static void ShowColumnSum(this Grid grid, string columnName)
        {
            var col = (SAPbouiCOM.EditTextColumn)grid.Columns.Item(columnName);
            col.ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
            col.RightJustified = true;
        }

        public static void SelectRow(this Grid grid, int rowIndex)
        {
            try
            {
                if (rowIndex >= 0)
                    grid.Rows.SelectedRows.Add(rowIndex);
            }
            catch
            {
            }
        }
    }
}
