Imports WholeFoods.ServiceLibrary.DataAccess
Imports WholeFoods.ServiceLibrary.IRMA.Common
Imports System.Linq
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Configuration

Namespace IRMA

    <DataContract()>
    Public Class InvoiceCharge

        <DataMember()>
        Public Property SACType As String
        <DataMember()>
        Public Property GLPurchaseAccount As Integer
        <DataMember()>
        Public Property Description As String
        <DataMember()>
        Public Property ChargeID As Integer
        <DataMember()>
        Public Property IsAllowance As String
        <DataMember()>
        Public Property ElementName As String
        <DataMember()>
        Public Property ChargeValue As Decimal


        Public Shared Function GetOrderInvoiceCharges(ByVal orderHeaderID As Integer) As List(Of InvoiceCharge)

            logger.Info("GetOrderInvoiceCharges() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim dataTable As DataTable
            Dim parameterList As New ArrayList
            Dim currentParameter As DBParam
            Dim invoiceCharges As New List(Of InvoiceCharge)

            ' ******* Parameters ************
            currentParameter = New DBParam
            currentParameter.Name = "OrderHeader_Id"
            currentParameter.Value = orderHeaderID
            currentParameter.Type = DBParamType.Int
            parameterList.Add(currentParameter)

            Try
                dataTable = factory.GetStoredProcedureDataTable("OrderInvoice_GetOrderInvoiceSpecCharges", parameterList)

                For Each dataRow As DataRow In dataTable.Rows
                    Dim invoiceCharge As New InvoiceCharge

                    invoiceCharge.SACType = dataRow.Item("Type")
                    invoiceCharge.ChargeValue = NotNull(dataRow.Item("Value"), Nothing)
                    invoiceCharge.GLPurchaseAccount = NotNull(dataRow.Item("GLAccount"), Nothing)
                    invoiceCharge.Description = NotNull(dataRow.Item("Description"), Nothing)
                    invoiceCharge.ChargeID = dataRow.Item("Charge_ID")
                    invoiceCharge.IsAllowance = dataRow.Item("IsAllowance")
                    invoiceCharge.ElementName = NotNull(dataRow.Item("ElementName"), Nothing)

                    invoiceCharges.Add(invoiceCharge)
                Next

                Return invoiceCharges
            Catch ex As Exception
                Throw ex
            Finally
                connectionCleanup(factory)
            End Try

        End Function

        Public Shared Function GetGLAccountSubteams(ByVal orderHeaderID As Integer) As List(Of Lists.Subteam)

            logger.Info("GetSubteamGLAccount() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim dataTable As DataTable
            Dim parameterList As New ArrayList
            Dim currentParameter As DBParam
            Dim subteams As New List(Of Lists.Subteam)


            ' ******* Parameters ************
            currentParameter = New DBParam
            currentParameter.Name = "OrderHeader_ID"
            currentParameter.Value = orderHeaderID
            currentParameter.Type = DBParamType.Int
            parameterList.Add(currentParameter)

            Try
                dataTable = factory.GetStoredProcedureDataTable("OrderInvoice_GetGLAcctSubteams", parameterList)

                For Each dataRow As DataRow In dataTable.Rows
                    Dim subteam As New Lists.Subteam

                    subteam.SubteamNo = dataRow.Item("SubTeam_No")
                    subteam.SubteamName = NotNull(dataRow.Item("SubTeamGLAcct"), Nothing)

                    subteams.Add(subteam)
                Next

                Return subteams
            Catch ex As Exception
                Throw ex
            Finally
                connectionCleanup(factory)
            End Try
        End Function

        Public Shared Function GetAllocatedCharges() As List(Of InvoiceCharge)
            logger.Info("GetAllocatedCharges() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim dataTable As DataTable
            Dim allocatedCharges As New List(Of InvoiceCharge)

            Try
                dataTable = factory.GetStoredProcedureDataTable("Einvoicing_GetAllocatedCharges")

                For Each dataRow As DataRow In dataTable.Rows
                    Dim allocatedCharge As New InvoiceCharge

                    allocatedCharge.ElementName = dataRow.Item("AllocatedCharge")
                    allocatedCharge.IsAllowance = dataRow.Item("ChargeOrAllowance")

                    allocatedCharges.Add(allocatedCharge)
                Next

                Return allocatedCharges
            Catch ex As Exception
                Throw ex
            Finally
                connectionCleanup(factory)
            End Try

        End Function

        Public Shared Function AddInvoiceCharge(ByVal orderHeaderID As Integer, ByVal SACTypeID As Integer, ByVal description As String, _
                                                ByVal subteamNumber As Integer, ByVal allowance As Boolean, ByVal amount As Decimal) As Result
            logger.Info("AddInvoiceCharge() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim parameterList As New ArrayList
            Dim currentParameter As DBParam
            Dim result As New Result


            ' ******* Parameters ************
            currentParameter = New DBParam
            currentParameter.Name = "OrderHeader_Id"
            currentParameter.Value = orderHeaderID
            currentParameter.Type = DBParamType.Int
            parameterList.Add(currentParameter)

            currentParameter = New DBParam
            currentParameter.Name = "SACType_ID"
            currentParameter.Value = SACTypeID
            currentParameter.Type = DBParamType.Int
            parameterList.Add(currentParameter)

            currentParameter = New DBParam
            currentParameter.Name = "Description"
            currentParameter.Value = description
            currentParameter.Type = DBParamType.String
            parameterList.Add(currentParameter)

            currentParameter = New DBParam
            currentParameter.Name = "SubTeam_No"
            currentParameter.Value = subteamNumber
            currentParameter.Type = DBParamType.Int
            parameterList.Add(currentParameter)

            currentParameter = New DBParam
            currentParameter.Name = "isAllowance"
            currentParameter.Value = allowance
            currentParameter.Type = DBParamType.Bit
            parameterList.Add(currentParameter)

            currentParameter = New DBParam
            currentParameter.Name = "Value"
            currentParameter.Value = amount
            currentParameter.Type = DBParamType.Money
            parameterList.Add(currentParameter)

            Try
                factory.ExecuteStoredProcedure("OrderInvoice_InsOrderInvoiceSpecCharge", parameterList)

                result.FunctionName = "OrderInvoice_InsOrderInvoiceSpecCharge"
                result.Status = True

                logger.Info("AddInvoiceCharge() completed successfully.")
                Return result

            Catch ex As Exception
                With result
                    .FunctionName = "OrderInvoice_InsOrderInvoiceSpecCharge"
                    .ErrorMessage = "OrderInvoice_InsOrderInvoiceSpecCharge failed."
                    .Exception = ex.Message
                    .SQLString = factory.GetSQLString("OrderInvoice_InsOrderInvoiceSpecCharge", parameterList)
                    .Status = False
                End With

                Return result

            Finally
                connectionCleanup(factory)

            End Try

        End Function

        Public Shared Function RemoveInvoiceCharge(ByVal chargeID As Integer) As Result

            logger.Info("RemoveInvoiceCharge() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim parameterList As New ArrayList
            Dim currentParameter As DBParam
            Dim result As New Result


            ' ******* Parameters ************
            currentParameter = New DBParam
            currentParameter.Name = "Charge_Id"
            currentParameter.Value = chargeID
            currentParameter.Type = DBParamType.Int
            parameterList.Add(currentParameter)


            Try
                factory.ExecuteStoredProcedure("OrderInvoice_DelOrderInvoiceSpecCharge", parameterList)

                result.FunctionName = "OrderInvoice_DelOrderInvoiceSpecCharge"
                result.Status = True

                logger.Info("RemoveInvoiceCharge() completed successfully.")
                Return result

            Catch ex As Exception
                With result
                    .FunctionName = "OrderInvoice_DelOrderInvoiceSpecCharge"
                    .ErrorMessage = "OrderInvoice_DelOrderInvoiceSpecCharge failed."
                    .Exception = ex.Message
                    .SQLString = factory.GetSQLString("OrderInvoice_DelOrderInvoiceSpecCharge", parameterList)
                    .Status = False
                End With

                Return result

            Finally
                connectionCleanup(factory)

            End Try

        End Function

    End Class
End Namespace