namespace CursoUIApi.DI
{
    public static class MyQuery
    {
        public static string GetQueryFaturamento(string pData1, string pData2, string pCardCode)
        {
            var query = $@"SELECT 
	                          '' as ""Selecionar""
	                          ,T0.""DocEntry"" as ""DocSap""
	                          ,T0.""DocNum"" as ""NumPedido""
	                          ,T0.""CardCode"" as ""Cliente""
	                          ,T1.""CardName"" as ""Descrição""
	                          ,T1.""ShipToDef"" as ""Endereço""
                              ,T3.""State"" as ""UF""
                              ,T4.""Name"" as ""Cidade""
	                          ,T0.""DocDate"" as ""Data""
	                          ,T0.""DocTotal"" as ""ValorTotal""
	                          ,(Select ""SlpName"" From OSLP Where ""SlpCode"" = T0.""SlpCode"") as ""Vendedor""
	                          ,T0.""GroupNum""
	                          ,(Select ""PymntGroup"" From OCTG Where ""GroupNum"" = T0.""GroupNum"") as ""Condicao""
	                          ,(Case 
                                    When T0.""CANCELED"" = 'Y' Then 'Cancelado'
                                    When T0.""DocStatus"" = 'C' Then 'Fechado'
                                    When (Select Count(1) From RDR1 where ""DocEntry"" = T0.""DocEntry"" and ""TrgetEntry"" > 0) > 0 Then 'Faturado Parcial' 
                                    Else 'Aberto' 
                                End) as ""Status""
                              ,T0.""BPLId""
                              ,(Select Max(""BPLName"") From OBPL where ""BPLId"" = T0.""BPLId"") as ""Empresa""
                              ,T0.""Comments""
                        FROM ORDR T0 Inner Join
                             OCRD T1 on T0.""CardCode"" = T1.""CardCode"" INNER JOIN 
                             RDR12 T3 ON T0.""DocEntry"" = T3.""DocEntry"" LEFT JOIN
						     OCNT T4 ON T3.""County"" = T4.""AbsId""
                        WHERE T0.""DocStatus"" = 'O' ";

            if (!string.IsNullOrWhiteSpace(pData1))
                query += $@" AND T0.""DocDate"" >= CAST('{pData1}' AS DATE)";

            if (!string.IsNullOrWhiteSpace(pData2))
                query += $@" AND T0.""DocDate"" <= CAST('{pData2}' AS DATE)";

            if (!string.IsNullOrWhiteSpace(pCardCode))
                query += $@" AND T0.""CardCode"" = '{pCardCode}'";

            query += $@" ORDER BY T0.""DocEntry"" desc";

            return query;
        }
        public static string GetQueryFaturamentoItens(int docEntry)
        {
            var query = $@"Select 
                                T0.""DocEntry"" as ""Doc.Sap""
                               ,T0.""LineNum"" as ""LineNum""
                               ,T0.""ItemCode"" as ""Item""
                               ,T0.""Dscription"" as ""Descrição""
                               ,Case When T0.""OpenQty"" = 0 then 'Y' else 'N' End as ""Faturado""
                               ,T0.""unitMsr"" as ""Unidade""
                               ,T0.""Price"" as ""Preço""
                               ,T0.""Quantity"" as ""Qtd Pedido""
                               ,T0.""OpenQty"" as ""Quantidade""
                               ,T0.""OpenQty"" as ""Qtd a Faturar""
							   ,T0.""LineTotal"" as ""Valor Total""
							   ,T0.""WhsCode"" as ""Deposito""
                               ,'' as ""Obs""
                          from RDR1 T0 Inner Join
							   OITM T1 on T0.""ItemCode"" = T1.""ItemCode""
                          where T0.""DocEntry"" = {docEntry}";

            return query;
        }
    }
}
