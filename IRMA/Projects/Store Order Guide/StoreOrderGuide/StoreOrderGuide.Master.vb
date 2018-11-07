Imports System
Imports System.Net
Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls

Partial Class AppMaster
    Inherits System.Web.UI.MasterPage
    Dim Common As StoreOrderGuide.Common

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ServerLabel.Text = String.Format("Server: {0}", Dns.GetHostName)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '//Accidental redirect circular trap
        If Page.Title.ToLower.Equals("login") Then Exit Sub

        '//Write app details to header
        lblAppDetails.Text = HttpContext.Current.Application("Region") + " - " + HttpContext.Current.Application("Environment") + " (" + HttpContext.Current.Application("Version") + ")"

        '//Check for login and write to header if present
        If Common.ValidateSession() Then
            litLoggedInUser.Text = HttpContext.Current.Session("UserName") & "(" & HttpContext.Current.Session("StoreNo") & ")"
        End If

        '//User validation 
        If HttpContext.Current.Session("SuperUser") <> "True" Then
            Select Case Page.Title
                Case "Catalog"
                    If HttpContext.Current.Session("Warehouse") <> "True" Then
                        Response.Redirect("~/AccessDenied.aspx", True)
                    End If

                Case "Order"
                    If HttpContext.Current.Session("Buyer") <> "True" Then
                        If HttpContext.Current.Session("Warehouse") <> "True" Then
                            Response.Redirect("~/AccessDenied.aspx", True)
                        End If
                    End If

                Case "Admin"
                    If HttpContext.Current.Session("Admin") <> "True" Then
                        Response.Redirect("~/AccessDenied.aspx", True)
                    End If

                Case "Schedule"
                    If HttpContext.Current.Session("Schedule") <> "True" Then
                        Response.Redirect("~/AccessDenied.aspx", True)
                    End If
            End Select
        End If
    End Sub
End Class