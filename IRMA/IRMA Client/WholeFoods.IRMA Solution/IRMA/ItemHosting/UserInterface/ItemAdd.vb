Option Strict Off
Option Explicit On

Imports log4net
Imports System.Text
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.Utility

Friend Class frmItemAdd
    Inherits System.Windows.Forms.Form

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private IsInitializing As Boolean
    Private iComboIndex() As Integer
    Private plSubTeam_No As Integer
    Private mdtTaxClass As DataTable
    Private isShowOption_DoNotSendToScale As Boolean
    Private mgBydt As DataTable

    Public psIdentifierType As String
	Public psIdentifier As String
    Public psCheckDigit As String
    Public piNationalID As Int16
    Public isScaleIdentifier As Boolean
    Public keepItemIdentiferAddOpen As Boolean

    Private Sub frmItemAdd_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        txtField(iItemIdentifier).Text = Me.psIdentifier
        LoadPackageDescriptionUnit(cmbField(iItemPackage_Unit_ID), False)
        LoadItemUnitsVendor(cmbField(iItemVendor_Unit_ID), False)
        LoadItemUnitsVendor(cmbField(iItemDistribution_Unit_ID), False, True)
        LoadItemUnitsCost(cmbField(iItemRetail_Unit_ID), False)
        LoadNatClass(cmbField(iItemNatClassID))

        '-- Load data into the Tax class combo
        mdtTaxClass = GetTaxClassificationData()
        cmbTaxClass.DataSource = mdtTaxClass
        cmbTaxClass.DisplayMember = "TaxClassDesc"
        cmbTaxClass.ValueMember = "TaxClassID"
        cmbTaxClass.SelectedIndex = -1

        cmbLabelType.DataSource = LabelTypeDAO.GetLabelTypeList
        cmbLabelType.DisplayMember = "LabelTypeDesc"
        cmbLabelType.ValueMember = "LabelTypeID"
        cmbLabelType.SelectedIndex = -1

        '-- Load data into the combos
        ComboBox_Brand.DataSource = GetBrandData()
        ComboBox_Brand.DisplayMember = "Brand_Name"
        ComboBox_Brand.ValueMember = "Brand_ID"
        ComboBox_Brand.SelectedIndex = -1

        mgBydt = ItemManagerDAO.GetItemManagers()
        cmbManagedBy.DataSource = mgBydt
        cmbManagedBy.DisplayMember = "Value"
        cmbManagedBy.ValueMember = "Manager_ID"
        cmbManagedBy.SelectedIndex = -1

        Dim storeJurisdiction As New StoreJurisdictionDAO
        ComboBoxJurisdiction.DataSource = storeJurisdiction.GetJurisdictionList
        ComboBoxJurisdiction.DisplayMember = "StoreJurisdictionDesc"
        ComboBoxJurisdiction.ValueMember = "StoreJurisdictionID"
        ComboBoxJurisdiction.SelectedIndex = 0

        'determine if user is entering a scale item
        IsScaleItem()

        If Not isScaleIdentifier Then
            '-- HIDE "# Digits Sent To Scale" if this is NOT a scale identifier 
            Me.GroupBox_NumScaleDigits.Visible = False
            Me.Label_NumScaleDigits.Visible = False

            '-- HIDE "do not send to scales" checkbox
            Me.Label_SendToScale.Visible = False
            Me.GroupBox_SendToScale.Visible = False
        Else
            '-- DISABLE the "# Digits Sent To Scale" section, but display the number of digits being sent 
            '   as determined by the regional setting
            If gsPluDigitsSentToScale.Equals("ALWAYS 4") Then
                Me.GroupBox_NumScaleDigits.Enabled = False
                Me.RadioButton_NumScaleDigits_4.Checked = True
            ElseIf gsPluDigitsSentToScale.Equals("ALWAYS 5") Then
                Me.GroupBox_NumScaleDigits.Enabled = False
                Me.RadioButton_NumScaleDigits_5.Checked = True
            End If
        End If

        'show "Do NOT Send To Scale" checkbox only if item is a scale item AND regional flag = TRUE
        isShowOption_DoNotSendToScale = InstanceDataDAO.IsFlagActive("ShowOption_DoNotSendToScale")
        If Not isScaleIdentifier Or Not isShowOption_DoNotSendToScale Then
            Me.Label_SendToScale.Visible = False
            Me.GroupBox_SendToScale.Visible = False
        End If

        '-- Identifiers in this range will be reserved as non-retail ingredient items that need to be added to Icon
        If ((Convert.ToDouble(psIdentifier) >= 46000000000 And Convert.ToDouble(psIdentifier) <= "46999999999") Or (Convert.ToDouble(psIdentifier) >= "48000000000" And Convert.ToDouble(psIdentifier) <= "48999999999")) Then
            chkRetailSale.Enabled = False
        End If

        Dim bDisableBrandAdditions As Boolean = ConfigurationServices.AppSettings("DisableBrandAdditions")
        SetActive(AddBrandButton, Not bDisableBrandAdditions, bDisableBrandAdditions)

        HideFields()
    End Sub

    Private Sub HideFields()

        ' Managed By group box
        grpManageBy.Visible = InstanceDataDAO.IsFlagActive("ShowManagedBy")

        ' Disable the store jurisdiction combo box if the region does not support multiple jurisdictions
        If InstanceDataDAO.IsFlagActive("UseStoreJurisdictions") Then
            ComboBoxJurisdiction.Enabled = True
        Else
            ComboBoxJurisdiction.Enabled = False
        End If

    End Sub

    ''' <summary>
    ''' determines if identifier created and selected Identifier Type matches the criteria of a scale item
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub IsScaleItem()
        If (ScaleDAO.IsScaleIdentifier(psIdentifier) Or isScaleIdentifier) Then
            isScaleIdentifier = True
        Else
            isScaleIdentifier = False
        End If
    End Sub

    Private Sub chkCostedByWeight_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostedByWeight.CheckedChanged
        If IsInitializing Then Exit Sub

        Dim currValue As Integer

        If cmbField(iItemPackage_Unit_ID).SelectedIndex > -1 Then
            currValue = VB6.GetItemData(cmbField(iItemPackage_Unit_ID), cmbField(iItemPackage_Unit_ID).SelectedIndex)
        End If

        LoadPackageDescriptionUnit(cmbField(iItemPackage_Unit_ID), False)

        If currValue > 0 Then
            cmbField(iItemPackage_Unit_ID).SelectedIndex = -1
            SetCombo(cmbField(iItemPackage_Unit_ID), currValue)
        End If

        currValue = 0
        If cmbField(iItemRetail_Unit_ID).SelectedIndex > -1 Then
            currValue = cmbField(iItemRetail_Unit_ID).SelectedValue
        End If

        LoadItemUnitsCost(cmbField(iItemRetail_Unit_ID), False, True)

        If currValue > 0 Then
            cmbField(iItemRetail_Unit_ID).SelectedValue = currValue
        End If

        currValue = 0
        If cmbField(iItemVendor_Unit_ID).SelectedIndex > -1 Then
            currValue = cmbField(iItemVendor_Unit_ID).SelectedValue
        End If

        LoadItemUnitsVendor(cmbField(iItemVendor_Unit_ID), (chkCostedByWeight.CheckState = System.Windows.Forms.CheckState.Checked)) 'this added - Alex Z - BUG #6565 - 05/22/08

        If currValue > 0 Then
            cmbField(iItemVendor_Unit_ID).SelectedValue = currValue
        End If

        currValue = 0
        If cmbField(iItemDistribution_Unit_ID).SelectedIndex > -1 Then
            currValue = cmbField(iItemDistribution_Unit_ID).SelectedValue
        End If

        LoadItemUnitsVendor(cmbField(iItemDistribution_Unit_ID), (chkCostedByWeight.CheckState = System.Windows.Forms.CheckState.Checked)) 'this added - Alex Z - BUG #6670 - 06/08/08

        If currValue > 0 Then
            cmbField(iItemDistribution_Unit_ID).SelectedValue = currValue
        End If

        cmbField(iItemRetail_Unit_ID).SelectedIndex = cmbField(iItemPackage_Unit_ID).SelectedIndex

    End Sub

    Private Sub chkCostedByWeight_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkCostedByWeight.Enter
        chkCostedByWeight.BackColor = System.Drawing.SystemColors.Highlight
    End Sub

    Private Sub chkCostedByWeight_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkCostedByWeight.Leave
        chkCostedByWeight.BackColor = System.Drawing.SystemColors.Control
    End Sub

    Private Sub chkRetailSale_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkRetailSale.Enter
        chkRetailSale.BackColor = System.Drawing.SystemColors.Highlight
    End Sub

    Private Sub chkRetailSale_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkRetailSale.Leave
        chkRetailSale.BackColor = System.Drawing.SystemColors.Control
    End Sub

    Private Sub cmbField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbField.KeyPress, _cmbField_9.KeyPress, _cmbField_8.KeyPress, _cmbField_6.KeyPress, _cmbField_5.KeyPress, _cmbField_15.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        Dim Index As Short = cmbField.GetIndex(eventSender)

        If KeyAscii = 13 Then
            KeyAscii = 0
            System.Windows.Forms.SendKeys.Send("{TAB}")
        ElseIf KeyAscii = 8 Then
            If Index = iItemNatClassID Then cmbField(Index).SelectedIndex = -1
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click
        Dim sMsg As String = String.Empty
        Dim iNumScaleDigits As Integer
        Dim skipSave As Boolean
        Dim bIdentifierOK As Boolean = False

        If Trim(txtField(iItemItem_Description).Text) = "" Then
            sMsg = String.Format(ResourcesIRMA.GetString("Required"), lblDescription.Text.Replace(":", ""))
            MsgBox(sMsg, MsgBoxStyle.Critical, Me.Text)
            txtField(iItemItem_Description).Focus()
            Exit Sub
        End If

        If Trim(txtField(iItemPOS_Description).Text) = "" Then
            sMsg = String.Format(ResourcesIRMA.GetString("Required"), lblPOSDesc.Text.Replace(":", ""))
            MsgBox(sMsg, MsgBoxStyle.Critical, Me.Text)
            txtField(iItemPOS_Description).Focus()
            Exit Sub
        End If

        Select Case -1
            Case ComboBoxJurisdiction.SelectedIndex
                sMsg = String.Format(ResourcesIRMA.GetString("Required"), Label_DefaultJurisdiction.Text.Replace(":", ""))
                MsgBox(sMsg, MsgBoxStyle.Critical, Me.Text)
                cmbTaxClass.Focus()
                Exit Sub
            Case cmbTaxClass.SelectedIndex
                sMsg = String.Format(ResourcesIRMA.GetString("Required"), lblTaxClass.Text.Replace(":", ""))
                MsgBox(sMsg, MsgBoxStyle.Critical, Me.Text)
                cmbTaxClass.Focus()
                Exit Sub
            Case Me.HierarchySelector1.cmbSubTeam.SelectedIndex
                sMsg = String.Format(ResourcesIRMA.GetString("Required"), Me.HierarchySelector1.lblSubTeam.Text.Replace(":", ""))
                MsgBox(sMsg, MsgBoxStyle.Critical, Me.Text)
                Me.HierarchySelector1.cmbSubTeam.Focus()
                Exit Sub
            Case Me.HierarchySelector1.cmbCategory.SelectedIndex
                sMsg = String.Format(ResourcesIRMA.GetString("Required"), Me.HierarchySelector1.lblCategory.Text.Replace(":", ""))
                MsgBox(sMsg, MsgBoxStyle.Critical, Me.Text)
                Me.HierarchySelector1.cmbCategory.Focus()
                Exit Sub
            Case ComboBox_Brand.SelectedIndex
                sMsg = String.Format(ResourcesIRMA.GetString("Required"), Label_Brand.Text.Replace(":", ""))
                MsgBox(sMsg, MsgBoxStyle.Critical, Me.Text)
                Me.HierarchySelector1.cmbCategory.Focus()
                Exit Sub
        End Select

        ' require the bottom two levels of the hierarchy only if the region uses four levels
        If HierarchySelector1.UsesFourLevelHierarchy Then
            Select Case -1
                Case Me.HierarchySelector1.cmbLevel3.SelectedIndex
                    sMsg = String.Format(ResourcesIRMA.GetString("Required"), Me.HierarchySelector1.lblLevel3.Text.Replace(":", ""))
                    MsgBox(sMsg, MsgBoxStyle.Critical, Me.Text)
                    Me.HierarchySelector1.cmbLevel3.Focus()
                    Exit Sub
                Case Me.HierarchySelector1.cmbLevel4.SelectedIndex
                    sMsg = String.Format(ResourcesIRMA.GetString("Required"), Me.HierarchySelector1.lblLevel4.Text.Replace(":", ""))
                    MsgBox(sMsg, MsgBoxStyle.Critical, Me.Text)
                    Me.HierarchySelector1.cmbLevel4.Focus()
                    Exit Sub
            End Select
        End If

        'Look at InstanceDataFlags, figure out if LabelType is required.
        If InstanceDataDAO.IsFlagActive("Required_LabelType") AndAlso (cmbLabelType.Text Is Nothing Or cmbLabelType.Text = "") Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), Label_LabelType.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
            cmbLabelType.Focus()
            Exit Sub
        End If

        If InstanceDataDAO.IsFlagActive("Required_ManagedBy") AndAlso (cmbManagedBy.SelectedIndex = -1) Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), grpManageBy.Text), MsgBoxStyle.Critical, Me.Text)
            cmbManagedBy.Focus()
            Exit Sub
        End If
        
        'Package Desc 1
        If txtField(iItemPackage_Desc1).Text = vbNullString Then
            sMsg = String.Format(ResourcesIRMA.GetString("Required"), ResourcesItemHosting.GetString("PkgDesc1"))
            MsgBox(sMsg, MsgBoxStyle.Critical, Me.Text)
            txtField(iItemPackage_Desc1).Focus()
            Exit Sub
        Else
            If Not IsNumeric(txtField(iItemPackage_Desc1).Text) Then
                sMsg = String.Format(ResourcesIRMA.GetString("Pack is not Numeric."), ResourcesItemHosting.GetString("IsPackNumeric"))
                MsgBox(sMsg, MsgBoxStyle.Critical, Me.Text)
                txtField(iItemPackage_Desc1).Focus()
                Exit Sub
            End If
            If Val(txtField(iItemPackage_Desc1).Text) = 0 Then
                sMsg = String.Format(ResourcesIRMA.GetString("NotZero"), ResourcesItemHosting.GetString("PkgDesc1"))
                MsgBox(sMsg, MsgBoxStyle.Critical, Me.Text)
                txtField(iItemPackage_Desc1).Focus()
                Exit Sub
            End If
        End If

        'Package Desc 2
        If txtField(iItemPackage_Desc2).Text = vbNullString Then
            sMsg = String.Format(ResourcesIRMA.GetString("Required"), ResourcesItemHosting.GetString("PkgDesc2"))
            MsgBox(sMsg, MsgBoxStyle.Critical, Me.Text)
            txtField(iItemPackage_Desc2).Focus()
            Exit Sub
        Else
            If Not IsNumeric(txtField(iItemPackage_Desc2).Text) Then
                sMsg = String.Format(ResourcesIRMA.GetString("Size is not Numeric."), ResourcesItemHosting.GetString("IsSizeNumeric"))
                MsgBox(sMsg, MsgBoxStyle.Critical, Me.Text)
                txtField(iItemPackage_Desc2).Focus()
                Exit Sub
            End If
            If Val(txtField(iItemPackage_Desc2).Text) = 0 Then
                sMsg = String.Format(ResourcesIRMA.GetString("NotZero"), ResourcesItemHosting.GetString("PkgDesc2"))
                MsgBox(sMsg, MsgBoxStyle.Critical, Me.Text)
                txtField(iItemPackage_Desc2).Focus()
                Exit Sub
            End If
        End If

        Select Case -1
            Case cmbField(iItemPackage_Unit_ID).SelectedIndex
                sMsg = String.Format(ResourcesIRMA.GetString("Required"), ResourcesItemHosting.GetString("PkgDescUnit"))
                MsgBox(sMsg, MsgBoxStyle.Critical, Me.Text)
                cmbField(iItemPackage_Unit_ID).Focus()
                Exit Sub
            Case cmbField(iItemVendor_Unit_ID).SelectedIndex
                sMsg = String.Format(ResourcesIRMA.GetString("Required"), String.Format(ResourcesItemHosting.GetString("Unit"), lblVendorOrder.Text.Replace(":", "")))
                MsgBox(sMsg, MsgBoxStyle.Critical, Me.Text)
                cmbField(iItemVendor_Unit_ID).Focus()
                Exit Sub
            Case cmbField(iItemDistribution_Unit_ID).SelectedIndex
                sMsg = String.Format(ResourcesIRMA.GetString("Required"), String.Format(ResourcesItemHosting.GetString("Unit"), lblDistribution.Text.Replace(":", "")))
                MsgBox(sMsg, MsgBoxStyle.Critical, Me.Text)
                cmbField(iItemDistribution_Unit_ID).Focus()
                Exit Sub
            Case cmbField(iItemRetail_Unit_ID).SelectedIndex
                sMsg = String.Format(ResourcesIRMA.GetString("Required"), String.Format(ResourcesItemHosting.GetString("Unit"), lblRetail.Text.Replace(":", "")))
                MsgBox(sMsg, MsgBoxStyle.Critical, Me.Text)
                cmbField(iItemRetail_Unit_ID).Focus()
                Exit Sub
        End Select

        '-- SCALE ITEMS ONLY -- determine NumPluDigitsSentToScale; perform validation if necessary
        If isScaleIdentifier Then
            Select Case True
                Case Me.RadioButton_NumScaleDigits_4.Checked
                    iNumScaleDigits = 4
                Case Me.RadioButton_NumScaleDigits_5.Checked
                    iNumScaleDigits = 5
            End Select

            '--VALIDATION: If the user has selected 4 digits and item is going to the scale, IRMA performs an additional validation 
            '--first on the scale department selected, then on the four PLU digits (2xXXXX00000)
            Do Until bIdentifierOK
                If Me.RadioButton_NumScaleDigits_4.Checked AndAlso _
                    (Me.GroupBox_SendToScale.Visible = False Or (Me.GroupBox_SendToScale.Visible AndAlso Not Me.RadioButton_SendToScale_No.Checked)) Then
                    'get any existing items with the same ScalePLU for the same SubTeam.ScaleDept for the selected subteam
                    Dim pluConflicts As ArrayList = ScaleDAO.GetScalePluItemConflicts(psIdentifier, iNumScaleDigits, ComboValue(Me.HierarchySelector1.cmbSubTeam))

                    If pluConflicts.Count > 0 Then
                        'display conflict error message form
                        Dim errorForm As New ItemAdd_ScalePluConflict
                        errorForm.IsShowOption_DoNotSendToScale = isShowOption_DoNotSendToScale
                        errorForm.PluConflicts = pluConflicts
                        If iNumScaleDigits = 5 Then
                            errorForm.RadioButton_Send5Digits.Enabled = False
                        End If
                        errorForm.ShowDialog()

                        Dim conflictOption As Integer = errorForm.SelectedOption

                        errorForm.Dispose()

                        'handle conflict options
                        '(1) Cancel and enter a new PLU  
                        '(2) Keep this PLU but do not send it to the scales 
                        '(3) Send 5 digits for this PLU  
                        Select Case conflictOption
                            Case 1
                                'exit form and return to ItemIdentifierAdd screen
                                skipSave = True
                                keepItemIdentiferAddOpen = True
                                bIdentifierOK = True
                                Exit Do
                            Case 2
                                Me.RadioButton_SendToScale_No.Checked = True
                                bIdentifierOK = True
                                Exit Do
                            Case 3
                                Me.RadioButton_NumScaleDigits_4.Checked = False
                                Me.RadioButton_NumScaleDigits_5.Checked = True
                                iNumScaleDigits = 5

                        End Select
                    Else
                        bIdentifierOK = True
                        Exit Do
                    End If
                Else
                    'Check for PLU conflicts for send to scale items
                    If Me.RadioButton_SendToScale_Yes.Checked Then
                        Dim pluConflicts As ArrayList = ScaleDAO.GetScalePluItemConflicts(psIdentifier, iNumScaleDigits, ComboValue(Me.HierarchySelector1.cmbSubTeam))

                        If pluConflicts.Count > 0 Then

                            'display conflict error message form
                            Dim errorForm As New ItemAdd_ScalePluConflict
                            errorForm.IsShowOption_DoNotSendToScale = isShowOption_DoNotSendToScale
                            errorForm.PluConflicts = pluConflicts
                            If iNumScaleDigits = 5 Then
                                errorForm.RadioButton_Send5Digits.Enabled = False
                            End If
                            errorForm.ShowDialog()

                            Dim conflictOption As Integer = errorForm.SelectedOption

                            errorForm.Dispose()

                            Select Case conflictOption
                                Case 1
                                    'exit form and return to ItemIdentifierAdd screen
                                    skipSave = True
                                    keepItemIdentiferAddOpen = True
                                    bIdentifierOK = True
                                    Exit Do
                                Case 2
                                    Me.RadioButton_SendToScale_No.Checked = True
                                    bIdentifierOK = True
                                    Exit Do
                                Case 3
                                    Me.RadioButton_NumScaleDigits_4.Checked = False
                                    Me.RadioButton_NumScaleDigits_5.Checked = True
                                    iNumScaleDigits = 5
                            End Select
                        Else
                            bIdentifierOK = True
                            Exit Do
                        End If
                    Else
                        bIdentifierOK = True
                        Exit Do
                    End If
                End If
            Loop
        End If

        If Not skipSave Then
            Try
                '-- Add the new record
                gRSRecordset = SQLOpenRecordSet("EXEC InsertItem '" & Replace(ConvertQuotes(Trim(UCase(txtField(iItemPOS_Description).Text))), ",", "", , , CompareMethod.Text) & "', '" & _
                                                                        ConvertQuotes(Trim(txtField(iItemItem_Description).Text)) & "', " & _
                                                                        ComboValue((Me.HierarchySelector1.cmbSubTeam)) & ", " & _
                                                                        ComboValue((Me.HierarchySelector1.cmbCategory)) & ", " & _
                                                                        IIf(cmbField(iItemRetail_Unit_ID).SelectedIndex = -1, "null", VB6.GetItemData(cmbField(iItemRetail_Unit_ID), cmbField(iItemRetail_Unit_ID).SelectedIndex)) & ", " & _
                                                                        IIf(cmbField(iItemPackage_Unit_ID).SelectedIndex = -1, "null", VB6.GetItemData(cmbField(iItemPackage_Unit_ID), cmbField(iItemPackage_Unit_ID).SelectedIndex)) & ", " & _
                                                                        txtField(iItemPackage_Desc1).Text & ", " & _
                                                                        txtField(iItemPackage_Desc2).Text & ", " & _
                                                                        "'" & Me.psIdentifierType & _
                                                                        "','" & Me.psIdentifier & "','" & _
                                                                        Me.psCheckDigit & _
                                                                        "'," & giUserID & _
                                                                        "," & chkRetailSale.CheckState & _
                                                                        ", " & ComboValue(cmbField(iItemNatClassID)) & _
                                                                        "," & chkCostedByWeight.CheckState & "," & _
                                                                        IIf(cmbField(iItemVendor_Unit_ID).SelectedIndex = -1, "null", VB6.GetItemData(cmbField(iItemVendor_Unit_ID), cmbField(iItemVendor_Unit_ID).SelectedIndex)) & ", " & _
                                                                        IIf(cmbField(iItemDistribution_Unit_ID).SelectedIndex = -1, "null", VB6.GetItemData(cmbField(iItemDistribution_Unit_ID), cmbField(iItemDistribution_Unit_ID).SelectedIndex)) & ", " & _
                                                                        IIf(cmbTaxClass.Text = "", "null", cmbTaxClass.SelectedValue) & ", " & _
                                                                        IIf(cmbLabelType.Text = "", "null", cmbLabelType.SelectedValue) & ", " & _
                                                                        IIf(ComboBox_Brand.Text = "", "null", ComboBox_Brand.SelectedValue) & ", " & _
                                                                        Me.piNationalID & "," & _
                                                                        IIf(iNumScaleDigits <= 0, "null", iNumScaleDigits) & "," & _
                                                                        IIf(Me.GroupBox_SendToScale.Visible, Not Me.RadioButton_SendToScale_No.Checked, isScaleIdentifier) & "," & _
                                                                        IIf(ComboBoxJurisdiction.Text = "", "null", ComboBoxJurisdiction.SelectedValue) & ", " & _
                                                                        ComboValue((Me.HierarchySelector1.cmbLevel4)) & ", " & _
                                                                        "'IRMA Client'," & _
                                                                        chkFoodStamps.CheckState & "," & _
                                                                        chkOrganic.CheckState & "," & _
                                                                        IIf(cmbManagedBy.Text = "", "null", cmbManagedBy.SelectedValue), _
                                                                        DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                glItemID = gRSRecordset.Fields("Item_Key").Value
            Catch ex As Exception
                logger.Error("cmdAdd_Click exception", ex)
            Finally
                If gRSRecordset IsNot Nothing Then
                    gRSRecordset.Close()
                    gRSRecordset = Nothing
                End If
            End Try
        End If

        '-- Go back to the previous form
        Me.Close()
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub frmItemAdd_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Me.psIdentifier = String.Empty
    End Sub

    Private Sub txtField_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.Enter
        Dim Index As Short = txtField.GetIndex(eventSender)

        '-- highlight the string
        If (Not txtField(Index).ReadOnly) And (txtField(Index).Enabled) And (txtField(Index).Visible) Then HighlightText(txtField(Index))
    End Sub

    Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress
        Dim KeyAscii As Short = Asc(EventArgs.KeyChar)
        Dim Index As Short = txtField.GetIndex(eventSender)

        If KeyAscii = 13 Then
            KeyAscii = 0
            System.Windows.Forms.SendKeys.Send("{TAB}")
        Else
            If (Not txtField(Index).ReadOnly) And (txtField(Index).Enabled) Then
                '-- Restrict key presses to that type of field
                KeyAscii = ValidateKeyPressEvent(KeyAscii, txtField(Index).Tag, txtField(Index), 0, 0, 0)
                If Index = iItemPOS_Description Then
                    KeyAscii = Asc(UCase(Chr(KeyAscii)))
                End If
            End If
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cmbField_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbField.SelectedIndexChanged
        If Me.IsInitializing Then Exit Sub

        Dim Index As Short = cmbField.GetIndex(sender)

        If (Index = iItemPackage_Unit_ID) And (chkCostedByWeight.CheckState = System.Windows.Forms.CheckState.Checked) Then

            ' If SelectedIndex is -1, the GetItemData routine bombs - avoid 
            If cmbField(iItemPackage_Unit_ID).SelectedIndex > -1 Then
                cmbField(iItemRetail_Unit_ID).SelectedIndex = cmbField(iItemPackage_Unit_ID).SelectedIndex
            End If

        End If
    End Sub

    Private Sub cmbTaxClass_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbTaxClass.KeyPress
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then cmbTaxClass.SelectedIndex = -1

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then e.Handled = True
    End Sub

    Private Sub ComboBox_Brand_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ComboBox_Brand.KeyPress
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then ComboBox_Brand.SelectedIndex = -1

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then e.Handled = True
    End Sub

    Private Sub Button_ViewTaxFlags_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_ViewTaxFlags.Click
        If Me.cmbTaxClass.SelectedValue <= 0 Then
            MessageBox.Show(String.Format(ResourcesTaxHosting.GetString("msg_selectTaxClass"), ResourcesTaxHosting.GetString("label_header_taxClass")), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            Dim taxFlagForm As New Form_ViewTaxFlags
            taxFlagForm.TaxClassID = Me.cmbTaxClass.SelectedValue
            taxFlagForm.ShowDialog(Me)
            taxFlagForm.Close()
            taxFlagForm.Dispose()
        End If
    End Sub

    Private Sub AdjustWidthComboBox_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles _cmbField_15.DropDown
        Dim senderComboBox As ComboBox = CType(sender, ComboBox)
        Dim width As Integer = senderComboBox.DropDownWidth
        Dim g As Graphics = senderComboBox.CreateGraphics()
        Dim font As Font = senderComboBox.Font
        Dim vertScrollBarWidth As Integer = CType(IIf((senderComboBox.Items.Count > senderComboBox.MaxDropDownItems), SystemInformation.VerticalScrollBarWidth, 0), Integer)

        Dim newWidth As Integer
        Dim itemEnum As IEnumerator = CType(sender, ComboBox).Items.GetEnumerator
        Dim currentItem As String

        While itemEnum.MoveNext
            currentItem = CType(itemEnum.Current, Microsoft.VisualBasic.Compatibility.VB6.ListBoxItem).ItemString
            newWidth = CType(g.MeasureString(currentItem, font).Width, Integer) + vertScrollBarWidth

            If width < newWidth Then
                width = newWidth
            End If
        End While

        senderComboBox.DropDownWidth = width
    End Sub

    Private Sub AddBrandButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddBrandButton.Click
        Dim lLoop As Integer
        Dim lMax As Integer
        Dim lMaxValue As Integer

        glBrandID = 0
        frmBrandAdd.ShowDialog()
        frmBrandAdd.Close()
        frmBrandAdd.Dispose()

        If glBrandID = -2 Then
            ComboBox_Brand.DataSource = Nothing
            ComboBox_Brand.Items.Clear()
            ComboBox_Brand.DataSource = GetBrandData()

            ComboBox_Brand.DisplayMember = "Brand_Name"
            ComboBox_Brand.ValueMember = "Brand_ID"

            For lLoop = 0 To ComboBox_Brand.Items.Count - 1
                ComboBox_Brand.SelectedIndex = lLoop
                If ComboBox_Brand.SelectedValue > lMaxValue Then
                    lMaxValue = ComboBox_Brand.SelectedValue
                    lMax = lLoop
                End If
            Next lLoop
            ComboBox_Brand.SelectedIndex = lMax

        End If

    End Sub

    Private Sub SetAttributeValues()

        Dim itemAttributeDefaultsBO As New ItemAttributeDefaultsBO()

        ' set the hierarchy level
        ' for regions with a two level hierarchy the ProdHierarchyLevel4_ID will be ignored
        ' for regions with a four level hierarchy the Category_ID will be ignored
        itemAttributeDefaultsBO.Category_ID = Me.HierarchySelector1.SelectedCategoryId
        itemAttributeDefaultsBO.ProdHierarchyLevel4_ID = Me.HierarchySelector1.SelectedLevel4Id

        ' get the default values for defaultable fields on this form
        itemAttributeDefaultsBO.SetAttributeDefaults()
        SetTaxClass(cmbTaxClass, itemAttributeDefaultsBO.TaxClassID)
        chkCostedByWeight.Checked = itemAttributeDefaultsBO.CostedByWeight
        chkRetailSale.Checked = itemAttributeDefaultsBO.Retail_Sale
        SetLabelType(cmbLabelType, itemAttributeDefaultsBO.LabelType_ID)
        cmbManagedBy.SelectedValue = itemAttributeDefaultsBO.ManagedBy

    End Sub

    Sub SetTaxClass(ByRef cmbTaxClass As System.Windows.Forms.ComboBox, ByRef iValue As Integer)

        If iValue = -1 Then
            cmbTaxClass.SelectedIndex = -1
        Else
            Dim iLoop As Integer

            For iLoop = 0 To cmbTaxClass.Items.Count - 1
                '-- See if its the right data
                If CType(cmbTaxClass.Items(iLoop), DataRowView).Row(0) = iValue Then
                    '-- if so then set and exit
                    cmbTaxClass.SelectedIndex = iLoop
                    Exit For
                End If
            Next iLoop
        End If
    End Sub

    Sub SetLabelType(ByRef cmbLabelType As System.Windows.Forms.ComboBox, ByRef iValue As Integer)

        If iValue = -1 Then
            cmbLabelType.SelectedIndex = -1
        Else
            Dim iLoop As Integer

            For iLoop = 0 To cmbLabelType.Items.Count - 1
                '-- See if its the right data
                If CType(cmbLabelType.Items(iLoop), LabelTypeBO).LabelTypeID = iValue Then
                    '-- if so then set and exit
                    cmbLabelType.SelectedIndex = iLoop
                    Exit For
                End If
            Next iLoop
        End If
    End Sub

    Private Sub HierarchySelector1_CategoryChanged() Handles HierarchySelector1.CategoryChanged

        ' only trigger setting defaults on category change if the
        ' region uses the two level hierarchy
        If Not HierarchySelector1.UsesFourLevelHierarchy Then
            SetAttributeValues()
        End If

    End Sub

    Private Sub HierarchySelector1_Level4Changed() Handles HierarchySelector1.Level4Changed

        SetAttributeValues()

    End Sub

End Class