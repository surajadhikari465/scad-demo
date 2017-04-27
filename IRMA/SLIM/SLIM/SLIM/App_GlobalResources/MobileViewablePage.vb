Imports Microsoft.VisualBasic

Public Class MobileViewablePage
    Inherits System.Web.UI.Page
    Protected _isMobileDevice As Boolean = False

    Public ReadOnly Property IsMobileDevice() As Boolean
        Get
            Return _isMobileDevice
        End Get
    End Property

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        _isMobileDevice = Request.Browser.IsMobileDevice

        If _isMobileDevice Then
            Me.MasterPageFile = "~/UserInterface/Mobile.master"
        End If
    End Sub

    Private Sub MarkCurrentMasterPageLink(ByVal linkID As String)
        CType(Me.Master.FindControl(linkID), HyperLink).CssClass = "CurrentSection"
    End Sub

    Protected Sub MarkCurrentWebQueryLink()
        MarkCurrentMasterPageLink("lnkWebQuery")
    End Sub

    Protected Sub MarkCurrentAuthorizationLink()
        MarkCurrentMasterPageLink("lnkAuthorizations")
    End Sub

    Protected Sub MarkCurrentRefreshPOSLink()
        MarkCurrentMasterPageLink("lnkRefresh")
    End Sub

End Class
