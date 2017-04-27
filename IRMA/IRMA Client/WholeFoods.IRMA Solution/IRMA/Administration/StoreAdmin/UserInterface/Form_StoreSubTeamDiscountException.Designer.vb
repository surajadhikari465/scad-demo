<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_StoreSubTeamDiscountException
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
        Me.Label_Store = New System.Windows.Forms.Label()
        Me.ComboBox_Stores = New System.Windows.Forms.ComboBox()
        Me.DataGridView_DiscountExceptions = New System.Windows.Forms.DataGridView()
        Me.Label_DiscountExceptions = New System.Windows.Forms.Label()
        Me.Button_Close = New System.Windows.Forms.Button()
        Me.Button_Save = New System.Windows.Forms.Button()
        CType(Me.DataGridView_DiscountExceptions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label_Store
        '
        Me.Label_Store.AutoSize = True
        Me.Label_Store.Location = New System.Drawing.Point(13, 13)
        Me.Label_Store.Name = "Label_Store"
        Me.Label_Store.Size = New System.Drawing.Size(32, 13)
        Me.Label_Store.TabIndex = 0
        Me.Label_Store.Text = "Store"
        '
        'ComboBox_Stores
        '
        Me.ComboBox_Stores.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_Stores.FormattingEnabled = True
        Me.ComboBox_Stores.Location = New System.Drawing.Point(51, 10)
        Me.ComboBox_Stores.Name = "ComboBox_Stores"
        Me.ComboBox_Stores.Size = New System.Drawing.Size(447, 21)
        Me.ComboBox_Stores.TabIndex = 1
        '
        'DataGridView_DiscountExceptions
        '
        Me.DataGridView_DiscountExceptions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView_DiscountExceptions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView_DiscountExceptions.Location = New System.Drawing.Point(16, 66)
        Me.DataGridView_DiscountExceptions.Name = "DataGridView_DiscountExceptions"
        Me.DataGridView_DiscountExceptions.Size = New System.Drawing.Size(483, 235)
        Me.DataGridView_DiscountExceptions.TabIndex = 2
        '
        'Label_DiscountExceptions
        '
        Me.Label_DiscountExceptions.AutoSize = True
        Me.Label_DiscountExceptions.Location = New System.Drawing.Point(16, 47)
        Me.Label_DiscountExceptions.Name = "Label_DiscountExceptions"
        Me.Label_DiscountExceptions.Size = New System.Drawing.Size(104, 13)
        Me.Label_DiscountExceptions.TabIndex = 3
        Me.Label_DiscountExceptions.Text = "Discount Exceptions"
        '
        'Button_Close
        '
        Me.Button_Close.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Close.Location = New System.Drawing.Point(423, 312)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(75, 23)
        Me.Button_Close.TabIndex = 4
        Me.Button_Close.Text = "Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'Button_Save
        '
        Me.Button_Save.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Save.Location = New System.Drawing.Point(342, 312)
        Me.Button_Save.Name = "Button_Save"
        Me.Button_Save.Size = New System.Drawing.Size(75, 23)
        Me.Button_Save.TabIndex = 5
        Me.Button_Save.Text = "Save"
        Me.Button_Save.UseVisualStyleBackColor = True
        '
        'Form_StoreSubTeamDiscountException
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(514, 347)
        Me.Controls.Add(Me.Button_Save)
        Me.Controls.Add(Me.Button_Close)
        Me.Controls.Add(Me.Label_DiscountExceptions)
        Me.Controls.Add(Me.DataGridView_DiscountExceptions)
        Me.Controls.Add(Me.ComboBox_Stores)
        Me.Controls.Add(Me.Label_Store)
        Me.Name = "Form_StoreSubTeamDiscountException"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Store / SubTeam Discount Exceptions"
        CType(Me.DataGridView_DiscountExceptions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label_Store As System.Windows.Forms.Label
    Friend WithEvents ComboBox_Stores As System.Windows.Forms.ComboBox
    Friend WithEvents DataGridView_DiscountExceptions As System.Windows.Forms.DataGridView
    Friend WithEvents Label_DiscountExceptions As System.Windows.Forms.Label
    Friend WithEvents Button_Close As System.Windows.Forms.Button
    Friend WithEvents Button_Save As System.Windows.Forms.Button
End Class
