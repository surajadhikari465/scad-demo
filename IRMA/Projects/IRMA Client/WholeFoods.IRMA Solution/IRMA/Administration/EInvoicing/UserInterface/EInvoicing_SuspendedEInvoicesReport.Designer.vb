<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EInvoicing_SuspendedEInvoicesReport
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.UltraDate_StartDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me.UltraDate_EndDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me.ComboBox_Stores = New System.Windows.Forms.ComboBox
        Me.ComboBox_Vendors = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.LinkLabel_ClearStartDate = New System.Windows.Forms.LinkLabel
        Me.LinkLabel_ClearEndDate = New System.Windows.Forms.LinkLabel
        Me.LinkLabel_ClearStore = New System.Windows.Forms.LinkLabel
        Me.LinkLabel_ClearVendor = New System.Windows.Forms.LinkLabel
        Me.LinkLabel_ClearAll = New System.Windows.Forms.LinkLabel
        Me.Button_RunReport = New System.Windows.Forms.Button
        CType(Me.UltraDate_StartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraDate_EndDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'UltraDate_StartDate
        '
        Me.UltraDate_StartDate.Location = New System.Drawing.Point(127, 47)
        Me.UltraDate_StartDate.Name = "UltraDate_StartDate"
        Me.UltraDate_StartDate.Size = New System.Drawing.Size(200, 21)
        Me.UltraDate_StartDate.TabIndex = 0
        '
        'UltraDate_EndDate
        '
        Me.UltraDate_EndDate.Location = New System.Drawing.Point(127, 74)
        Me.UltraDate_EndDate.Name = "UltraDate_EndDate"
        Me.UltraDate_EndDate.Size = New System.Drawing.Size(200, 21)
        Me.UltraDate_EndDate.TabIndex = 1
        '
        'ComboBox_Stores
        '
        Me.ComboBox_Stores.FormattingEnabled = True
        Me.ComboBox_Stores.Location = New System.Drawing.Point(127, 101)
        Me.ComboBox_Stores.Name = "ComboBox_Stores"
        Me.ComboBox_Stores.Size = New System.Drawing.Size(199, 21)
        Me.ComboBox_Stores.TabIndex = 2
        '
        'ComboBox_Vendors
        '
        Me.ComboBox_Vendors.FormattingEnabled = True
        Me.ComboBox_Vendors.Location = New System.Drawing.Point(127, 128)
        Me.ComboBox_Vendors.Name = "ComboBox_Vendors"
        Me.ComboBox_Vendors.Size = New System.Drawing.Size(199, 21)
        Me.ComboBox_Vendors.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(40, 49)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 16)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Start Date:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(45, 76)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 16)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "End Date:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(72, 102)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(49, 16)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Store:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(59, 129)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 16)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Vendor:"
        '
        'LinkLabel_ClearStartDate
        '
        Me.LinkLabel_ClearStartDate.AutoSize = True
        Me.LinkLabel_ClearStartDate.Location = New System.Drawing.Point(332, 51)
        Me.LinkLabel_ClearStartDate.Name = "LinkLabel_ClearStartDate"
        Me.LinkLabel_ClearStartDate.Size = New System.Drawing.Size(31, 13)
        Me.LinkLabel_ClearStartDate.TabIndex = 8
        Me.LinkLabel_ClearStartDate.TabStop = True
        Me.LinkLabel_ClearStartDate.Text = "Clear"
        '
        'LinkLabel_ClearEndDate
        '
        Me.LinkLabel_ClearEndDate.AutoSize = True
        Me.LinkLabel_ClearEndDate.Location = New System.Drawing.Point(332, 78)
        Me.LinkLabel_ClearEndDate.Name = "LinkLabel_ClearEndDate"
        Me.LinkLabel_ClearEndDate.Size = New System.Drawing.Size(31, 13)
        Me.LinkLabel_ClearEndDate.TabIndex = 9
        Me.LinkLabel_ClearEndDate.TabStop = True
        Me.LinkLabel_ClearEndDate.Text = "Clear"
        '
        'LinkLabel_ClearStore
        '
        Me.LinkLabel_ClearStore.AutoSize = True
        Me.LinkLabel_ClearStore.Location = New System.Drawing.Point(332, 104)
        Me.LinkLabel_ClearStore.Name = "LinkLabel_ClearStore"
        Me.LinkLabel_ClearStore.Size = New System.Drawing.Size(31, 13)
        Me.LinkLabel_ClearStore.TabIndex = 10
        Me.LinkLabel_ClearStore.TabStop = True
        Me.LinkLabel_ClearStore.Text = "Clear"
        '
        'LinkLabel_ClearVendor
        '
        Me.LinkLabel_ClearVendor.AutoSize = True
        Me.LinkLabel_ClearVendor.Location = New System.Drawing.Point(332, 131)
        Me.LinkLabel_ClearVendor.Name = "LinkLabel_ClearVendor"
        Me.LinkLabel_ClearVendor.Size = New System.Drawing.Size(31, 13)
        Me.LinkLabel_ClearVendor.TabIndex = 11
        Me.LinkLabel_ClearVendor.TabStop = True
        Me.LinkLabel_ClearVendor.Text = "Clear"
        '
        'LinkLabel_ClearAll
        '
        Me.LinkLabel_ClearAll.AutoSize = True
        Me.LinkLabel_ClearAll.Location = New System.Drawing.Point(332, 167)
        Me.LinkLabel_ClearAll.Name = "LinkLabel_ClearAll"
        Me.LinkLabel_ClearAll.Size = New System.Drawing.Size(45, 13)
        Me.LinkLabel_ClearAll.TabIndex = 12
        Me.LinkLabel_ClearAll.TabStop = True
        Me.LinkLabel_ClearAll.Text = "Clear All"
        '
        'Button_RunReport
        '
        Me.Button_RunReport.Location = New System.Drawing.Point(127, 162)
        Me.Button_RunReport.Name = "Button_RunReport"
        Me.Button_RunReport.Size = New System.Drawing.Size(199, 22)
        Me.Button_RunReport.TabIndex = 13
        Me.Button_RunReport.Text = "Run Report"
        Me.Button_RunReport.UseVisualStyleBackColor = True
        '
        'EInvoicing_SuspendedEInvoicesReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(417, 230)
        Me.Controls.Add(Me.Button_RunReport)
        Me.Controls.Add(Me.LinkLabel_ClearAll)
        Me.Controls.Add(Me.LinkLabel_ClearVendor)
        Me.Controls.Add(Me.LinkLabel_ClearStore)
        Me.Controls.Add(Me.LinkLabel_ClearEndDate)
        Me.Controls.Add(Me.LinkLabel_ClearStartDate)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboBox_Vendors)
        Me.Controls.Add(Me.ComboBox_Stores)
        Me.Controls.Add(Me.UltraDate_EndDate)
        Me.Controls.Add(Me.UltraDate_StartDate)
        Me.Name = "EInvoicing_SuspendedEInvoicesReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Suspended EInvoices Report"
        CType(Me.UltraDate_StartDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraDate_EndDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UltraDate_StartDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraDate_EndDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents ComboBox_Stores As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox_Vendors As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents LinkLabel_ClearStartDate As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel_ClearEndDate As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel_ClearStore As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel_ClearVendor As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel_ClearAll As System.Windows.Forms.LinkLabel
    Friend WithEvents Button_RunReport As System.Windows.Forms.Button
End Class
