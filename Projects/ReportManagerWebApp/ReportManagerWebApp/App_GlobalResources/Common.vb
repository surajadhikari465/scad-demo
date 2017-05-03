Imports Microsoft.VisualBasic

Public Class WFM_Common

    Public Shared Function GetUniversalDateString(ByRef DateValue As Date) As String

        Dim sDateString As String

        Const DATE_DELIMITER As Char = "-"

        sDateString = Year(DateValue).ToString & DATE_DELIMITER _
                        & Month(DateValue).ToString & DATE_DELIMITER _
                        & Day(DateValue).ToString

        Return sDateString

    End Function

    Public Shared Function GetBaseReportUrlBuilder(ByVal reportPageName As String, ByVal format As String) As System.Text.StringBuilder
        Dim sReportURL As New System.Text.StringBuilder

        'report name
        sReportURL.AppendFormat("{0}{1}_{2}", HttpContext.Current.Application.Get("reportingServicesURL"), HttpContext.Current.Application.Get("region"), reportPageName)

        'report display
        If format <> "HTML" Then
            sReportURL.AppendFormat("&rs:Format={0}", format)
        End If

        sReportURL.Append("&rs:Command=Render&rc:Parameters=False")

        Return sReportURL
    End Function

End Class
