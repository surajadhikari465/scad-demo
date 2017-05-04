<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ReprintShelfTagsForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.btnCheckStoreFTP = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnCheckReprintRequests = New System.Windows.Forms.Button
        Me.lblInfo = New System.Windows.Forms.Label
        Me.pbrProgress = New System.Windows.Forms.ProgressBar
        Me.lblStore = New System.Windows.Forms.Label
        Me.rtbOutputText = New System.Windows.Forms.RichTextBox
        Me.lstStores = New System.Windows.Forms.ListBox
        Me.btnSelectAll = New System.Windows.Forms.Button
        Me.btnSelectNone = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnStopProcessing = New System.Windows.Forms.Button
        Me.btnEmailLog = New System.Windows.Forms.Button
        Me.dtpDateRangeEnd = New System.Windows.Forms.DateTimePicker
        Me.lblDateRangeEnd = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnCheckStoreFTP
        '
        Me.btnCheckStoreFTP.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCheckStoreFTP.Location = New System.Drawing.Point(276, 341)
        Me.btnCheckStoreFTP.Name = "btnCheckStoreFTP"
        Me.btnCheckStoreFTP.Size = New System.Drawing.Size(76, 23)
        Me.btnCheckStoreFTP.TabIndex = 3
        Me.btnCheckStoreFTP.Text = "Check FTP"
        Me.ToolTip1.SetToolTip(Me.btnCheckStoreFTP, "Start tlog import process for selected stores")
        Me.btnCheckStoreFTP.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExit.Location = New System.Drawing.Point(441, 341)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(75, 23)
        Me.btnExit.TabIndex = 5
        Me.btnExit.Text = "E&xit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnCheckReprintRequests
        '
        Me.btnCheckReprintRequests.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCheckReprintRequests.Location = New System.Drawing.Point(195, 341)
        Me.btnCheckReprintRequests.Name = "btnCheckReprintRequests"
        Me.btnCheckReprintRequests.Size = New System.Drawing.Size(76, 23)
        Me.btnCheckReprintRequests.TabIndex = 4
        Me.btnCheckReprintRequests.Text = "Reprint Tags"
        Me.ToolTip1.SetToolTip(Me.btnCheckReprintRequests, "Check if tlogs have been imported for selected stores")
        Me.btnCheckReprintRequests.UseVisualStyleBackColor = True
        '
        'lblInfo
        '
        Me.lblInfo.AutoSize = True
        Me.lblInfo.Location = New System.Drawing.Point(199, 17)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(0, 13)
        Me.lblInfo.TabIndex = 4
        '
        'pbrProgress
        '
        Me.pbrProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbrProgress.Location = New System.Drawing.Point(195, 325)
        Me.pbrProgress.Name = "pbrProgress"
        Me.pbrProgress.Size = New System.Drawing.Size(321, 10)
        Me.pbrProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.pbrProgress.TabIndex = 5
        '
        'lblStore
        '
        Me.lblStore.AutoSize = True
        Me.lblStore.Location = New System.Drawing.Point(9, 12)
        Me.lblStore.Name = "lblStore"
        Me.lblStore.Size = New System.Drawing.Size(43, 13)
        Me.lblStore.TabIndex = 7
        Me.lblStore.Text = "Store(s)"
        '
        'rtbOutputText
        '
        Me.rtbOutputText.AcceptsTab = True
        Me.rtbOutputText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbOutputText.AutoWordSelection = True
        Me.rtbOutputText.Location = New System.Drawing.Point(195, 41)
        Me.rtbOutputText.Name = "rtbOutputText"
        Me.rtbOutputText.ReadOnly = True
        Me.rtbOutputText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
        Me.rtbOutputText.Size = New System.Drawing.Size(321, 278)
        Me.rtbOutputText.TabIndex = 7
        Me.rtbOutputText.Text = ""
        '
        'lstStores
        '
        Me.lstStores.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstStores.FormattingEnabled = True
        Me.lstStores.Location = New System.Drawing.Point(12, 36)
        Me.lstStores.Name = "lstStores"
        Me.lstStores.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstStores.Size = New System.Drawing.Size(168, 329)
        Me.lstStores.TabIndex = 0
        '
        'btnSelectAll
        '
        Me.btnSelectAll.Location = New System.Drawing.Point(58, 7)
        Me.btnSelectAll.Name = "btnSelectAll"
        Me.btnSelectAll.Size = New System.Drawing.Size(58, 23)
        Me.btnSelectAll.TabIndex = 1
        Me.btnSelectAll.Text = "All"
        Me.ToolTip1.SetToolTip(Me.btnSelectAll, "Select all stores")
        Me.btnSelectAll.UseVisualStyleBackColor = True
        '
        'btnSelectNone
        '
        Me.btnSelectNone.Location = New System.Drawing.Point(122, 7)
        Me.btnSelectNone.Name = "btnSelectNone"
        Me.btnSelectNone.Size = New System.Drawing.Size(58, 23)
        Me.btnSelectNone.TabIndex = 2
        Me.btnSelectNone.Text = "None"
        Me.ToolTip1.SetToolTip(Me.btnSelectNone, "Clear selected stores")
        Me.btnSelectNone.UseVisualStyleBackColor = True
        '
        'btnStopProcessing
        '
        Me.btnStopProcessing.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnStopProcessing.Location = New System.Drawing.Point(357, 341)
        Me.btnStopProcessing.Name = "btnStopProcessing"
        Me.btnStopProcessing.Size = New System.Drawing.Size(76, 23)
        Me.btnStopProcessing.TabIndex = 6
        Me.btnStopProcessing.Text = "Stop Job"
        Me.ToolTip1.SetToolTip(Me.btnStopProcessing, "Stop current process")
        Me.btnStopProcessing.UseVisualStyleBackColor = True
        '
        'btnEmailLog
        '
        Me.btnEmailLog.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEmailLog.Location = New System.Drawing.Point(441, 12)
        Me.btnEmailLog.Name = "btnEmailLog"
        Me.btnEmailLog.Size = New System.Drawing.Size(75, 23)
        Me.btnEmailLog.TabIndex = 8
        Me.btnEmailLog.Text = "Email Log"
        Me.ToolTip1.SetToolTip(Me.btnEmailLog, "Send log via e-mail")
        Me.btnEmailLog.UseVisualStyleBackColor = True
        Me.btnEmailLog.Visible = False
        '
        'dtpDateRangeEnd
        '
        Me.dtpDateRangeEnd.CustomFormat = "dddd dd-MMM-yyyy"
        Me.dtpDateRangeEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpDateRangeEnd.Location = New System.Drawing.Point(3, 91)
        Me.dtpDateRangeEnd.MaxDate = New Date(2020, 12, 31, 0, 0, 0, 0)
        Me.dtpDateRangeEnd.MinDate = New Date(2001, 1, 1, 0, 0, 0, 0)
        Me.dtpDateRangeEnd.Name = "dtpDateRangeEnd"
        Me.dtpDateRangeEnd.Size = New System.Drawing.Size(168, 20)
        Me.dtpDateRangeEnd.TabIndex = 11
        Me.ToolTip1.SetToolTip(Me.dtpDateRangeEnd, "Date to Parse")
        Me.dtpDateRangeEnd.Visible = False
        '
        'lblDateRangeEnd
        '
        Me.lblDateRangeEnd.AutoSize = True
        Me.lblDateRangeEnd.Location = New System.Drawing.Point(0, 75)
        Me.lblDateRangeEnd.Name = "lblDateRangeEnd"
        Me.lblDateRangeEnd.Size = New System.Drawing.Size(92, 13)
        Me.lblDateRangeEnd.TabIndex = 12
        Me.lblDateRangeEnd.Text = "End of date range"
        Me.lblDateRangeEnd.Visible = False
        '
        'ReprintShelfTagsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(528, 372)
        Me.Controls.Add(Me.btnCheckStoreFTP)
        Me.Controls.Add(Me.btnEmailLog)
        Me.Controls.Add(Me.btnStopProcessing)
        Me.Controls.Add(Me.btnSelectNone)
        Me.Controls.Add(Me.btnSelectAll)
        Me.Controls.Add(Me.lstStores)
        Me.Controls.Add(Me.rtbOutputText)
        Me.Controls.Add(Me.lblStore)
        Me.Controls.Add(Me.pbrProgress)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.btnCheckReprintRequests)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.lblDateRangeEnd)
        Me.Controls.Add(Me.dtpDateRangeEnd)
        Me.Name = "ReprintShelfTagsForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ReprintShelfTags"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCheckStoreFTP As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnCheckReprintRequests As System.Windows.Forms.Button
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents pbrProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents lblStore As System.Windows.Forms.Label
    Friend WithEvents rtbOutputText As System.Windows.Forms.RichTextBox
    Friend WithEvents lstStores As System.Windows.Forms.ListBox
    Friend WithEvents btnSelectAll As System.Windows.Forms.Button
    Friend WithEvents btnSelectNone As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnStopProcessing As System.Windows.Forms.Button
    Friend WithEvents btnEmailLog As System.Windows.Forms.Button
    Friend WithEvents dtpDateRangeEnd As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblDateRangeEnd As System.Windows.Forms.Label
End Class
