Imports Microsoft.VisualBasic
Imports WholeFoods.Utility.SMTP
Imports System.Web.Mail

Public Class Common
    Enum enumObjectType
        StrType = 0
        IntType = 1
        DblType = 2
    End Enum

    Public Shared Sub SetDropDownStyle(ByVal DropDown As DropDownList)
        Dim MyStyle As New Style

        MyStyle.Font.Name = "Calibri"

        DropDown.ApplyStyle(MyStyle)
    End Sub

    Public Shared Sub SendEmail(ByVal msgSubject As String, ByVal msgBody As String, ByVal msgTo As String)
        Dim msgCC As String = msgTo

        Const msgFrom As String = "StoreOrderGuide@wholefoods.com"
        Const host As String = "smtp.wholefoods.com"

        Dim smtp As New SMTP(host)
        Try
            smtp.send(msgBody, msgTo, msgCC, msgFrom, msgSubject)
        Catch ex As Exception
            LogError(ex)
        End Try
    End Sub

    Public Shared Sub SendEmailHTML(ByVal msgSubject As String, ByVal msgBody As String, ByVal msgTo As String)

        Dim mail As New MailMessage()
        mail.To = msgTo
        mail.From = "StoreOrderGuide@wholefoods.com"
        mail.Subject = msgSubject
        mail.Body = msgBody
        mail.BodyFormat = MailFormat.Html
        SmtpMail.SmtpServer = "smtp.wholefoods.com"

        Try
            SmtpMail.Send(mail)
        Catch ex As Exception
            LogError(ex)
        End Try
    End Sub

    Public Shared Sub SendSupportEmail(ByVal msgSubject As String, ByVal ErrorID As Integer, ByVal ErrorMessage As String, ByVal msgTo As String)
        Dim msgCC As String = msgTo
        Dim msgBody As String = "Region/Environment: " + HttpContext.Current.Application("Region") + "/" + HttpContext.Current.Application("Environment") + vbCrLf + vbCrLf + "ErrorID: " + ErrorID.ToString + vbCrLf + vbCrLf + "ErrorMessage: " + ErrorMessage

        Const msgFrom As String = "StoreOrderGuideSupport@wholefoods.com"
        Const host As String = "smtp.wholefoods.com"

        Dim smtp As New SMTP(host)

        smtp.send(msgBody, msgTo, msgCC, msgFrom, msgSubject)
    End Sub

    Public Shared Sub MessageToUser(ByVal currentPage As Page, ByVal error1 As Exception, ByVal log As Boolean)
        Try
            '// This error happens anytime that you have Response.Redirect inside of a Try ... Catch block
            If error1.Message.Equals("Thread was being aborted.") Then Exit Sub

            Dim lblError As New UI.WebControls.Label
            Dim mainContent As Object = Nothing

            If log = True Then
                LogError(error1)
            End If

            If currentPage.Master IsNot Nothing Then
                mainContent = currentPage.Master.FindControl("pnlErrorMessage")
            End If

            If mainContent Is Nothing Then
                mainContent = currentPage
            End If

            mainContent.Visible = True

            lblError.Text &= error1.Message

            mainContent.Controls.AddAt(0, lblError)
        Catch ex As Exception
            LogError(ex)
        End Try
    End Sub

    Public Shared Sub SuccessToUser(ByVal currentPage As Page, ByVal dataMsg As Data.DataSet)
        Try
            Dim bltOrders As New UI.WebControls.BulletedList
            Dim lblOrders As New UI.WebControls.Label
            Dim mainContent As Object = Nothing

            If currentPage.Master IsNot Nothing Then
                mainContent = currentPage.Master.FindControl("pnlSuccessMessage")
            End If

            If mainContent Is Nothing Then
                mainContent = currentPage
            End If

            mainContent.Visible = True

            lblOrders.Text = "Order entry successful! The following Orders were inserted into IRMA:"

            bltOrders.DataTextField = "OrderHeader_ID"

            ' TFS 5524 show distinct order_ids from SOG_SetOrder stored procedure output:
            'bltOrders.DataSource = dataMsg

            Dim dtUniqRecords As New DataTable
            dtUniqRecords = dataMsg.Tables(0).DefaultView.ToTable(True, "OrderHeader_ID")
            bltOrders.DataSource = dtUniqRecords

            bltOrders.DataBind()

            mainContent.Controls.AddAt(0, lblOrders)
            mainContent.Controls.AddAt(1, bltOrders)
        Catch ex As Exception
            LogError(ex)
        End Try
    End Sub

    Public Shared Sub LogError(ByRef ErrorMessage As Exception)
        Dim Dal As New Dal
        Dim user As String
        If HttpContext.Current.Session("UserName") Is Nothing Then
            user = "Unknown"
        Else
            user = HttpContext.Current.Session("UserName")
        End If
        Dim ErrorID As Integer = Dal.AddError(ErrorMessage, user, GetClientComputer())

        SendSupportEmail("StoreOrderGuide - Error", ErrorID, ErrorMessage.Message, HttpContext.Current.Application("SupportEmail"))
    End Sub

    Public Shared Sub SendToDefaultPage(ByVal currentPage As Page)
        Try
            HttpContext.Current.Response.Redirect(currentPage.ResolveUrl(HttpContext.Current.Application("HomePage")))

        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Shared Function GetItemCatalogDatabase() As Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase
        Return New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(System.Configuration.ConfigurationManager.ConnectionStrings(ConfigurationManager.AppSettings("ConnectionStringName")).ConnectionString)
    End Function

    Public Shared Function GetValue(ByVal dr As Data.Common.DbDataReader, ByVal columnName As String, ByVal defaultValue As Object) As Object
        Dim result As Object = defaultValue

        Try
            If Not IsDBNull(dr(columnName)) Then
                result = dr(columnName)
            End If

        Catch ex As Exception
            LogError(ex)
            Throw New Exception("Encountered error, " & ex.Message & ". Retrieving column '" & columnName & "' from data reader.")
        End Try

        Return result
    End Function

    Public Shared Function OK(ByVal this As Object) As Boolean
        Return (Not this Is Nothing)
    End Function

    Public Shared Function NotOK(ByVal this As Object) As Boolean
        Return (this Is Nothing)
    End Function

    Public Shared Function GetClientComputer() As String
        Dim host As String

        Try
            host = System.Net.Dns.GetHostEntry(HttpContext.Current.Request.UserHostAddress).HostName
        Catch ex As Exception
            host = "UNKNOWN"
        End Try

        Return host
    End Function

    Public Shared Function ValidateSession() As Boolean
        If HttpContext.Current.Session.Count > 0 Then
            Return True
        Else
            FormsAuthentication.RedirectToLoginPage()
        End If
    End Function

    Public Shared Function CheckDBNull(ByVal obj As Object, Optional ByVal ObjectType As enumObjectType = enumObjectType.StrType) As Object
        Dim objReturn As Object
        objReturn = obj

        If ObjectType = enumObjectType.StrType And IsDBNull(obj) Then
            objReturn = ""
        ElseIf ObjectType = enumObjectType.IntType And IsDBNull(obj) Then
            objReturn = 0
        ElseIf ObjectType = enumObjectType.DblType And IsDBNull(obj) Then
            objReturn = 0.0
        End If

        Return objReturn
    End Function
End Class