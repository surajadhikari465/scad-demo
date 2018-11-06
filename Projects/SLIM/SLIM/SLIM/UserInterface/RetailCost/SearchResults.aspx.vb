
Partial Class UserInterface_RetailCost_SearchResults
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Label1.Text = (GridView1.Rows.Count * GridView1.PageCount) & " Row(s) Found"
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Response.Redirect("RetailCost.aspx?ItemKey=" & _
        GridView1.SelectedValue & "&UPC=" & _
        GridView1.SelectedRow.Cells(1).Text & _
        "&Description=" & GridView1.SelectedRow.Cells(3).Text, False)
    End Sub
End Class
