
Partial Class UserInterface_WebQuery_Main
    Inherits WebQueryPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        UpdateMenuLinks()
    End Sub

    Protected Sub UpdateMenuLinks()
        If Not _isMobileDevice Then
            If Session("LoggedIn") = 2 Then
                Master.HideMenuLinks("ISS", "ISSNew", False)
                Master.HideMenuLinks("ItemRequest", "NewItem", False)
            Else
                If Not Session("Store_No") > 0 Then
                    Master.HideMenuLinks("ISS", "ISSNew", False)
                    Master.HideMenuLinks("ItemRequest", "NewItem", False)
                Else
                    Master.HideMenuLinks("ISS", "ISSNew", True)
                    Master.HideMenuLinks("ItemRequest", "NewItem", True)
                End If
            End If
        End If
    End Sub
End Class
