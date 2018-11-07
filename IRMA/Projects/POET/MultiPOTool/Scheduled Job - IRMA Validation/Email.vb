Imports WholeFoods.Utility.SMTP
Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess

Public Class Email
    Public Shared Sub InsertErrorLog(ByVal ErrorMessage As String)

        Dim con As String = ConfigurationManager.ConnectionStrings("MultiPOTool").ConnectionString()
        Dim factory As New DataFactory(con, True)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "ErrorMessage"
            currentParam.Value = ErrorMessage
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("InsertErrorLog", paramList)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub SendEmail(ByVal msgBody As String)
        Dim environment As String = ConfigurationManager.AppSettings("environment")
        Dim msgSubject As String = environment & " - POET Error"

        Dim msgTo As String = ConfigurationManager.AppSettings("Admin")
        Dim msgCC As String = ConfigurationManager.AppSettings("CC")

        Const msgFrom As String = "POET@wholefoods.com"
        Const host As String = "smtp.wholefoods.com"

        Dim smtp As New SMTP(host)
        Try
            smtp.send(msgBody, msgTo, msgCC, msgFrom, msgSubject)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Sub


    Public Sub SendEmail(ByVal UploadSessionID As String, ByVal msgBody As String, ByVal msgTo As String, ByVal msgCC As String, Optional ByVal Exception As Boolean = False)
        Dim environment As String = ConfigurationManager.AppSettings("environment")
        Dim msgSubject As String = environment & " POET - Validation Notice " & "[Upload SessionID = " & UploadSessionID & "]"

        If Exception = True Then
            msgSubject = environment & " POET - Validation EXCEPTIONS " & "[Upload SessionID = " & UploadSessionID & "]"
        End If

        Const msgFrom As String = "POET@wholefoods.com"
        Const host As String = "smtp.wholefoods.com"
        If Trim(msgCC) = String.Empty Then
            msgCC = Nothing
        End If
        Dim smtp As New SMTP(host)
        Try
            smtp.send(msgBody, msgTo, msgCC, msgFrom, msgSubject)
            InsertErrorLog("VALIDATION EMAIL=> " + msgSubject)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try 'test test--- conversion complete
    End Sub
End Class
