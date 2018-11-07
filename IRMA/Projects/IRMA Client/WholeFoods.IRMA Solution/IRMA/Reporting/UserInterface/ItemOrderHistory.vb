Option Strict Off
Option Explicit On
Friend Class ItemOrderHistory
    Inherits System.Windows.Forms.Form
    Private m_bLockIdentifier As Boolean

    Private Sub frmItemOrderHist_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        '-- Load the combo boxes
        LoadStore(cboStore)

        If glStore_Limit > 0 Then
            SetActive(cboStore, False)
            SetCombo(cboStore, glStore_Limit)
        Else
            cboStore.SelectedIndex = -1
        End If


        LoadVendors(cboVendor)
        cboVendor.SelectedIndex = -1

        dtpStartDate.Value = DateAdd(DateInterval.Day, -7, SystemDateTime(True))
        '08/11/2009 - chip.vimond@wholefoods.com - Bug 10749 #1 fix
        '   Replaced -1 in DateAdd function with 0 to return today instead of yesterday. 
        '   Note that this now defaults report range to 8 days.
        dtpEndDate.Value = DateAdd(DateInterval.Day, 0, SystemDateTime(True))
    End Sub

    Private Sub cboStore_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cboStore.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        If KeyAscii = 8 Then
            Me.cboStore.SelectedIndex = -1
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cboVendor_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cboVendor.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        If KeyAscii = 8 Then
            Me.cboVendor.SelectedIndex = -1
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        ' make sure identifier is not empty
        If Trim(txtIdentifier.Text) = "" Then
            MsgBox("Identifier can not be empty.")
            Exit Sub
        End If

        'if either start date has a value or end date has a value then make sure they both have a value
        Dim bHasDate As Boolean

        If dtpStartDate.Checked AndAlso dtpEndDate.Checked Then
            If dtpEndDate.Value < dtpStartDate.Value Then
                MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
            bHasDate = True
        ElseIf dtpStartDate.Checked Then
            MsgBox("End Date is required if Start Date is checked.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, Me.Text)
            dtpEndDate.Focus()
            Exit Sub
        ElseIf dtpEndDate.Checked Then
            MsgBox("Start Date is required if End Date is checked.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, Me.Text)
            dtpEndDate.Focus()
            Exit Sub
        Else
            bHasDate = False
        End If

        Dim sReportURL As New System.Text.StringBuilder
        Dim filename As String = "ItemOrderHistoryReport"

        sReportURL.Append(filename)
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")
        sReportURL.Append("&ItemIdentifier=" & txtIdentifier.Text)

        If Me.cboStore.SelectedIndex <> -1 Then
            sReportURL.Append("&Store_No=" & VB6.GetItemData(cboStore, cboStore.SelectedIndex))
        Else
            sReportURL.Append("&Store_No:isnull=true")
        End If

        If cboVendor.SelectedIndex <> -1 Then
            sReportURL.Append("&Vendor_ID=" & VB6.GetItemData(cboVendor, cboVendor.SelectedIndex))
        Else
            sReportURL.Append("&Vendor_ID:isnull=true")
        End If

        sReportURL.Append("&TopN=" & NumericUpDown1.Value.ToString)

        If bHasDate Then
            sReportURL.Append("&StartDate=" & Format(dtpStartDate.Value, "M/dd/yyyy"))
            sReportURL.Append("&EndDate=" & Format(dtpEndDate.Value, "M/dd/yyyy"))
        Else
            sReportURL.Append("&StartDate:isnull=true")
            sReportURL.Append("&EndDate:isnull=true")
        End If

        If NumericUpDown1.Value = 0 Then
            sReportURL.Append("&ReportTitle=All Orders")
        Else
            sReportURL.Append("&ReportTitle=" & "Last " & NumericUpDown1.Value.ToString & " Orders")
        End If

        Call ReportingServicesReport(sReportURL.ToString)
    End Sub

    Public Property Identifier() As String
        Get
            Identifier = txtIdentifier.Text
        End Get
        Set(ByVal Value As String)
            txtIdentifier.Text = Value
        End Set
    End Property

    Public Property LockIdentifier() As Boolean
        Get
            LockIdentifier = m_bLockIdentifier
        End Get
        Set(ByVal Value As Boolean)
            m_bLockIdentifier = Value
            SetActive(txtIdentifier, Not (Value))
        End Set
    End Property

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

End Class