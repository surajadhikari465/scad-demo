Partial Public Class Login
    Inherits System.Web.UI.Page
    Dim Common As StoreOrderGuide.Common

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Common.OK(Request("logout")) Then
                If Request("logout") = "1" Then
                    Session.Clear()

                    Dim menu As Object = Page.Master.FindControl("MainMenu")
                    menu.Visible = False

                    Login1.Focus()
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Protected Sub Login1_Authenticate(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.AuthenticateEventArgs) Handles Login1.Authenticate
        Dim User As New User(Trim(Login1.UserName), Trim(Login1.Password))

        Try
            If User.Login() Then
                e.Authenticated = True

                HttpContext.Current.Session("UserName") = User.UserName
                HttpContext.Current.Session("UserID") = User.UserID
                HttpContext.Current.Session("UserName") = User.UserName
                HttpContext.Current.Session("Email") = User.Email
                HttpContext.Current.Session("StoreNo") = User.StoreNo
                HttpContext.Current.Session("Admin") = User.Admin
                HttpContext.Current.Session("Buyer") = User.Buyer
                HttpContext.Current.Session("Warehouse") = User.Warehouse
                HttpContext.Current.Session("Schedule") = User.Schedule
                HttpContext.Current.Session("SuperUser") = User.SuperUser

                If User.Warehouse = True Then
                    Response.Redirect("~/Catalog/Catalog.aspx", True)
                ElseIf User.Buyer = True Then
                    Response.Redirect("~/Order/Order.aspx", True)
                ElseIf User.Admin = True Then
                    Response.Redirect("~/AdminSettings/AdminSettings.aspx", True)
                Else
                    Response.Redirect("~/Default.aspx")
                End If
            Else
                Common.MessageToUser(Page, New Exception("Invalid username or password."), False)
            End If

        Catch ex As Exception
            Common.MessageToUser(Page, ex, False)
        End Try
    End Sub

End Class