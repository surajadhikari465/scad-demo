Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.Common.DataAccess
Imports log4net

Public Class DefaultAttributeValues

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


#Region "Constant, Field, Enum, and Property Definitions"

    Private Const TEXTBOX_WIDTH As Integer = 150
    Private Const DROPDOWN_WIDTH As Integer = 150

    Public Enum SaveOrDeleteActions
        Save
        Delete
    End Enum

    Private _attributeControls As Hashtable = New Hashtable
    Private _itemDefaultAttributes As ArrayList
    Private _attributeValueChanged As Boolean = False
    Private _canDelete As Boolean = False
    Private _resetingHierarchyToPreviousValues As Boolean = False

    Public Property AttributeValueChanged() As Boolean
        Get
            Return _attributeValueChanged
        End Get
        Set(ByVal value As Boolean)
            _attributeValueChanged = value

            Me.cmdSave.Enabled = value

        End Set
    End Property

    Public Property CanDelete() As Boolean
        Get
            Return _canDelete
        End Get
        Set(ByVal value As Boolean)
            _canDelete = value

            Me.cmdDelete.Enabled = value

        End Set
    End Property

#End Region

#Region "Private Methods"

    Private Sub DefaultAttributeValues_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        logger.Debug("DefaultAttributeValues_Load Entry")

        BuildAttributeControls()

        logger.Debug("DefaultAttributeValues_Load Exit")

    End Sub

    Private Sub CreateLabel(ByVal itemDefaultAttribute As ItemDefaultAttributeBO, ByVal yPos As Integer)

        logger.Debug("CreateLabel Entry")

        Dim attributeControlLabel As Label = New Label()
        attributeControlLabel.AutoSize = True
        attributeControlLabel.Location = New System.Drawing.Point(6, yPos + 3)
        attributeControlLabel.Text = itemDefaultAttribute.AttributeName

        attributeControlLabel.Size = New System.Drawing.Size(39, 13)

        attributeControlLabel.Name = itemDefaultAttribute.AttributeField
        Me.panelAttributeControls.Controls.Add(attributeControlLabel)

        logger.Debug("CreateLabel Exit")

    End Sub

    Private Sub ClearAttributeControls()

        logger.Debug("ClearAttributeControls Entry")

        ' clear the table of controls built for the previously selected hierarchy position
        _attributeControls = New Hashtable

        ' remove the previous controls from the parent groupbox
        Me.panelAttributeControls.Controls.Clear()

        ' reset the changed flag
        AttributeValueChanged = False

        ' reset the delete flag
        CanDelete = False

        logger.Debug("ClearAttributeControls Exit")

    End Sub

    Private Sub BuildAttributeControls()

        logger.Debug("BuildAttributeControls Entry")


        If Not _resetingHierarchyToPreviousValues Then
            ' check if there are changes to save and if the user wants to save them or not or cancel the build
            Dim checkForChangeDialogResult As DialogResult = CheckForChange()

            If checkForChangeDialogResult = Windows.Forms.DialogResult.Cancel Then

                ' revert the hierarchy control's combobox selections back to what they were
                ' before they changed and triggered the execution of this method

                ' set this to keep this method from being called again when the hierarchy
                ' combo selections are reset
                _resetingHierarchyToPreviousValues = True

                Me.HierarchySelector1.ResetToPreviousIds()
                

                _resetingHierarchyToPreviousValues = False

            Else

                ' user wants to save changes first
                If checkForChangeDialogResult = Windows.Forms.DialogResult.Yes Then

                    SaveOrDeleteDefaultAttributeValues(SaveOrDeleteActions.Save)

                End If

                Dim localCanDelete As Boolean = False
                Dim attributeControl As Control = Nothing
                Dim tabIndex As Integer = 0
                Dim yPos As Integer = 19

                Dim usesFourLevelHierarchy As Boolean = InstanceDataDAO.IsFlagActive("FourLevelHierarchy")
                Dim selectedCategoryId As Integer = Me.HierarchySelector1.SelectedCategoryId
                Dim selectedLevel4Id As Integer = Me.HierarchySelector1.SelectedLevel4Id

                ClearAttributeControls()

                ' check to see if the combobox for the lowest level of the hierarchy has a value selected
                ' for regions using four levels this is the level 4 combobox, for all others it is the class (category) combobox
                Dim isHierarchyPositionSelected As Boolean = ((Not usesFourLevelHierarchy) And selectedCategoryId >= 1) Or _
                    (usesFourLevelHierarchy And selectedLevel4Id >= 1)

                If isHierarchyPositionSelected Then

                    Try

                        ' this is reset to default in the Finally block below
                        Me.Cursor = Cursors.WaitCursor

                        _itemDefaultAttributes = ItemDefaultAttributeDAO.GetItemDefaultAttributes _
                            (Me.HierarchySelector1.SelectedLevel4Id, Me.HierarchySelector1.SelectedCategoryId)

                        Dim itemDefaultAttribute As ItemDefaultAttributeBO
                        For Each itemDefaultAttribute In _itemDefaultAttributes
                            Select Case itemDefaultAttribute.ControlType
                                Case 1 ' Text field

                                    ' Create Label
                                    CreateLabel(itemDefaultAttribute, yPos)

                                    ' Create TextBox
                                    attributeControl = New TextBox()
                                    attributeControl.Location = New System.Drawing.Point(127, yPos)
                                    yPos = yPos + 26

                                    attributeControl.Name = itemDefaultAttribute.AttributeName
                                    attributeControl.Size = New System.Drawing.Size(TEXTBOX_WIDTH, 20)
                                    attributeControl.TabIndex = tabIndex

                                    attributeControl.Text = itemDefaultAttribute.Value

                                    AddHandler attributeControl.TextChanged, AddressOf HandleAttributeValueChanged

                                Case 2 ' Dropdown

                                    ' Create Label
                                    CreateLabel(itemDefaultAttribute, yPos)

                                    ' Create ComboBox
                                    attributeControl = New ComboBox()
                                    CType(attributeControl, ComboBox).DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
                                    attributeControl.Location = New System.Drawing.Point(127, yPos)
                                    yPos = yPos + 27

                                    attributeControl.Name = itemDefaultAttribute.AttributeName
                                    attributeControl.Size = New System.Drawing.Size(DROPDOWN_WIDTH, 20)
                                    attributeControl.TabIndex = tabIndex

                                    ItemDefaultAttributeDAO.RunPopulateProcedure(itemDefaultAttribute.PopulateProcedure, _
                                        itemDefaultAttribute.IndexField, itemDefaultAttribute.DescriptionField, CType(attributeControl, ComboBox))

                                    ' use a local var for value because SetCombo takes the value by ref and changes Nothings into empty strings
                                    Dim localValue As String = itemDefaultAttribute.Value
                                    SetCombo(CType(attributeControl, ComboBox), localValue)

                                    AddHandler CType(attributeControl, ComboBox).SelectedIndexChanged, AddressOf HandleAttributeValueChanged

                                Case 3 ' Checkbox

                                    ' Create Label
                                    CreateLabel(itemDefaultAttribute, yPos)

                                    ' Create ComboBox
                                    attributeControl = New CheckBox()
                                    attributeControl.AutoSize = True
                                    attributeControl.Size = New System.Drawing.Size(81, yPos - 3)

                                    attributeControl.Location = New System.Drawing.Point(127, yPos)
                                    yPos = yPos + 23

                                    attributeControl.TabIndex = tabIndex

                                    CType(attributeControl, CheckBox).UseVisualStyleBackColor = True

                                    If Not IsNothing(itemDefaultAttribute.Value) Then
                                        CType(attributeControl, CheckBox).Checked = itemDefaultAttribute.Value.Equals("True")
                                    End If

                                    AddHandler CType(attributeControl, CheckBox).CheckedChanged, AddressOf HandleAttributeValueChanged

                            End Select

                            If Not IsNothing(attributeControl) Then
                                attributeControl.Name = itemDefaultAttribute.AttributeField
                                Me.panelAttributeControls.Controls.Add(attributeControl)
                                _attributeControls.Add(itemDefaultAttribute.AttributeField, attributeControl)

                                tabIndex = tabIndex + 1

                                ' do this to manage the enabled state of the delete button
                                If Not (itemDefaultAttribute.Value Is Nothing) Then
                                    localCanDelete = True
                                End If

                            End If

                        Next

                        CanDelete = localCanDelete

                    Catch ex As Exception
                        Dim message As String = ex.Message

                        If Not IsNothing(ex.InnerException) Then
                            message = ex.InnerException.Message
                        End If
                        MessageBox.Show("The following error has occured. Please report this to the IRMA support team." + _
                            ControlChars.NewLine + ControlChars.NewLine + message)

                        logger.Info("The following error has occured. Please report this to the IRMA support team." + _
                            ControlChars.NewLine + ControlChars.NewLine + message)

                    Finally

                        Me.Cursor = Cursors.Default

                    End Try

                End If
            End If
            End If

        logger.Debug("BuildAttributeControls Exit")
    End Sub

    Private Sub SaveOrDeleteDefaultAttributeValues(ByVal saveOrDeleteAction As SaveOrDeleteActions)

        logger.Debug("SaveOrDeleteDefaultAttributeValues Entry")

        Dim itemDefaultValue As ItemDefaultValueBO
        Dim currentControl As Control

        Dim prodHierarchyLevel4ID As Integer = Me.HierarchySelector1.SelectedLevel4Id
        Dim categoryId As Integer = Me.HierarchySelector1.SelectedCategoryId

        Try
            ' this is reset to default in the Finally block below
            Me.Cursor = Cursors.WaitCursor

            Dim itemDefaultAttribute As ItemDefaultAttributeBO
            For Each itemDefaultAttribute In _itemDefaultAttributes

                itemDefaultValue = New ItemDefaultValueBO()

                itemDefaultValue.ItemDefaultAttributeID = itemDefaultAttribute.ID

                If InstanceDataDAO.IsFlagActive("FourLevelHierarchy") Then
                    itemDefaultValue.ProdHierarchyLevel4ID = Me.HierarchySelector1.SelectedLevel4Id
                Else
                    itemDefaultValue.CategoryID = Me.HierarchySelector1.SelectedCategoryId
                End If

                currentControl = CType(_attributeControls.Item(itemDefaultAttribute.AttributeField), Control)

                ' Get the control's value in the way dictated by the type of control.
                Select Case itemDefaultAttribute.ControlType
                    Case 1 ' Text field

                        itemDefaultValue.Value = CType(currentControl, TextBox).Text

                    Case 2 ' Dropdown

                        Dim comboBoxControl As ComboBox = CType(currentControl, ComboBox)

                        If comboBoxControl.SelectedIndex > -1 Then
                            itemDefaultValue.Value = CStr(VB6.GetItemData(comboBoxControl, comboBoxControl.SelectedIndex))
                        Else
                            ' there is nothing selected in the combo so there is nothing to save
                            Continue For
                        End If

                    Case 3 ' Checkbox

                        itemDefaultValue.Value = CType(CType(currentControl, CheckBox).Checked, String)

                End Select

                If saveOrDeleteAction = SaveOrDeleteActions.Save Then
                    ItemDefaultValueDAO.SaveItemDefaultValues(itemDefaultValue.ItemDefaultAttributeID, _
                        itemDefaultValue.ProdHierarchyLevel4ID, itemDefaultValue.CategoryID, itemDefaultValue.Value)

                    ' set the can delete flag to enable the delete button
                    CanDelete = True

                Else
                    ItemDefaultValueDAO.DeleteItemDefaultValues(itemDefaultAttribute.ID, _
                        itemDefaultValue.ProdHierarchyLevel4ID, itemDefaultValue.CategoryID, itemDefaultValue.Value)

                    ' clear the can delete flag to disable the delete button
                    CanDelete = False

                    ' reuild the controls as a simple way to clear their values
                    BuildAttributeControls()

                End If

            Next

            ' reset the changed flag
            AttributeValueChanged = False

        Catch ex As Exception
            ' reset the changed flag
            AttributeValueChanged = False

        Finally

            Me.Cursor = Cursors.Default

        End Try

        logger.Debug("SaveOrDeleteDefaultAttributeValues Exit")

    End Sub

    Private Function CheckForChange() As DialogResult

        logger.Debug("CheckForChange Entry")


        Dim aDialogResult As DialogResult = Windows.Forms.DialogResult.None

        If AttributeValueChanged() Then
            If SaveIsOkay() Then
                aDialogResult = MessageBox.Show("Save your Changes?", "Default Attribute Values", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
            End If
        End If

        logger.Debug("CheckForChange Exit")

        Return aDialogResult

    End Function

    Private Function SaveIsOkay() As Boolean
        ' a Dirty Hack(tm) to prevent regions using Level 4 hierarchy from saving when a
        ' level 4 value hasn't been selected yet.
        If InstanceDataDAO.IsFlagActive("FourLevelHierarchy") Then
            If (HierarchySelector1.SelectedLevel4Id = 0) Then
                Return False
            End If
        End If

        Return True

    End Function

#End Region

#Region "Event Handlers"

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        logger.Debug("cmdSave_Click Entry")

        ' a Dirty Hack(tm) to prevent regions using Level 4 hierarchy from saving when a
        ' level 4 value hasn't been selected yet.
        If InstanceDataDAO.IsFlagActive("FourLevelHierarchy") Then
            If (HierarchySelector1.SelectedLevel4Id = 0) Then
                MessageBox.Show("You must select a Level 4 value to save!", "Oops!", MessageBoxButtons.OK)
                Exit Sub
            End If
        End If

        SaveOrDeleteDefaultAttributeValues(SaveOrDeleteActions.Save)

        logger.Debug("cmdSave_Click Exit")

    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

        logger.Debug("cmdDelete_Click Entry")

        If MessageBox.Show("Delete the set of default values" + ControlChars.NewLine + "for this position in the hierarchy?", "Default Attribute Values", MessageBoxButtons.YesNo, _
            MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            SaveOrDeleteDefaultAttributeValues(SaveOrDeleteActions.Delete)
        End If

        logger.Debug("cmdDelete_Click Exit")

    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click

        logger.Debug("cmdExit_Click Entry")

        Dim checkForChangeDialogResult As DialogResult = CheckForChange()

        If checkForChangeDialogResult <> Windows.Forms.DialogResult.Cancel Then

            If checkForChangeDialogResult = Windows.Forms.DialogResult.Yes Then
                SaveOrDeleteDefaultAttributeValues(SaveOrDeleteActions.Save)
            End If

            Me.Close()

        End If

        logger.Debug("cmdExit_Click Exit")


    End Sub

    Private Sub HierarchySelector1_AddHierarchyNode(ByRef e As CancelableEventArgs) Handles HierarchySelector1.AddHierarchyNode


        logger.Debug("HierarchySelector1_AddHierarchyNode Entry")

        Dim checkForChangeDialogResult As DialogResult = CheckForChange()

        e.Cancel = (checkForChangeDialogResult = Windows.Forms.DialogResult.Cancel)

        If Not e.Cancel Then
            If checkForChangeDialogResult = Windows.Forms.DialogResult.Yes Then
                SaveOrDeleteDefaultAttributeValues(SaveOrDeleteActions.Save)
            Else
                AttributeValueChanged = False
                ' Rebuild controls to undo any changes
                BuildAttributeControls()
            End If
        End If

        logger.Debug("HierarchySelector1_AddHierarchyNode Exit")
    End Sub

    Private Sub HandleAttributeValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        AttributeValueChanged = True

    End Sub


    Private Sub HierarchySelector1_HierarchySelectionChanged() Handles HierarchySelector1.HierarchySelectionChanged

        logger.Debug("HierarchySelector1_HierarchySelectionChanged Entry")
        BuildAttributeControls()
        logger.Debug("HierarchySelector1_HierarchySelectionChanged Exit")
    End Sub

#End Region

End Class
