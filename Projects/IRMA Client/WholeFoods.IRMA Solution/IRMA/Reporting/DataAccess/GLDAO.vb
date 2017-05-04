Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.InvoiceThirdPartyFreight.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Ordering.DataAccess

    Public Class GLDAO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Shared Sub CommitGLTransactions(ByVal GLBO As GLBO)

            logger.Debug("CommitGLTransactions entry: Type=" + GLBO.TransactionType.ToString)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As DataTable = Nothing

            ' setup parameters for stored proc

            If Not GLBO.StoreNo = -1 Then
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = GLBO.StoreNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
            End If

            If Not GLBO.StartDate = String.Empty Then
                currentParam = New DBParam
                currentParam.Name = "StartDate"
                currentParam.Value = GLBO.StartDate
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)
            End If

            If Not GLBO.EndDate = String.Empty Then
                currentParam = New DBParam
                currentParam.Name = "EndDate"
                currentParam.Value = GLBO.EndDate
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)
            End If

            If Not GLBO.CurrentDate = String.Empty Then
                currentParam = New DBParam
                currentParam.Name = "CurrDate"
                currentParam.Value = GLBO.CurrentDate
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)
            End If

            If GLBO.TransactionType = enumOrderType.Distribution Then
                results = factory.GetStoredProcedureDataTable("dbo.CommitGLUploadDistributions", paramList)
            ElseIf GLBO.TransactionType = enumOrderType.Transfer Then
                results = factory.GetStoredProcedureDataTable("dbo.CommitGLUploadTransfers", paramList)
            End If

            logger.Debug("CommitGLTransactions exit")

        End Sub

        Public Shared Function GetGLTransactions(ByVal GLBO As GLBO) As DataTable

            logger.Debug("GetGLTransactions entry: Type=" + GLBO.TransactionType.ToString)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As DataTable = Nothing

            ' setup parameters for stored proc

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            If Not GLBO.StoreNo = -1 Then
                currentParam.Value = GLBO.StoreNo
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StartDate"
            If GLBO.StartDate.Equals(String.Empty) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = GLBO.StartDate
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "EndDate"
            If GLBO.EndDate.Equals(String.Empty) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = GLBO.EndDate
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "CurrDate"
            If GLBO.CurrentDate.Equals(String.Empty) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = GLBO.CurrentDate
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            If GLBO.TransactionType = enumOrderType.Distribution Then
                results = factory.GetStoredProcedureDataTable("dbo.GetGLUploadDistributions", paramList)
            ElseIf GLBO.TransactionType = enumOrderType.Transfer Then
                results = factory.GetStoredProcedureDataTable("dbo.GetGLUploadTransfers", paramList)
            End If

            Return results

            logger.Debug("GetGLTransactions exit")

        End Function

    End Class

End Namespace
