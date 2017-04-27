Imports System
Imports System.Net

Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Label2.Text = String.Format("Server: {0}", Dns.GetHostName)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Request.Item("msg") = Nothing Then
            Select Case Request.Item("msg").ToString
                Case "Expired"
                    Label1.Text = "Your session has expired. Please log in again."
                Case Else
                    Label1.Text = String.Empty
            End Select
        End If
        'Response.Redirect("UserInterface/Main.aspx", False)

        Dim da As New UsersTableAdapters.UsersTableAdapter

        Dim Region As String = String.Empty
        Dim Version As String = String.Empty

        Region = da.GetRegion()
        Version = VersionDAO.GetVersionInfo("SLIM").ToString

        Session("Region") = Region
        Session("Version") = Version

        Session("LoggedIn") = 0
        Login1.Focus()

    End Sub

    Protected Sub Login1_Authenticate(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.AuthenticateEventArgs) Handles Login1.Authenticate
        ' *********** Instantiate (?) the class and check ifValidUser **************
        Dim ad As New AD_Access(Trim(Login1.UserName), Trim(Login1.Password))
        Dim ValUser, ValGroup As Boolean
        Dim SuperUser As Boolean
        Dim SSI As Boolean
        Dim Region As String = String.Empty
        Dim Version As String = String.Empty
        Dim HasLocation As Boolean
        Try
            ValUser = ad.IsValidUser
        Catch ex As Exception
            Label1.Text = "Error =>" & ex.Message
        End Try
        ' ************ Check IRMA Users Table *************
        Dim UserID As Integer
        Try
            Dim da As New UsersTableAdapters.UsersTableAdapter
            UserID = da.GetUserID(Trim(Login1.UserName))
            Region = da.GetRegion()
            Version = VersionDAO.GetVersionInfo("SLIM").ToString
            If Not UserID = 0 Then
                SuperUser = da.GetUserCredentials(UserID)
                HasLocation = da.HasStoreLocation(UserID)
                SSI = da.GetUserTitle(Application.Get("SSI_TitleDescription"), UserID)
                If SuperUser = True Then
                    Session("AccessLevel") = 3
                    Session("ISSConfigAuth") = True
                ElseIf SSI = True Then
                    Session("AccessLevel") = 2
                Else
                    Session("AccessLevel") = 1
                End If

            End If
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
        ' ************ ********************* *************
        If Not Session("group") Is Nothing Then
            Try
                ValGroup = True  ' ad.IsValidGroup(Session("group"))
            Catch ex As Exception
                Label1.Text = "Error =>" & ex.Message
            End Try
        End If

        ValGroup = True

        If ValUser And ValGroup And UserID Then
            e.Authenticated = True
        End If

        If (e.Authenticated) Then
            Session("UserID") = UserID
            Session("LoggedIn") = 1
            Session("UserName") = Trim(Login1.UserName)
            SetSessionPermissions()
            Response.Redirect("~/UserInterface/Main.aspx", True)
        End If

    End Sub

    Protected Sub Button_UnsecureWebQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_UnsecureWebQuery.Click
        Session("LoggedIn") = 2
        Session("WebQuery") = True
        Response.Redirect("~/UserInterface/WebQuery/Main.aspx", True)
    End Sub

    Private Sub SetSessionPermissions()

        If Session("LoggedIn") = 1 Then

            Try

                Dim da As New SlimAccessTableAdapters.SlimAccessTableAdapter
                Dim dt As New SlimAccess.SlimAccessDataTable
                dt = da.GetAccessByUserID(Session("UserID"))
                Dim dr As SlimAccess.SlimAccessRow = dt.Rows(0)
                ' ***** Set Session Variables ********
                Session("UserAdmin") = dr.UserAdmin
                Session("ItemRequest") = dr.ItemRequest
                Session("VendorRequest") = dr.VendorRequest
                Session("IRMAPush") = dr.IRMAPush
                Session("StoreSpecials") = dr.StoreSpecials
                Session("Authorizations") = dr.Authorizations
                Session("RetailCost") = dr.RetailCost
                Session("WebQuery") = dr.WebQuery
                Session("Slim_ScaleInfo") = dr.ScaleInfo
                Session("ECommerce") = dr.ECommerce
            Catch ex As Exception

            End Try

        End If

    End Sub

End Class
