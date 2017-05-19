Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess

Public Class DALSpreadSheetUpload
    Implements IUploadSpreadSheet

    Private _con As String = ConfigurationManager.ConnectionStrings("MultiPOTool").ConnectionString
    Public Sub InsertOrderHeader(ByVal POHeaderData As DataTable) Implements IUploadSpreadSheet.InsertOrderHeader

        ' **** SET UP BULK COPY ****
        Using bulk As New SqlBulkCopy(_con, SqlBulkCopyOptions.UseInternalTransaction Or SqlBulkCopyOptions.KeepIdentity Or SqlBulkCopyOptions.TableLock)
            bulk.DestinationTableName = "dbo.POHeader"
            Try
                Debug.WriteLine("BulkCopying PO Header Data")
                bulk.BatchSize = POHeaderData.Rows.Count
                bulk.WriteToServer(POHeaderData)
                Debug.WriteLine("BulkCopying PO Header Data - Success")
            Catch ex As Exception
                Debug.WriteLine("ERROR -- {0}", ex.Message)
                Utility.InsertErrorLog(String.Format("Message: {0} - Stacktrace {1} - Inserting PO Header - UploadSession - {2}", ex.Message, ex.StackTrace, POHeaderData.Rows(0).Item("UploadSessionHistoryID").ToString))
                ' **** Clean up UploadSessionHistory ****
                DeleteUploadSession(CInt(POHeaderData.Rows(0).Item("UploadSessionHistoryID")))
                Throw ex
            End Try
        End Using

        'TODO: Add Error Handling + Throw Exception


    End Sub

    Public Sub InsertOrderItem(ByVal POItemData As DataTable) Implements IUploadSpreadSheet.InsertOrderItem

        ' **** SET UP BULK COPY ****
        Using bulk As New SqlBulkCopy(_con, SqlBulkCopyOptions.UseInternalTransaction)
            bulk.DestinationTableName = "dbo.POItem"
            Try
                Debug.WriteLine("BulkCopying PO Item Data")
                bulk.BatchSize = POItemData.Rows.Count
                bulk.WriteToServer(POItemData)
                Debug.WriteLine("BulkCopying PO Item Data - Success")
            Catch ex As Exception
                Debug.WriteLine("ERROR -- {0}", ex.Message)
                Utility.InsertErrorLog(String.Format("Message: {0} - Stacktrace {1} - Inserting PO Item", ex.Message, ex.StackTrace))
                Throw ex
            End Try
        End Using


    End Sub

    Public Function InsertSessionHistory(ByVal param As ArrayList) As Integer Implements IUploadSpreadSheet.InsertSessionHistory


        Dim factory As New DataFactory(_con, True)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam
        Dim results As ArrayList

        Try
            Debug.WriteLine("Inserting Session History")
            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "UserID"
            currentParam.Value = param.Item(0)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "FileName"
            currentParam.Value = param.Item(1)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UploadedDate"
            currentParam.Value = param.Item(2)
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SessionHIstoryID"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.ExecuteStoredProcedure("InsertSessionHistory", paramList)
            Debug.WriteLine("Inserting Session History - Success")
        Catch ex As Exception
            Debug.WriteLine("Inserting Session History - Fail")
            Throw ex
        End Try

        Return CInt(results.Item(0).ToString)

    End Function

    Public Function GetNextPOHeaderID() As Integer

        Dim factory As New DataFactory(_con, True)
        Dim sql As New StringBuilder
        Dim result As Object = Nothing

        sql.Append("select max(POHeaderID) from POHeader")


        Try
            result = factory.ExecuteScalar(sql.ToString)
        Catch ex As Exception
            Debug.WriteLine(ex.InnerException)
            Debug.WriteLine(ex.Message)
            Throw ex
        End Try

        Return CInt(result)


    End Function
    Private Sub DeleteUploadSession(ByVal UploadSessionHistoryID As Integer)

        Dim factory As New DataFactory(_con, True)
        Dim sql As New StringBuilder

        sql.AppendFormat("delete from validationQueue where UploadSessionHistoryID = {0}; delete from UploadSessionHistory where UploadSessionHistoryID = {0}", UploadSessionHistoryID)


        Try
            factory.ExecuteNonQuery(sql.ToString)
        Catch ex As Exception
            Debug.WriteLine(ex.InnerException)
            Debug.WriteLine(ex.Message)
        End Try


    End Sub


    Public Function GetUploadedPOItems(ByVal UploadSessionID As Integer) As DataTable

        Dim factory As New DataFactory(_con, True)
        Dim result As New DataTable
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "UploadSessionID"
        currentParam.Value = UploadSessionID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        Try
            result = factory.GetStoredProcedureDataTable("GetPOItemsByUploadSessionID", paramList)
        Catch ex As Exception
            Debug.WriteLine(ex.InnerException)
            Debug.WriteLine(ex.Message)
            Throw ex
        End Try

        Return result


    End Function

    Public Function GetUploadedPOHeaders(ByVal UploadSessionID As Integer) As DataTable

        Dim factory As New DataFactory(_con, True)
        Dim result As New DataTable
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "UploadSessionID"
        currentParam.Value = UploadSessionID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        Try
            result = factory.GetStoredProcedureDataTable("GetPOHeadersByUploadSessionID", paramList)
        Catch ex As Exception
            Debug.WriteLine(ex.InnerException)
            Debug.WriteLine(ex.Message)
            Throw ex
        End Try

        Return result


    End Function


End Class
