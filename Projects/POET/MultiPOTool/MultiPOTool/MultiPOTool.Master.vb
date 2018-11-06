Imports System.Net

Partial Public Class MultiPOTool
    Inherits System.Web.UI.MasterPage

    Private User As String
    Private Region As String
    Private Today As String


    Public Property UserLbl() As String
        Get
            Return lblUserName.Text
        End Get
        Set(ByVal value As String)
            lblUserName.Text = value
        End Set
    End Property
    Public Property RegionLbl() As String
        Get
            Return lblRegion.Text
        End Get
        Set(ByVal value As String)
            lblRegion.Text = value
        End Set
    End Property
    Public Property DateLbl() As String
        Get
            Return lblToday.Text
        End Get
        Set(ByVal value As String)
            lblToday.Text = value
        End Set
    End Property
    Public Property TitleLbl() As String
        Get
            Return lblTitle.Text
        End Get
        Set(ByVal value As String)
            lblTitle.Text = value
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("regionName") = "United Kingdom" Then
            DateLbl = Date.Today.ToString("dd/MM/yyyy")
        Else
            DateLbl = Date.Today.ToString("d")
        End If

        If Page.IsPostBack = False Then
            Dim version As New BOManageUsers
            Dim st As String = version.SelectAppVersion

            TitleLbl = "" '"POET v" & st

        End If
        If Session("UserName") Is Nothing Then
            lblUserName.Text = String.Empty
        Else
            lblUserName.Text = "Logged in as: " & Session("UserName").ToString
        End If


        If Session("regionName") Is Nothing Then
            lblRegion.Text = String.Empty
        Else
            lblRegion.Text = "Region: " & Session("regionName").ToString
        End If

        If Session("AccessLevel") Is Nothing Then

        Else

        End If

        lblToday.Text = "Today's Date: " & DateLbl
        hpl_Email.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings("SupportEmail").ToString()


        If Session("UserID") Is Nothing Then 'Not Request.Path Like "*Login.aspx" And 
            Response.Write("this is kicking me out :)")
            Response.Redirect("Login.aspx", True)

        End If

        
        lbl_Copyright.Text = System.Configuration.ConfigurationSettings.AppSettings("CopyRights").ToString()
        lblServer.Text = "Server: " & Dns.GetHostName()

        Dim dt As DataTable
        dt = BOPONumbers.GetVersion.Tables(0)


        If dt.Rows.Count > 0 Then
            lblReleaseNo.Text = "Version: " & dt.Rows(0)("version")
        End If

    End Sub

    Private Sub lnkLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLogout.Click
        Session.Abandon()
        HttpContext.Current.Response.Redirect("~/Login.aspx")
    End Sub

    Private Sub mnuHeader_MenuItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles mnuHeader.MenuItemDataBound
        Dim item As MenuItem = e.Item
        Dim str As String = e.Item.NavigateUrl
        Dim parts As String() = str.Split(New Char() {"/"c})

        Dim part As String
        For Each part In parts
            If part.EndsWith("aspx") Then
                item.NavigateUrl = HttpContext.Current.Response.ApplyAppPathModifier(part)
            End If
        Next




    End Sub
End Class