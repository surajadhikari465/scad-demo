Imports Microsoft.VisualBasic

Public Class ItemECommercePage
    Inherits MobileViewablePage

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If _isMobileDevice Then
            MarkCurrentAuthorizationLink()
        End If
    End Sub

End Class
