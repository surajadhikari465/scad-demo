Imports Microsoft.VisualBasic

Public Class RefreshPOSPage
    Inherits MobileViewablePage

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If _isMobileDevice Then
            MarkCurrentRefreshPOSLink()
        End If
    End Sub

End Class
