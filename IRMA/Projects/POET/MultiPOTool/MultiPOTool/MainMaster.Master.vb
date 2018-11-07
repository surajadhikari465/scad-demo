Imports System.Net

Public Class Site1
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    
        lbl_Copyright.Text = System.Configuration.ConfigurationSettings.AppSettings("CopyRights").ToString()
        lblServer.Text = "Server: " & Dns.GetHostName()
        hpl_Email.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings("SupportEmail").ToString()


        If BOPONumbers.GetVersion.tables(0).rows.count > 0 Then
            lblReleaseNo.Text = "Version: " & BOPONumbers.GetVersion.Tables(0).Rows(0)("version")
        End If

    End Sub
End Class