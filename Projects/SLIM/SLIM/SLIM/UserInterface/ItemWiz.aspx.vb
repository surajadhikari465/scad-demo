
Partial Class UserInterface_ItemWiz
    Inherits System.Web.UI.Page

    Protected Sub GridView1_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridView1.RowUpdating
        Label1.Text = "We are updating!"
    End Sub
End Class
