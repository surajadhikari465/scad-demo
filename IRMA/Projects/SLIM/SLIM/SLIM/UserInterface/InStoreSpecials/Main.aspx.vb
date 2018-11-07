Imports System.Xml
Imports System.IO

Partial Class UserInterface_InStoreSpecials_Main
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' ****** Check Permission **************
            If Not Session("StoreSpecials") = True And Not Session("AccessLevel") = 3 Then
                Response.Redirect("~/AccessDenied.aspx", True)
            End If
            ' *****************************
        End If
    End Sub

End Class
