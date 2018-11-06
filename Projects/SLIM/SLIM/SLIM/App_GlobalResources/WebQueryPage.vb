Imports Microsoft.VisualBasic

Public Class WebQueryPage
    Inherits MobileViewablePage

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If _isMobileDevice Then
            MarkCurrentWebQueryLink()
        Else
            If Session("LoggedIn") = 1 Then
                Me.Master.Page.Title = "Secure WebQuery"
            Else
                Me.Master.Page.Title = "UnSecure WebQuery"
            End If
        End If
    End Sub
End Class
