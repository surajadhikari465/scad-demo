Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.InvoiceThirdPartyFreight.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility
Namespace WholeFoods.IRMA.Ordering.DataAccess
    Public Class DistributionCreditOrderDAO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        ''' <summary>
        ''' Read a the Latest record from OrderItem table based on OrderHeaderID.
        ''' For creating a new return order from Store to Distribution Center.
        ''' 
        ''' This class is calling from DistributionCreditOrder.Vb class.
        ''' </summary>
        ''' <param name="OrderHeaderID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetDistributionCreditOrderOrderItemID(ByVal OrderHeaderID As Integer) As Int32

            logger.Debug("GetDistributionCreditOrderOrderItemID entry: OrderHeaderID=" + OrderHeaderID.ToString)

            Dim iOrderItemId As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "OrderItem_ID"
                currentParam.Value = DBNull.Value
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = OrderHeaderID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Record"
                currentParam.Value = -1
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute Stored Procedure to fetch the OrderItem Value.
                results = factory.GetStoredProcedureDataReader("dbo.GetOrderItemInfo", paramList)

                ' Get the OrderItemId value from DataReader.
                If results.HasRows Then
                    While results.Read
                        If (Not results.IsDBNull(results.GetOrdinal("OrderItem_ID"))) Then
                            iOrderItemId = results.GetInt32(results.GetOrdinal("OrderItem_ID"))
                        Else
                            iOrderItemId = 0
                        End If
                    End While
                End If
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetDistributionCreditOrderOrderItemID exit")
            Return iOrderItemId
        End Function
    End Class
End Namespace
