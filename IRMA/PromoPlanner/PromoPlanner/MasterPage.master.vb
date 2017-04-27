Imports System.Net

Partial Class MasterPage
    Inherits System.Web.UI.MasterPage
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ServerLabel.Text = String.Format("Server: {0}", Dns.GetHostName)
    End Sub
End Class

