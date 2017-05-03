Imports WholeFoods.IRMA.EPromotions.BusinessLogic

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_PromotionOfferEditor
    Inherits Form_IRMABase

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
        Dim DescLabel As System.Windows.Forms.Label
        Dim Label5 As System.Windows.Forms.Label
        Dim Appearance37 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand4 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("PromotionOfferMemberBO", -1)
        Dim UltraGridColumn49 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ModifiedDate")
        Dim UltraGridColumn50 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("IsDirty")
        Dim UltraGridColumn51 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Quantity")
        Dim UltraGridColumn52 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("GroupName")
        Dim UltraGridColumn53 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OfferID")
        Dim UltraGridColumn54 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Loading")
        Dim UltraGridColumn55 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("EntityState")
        Dim UltraGridColumn56 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UserID")
        Dim UltraGridColumn57 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Purpose")
        Dim UltraGridColumn58 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Modified")
        Dim UltraGridColumn59 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("JoinLogic")
        Dim UltraGridColumn60 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("isDeleted")
        Dim UltraGridColumn61 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("IsNew")
        Dim UltraGridColumn62 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("GroupID")
        Dim UltraGridColumn63 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OfferMemberID")
        Dim UltraGridColumn64 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CreateDate")
        Dim Appearance38 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance39 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance40 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance41 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance42 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance43 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance44 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance45 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance46 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance47 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance48 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance49 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand5 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("PromotionOfferMemberBO", -1)
        Dim UltraGridColumn65 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ModifiedDate")
        Dim UltraGridColumn66 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("IsDirty")
        Dim UltraGridColumn67 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Quantity")
        Dim UltraGridColumn68 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("GroupName")
        Dim UltraGridColumn69 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OfferID")
        Dim UltraGridColumn70 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Loading")
        Dim UltraGridColumn71 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("EntityState")
        Dim UltraGridColumn72 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UserID")
        Dim UltraGridColumn73 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Purpose")
        Dim UltraGridColumn74 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Modified")
        Dim UltraGridColumn75 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("JoinLogic")
        Dim UltraGridColumn76 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("isDeleted")
        Dim UltraGridColumn77 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("IsNew")
        Dim UltraGridColumn78 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("GroupID", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Descending, False)
        Dim UltraGridColumn79 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OfferMemberID")
        Dim UltraGridColumn80 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CreateDate")
        Dim Appearance50 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance51 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance52 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance53 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance54 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance55 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance56 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance57 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance58 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance59 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance60 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance61 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand6 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("PromotionOfferMemberBO", -1)
        Dim UltraGridColumn81 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ModifiedDate")
        Dim UltraGridColumn82 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("IsDirty")
        Dim UltraGridColumn83 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Quantity")
        Dim UltraGridColumn84 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("GroupName")
        Dim UltraGridColumn85 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OfferID")
        Dim UltraGridColumn86 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Loading")
        Dim UltraGridColumn87 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("EntityState")
        Dim UltraGridColumn88 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UserID")
        Dim UltraGridColumn89 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Purpose")
        Dim UltraGridColumn90 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Modified")
        Dim UltraGridColumn91 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("JoinLogic")
        Dim UltraGridColumn92 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("isDeleted")
        Dim UltraGridColumn93 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("IsNew")
        Dim UltraGridColumn94 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("GroupID")
        Dim UltraGridColumn95 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OfferMemberID")
        Dim UltraGridColumn96 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CreateDate")
        Dim Appearance62 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance63 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance64 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance65 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance66 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance67 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance68 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance69 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance70 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance71 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance72 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Me.GroupBox_PromotionalOffer = New System.Windows.Forms.GroupBox
        Me.TextBox_ReferenceCode = New System.Windows.Forms.TextBox
        Me.BindingSource_PromotionOffer = New System.Windows.Forms.BindingSource(Me.components)
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboBox_Subteam = New System.Windows.Forms.ComboBox
        Me.ComboBox_TaxClass = New System.Windows.Forms.ComboBox
        Me.DateTimePicker_EndDate = New System.Windows.Forms.DateTimePicker
        Me.DateTimePicker_StartDate = New System.Windows.Forms.DateTimePicker
        Me.ComboBox_PricingMethod = New System.Windows.Forms.ComboBox
        Me.BindingSource_PricingMethod = New System.Windows.Forms.BindingSource(Me.components)
        Me.TextBox_Description = New System.Windows.Forms.TextBox
        Me.Label_To = New System.Windows.Forms.Label
        Me.Label_From = New System.Windows.Forms.Label
        Me.Label_PricingMethod = New System.Windows.Forms.Label
        Me.GroupBox = New System.Windows.Forms.GroupBox
        Me.UltraGrid_MeetOneRequirements = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.BindingSource_Requirements = New System.Windows.Forms.BindingSource(Me.components)
        Me.UltraGrid_MandatoryRequirements = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.Button_AddMandatory = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button_DeleteMandatory = New System.Windows.Forms.Button
        Me.Button_EditMandatory = New System.Windows.Forms.Button
        Me.Button_DeleteMeetOne = New System.Windows.Forms.Button
        Me.Button_EditMeetOne = New System.Windows.Forms.Button
        Me.Button_AddMeetOne = New System.Windows.Forms.Button
        Me.GroupBox_Reward = New System.Windows.Forms.GroupBox
        Me.UltraGrid_Reward = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.Button_RewardDelete = New System.Windows.Forms.Button
        Me.Button_EditRewardGroup = New System.Windows.Forms.Button
        Me.TextBox_Amount = New System.Windows.Forms.TextBox
        Me.Label_Amount = New System.Windows.Forms.Label
        Me.ComboBox_RewardType = New System.Windows.Forms.ComboBox
        Me.Label_RewardType = New System.Windows.Forms.Label
        Me.Button_AddRewardGroup = New System.Windows.Forms.Button
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.Button_Ok = New System.Windows.Forms.Button
        Me.BindingSource_PromotionGroups = New System.Windows.Forms.BindingSource(Me.components)
        Me.Button_AssociateStores = New System.Windows.Forms.Button
        Me.Button_ManageGroups = New System.Windows.Forms.Button
        Me.Button_Unlock = New System.Windows.Forms.Button
        DescLabel = New System.Windows.Forms.Label
        Label5 = New System.Windows.Forms.Label
        Me.GroupBox_PromotionalOffer.SuspendLayout()
        CType(Me.BindingSource_PromotionOffer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSource_PricingMethod, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox.SuspendLayout()
        CType(Me.UltraGrid_MeetOneRequirements, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSource_Requirements, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraGrid_MandatoryRequirements, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_Reward.SuspendLayout()
        CType(Me.UltraGrid_Reward, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSource_PromotionGroups, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DescLabel
        '
        DescLabel.AutoSize = True
        DescLabel.Location = New System.Drawing.Point(8, 16)
        DescLabel.Name = "DescLabel"
        DescLabel.Size = New System.Drawing.Size(60, 13)
        DescLabel.TabIndex = 0
        DescLabel.Text = "Description"
        '
        'Label5
        '
        Label5.AutoSize = True
        Label5.Location = New System.Drawing.Point(228, 15)
        Label5.Name = "Label5"
        Label5.Size = New System.Drawing.Size(85, 13)
        Label5.TabIndex = 0
        Label5.Text = "Reference Code"
        '
        'GroupBox_PromotionalOffer
        '
        Me.GroupBox_PromotionalOffer.Controls.Add(Label5)
        Me.GroupBox_PromotionalOffer.Controls.Add(Me.TextBox_ReferenceCode)
        Me.GroupBox_PromotionalOffer.Controls.Add(Me.Label4)
        Me.GroupBox_PromotionalOffer.Controls.Add(Me.Label3)
        Me.GroupBox_PromotionalOffer.Controls.Add(Me.ComboBox_Subteam)
        Me.GroupBox_PromotionalOffer.Controls.Add(Me.ComboBox_TaxClass)
        Me.GroupBox_PromotionalOffer.Controls.Add(Me.DateTimePicker_EndDate)
        Me.GroupBox_PromotionalOffer.Controls.Add(Me.DateTimePicker_StartDate)
        Me.GroupBox_PromotionalOffer.Controls.Add(Me.ComboBox_PricingMethod)
        Me.GroupBox_PromotionalOffer.Controls.Add(Me.TextBox_Description)
        Me.GroupBox_PromotionalOffer.Controls.Add(DescLabel)
        Me.GroupBox_PromotionalOffer.Controls.Add(Me.Label_To)
        Me.GroupBox_PromotionalOffer.Controls.Add(Me.Label_From)
        Me.GroupBox_PromotionalOffer.Controls.Add(Me.Label_PricingMethod)
        Me.GroupBox_PromotionalOffer.Location = New System.Drawing.Point(12, 2)
        Me.GroupBox_PromotionalOffer.Name = "GroupBox_PromotionalOffer"
        Me.GroupBox_PromotionalOffer.Size = New System.Drawing.Size(535, 102)
        Me.GroupBox_PromotionalOffer.TabIndex = 0
        Me.GroupBox_PromotionalOffer.TabStop = False
        Me.GroupBox_PromotionalOffer.Text = "Promotional Offer"
        '
        'TextBox_ReferenceCode
        '
        Me.TextBox_ReferenceCode.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.BindingSource_PromotionOffer, "ReferenceCode", True))
        Me.TextBox_ReferenceCode.Location = New System.Drawing.Point(231, 31)
        Me.TextBox_ReferenceCode.MaxLength = 20
        Me.TextBox_ReferenceCode.Name = "TextBox_ReferenceCode"
        Me.TextBox_ReferenceCode.Size = New System.Drawing.Size(93, 20)
        Me.TextBox_ReferenceCode.TabIndex = 7
        '
        'BindingSource_PromotionOffer
        '
        Me.BindingSource_PromotionOffer.DataSource = GetType(WholeFoods.IRMA.EPromotions.BusinessLogic.PromotionOfferBO)
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(426, 15)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(83, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Loss Dept Code"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(332, 15)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Loss Vat Code"
        '
        'ComboBox_Subteam
        '
        Me.ComboBox_Subteam.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.BindingSource_PromotionOffer, "SubTeamNo", True))
        Me.ComboBox_Subteam.DisplayMember = "PricingMethodID"
        Me.ComboBox_Subteam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Subteam.FormattingEnabled = True
        Me.ComboBox_Subteam.Location = New System.Drawing.Point(429, 32)
        Me.ComboBox_Subteam.Name = "ComboBox_Subteam"
        Me.ComboBox_Subteam.Size = New System.Drawing.Size(100, 21)
        Me.ComboBox_Subteam.TabIndex = 9
        Me.ComboBox_Subteam.ValueMember = "PricingMethodID"
        '
        'ComboBox_TaxClass
        '
        Me.ComboBox_TaxClass.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.BindingSource_PromotionOffer, "TaxClassID", True))
        Me.ComboBox_TaxClass.DisplayMember = "PricingMethodID"
        Me.ComboBox_TaxClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_TaxClass.DropDownWidth = 200
        Me.ComboBox_TaxClass.FormattingEnabled = True
        Me.ComboBox_TaxClass.Location = New System.Drawing.Point(335, 32)
        Me.ComboBox_TaxClass.Name = "ComboBox_TaxClass"
        Me.ComboBox_TaxClass.Size = New System.Drawing.Size(88, 21)
        Me.ComboBox_TaxClass.TabIndex = 8
        Me.ComboBox_TaxClass.ValueMember = "PricingMethodID"
        '
        'DateTimePicker_EndDate
        '
        Me.DateTimePicker_EndDate.CustomFormat = "M/d/yyyy"
        Me.DateTimePicker_EndDate.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_PromotionOffer, "EndDate", True))
        Me.DateTimePicker_EndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker_EndDate.Location = New System.Drawing.Point(335, 72)
        Me.DateTimePicker_EndDate.Name = "DateTimePicker_EndDate"
        Me.DateTimePicker_EndDate.Size = New System.Drawing.Size(94, 20)
        Me.DateTimePicker_EndDate.TabIndex = 12
        '
        'DateTimePicker_StartDate
        '
        Me.DateTimePicker_StartDate.CustomFormat = "M/d/yyyy"
        Me.DateTimePicker_StartDate.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_PromotionOffer, "StartDate", True))
        Me.DateTimePicker_StartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker_StartDate.Location = New System.Drawing.Point(231, 72)
        Me.DateTimePicker_StartDate.Name = "DateTimePicker_StartDate"
        Me.DateTimePicker_StartDate.Size = New System.Drawing.Size(92, 20)
        Me.DateTimePicker_StartDate.TabIndex = 11
        '
        'ComboBox_PricingMethod
        '
        Me.ComboBox_PricingMethod.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.BindingSource_PromotionOffer, "PriceMethodID", True))
        Me.ComboBox_PricingMethod.DataSource = Me.BindingSource_PricingMethod
        Me.ComboBox_PricingMethod.DisplayMember = "Name"
        Me.ComboBox_PricingMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_PricingMethod.FormattingEnabled = True
        Me.ComboBox_PricingMethod.Location = New System.Drawing.Point(11, 71)
        Me.ComboBox_PricingMethod.Name = "ComboBox_PricingMethod"
        Me.ComboBox_PricingMethod.Size = New System.Drawing.Size(209, 21)
        Me.ComboBox_PricingMethod.TabIndex = 10
        Me.ComboBox_PricingMethod.ValueMember = "PricingMethodID"
        '
        'BindingSource_PricingMethod
        '
        Me.BindingSource_PricingMethod.DataSource = GetType(WholeFoods.IRMA.EPromotions.BusinessLogic.PricingMethodBO)
        '
        'TextBox_Description
        '
        Me.TextBox_Description.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.BindingSource_PromotionOffer, "Desc", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.TextBox_Description.Location = New System.Drawing.Point(11, 32)
        Me.TextBox_Description.MaxLength = 20
        Me.TextBox_Description.Name = "TextBox_Description"
        Me.TextBox_Description.Size = New System.Drawing.Size(209, 20)
        Me.TextBox_Description.TabIndex = 6
        '
        'Label_To
        '
        Me.Label_To.AutoSize = True
        Me.Label_To.Location = New System.Drawing.Point(332, 55)
        Me.Label_To.Name = "Label_To"
        Me.Label_To.Size = New System.Drawing.Size(20, 13)
        Me.Label_To.TabIndex = 5
        Me.Label_To.Text = "To"
        '
        'Label_From
        '
        Me.Label_From.AutoSize = True
        Me.Label_From.Location = New System.Drawing.Point(228, 55)
        Me.Label_From.Name = "Label_From"
        Me.Label_From.Size = New System.Drawing.Size(30, 13)
        Me.Label_From.TabIndex = 4
        Me.Label_From.Text = "From"
        '
        'Label_PricingMethod
        '
        Me.Label_PricingMethod.AutoSize = True
        Me.Label_PricingMethod.Location = New System.Drawing.Point(12, 55)
        Me.Label_PricingMethod.Name = "Label_PricingMethod"
        Me.Label_PricingMethod.Size = New System.Drawing.Size(78, 13)
        Me.Label_PricingMethod.TabIndex = 3
        Me.Label_PricingMethod.Text = "Pricing Method"
        '
        'GroupBox
        '
        Me.GroupBox.Controls.Add(Me.UltraGrid_MeetOneRequirements)
        Me.GroupBox.Controls.Add(Me.UltraGrid_MandatoryRequirements)
        Me.GroupBox.Controls.Add(Me.Button_AddMandatory)
        Me.GroupBox.Controls.Add(Me.Label2)
        Me.GroupBox.Controls.Add(Me.Label1)
        Me.GroupBox.Controls.Add(Me.Button_DeleteMandatory)
        Me.GroupBox.Controls.Add(Me.Button_EditMandatory)
        Me.GroupBox.Controls.Add(Me.Button_DeleteMeetOne)
        Me.GroupBox.Controls.Add(Me.Button_EditMeetOne)
        Me.GroupBox.Controls.Add(Me.Button_AddMeetOne)
        Me.GroupBox.Location = New System.Drawing.Point(12, 120)
        Me.GroupBox.Name = "GroupBox"
        Me.GroupBox.Size = New System.Drawing.Size(639, 291)
        Me.GroupBox.TabIndex = 1
        Me.GroupBox.TabStop = False
        Me.GroupBox.Text = "Purchase Requirements"
        '
        'UltraGrid_MeetOneRequirements
        '
        Me.UltraGrid_MeetOneRequirements.DataSource = Me.BindingSource_Requirements
        Appearance37.BackColor = System.Drawing.SystemColors.Window
        Appearance37.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.Appearance = Appearance37
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        UltraGridColumn49.Header.VisiblePosition = 0
        UltraGridColumn49.Hidden = True
        UltraGridColumn50.Header.VisiblePosition = 6
        UltraGridColumn50.Hidden = True
        UltraGridColumn51.Header.VisiblePosition = 10
        UltraGridColumn51.MaskInput = "nnnn"
        UltraGridColumn51.MaxLength = 4
        UltraGridColumn51.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        UltraGridColumn51.Width = 65
        UltraGridColumn52.Header.VisiblePosition = 4
        UltraGridColumn52.Hidden = True
        UltraGridColumn53.Header.VisiblePosition = 3
        UltraGridColumn53.Hidden = True
        UltraGridColumn54.Header.VisiblePosition = 7
        UltraGridColumn54.Hidden = True
        UltraGridColumn55.Header.VisiblePosition = 8
        UltraGridColumn55.Hidden = True
        UltraGridColumn56.Header.VisiblePosition = 2
        UltraGridColumn56.Hidden = True
        UltraGridColumn57.Header.VisiblePosition = 1
        UltraGridColumn57.Hidden = True
        UltraGridColumn58.Header.VisiblePosition = 13
        UltraGridColumn58.Hidden = True
        UltraGridColumn59.Header.VisiblePosition = 5
        UltraGridColumn59.Hidden = True
        UltraGridColumn60.Header.VisiblePosition = 9
        UltraGridColumn60.Hidden = True
        UltraGridColumn61.Header.VisiblePosition = 12
        UltraGridColumn61.Hidden = True
        UltraGridColumn62.Header.VisiblePosition = 11
        UltraGridColumn62.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
        UltraGridColumn62.Width = 367
        UltraGridColumn63.Header.VisiblePosition = 14
        UltraGridColumn63.Hidden = True
        UltraGridColumn64.Header.VisiblePosition = 15
        UltraGridColumn64.Hidden = True
        UltraGridBand4.Columns.AddRange(New Object() {UltraGridColumn49, UltraGridColumn50, UltraGridColumn51, UltraGridColumn52, UltraGridColumn53, UltraGridColumn54, UltraGridColumn55, UltraGridColumn56, UltraGridColumn57, UltraGridColumn58, UltraGridColumn59, UltraGridColumn60, UltraGridColumn61, UltraGridColumn62, UltraGridColumn63, UltraGridColumn64})
        UltraGridBand4.GroupHeadersVisible = False
        UltraGridBand4.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.BandsSerializer.Add(UltraGridBand4)
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance38.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance38.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance38.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.GroupByBox.Appearance = Appearance38
        Appearance39.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance39
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.GroupByBox.Hidden = True
        Appearance40.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance40.BackColor2 = System.Drawing.SystemColors.Control
        Appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance40.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.GroupByBox.PromptAppearance = Appearance40
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.MaxRowScrollRegions = 1
        Appearance41.BackColor = System.Drawing.SystemColors.Window
        Appearance41.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.Override.ActiveCellAppearance = Appearance41
        Appearance42.BackColor = System.Drawing.SystemColors.Highlight
        Appearance42.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.Override.ActiveRowAppearance = Appearance42
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance43.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.Override.CardAreaAppearance = Appearance43
        Appearance44.BorderColor = System.Drawing.Color.Silver
        Appearance44.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.Override.CellAppearance = Appearance44
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.Override.CellPadding = 0
        Appearance45.BackColor = System.Drawing.SystemColors.Control
        Appearance45.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance45.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance45.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.Override.GroupByRowAppearance = Appearance45
        Appearance46.TextHAlign = Infragistics.Win.HAlign.Left
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.Override.HeaderAppearance = Appearance46
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance47.BackColor = System.Drawing.SystemColors.Window
        Appearance47.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.Override.RowAppearance = Appearance47
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance48.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.Override.TemplateAddRowAppearance = Appearance48
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_MeetOneRequirements.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraGrid_MeetOneRequirements.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGrid_MeetOneRequirements.Location = New System.Drawing.Point(11, 38)
        Me.UltraGrid_MeetOneRequirements.Name = "UltraGrid_MeetOneRequirements"
        Me.UltraGrid_MeetOneRequirements.Size = New System.Drawing.Size(558, 111)
        Me.UltraGrid_MeetOneRequirements.TabIndex = 2
        Me.UltraGrid_MeetOneRequirements.TabStop = False
        Me.UltraGrid_MeetOneRequirements.Text = "UltraGrid1"
        '
        'BindingSource_Requirements
        '
        Me.BindingSource_Requirements.DataSource = GetType(WholeFoods.IRMA.EPromotions.BusinessLogic.PromotionOfferMemberBO)
        '
        'UltraGrid_MandatoryRequirements
        '
        Me.UltraGrid_MandatoryRequirements.DataSource = Me.BindingSource_Requirements
        Appearance49.BackColor = System.Drawing.SystemColors.Window
        Appearance49.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.Appearance = Appearance49
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        UltraGridColumn65.Header.VisiblePosition = 0
        UltraGridColumn65.Hidden = True
        UltraGridColumn66.Header.VisiblePosition = 6
        UltraGridColumn66.Hidden = True
        UltraGridColumn67.Header.VisiblePosition = 10
        UltraGridColumn67.MaskInput = "nnnn"
        UltraGridColumn67.MaxLength = 4
        UltraGridColumn67.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        UltraGridColumn67.Width = 61
        UltraGridColumn68.Header.VisiblePosition = 4
        UltraGridColumn68.Hidden = True
        UltraGridColumn69.Header.VisiblePosition = 3
        UltraGridColumn69.Hidden = True
        UltraGridColumn70.Header.VisiblePosition = 7
        UltraGridColumn70.Hidden = True
        UltraGridColumn71.Header.VisiblePosition = 8
        UltraGridColumn71.Hidden = True
        UltraGridColumn72.Header.VisiblePosition = 2
        UltraGridColumn72.Hidden = True
        UltraGridColumn73.Header.VisiblePosition = 1
        UltraGridColumn73.Hidden = True
        UltraGridColumn74.Header.VisiblePosition = 13
        UltraGridColumn74.Hidden = True
        UltraGridColumn75.Header.VisiblePosition = 5
        UltraGridColumn75.Hidden = True
        UltraGridColumn76.Header.VisiblePosition = 9
        UltraGridColumn76.Hidden = True
        UltraGridColumn77.Header.VisiblePosition = 12
        UltraGridColumn77.Hidden = True
        UltraGridColumn78.Header.VisiblePosition = 11
        UltraGridColumn78.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
        UltraGridColumn78.Width = 367
        UltraGridColumn79.Header.VisiblePosition = 14
        UltraGridColumn79.Hidden = True
        UltraGridColumn80.Header.VisiblePosition = 15
        UltraGridColumn80.Hidden = True
        UltraGridBand5.Columns.AddRange(New Object() {UltraGridColumn65, UltraGridColumn66, UltraGridColumn67, UltraGridColumn68, UltraGridColumn69, UltraGridColumn70, UltraGridColumn71, UltraGridColumn72, UltraGridColumn73, UltraGridColumn74, UltraGridColumn75, UltraGridColumn76, UltraGridColumn77, UltraGridColumn78, UltraGridColumn79, UltraGridColumn80})
        UltraGridBand5.GroupHeadersVisible = False
        UltraGridBand5.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.BandsSerializer.Add(UltraGridBand5)
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance50.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance50.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance50.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance50.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.GroupByBox.Appearance = Appearance50
        Appearance51.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance51
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.GroupByBox.Hidden = True
        Appearance52.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance52.BackColor2 = System.Drawing.SystemColors.Control
        Appearance52.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance52.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.GroupByBox.PromptAppearance = Appearance52
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.MaxRowScrollRegions = 1
        Appearance53.BackColor = System.Drawing.SystemColors.Window
        Appearance53.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.Override.ActiveCellAppearance = Appearance53
        Appearance54.BackColor = System.Drawing.SystemColors.Highlight
        Appearance54.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.Override.ActiveRowAppearance = Appearance54
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance55.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.Override.CardAreaAppearance = Appearance55
        Appearance56.BorderColor = System.Drawing.Color.Silver
        Appearance56.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.Override.CellAppearance = Appearance56
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.Override.CellPadding = 0
        Appearance57.BackColor = System.Drawing.SystemColors.Control
        Appearance57.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance57.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance57.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance57.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.Override.GroupByRowAppearance = Appearance57
        Appearance58.TextHAlign = Infragistics.Win.HAlign.Left
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.Override.HeaderAppearance = Appearance58
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance59.BackColor = System.Drawing.SystemColors.Window
        Appearance59.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.Override.RowAppearance = Appearance59
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance60.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.Override.TemplateAddRowAppearance = Appearance60
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_MandatoryRequirements.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraGrid_MandatoryRequirements.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGrid_MandatoryRequirements.Location = New System.Drawing.Point(11, 174)
        Me.UltraGrid_MandatoryRequirements.Name = "UltraGrid_MandatoryRequirements"
        Me.UltraGrid_MandatoryRequirements.Size = New System.Drawing.Size(558, 111)
        Me.UltraGrid_MandatoryRequirements.TabIndex = 3
        Me.UltraGrid_MandatoryRequirements.TabStop = False
        Me.UltraGrid_MandatoryRequirements.Text = "UltraGrid1"
        '
        'Button_AddMandatory
        '
        Me.Button_AddMandatory.Location = New System.Drawing.Point(575, 174)
        Me.Button_AddMandatory.Name = "Button_AddMandatory"
        Me.Button_AddMandatory.Size = New System.Drawing.Size(56, 21)
        Me.Button_AddMandatory.TabIndex = 7
        Me.Button_AddMandatory.Text = "Add"
        Me.Button_AddMandatory.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 160)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(125, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Mandatory Requirements"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(146, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Must Meet One Requirement "
        '
        'Button_DeleteMandatory
        '
        Me.Button_DeleteMandatory.Location = New System.Drawing.Point(575, 227)
        Me.Button_DeleteMandatory.Name = "Button_DeleteMandatory"
        Me.Button_DeleteMandatory.Size = New System.Drawing.Size(56, 20)
        Me.Button_DeleteMandatory.TabIndex = 9
        Me.Button_DeleteMandatory.Text = "Delete"
        Me.Button_DeleteMandatory.UseVisualStyleBackColor = True
        '
        'Button_EditMandatory
        '
        Me.Button_EditMandatory.Location = New System.Drawing.Point(575, 201)
        Me.Button_EditMandatory.Name = "Button_EditMandatory"
        Me.Button_EditMandatory.Size = New System.Drawing.Size(56, 20)
        Me.Button_EditMandatory.TabIndex = 8
        Me.Button_EditMandatory.Text = "Edit"
        Me.Button_EditMandatory.UseVisualStyleBackColor = True
        '
        'Button_DeleteMeetOne
        '
        Me.Button_DeleteMeetOne.Location = New System.Drawing.Point(575, 92)
        Me.Button_DeleteMeetOne.Name = "Button_DeleteMeetOne"
        Me.Button_DeleteMeetOne.Size = New System.Drawing.Size(56, 21)
        Me.Button_DeleteMeetOne.TabIndex = 6
        Me.Button_DeleteMeetOne.Text = "Delete"
        Me.Button_DeleteMeetOne.UseVisualStyleBackColor = True
        '
        'Button_EditMeetOne
        '
        Me.Button_EditMeetOne.Location = New System.Drawing.Point(575, 65)
        Me.Button_EditMeetOne.Name = "Button_EditMeetOne"
        Me.Button_EditMeetOne.Size = New System.Drawing.Size(56, 21)
        Me.Button_EditMeetOne.TabIndex = 5
        Me.Button_EditMeetOne.Text = "Edit"
        Me.Button_EditMeetOne.UseVisualStyleBackColor = True
        '
        'Button_AddMeetOne
        '
        Me.Button_AddMeetOne.Location = New System.Drawing.Point(575, 39)
        Me.Button_AddMeetOne.Name = "Button_AddMeetOne"
        Me.Button_AddMeetOne.Size = New System.Drawing.Size(56, 21)
        Me.Button_AddMeetOne.TabIndex = 4
        Me.Button_AddMeetOne.Text = "Add"
        Me.Button_AddMeetOne.UseVisualStyleBackColor = True
        '
        'GroupBox_Reward
        '
        Me.GroupBox_Reward.Controls.Add(Me.UltraGrid_Reward)
        Me.GroupBox_Reward.Controls.Add(Me.Button_RewardDelete)
        Me.GroupBox_Reward.Controls.Add(Me.Button_EditRewardGroup)
        Me.GroupBox_Reward.Controls.Add(Me.TextBox_Amount)
        Me.GroupBox_Reward.Controls.Add(Me.Label_Amount)
        Me.GroupBox_Reward.Controls.Add(Me.ComboBox_RewardType)
        Me.GroupBox_Reward.Controls.Add(Me.Label_RewardType)
        Me.GroupBox_Reward.Controls.Add(Me.Button_AddRewardGroup)
        Me.GroupBox_Reward.Location = New System.Drawing.Point(12, 417)
        Me.GroupBox_Reward.Name = "GroupBox_Reward"
        Me.GroupBox_Reward.Size = New System.Drawing.Size(639, 181)
        Me.GroupBox_Reward.TabIndex = 2
        Me.GroupBox_Reward.TabStop = False
        Me.GroupBox_Reward.Text = "Reward"
        '
        'UltraGrid_Reward
        '
        Me.UltraGrid_Reward.DataSource = Me.BindingSource_Requirements
        Appearance61.BackColor = System.Drawing.SystemColors.Window
        Appearance61.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_Reward.DisplayLayout.Appearance = Appearance61
        Me.UltraGrid_Reward.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        UltraGridColumn81.Header.VisiblePosition = 0
        UltraGridColumn81.Hidden = True
        UltraGridColumn82.Header.VisiblePosition = 6
        UltraGridColumn82.Hidden = True
        UltraGridColumn83.Header.VisiblePosition = 10
        UltraGridColumn84.Header.VisiblePosition = 4
        UltraGridColumn84.Hidden = True
        UltraGridColumn85.Header.VisiblePosition = 2
        UltraGridColumn85.Hidden = True
        UltraGridColumn86.Header.VisiblePosition = 7
        UltraGridColumn86.Hidden = True
        UltraGridColumn87.Header.VisiblePosition = 8
        UltraGridColumn87.Hidden = True
        UltraGridColumn88.Header.VisiblePosition = 5
        UltraGridColumn88.Hidden = True
        UltraGridColumn89.Header.VisiblePosition = 1
        UltraGridColumn89.Hidden = True
        UltraGridColumn90.Header.VisiblePosition = 13
        UltraGridColumn90.Hidden = True
        UltraGridColumn91.Header.VisiblePosition = 3
        UltraGridColumn91.Hidden = True
        UltraGridColumn92.Header.VisiblePosition = 9
        UltraGridColumn92.Hidden = True
        UltraGridColumn93.Header.VisiblePosition = 12
        UltraGridColumn93.Hidden = True
        UltraGridColumn94.Header.VisiblePosition = 11
        UltraGridColumn94.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
        UltraGridColumn94.Width = 364
        UltraGridColumn95.Header.VisiblePosition = 14
        UltraGridColumn95.Hidden = True
        UltraGridColumn96.Header.VisiblePosition = 15
        UltraGridColumn96.Hidden = True
        UltraGridBand6.Columns.AddRange(New Object() {UltraGridColumn81, UltraGridColumn82, UltraGridColumn83, UltraGridColumn84, UltraGridColumn85, UltraGridColumn86, UltraGridColumn87, UltraGridColumn88, UltraGridColumn89, UltraGridColumn90, UltraGridColumn91, UltraGridColumn92, UltraGridColumn93, UltraGridColumn94, UltraGridColumn95, UltraGridColumn96})
        UltraGridBand6.GroupHeadersVisible = False
        UltraGridBand6.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_Reward.DisplayLayout.BandsSerializer.Add(UltraGridBand6)
        Me.UltraGrid_Reward.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_Reward.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance62.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance62.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance62.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance62.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_Reward.DisplayLayout.GroupByBox.Appearance = Appearance62
        Appearance63.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_Reward.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance63
        Me.UltraGrid_Reward.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_Reward.DisplayLayout.GroupByBox.Hidden = True
        Appearance64.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance64.BackColor2 = System.Drawing.SystemColors.Control
        Appearance64.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance64.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_Reward.DisplayLayout.GroupByBox.PromptAppearance = Appearance64
        Me.UltraGrid_Reward.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_Reward.DisplayLayout.MaxRowScrollRegions = 1
        Appearance65.BackColor = System.Drawing.SystemColors.Window
        Appearance65.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_Reward.DisplayLayout.Override.ActiveCellAppearance = Appearance65
        Appearance66.BackColor = System.Drawing.SystemColors.Highlight
        Appearance66.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid_Reward.DisplayLayout.Override.ActiveRowAppearance = Appearance66
        Me.UltraGrid_Reward.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_Reward.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance67.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_Reward.DisplayLayout.Override.CardAreaAppearance = Appearance67
        Appearance68.BorderColor = System.Drawing.Color.Silver
        Appearance68.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_Reward.DisplayLayout.Override.CellAppearance = Appearance68
        Me.UltraGrid_Reward.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_Reward.DisplayLayout.Override.CellPadding = 0
        Appearance69.BackColor = System.Drawing.SystemColors.Control
        Appearance69.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance69.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance69.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance69.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_Reward.DisplayLayout.Override.GroupByRowAppearance = Appearance69
        Appearance70.TextHAlign = Infragistics.Win.HAlign.Left
        Me.UltraGrid_Reward.DisplayLayout.Override.HeaderAppearance = Appearance70
        Me.UltraGrid_Reward.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_Reward.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance71.BackColor = System.Drawing.SystemColors.Window
        Appearance71.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_Reward.DisplayLayout.Override.RowAppearance = Appearance71
        Me.UltraGrid_Reward.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance72.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_Reward.DisplayLayout.Override.TemplateAddRowAppearance = Appearance72
        Me.UltraGrid_Reward.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_Reward.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_Reward.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraGrid_Reward.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGrid_Reward.Location = New System.Drawing.Point(11, 58)
        Me.UltraGrid_Reward.Name = "UltraGrid_Reward"
        Me.UltraGrid_Reward.Size = New System.Drawing.Size(558, 111)
        Me.UltraGrid_Reward.TabIndex = 2
        Me.UltraGrid_Reward.TabStop = False
        Me.UltraGrid_Reward.Text = "UltraGrid2"
        '
        'Button_RewardDelete
        '
        Me.Button_RewardDelete.Location = New System.Drawing.Point(575, 112)
        Me.Button_RewardDelete.Name = "Button_RewardDelete"
        Me.Button_RewardDelete.Size = New System.Drawing.Size(56, 21)
        Me.Button_RewardDelete.TabIndex = 7
        Me.Button_RewardDelete.Text = "Delete"
        Me.Button_RewardDelete.UseVisualStyleBackColor = True
        '
        'Button_EditRewardGroup
        '
        Me.Button_EditRewardGroup.Location = New System.Drawing.Point(575, 85)
        Me.Button_EditRewardGroup.Name = "Button_EditRewardGroup"
        Me.Button_EditRewardGroup.Size = New System.Drawing.Size(56, 21)
        Me.Button_EditRewardGroup.TabIndex = 6
        Me.Button_EditRewardGroup.Text = "Edit"
        Me.Button_EditRewardGroup.UseVisualStyleBackColor = True
        '
        'TextBox_Amount
        '
        Me.TextBox_Amount.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.BindingSource_PromotionOffer, "RewardAmount", True))
        Me.TextBox_Amount.Location = New System.Drawing.Point(180, 32)
        Me.TextBox_Amount.Name = "TextBox_Amount"
        Me.TextBox_Amount.Size = New System.Drawing.Size(62, 20)
        Me.TextBox_Amount.TabIndex = 4
        '
        'Label_Amount
        '
        Me.Label_Amount.AutoSize = True
        Me.Label_Amount.Location = New System.Drawing.Point(177, 16)
        Me.Label_Amount.Name = "Label_Amount"
        Me.Label_Amount.Size = New System.Drawing.Size(43, 13)
        Me.Label_Amount.TabIndex = 1
        Me.Label_Amount.Text = "Amount"
        '
        'ComboBox_RewardType
        '
        Me.ComboBox_RewardType.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.BindingSource_PromotionOffer, "RewardID", True))
        Me.ComboBox_RewardType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_RewardType.FormattingEnabled = True
        Me.ComboBox_RewardType.Location = New System.Drawing.Point(11, 31)
        Me.ComboBox_RewardType.Name = "ComboBox_RewardType"
        Me.ComboBox_RewardType.Size = New System.Drawing.Size(155, 21)
        Me.ComboBox_RewardType.TabIndex = 3
        '
        'Label_RewardType
        '
        Me.Label_RewardType.AutoSize = True
        Me.Label_RewardType.Location = New System.Drawing.Point(14, 16)
        Me.Label_RewardType.Name = "Label_RewardType"
        Me.Label_RewardType.Size = New System.Drawing.Size(71, 13)
        Me.Label_RewardType.TabIndex = 0
        Me.Label_RewardType.Text = "Reward Type"
        '
        'Button_AddRewardGroup
        '
        Me.Button_AddRewardGroup.Location = New System.Drawing.Point(575, 58)
        Me.Button_AddRewardGroup.Name = "Button_AddRewardGroup"
        Me.Button_AddRewardGroup.Size = New System.Drawing.Size(56, 21)
        Me.Button_AddRewardGroup.TabIndex = 5
        Me.Button_AddRewardGroup.Text = "Add"
        Me.Button_AddRewardGroup.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Cancel.Location = New System.Drawing.Point(449, 604)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(98, 21)
        Me.Button_Cancel.TabIndex = 2
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Button_Ok
        '
        Me.Button_Ok.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Ok.Location = New System.Drawing.Point(553, 604)
        Me.Button_Ok.Name = "Button_Ok"
        Me.Button_Ok.Size = New System.Drawing.Size(98, 21)
        Me.Button_Ok.TabIndex = 3
        Me.Button_Ok.Text = "OK"
        Me.Button_Ok.UseVisualStyleBackColor = True
        '
        'BindingSource_PromotionGroups
        '
        Me.BindingSource_PromotionGroups.DataSource = GetType(WholeFoods.IRMA.EPromotions.BusinessLogic.ItemGroupBO)
        '
        'Button_AssociateStores
        '
        Me.Button_AssociateStores.Location = New System.Drawing.Point(553, 36)
        Me.Button_AssociateStores.Name = "Button_AssociateStores"
        Me.Button_AssociateStores.Size = New System.Drawing.Size(98, 21)
        Me.Button_AssociateStores.TabIndex = 1
        Me.Button_AssociateStores.Text = "Associate Stores"
        Me.Button_AssociateStores.UseVisualStyleBackColor = True
        '
        'Button_ManageGroups
        '
        Me.Button_ManageGroups.Location = New System.Drawing.Point(553, 9)
        Me.Button_ManageGroups.Name = "Button_ManageGroups"
        Me.Button_ManageGroups.Size = New System.Drawing.Size(98, 21)
        Me.Button_ManageGroups.TabIndex = 0
        Me.Button_ManageGroups.Text = "Manage Groups"
        Me.Button_ManageGroups.UseVisualStyleBackColor = True
        '
        'Button_Unlock
        '
        Me.Button_Unlock.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Unlock.Location = New System.Drawing.Point(12, 604)
        Me.Button_Unlock.Name = "Button_Unlock"
        Me.Button_Unlock.Size = New System.Drawing.Size(98, 21)
        Me.Button_Unlock.TabIndex = 4
        Me.Button_Unlock.Text = "Unlock"
        Me.Button_Unlock.UseVisualStyleBackColor = True
        '
        'Form_PromotionOfferEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(655, 631)
        Me.Controls.Add(Me.Button_Unlock)
        Me.Controls.Add(Me.Button_AssociateStores)
        Me.Controls.Add(Me.Button_ManageGroups)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.Button_Ok)
        Me.Controls.Add(Me.GroupBox_Reward)
        Me.Controls.Add(Me.GroupBox)
        Me.Controls.Add(Me.GroupBox_PromotionalOffer)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_PromotionOfferEditor"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Promotion Offer Editor"
        Me.GroupBox_PromotionalOffer.ResumeLayout(False)
        Me.GroupBox_PromotionalOffer.PerformLayout()
        CType(Me.BindingSource_PromotionOffer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSource_PricingMethod, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox.ResumeLayout(False)
        Me.GroupBox.PerformLayout()
        CType(Me.UltraGrid_MeetOneRequirements, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSource_Requirements, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraGrid_MandatoryRequirements, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_Reward.ResumeLayout(False)
        Me.GroupBox_Reward.PerformLayout()
        CType(Me.UltraGrid_Reward, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSource_PromotionGroups, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox_PromotionalOffer As System.Windows.Forms.GroupBox
    Friend WithEvents Label_To As System.Windows.Forms.Label
    Friend WithEvents Label_From As System.Windows.Forms.Label
    Friend WithEvents Label_PricingMethod As System.Windows.Forms.Label
    Friend WithEvents GroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents Button_AddMeetOne As System.Windows.Forms.Button
    Friend WithEvents GroupBox_Reward As System.Windows.Forms.GroupBox
    Friend WithEvents Button_AddRewardGroup As System.Windows.Forms.Button
    Friend WithEvents Label_RewardType As System.Windows.Forms.Label
    Friend WithEvents TextBox_Amount As System.Windows.Forms.TextBox
    Friend WithEvents Label_Amount As System.Windows.Forms.Label
    Friend WithEvents ComboBox_RewardType As System.Windows.Forms.ComboBox
    Friend WithEvents Button_DeleteMeetOne As System.Windows.Forms.Button
    Friend WithEvents Button_EditMeetOne As System.Windows.Forms.Button
    Friend WithEvents Button_RewardDelete As System.Windows.Forms.Button
    Friend WithEvents Button_EditRewardGroup As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents Button_Ok As System.Windows.Forms.Button
    Friend WithEvents BindingSource_Requirements As System.Windows.Forms.BindingSource
    Friend WithEvents BindingSource_PricingMethod As System.Windows.Forms.BindingSource
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button_DeleteMandatory As System.Windows.Forms.Button
    Friend WithEvents Button_EditMandatory As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button_AddMandatory As System.Windows.Forms.Button
    Friend WithEvents UltraGrid_MandatoryRequirements As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents UltraGrid_Reward As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents DateTimePicker_EndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents BindingSource_PromotionOffer As System.Windows.Forms.BindingSource
    Friend WithEvents DateTimePicker_StartDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents ComboBox_PricingMethod As System.Windows.Forms.ComboBox
    Friend WithEvents TextBox_Description As System.Windows.Forms.TextBox
    Friend WithEvents BindingSource_PromotionGroups As System.Windows.Forms.BindingSource
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboBox_Subteam As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox_TaxClass As System.Windows.Forms.ComboBox
    Friend WithEvents TextBox_ReferenceCode As System.Windows.Forms.TextBox
    Friend WithEvents Button_AssociateStores As System.Windows.Forms.Button
    Friend WithEvents Button_ManageGroups As System.Windows.Forms.Button
    Friend WithEvents UltraGrid_MeetOneRequirements As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents Button_Unlock As System.Windows.Forms.Button

End Class
