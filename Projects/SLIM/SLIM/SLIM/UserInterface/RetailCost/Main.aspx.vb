
Partial Class UserInterface_RetailCost_Main
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' ****** Check Permission **************
        If Not IsPostBack Then
            If Not Session("RetailCost") = True And Not Session("AccessLevel") = 3 Then
                Response.Redirect("~/AccessDenied.aspx", True)
            End If
            If Session("AccessLevel") = 1 Then
                depDropDown.DataSourceID = "SqlDataSource5"
                depDropDown.DataBind()
            End If
        End If
        ' *****************************
    End Sub

    Protected Sub SearchItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchItem.Click

        ' ********** Validate the User Input *************
        Dim upc, desc As String
        upc = upcTxBx.Text.ToString.Trim()
        desc = descTxBx.Text.ToString.Trim()
        Response.Redirect("Searchresults.aspx?UPC=" & upc & "&Description=" & desc & _
                    "&Dept=" & depDropDown.SelectedValue & "&Brand=" & brandDropDown.SelectedValue, False)
        ' *************************************************
    End Sub

    Protected Sub depDropDown_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles depDropDown.SelectedIndexChanged
        If Not depDropDown.SelectedValue = 0 Then
            brandDropDown.Items.Clear()
            brandDropDown.DataSourceID = "SqlDataSource2"
            brandDropDown.Items.Insert(0, "--Select--")
            brandDropDown.Items.Item(0).Value = 0
            brandDropDown.DataBind()
        Else
            brandDropDown.Items.Clear()
            brandDropDown.DataSourceID = "SqlDataSource3"
            brandDropDown.Items.Insert(0, "--Select--")
            brandDropDown.Items.Item(0).Value = 0
            brandDropDown.DataBind()
        End If
    End Sub
End Class
