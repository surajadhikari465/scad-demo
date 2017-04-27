Imports System.Globalization

Partial Class Item_NotOnFile
    Inherits System.Web.UI.Page

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        If Page.IsValid Then
            Dim builder As StringBuilder = WFM_Common.GetBaseReportUrlBuilder("NotOnFile", cmbReportFormat.SelectedValue)

            builder.AppendFormat("&EndOfWeekDate={0:d}", DateTime.ParseExact(wdcEndDate.Text, "d", System.Threading.Thread.CurrentThread.CurrentCulture))

            Response.Redirect(builder.ToString())
        End If
    End Sub
End Class
