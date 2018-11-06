Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Ordering.DataAccess

    Public Class GLDAO

        Public Enum enumOrderType
            Purchase = 1
            Distribution = 2
            Transfer = 3
            Flowthru = 4
        End Enum

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
                'ElseIf GLBO.TransactionType = enumOrderType.Transfer Then
                '    results = factory.GetStoredProcedureDataTable("dbo.CommitGLUploadTransfers")
            ElseIf GLBO.TransactionType = 5 Then 'commit all records
                results = factory.GetStoredProcedureDataTable("dbo.CommitAllGLUploads")
            End If

            logger.Debug("CommitGLTransactions exit")

        End Sub
        Public Shared Sub CommitGLTransferTransactions(ByVal currentRegion As String)

            logger.Debug("CommitGLTransferTransactions entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As DataTable = Nothing

            currentParam = New DBParam
            currentParam.Name = "Region_Code"
            currentParam.Value = currentRegion
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            results = factory.GetStoredProcedureDataTable("dbo.CommitGLUploadTransfers", paramList)

            logger.Debug("CommitGLTransferTransactions exit")

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
            ElseIf GLBO.TransactionType = 5 Then 'return all records
                results = factory.GetStoredProcedureDataTable("dbo.GetAllGLUpload")
            End If

            Return results

            logger.Debug("GetGLTransactions exit")

        End Function

        Public Shared Function GetGLTransferTransactions(ByVal GLBO As GLBO, ByVal currentRegion As String) As DataTable

            logger.Debug("GetGLTransferTransactions entry: Type=" + GLBO.TransactionType.ToString)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As DataTable = Nothing

            ' setup parameters for stored proc

            currentParam = New DBParam
            currentParam.Name = "CurrDate"
            If GLBO.CurrentDate.Equals(String.Empty) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = GLBO.CurrentDate
            End If
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Region_Code"
            currentParam.Value = currentRegion
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            results = factory.GetStoredProcedureDataTable("dbo.GetGLUploadTransfersByGroup", paramList)

            Return results

            logger.Debug("GetGLTransferTransactions exit")

        End Function

        Public Shared Function GetFiscalCalendarInfo(ByVal dateValue As Date) As DataTable

            logger.Debug("GetFiscalCalendarInfo entry: Type=" + dateValue.ToString)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As DataTable = Nothing

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "@myDate"
            currentParam.Value = dateValue
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            results = factory.GetStoredProcedureDataTable("dbo.GetFiscalCalendarInfo", paramList)

            Return results

            logger.Debug("GetFiscalCalendarInfo exit")

        End Function

        Public Sub New()

        End Sub
    End Class
End Namespace
