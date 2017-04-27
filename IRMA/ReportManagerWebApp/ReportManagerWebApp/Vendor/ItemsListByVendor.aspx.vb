
Partial Class Item_ItemsListByVendor
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete

        radSelectBy.ClearSelection()
        radSelectBy.SelectedIndex = 0
        Call ChangeSelectedOption()

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        oListItemAll = Nothing
        oListItemDefault = Nothing

    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder




        'report name
        sReportURL.Append(Application.Get("region") + "_" + "ItemsByVendor")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        'report parameters
        Select Case radSelectBy.SelectedIndex
            Case 0 : sReportURL.Append("&IsRegional=True")
            Case 1 : sReportURL.Append("&IsRegional=False&Zone_ID=" & cmbSelection.SelectedValue)
            Case 2 : sReportURL.Append("&IsRegional=False&Store_No_List=" & cmbSelection.SelectedValue)
            Case Else
                '??????????
                MsgBox("Unknown 'Select By' button index!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "")
        End Select

        If cmbVendor.SelectedValue <> 0 Then
            sReportURL.Append("&Vendor_ID=" & cmbVendor.SelectedValue)
        Else
            sReportURL.Append("&Vendor_ID:isnull=true")
        End If

        If cmbTeam.SelectedValue <> 0 Then
            sReportURL.Append("&Team_No=" & cmbTeam.SelectedValue)
        Else
            sReportURL.Append("&Team_No:isnull=true")
        End If

        If cmbSubTeam.SelectedValue <> 0 Then
            sReportURL.Append("&SubTeam_No=" & cmbSubTeam.SelectedValue)
        Else
            sReportURL.Append("&SubTeam_No:isnull=true")
        End If

        If cmbCategory.SelectedValue <> 0 Then
            sReportURL.Append("&Category_ID=" & cmbCategory.SelectedValue)
        Else
            sReportURL.Append("&Category_ID:isnull=true")
        End If

        If cmbBrand.SelectedValue <> 0 Then
            sReportURL.Append("&Brand_ID=" & cmbBrand.SelectedValue)
        Else
            sReportURL.Append("&Brand_ID:isnull=true")
        End If

        If txtIdentifier.Text.Trim.Length <> 0 Then
            sReportURL.Append("&Identifier=" & txtIdentifier.Text.Trim.ToString)
        Else
            sReportURL.Append("&Identifier:isnull=true")
        End If

        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())

    End Sub

    Protected Sub radSelectBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radSelectBy.SelectedIndexChanged

        Call ChangeSelectedOption()

    End Sub

    Protected Sub cmbCategory_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCategory.DataBound

        'add the default item
        cmbCategory.Items.Insert(0, oListItemAll)

    End Sub

    Protected Sub cmbSelection_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSelection.DataBound

        'add the default item
        If radSelectBy.SelectedIndex <> 0 Then
            cmbSelection.Items.Insert(0, oListItemDefault)
        End If

    End Sub

    Protected Sub cmbSubteam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound

        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemAll)

    End Sub

    Protected Sub cmbTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTeam.DataBound

        'add the default item
        cmbTeam.Items.Insert(0, oListItemAll)

    End Sub

    Protected Sub cmbVendor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbVendor.DataBound

        'add the default item
        cmbVendor.Items.Insert(0, oListItemAll)

    End Sub

    Protected Sub cmbBrand_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbBrand.DataBound

        'add the default item
        cmbBrand.Items.Insert(0, oListItemAll)

    End Sub

    Protected Sub ChangeSelectedOption()

        Select Case radSelectBy.SelectedIndex
            Case 0  'Region
                cmbSelection.DataSourceID = ICRegions.ID
                cmbSelection.DataTextField = "RegionName"
                cmbSelection.DataValueField = "Region_ID"

            Case 1  'Zone
                cmbSelection.DataSourceID = ICZones.ID
                cmbSelection.DataTextField = "Zone_Name"
                cmbSelection.DataValueField = "Zone_ID"

            Case 2  'Store
                cmbSelection.DataSourceID = ICStores.ID
                cmbSelection.DataTextField = "Store_Name"
                cmbSelection.DataValueField = "Store_No"

        End Select

        lblSelectionType.Text = radSelectBy.SelectedValue

    End Sub

End Class
