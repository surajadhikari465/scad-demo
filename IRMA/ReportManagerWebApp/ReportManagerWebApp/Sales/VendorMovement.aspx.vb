Imports System.Globalization

Partial Class Sales_StoreVendorMovement
    Inherits System.Web.UI.Page

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        If Page.IsValid Then
            Dim sReportURL As New System.Text.StringBuilder


            Dim beginDate, endDate As DateTime
            Dim marketCulture, usCulture As CultureInfo
            usCulture = New CultureInfo("en-US")
            marketCulture = System.Threading.Thread.CurrentThread.CurrentCulture
            beginDate = DateTime.ParseExact(dteBegin.Text, "d", marketCulture)
            endDate = DateTime.ParseExact(dteEnd.Text, "d", marketCulture)



            'report name
            sReportURL.Append(Application.Get("region") + "_" + "StoreVendorMovement")

            'report display
            If cmbReportFormat.SelectedValue <> "HTML" Then
                sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
            End If

            sReportURL.Append("&rs:Command=Render")
            sReportURL.Append("&rc:Parameters=False")

            sReportURL.Append("&BeginDate=" & beginDate.ToString("d", usCulture))
            sReportURL.Append("&EndDate=" & endDate.ToString("d", usCulture))
            sReportURL.Append("&Top=" & txtResults.Text)
            sReportURL.Append("&Zone_ID=" & cmbZone.SelectedValue)

            If cmbStore.SelectedValue = "ALL" Then
                sReportURL.Append("&Store_No:isnull=true")
            Else
                sReportURL.Append("&Store_No=" & cmbStore.SelectedValue)
            End If

            If cmbSubteam.SelectedValue = "ALL" Then
                sReportURL.Append("&SubTeam_No:isnull=true")
            Else
                sReportURL.Append("&SubTeam_No=" & cmbSubteam.SelectedValue)
            End If

            sReportURL.Append("&Category_ID:isnull=true")
            sReportURL.Append("&FamilyCode:isnull=true")

            If lstVendors.SelectedValue = "" Then
                sReportURL.Append("&Vendor_ID:isnull=true")
            Else
                sReportURL.Append("&Vendor_ID=" & lstVendors.SelectedValue)
            End If

            sReportURL.Append("&ReverseOrder=" & IIf(rdbtnBottom.Checked, 1, 0))

            Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
        End If
    End Sub

    Protected Sub btnGetVendors_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetVendors.Click
        ' Clear out anything in the current search list
        lstVendors.ClearSelection()

        ' note write something to clear list if rdbtn selection changes.
        If rdbtn_SearchType.SelectedValue = 1 Then
            lstVendors.DataSourceID() = "ICVendors_CompanyName"
        ElseIf rdbtn_SearchType.SelectedValue = 2 Then
            lstVendors.DataSourceID() = "ICVendors_PSVendorID"
        ElseIf rdbtn_SearchType.SelectedValue = 3 Then
            lstVendors.DataSourceID() = "ICVendors_VendorID"
        End If

        lstVendors.DataTextField() = "CompanyName"
        lstVendors.DataValueField() = "Vendor_ID"

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            cmbStore.Items.Add("ALL")
            cmbStore.AppendDataBoundItems = True
            cmbStore.DataSourceID = "ICStores"
            cmbStore.DataTextField = "Store_Name"
            cmbStore.DataValueField = "Store_No"

            cmbSubteam.Items.Add("ALL")
            cmbSubteam.AppendDataBoundItems = True
            cmbSubteam.DataSourceID = "ICSubteams"
            cmbSubteam.DataTextField = "SubTeam_Name"
            cmbSubteam.DataValueField = "SubTeam_No"

            Dim market As String = Application.Get("market")
            If market.Equals("UK") Then
                System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB")
            End If

        End If
    End Sub

    Protected Sub ICVendors_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles ICVendors_CompanyName.Selected, ICVendors_PSVendorID.Selected, ICVendors_VendorID.Selected
        'lblVendorCount.Text = e.AffectedRows.ToString()

        If e.AffectedRows > 0 Then  ' if results are returned display nothing
            lblVendorCount.Text = ""
        Else ' if NO results are returned display a message
            lblVendorCount.Text = "No results found."
        End If

    End Sub
End Class
