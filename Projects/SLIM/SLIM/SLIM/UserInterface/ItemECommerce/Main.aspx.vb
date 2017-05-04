
Partial Class UserInterface_ItemEcommerce_Main
    Inherits ItemECommercePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' ****** Check Permission **************
        If Not IsPostBack Then
            If Not Session("ECommerce") = True And Not Session("AccessLevel") = 3 Then
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
