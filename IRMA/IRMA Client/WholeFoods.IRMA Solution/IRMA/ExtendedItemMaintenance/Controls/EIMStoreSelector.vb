
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.IRMA.ModelLayer
Imports Infragistics.Win.UltraWinGrid

Public Class EIMStoreSelector

#Region "Public Events"

    Public Delegate Sub SelectionChangedEventHandler()
    Public Event SelectionChanged As SelectionChangedEventHandler

#End Region

#Region "Public Enums"

    Public Enum LayoutStyles
        Horizontal
        Vertical
    End Enum

#End Region
#Region "Private Enums"

    Private Enum SyncDirections
        ToOtherStoreSelector
        FromOtherStoreSelector
    End Enum

#End Region

#Region "Fields and Properties"

    Private _populatingCombos As Boolean
    Private _isInitializing As Boolean
    Private _isSettingSelection As Boolean
    Private _storesDataTable As DataTable
    Private _currentUploadSession As UploadSession
    Private _currentUploadTypeCode As String
    Private _allowStoreSelection As Boolean = True
    Private _otherStoreSelector As EIMStoreSelector
    Private _layoutStyle As LayoutStyles = LayoutStyles.Horizontal

    Public WriteOnly Property SettingSelection() As Boolean
        Set(ByVal value As Boolean)
            _isSettingSelection = value
        End Set
    End Property

    Public Property CurrentUploadTypeCode() As String
        Get
            Return _currentUploadTypeCode
        End Get
        Set(ByVal value As String)
            _currentUploadTypeCode = value

            If Not IsNothing(_currentUploadTypeCode) Then
                If _currentUploadTypeCode.Equals(EIM_Constants.PRICE_UPLOAD_CODE) Then
                    Me.ButtonCopyTo.Text = "Copy to Cost Upload"
                    Me.ButtonCopyFrom.Text = "Copy from Cost Upload"
                Else
                    Me.ButtonCopyTo.Text = "Copy to Price Upload"
                    Me.ButtonCopyFrom.Text = "Copy from Price Upload"
                End If
            End If

        End Set
    End Property

    Public Property CurrentUploadSession() As UploadSession
        Get
            Return _currentUploadSession
        End Get
        Set(ByVal value As UploadSession)
            _currentUploadSession = value

            If Not IsNothing(value) And Me.AllowStoreSelection Then

                LoadDataToUI()

                ' disable the entire control if there is no
                ' corresponding UploadType for the current UploadSession
                Dim theCurrentUploadSessionUploadType As UploadSessionUploadType = GetTheCurrentUploadSessionUploadType()

                ' disable the sync buttons if there is no
                ' corresponding UploadType for the other UploadSession
                Dim theOtherUploadSessionUploadType As UploadSessionUploadType = GetTheOtherUploadSessionUploadType()
                Me.ButtonCopyFrom.Enabled = Not IsNothing(theOtherUploadSessionUploadType) And Me.AllowStoreSelection
                Me.ButtonCopyTo.Enabled = Not IsNothing(theOtherUploadSessionUploadType) And Me.AllowStoreSelection
            End If

        End Set
    End Property

    Public Overloads Property Enabled() As Boolean
        Get
            Return MyBase.Enabled
        End Get
        Set(ByVal value As Boolean)

            MyBase.Enabled = value

        End Set
    End Property

    Public Property AllowStoreSelection() As Boolean
        Get
            Return _allowStoreSelection
        End Get
        Set(ByVal value As Boolean)
            _allowStoreSelection = value

            Me.Enabled = value

        End Set
    End Property

    Public Property LayoutStyle() As LayoutStyles
        Get
            Return _layoutStyle
        End Get
        Set(ByVal value As LayoutStyles)
            _layoutStyle = value
        End Set
    End Property

    Public Property OtherStoreSelector() As EIMStoreSelector
        Get
            Return _otherStoreSelector
        End Get
        Set(ByVal value As EIMStoreSelector)
            _otherStoreSelector = value
        End Set
    End Property

    Public ReadOnly Property HasStoreSelection() As Boolean
        Get
            Dim doesHaveStoreSelection As Boolean = False

            If Not IsNothing(Me.CurrentUploadSession) Then

                Me.LoadDataFromUI()

                Dim theUploadSessionUploadType As UploadSessionUploadType = Me.CurrentUploadSession.FindUploadSessionUploadType(Me.CurrentUploadTypeCode, True)

                If Not IsNothing(theUploadSessionUploadType) Then

                    Dim theSelectedStoreCount As Integer = 0

                    For Each theUploadSessionUploadTypeStore As UploadSessionUploadTypeStore In _
                            theUploadSessionUploadType.UploadSessionUploadTypeStoreCollection

                        If Not theUploadSessionUploadTypeStore.IsMarkedForDelete Then
                            theSelectedStoreCount = theSelectedStoreCount + 1
                        End If
                    Next

                    doesHaveStoreSelection = theSelectedStoreCount > 0
                End If
            Else
                doesHaveStoreSelection = Me.UltraGrid1.Selected.Rows.Count > 0
            End If

            Return doesHaveStoreSelection

        End Get
    End Property

    ''' <summary>
    ''' Returns a comma delimeted string of selected store ids.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SelectedStoreIdString() As String
        Get
            Dim theSelectedStoreIdString As String = ""

            Dim theIndex As Integer = 0
            For Each theGridRow As UltraGridRow In Me.UltraGrid1.Selected.Rows

                theIndex = theIndex + 1

                theSelectedStoreIdString = theSelectedStoreIdString + CStr(theGridRow.Cells("Store_No").Value)

                If theIndex < Me.UltraGrid1.Selected.Rows.Count Then
                    theSelectedStoreIdString = theSelectedStoreIdString + ","
                End If
            Next

            Return theSelectedStoreIdString

        End Get
    End Property

#End Region

#Region "Event Handlers"

    Private Sub StoreSelector_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Me.DesignMode Then
            LoadZone(cmbZones)

            '-- Fill out the store list
            _storesDataTable = StoreDAO.GetRetailStoreList()
            UltraGrid1.DataSource = _storesDataTable

            StoreListGridLoadStatesCombo(_storesDataTable, cmbStates)

            SetComboEnabledStates()

            If Not CheckAllStoreSelectionEnabled() Then
                AllRadioButton.Text = "All 365"
            End If

            Me.LoadDataToUI()

        End If

        If Me.LayoutStyle = LayoutStyles.Vertical Then

            Me.UltraGrid1.Location = New Point(3, 105)
            Me.UltraGrid1.Size = New Size(219, 96)
        Else

            Me.UltraGrid1.Location = New Point(228, 3)
            Me.UltraGrid1.Size = New Size(173, 96)
        End If

    End Sub


    Private Sub cmbStates_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbStates.SelectedIndexChanged
        Dim iFirstStore As Short

        If _isInitializing Or _populatingCombos Then Exit Sub

        iFirstStore = -1

        _populatingCombos = True

        StoreListGridSelectByState(UltraGrid1, VB6.GetItemString(cmbStates, cmbStates.SelectedIndex))

        _populatingCombos = False


    End Sub

    Private Sub cmbZones_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbZones.SelectedIndexChanged

        If _populatingCombos Or _isInitializing Then Exit Sub
        SetComboEnabledStates()

        _isSettingSelection = True

        If cmbZones.SelectedIndex > -1 Then
            StoreListGridSelectByZone(UltraGrid1, VB6.GetItemData(cmbZones, cmbZones.SelectedIndex))
        Else
            UltraGrid1.Selected.Rows.Clear()
        End If

        _isSettingSelection = False

    End Sub

    Private Sub AllRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllRadioButton.CheckedChanged

        If _isSettingSelection Or _populatingCombos Or _isInitializing Then Exit Sub

        _isSettingSelection = True

        '-- All Stores or All 365 for RM
        If CheckAllStoreSelectionEnabled() Then
            StoreListGridSelectAll(UltraGrid1, True)
        Else
            StoreListGridSelectAll365(UltraGrid1)
        End If

        _isSettingSelection = False

    End Sub

    Private Sub ManualRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManualRadioButton.CheckedChanged

        If _isSettingSelection Or _populatingCombos Or _isInitializing Then Exit Sub

        ClearSelection()
    End Sub

    Private Sub AllWFMRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllWFMRadioButton.CheckedChanged

        If _isSettingSelection Or _populatingCombos Or _isInitializing Then Exit Sub

        _isSettingSelection = True

        StoreListGridSelectAllWFM(UltraGrid1)

        _isSettingSelection = False

    End Sub

    Private Sub ZoneRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZoneRadioButton.CheckedChanged, _
         StateRadioButton.CheckedChanged

        If _isSettingSelection Or _populatingCombos Or _isInitializing Then Exit Sub

        SetComboEnabledStates()

    End Sub

    Private Sub UltraGrid1_AfterSelectChange(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles UltraGrid1.AfterSelectChange

        If _isSettingSelection Or _populatingCombos Or _isInitializing Then Exit Sub

        ManualRadioButton.Checked = True

        RaiseEvent SelectionChanged()

    End Sub

    Private Sub ButtonCopyFrom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCopyFrom.Click

        Synchronize(SyncDirections.FromOtherStoreSelector)

    End Sub

    Private Sub ButtonCopyTo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCopyTo.Click

        Synchronize(SyncDirections.ToOtherStoreSelector)

    End Sub

#End Region

#Region "Public Methods"

    Public Sub ClearSelection()

        _isSettingSelection = True

        UltraGrid1.Selected.Rows.Clear()
        cmbZones.SelectedIndex = -1
        cmbStates.SelectedIndex = -1

        ' clear all selected stores
        ' *if* there are current upload type and session assigned
        If Not IsNothing(Me.CurrentUploadSession) And Not IsNothing(Me.CurrentUploadTypeCode) Then

            Dim theCurrentUploadSessionUploadType As UploadSessionUploadType = _
                Me.CurrentUploadSession.FindUploadSessionUploadType(Me.CurrentUploadTypeCode, True)

            If Not IsNothing(theCurrentUploadSessionUploadType) Then

                For Each theCurrentUploadSessionUploadTypeStore As UploadSessionUploadTypeStore In _
                        theCurrentUploadSessionUploadType.UploadSessionUploadTypeStoreCollection

                    theCurrentUploadSessionUploadTypeStore.IsMarkedForDelete = True
                Next
            End If
        End If

        _isSettingSelection = False

    End Sub

    ''' <summary>
    ''' Load the data from the StoreSelector controls
    ''' into the CurrentUploadSession.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadDataFromUI()

        If Not IsNothing(Me.CurrentUploadSession) Then

            Dim theUploadSessionUploadType As UploadSessionUploadType = Me.CurrentUploadSession.FindUploadSessionUploadType(Me.CurrentUploadTypeCode, True)

            If Not IsNothing(theUploadSessionUploadType) Then

                ' get the store selection type
                If Me.AllRadioButton.Checked Then
                    theUploadSessionUploadType.StoreSelectionType = "allstores"
                ElseIf Me.AllWFMRadioButton.Checked Then
                    theUploadSessionUploadType.StoreSelectionType = "allwfm"
                ElseIf Me.ZoneRadioButton.Checked Then
                    theUploadSessionUploadType.StoreSelectionType = "byzone"
                ElseIf Me.StateRadioButton.Checked Then
                    theUploadSessionUploadType.StoreSelectionType = "bystate"
                ElseIf Me.ManualRadioButton.Checked Then
                    theUploadSessionUploadType.StoreSelectionType = "manual"
                End If

                ' now get the zone or state
                theUploadSessionUploadType.ZoneID = ComboVal(Me.cmbZones)
                If theUploadSessionUploadType.ZoneID = 0 Then
                    theUploadSessionUploadType.IsZoneIDNull = True
                End If

                theUploadSessionUploadType.State = VB6.GetItemString(Me.cmbStates, cmbStates.SelectedIndex)
                If String.IsNullOrEmpty(theUploadSessionUploadType.State) Then
                    theUploadSessionUploadType.IsStateNull = True
                End If

                ' and mark all existing UploadSessionUploadTypeStore for delete
                ' they will be deleted when the UploadSession is saved
                ' we'll add new ones below
                For Each theUploadSessionUploadTypeStore As UploadSessionUploadTypeStore In _
                            theUploadSessionUploadType.UploadSessionUploadTypeStoreCollection

                    theUploadSessionUploadTypeStore.IsMarkedForDelete = True
                Next

                ' now get the stores
                For Each theGridRow As UltraGridRow In Me.UltraGrid1.Selected.Rows
                    Dim theUploadSessionUploadTypeStore As UploadSessionUploadTypeStore = _
                            New UploadSessionUploadTypeStore(theUploadSessionUploadType)

                    theUploadSessionUploadTypeStore.StoreNo = CInt(theGridRow.Cells("Store_No").Value)
                Next

            End If
        End If

    End Sub

#End Region

#Region "Private Methods"

    Private Function GetTheCurrentUploadSessionUploadType() As UploadSessionUploadType

        Dim theCurrentUploadSessionUploadType As UploadSessionUploadType = Nothing

        theCurrentUploadSessionUploadType = Me.CurrentUploadSession.FindUploadSessionUploadType(Me.CurrentUploadTypeCode, True)

        Return theCurrentUploadSessionUploadType

    End Function

    Private Function GetTheOtherUploadSessionUploadType() As UploadSessionUploadType

        Dim theOtherUploadSessionUploadType As UploadSessionUploadType = Nothing
        Dim theOtherUploadTypeCode As String

        ' get the other UploadTypeCode
        If Me.CurrentUploadTypeCode.Equals(EIM_Constants.PRICE_UPLOAD_CODE) Then
            theOtherUploadTypeCode = EIM_Constants.COST_UPLOAD_CODE
        Else
            theOtherUploadTypeCode = EIM_Constants.PRICE_UPLOAD_CODE
        End If

        theOtherUploadSessionUploadType = Me.CurrentUploadSession.FindUploadSessionUploadType(theOtherUploadTypeCode, True)

        Return theOtherUploadSessionUploadType

    End Function

    Private Sub Synchronize(ByVal inSyncDirection As SyncDirections)

        If Not IsNothing(Me.CurrentUploadSession) Then

            Dim thisUploadSessionUploadType As UploadSessionUploadType = Me.CurrentUploadSession.FindUploadSessionUploadType(Me.CurrentUploadTypeCode, True)
            Dim theOtherUploadSessionUploadType As UploadSessionUploadType = GetTheOtherUploadSessionUploadType()

            Dim theFromUploadSessionUploadType As UploadSessionUploadType
            Dim theToUploadSessionUploadType As UploadSessionUploadType

            If inSyncDirection = SyncDirections.FromOtherStoreSelector Then
                theFromUploadSessionUploadType = theOtherUploadSessionUploadType
                theToUploadSessionUploadType = thisUploadSessionUploadType
            Else
                theFromUploadSessionUploadType = thisUploadSessionUploadType
                theToUploadSessionUploadType = theOtherUploadSessionUploadType
            End If

            If Not IsNothing(theFromUploadSessionUploadType) And Not IsNothing(theToUploadSessionUploadType) Then

                Me.LoadDataFromUI()

                theToUploadSessionUploadType.StoreSelectionType = theFromUploadSessionUploadType.StoreSelectionType
                theToUploadSessionUploadType.ZoneID = theFromUploadSessionUploadType.ZoneID
                theToUploadSessionUploadType.IsZoneIDNull = theFromUploadSessionUploadType.IsZoneIDNull
                theToUploadSessionUploadType.State = theFromUploadSessionUploadType.State
                theToUploadSessionUploadType.IsStateNull = theFromUploadSessionUploadType.IsStateNull

                ' and mark all existing UploadSessionUploadTypeStore for delete
                ' they will be deleted when the UploadSession is saved
                ' we'll add new ones below
                For Each theUploadSessionUploadTypeStore As UploadSessionUploadTypeStore In _
                            theToUploadSessionUploadType.UploadSessionUploadTypeStoreCollection

                    theUploadSessionUploadTypeStore.IsMarkedForDelete = True
                Next

                ' add the new ones
                Dim theNewUploadSessionUploadTypeStore As UploadSessionUploadTypeStore
                'add the stores from the old to the new
                For Each theUploadSessionUploadTypeStore As UploadSessionUploadTypeStore In _
                    theFromUploadSessionUploadType.UploadSessionUploadTypeStoreCollection

                    ' copy it
                    theNewUploadSessionUploadTypeStore = CType(theUploadSessionUploadTypeStore.GetNewCopy(), UploadSessionUploadTypeStore)
                    theNewUploadSessionUploadTypeStore.UploadSessionUploadTypeStoreID = UploadSessionUploadTypeStore.NextTemporaryId
                    theNewUploadSessionUploadTypeStore.IsNew = True

                    ' set its parent and add it to the parents collection
                    theNewUploadSessionUploadTypeStore.UploadSessionUploadType = theToUploadSessionUploadType
                    theToUploadSessionUploadType.AddUploadSessionUploadTypeStore(theNewUploadSessionUploadTypeStore)

                Next

                ' now tell the StoreSelectors to update their controls
                Me.LoadDataToUI()
                OtherStoreSelector.LoadDataToUI()

                ' give the user some feedback by flashing the control
                Me.Enabled = False
                Thread.Sleep(500)
                Me.Enabled = True

            End If
        End If

    End Sub

    Private Sub SetComboEnabledStates()

        _populatingCombos = True

        'Zones.
        If ZoneRadioButton.Checked = True Then
            cmbZones.Enabled = True
            cmbZones.BackColor = System.Drawing.SystemColors.Window
        Else
            cmbZones.SelectedIndex = -1
            cmbZones.Enabled = False
            cmbZones.BackColor = System.Drawing.SystemColors.Control
        End If

        'States.
        If StateRadioButton.Checked = True Then
            cmbStates.Enabled = True
            cmbStates.BackColor = System.Drawing.SystemColors.Window
        Else
            cmbStates.SelectedIndex = -1
            cmbStates.Enabled = False
            cmbStates.BackColor = System.Drawing.SystemColors.Control
        End If

        _populatingCombos = False

    End Sub

    ''' <summary>
    ''' Load the data from the CurrentUploadSession
    '''  into the StoreSelector controls.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadDataToUI()

        If Not IsNothing(Me.CurrentUploadSession) Then
            _populatingCombos = True
            _isSettingSelection = True

            If Not IsNothing(_currentUploadSession) And Not IsNothing(CurrentUploadTypeCode) Then

                Dim theUploadSessionUploadType As UploadSessionUploadType = Me.CurrentUploadSession.FindUploadSessionUploadType(Me.CurrentUploadTypeCode, True)

                If Not IsNothing(theUploadSessionUploadType) Then

                    ' set the store selection type radio buttons
                    If Not IsNothing(theUploadSessionUploadType.StoreSelectionType) Then
                        If theUploadSessionUploadType.StoreSelectionType.ToLower().Equals("allstores") Then
                            Me.AllRadioButton.Checked = True
                        ElseIf theUploadSessionUploadType.StoreSelectionType.ToLower().Equals("allwfm") Then
                            Me.AllWFMRadioButton.Checked = True
                        ElseIf theUploadSessionUploadType.StoreSelectionType.ToLower().Equals("byzone") Then
                            Me.ZoneRadioButton.Checked = True
                        ElseIf theUploadSessionUploadType.StoreSelectionType.ToLower().Equals("bystate") Then
                            Me.StateRadioButton.Checked = True
                        ElseIf theUploadSessionUploadType.StoreSelectionType.ToLower().Equals("manual") Then
                            Me.ManualRadioButton.Checked = True
                        End If
                    End If

                    ' now set the zone or state
                    If Not theUploadSessionUploadType.IsZoneIDNull Then
                        SetCombo(Me.cmbZones, theUploadSessionUploadType.ZoneID)
                    ElseIf Not theUploadSessionUploadType.IsStateNull Then
                        SetStateCombo(theUploadSessionUploadType.State)
                    End If

                    SetComboEnabledStates()

                    ' clear any current store selection
                    Me.UltraGrid1.Selected.Rows.Clear()

                    ' now set the stores
                    For Each theUploadSessionUploadTypeStore As UploadSessionUploadTypeStore In _
                            theUploadSessionUploadType.UploadSessionUploadTypeStoreCollection

                        If Not theUploadSessionUploadTypeStore.IsMarkedForDelete Then
                            For Each theGridRow As UltraGridRow In Me.UltraGrid1.Rows
                                If CInt(theGridRow.Cells("Store_No").Value) = theUploadSessionUploadTypeStore.StoreNo Then
                                    Me.UltraGrid1.Selected.Rows.Add(theGridRow)
                                    Exit For
                                End If
                            Next
                        End If
                    Next
                End If
            End If

            Me.Invalidate()
            _populatingCombos = False
            _isSettingSelection = False
        End If

    End Sub

    Private Sub SetStateCombo(ByRef inState As String)

        Dim Locked As Boolean
        Dim iLoop As Integer

        If IsNothing(inState) Then inState = String.Empty

        'Save the state of the combo (Locked or not).
        Locked = Not Me.cmbStates.Enabled
        Me.cmbStates.Enabled = True

        If inState = String.Empty Then
            Me.cmbStates.SelectedIndex = -1
        Else
            For iLoop = 0 To Me.cmbStates.Items.Count - 1
                '-- See if its the right data
                If VB6.GetItemString(Me.cmbStates, iLoop) = inState Then
                    '-- if so then set and exit
                    Me.cmbStates.SelectedIndex = iLoop
                    Exit For
                End If
            Next iLoop
        End If

        'Set the state back.
        Me.cmbStates.Enabled = Not Locked

    End Sub


#End Region

End Class
