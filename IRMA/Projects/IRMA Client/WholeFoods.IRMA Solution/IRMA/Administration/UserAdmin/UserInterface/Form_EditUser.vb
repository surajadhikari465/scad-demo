Imports System.Text
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.IRMA.Administration.Common.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports System.Net.Mail
Imports log4net
Imports System.DirectoryServices

Public Class Form_EditUser

#Region "Class Level Vars and Property Definitions"
    ''' <summary>
    '''Active directory location for this class.
    ''' </summary>
    ''' <remarks></remarks>
    Private strPath = String.Format("LDAP://{0}", ConfigurationServices.AppSettings("LDAP_Server"))
    ''' <summary>
    ''' Log4Net logger for this class.
    ''' </summary>
    ''' <remarks></remarks>
    Private logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    ''' <summary>
    ''' The current action that is being performed: New or Edit
    ''' </summary>
    ''' <remarks></remarks>
    Private _currentAction As FormAction
    ''' <summary>
    ''' Value of the current user configuration for edits
    ''' </summary>
    ''' <remarks></remarks>
    Private _userConfig As UserBO
    ''' <summary>
    ''' Flag to keep track of user changes that have not been saved.
    ''' </summary>
    ''' <remarks></remarks>
    Private hasChanges As Boolean

    ''' <summary>
    ''' True when the form is being loaded.
    ''' </summary>
    ''' <remarks></remarks>
    Private _initializing As Boolean

    ''' <summary>
    ''' Stores the valid titles in the Titles table.
    ''' </summary>
    ''' <remarks></remarks>
    Private _titles As DataTable

    ''' <summary>
    ''' Indicates whether all Facility/Team associations should be removed from the user's profile.
    ''' </summary>
    ''' <remarks></remarks>
    Private _removeFacilityTeams As Boolean = False

#End Region

#Region "Events raised by this form"
    ''' <summary>
    ''' This event is raised when a child form makes a change that requires the
    ''' data on the calling form to be updated.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event UpdateCallingForm()

    ''' <summary>
    ''' This event is raised whenver form data is changed to indicate that it needs to be saved.
    ''' </summary>
    ''' <remarks></remarks>
    Private Event FormDataChanged()

#End Region

#Region "Events handled by this form"

#Region "Load Form"
    ''' <summary>
    ''' Load the form, pre-filling with the existing data for an edit or querying the database to populate 
    ''' the form for an add.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_EditUser_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        logger.Debug("Form_EditUser_Load entry")

        Me._initializing = True

        'set up button and title bar labels
        LoadText()

        Try

            ' populate drop-down selectors
            PopulateData()
            EnableCheckboxes(gbSecurityAdministrator, gbUserMaintenance)

            ' Show/Hide form controls and pre-fill data based on the form action
            Select Case _currentAction
                Case FormAction.Create
                    ' initialize the User 
                    _userConfig = New UserBO()
                    ' Default the changes flag 
                    'hasChanges = True
                    ' Pre-fill default vaues
                    CheckBox_AcctEnabled.Checked = True
                    Me.Button_DisableAccount.Enabled = False
                Case FormAction.Edit
                    ' Default the changes flag 
                    hasChanges = False
                    ' disable the ability to change the user account name
                    Me.TextBox_UserName.Enabled = False
                    ' Pre-fill for values that have defaults on create
                    CheckBox_AcctEnabled.Checked = _userConfig.AccountEnabled
                    If _userConfig.AccountEnabled = False Then
                        Me.Button_Save.Enabled = False
                        Me.Button_DisableAccount.Enabled = False
                    Else
                        CheckBox_AcctEnabled.Enabled = False
                    End If

                    GetUserStoreTeamTitles()
                    GetUserSubteams()

            End Select
            LoadUserDetails()

            SetToolTips()

        Catch ex As DataFactoryException
            logger.Error("Exception: ", ex)
            'display a message to the user
            DisplayErrorMessage(ERROR_DB)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_EditUser form: Form_EditUser_Load sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
        Finally
            TextBox_UserName.Focus()
            TextBox_UserName.SelectAll()
            _initializing = False
        End Try
        logger.Debug("Form_EditUser_Load exit")
    End Sub

    Private Sub LoadUserDetails()
        Dim hideSlimFunctionality As Boolean = InstanceDataDAO.IsFlagActive("HideSlimFunctionality")

        ' Required data to identify the user
        TextBox_UserName.Text = _userConfig.UserName
        TextBox_FullName.Text = _userConfig.FullName

        ' Informational data about the user
        ComboBox_Title.SelectedValue = _userConfig.Title
        ComboBox_HomeLocation.SelectedValue = _userConfig.TelxonStoreLimit
        TextBox_Printer.Text = _userConfig.Printer
        TextBox_Email.Text = _userConfig.Email
        If _userConfig.CoverPage Is Nothing Or _userConfig.CoverPage = String.Empty Then
            TextBox_CoverPg.Text = "default"
        Else
            TextBox_CoverPg.Text = _userConfig.CoverPage
        End If

        TextBox_PagerEmail.Text = _userConfig.PagerEmail
        TextBox_PhoneNo.Text = _userConfig.PhoneNumber
        TextBox_FaxNo.Text = _userConfig.FaxNumber

        ' Flags for the Roles assigned to the user
        CheckBox_Role_Accountant.Checked = _userConfig.Accountant
        CheckBox_Role_BatchBuildOnly.Checked = _userConfig.BatchBuildOnly
        CheckBox_Role_Buyer.Checked = _userConfig.Buyer
        CheckBox_Role_Coordinator.Checked = _userConfig.Coordinator
        CheckBox_Role_CostAdmin.Checked = _userConfig.CostAdmin
        CheckBox_Role_FacilityCreditProcessor.Checked = _userConfig.FacilityCreditProcessor
        CheckBox_Role_DCAdmin.Checked = _userConfig.DCAdmin
        CheckBox_Role_DeletePO.Checked = _userConfig.DeletePO
        CheckBox_Role_Distributor.Checked = _userConfig.Distributor
        CheckBox_Role_Einvoicing.Checked = _userConfig.EInvoicingAdministrator
        CheckBox_Role_InventoryAdmin.Checked = _userConfig.InventoryAdministrator
        CheckBox_Role_ItemAdmin.Checked = _userConfig.ItemAdministrator
        CheckBox_Role_LockAdmin.Checked = _userConfig.LockAdministrator
        CheckBox_Role_POAccountant.Checked = _userConfig.POAccountant
        CheckBox_Role_POApprovalAdmin.Checked = _userConfig.POApprovalAdmin
        CheckBox_Role_POEditor.Checked = _userConfig.POEditor
        CheckBox_Role_PriceBatchProcessor.Checked = _userConfig.PriceBatchProcessor
        CheckBox_Role_Shrink.Checked = _userConfig.Shrink
        CheckBox_Role_ShrinkAdmin.Checked = _userConfig.ShrinkAdmin
        CheckBox_Role_SuperUser.Checked = _userConfig.SuperUser
        CheckBox_Role_VendorAdmin.Checked = _userConfig.VendorAdministrator
        CheckBox_Role_VendorCostDiscrepancyAdmin.Checked = _userConfig.VendorCostDiscrepancyAdmin
        CheckBox_Role_Warehouse.Checked = _userConfig.Warehouse
        CheckBox_Role_Shrink.Checked = _userConfig.Shrink
        CheckBox_Role_ShrinkAdmin.Checked = _userConfig.ShrinkAdmin
        CheckBox_Role_TaxAdministrator.Checked = _userConfig.TaxAdministrator
        CheckBox_Role_CancelAllSales.Checked = _userConfig.CancellAllSales

        ' Admin Access attributes
        CheckBox_Role_AppConfigAdmin.Checked = _userConfig.ApplicationConfigAdmin
        CheckBox_Role_SystemConfigurationAdministrator.Checked = _userConfig.SystemConfigurationAdministrator
        CheckBox_Role_DataAdministrator.Checked = _userConfig.DataAdministrator
        CheckBox_Role_JobAdministrator.Checked = _userConfig.JobAdministrator
        CheckBox_Role_POSInterfaceAdministrator.Checked = _userConfig.POSInterfaceAdministrator
        CheckBox_Role_StoreAdministrator.Checked = _userConfig.StoreAdministrator
        CheckBox_Role_SecurityAdministrator.Checked = _userConfig.SecurityAdministrator
        CheckBox_Role_UserMaintenance.Checked = _userConfig.UserMaintenance

        ' enable/disable and set SLIM Access attributes
        If hideSlimFunctionality Then
            Dim location As Point = CheckBox_RoleSLIMItemRequest.Location
            HideSlimRelatedControls(False)
            RelocateSecureQueryCheckBox(location)
        Else
            CheckBox_RoleSLIMUserAdmin.Checked = _userConfig.SLIMUserAdmin
            CheckBox_RoleSLIMVendorRequest.Checked = _userConfig.SLIMVendorRequest
            CheckBox_RoleSLIMItemRequest.Checked = _userConfig.SLIMItemRequest
            CheckBox_RoleSLIMStoreSpecials.Checked = _userConfig.SLIMStoreSpecials

            CheckBox_RoleSLIMScaleInfo.Checked = _userConfig.SLIMScaleInfo
            CheckBox_RoleSLIMRetailCost.Checked = _userConfig.SLIMRetailCost
            CheckBox_RoleSLIMPushToIRMA.Checked = _userConfig.SLIMPushToIRMA
            CheckBox_RoleSLIMAuthorizations.Checked = _userConfig.SLIMAuthorizations
            CheckBox_RoleSLIMECommerce.Checked = _userConfig.SLIMECommerce
        End If

        CheckBox_RoleSLIMSecureQuery.Checked = _userConfig.SLIMSecureQuery

        ' Promo Planner access is dependent on values in the UserAccess table.
        ' Store Buyer role for promo planner requires a 1 (Store), Order Admin permissions require a 3 (Region)
        If _userConfig.PromoAccessLevel = 1 Then
            Radio_RolePromoBuyer.Checked = True
        ElseIf _userConfig.PromoAccessLevel = 3 Then
            Radio_RolePromoAdmin.Checked = True
        Else : Radio_RolePromoNone.Checked = True
        End If
    End Sub

    Private Sub RelocateSecureQueryCheckBox(ByVal location As Point)
        CheckBox_RoleSLIMSecureQuery.Location = location
    End Sub

    Private Sub HideSlimRelatedControls(ByVal isVisible As Boolean)
        CheckBox_RoleSLIMUserAdmin.Visible = isVisible
        CheckBox_RoleSLIMVendorRequest.Visible = isVisible
        CheckBox_RoleSLIMItemRequest.Visible = isVisible
        CheckBox_RoleSLIMStoreSpecials.Visible = isVisible

        CheckBox_RoleSLIMScaleInfo.Visible = isVisible
        CheckBox_RoleSLIMRetailCost.Visible = isVisible
        CheckBox_RoleSLIMPushToIRMA.Visible = isVisible
        CheckBox_RoleSLIMAuthorizations.Visible = isVisible
        CheckBox_RoleSLIMECommerce.Visible = isVisible
    End Sub

    ''' <summary>
    ''' Sets localized string text for buttons and title bar.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadText()
        Select Case _currentAction
            Case FormAction.Create
                Me.Text = ResourcesAdministration.GetString("label_titleBar_EditUser_Add")
            Case FormAction.Edit
                Me.Text = ResourcesAdministration.GetString("label_titleBar_EditUser_Edit")
        End Select
    End Sub

    ''' <summary>
    ''' Pre-populates the various drop-downs on the form.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateData()

        Dim dt As DataTable = Nothing
        Dim dr As DataRow = Nothing
        Dim _node As TreeNode = Nothing

        ' populate titles
        Me.ComboBox_Title.DataSource = TitleDAO.GetTitles()
        Me.ComboBox_Title.DisplayMember = "Title_Desc"
        Me.ComboBox_Title.ValueMember = "Title_ID"
        Me.ComboBox_Title.SelectedIndex = -1

        ' populate home locations
        Me.ComboBox_HomeLocation.DataSource = LocationDAO.GetLocations()
        Me.ComboBox_HomeLocation.DisplayMember = "Store_Name"
        Me.ComboBox_HomeLocation.ValueMember = "Store_No"
        Me.ComboBox_HomeLocation.SelectedIndex = -1

        ' populate common titles datatable
        Me._titles = TitleDAO.GetTitles()

        ' populate subteams
        Me.ComboBox_SubTeams.DataSource = SubTeamDAO.GetSubteams()
        Me.ComboBox_SubTeams.DisplayMember = "Subteam_Description"
        Me.ComboBox_SubTeams.ValueMember = "SubTeam_No"
        Me.ComboBox_SubTeams.SelectedIndex = -1

        ' populate facilities tree nodes
        Try
            dt = New DataTable
            dt = LocationDAO.GetLocations()
            For Each dr In dt.Rows
                _node = New TreeNode
                _node.Name = dr.Item("Store_No")
                _node.Text = dr.Item("Store_Name")
                If Not _node.Name = "0" Then
                    Me.TreeView_DataTree.Nodes("_node0").Nodes.Add(_node)
                End If
            Next
        Catch ex As Exception
            logger.Error("Exception: ", ex)
            MessageBox.Show(ex.Message)
            Dim args(1) As String
            args(0) = "Form_EditUser form: PopulateData(), LOCATIONDAL.GetLocations()"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
            Me.Close()
        End Try

        ' populate teams tree nodes
        Try
            dt = New DataTable
            dt = TeamDAO.GetRetailTeams
            For Each dr In dt.Rows
                _node = New TreeNode
                _node.Name = dr.Item("Team_No")
                _node.Text = dr.Item("Team_Name")
                Me.TreeView_DataTree.Nodes("_node1").Nodes.Add(_node)
            Next
        Catch ex As Exception
            logger.Error("Exception: ", ex)
            MessageBox.Show(ex.Message)
            Dim args(1) As String
            args(0) = "Form_EditUser form: PopulateData(), TEAMDAL.GetTeams()"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
            Me.Close()
        End Try

    End Sub

    ''' <summary>
    ''' Pre-populates the TreeView_Locations TreeView with the current user's entries in ItemCatalog.UserStoreTeamTitle.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetUserStoreTeamTitles()

        Dim dt As DataTable = Nothing
        Dim dr As DataRow = Nothing
        Dim _locationNode As TreeNode = Nothing
        Dim _teamNode As TreeNode = Nothing

        Try

            dt = New DataTable
            dt = UserDAO.GetUserStoreTeamTitle(_userConfig.UserId)

            If dt.Rows.Count > 0 Then

                ' first add all the store locations
                For Each dr In dt.Rows

                    _locationNode = New TreeNode()
                    _teamNode = New TreeNode()

                    With dr

                        _locationNode.Name = .Item("Store_No")
                        _locationNode.Text = .Item("Store_Name")
                        _teamNode.Name = .Item("Team_No")
                        _teamNode.Text = .Item("Team_Name")

                        If _locationNode.Name = 0 Then

                            Me.TreeView_Locations.Nodes("_node3").Nodes.Add(_teamNode)

                        Else

                            If Not Me.TreeView_Locations.Nodes("_node4").Nodes.ContainsKey(_locationNode.Name) Then
                                Me.TreeView_Locations.Nodes("_node4").Nodes.Add(_locationNode)
                            End If

                            If Not Me.TreeView_Locations.Nodes("_node4").Nodes(_locationNode.Name).Nodes.ContainsKey(_teamNode.Name) Then
                                Me.TreeView_Locations.Nodes("_node4").Nodes(_locationNode.Name).Nodes.Add(_teamNode)
                            End If

                        End If

                    End With

                Next

            End If

        Catch ex As Exception
            logger.Error("Exception: ", ex)
            MessageBox.Show(ex.Message)
            Dim args(1) As String
            args(0) = "Form_EditUser form: GetUserStoreTeamTitles"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
            Me.Close()
        End Try

    End Sub

    ''' <summary>
    ''' Pre-populates the TreeView_SubTeams TreeView with the current user's entries in ItemCatalog.UserSubteam.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetUserSubteams()

        Dim dt As DataTable = Nothing
        Dim dr As DataRow = Nothing
        Dim _subteamNode As TreeNode = Nothing

        Try

            dt = New DataTable
            dt = UserDAO.GetUserSubTeam(Me._userConfig.UserId)

            If dt.Rows.Count > 0 Then

                For Each dr In dt.Rows

                    _subteamNode = New TreeNode()

                    With dr

                        _subteamNode.Name = .Item("SubTeam_No")
                        _subteamNode.Text = .Item("SubTeam_Name")
                        _subteamNode.Checked = .Item("Regional_Coordinator")

                        Me.TreeView_SubTeams.Nodes.Add(_subteamNode)

                    End With

                Next

            End If

        Catch ex As Exception
            logger.Error("Exception: ", ex)
            MessageBox.Show(ex.Message)
            Dim args(1) As String
            args(0) = "Form_EditUser form: GetUserSubteams()"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
            Me.Close()
        End Try


    End Sub

#End Region

#Region "Save Button"

    ''' <summary>
    ''' Save the changes to the database and update the parent form.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ProcessSave()
        logger.Debug("ProcessSave entry")
        Try

            If ValidateData() Then

                Dim success As Boolean = False

                ' Populate the business object with the form data and save the change to the database
                _userConfig.AccountEnabled = CheckBox_AcctEnabled.Checked
                _userConfig.UserName = TextBox_UserName.Text.Trim
                _userConfig.FullName = TextBox_FullName.Text.Trim
                _userConfig.Title = ComboBox_Title.SelectedValue

                If CInt(ComboBox_HomeLocation.SelectedValue) = 0 Then
                    _userConfig.TelxonStoreLimit = 0
                    _userConfig.RecvLogStoreLimit = 0
                Else
                    _userConfig.TelxonStoreLimit = ComboBox_HomeLocation.SelectedValue
                    _userConfig.RecvLogStoreLimit = ComboBox_HomeLocation.SelectedValue
                End If

                _userConfig.Printer = TextBox_Printer.Text.Trim
                _userConfig.Email = TextBox_Email.Text.Trim
                _userConfig.CoverPage = TextBox_CoverPg.Text.Trim
                _userConfig.PagerEmail = TextBox_PagerEmail.Text.Trim
                _userConfig.PhoneNumber = TextBox_PhoneNo.Text.Trim
                _userConfig.FaxNumber = TextBox_FaxNo.Text.Trim

                _userConfig.Accountant = CheckBox_Role_Accountant.Checked
                _userConfig.BatchBuildOnly = CheckBox_Role_BatchBuildOnly.Checked
                _userConfig.Buyer = CheckBox_Role_Buyer.Checked
                _userConfig.Coordinator = CheckBox_Role_Coordinator.Checked
                _userConfig.CostAdmin = CheckBox_Role_CostAdmin.Checked
                _userConfig.FacilityCreditProcessor = CheckBox_Role_FacilityCreditProcessor.Checked
                _userConfig.DCAdmin = CheckBox_Role_DCAdmin.Checked
                _userConfig.DeletePO = CheckBox_Role_DeletePO.Checked
                _userConfig.Distributor = CheckBox_Role_Distributor.Checked
                _userConfig.EInvoicingAdministrator = CheckBox_Role_Einvoicing.Checked
                _userConfig.InventoryAdministrator = CheckBox_Role_InventoryAdmin.Checked
                _userConfig.ItemAdministrator = CheckBox_Role_ItemAdmin.Checked
                _userConfig.LockAdministrator = CheckBox_Role_LockAdmin.Checked
                _userConfig.POAccountant = CheckBox_Role_POAccountant.Checked
                _userConfig.POApprovalAdmin = CheckBox_Role_POApprovalAdmin.Checked
                _userConfig.POEditor = CheckBox_Role_POEditor.Checked
                _userConfig.PriceBatchProcessor = CheckBox_Role_PriceBatchProcessor.Checked
                _userConfig.Shrink = CheckBox_Role_Shrink.Checked
                _userConfig.ShrinkAdmin = CheckBox_Role_ShrinkAdmin.Checked
                _userConfig.SuperUser = CheckBox_Role_SuperUser.Checked
                _userConfig.TaxAdministrator = CheckBox_Role_TaxAdministrator.Checked
                _userConfig.VendorAdministrator = CheckBox_Role_VendorAdmin.Checked
                _userConfig.VendorCostDiscrepancyAdmin = CheckBox_Role_VendorCostDiscrepancyAdmin.Checked
                _userConfig.Warehouse = CheckBox_Role_Warehouse.Checked
                _userConfig.CancellAllSales = CheckBox_Role_CancelAllSales.Checked

                ' Admin Access attributes
                _userConfig.ApplicationConfigAdmin = CheckBox_Role_AppConfigAdmin.Checked
                _userConfig.SystemConfigurationAdministrator = CheckBox_Role_SystemConfigurationAdministrator.Checked
                _userConfig.DataAdministrator = CheckBox_Role_DataAdministrator.Checked
                _userConfig.JobAdministrator = CheckBox_Role_JobAdministrator.Checked
                _userConfig.POSInterfaceAdministrator = CheckBox_Role_POSInterfaceAdministrator.Checked
                _userConfig.StoreAdministrator = CheckBox_Role_StoreAdministrator.Checked
                _userConfig.SecurityAdministrator = CheckBox_Role_SecurityAdministrator.Checked
                _userConfig.UserMaintenance = CheckBox_Role_UserMaintenance.Checked

                ' SLIM Access attributes
                _userConfig.SLIMUserAdmin = Me.CheckBox_RoleSLIMUserAdmin.Checked
                _userConfig.SLIMItemRequest = Me.CheckBox_RoleSLIMItemRequest.Checked
                _userConfig.SLIMVendorRequest = Me.CheckBox_RoleSLIMVendorRequest.Checked
                _userConfig.SLIMPushToIRMA = Me.CheckBox_RoleSLIMPushToIRMA.Checked
                _userConfig.SLIMStoreSpecials = Me.CheckBox_RoleSLIMStoreSpecials.Checked
                _userConfig.SLIMRetailCost = Me.CheckBox_RoleSLIMRetailCost.Checked
                _userConfig.SLIMAuthorizations = Me.CheckBox_RoleSLIMAuthorizations.Checked
                _userConfig.SLIMSecureQuery = Me.CheckBox_RoleSLIMSecureQuery.Checked
                _userConfig.SLIMScaleInfo = Me.CheckBox_RoleSLIMScaleInfo.Checked
                _userConfig.SLIMECommerce = Me.CheckBox_RoleSLIMECommerce.Checked

                ' Promo Planner access is dependent on values in the UserAccess table.
                ' Store Buyer role for promo planner requires a 1 (Store), Order Admin permissions require a 3 (Region)
                If Me.Radio_RolePromoAdmin.Checked Then
                    _userConfig.PromoAccessLevel = 3
                ElseIf Me.Radio_RolePromoBuyer.Checked Then
                    _userConfig.PromoAccessLevel = 1
                Else
                    _userConfig.PromoAccessLevel = Nothing
                End If

                'validate data
                Dim statusList As ArrayList = _userConfig.ValidateUserConfigData()
                Dim statusEnum As IEnumerator = statusList.GetEnumerator
                Dim message As New StringBuilder
                Dim currentStatus As UserConfigStatus

                'loop through possible validation erorrs and build message string containing all errors
                While statusEnum.MoveNext
                    currentStatus = CType(statusEnum.Current, UserConfigStatus)

                    Select Case currentStatus
                        Case UserConfigStatus.Error_Required_UserName
                            message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Label_UserName.Text))
                            message.Append(Environment.NewLine)
                        Case UserConfigStatus.Error_Required_FullName
                            message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Label_FullName.Text))
                            message.Append(Environment.NewLine)
                    End Select
                End While

                ' save the data or display a validation error message
                If message.Length <= 0 Then
                    success = True

                    'data is valid - perform insert/update
                    Select Case _currentAction
                        Case FormAction.Create
                            _userConfig = UserDAO.AddUserRecord(_userConfig)
                        Case FormAction.Edit
                            UserDAO.UpdateUserRecord(_userConfig)
                    End Select

                    If Me._removeFacilityTeams Then
                        Dim _nodeTeam As TreeNode = Nothing
                        Dim _usst As UserStoreTeamTitleBO
                        For Each _nodeTeam In Me.TreeView_Locations.SelectedNode.Nodes
                            _usst = New UserStoreTeamTitleBO(Me._userConfig.UserId, _nodeTeam.Parent.Name, _nodeTeam.Name, Me._userConfig.Title)
                            UserStoreTeamTitleBO.Remove(_usst)
                            _nodeTeam.Remove()
                        Next
                        Me.TreeView_Locations.SelectedNode.Remove()
                    End If

                    Me._removeFacilityTeams = False

                    ' set the changes flag to false because they've been saved
                    Me.hasChanges = False

                    ' Raise the save event - allows the data on the parent form to be refreshed
                    RaiseEvent UpdateCallingForm()

                    ' Close the child window
                    ' not any more - instead disable the Save button to indicate there are no changes to save.
                    'Me.Close()
                    Me.Button_Save.Enabled = False
                    If _userConfig.AccountEnabled Then
                        Me.Button_DisableAccount.Enabled = True
                        Me.CheckBox_AcctEnabled.Enabled = False
                    End If

                    Me._currentAction = FormAction.Edit

                Else
                    success = False
                    'display error msg
                    MessageBox.Show(message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If

            End If

        Catch ex As DataFactoryException
            logger.Error("Exception: ", ex)
            'display a message to the user
            DisplayErrorMessage(ERROR_DB)
            'send message about exception
            Dim args(0) As String
            args(0) = "Form_EditUser form: ProcessSave sub"
            ErrorHandler.ProcessError(ErrorType.DataFactoryException, args, SeverityLevel.Warning, ex)
            ' Close the child window
            Me.Close()
        End Try
        logger.Debug("ProcessSave exit")
        Me._initializing = True
        'Reset role checkboxes in case the current user was editted.
        Me.ResetRoleCheckboxes()
        Me.PopulateRoleCheckboxes()
        Me.LoadUserDetails()
        Me.EnableCheckboxes(gbSecurityAdministrator, gbUserMaintenance)
        Me._initializing = False
        Me.Cursor = Cursors.Default

    End Sub

    ''' <summary>
    ''' The user selected the Save button - apply the changes.
    ''' Close the form and return focus to the View Stores form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Save.Click
        logger.Debug("Button_Save_Click entry")

        Dim sConflicts As String
        Dim sEmail As String
        Dim frm As New Form_RoleConflictReasons

        Try
            Cursor = Cursors.WaitCursor

            sConflicts = CheckForRoleConflicts()

            Try
                sEmail = ConfigurationServices.AppSettings("IRMASOXRoleConflictEmail")
            Catch ex As Exception
                MsgBox("Your changes will not be saved because there is no IRMA SOX Conflict e-mail address set up.  Please have the System Administrator add the IRMASOXRoleConflictEmail key to the IRMA Client and try again.", MsgBoxStyle.Critical, "IRMA SOX Role Conflict")
                Exit Sub
            End Try

            If sConflicts <> "" Then
                frm.RoleConflicts = sConflicts
                frm.ConflictType = "U"
                frm.User = _userConfig.UserName
                frm.UserId = _userConfig.UserId
                frm.ShowDialog()
            Else
                frm.ConflictRiskAccepted = True
            End If

            If frm.ConflictRiskAccepted Then
                ProcessSave()
            End If
        Catch ex As Exception
            logger.Error("Exception: ", ex)
            MessageBox.Show(ex.Message)
            Dim args(1) As String
            args(0) = "Form_EditUser form: Button_Save_Click"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
            Me.Close()
        Finally
            Me.Cursor = Cursors.Default
        End Try

        logger.Debug("Button_Save_Click exit")
    End Sub

    ''' <summary>
    ''' Validates the user form data. Invalid data is highlighted using the error provider control.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateData() As Boolean

        logger.Debug("ValidateData entry")

        Me.Form_ErrorProvider.Clear()

        Dim strTitle As String
        Dim shrinkTitle As Boolean = False

        Dim errCount As Integer = 0

        If Me.CheckBox_AcctEnabled.Checked Then

            strTitle = Trim(ComboBox_Title.Text.ToLower())
            If strTitle <> String.Empty And InStr(strTitle, "shrink") > 0 Then
                shrinkTitle = True
            End If

            If Not ValidateUserEmailAccount() Then PopulateADEmailAccount()

            'If Not shrinkTitle Then PopulateADEmailAccount()
            'If Not ValidateEmail() Then
            '    If shrinkTitle Then
            '        PopulateADEmailAccount()
            '    Else
            '        Me.Form_ErrorProvider.SetError(Me.TextBox_Email, "A mailbox must be added to this user's Active Directory Account before this change can be made.")
            '        errCount = errCount + 1
            '    End If
            'End If

            If Me.TextBox_Email.Text.Length = 0 And shrinkTitle = False Then
                Me.Form_ErrorProvider.SetError(Me.TextBox_Email, "A mailbox must be added to this user's Active Directory Account before this change can be made.")
                errCount = errCount + 1
            End If

            If Me.TextBox_FullName.Text.Length = 0 Then
                Me.Form_ErrorProvider.SetError(Me.TextBox_FullName, "Required")
                errCount = errCount + 1
            End If

            If Me.TextBox_UserName.Text.Length = 0 Then
                Me.Form_ErrorProvider.SetError(Me.TextBox_UserName, "Required")
                errCount = errCount + 1
            End If

            ' this may need to be tweaked to allow certain IRMA roles to have a NULL
            ' Telxon_Store_Limit value.
            If Me.ComboBox_HomeLocation.SelectedIndex = -1 Then
                Me.Form_ErrorProvider.SetError(Me.ComboBox_HomeLocation, "Required")
                errCount = errCount + 1
            End If

            If Me.ComboBox_Title.SelectedIndex = -1 Then
                Me.Form_ErrorProvider.SetError(Me.ComboBox_Title, "Required")
                errCount = errCount + 1
            End If

        Else
            errCount = 0
        End If


        If errCount = 0 Then
            Me.Form_ErrorProvider.Clear()
            Return True
        Else
            Return False
        End If

        logger.Debug("ValidateData exit")

    End Function

#End Region

#Region "Cancel Button"
    ''' <summary>
    ''' The user selected the Cancel button - do not save the changes.
    ''' Close the form and return focus to the View Stores form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        logger.Debug("Button_Cancel_Click entry")
        ' Set the flag so they are not prompted to save
        hasChanges = False
        ' Close the child window
        Me.Close()
        logger.Debug("Button_Cancel_Click exit")
    End Sub
#End Region

#Region "Close Form"
    ''' <summary>
    ''' Confirm user wants to save any changed data when they are clicking 'X' button in top-right of window
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_EditUser_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If hasChanges Then
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), ResourcesCommon.GetString("msg_titleConfirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = Windows.Forms.DialogResult.Yes Then
                ' Save the updates to the database
                If Me.ValidateData Then
                    ProcessSave()
                Else
                    e.Cancel = True
                End If
            End If
        End If
    End Sub
#End Region

#Region "Edit data"

    ''' <summary>
    ''' Sets the hasChanges form indicator to True and enables the Save button.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FormHasChanges() Handles Me.FormDataChanged

        If Me.CheckBox_AcctEnabled.Checked Then
            Me.hasChanges = True
            Me.Button_Save.Enabled = True
        End If

    End Sub

    ''' <summary>
    ''' Disables the user account immediately.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_DisableAccount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_DisableAccount.Click
        ProcessDisable()
    End Sub

    Private Sub Button_ViewRoleConflicts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_ViewRoleConflicts.Click
        Dim frm As New Form_ManageRoleConflicts

        frm.IsReadOnly = True
        frm.ShowDialog()
    End Sub

    Private Sub CheckBox_AcctEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_AcctEnabled.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub TextBox_UserName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_UserName.TextChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub TextBox_Title_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub TextBox_FullName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_FullName.TextChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub TextBox_Printer_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_Printer.TextChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub TextBox_Email_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_Email.TextChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub TextBox_CoverPg_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_CoverPg.TextChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub TextBox_PagerEmail_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_PagerEmail.TextChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub TextBox_PhoneNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_PhoneNo.TextChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub TextBox_FaxNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_FaxNo.TextChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_Accountant_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Role_Accountant.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_Buyer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Role_Buyer.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_Coordinator_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Role_Coordinator.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_FacilityCreditProcessor_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Role_FacilityCreditProcessor.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_Distributor_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Role_Distributor.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_InventoryAdmin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Role_InventoryAdmin.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_ItemAdmin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Role_ItemAdmin.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_LockAdmin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Role_LockAdmin.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_POAccountant_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Role_POAccountant.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_PriceBatchProcessor_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Role_PriceBatchProcessor.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_SuperUser_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Role_SuperUser.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_VendorAdmin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Role_VendorAdmin.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_Warehouse_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Role_Warehouse.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_CancelAllSales_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_Role_CancelAllSales.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub ComboBox_HomeLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox_HomeLocation.SelectedIndexChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub
    Private Sub PopulateADEmailAccount()

        Dim _writeInfo As Boolean = False
        Dim notFoundMsg As String = "User '" & Me.TextBox_UserName.Text.Trim & "' was not found in the directory.  Please verify the user's login/account name and try again."

        Try
            ' setup the search in AD for the user's detail
            Try
                If strPath = "LDAP://" Then Throw New Exception()
            Catch exLDAP As Exception
                Throw New Exception("LDAP_Server has not been configured")
            End Try

            Dim objDirEntry As New System.DirectoryServices.DirectoryEntry(strPath.ToString())
            Dim objDirSearcher As New System.DirectoryServices.DirectorySearcher(objDirEntry)
            Dim objCollSearchResult As System.DirectoryServices.SearchResultCollection
            Dim objlSearchResult As System.DirectoryServices.SearchResult
            Dim objCollResultProperty As System.DirectoryServices.ResultPropertyCollection
            Dim objCollResultPropertyValue As System.DirectoryServices.ResultPropertyValueCollection

            objDirSearcher.Filter = ("(&(objectClass=user)(samaccountname=" & Me.TextBox_UserName.Text.Trim & "))")

            ' search AD for the user account
            objCollSearchResult = objDirSearcher.FindAll()

            If objCollSearchResult.Count = 1 Then

                ' pull the AD properties that we can and load the text boxes
                objlSearchResult = objCollSearchResult.Item(0)
                objCollResultProperty = objlSearchResult.Properties

                objCollResultPropertyValue = objCollResultProperty.Item("mail")
                If objCollResultPropertyValue.Count > 0 Then
                    Me.TextBox_Email.Text = objCollResultPropertyValue.Item(0)
                Else
                    Me.TextBox_Email.Text = String.Empty
                End If

                'Me.ComboBox_Title.Focus()
            End If

            ' dispose of all the AD objects
            objDirEntry.Dispose()
            objDirSearcher.Dispose()
            objCollSearchResult.Dispose()
            objlSearchResult = Nothing
            objCollResultProperty = Nothing
            objCollResultPropertyValue = Nothing

        Catch ex As System.Exception
            logger.Error(String.Format("LDAP lookup failed for user '" & Me.TextBox_UserName.Text.Trim & "'. [ {0} ]", ex.Message), ex)
            Throw New Exception(notFoundMsg, ex)
        End Try

    End Sub

    Private Sub ComboBox_Title_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox_Title.SelectedIndexChanged
        If Not Me._initializing Then

            Dim strTitle As String

            strTitle = Trim(ComboBox_Title.Text.ToLower())
            If Trim(Me.TextBox_Email.Text) = String.Empty And Not (InStr(strTitle, "shrink") > 0) Then
                PopulateADEmailAccount()
            End If

            PopulateRoleCheckboxes()
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_RoleSLIMAuthorizations_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_RoleSLIMAuthorizations.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_RoleSLIMItemRequest_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_RoleSLIMItemRequest.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_RoleSLIMPushToIRMA_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_RoleSLIMPushToIRMA.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_RoleSLIMRetailCost_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_RoleSLIMRetailCost.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_RoleSLIMScaleInfo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_RoleSLIMScaleInfo.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_RoleSLIMSecureQuery_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_RoleSLIMSecureQuery.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_RoleSLIMStoreSpecials_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_RoleSLIMStoreSpecials.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_Einvoicing_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_Role_Einvoicing.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_BatchBuildOnly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Role_BatchBuildOnly.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_DCAdmin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Role_DCAdmin.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_DeletePO_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Role_DeletePO.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_TaxAdministrator_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Role_TaxAdministrator.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_AddAllSubTeams_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_AddAllSubTeams.CheckedChanged
        Me.ComboBox_SubTeams.Enabled = Not Me.CheckBox_AddAllSubTeams.Checked

        If Me.CheckBox_AddAllSubTeams.Checked Then
            Me.ComboBox_SubTeams.SelectedIndex = -1
        End If
    End Sub

    Private Sub CheckBox_RemoveAllSubTeams_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_RemoveAllSubTeams.CheckedChanged
        If Me.Button_RemoveSubTeam.Enabled = False Then Me.Button_RemoveSubTeam.Enabled = True
    End Sub

    Private Sub CheckBox_RoleSLIMUserAdmin_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_RoleSLIMUserAdmin.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_RoleSLIMVendorRequest_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_RoleSLIMVendorRequest.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_CostAdmin_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_Role_CostAdmin.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_VendorCostDiscrepancyAdmin_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_Role_VendorCostDiscrepancyAdmin.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub
    Private Sub CheckBox_Role_POApprovalAdmin_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_Role_POApprovalAdmin.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_AppConfigAdmin_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_Role_AppConfigAdmin.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_DataAdministrator_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_Role_DataAdministrator.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_JobAdministrator_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_Role_JobAdministrator.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_POSInterfaceAdministrator_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_Role_POSInterfaceAdministrator.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_StoreAdministrator_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_Role_StoreAdministrator.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_UserMaintenance_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_Role_UserMaintenance.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_Shrink_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_Role_Shrink.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_ShrinkAdmin_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_Role_ShrinkAdmin.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_POEditor_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Role_POEditor.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_SecurityAdministrator_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox_Role_SecurityAdministrator.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_Role_SystemConfigurationAdministrator_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox_Role_SystemConfigurationAdministrator.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    Private Sub CheckBox_RoleSLIMECommerce_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_RoleSLIMECommerce.CheckedChanged
        If Not Me._initializing Then
            RaiseEvent FormDataChanged()
        End If
    End Sub

    ''' <summary>
    ''' This event triggers UI changes based on the CheckState of the Radio_RolePromoAdmin check box.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Radio_RolePromoAdmin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Radio_RolePromoAdmin.CheckedChanged

        If Not Me._initializing Then

            RaiseEvent FormDataChanged()

        End If

    End Sub

    ''' <summary>
    ''' This event triggers UI changes based on the CheckState of the Radio_RolePromoBuyer check box.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Radio_RolePromoBuyer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Radio_RolePromoBuyer.CheckedChanged

        If Not Me._initializing Then

            RaiseEvent FormDataChanged()

        End If

    End Sub

    ''' <summary>
    ''' This event triggers UI changes based on the CheckState of the Radio_RolePromoNone check box.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Radio_RolePromoNone_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Radio_RolePromoNone.CheckedChanged

        If Not Me._initializing Then

            RaiseEvent FormDataChanged()


        End If
    End Sub

    Private Sub ProcessDisable()

        logger.Debug("ProcessDisable Enter")
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Button_DisableAccount.Enabled = False
            UserDAO.DeleteUserRecord(_userConfig.UserId)
            hasChanges = False
            ' Raise the save event - allows the data on the parent form to be refreshed
            RaiseEvent UpdateCallingForm()
            Me.Cursor = Cursors.Default
            Me.Close()
        Catch ex As Exception
            logger.Error("Exception: ", ex)
            'display a message to the user
            DisplayErrorMessage(ex.Message)
            'send message about exception
            Dim args(0) As String
            args(0) = "Form_EditUser form: ProcessDisable sub"
            ErrorHandler.ProcessError(ErrorType.Administration_UserDisableAccount, args, SeverityLevel.Warning, ex)
            ' Close the child window
            Me.Close()
        Finally
            Me.Cursor = Cursors.Default
        End Try
        logger.Debug("ProcessDisable exit")

    End Sub


    Private Sub PopulateRoleCheckboxes()
        Dim dt As DataTable = Nothing
        Dim dr As DataRow = Nothing

        If IsNumeric(ComboBox_Title.SelectedValue) Then
            ResetRoleCheckboxes()
            dt = TitleDAO.GetTitlePermissions(ComboBox_Title.SelectedValue)

            For Each dr In dt.Rows
                CheckBox_Role_Accountant.Checked = CBool(dr.Item("Accountant"))
                CheckBox_Role_BatchBuildOnly.Checked = CBool(dr.Item("BatchBuildOnly"))
                CheckBox_Role_Buyer.Checked = CBool(dr.Item("Buyer"))
                CheckBox_Role_Coordinator.Checked = CBool(dr.Item("Coordinator"))
                CheckBox_Role_CostAdmin.Checked = CBool(dr.Item("CostAdministrator"))
                CheckBox_Role_FacilityCreditProcessor.Checked = CBool(dr.Item("FacilityCreditProcessor"))
                CheckBox_Role_DCAdmin.Checked = CBool(dr.Item("DCAdmin"))
                CheckBox_Role_DeletePO.Checked = CBool(dr.Item("DeletePO"))
                CheckBox_Role_Einvoicing.Checked = CBool(dr.Item("EInvoicing"))
                CheckBox_Role_InventoryAdmin.Checked = CBool(dr.Item("InventoryAdministrator"))
                CheckBox_Role_ItemAdmin.Checked = CBool(dr.Item("ItemAdministrator"))
                CheckBox_Role_LockAdmin.Checked = CBool(dr.Item("LockAdministrator"))
                CheckBox_Role_POAccountant.Checked = CBool(dr.Item("POAccountant"))
                CheckBox_Role_POApprovalAdmin.Checked = CBool(dr.Item("POApprovalAdministrator"))
                CheckBox_Role_POEditor.Checked = CBool(dr.Item("POEditor"))
                CheckBox_Role_PriceBatchProcessor.Checked = CBool(dr.Item("PriceBatchProcessor"))
                CheckBox_Role_Distributor.Checked = CBool(dr.Item("Distributor"))
                CheckBox_Role_VendorAdmin.Checked = CBool(dr.Item("VendorAdministrator"))
                CheckBox_Role_VendorCostDiscrepancyAdmin.Checked = CBool(dr.Item("VendorCostDiscrepancyAdmin"))
                CheckBox_Role_Warehouse.Checked = CBool(dr.Item("Warehouse"))
                CheckBox_Role_AppConfigAdmin.Checked = CBool(dr.Item("ApplicationConfigAdmin"))
                CheckBox_Role_DataAdministrator.Checked = CBool(dr.Item("DataAdministrator"))
                CheckBox_Role_JobAdministrator.Checked = CBool(dr.Item("JobAdministrator"))
                CheckBox_Role_POSInterfaceAdministrator.Checked = CBool(dr.Item("POSInterfaceAdministrator"))
                CheckBox_Role_StoreAdministrator.Checked = CBool(dr.Item("StoreAdministrator"))
                CheckBox_Role_UserMaintenance.Checked = CBool(dr.Item("UserMaintenance"))
                CheckBox_Role_Shrink.Checked = CBool(dr.Item("Shrink"))
                CheckBox_Role_ShrinkAdmin.Checked = CBool(dr.Item("ShrinkAdmin"))
                CheckBox_Role_TaxAdministrator.Checked = CBool(dr.Item("TaxAdministrator"))
                CheckBox_Role_CancelAllSales.Checked = CBool(dr.Item("CancelAllSales"))

                'The following roles don't get assigned a value based on the title setup since these roles can only be toggled via ECC (for SOX reasons)
                'TFS5330 -  Security Admin role can grant System Config and Super User
                '           Security Admin and User Maintenance can grant Security Admin
                CheckBox_Role_SuperUser.Checked = CBool(dr.Item("SuperUser"))
                CheckBox_Role_SystemConfigurationAdministrator.Enabled = CBool(dr.Item("SystemConfigurationAdministrator"))
                CheckBox_Role_SecurityAdministrator.Checked = CBool(dr.Item("SecurityAdministrator"))
            Next
        End If
    End Sub

    Private Sub ResetRoleCheckboxes()
        CheckBox_Role_Accountant.Checked = False
        CheckBox_Role_BatchBuildOnly.Checked = False
        CheckBox_Role_Buyer.Checked = False
        CheckBox_Role_Coordinator.Checked = False
        CheckBox_Role_CostAdmin.Checked = False
        CheckBox_Role_FacilityCreditProcessor.Checked = False
        CheckBox_Role_DCAdmin.Checked = False
        CheckBox_Role_DeletePO.Checked = False
        CheckBox_Role_Einvoicing.Checked = False
        CheckBox_Role_InventoryAdmin.Checked = False
        CheckBox_Role_ItemAdmin.Checked = False
        CheckBox_Role_LockAdmin.Checked = False
        CheckBox_Role_POAccountant.Checked = False
        CheckBox_Role_POApprovalAdmin.Checked = False
        CheckBox_Role_POEditor.Checked = False
        CheckBox_Role_PriceBatchProcessor.Checked = False
        CheckBox_Role_Distributor.Checked = False
        CheckBox_Role_VendorAdmin.Checked = False
        CheckBox_Role_VendorCostDiscrepancyAdmin.Checked = False
        CheckBox_Role_Warehouse.Checked = False
        CheckBox_Role_AppConfigAdmin.Checked = False
        CheckBox_Role_DataAdministrator.Checked = False
        CheckBox_Role_JobAdministrator.Checked = False
        CheckBox_Role_POSInterfaceAdministrator.Checked = False
        CheckBox_Role_StoreAdministrator.Checked = False
        CheckBox_Role_UserMaintenance.Checked = False
        CheckBox_Role_Shrink.Checked = False
        CheckBox_Role_ShrinkAdmin.Checked = False
        CheckBox_Role_TaxAdministrator.Checked = False
        CheckBox_Role_CancelAllSales.Checked = False

        'The following roles don't get cleared since these roles can only be toggled via ECC (for SOX reasons)
        'TFS5330 -  Security Admin role can grant System Config and Super User
        '           Security Admin and User Maintenance can grant Security Admin
        CheckBox_Role_SuperUser.Checked = False
        'CheckBox_Role_SystemConfigurationAdministrator.Enabled = False
        'CheckBox_Role_SecurityAdministrator.Checked = False
    End Sub

    Private Sub EnableCheckboxes(ByVal blnSecurityAdmin As Boolean, ByVal blnUserMaint As Boolean)
        CheckBox_Role_Accountant.Enabled = blnSecurityAdmin
        CheckBox_Role_BatchBuildOnly.Enabled = blnSecurityAdmin
        CheckBox_Role_Buyer.Enabled = blnSecurityAdmin
        CheckBox_Role_Coordinator.Enabled = blnSecurityAdmin
        CheckBox_Role_CostAdmin.Enabled = blnSecurityAdmin
        CheckBox_Role_FacilityCreditProcessor.Enabled = blnSecurityAdmin
        CheckBox_Role_DCAdmin.Enabled = blnSecurityAdmin
        CheckBox_Role_DeletePO.Enabled = blnSecurityAdmin
        CheckBox_Role_Einvoicing.Enabled = blnSecurityAdmin
        CheckBox_Role_InventoryAdmin.Enabled = blnSecurityAdmin
        CheckBox_Role_ItemAdmin.Enabled = blnSecurityAdmin
        CheckBox_Role_LockAdmin.Enabled = blnSecurityAdmin
        CheckBox_Role_POAccountant.Enabled = blnSecurityAdmin
        CheckBox_Role_POApprovalAdmin.Enabled = blnSecurityAdmin
        CheckBox_Role_POEditor.Enabled = blnSecurityAdmin
        CheckBox_Role_PriceBatchProcessor.Enabled = blnSecurityAdmin
        CheckBox_Role_Distributor.Enabled = blnSecurityAdmin
        CheckBox_Role_VendorAdmin.Enabled = blnSecurityAdmin
        CheckBox_Role_VendorCostDiscrepancyAdmin.Enabled = blnSecurityAdmin
        CheckBox_Role_Warehouse.Enabled = blnSecurityAdmin
        CheckBox_Role_AppConfigAdmin.Enabled = blnSecurityAdmin
        CheckBox_Role_DataAdministrator.Enabled = blnSecurityAdmin
        CheckBox_Role_JobAdministrator.Enabled = blnSecurityAdmin
        CheckBox_Role_POSInterfaceAdministrator.Enabled = blnSecurityAdmin
        CheckBox_Role_StoreAdministrator.Enabled = blnSecurityAdmin
        CheckBox_Role_UserMaintenance.Enabled = blnSecurityAdmin
        CheckBox_Role_Shrink.Enabled = blnSecurityAdmin
        CheckBox_Role_ShrinkAdmin.Enabled = blnSecurityAdmin
        CheckBox_Role_TaxAdministrator.Enabled = blnSecurityAdmin
        CheckBox_Role_CancelAllSales.Enabled = blnSecurityAdmin

        'The following roles will never be enabled since these roles can only be toggled via ECC (for SOX reasons)
        'TFS5330 -  Security Admin role can grant System Config and Super User
        '           Security Admin and User Maintenance can grant Security Admin
        CheckBox_Role_SuperUser.Enabled = False
        CheckBox_Role_SystemConfigurationAdministrator.Enabled = blnSecurityAdmin
        CheckBox_Role_SecurityAdministrator.Enabled = blnSecurityAdmin Or blnUserMaint
    End Sub

    Private Function CheckForRoleConflicts() As String
        Dim dt As DataTable
        Dim dr As DataRow
        Dim ctrl As Control
        Dim ctrl2 As Control
        Dim blnPrimaryChecked As Boolean = False
        Dim blnConflictChecked As Boolean = False
        Dim sConflictsExist As String = ""

        dt = TitleDAO.GetRoleConflicts

        For Each dr In dt.Rows
            For Each ctrl In Me.TabPage_RolesIRMA.Controls
                If TypeOf ctrl Is GroupBox Then
                    For Each ctrl2 In ctrl.Controls
                        If UCase(ctrl2.Text) = UCase(dr("Role1").ToString) Then
                            blnPrimaryChecked = CType(ctrl2, CheckBox).Checked
                        End If

                        If UCase(ctrl2.Text) = UCase(dr("Role2").ToString) Then
                            blnConflictChecked = CType(ctrl2, CheckBox).Checked
                        End If
                    Next
                End If
            Next

            If blnPrimaryChecked And blnConflictChecked Then
                sConflictsExist = sConflictsExist & dr("Role1").ToString & "^" & dr("Role2") & "|"
            End If
        Next

        sConflictsExist = CheckIfConflictAlreadyLogged(sConflictsExist)

        Return sConflictsExist
    End Function

    Private Function CheckIfConflictAlreadyLogged(ByVal sConflicts As String)
        Dim dt As DataTable
        Dim x As Integer = 0

        dt = TitleDAO.GetRoleConflictReason("U", -1, _userConfig.UserId)

        For x = 0 To dt.Rows.Count - 1
            If InStr(sConflicts, dt.Rows(x)("Role1") & "^" & dt.Rows(x)("Role2")) > 0 Then
                sConflicts = Replace(sConflicts, dt.Rows(x)("Role1") & "^" & dt.Rows(x)("Role2"), "")
            End If

            If InStr(sConflicts, dt.Rows(x)("Role2") & "^" & dt.Rows(x)("Role1")) > 0 Then
                sConflicts = Replace(sConflicts, dt.Rows(x)("Role2") & "^" & dt.Rows(x)("Role1"), "")
            End If
        Next

        sConflicts = Replace(sConflicts, "||", "|")

        If Len(sConflicts) > 0 Then
            If Len(sConflicts) = 1 Then
                sConflicts = ""
            Else
                If Mid(sConflicts, 1, 1) = "|" Then sConflicts = Mid(sConflicts, 2)
            End If
        End If

        If Len(sConflicts) > 0 Then
            If Mid(sConflicts, Len(sConflicts), 1) = "|" Then sConflicts = Mid(sConflicts, 1, Len(sConflicts) - 1)
        End If

        Return sConflicts
    End Function

    Private Sub SetToolTips()
        Dim ctrl As Control
        Dim ctrl2 As Control

        For Each ctrl In Me.TabPage_RolesIRMA.Controls
            If TypeOf ctrl Is GroupBox Then
                For Each ctrl2 In ctrl.Controls
                    If TypeOf ctrl2 Is CheckBox Then ttRoleDescription.SetToolTip(ctrl2, TitleDAO.GetToolTipText(ctrl2.Text))
                Next
            End If
        Next
    End Sub

#End Region

#Region " AD Search"

    ''' <summary>
    ''' Searches Active Direwctory display name, email address and phone number of the user
    ''' from Active Directory.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Button_SearchUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_SearchUser.Click

        ' Make sure there's a value in the username field.
        If String.IsNullOrEmpty(Me.TextBox_UserName.Text.Trim) Then
            MessageBox.Show("Please enter a value in the 'User Name' field to perform a directory search.", "User Name Required for AD Search", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        ' Make sure username does not contain spaces (LDAP lookup throws exception if it does, so we're just saving the user from that error).
        If Me.TextBox_UserName.Text.Trim.Contains(" ") Then
            MessageBox.Show("The 'User Name' value cannot contain spaces.  Please update and try again.", "Invalid User Name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        ' disable the button while searching
        Me.Button_SearchUser.Enabled = False

        Me.Cursor = Cursors.WaitCursor

        Select Case Me._currentAction

            Case FormAction.Create

                ' if we are creating a new account, empty the text boxes before each new search
                Me.TextBox_Email.Text = String.Empty
                Me.TextBox_FullName.Text = String.Empty
                Me.TextBox_PhoneNo.Text = String.Empty

        End Select

        ' get the user's details from Active Directory
        ' Handle exceptions thrown so it doesn't crash client.
        Try
            SetUserDetails()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Directory-Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' renable the button after the search is complete
        Me.Button_SearchUser.Enabled = True

        Me.Cursor = Cursors.Default

    End Sub

    ''' <summary>
    ''' Loads the User Form with the full display name, email address and phone number of the user
    ''' from Active Directory.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetUserDetails()

        Dim _writeInfo As Boolean = False
        Dim notFoundMsg As String = "User '" & Me.TextBox_UserName.Text.Trim & "' was not found in the directory.  Please verify the user's login/account name and try again."

        Try

            ' setup the search in AD for the user's detail
            Try
                If strPath = "LDAP://" Then Throw New Exception()
            Catch exLDAP As Exception
                Throw New Exception("LDAP_Server has not been configured")
            End Try

            Dim objDirEntry As New System.DirectoryServices.DirectoryEntry(strPath.ToString())
            Dim objDirSearcher As New System.DirectoryServices.DirectorySearcher(objDirEntry)
            Dim objCollSearchResult As System.DirectoryServices.SearchResultCollection
            Dim objlSearchResult As System.DirectoryServices.SearchResult
            Dim objCollResultProperty As System.DirectoryServices.ResultPropertyCollection
            Dim objCollResultPropertyValue As System.DirectoryServices.ResultPropertyValueCollection

            objDirSearcher.Filter = ("(&(objectClass=user)(samaccountname=" & Me.TextBox_UserName.Text.Trim & "))")

            ' search AD for the user account
            objCollSearchResult = objDirSearcher.FindAll()

            Select Case objCollSearchResult.Count

                Case 0

                    Me.hasChanges = False

                    ' no results found in AD
                    MessageBox.Show(notFoundMsg, "User Not Found in Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning)

                    If Me._userConfig.AccountEnabled Then
                        ProcessDisable()
                    End If

                Case 1

                    ' if we are editing an existing user, verify that we want to overwrite what is already there
                    If Me._currentAction = FormAction.Edit Then

                        If MessageBox.Show("Overwrite the existing user information with the information from Active Directory?", "Confirm Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then

                            ' we're good, so bring in the AD information
                            _writeInfo = True

                        End If

                    ElseIf Me._currentAction = FormAction.Create Then

                        ' if we are creating a new user, make sure an account doesn't already exist in IRMA
                        If UserDAO.ValidateIRMALogin(Me.TextBox_UserName.Text) Then

                            _writeInfo = False

                            Me.TextBox_UserName.Text = String.Empty

                            Me.hasChanges = False

                            MessageBox.Show("This user already has an IRMA Account.", "IRMA Account Exists", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        Else

                            ' we're good, so bring in the AD information
                            _writeInfo = True

                        End If

                    End If

                    If _writeInfo Then

                        ' pull the AD properties that we can and load the text boxes
                        objlSearchResult = objCollSearchResult.Item(0)
                        objCollResultProperty = objlSearchResult.Properties

                        objCollResultPropertyValue = objCollResultProperty.Item("mail")
                        If objCollResultPropertyValue.Count > 0 Then
                            Me.TextBox_Email.Text = objCollResultPropertyValue.Item(0)
                        Else
                            Me.TextBox_Email.Text = String.Empty
                        End If

                        objCollResultPropertyValue = objCollResultProperty.Item("displayName")
                        If objCollResultPropertyValue.Count > 0 Then
                            Me.TextBox_FullName.Text = objCollResultPropertyValue.Item(0)
                        End If

                        objCollResultPropertyValue = objCollResultProperty.Item("telephoneNumber")
                        If objCollResultPropertyValue.Count > 0 Then
                            Me.TextBox_PhoneNo.Text = objCollResultPropertyValue.Item(0)
                        End If

                        'focus on the next required field
                        Me.ComboBox_Title.Focus()

                        ' if the account is being re-enabled, then check the enabled checkbox 
                        If Not Me._userConfig.AccountEnabled And Me._currentAction = FormAction.Edit Then
                            If MessageBox.Show("Would you like to re-enable this user account?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                Me.CheckBox_AcctEnabled.Checked = True
                            End If
                        End If

                    Else

                        ' set focus back on the user name since the one entered was not valid
                        Me.TextBox_UserName.Focus()

                    End If

                Case Else

                    Exit Try

            End Select

            ' dispose of all the AD objects
            objDirEntry.Dispose()
            objDirSearcher.Dispose()
            objCollSearchResult.Dispose()
            objlSearchResult = Nothing
            objCollResultProperty = Nothing
            objCollResultPropertyValue = Nothing

        Catch ex As System.Exception
            logger.Error(String.Format("LDAP lookup failed for user '" & Me.TextBox_UserName.Text.Trim & "'. [ {0} ]", ex.Message), ex)
            Throw New Exception(notFoundMsg, ex)
        End Try

    End Sub

#End Region

#Region " UserStoreTeamTitle Tab events"

    ''' <summary>
    ''' Adds a single UserStoreTeamTitle entry.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_AddUserStoreTeamTitle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_AddUserStoreTeamTitle.Click

        logger.Debug("Button_AddUserStoreTeamTitle_Click Enter")

        Dim _badMove As Boolean = False
        Dim _snode As TreeNode = Nothing
        Dim _sourceNode As TreeNode = Nothing
        Dim _dNode As TreeNode = Nothing
        Dim _destinationNode As TreeNode = Nothing
        Dim _ustt As UserStoreTeamTitleBO = Nothing

        Try

            Me.Cursor = Cursors.WaitCursor

            ' make sure there is a selected node in the source destination trees
            If Me.TreeView_DataTree.SelectedNode IsNot Nothing And Me.TreeView_Locations.SelectedNode IsNot Nothing Then

                ' clone the source and desintation nodes so we can work with them
                _destinationNode = New TreeNode
                _destinationNode = Me.TreeView_Locations.SelectedNode.Clone

                _sourceNode = New TreeNode
                _sourceNode = Me.TreeView_DataTree.SelectedNode.Clone

                ' if the node has no parent, then we're adding a team to a store
                If Me.TreeView_Locations.SelectedNode.Parent IsNot Nothing Or Me.TreeView_Locations.SelectedNode.Name = "_node3" Then

                    ' make sure that all the form data is valid since we are dependent on it for a proper UserStoreTeamTitle entry
                    If Me.ValidateData() Then

                        ' if the user made any changes, ask to save them first
                        If Me.hasChanges Then

                            If MessageBox.Show("Changes have been made to the User Detail information and must be saved first. Do you want to save your existing changes?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then

                                ' if the user says yes, do our thing
                                ' save the form data
                                Me.ProcessSave()

                            Else

                                ' we have unreliable data and it could change so don't move create the userstoreteamtitle entries
                                Exit Try

                            End If

                        End If

                        ' don't add the selected node if it already exists
                        If Not Me.TreeView_Locations.SelectedNode.Nodes.ContainsKey(_sourceNode.Name) Then

                            ' we'll pass this UserStoreTeamTitleBO object to the Add Method
                            Dim _entry As UserStoreTeamTitleBO

                            If Me.TreeView_Locations.SelectedNode.Parent Is Nothing Then
                                _entry = New UserStoreTeamTitleBO(Me._userConfig.UserId, 0, _sourceNode.Name, Me._userConfig.Title)
                            Else
                                _entry = New UserStoreTeamTitleBO(Me._userConfig.UserId, Me.TreeView_Locations.SelectedNode.Name, _sourceNode.Name, Me._userConfig.Title)
                            End If

                            ' now attempt to save the USST object to the db - if true, remove the source node
                            ' from the source tree and move it to the destination tree
                            If UserStoreTeamTitleBO.Add(_entry) Then

                                ' record was created so now add the node to the destination and expand the parent
                                ' disable the add button until the next go-round.
                                Me.TreeView_Locations.SelectedNode.Nodes.Add(_sourceNode)
                                Me.TreeView_Locations.SelectedNode.Expand()
                                Me.Button_AddUserStoreTeamTitle.Enabled = False

                            End If

                        Else

                            ' combination exists so tell the user
                            MessageBox.Show("This combination already exists.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Try

                        End If

                    End If

                ElseIf Me.TreeView_DataTree.SelectedNode IsNot Nothing And Me.TreeView_Locations.SelectedNode.Name = "_node4" And Not Me.TreeView_Locations.SelectedNode.Nodes.ContainsKey(_sourceNode.Name) Then

                    ' othwerwise we're adding a store to the stores node and no db work is to be done yet
                    Me.TreeView_Locations.Nodes(_destinationNode.Name).Nodes.Add(_sourceNode)
                    Me.TreeView_Locations.Nodes(_destinationNode.Name).Expand()

                End If

            End If

        Catch ex As Exception

            logger.Error("Exception: ", ex)
            'display a message to the user
            DisplayErrorMessage(ex.Message)
            'send message about exception
            Dim args(0) As String
            args(0) = "Form_EditUser form: Button_AddUserStoreTeamTitle_Click sub"
            ErrorHandler.ProcessError(ErrorType.Administration_UserStoreTeamTitle, args, SeverityLevel.Warning, ex)
            ' Close the child window
            Me.Close()

        Finally

            Me.Cursor = Cursors.Default

        End Try

        logger.Debug("Button_AddUserStoreTeamTitle_Click Exit")

    End Sub

    ''' <summary>
    ''' Adds multiple UserStoreTeamTitle entries.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_AddAllUserStoreTeamTitle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_AddAllUserStoreTeamTitle.Click

        logger.Debug("Button_AddAllUserStoreTeamTitle_Click Enter")

        Dim _badMove As Boolean = False
        Dim _snode As TreeNode = Nothing
        Dim _sourceNode As TreeNode = Nothing
        Dim _dNode As TreeNode = Nothing
        Dim _destinationNode As TreeNode = Nothing
        Dim _ustt As UserStoreTeamTitleBO = Nothing

        Try

            Me.Cursor = Cursors.WaitCursor

            If Me.TreeView_DataTree.SelectedNode IsNot Nothing And Me.TreeView_Locations.SelectedNode IsNot Nothing Then

                ' handles disallowed moves first
                If Me.TreeView_DataTree.SelectedNode.Name = "_node0" And Me.TreeView_Locations.SelectedNode.Name = "_node3" Then
                    ' you can't move All Facilities to the Facilities Associations Node
                    ' Only Teams can be moved to this node.
                    _badMove = True

                ElseIf Me.TreeView_DataTree.SelectedNode.Name = "_node1" And Not Me.TreeView_Locations.SelectedNode.Name = "_node3" Then
                    ' you can't move All Teams to the Facility/Teams associations node
                    ' Teams must be associated with an existing store child in this node.
                    _badMove = True

                End If

                If Not _badMove Then

                    ' make sure that all the form data is valid since we are dependent on it for a proper UserStoreTeamTitle entry
                    If Me.ValidateData() Then

                        ' if the user made any changes, ask to save them first
                        If Me.hasChanges Then

                            If MessageBox.Show("Changes have been made to the User Detail information and must be saved first. Do you want to save your existing changes?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then

                                ' if the user says yes, do our thing
                                ' save the form data
                                Me.ProcessSave()

                            Else

                                ' we have unreliable data and it could change so don't move create the userstoreteamtitle entries
                                Exit Try

                            End If

                        End If

                        Me.TreeView_Locations.BeginUpdate()

                        ' start moving the nodes over to the destination tree
                        For Each _snode In Me.TreeView_DataTree.SelectedNode.Nodes

                            Dim _node As TreeNode = Nothing



                            If Not Me.TreeView_Locations.SelectedNode.Nodes.ContainsKey(_snode.Name) Then

                                _ustt = New UserStoreTeamTitleBO(Me._userConfig.UserId, 0, _snode.Name, Me._userConfig.Title)

                                If UserStoreTeamTitleBO.Add(_ustt) Then

                                    _node = New TreeNode
                                    _node = _snode.Clone

                                    Me.TreeView_Locations.SelectedNode.Nodes.Add(_node)

                                End If

                            End If



                        Next

                        Me.TreeView_Locations.EndUpdate()

                        Me.TreeView_Locations.SelectedNode.Expand()

                        Me.Button_AddAllUserStoreTeamTitle.Enabled = False

                    End If

                End If

            Else

                MessageBox.Show(String.Format(ResourcesAdministration.GetString("msg_validation_BadNodeMove"), "all " & _
                                        Me.TreeView_DataTree.SelectedNode.Text, Me.TreeView_Locations.SelectedNode.Text), _
                                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception

            logger.Error("Exception: ", ex)
            'display a message to the user
            DisplayErrorMessage(ex.Message)
            'send message about exception
            Dim args(0) As String
            args(0) = "Form_EditUser form: Button_AddAllUserStoreTeamTitle_Click sub"
            ErrorHandler.ProcessError(ErrorType.Administration_UserStoreTeamTitle, args, SeverityLevel.Warning, ex)
            ' Close the child window
            Me.Close()

        Finally

            Me.Cursor = Cursors.Default

        End Try

        logger.Debug("Button_AddAllUserStoreTeamTitle_Click Exit")


    End Sub

    ''' <summary>
    ''' Triggers the appropriate UI changes based on the user selections in the TreeView_Locations TreeView control.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TreeView_Locations_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView_Locations.NodeMouseClick

        logger.Debug("TreeView_Locations_NodeMouseClick Enter")

        Try

            Me.Button_AddAllUserStoreTeamTitle.Enabled = False
            Me.Button_AddUserStoreTeamTitle.Enabled = False
            Me.Button_RemoveUserStoreTeamTitle.Enabled = False

            ' RULES for allowed node moves
            ' 1. If the source tree node clicked is _node1 ("Teams") AND the node has child nodes AND the destination
            '       node clicked is _node3 ("All facilities"), enable Button_AddAllUserStoreTeamTitle.
            ' 2. If the source tree node clicked is not _node0 or _node3, test the parents of the source node and
            '       2.1 if the parent is _node0 and the desintation node clicked is _node4 enable Button_AddUserStoreTeamTitle.
            '       2.2 if the parent is _node1 and the desintation node clicked is _node3 enable Button_AddUserStoreTeamTitle.
            '       2.3 if the parent is _node1 and the desintation node clicked is a child of _node4 enable Button_AddUserStoreTeamTitle.

            If Me.TreeView_DataTree.SelectedNode IsNot Nothing Then

                If Me.TreeView_DataTree.SelectedNode.Name = "_node1" And Me.TreeView_DataTree.SelectedNode.Nodes.Count > 0 And e.Node.Name = "_node3" Then

                    ' Rule 1
                    Me.Button_AddAllUserStoreTeamTitle.Enabled = True

                ElseIf Not Me.TreeView_DataTree.SelectedNode.Name = "_node0" Or Me.TreeView_DataTree.SelectedNode.Name = "_node1" Then

                    If Me.TreeView_DataTree.SelectedNode.Parent IsNot Nothing Then

                        ' Rule 2
                        Select Case Me.TreeView_DataTree.SelectedNode.Parent.Name

                            Case "_node0"

                                ' 2.1
                                If e.Node.Name = "_node4" And Not e.Node.Nodes.ContainsKey(Me.TreeView_DataTree.SelectedNode.Name) Then
                                    Me.Button_AddUserStoreTeamTitle.Enabled = True
                                End If

                            Case "_node1"

                                ' 2.2
                                If e.Node.Name = "_node3" And Not e.Node.Nodes.ContainsKey(Me.TreeView_DataTree.SelectedNode.Name) Then
                                    Me.Button_AddUserStoreTeamTitle.Enabled = True
                                End If

                                ' 2.3
                                If e.Node.Parent IsNot Nothing Then
                                    If e.Node.Parent.Name = "_node4" Then
                                        Me.Button_AddUserStoreTeamTitle.Enabled = True
                                    End If
                                End If

                        End Select

                    End If

                End If

            End If

            ' Rules for node removes
            If Not e.Node.Name = "_node3" And Not e.Node.Name = "_node4" Then
                ' user has clicked a node other than one of the root nodes so allow the removal.
                Me.Button_RemoveUserStoreTeamTitle.Enabled = True
            End If

        Catch ex As Exception

            logger.Error("Exception: ", ex)
            'display a message to the user
            DisplayErrorMessage(ex.Message)
            'send message about exception
            Dim args(0) As String
            args(0) = "Form_EditUser form: TreeView_Locations_NodeMouseClick sub"
            ErrorHandler.ProcessError(ErrorType.Administration_UserStoreTeamTitle, args, SeverityLevel.Warning, ex)
            ' Close the child window
            Me.Close()

        End Try

        logger.Debug("TreeView_Locations_NodeMouseClick Exit")

    End Sub

    ''' <summary>
    ''' Removes a single UserStoreTeamTitle entry.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_RemoveUserStoreTeamTitle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RemoveUserStoreTeamTitle.Click
        logger.Debug("Button_RemoveUserStoreTeamTitle_Click Enter")
        Try

            Me.Button_AddAllUserStoreTeamTitle.Enabled = False
            Me.Button_AddUserStoreTeamTitle.Enabled = False
            Me.Button_RemoveUserStoreTeamTitle.Enabled = False

            If Not Me.TreeView_Locations.SelectedNode.Parent Is Nothing Then

                ' make sure that all the form data is valid since we are dependent on it for a proper UserStoreTeamTitle entry
                If Me.ValidateData() Then

                    ' if the user made any changes, ask to save them first
                    If Me.hasChanges Then

                        If MessageBox.Show("Changes have been made to the User Detail information and must be saved first. Do you want to save your existing changes?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then

                            ' if the user says yes, do our thing
                            ' save the form data
                            Me.ProcessSave()

                        Else

                            ' we have unreliable data and it could change so don't move create the userstoreteamtitle entries
                            Exit Try

                        End If

                    End If

                    Dim _usst As New UserStoreTeamTitleBO

                    Select Case Me.TreeView_Locations.SelectedNode.Parent.Name

                        Case "_node3"

                            _usst = New UserStoreTeamTitleBO(Me._userConfig.UserId, 0, Me.TreeView_Locations.SelectedNode.Name, Me._userConfig.Title)
                            UserStoreTeamTitleBO.Remove(_usst)
                            Me.TreeView_Locations.SelectedNode.Remove()

                        Case "_node4"

                            Dim _nodeTeam As TreeNode = Nothing

                            For Each _nodeTeam In Me.TreeView_Locations.SelectedNode.Nodes

                                _usst = New UserStoreTeamTitleBO(Me._userConfig.UserId, _nodeTeam.Parent.Name, _nodeTeam.Name, Me._userConfig.Title)
                                UserStoreTeamTitleBO.Remove(_usst)
                                _nodeTeam.Remove()

                            Next

                            Me.TreeView_Locations.SelectedNode.Remove()


                        Case Else

                            _usst = New UserStoreTeamTitleBO(Me._userConfig.UserId, Me.TreeView_Locations.SelectedNode.Parent.Name, Me.TreeView_Locations.SelectedNode.Name, Me._userConfig.Title)
                            UserStoreTeamTitleBO.Remove(_usst)
                            Me.TreeView_Locations.SelectedNode.Remove()

                    End Select

                End If

            End If

        Catch ex As Exception

            logger.Error("Exception: ", ex)
            'display a message to the user
            DisplayErrorMessage(ex.Message)
            'send message about exception
            Dim args(0) As String
            args(0) = "Form_EditUser form: Button_RemoveUserStoreTeamTitle_Click sub"
            ErrorHandler.ProcessError(ErrorType.Administration_UserStoreTeamTitle, args, SeverityLevel.Warning, ex)
            ' Close the child window
            Me.Close()

        End Try

        logger.Debug("Button_RemoveUserStoreTeamTitle_Click Exit")

    End Sub

    ''' <summary>
    ''' Triggers appropriate UI changes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TreeView_DataTree_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView_DataTree.NodeMouseClick
        Me.Button_AddAllUserStoreTeamTitle.Enabled = False
        Me.Button_AddUserStoreTeamTitle.Enabled = False
        Me.Button_RemoveUserStoreTeamTitle.Enabled = False
    End Sub

    Private Function ValidateUserEmailAccount() As Boolean
        If Me.TextBox_Email.Text.Trim = String.Empty Then Return True

        Dim notFoundMsg As String = "User '" & Me.TextBox_UserName.Text.Trim & "' was not found in the directory.  Please verify the user's login/account name and try again."

        Try
            ' setup the search in AD for the user's detail
            Dim objDirEntry As New System.DirectoryServices.DirectoryEntry(strPath.ToString())
            Dim objDirSearcher As New System.DirectoryServices.DirectorySearcher(objDirEntry)
            Dim objCollSearchResult As System.DirectoryServices.SearchResultCollection
            Dim objlSearchResult As System.DirectoryServices.SearchResult
            Dim objCollResultProperty As System.DirectoryServices.ResultPropertyCollection
            Dim objCollResultPropertyValue As System.DirectoryServices.ResultPropertyValueCollection

            objDirSearcher.Filter = ("(&(objectClass=user)(samaccountname=" & Me.TextBox_UserName.Text.Trim & "))")

            ' search AD for the user account
            objCollSearchResult = objDirSearcher.FindAll()

            If objCollSearchResult.Count = 1 Then
                objlSearchResult = objCollSearchResult.Item(0)
                objCollResultProperty = objlSearchResult.Properties

                objCollResultPropertyValue = objCollResultProperty.Item("mail")
                If objCollResultPropertyValue.Count > 0 Then
                    If Trim(Me.TextBox_Email.Text) <> Trim(objCollResultPropertyValue.Item(0)) Then
                        Return False
                    End If
                Else
                    If Trim(Me.TextBox_Email.Text).Length > 0 Then
                        Return False
                    End If
                End If
            End If

            ' dispose of all the AD objects
            objDirEntry.Dispose()
            objDirSearcher.Dispose()
            objCollSearchResult.Dispose()
            objlSearchResult = Nothing
            objCollResultProperty = Nothing
            objCollResultPropertyValue = Nothing

            Return True

        Catch ex As System.Exception
            logger.Error(String.Format("LDAP lookup failed for user '" & Me.TextBox_UserName.Text.Trim & "'. [ {0} ]", ex.Message), ex)
            Throw New Exception(notFoundMsg, ex)
        End Try

    End Function

    ''' <summary>
    ''' Validates that email exists in AD.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function ValidateEmail() As Boolean

        If Me.TextBox_Email.Text.Trim = String.Empty Then Return True

        Try
            If strPath = "LDAP://" Then Throw New Exception()

        Catch exLDAP As Exception
            Throw New Exception("LDAP_Server has not been configured")
        End Try

        Try
            Dim objDirEntry As New DirectoryEntry(strPath.ToString())
            Dim objDirSearcher As New DirectorySearcher(objDirEntry)
            Dim objCollSearchResult As SearchResultCollection

            objDirSearcher.Filter = ("(&(objectClass=user)(mail=" & Me.TextBox_Email.Text.Trim & "))")

            ' search AD for the user account
            objCollSearchResult = objDirSearcher.FindAll()

            Select Case objCollSearchResult.Count
                Case 0
                    Return False
                Case Else
                    Return True
            End Select

        Catch ex As System.Exception
            logger.Error(String.Format("LDAP email lookup failed for email '" & Me.TextBox_Email.Text.Trim & "'. [ {0} ]", ex.Message), ex)
            Throw New Exception("Email '" & Me.TextBox_Email.Text.Trim & "' was not found in the AD", ex)
        End Try
    End Function

#End Region

#Region " UsersSubTeam tab events"

    ''' <summary>
    ''' Adds a single User/SubTeam entry.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_AddSubTeam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_AddSubTeam.Click
        logger.Debug("Button_AddSubTeam_Click Enter")
        Dim _selectedSubTeam As Integer

        Try

            If Me.CheckBox_AddAllSubTeams.Checked Then
                Cursor = Cursors.WaitCursor

                For x As Integer = 0 To Me.ComboBox_SubTeams.Items.Count - 1
                    Me.ComboBox_SubTeams.SelectedIndex = x
                    _selectedSubTeam = CInt(Me.ComboBox_SubTeams.SelectedValue)

                    If Not _selectedSubTeam = -1 And Not Me.TreeView_SubTeams.Nodes.ContainsKey(_selectedSubTeam) Then
                        Dim _node As New TreeNode

                        _node.Name = _selectedSubTeam
                        _node.Text = Me.ComboBox_SubTeams.Text
                        _node.Checked = Me.CheckBox_IsCoordinator.Checked.ToString

                        Dim _entry As New UsersSubTeamBO(Me._userConfig.UserId, _selectedSubTeam, Me.CheckBox_IsCoordinator.Checked)

                        If UsersSubTeamBO.Add(_entry) Then
                            Me.TreeView_SubTeams.Nodes.Add(_node)
                        End If
                    End If
                Next

                Me.CheckBox_AddAllSubTeams.Checked = False
                Cursor = Cursors.Arrow
            Else
                _selectedSubTeam = CInt(Me.ComboBox_SubTeams.SelectedValue)

                If Not _selectedSubTeam = -1 And Not Me.TreeView_SubTeams.Nodes.ContainsKey(_selectedSubTeam) Then
                    Dim _node As New TreeNode

                    _node.Name = _selectedSubTeam
                    _node.Text = Me.ComboBox_SubTeams.Text
                    _node.Checked = Me.CheckBox_IsCoordinator.Checked.ToString

                    Dim _entry As New UsersSubTeamBO(Me._userConfig.UserId, _selectedSubTeam, Me.CheckBox_IsCoordinator.Checked)

                    If UsersSubTeamBO.Add(_entry) Then
                        Me.TreeView_SubTeams.Nodes.Add(_node)
                    End If
                End If
            End If

        Catch ex As Exception

            logger.Error("Exception: ", ex)
            'display a message to the user
            DisplayErrorMessage(ex.Message)
            'send message about exception
            Dim args(0) As String
            args(0) = "Form_EditUser form: Button_AddSubTeam_Click sub"
            ErrorHandler.ProcessError(ErrorType.Administration_UserStoreTeamTitle, args, SeverityLevel.Warning, ex)
            ' Close the child window
            Me.Close()

        Finally

            Me.Cursor = Cursors.Default

        End Try

        logger.Debug("Button_AddSubTeam_Click Exit")

    End Sub

    ''' <summary>
    ''' Removes a single User/SubTeam entry
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_RemoveSubTeam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RemoveSubTeam.Click
        logger.Debug("Button_RemoveSubTeam_Click Enter")

        Dim eNodeEnum As IEnumerator
        Dim currentNode As TreeNode

        Try
            If Me.CheckBox_RemoveAllSubTeams.Checked Then
                Do While Me.TreeView_SubTeams.Nodes.Count > 0
                    Dim _entry As New UsersSubTeamBO(Me._userConfig.UserId, Me.TreeView_SubTeams.Nodes(0).Name, Me.TreeView_SubTeams.Nodes(0).Checked)

                    If UsersSubTeamBO.Remove(_entry) Then
                        Me.TreeView_SubTeams.Nodes(0).Remove()
                    End If
                Loop

                Me.CheckBox_RemoveAllSubTeams.Checked = False
            Else
                eNodeEnum = Me.TreeView_SubTeams.Nodes.GetEnumerator
                While eNodeEnum.MoveNext
                    currentNode = CType(eNodeEnum.Current, TreeNode)
                    If currentNode.Checked Then
                        Dim _entry As New UsersSubTeamBO(Me._userConfig.UserId, currentNode.Name, currentNode.Checked)
                        If UsersSubTeamBO.Remove(_entry) Then
                            currentNode.Remove()
                        End If
                    End If
                End While
            End If
            Me.Button_RemoveSubTeam.Enabled = False
        Catch ex As Exception
            logger.Error("Exception: ", ex)
            'display a message to the user
            DisplayErrorMessage(ex.Message)
            'send message about exception
            Dim args(0) As String
            args(0) = "Form_EditUser form: Button_RemoveSubTeam_Click sub"
            ErrorHandler.ProcessError(ErrorType.Administration_UserStoreTeamTitle, args, SeverityLevel.Warning, ex)
            ' Close the child window
            Me.Close()

        Finally
            Me.Cursor = Cursors.Default
        End Try

        logger.Debug("Button_RemoveSubTeam_Click Exit")

    End Sub

    ''' <summary>
    ''' Sets the Regional_Coordinator flag on the User/Subtea entry.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TreeView_SubTeams_BeforeCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles TreeView_SubTeams.BeforeCheck

        logger.Debug("TreeView_SubTeams_BeforeCheck Enter")

        Try

            Dim _isCoordinator As Boolean = Not (e.Node.Checked)

            Dim _entry As New UsersSubTeamBO(Me._userConfig.UserId, e.Node.Name, _isCoordinator)

            If Not UsersSubTeamBO.Update(_entry) Then

                e.Cancel = True

            End If

        Catch ex As Exception

            logger.Error("Exception: ", ex)
            'display a message to the user
            DisplayErrorMessage(ex.Message)
            'send message about exception
            Dim args(0) As String
            args(0) = "Form_EditUser form: TreeView_SubTeams_BeforeCheck sub"
            ErrorHandler.ProcessError(ErrorType.Administration_UserStoreTeamTitle, args, SeverityLevel.Warning, ex)
            ' Close the child window
            Me.Close()

        Finally

            Me.Cursor = Cursors.Default

        End Try

        logger.Debug("TreeView_SubTeams_BeforeCheck Exit")

    End Sub

    ''' <summary>
    ''' Enables the Button_RemoveSubTeam button after a node is selected.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TreeView_SubTeams_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView_SubTeams.NodeMouseClick
        Me.Button_RemoveSubTeam.Enabled = True
    End Sub

    ''' <summary>
    ''' Depending on the tab selected, makes visible or invisible the label notifying the user that changes happen immediately.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TabControl_SecuritySettings_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl_SecuritySettings.SelectedIndexChanged

        Me.Label_ChangesHappenImmediately.Visible = False

        Select Case Me.TabControl_SecuritySettings.SelectedIndex

            Case 3, 4

                Me.Label_ChangesHappenImmediately.Visible = True

        End Select

    End Sub

#End Region

#End Region

#Region "Property Definitions"
    Public Property CurrentAction() As FormAction
        Get
            Return _currentAction
        End Get
        Set(ByVal value As FormAction)
            _currentAction = value
        End Set
    End Property

    Public Property UserConfig() As UserBO
        Get
            Return _userConfig
        End Get
        Set(ByVal value As UserBO)
            _userConfig = value
        End Set
    End Property
#End Region

End Class
