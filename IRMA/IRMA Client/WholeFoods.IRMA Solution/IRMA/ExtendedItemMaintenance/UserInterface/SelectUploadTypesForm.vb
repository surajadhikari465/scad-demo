Imports System.Collections
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.IRMA.ModelLayer.DataAccess
Imports WholeFoods.IRMA.ExtendedItemMaintenance.Logic

''' <summary>
''' Lets user select any combination of upload types.
''' 
''' Created By:    David Marine
''' Created:       Feb 09, 2007
''' </summary>
''' <remarks></remarks>
Public Class SelectUploadTypesForm

#Region "Fields and Properties"

    Private _currentUploadSession As UploadSession
    Private _sessionAction As SessionActions = SessionActions.SaveNew
    Private _templateListsEnabled As Boolean = True
    Private _isInitializing As Boolean = True

    Public Property CurrentUploadSession() As UploadSession
        Get
            Return _currentUploadSession
        End Get
        Set(ByVal value As UploadSession)

            If Not IsNothing(value) Then

                _currentUploadSession = value

                Me.SessionName = _currentUploadSession.Name
                Me.CheckBoxNewItemSession.Checked = _currentUploadSession.IsNewItemSessionFlag
                Me.CheckBoxDeleteItemSession.Checked = _currentUploadSession.IsDeleteItemSessionFlag

                _isInitializing = True

                SetUploadTypeCheckBox(EIM_Constants.ITEM_MAINTENANCE_CODE, Me.CheckBoxItemMaintenance)
                SetUploadTypeCheckBox(EIM_Constants.PRICE_UPLOAD_CODE, Me.CheckBoxPriceUpload)
                SetUploadTypeCheckBox(EIM_Constants.COST_UPLOAD_CODE, Me.CheckBoxCostUpload)

                If Me.CurrentUploadSession.IsFromSLIM Then
                    Me.CheckBoxNewItemSession.Checked = True
                    Me.CheckBoxNewItemSession.Enabled = False
                End If

                _isInitializing = False

            End If
        End Set
    End Property

    Public Property SessionAction() As SessionActions
        Get
            Return _sessionAction
        End Get
        Set(ByVal value As SessionActions)
            _sessionAction = value

            Me.TextBoxSessionName.Enabled = False
            Me.GroupBoxUploadTypesAndTemplates.Visible = True
            Me.Height = 407

            Select Case _sessionAction
                Case SessionActions.SaveNew
                    Me.TextBoxSessionName.Enabled = True
                    Me.TemplateListsEnabled = False
                    Me.Text = "Save your New Session"
                    Me.TextBoxNotes.Text = "This will save your new session to the database so you can load and resume your work later."
                    Me.ButtonOK.Text = "Save"
                    Me.GroupBoxUploadTypesAndTemplates.Visible = False
                    Me.CheckBoxNewItemSession.Enabled = False
                    Me.CheckBoxDeleteItemSession.Enabled = False
                    Me.Height = 233
                Case SessionActions.SaveExisting
                    Me.TextBoxSessionName.Enabled = True
                    Me.TemplateListsEnabled = False
                    Me.Text = "Save your Existing Session"
                    Me.TextBoxNotes.Text = "This will save your existing session to the database so you can load and resume your work on it later." + _
                        ControlChars.NewLine + ControlChars.NewLine + "Warning: This will overwrite any existing data in the database for this session."
                    Me.ButtonOK.Text = "Save"
                    Me.GroupBoxUploadTypesAndTemplates.Visible = False
                    Me.CheckBoxNewItemSession.Enabled = False
                    Me.CheckBoxDeleteItemSession.Enabled = False
                    Me.Height = 233
                Case SessionActions.LoadItems
                    Me.TextBoxSessionName.Text = ""
                    Me.Text = "Load Items into a new Session"
                    Me.TextBoxNotes.Text = "This will load items into a new session." + _
                        ControlChars.NewLine + ControlChars.NewLine + " You may then save it to the database or export it to a spreadsheet " + _
                        "so you can load or import and resume your work later."
                    Me.ButtonOK.Text = "Load"

                    Me.CheckBoxNewItemSession.Checked = False
                    Me.CheckBoxNewItemSession.Enabled = False
                    Me.CheckBoxDeleteItemSession.Enabled = True
                    Me.TemplateListsEnabled = True
                Case SessionActions.Import
                    Me.TextBoxSessionName.Text = ""
                    Me.Text = "Import from a Spreadsheet"
                    Me.TextBoxNotes.Text = "This will import items into a new session." + _
                        ControlChars.NewLine + ControlChars.NewLine + " You may then save it to the database " + _
                        "so you can load or import and resume your work on it later."
                    Me.ButtonOK.Text = "Import"
                    Me.CheckBoxNewItemSession.Enabled = True
                    Me.CheckBoxDeleteItemSession.Enabled = True
                    Me.TemplateListsEnabled = True
                Case SessionActions.Export
                    Me.Text = "Export to a Spreadsheet"
                    Me.TextBoxNotes.Text = "This will export your session to a spreadsheet so you can import and resume your work later."
                    Me.ButtonOK.Text = "Export"
                    Me.CheckBoxNewItemSession.Enabled = False
                    Me.CheckBoxDeleteItemSession.Enabled = False
                    Me.TemplateListsEnabled = True
            End Select

        End Set
    End Property

    Public Property SessionName() As String
        Get
            Return Me.TextBoxSessionName.Text
        End Get
        Set(ByVal value As String)
            Me.TextBoxSessionName.Text = value
        End Set
    End Property

    Public Property TemplateListsEnabled() As Boolean
        Get
            Return _templateListsEnabled
        End Get
        Set(ByVal value As Boolean)
            _templateListsEnabled = value


            If _templateListsEnabled Then

                _isInitializing = True

                SetTemplateComboBoxDataSources(EIM_Constants.ITEM_MAINTENANCE_CODE, Me.ComboBoxItemMaintenanceTemplates)
                SetTemplateComboBoxDataSources(EIM_Constants.PRICE_UPLOAD_CODE, Me.ComboBoxPriceUploadTemplates)
                SetTemplateComboBoxDataSources(EIM_Constants.COST_UPLOAD_CODE, Me.ComboBoxCostUploadTemplates)

                _isInitializing = False

            End If

            Me.ComboBoxItemMaintenanceTemplates.Enabled = _templateListsEnabled And Me.CheckBoxItemMaintenance.Enabled
            Me.ComboBoxPriceUploadTemplates.Enabled = _templateListsEnabled And Me.CheckBoxItemMaintenance.Enabled
            Me.ComboBoxCostUploadTemplates.Enabled = _templateListsEnabled And Me.CheckBoxItemMaintenance.Enabled

        End Set
    End Property

#End Region

#Region "Event Handlers"

    Private Sub SelectUploadTypesForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        _isInitializing = True

        Me.ComboBoxItemMaintenanceTemplates.SelectedIndex = 0
        Me.ComboBoxPriceUploadTemplates.SelectedIndex = 0
        Me.ComboBoxCostUploadTemplates.SelectedIndex = 0

        _isInitializing = False

    End Sub

    Private Sub CheckBoxItemMaintenance_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxItemMaintenance.CheckedChanged

        ' only allow a template to be selected if the user has selected
        ' the corresponding UploadType
        Me.ComboBoxItemMaintenanceTemplates.Enabled = Me.TemplateListsEnabled And Me.CheckBoxItemMaintenance.Checked

        SetUploadTypeByUsersSelection(EIM_Constants.ITEM_MAINTENANCE_CODE, Me.CheckBoxItemMaintenance)

        ManageUIAppearance()

    End Sub

    Private Sub CheckBoxPriceUpload_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxPriceUpload.CheckedChanged

        ' only allow a template to be selected if the user has selected
        ' the corresponding UploadType
        Me.ComboBoxPriceUploadTemplates.Enabled = Me.TemplateListsEnabled And Me.CheckBoxPriceUpload.Checked

        SetUploadTypeByUsersSelection(EIM_Constants.PRICE_UPLOAD_CODE, Me.CheckBoxPriceUpload)

        ManageUIAppearance()

    End Sub

    Private Sub CheckBoxCostUpload_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxCostUpload.CheckedChanged

        ' only allow a template to be selected if the user has selected
        ' the corresponding UploadType
        Me.ComboBoxCostUploadTemplates.Enabled = Me.TemplateListsEnabled And Me.CheckBoxCostUpload.Checked

        SetUploadTypeByUsersSelection(EIM_Constants.COST_UPLOAD_CODE, Me.CheckBoxCostUpload)

        ManageUIAppearance()

    End Sub

    Private Sub ComboBoxItemMaintenanceTemplates_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxItemMaintenanceTemplates.SelectedIndexChanged

        If _isInitializing Then Exit Sub

        SetUploadSessionUploadTypesSelectedTemplate(EIM_Constants.ITEM_MAINTENANCE_CODE, _
            Me.ComboBoxItemMaintenanceTemplates)

    End Sub

    Private Sub ComboBoxPriceUploadTemplates_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxPriceUploadTemplates.SelectedIndexChanged

        If _isInitializing Then Exit Sub

        SetUploadSessionUploadTypesSelectedTemplate(EIM_Constants.PRICE_UPLOAD_CODE, _
            Me.ComboBoxPriceUploadTemplates)

    End Sub

    Private Sub ComboBoxCostUploadTemplates_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxCostUploadTemplates.SelectedIndexChanged

        If _isInitializing Then Exit Sub

        SetUploadSessionUploadTypesSelectedTemplate(EIM_Constants.COST_UPLOAD_CODE, _
            Me.ComboBoxCostUploadTemplates)

    End Sub

    Private Sub CheckBoxNewItemSession_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxNewItemSession.CheckedChanged

        Me.CurrentUploadSession.IsNewItemSessionFlag = _
            Me.CheckBoxNewItemSession.Checked

    End Sub

    Private Sub CheckBoxDeleteItemSession_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxDeleteItemSession.CheckedChanged

        Me.CurrentUploadSession.IsDeleteItemSessionFlag = _
                    Me.CheckBoxDeleteItemSession.Checked

        If CheckBoxDeleteItemSession.Checked = True Then
            ' If it's a Delete Item session disable other uploads
            Me.ComboBoxPriceUploadTemplates.Enabled = False
            Me.CheckBoxPriceUpload.Checked = False
            Me.CheckBoxPriceUpload.Enabled = False
            Me.ComboBoxCostUploadTemplates.Enabled = False
            Me.CheckBoxCostUpload.Checked = False
            Me.CheckBoxCostUpload.Enabled = False
            Me.CheckBoxNewItemSession.Enabled = False
            Me.CheckBoxNewItemSession.Checked = False
        Else
            Me.ComboBoxPriceUploadTemplates.Enabled = True
            Me.CheckBoxPriceUpload.Enabled = True
            Me.ComboBoxCostUploadTemplates.Enabled = True
            Me.CheckBoxCostUpload.Enabled = True
            Me.CheckBoxNewItemSession.Enabled = True
        End If

    End Sub
#End Region

#Region "Private Methods"

    Private Sub SetTemplateComboBoxDataSources(ByVal inUploadTypeCode As String, ByRef inTemplateComboBox As ComboBox)

        Dim theTemplateCollection As BusinessObjectCollection

        theTemplateCollection = _
                        UploadTypeTemplateDAO.Instance.GetUploadTypeTemplatesByUploadTypeCode(inUploadTypeCode)

        If theTemplateCollection.Count > 0 Then
            theTemplateCollection.SortByPropertyValue("Name")

            inTemplateComboBox.DataSource = theTemplateCollection
            inTemplateComboBox.ValueMember = "UploadTypeTemplateID"
            inTemplateComboBox.DisplayMember = "Name"
        Else
            Throw New Exception("EIM is not configured properly. There must be at least one attribute template, the ' - All Attributes - ' template, " + _
                "for each upload type (grid). There is no template configured for the " + inUploadTypeCode + " upload type.")
        End If
    End Sub

    Private Sub SetUploadSessionUploadTypesSelectedTemplate(ByVal inUploadTypeCode As String, ByRef inTemplateComboBox As ComboBox)

        If Not IsNothing(Me.CurrentUploadSession) Then
            Dim theUploadSessionUploadType As UploadSessionUploadType = _
                CType(Me.CurrentUploadSession.FindUploadSessionUploadType(inUploadTypeCode, True), UploadSessionUploadType)

            If Not IsNothing(theUploadSessionUploadType) Then
                Dim theUploadTypeTemplate As UploadTypeTemplate = _
                    CType(inTemplateComboBox.SelectedItem, UploadTypeTemplate)

                If Not IsNothing(theUploadTypeTemplate) Then

                    '' mark the UploadSessionUploadType's template for delete
                    '' if the user selected the default faux all attributes template
                    'If theUploadTypeTemplate.UploadTypeTemplateID < 0 Then
                    '    If Not IsNothing(theUploadSessionUploadType.UploadTypeTemplate) Then
                    '        theUploadSessionUploadType.UploadTypeTemplate.IsMarkedForDelete = True
                    '    End If
                    'Else
                    ' set the new template

                    theUploadSessionUploadType.UploadTypeTemplate = theUploadTypeTemplate

                    ' End If
                End If
            End If
        End If

    End Sub

    Private Sub SetUploadTypeCheckBox(ByVal inUploadTypeCode As String, ByRef inUploadTypeCheckBox As CheckBox)

        Dim theUploadSessionUploadType As UploadSessionUploadType = Me.CurrentUploadSession.FindUploadSessionUploadType(inUploadTypeCode, True)
        If Not IsNothing(theUploadSessionUploadType) Then
            inUploadTypeCheckBox.Checked = True
        Else
            inUploadTypeCheckBox.Checked = False
        End If

        inUploadTypeCheckBox.Enabled = inUploadTypeCheckBox.Checked

    End Sub

    Private Sub SetUploadTypeByUsersSelection(ByVal inUploadTypeCode As String, ByRef inUploadTypeCheckBox As CheckBox)

        If _isInitializing Then Exit Sub

        Dim theUploadSessionUploadType As UploadSessionUploadType = _
           Me.CurrentUploadSession.FindUploadSessionUploadType(inUploadTypeCode, False)

        If inUploadTypeCheckBox.Checked Then

            If IsNothing(theUploadSessionUploadType) Then

                Dim theUploadType As UploadType = UploadTypeDAO.Instance.GetUploadTypeByPK(inUploadTypeCode)

                _currentUploadSession.AddUploadSessionUploadType(New UploadSessionUploadType(Nothing, theUploadType, Me.CurrentUploadSession))
            Else
                ' unmark the existing UploadType for delete
                theUploadSessionUploadType.IsMarkedForDelete = False
            End If
        Else
            ' mark the unselected UploadType for delete
            theUploadSessionUploadType.IsMarkedForDelete = True
        End If

        ' always do this when changing the UploadTypes for the session!
        _currentUploadSession.BuildQuickLookUpCollections()

    End Sub

    Private Sub ManageUIAppearance()

        Dim enableOkButton As Boolean = True

        If Me.SessionAction <> SessionActions.SaveExisting And Me.SessionAction <> SessionActions.SaveNew Then
            enableOkButton = _
                        (Me.CheckBoxItemMaintenance.Enabled And Me.CheckBoxItemMaintenance.Checked) Or _
                        (Me.CheckBoxPriceUpload.Enabled And Me.CheckBoxPriceUpload.Checked) Or _
                        (Me.CheckBoxCostUpload.Enabled And Me.CheckBoxCostUpload.Checked)
        End If

        Me.ButtonOK.Enabled = enableOkButton

    End Sub

#End Region

    
End Class