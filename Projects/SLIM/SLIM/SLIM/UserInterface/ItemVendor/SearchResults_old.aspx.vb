
Partial Class UserInterface_ItemVendor_SearchResults_old
    Inherits System.Web.UI.Page

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Response.Redirect("Authorizations.aspx?ItemKey=" & _
     GridView1.SelectedValue & "&UPC=" & _
     GridView1.SelectedRow.Cells(1).Text & _
     "&Description=" & GridView1.SelectedRow.Cells(3).Text, False)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Label1.Text = (GridView1.Rows.Count * GridView1.PageCount) & " Row(s) Found"
    End Sub
End Class
