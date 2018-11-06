Partial Class MenuControl
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MenuSource.DataFile = "~/Menus/" + Application.Get("region") + "/Reports.xml"
        MenuSource.DataBind()
    End Sub

    Protected Sub MenuTreeView_TreeNodeDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.TreeNodeEventArgs) Handles MenuTreeView.TreeNodeDataBound
        Dim dPath As String = e.Node.DataPath
        Dim vPath As String = e.Node.ValuePath
        If vPath = Request.Item("valuePath") Then
            e.Node.Expand()
        End If

        If InStr(e.Node.NavigateUrl, "{0}") Then
            e.Node.NavigateUrl = String.Format(e.Node.NavigateUrl, Application.Get("reportingServicesURL"))
        End If

    End Sub
End Class


