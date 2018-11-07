
Partial Class FiscalCalendar_FiscalCalendar
    Inherits System.Web.UI.Page

    Protected Sub lbtnFiscalCalendar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnFiscalCalendar.Click
        Response.Redirect("fisc_cal.xls")
    End Sub

    Protected Sub lbtnSWCommCodes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnSWCommCodes.Click
        Response.Redirect("CommodityCodes.xls")
    End Sub
End Class
