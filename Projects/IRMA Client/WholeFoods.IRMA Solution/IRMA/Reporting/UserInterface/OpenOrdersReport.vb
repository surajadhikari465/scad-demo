Imports System.Text
Imports WholeFoods.Utility

Friend Class frmOpenOrdersReport
    Inherits System.Windows.Forms.Form

    Private Sub frmOpenOrdersReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Dim dtCurrentDate As Date = SystemDateTime()

        '-- Load the combo boxes
        LoadCustomer(cmbStore)
        LoadAllSubTeams(cmbSubTeam)
        LoadVendors(cmbVendor)

        LoadUsers(cmbUser)
        cmbUser.Text = gsUserName

        dtpOrderStartDate.Value = dtCurrentDate
        dtpOrderStartDate.ShowCheckBox = True
        dtpOrderStartDate.Checked = False

        dtpOrderEndDate.Value = dtCurrentDate
        dtpOrderEndDate.ShowCheckBox = True
        dtpOrderEndDate.Checked = False

        dtpExpectedStartDate.Value = dtCurrentDate
        dtpExpectedStartDate.ShowCheckBox = True
        dtpExpectedStartDate.Checked = False

        dtpExpectedEndDate.Value = dtCurrentDate
        dtpExpectedEndDate.ShowCheckBox = True
        dtpExpectedEndDate.Checked = False
    End Sub

#Region "Keypress Event Handlers"

    Private Sub cmbStore_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbStore.KeyPress
        Dim KeyAscii As Short = CShort(Asc(eventArgs.KeyChar))

        If KeyAscii = 8 Then cmbStore.SelectedIndex = -1

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cmbSubTeam_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubTeam.KeyPress
        Dim KeyAscii As Short = CShort(Asc(eventArgs.KeyChar))

        If KeyAscii = 8 Then cmbSubTeam.SelectedIndex = -1

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cmbUser_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbUser.KeyPress
        Dim KeyAscii As Short = CShort(Asc(eventArgs.KeyChar))

        If KeyAscii = 8 Then cmbUser.SelectedIndex = -1

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cmbVendor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbVendor.KeyPress
        Dim KeyAscii As Short = CShort(Asc(e.KeyChar))

        If KeyAscii = 8 Then cmbVendor.SelectedIndex = -1

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
    End Sub
#End Region

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        Dim reportUrlBuilder As StringBuilder

        If dtpExpectedStartDate.Checked AndAlso dtpExpectedEndDate.Checked Then
            If dtpExpectedEndDate.Value < dtpExpectedStartDate.Value Then
                MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Exclamation, Me.Text)
                dtpExpectedEndDate.Focus()
                Exit Sub
            End If
        End If

        If dtpOrderStartDate.Checked AndAlso dtpOrderEndDate.Checked Then
            If dtpOrderEndDate.Value < dtpOrderStartDate.Value Then
                MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Exclamation, Me.Text)
                dtpOrderEndDate.Focus()
                Exit Sub
            End If
        End If

        reportUrlBuilder = New StringBuilder()

        ' [Tom Lux, 2/23/09] Making standard and detailed 'Open Orders' reports generic/global: removing region abbr from report name.
        reportUrlBuilder.AppendFormat("{0}&rs:Command=Render&rc:Parameters=False", _
            IIf(chkDetail.Checked, "OpenOrdersDetailReport", "OpenOrdersReport"))

        If cmbStore.SelectedIndex > -1 Then
            reportUrlBuilder.AppendFormat("&ReceiveLocation_ID={0}", VB6.GetItemData(cmbStore, cmbStore.SelectedIndex))
        End If

        If cmbSubTeam.SelectedIndex > -1 Then
            reportUrlBuilder.AppendFormat("&SubTeam_No={0}", VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
        End If

        If cmbVendor.SelectedIndex > -1 Then
            reportUrlBuilder.AppendFormat("&Vendor_ID={0}", VB6.GetItemData(cmbVendor, cmbVendor.SelectedIndex))
        End If

        If dtpExpectedStartDate.Checked Then
            reportUrlBuilder.AppendFormat("&Expected_DateStart={0}", VB6.Format(dtpExpectedStartDate.Value.ToString(), "YYYY-MM-DD"))
        End If

        If dtpExpectedEndDate.Checked Then
            reportUrlBuilder.AppendFormat("&Expected_DateEnd={0}", VB6.Format(dtpExpectedEndDate.Value.ToString(), "YYYY-MM-DD"))
        End If

        If dtpOrderStartDate.Checked Then
            reportUrlBuilder.AppendFormat("&OrderDateStart={0}", VB6.Format(dtpOrderStartDate.Value.ToString(), "YYYY-MM-DD"))
        End If

        If dtpOrderEndDate.Checked Then
            reportUrlBuilder.AppendFormat("&OrderDateEnd={0}", VB6.Format(dtpOrderEndDate.Value.ToString(), "YYYY-MM-DD"))
        End If

        If optRptType(0).Checked Then
            reportUrlBuilder.Append("&Return_Orders=1")
        ElseIf optRptType(1).Checked Then
            reportUrlBuilder.Append("&Return_Orders=0")
        End If

        ' [Tom Lux, 2/23/09] This param is defined as a boolean in the report, so it must be passed as True/False, not 0/1.
        If chkPreOrder.Checked Then
            reportUrlBuilder.Append("&Pre_Order=True")
        Else
            reportUrlBuilder.Append("&Pre_Order=False")
        End If

        If chkIncludeBlankPOs.Checked Then
            reportUrlBuilder.Append("&IncludeBlankPOs=True")
        Else
            reportUrlBuilder.Append("&IncludeBlankPOs=False")
        End If

        If cmbUser.SelectedIndex > -1 Then
            reportUrlBuilder.AppendFormat("&User_ID={0}", VB6.GetItemData(cmbUser, cmbUser.SelectedIndex))
        End If

        ReportingServicesReport(reportUrlBuilder.ToString())
    End Sub
End Class