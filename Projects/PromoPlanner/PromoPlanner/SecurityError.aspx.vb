
Partial Class SecurityError
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim errMessage As String = Request.Item("error")
        Select Case errMessage
            Case "1"
                errMsg.Text = "You were recognized as a valid WFM user but your IRMA Security Setup does not have the correct configuration for the Promo Admin role. Please contact the Regional Data Team to correct this problem. ERROR: UserStoreTeamTitle Store_No Not Zero"
            Case "2"
                errMsg.Text = "You were recognized as a valid WFM user but your IRMA Security Setup does not have the correct configuration for the Promo Ordering role. Please contact the Regional Data Team to correct this problem. ERROR: UserStoreTeamTitle Store_No Not Specified"
            Case Else
                errMsg.Text = "You were recognized as a valid WFM user but your IRMA Security Setup does not have the correct configuration to access Promo Planner. Please contact the Regional Data Team to correct this problem. ERROR: UserStoreTeamTitle Not Populated"
        End Select
    End Sub
End Class
