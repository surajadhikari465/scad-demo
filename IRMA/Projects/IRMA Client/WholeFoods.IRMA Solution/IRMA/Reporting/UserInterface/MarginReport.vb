Option Strict Off
Option Explicit On
Imports WholeFoods.Utility
Friend Class frmMarginReport
    Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean
	
	Private Sub cmbField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbField.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		Dim Index As Short = cmbField.GetIndex(eventSender)
		
		If KeyAscii = 8 Then cmbField(Index).SelectedIndex = -1
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub cmdVendorSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdVendorSearch.Click
		
		'-- Set glvendorid to none found
		glVendorID = 0
		
		'-- Set the search type
		giSearchType = iSearchVendorCompany
		
		'-- Open the search form
		frmSearch.Text = "Search for Vendor by Company Name"
		frmSearch.ShowDialog()
        frmSearch.Dispose()
		
		'-- if its not zero, then something was found
		If glVendorID <> 0 Then
			txtVendor.Tag = glVendorID
            txtVendor.Text = ReturnVendorName(glVendorID)
		End If
		
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        Dim sReportURL As New System.Text.StringBuilder

        sReportURL.Append(ConfigurationServices.AppSettings("Region"))
        If optVendor.Checked Then  ' Open Margin By Vendor report
            'report name
            sReportURL.Append("_MarginByVendor")

        Else  ' Open Margin By Subteam report
            'report name
            sReportURL.Append("_MarginBySubTeam")
        End If

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        'store parameter
        If cmbField(0).SelectedIndex > -1 Then
            sReportURL.Append("&Store_No=" & VB6.GetItemData(cmbField(0), cmbField(0).SelectedIndex))
        Else
            sReportURL.Append("&Store_No:isnull=true")
        End If

        ' if vendor report add vendor parameter, if subteam report add subteam parameter
        If optVendor.Checked Then  ' Use Margin By Vendor report
            If CInt(txtVendor.Tag) = -1 Then
                MsgBox("Vendor not selected", MsgBoxStyle.Exclamation, "Error!")
                Exit Sub
            Else
                sReportURL.Append("&Vendor_ID=" & txtVendor.Tag)
            End If
        Else  ' Use Margin By Subteam report
            If cmbField(1).SelectedIndex > -1 Then
                sReportURL.Append("&SubTeam_No=" & VB6.GetItemData(cmbField(1), cmbField(1).SelectedIndex))
            Else
                MsgBox("Subteam not selected", MsgBoxStyle.Exclamation, "Error!")
                Exit Sub
            End If
        End If

        ' add the margin min and max limits
        If txtField(0).Text = "" Or txtField(1).Text = "" Then
            MsgBox("Margin limits must be entered.", MsgBoxStyle.Exclamation, "Error!")
            Exit Sub
        Else
            sReportURL.Append("&Minval=" & txtField(0).Text)
            sReportURL.Append("&Maxval=" & txtField(1).Text)
        End If

        ' add if searching in-range or out-of-range parameter
        sReportURL.Append("&Range=" & IIf(optInRange.Checked, "true", "false"))

        ' call the report
        Call ReportingServicesReport(sReportURL.ToString)

	End Sub
	
	Private Sub frmMarginReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		CenterForm(Me)
		
		LoadStore(cmbField(0))
        LoadAllSubTeams(cmbField(1))

        If glStore_Limit > 0 Then
            SetActive(cmbField(0), False)
            SetCombo(cmbField(0), glStore_Limit)
        Else
            cmbField(0).SelectedIndex = -1
        End If
		
		SetActive(txtVendor, False)
		optVendor_CheckedChanged(optVendor, New System.EventArgs())
	End Sub
	
    Private Sub optSubTeam_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optSubTeam.CheckedChanged
        If Me.IsInitializing = True Then Exit Sub

        If eventSender.Checked Then

            lblLabel(0).Visible = False
            txtVendor.Visible = False
            cmdVendorSearch.Visible = False
            lblLabel(5).Visible = True
            cmbField(1).Visible = True

        End If
    End Sub
	
    Private Sub optVendor_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optVendor.CheckedChanged
        If Me.IsInitializing = True Then Exit Sub

        If eventSender.Checked Then

            lblLabel(0).Visible = True
            txtVendor.Visible = True
            cmdVendorSearch.Visible = True
            lblLabel(5).Visible = False
            cmbField(1).Visible = False

        End If
    End Sub

  Private Sub txtField_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.Enter
    CType(eventSender, TextBox).SelectAll()
  End Sub

  Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress
    Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
    Dim Index As Short = txtField.GetIndex(eventSender)

    '-- Restrict key presses to that type of field
    KeyAscii = ValidateKeyPressEvent(KeyAscii, "NUMBER", txtField(Index), 0, 0, 0)

    eventArgs.KeyChar = Chr(KeyAscii)
    If KeyAscii = 0 Then
      eventArgs.Handled = True
    End If
  End Sub

  Private Sub _lblLabel_0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _lblLabel_0.Click

  End Sub
End Class