
Partial Class UserInterface_RefreshPOSPage_Main
    Inherits RefreshPOSPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' ****** Check Permission **************
        If Not IsPostBack Then
            If Not Session("ItemRequest") = True Then
                Response.Redirect("~/AccessDenied.aspx", True)
            End If
        End If
        ' *****************************
        UpdateMenuLinks()
    End Sub

    Protected Sub UpdateMenuLinks()
        If Not _isMobileDevice Then
            If Not Session("Store_No") > 0 Then
                Master.HideMenuLinks("ISS", "ISSNew", False)
                Master.HideMenuLinks("ItemRequest", "NewItem", False)
            Else
                Master.HideMenuLinks("ISS", "ISSNew", True)
                Master.HideMenuLinks("ItemRequest", "NewItem", True)
            End If
        End If
    End Sub
End Class
