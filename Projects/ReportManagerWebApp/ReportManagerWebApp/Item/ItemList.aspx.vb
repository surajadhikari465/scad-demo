
Partial Class Item_ItemList
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete

        radSelectBy.ClearSelection()
        radSelectBy.SelectedIndex = 0
        Call ChangeSelectedOption()
        'lblNatItems.Visible = False

    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder

        'report name
        sReportURL.Append(Application.Get("region") + "_" + "ItemList")
        'sReportURL.Append("ItemList")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        'report parameters
        If cmbStore.SelectedValue <> 0 Then
            sReportURL.Append("&StoreNo=" & cmbStore.SelectedValue)
        End If


        If cmbSubTeam.SelectedValue <> 0 Then
            sReportURL.Append("&SubTeamNo=" & cmbSubTeam.SelectedValue)
        End If


        Select Case radSelectBy.SelectedIndex
            Case 0
                If txtVendor.Text.Trim.Length <> 0 Then
                    sReportURL.Append("&Vendor=" & txtVendor.Text.Trim)
                End If
            Case 1
                If cmbVendor.SelectedValue <> 0 Then
                    sReportURL.Append("&Vendor=" & cmbVendor.SelectedItem.Text)
                End If
            Case Else
                '??????????
                MsgBox("Unknown 'Vendor Match' button index!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "")
        End Select

        If txtItemDescription.Text.Trim.Length <> 0 Then
            sReportURL.Append("&ItemDesc=" & txtItemDescription.Text.Trim)
        End If
        If txtIdentifier.Text.Trim.Length <> 0 Then
            sReportURL.Append("&Identifier=" & txtIdentifier.Text.Trim)
        End If
        If txtVendorItemID.Text.Trim.Length <> 0 Then
            sReportURL.Append("&ItemID=" & txtVendorItemID.Text.Trim)
        End If



        If chkIncludeDiscontinued.Checked = True Then
            sReportURL.Append("&IncludeDiscontinuedItems=" & chkIncludeDiscontinued.Checked.ToString)
        End If

        If chkWFMOnly.Checked = True Then
            sReportURL.Append("&WFMItemsOnly=True")
        End If

        If chkNatItems.Checked = True Then
            sReportURL.Append("&NationalItems=True")
        End If

        'If cmbTeam.SelectedValue <> 0 Then
        '    sReportURL.Append("&TeamNo=" & cmbTeam.SelectedValue)
        'End If

        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())

    End Sub

    Protected Sub radSelectBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radSelectBy.SelectedIndexChanged

        Call ChangeSelectedOption()

    End Sub

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound

        'add the default item
        cmbStore.Items.Insert(0, oListItemDefault)

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

    Protected Sub ChangeSelectedOption()

        Select Case radSelectBy.SelectedIndex
            Case 0  'Partial
                lblSelectionType.Text = "Vendor LIKE"
                txtVendor.Visible = True
                cmbVendor.Visible = False

            Case 1  'Exact
                lblSelectionType.Text = "Vendor"
                txtVendor.Visible = False
                cmbVendor.Visible = True

        End Select


    End Sub

    'Protected Sub chkNatItems_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkNatItems.CheckedChanged
    '    If chkNatItems.Checked = True Then
    '        radSelectBy.Enabled = False
    '        txtVendor.Enabled = False
    '        cmbVendor.Enabled = False
    '        txtVendorItemID.Enabled = False
    '    Else
    '        radSelectBy.Enabled = True
    '        txtVendor.Enabled = True
    '        cmbVendor.Enabled = True
    '        txtVendorItemID.Enabled = True
    '    End If

    'End Sub
End Class
