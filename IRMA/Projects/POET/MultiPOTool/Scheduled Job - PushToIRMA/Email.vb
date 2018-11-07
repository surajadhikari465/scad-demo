Imports WholeFoods.Utility.SMTP

Public Class Email

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
    'test for project conversion
End Class
