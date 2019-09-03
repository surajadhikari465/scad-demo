Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ModelLayer.DataAccess

''' <summary>
''' This is a reusable control for selecting a node in the linked financial/product
''' hierarchy. If the region does not use the four level hierarchy then the Level 3 and 4
''' comboboxes and corresponding add buttons will be disabled.
''' 
''' <b>Important Usage Note:</b>
''' There is a bug in VB.NET VS 2005 that comes up when a user control is placed on a form
''' in the same project. VS 2005 adds a self referential project reference to the project that
''' causes compilation to fail.
''' 
''' The work around is to manually delete this reference (it will be named "Inventory") after each time
''' this control is placed on a form in the form designer.
''' 
''' Please contact David Marine (512-826-8644) for further information.
''' 
''' </summary>
''' <remarks></remarks>
Public Class HierarchySelector

#Region "Fields and Properties Definitions"

    Public Delegate Sub CategoryChangedEventHandler()
    Public Event CategoryChanged As CategoryChangedEventHandler

    Public Delegate Sub Level4ChangedEventHandler()
    Public Event Level4Changed As Level4ChangedEventHandler

    Public Delegate Sub HierarchySelectionChangedEventHandler()
    Public Event HierarchySelectionChanged As HierarchySelectionChangedEventHandler

    Public Delegate Sub AddHierarchyNodeEventHandler(ByRef e As CancelableEventArgs)
    Public Event AddHierarchyNode As AddHierarchyNodeEventHandler

    Private _isInitializing As Boolean

    Private _isInitialized As Boolean = False
    Private _usesFourLevelHierarchy As Boolean = False

    Private _itemIdentifier As String
    Private _selectedSubTeamId As Integer
    Private _selectedSubTeamName As String
    Private _selectedCategoryId As Integer
    Private _selectedCategoryName As String
    Private _selectedLevel3Id As Integer
    Private _selectedLevel3Name As String
    Private _selectedLevel4Id As Integer
    Private _selectedLevel4Name As String

    Private _previousSubTeamId As Integer
    Private _previousCategoryId As Integer
    Private _previousLevel3Id As Integer
    Private _previousLevel4Id As Integer

    Public Property ItemIdentifier() As String
        Get
            Return _itemIdentifier
        End Get
        Set(ByVal value As String)
            _itemIdentifier = value

            If Not IsNothing(value) Then
                Dim theSubteamNo As Integer
                Dim theCategoryId As Integer
                Dim theLevel3Id As Integer
                Dim theLevel4Id As Integer

                ' get the hierarchy position for the item by identifier
                EIMUtilityDAO.Instance.GetItemHierarchyByIdentifier(value, theSubteamNo, theCategoryId, theLevel3Id, theLevel4Id)

                ' set the hierarchy values
                Me.SelectedSubTeamId = theSubteamNo
                Me.SelectedCategoryId = theCategoryId

                If Me.UsesFourLevelHierarchy Then
                    Me.SelectedLevel3Id = theLevel3Id
                    Me.SelectedLevel4Id = theLevel4Id
                End If
            End If
        End Set
    End Property

    Public Property SelectedSubTeamId() As Integer
        Get
            Return GetComboValue(cmbSubTeam)
        End Get
        Set(ByVal value As Integer)
            SetCombo(cmbSubTeam, value)
        End Set
    End Property

    Public ReadOnly Property SelectedSubTeamName() As String
        Get
            Return GetComboDbString(cmbSubTeam)
        End Get
    End Property

    Public Property SelectedCategoryId() As Integer
        Get
            Return GetComboValue(cmbCategory)
        End Get
        Set(ByVal value As Integer)
            SetCombo(cmbCategory, value)
        End Set
    End Property

    Public ReadOnly Property SelectedCategoryName() As String
        Get
            Return GetComboDbString(cmbCategory)
        End Get
    End Property

    Public Property SelectedLevel3Id() As Integer
        Get
            Return GetComboValue(cmbLevel3)
        End Get
        Set(ByVal value As Integer)
            SetCombo(cmbLevel3, value)
        End Set
    End Property

    Public ReadOnly Property SelectedLevel3Name() As String
        Get
            Return GetComboDbString(cmbLevel3)
        End Get
    End Property

    Public Property SelectedLevel4Id() As Integer
        Get
            Return GetComboValue(cmbLevel4)
        End Get
        Set(ByVal value As Integer)
            SetCombo(cmbLevel4, value)
        End Set
    End Property

    Public ReadOnly Property SelectedLevel4Name() As String
        Get
            Return GetComboDbString(cmbLevel4)
        End Get
    End Property

    Public ReadOnly Property UsesFourLevelHierarchy() As Boolean
        Get
            Return _usesFourLevelHierarchy
        End Get
    End Property

	Public Property SubteamCheckBoxVisible As Boolean
		Get
			Return chkSubTeam.Visible
		End Get
		Set(value As Boolean)
			chkSubTeam.Visible = value
		End Set
	End Property

	Public Property ShowAllSubteams As Boolean
		Get
			Return chkSubTeam.Checked
		End Get
		Set(value As Boolean)
			chkSubTeam.Checked = value
		End Set
	End Property
#End Region

#Region "Public Methods"

	Public Sub Initialize()

        If Not _isInitialized Then
            _usesFourLevelHierarchy = InstanceDataDAO.IsFlagActive("FourLevelHierarchy")

            SetAddHierarchLevelsActive(True)

            _isInitializing = True

			LoadSubTeamByType(enumSubTeamType.All, cmbSubTeam, Nothing, -1, -1, chkSubTeam.Checked)

			_isInitializing = False

            _isInitialized = True

            SetFourLevelHierarchyComboboxesActive()
        End If
    End Sub

    Public Sub ClearSelection()
		cmbSubTeam.SelectedIndex = -1
	End Sub

    Public Sub ResetToPreviousIds()
        cmbSubTeam.SelectedIndex = _previousSubTeamId
        cmbCategory.SelectedIndex = _previousCategoryId
        cmbLevel3.SelectedIndex = _previousLevel3Id
        cmbLevel4.SelectedIndex = _previousLevel4Id
    End Sub

    Public Sub SetAddHierarchLevelsActive(ByVal isActive As Boolean)

        ' only allow item admins and super users to add hierarchy nodes
        isActive = isActive AndAlso (gbItemAdministrator OrElse gbSuperUser)

        Dim wasVisible As Boolean = Me.cmdAddCat.Visible

        Me.cmdAddCat.Visible = isActive
        Me.cmdAddLevel3.Visible = isActive AndAlso _usesFourLevelHierarchy
        Me.cmdAddLevel4.Visible = isActive AndAlso _usesFourLevelHierarchy

        Dim theWidthDelta As Integer = 32

        If wasVisible AndAlso Not isActive Then
			Me.pnlSubTeam.Width = Me.pnlSubTeam.Width + theWidthDelta
			Me.cmbCategory.Width = Me.cmbCategory.Width + theWidthDelta
			Me.cmbLevel3.Width = Me.cmbLevel3.Width + theWidthDelta
            Me.cmbLevel4.Width = Me.cmbLevel4.Width + theWidthDelta
        ElseIf Not wasVisible And isActive Then
			Me.pnlSubTeam.Width = Me.pnlSubTeam.Width - theWidthDelta
			Me.cmbCategory.Width = Me.cmbCategory.Width - theWidthDelta
            Me.cmbLevel3.Width = Me.cmbLevel3.Width - theWidthDelta
            Me.cmbLevel4.Width = Me.cmbLevel4.Width - theWidthDelta
        End If

    End Sub

    Public Sub SetHierarchLevelComboboxesActive(ByVal isActive As Boolean)

        ' only allow item admins and super users to add hierarchy nodes
        isActive = isActive AndAlso (gbItemAdministrator OrElse gbSuperUser)

		Me.pnlSubTeam.Visible = isActive
		Me.cmbCategory.Visible = isActive
 
        Me.pnlSubTeam.Enabled = isActive
        Me.cmbCategory.Enabled = isActive

        SetFourLevelHierarchyComboboxesActive(isActive)


    End Sub

    Public Sub SetHierarchLevelComboboxesVisible(ByVal isVisible As Boolean)

		Me.pnlSubTeam.Visible = isVisible
		Me.cmbCategory.Visible = isVisible
        If _usesFourLevelHierarchy Then
            Me.cmdAddLevel3.Visible = isVisible
            Me.cmdAddLevel4.Visible = isVisible
        End If

    End Sub

#End Region

#Region "Private Methods"

    Private Sub SetPreviousIds()
        _previousSubTeamId = cmbSubTeam.SelectedIndex
        _previousCategoryId = cmbCategory.SelectedIndex
        _previousLevel3Id = cmbLevel3.SelectedIndex
        _previousLevel4Id = cmbLevel4.SelectedIndex
    End Sub

    Private Function GetComboValue(ByRef cmbField As System.Windows.Forms.ComboBox) As Integer

        If cmbField.SelectedIndex <> -1 Then
            Return VB6.GetItemData(cmbField, cmbField.SelectedIndex)
        End If

    End Function

    Private Sub SetComboValue(ByRef cmbField As System.Windows.Forms.ComboBox, ByVal index As Integer)

        If index < cmbField.Items.Count Then
            cmbField.SelectedIndex = index
        End If

    End Sub

    Private Function GetComboDbValue(ByRef cmbField As System.Windows.Forms.ComboBox) As String

        Dim value As String = "NULL"

        If cmbField.SelectedIndex = -1 Then
            value = "NULL"
        Else
            value = CStr(VB6.GetItemData(cmbField, cmbField.SelectedIndex))
        End If

        Return value

    End Function

    Private Function GetComboDbString(ByRef cmbField As System.Windows.Forms.ComboBox) As String

        Dim value As String = "NULL"

        If cmbField.SelectedIndex = -1 Then
            value = "NULL"
        Else
            value = VB6.GetItemString(cmbField, cmbField.SelectedIndex)
        End If

        Return value

    End Function

    Private Sub SetFourLevelHierarchyComboboxesActive(Optional ByVal isActive As Boolean = True)
        ' show the bottom two comboboxes if the region uses a four level hierarchy
        ' panelFourLevel.Visible = _usesFourLevelHierarchy

        ' disable and hide the bottom two comboboxes if the region uses a four level hierarchy
        Me.lblLevel3.Visible = _usesFourLevelHierarchy
        Me.cmbLevel3.Visible = _usesFourLevelHierarchy
        Me.lblLevel4.Visible = _usesFourLevelHierarchy
        Me.cmbLevel4.Visible = _usesFourLevelHierarchy
        Me.cmdAddLevel3.Visible = _usesFourLevelHierarchy
        Me.cmdAddLevel4.Visible = _usesFourLevelHierarchy

        Me.lblLevel3.Enabled = isActive AndAlso _usesFourLevelHierarchy
        Me.cmbLevel3.Enabled = isActive AndAlso _usesFourLevelHierarchy
        Me.lblLevel4.Enabled = isActive AndAlso _usesFourLevelHierarchy
        Me.cmbLevel4.Enabled = isActive AndAlso _usesFourLevelHierarchy
        Me.cmdAddLevel3.Enabled = isActive AndAlso _usesFourLevelHierarchy
        Me.cmdAddLevel4.Enabled = isActive AndAlso _usesFourLevelHierarchy

    End Sub

#End Region

#Region "Event Handlers"

    Private Sub HierarchySelector_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Got to do this to keep it from running since the load event even fires in design mode!
        If Not Me.DesignMode Then
            Initialize()
        End If

        ' for some reason one of the combobox is white and the other is grey when
        ' they are disabled
        ' so let's force them to white
        If Not _usesFourLevelHierarchy Then

            Me.cmbLevel3.BackColor = Color.White
            Me.cmbLevel4.BackColor = Color.White

        End If

    End Sub

    Private Sub cmbSubTeam_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.SelectedIndexChanged

        RaiseEvent HierarchySelectionChanged()

        If Not _isInitializing Then

            If cmbSubTeam.SelectedIndex = -1 Then
                cmbCategory.Items.Clear()
            Else
                LoadCategory(cmbCategory, VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
            End If

            cmbLevel3.Items.Clear()
            cmbLevel4.Items.Clear()

        End If

    End Sub

    Private Sub cmbCategory_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCategory.SelectedIndexChanged

        RaiseEvent HierarchySelectionChanged()

        If Not _isInitializing Then

            ' Don't build the level 3 combobox if the region doesn't show all four hierarchy levels.
            If _usesFourLevelHierarchy Then

                If cmbCategory.SelectedIndex = -1 Then

                    cmbLevel3.Items.Clear()
                Else
                    LoadProdHierarchyLevel3s(cmbLevel3, VB6.GetItemData(cmbCategory, cmbCategory.SelectedIndex))
                End If

                cmbLevel4.Items.Clear()

            Else

                ' only raise the CategoryChanged event if the region does not use four hierarchy levels
                RaiseEvent CategoryChanged()

                SetPreviousIds()

            End If

        End If

    End Sub

    Private Sub cmbLevel3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLevel3.SelectedIndexChanged

        RaiseEvent HierarchySelectionChanged()

        If Not _isInitializing Then

            If cmbLevel3.SelectedIndex = -1 Then

                cmbLevel4.Items.Clear()
            Else
                LoadProdHierarchyLevel4s(cmbLevel4, VB6.GetItemData(cmbLevel3, cmbLevel3.SelectedIndex))
            End If

        End If

    End Sub

    Private Sub cmbSubTeam_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.TextChanged

        If cmbSubTeam.Text = "" Then cmbSubTeam.SelectedIndex = -1

    End Sub

    Private Sub cmbSubTeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubTeam.KeyPress

        If Asc(e.KeyChar) = 8 Then cmbSubTeam.SelectedIndex = -1

    End Sub

    Private Sub cmbCategory_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbCategory.KeyPress

        If Asc(e.KeyChar) = 8 Then cmbCategory.SelectedIndex = -1

    End Sub

    Private Sub cmbLevel3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbLevel3.KeyPress

        If Asc(e.KeyChar) = 8 Then cmbLevel3.SelectedIndex = -1

    End Sub

    Private Sub cmbLevel4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbLevel4.KeyPress

        If Asc(e.KeyChar) = 8 Then cmbLevel4.SelectedIndex = -1

    End Sub

    Private Sub cmdAddCat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddCat.Click

        If cmbSubTeam.SelectedIndex <> -1 Then

            Dim cancelableEventArg As New CancelableEventArgs()
            RaiseEvent AddHierarchyNode(cancelableEventArg)

            If Not cancelableEventArg.Cancel Then
                AddCategory(cmbCategory, VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
            End If

        End If

    End Sub

    Private Sub cmdAddLevel3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddLevel3.Click

        If cmbCategory.SelectedIndex <> -1 Then

            Dim cancelableEventArg As New CancelableEventArgs()
            RaiseEvent AddHierarchyNode(cancelableEventArg)

            If Not cancelableEventArg.Cancel Then
                AddLevel3(cmbLevel3, VB6.GetItemData(cmbCategory, cmbCategory.SelectedIndex), VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
            End If

        End If

    End Sub

    Private Sub cmdAddLevel4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddLevel4.Click

        If cmbLevel3.SelectedIndex <> -1 Then

            Dim cancelableEventArg As New CancelableEventArgs()
            RaiseEvent AddHierarchyNode(cancelableEventArg)

            If Not cancelableEventArg.Cancel Then
                AddLevel4(cmbLevel4, VB6.GetItemData(cmbLevel3, cmbLevel3.SelectedIndex), VB6.GetItemData(cmbCategory, cmbCategory.SelectedIndex))
            End If

        End If

    End Sub

    Private Sub cmbLevel4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLevel4.SelectedIndexChanged

        RaiseEvent HierarchySelectionChanged()
        RaiseEvent Level4Changed()

        SetPreviousIds()

    End Sub

	Private Sub chkSubTeam_Click(sender As Object, e As EventArgs) Handles chkSubTeam.Click
		Dim categoryIndex = cmbCategory.SelectedIndex
		RefreshSubteamCombo(cmbSubTeam, Nothing, chkSubTeam.Checked)

		Try
			If (cmbSubTeam.SelectedIndex > -1) Then cmbCategory.SelectedIndex = categoryIndex
        Catch ex As Exception
			cmbCategory.SelectedIndex = -1
		End Try

	End Sub
#End Region

End Class

''' <summary>
''' This is used as an argument for the AddHierarchyNode event the HierarchySelector control above
''' publishes and allows a form that handles the event to cancel the adding of a node in the hierarchy.
''' This is done, for example, in the DefaultAttributeValues form.
''' </summary>
''' <remarks></remarks>
Public Class CancelableEventArgs
    Inherits System.EventArgs

    Private _cancel As Boolean = False

    Public Property Cancel() As Boolean
        Get
            Return _cancel
        End Get
        Set(ByVal value As Boolean)
            _cancel = value
        End Set
    End Property

End Class

