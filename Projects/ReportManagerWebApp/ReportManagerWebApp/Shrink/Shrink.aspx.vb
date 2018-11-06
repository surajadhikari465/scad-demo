Imports System.Globalization

Partial Class Shrink_ShrinkReport
    Inherits System.Web.UI.Page

    'Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", "ALL")
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", "")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim market As String = Application.Get("market")
            If market.Equals("UK") Then
                System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB")
            End If
        End If
    End Sub

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete
        'set default report display format
        cmbReportFormat.SelectedValue = "HTML"
    End Sub

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound
        'add the default item
        cmbStore.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder

        Dim beginDate, endDate As DateTime
        Dim marketCulture, usCulture As CultureInfo
        marketCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        usCulture = New CultureInfo("en-US")
        beginDate = DateTime.ParseExact(dtStartDate.Text, "d", marketCulture)
        endDate = DateTime.ParseExact(dtEndDate.Text, "d", marketCulture)

        'report name
        sReportURL.Append("Shrink")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&Store_No=" & cmbStore.SelectedValue)

        Dim i As Integer
        Dim strSubTeamList As String

        strSubTeamList = String.Empty

        For i = 0 To listBoxSubTeams.Items.Count - 1
            If listBoxSubTeams.Items(i).Selected Then
                strSubTeamList = strSubTeamList & listBoxSubTeams.Items(i).Value & ","
            End If
        Next
        Dim loc As Integer
        loc = strSubTeamList.Length - 1

        Dim s As String = String.Empty
        If loc >= 0 Then
            s = strSubTeamList.Remove(loc)
        End If

        sReportURL.Append("&SubTeam_List=" & s)

        sReportURL.Append("&BeginDate=" & beginDate.ToString("yyyy-MM-dd"))
        sReportURL.Append("&EndDate=" & endDate.ToString("yyyy-MM-dd"))

        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

End Class
