Option Strict Off
Option Explicit On
 
Imports System.Web

Friend Class frmItemList
	Inherits System.Windows.Forms.Form

	Private Sub cmbSubTeam_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubTeam.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

		If KeyAscii = 8 Then cmbSubTeam.SelectedIndex = -1

		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

		'-- Unload search form
		Me.Close()

	End Sub

	Private Sub RunReport(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        Dim sReportURL As New System.Text.StringBuilder

        ' Store validation
        If cmbStore.SelectedIndex = -1 Then
            MsgBox("Store must be selected.", MsgBoxStyle.Exclamation, "Error")
            Exit Sub
        End If

        ' Setup Report URL
        sReportURL.Append(gsRegionCode)
        sReportURL.Append("_ItemList")

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        ' Add Report Parameters
        sReportURL.Append("&StoreNo=" & CStr(VB6.GetItemData(cmbStore, cmbStore.SelectedIndex)))

        If cmbSubTeam.SelectedIndex > -1 Then
			sReportURL.Append("&SubTeamNo=" & cmbSubTeam.SelectedItem.SubTeamNo.ToString())
		End If

        If Len(Trim(txtVendorName.Text)) <> 0 Then
            sReportURL.Append("&Vendor=" & HttpUtility.UrlEncode(Trim(txtVendorName.Text)))
        End If

        If Len(Trim(txtField(0).Text)) <> 0 Then
            sReportURL.Append("&ItemDesc=" & HttpUtility.UrlEncode(Trim(txtField(0).Text)))
        End If

        If Len(Trim(txtField(1).Text)) <> 0 Then
            sReportURL.Append("&Identifier=" & Trim(txtField(1).Text))
        End If

        If Len(Trim(txtField(5).Text)) <> 0 Then
            sReportURL.Append("&ItemID=" & Trim(txtField(5).Text))
        End If

        If chkDiscontinued.CheckState = CheckState.Checked Then
            sReportURL.Append("&IncludeDiscontinuedItems=" & CBool(chkDiscontinued.CheckState).ToString)
        End If

        If chkWFM.CheckState = CheckState.Checked Then
            sReportURL.Append("&WFMItemsOnly=" & CBool(chkWFM.CheckState).ToString)
        End If

        If chkNatItems.CheckState = CheckState.Checked Then
            sReportURL.Append("&NationalItems=" & CBool(chkNatItems.CheckState).ToString)
        End If

        ' Display Report
        Call ReportingServicesReport(sReportURL.ToString)

    End Sub

	Private Sub frmItemList_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

		'-- Center the form and the buttons on the form
		CenterForm(Me)

		cmbSubTeam.DataSource = WholeFoods.IRMA.ItemHosting.DataAccess.SubTeamDAO.GetSubteams
		'Also include the distribution centers in the store list TFS 6627
		LoadStore(cmbStore)

		If glStore_Limit > 0 Then
			SetActive(cmbStore, False)
			SetCombo(cmbStore, glStore_Limit)
		Else
			cmbStore.SelectedIndex = -1
		End If

		Call SetActive(txtVendorName, False)
		'hide the "Print Only" checkbox; unable to print directly in SQL Server Reporting Services
		chkPrintOnly.Visible = False

	End Sub

	Private Sub txtField_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.Enter
    CType(eventSender, TextBox).SelectAll()
  End Sub

  Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress
    Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
    Dim Index As Short = txtField.GetIndex(eventSender)

    KeyAscii = ValidateKeyPressEvent(KeyAscii, txtField(Index).Tag, txtField(Index), 0, 0, 0)

    eventArgs.KeyChar = Chr(KeyAscii)
    If KeyAscii = 0 Then
      eventArgs.Handled = True
    End If
  End Sub

  Private Sub chkNatItems_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkNatItems.Click
        If chkNatItems.CheckState = CheckState.Checked Then
            txtVendorName.Enabled = False
            txtField(5).Enabled = False
        Else
            txtVendorName.Enabled = True
            txtField(5).Enabled = True
        End If
    End Sub
    Private Sub cmdCompanySearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCompanySearch.Click

        glVendorID = 0
        '-- Set the search type
        giSearchType = iSearchAllVendors
        '-- Open the search form
        frmSearch.Text = "Search for Vendor by Company Name"
        frmSearch.ShowDialog()

        frmSearch.Close()
        frmSearch.Dispose()

        '-- if its not zero, then something was found
        If glVendorID <> 0 Then Call GetVendorName(glVendorID)

    End Sub

    Private Sub GetVendorName(ByRef lVendorID As Integer)

        Dim rsVendor As DAO.Recordset = Nothing
        Try
            rsVendor = SQLOpenRecordSet("EXEC GetVendorName " & glVendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Me.txtVendorName.Text = rsVendor.Fields("CompanyName").Value
        Finally
            If rsVendor IsNot Nothing Then
                rsVendor.Close()
                rsVendor = Nothing
            End If
        End Try

    End Sub
End Class
