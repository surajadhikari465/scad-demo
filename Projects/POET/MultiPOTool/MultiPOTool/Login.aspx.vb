Imports AD_Access
Imports WholeFoods.Utility
Imports WFM.UserAuthentication
Imports System.Net
Imports VB = Microsoft.VisualBasic

'test for conversion



Partial Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Dim UserCookie As HttpCookie = Request.Cookies("UserInfo")
            Dim LoginCookie As HttpCookie = Request.Cookies("LoginInfo")


            If Not UserCookie Is Nothing Then
                Login1.UserName = Server.HtmlEncode(UserCookie.Values("UserName").ToString)
                Login1.RememberMeSet = True
                Login1.Focus()
            End If

            If Not LoginCookie Is Nothing Then
                If LoginCookie.Values("UserOK") = True Then
                    SystemMessage.Text = ""
                Else
                    SystemMessage.Text = "Login failed. Please try again."
                    Login1.Focus()
                End If
            End If
        End If

    End Sub

    Protected Sub Login1_Authenticate(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.AuthenticateEventArgs) Handles Login1.Authenticate

        ' ****** Validate Users ********
        Dim IsADValidated As Boolean
        Dim IsUserValidated As Boolean
        Dim username As String
        Dim password As String
        Dim userCredentials As New DataTable
        Dim dc As New DataColumn

        Dim LoginCookie As New HttpCookie("LoginInfo")
        LoginCookie.Expires = DateTime.Now.AddDays(30)


        ' ****** Trim Entries ********

        username = Trim(Login1.UserName)
        password = Trim(Login1.Password)


        Dim Val As New BOUserLogin(username, password)
        ' ****** Validate AD ********
        IsADValidated = Val.ValidateUserActiveDirectory()
        ' ****** Validate Users Table ********
        IsUserValidated = Val.ValidateUserTable()

        If IsADValidated = True And IsUserValidated = True Then
            e.Authenticated = True
        Else
            e.Authenticated = False
        End If


        ' ****** Cache User Credentials ********
        If e.Authenticated Then

            userCredentials = Val.GetUserCredentials()

            For Each dc In userCredentials.Columns

                Session(dc.ColumnName) = userCredentials.Rows(0).Item(dc.ColumnName)

            Next

            ' ****** Set Cookie if remember me .... ******

            If Login1.RememberMeSet = True Then
                Dim UserCookie As New HttpCookie("UserInfo")
                UserCookie.Values("UserName") = username
                UserCookie.Values("PassWord") = password
                UserCookie.Expires = DateTime.Now.AddDays(30)
                Response.Cookies.Add(UserCookie)
            Else
                Response.Cookies("UserInfo").Expires = Date.Now
            End If

            LoginCookie.Values("UserOK") = True
        Else
            LoginCookie.Values("UserOK") = False
        End If

        Response.Cookies.Add(LoginCookie)

        Response.Redirect("~/Upload.aspx", False)

    End Sub

End Class