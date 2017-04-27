<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_PromotionOffer_AssociateStores
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.RadioButton_Clear = New System.Windows.Forms.RadioButton
        Me.RadioButton_All = New System.Windows.Forms.RadioButton
        Me.RadioButton_Manual = New System.Windows.Forms.RadioButton
        Me.ListBox_Stores = New System.Windows.Forms.ListBox
        Me.Button_OK = New System.Windows.Forms.Button
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label_NoStoresMsg = New System.Windows.Forms.Label
        Me.PromotionalOfferStoreBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GroupBox1.SuspendLayout()
        CType(Me.PromotionalOfferStoreBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RadioButton_Clear)
        Me.GroupBox1.Controls.Add(Me.RadioButton_All)
        Me.GroupBox1.Controls.Add(Me.RadioButton_Manual)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(349, 54)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Store Selection"
        '
        'RadioButton_Clear
        '
        Me.RadioButton_Clear.AutoSize = True
        Me.RadioButton_Clear.Location = New System.Drawing.Point(258, 19)
        Me.RadioButton_Clear.Name = "RadioButton_Clear"
        Me.RadioButton_Clear.Size = New System.Drawing.Size(51, 17)
        Me.RadioButton_Clear.TabIndex = 3
        Me.RadioButton_Clear.TabStop = True
        Me.RadioButton_Clear.Text = "None"
        Me.RadioButton_Clear.UseVisualStyleBackColor = True
        '
        'RadioButton_All
        '
        Me.RadioButton_All.AutoSize = True
        Me.RadioButton_All.Location = New System.Drawing.Point(57, 19)
        Me.RadioButton_All.Name = "RadioButton_All"
        Me.RadioButton_All.Size = New System.Drawing.Size(36, 17)
        Me.RadioButton_All.TabIndex = 1
        Me.RadioButton_All.TabStop = True
        Me.RadioButton_All.Text = "All"
        Me.RadioButton_All.UseVisualStyleBackColor = True
        '
        'RadioButton_Manual
        '
        Me.RadioButton_Manual.AutoSize = True
        Me.RadioButton_Manual.Location = New System.Drawing.Point(150, 19)
        Me.RadioButton_Manual.Name = "RadioButton_Manual"
        Me.RadioButton_Manual.Size = New System.Drawing.Size(60, 17)
        Me.RadioButton_Manual.TabIndex = 0
        Me.RadioButton_Manual.TabStop = True
        Me.RadioButton_Manual.Text = "Manual"
        Me.RadioButton_Manual.UseVisualStyleBackColor = True
        '
        'ListBox_Stores
        '
        Me.ListBox_Stores.FormattingEnabled = True
        Me.ListBox_Stores.Location = New System.Drawing.Point(12, 69)
        Me.ListBox_Stores.Name = "ListBox_Stores"
        Me.ListBox_Stores.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.ListBox_Stores.Size = New System.Drawing.Size(349, 251)
        Me.ListBox_Stores.TabIndex = 2
        '
        'Button_OK
        '
        Me.Button_OK.Location = New System.Drawing.Point(286, 345)
        Me.Button_OK.Name = "Button_OK"
        Me.Button_OK.Size = New System.Drawing.Size(75, 23)
        Me.Button_OK.TabIndex = 3
        Me.Button_OK.Text = "OK"
        Me.Button_OK.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button_Cancel.Location = New System.Drawing.Point(205, 345)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Button_Cancel.TabIndex = 4
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 323)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(277, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "* denotes a store at which this promotion is already active"
        '
        'Label_NoStoresMsg
        '
        Me.Label_NoStoresMsg.AutoSize = True
        Me.Label_NoStoresMsg.Location = New System.Drawing.Point(9, 159)
        Me.Label_NoStoresMsg.Name = "Label_NoStoresMsg"
        Me.Label_NoStoresMsg.Size = New System.Drawing.Size(352, 13)
        Me.Label_NoStoresMsg.TabIndex = 6
        Me.Label_NoStoresMsg.Text = "No Stores can be found that support the Pricing Method of this Promotion"
        Me.Label_NoStoresMsg.Visible = False
        '
        'PromotionalOfferStoreBindingSource
        '
        Me.PromotionalOfferStoreBindingSource.DataSource = GetType(WholeFoods.IRMA.EPromotions.BusinessLogic.PromotionalOfferStoreBO)
        '
        'Form_PromotionOffer_AssociateStores
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(375, 380)
        Me.Controls.Add(Me.Label_NoStoresMsg)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.Button_OK)
        Me.Controls.Add(Me.ListBox_Stores)
        Me.Controls.Add(Me.GroupBox1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_PromotionOffer_AssociateStores"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Assign Stores a Promotion"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PromotionalOfferStoreBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton_All As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_Manual As System.Windows.Forms.RadioButton
    Friend WithEvents ListBox_Stores As System.Windows.Forms.ListBox
    Friend WithEvents Button_OK As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PromotionalOfferStoreBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents Label_NoStoresMsg As System.Windows.Forms.Label
    Friend WithEvents RadioButton_Clear As System.Windows.Forms.RadioButton
End Class
