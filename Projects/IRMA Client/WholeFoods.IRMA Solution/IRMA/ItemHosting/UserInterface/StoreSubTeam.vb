Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.Common.DataAccess
Imports log4net
Public Class StoreSubTeam


    Dim _dataSet As DataSet
    Dim _TeamDataset As DataSet

    Dim _loading As Boolean = False

    Private _Store_No As Integer = -1
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Property Store_No() As Integer
        Get
            Return _Store_No
        End Get
        Set(ByVal value As Integer)
            _Store_No = value
        End Set
    End Property

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Sub New(ByVal Store_No As Integer)
        logger.Debug(" New Entry with Store_No=" + Store_No.ToString)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Store_No = Store_No

        logger.Debug("New Exit")


    End Sub

    Private Sub StoreSubTeam_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        logger.Debug("StoreSubTeam_Load Entry")
        LoadStoreInformation()
        LoadTeamInformation()
        If Me.Store_No > 0 Then LoadSubteamInformation()
        FormatGrid()
        SetFieldsStates()
        logger.Debug("StoreSubTeam_Load Exit")
    End Sub

    Private Sub LoadTeamInformation()
        logger.Debug("LoadTeamInformation Entry")

        Dim dao As TeamDAO = New TeamDAO
        _TeamDataset = dao.GetTeams()
        With UltraGrid_StoreTeamSubTeam.DisplayLayout
            .ValueLists.Add("TeamValues")
            With .ValueLists("TeamValues")
                .DisplayStyle = Infragistics.Win.ValueListDisplayStyle.DisplayText
                For Each item As DataRow In _TeamDataset.Tables(0).Rows
                    .ValueListItems.Add(item("Team_no").ToString, item("Team_Name").ToString)
                Next
            End With
        End With
        logger.Debug("LoadTeamInformation Exit")


    End Sub
    Private Sub LoadStoreInformation()

        logger.Debug("LoadStoreInformation Entry")

        Dim DAO As WholeFoods.IRMA.ItemHosting.DataAccess.StoreDAO = New WholeFoods.IRMA.ItemHosting.DataAccess.StoreDAO
        Dim DT As DataTable
        Dim DR As DataRow


        _loading = True
        'DT = StoreDAO.GetStoreList()
        DT = StoreDAO.GetStoreAndDCList()
        DR = DT.NewRow()
        DR("Store_no") = -1
        DR("store_name") = "< None >"
        DT.Rows.InsertAt(DR, 0)
        ComboBox_Store.DataSource = DT
        ComboBox_Store.DisplayMember = "Store_Name"
        ComboBox_Store.ValueMember = "Store_no"

        For Each item As DataRowView In ComboBox_Store.Items
            If Me.Store_No = CInt(item("Store_no")) Then
                ComboBox_Store.SelectedItem = item
            End If
        Next

        _loading = False

        logger.Debug("LoadStoreInformation Exit")

    End Sub
    Private Sub LoadSubteamInformation()

        logger.Debug("LoadSubteamInformation Entry")

        Dim DAO As StoreSubTeamDAO = New StoreSubTeamDAO
        _dataSet = DAO.GetSubTeamToTeamRelationships(Me.Store_No)

        Me.UltraGrid_StoreTeamSubTeam.DataSource = _dataSet.Tables(0)

        If Me.UltraGrid_StoreTeamSubTeam.Rows.Count > 0 Then
            'first row appears to be defaulted because it is highlighted, but it's not actually selected.
            'so if user tried to edit or delete upon entering the screen they must still click the highlighted row.
            'below code is actually selecting row that is already highlighted.
            Me.UltraGrid_StoreTeamSubTeam.Rows(0).Selected = True

            'Make the columns PS_Team_No and PS_SubTeam_No ready only if UKIPS is set to 1
            If InstanceDataDAO.IsFlagActive("UKIPS", glStoreID) Then
                Me.UltraGrid_StoreTeamSubTeam.DisplayLayout.Bands(0).Columns("PS_Team_No").CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
                Me.UltraGrid_StoreTeamSubTeam.DisplayLayout.Bands(0).Columns("PS_SubTeam_No").CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
            End If
            
        End If


        logger.Debug("LoadSubteamInformation Exit")


    End Sub

    Private Sub FormatGrid()


    End Sub

    Private Sub ComboBox_Store_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Store.SelectedIndexChanged

        logger.Debug("ComboBox_Store_SelectedIndexChanged Entry")

        Dim dr As DataRowView

        If Not _loading Then
            If Not ComboBox_Store.SelectedItem Is Nothing Then
                dr = CType(ComboBox_Store.SelectedItem, DataRowView)
                Me.Store_No = CInt(dr("store_no"))
                LoadSubteamInformation()
                FormatGrid()
            End If
        End If
        logger.Debug("ComboBox_Store_SelectedIndexChanged Exit")
    End Sub


    Private Sub UltraGrid_InitializeRow(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles UltraGrid_StoreTeamSubTeam.InitializeRow
        e.Row.Cells("Edit").Value = "Edit"

    End Sub

    Private Sub UltraGrid_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles UltraGrid_StoreTeamSubTeam.InitializeLayout

        logger.Debug("UltraGrid_InitializeLayout Entry")
        e.Layout.Bands(0).Columns("Edit").Width = 100
        e.Layout.Bands(0).Columns("Edit").Header.Caption = ""
        e.Layout.Bands(0).Columns("Edit").CellAppearance.Cursor = Cursors.Hand
        e.Layout.Bands(0).Columns("Edit").CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
        e.Layout.Bands(0).Columns("CostFactor").CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        e.Layout.Bands(0).Columns("ICVABBRV").CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Debug.WriteLine(e.Layout.Bands(0).Columns("Edit").CellButtonAppearance.BackColor.ToString)
        logger.Debug(" UltraGrid_InitializeLayout Exit")


    End Sub

    Private Sub UltraGrid_ClickCellButton(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles UltraGrid_StoreTeamSubTeam.ClickCellButton

        logger.Debug("UltraGrid_ClickCellButton Entry")

        Dim Row As Infragistics.Win.UltraWinGrid.UltraGridRow
        Dim Column As Infragistics.Win.UltraWinGrid.UltraGridColumn
        Dim Team_No As Integer = -1
        Dim SubTeam_No As Integer = -1
        Dim PS_Team_No As Integer = -1
        Dim PS_SubTeam_No As Integer = -1
        Dim CostFactor As Decimal = 0.0
        Dim ICVID As Integer = -1

        Row = e.Cell.Row
        Column = e.Cell.Row.Band.Columns("Team_No")
        Team_No = CInt(Row.Cells(UltraGrid_StoreTeamSubTeam.DisplayLayout.Bands(0).Columns(Column.Index).Index).Value)
        Column = e.Cell.Row.Band.Columns("SubTeam_No")
        SubTeam_No = CInt(Row.Cells(UltraGrid_StoreTeamSubTeam.DisplayLayout.Bands(0).Columns(Column.Index).Index).Value)
        Column = e.Cell.Row.Band.Columns("PS_Team_No")
        If Not Row.Cells(UltraGrid_StoreTeamSubTeam.DisplayLayout.Bands(0).Columns(Column.Index).Index).Value.Equals(DBNull.Value) Then
            PS_Team_No = CInt(Row.Cells(UltraGrid_StoreTeamSubTeam.DisplayLayout.Bands(0).Columns(Column.Index).Index).Value)
        End If
        Column = e.Cell.Row.Band.Columns("PS_SubTeam_No")
        If Not Row.Cells(UltraGrid_StoreTeamSubTeam.DisplayLayout.Bands(0).Columns(Column.Index).Index).Value.Equals(DBNull.Value) Then
            PS_SubTeam_No = CInt(Row.Cells(UltraGrid_StoreTeamSubTeam.DisplayLayout.Bands(0).Columns(Column.Index).Index).Value)
        End If
        Column = e.Cell.Row.Band.Columns("CostFactor")
        If Not Row.Cells(UltraGrid_StoreTeamSubTeam.DisplayLayout.Bands(0).Columns(Column.Index).Index).Value.Equals(DBNull.Value) Then
            CostFactor = CDec(Row.Cells(UltraGrid_StoreTeamSubTeam.DisplayLayout.Bands(0).Columns(Column.Index).Index).Value)
        End If
        Column = e.Cell.Row.Band.Columns("ICVID")
        If Not Row.Cells(UltraGrid_StoreTeamSubTeam.DisplayLayout.Bands(0).Columns(Column.Index).Index).Value.Equals(DBNull.Value) Then
            ICVID = CInt(Row.Cells(UltraGrid_StoreTeamSubTeam.DisplayLayout.Bands(0).Columns(Column.Index).Index).Value)
        End If

        Dim frm As StoreSubTeamEdit = New StoreSubTeamEdit(Me.Store_No, SubTeam_No, Team_No, PS_SubTeam_No, PS_Team_No, CostFactor, ICVID)
        frm.ShowDialog()
        frm.Dispose()
        LoadSubteamInformation()
        FormatGrid()

        logger.Debug("UltraGrid_ClickCellButton Exit")
    End Sub

    Private Sub Button_AddRelationship_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_AddRelationship.Click
        logger.Debug("Button_AddRelationship_Click Entry")

        If ComboBox_Store.SelectedIndex = 0 Then
            MsgBox("Choose a store to show Team/SubTeam relationships for.")
            logger.Info("Choose a store to show Team/SubTeam relationships for.")
            ComboBox_Store.Focus()
            Exit Sub
        End If

        Dim frm As New StoreSubTeamEdit(Me.Store_No)
        frm.ShowDialog()
        frm.Dispose()

        LoadSubteamInformation()
        FormatGrid()

        logger.Debug("Button_AddRelationship_Click Exit")

    End Sub

    ''' <summary>
    ''' Display an error message box using the custom error text.
    ''' </summary>
    ''' <param name="message"></param>
    ''' <remarks></remarks>
    Public Shared Sub DisplayErrorMessage(ByVal message As String)
        MessageBox.Show(message, ResourcesCommon.GetString("msg_titleGenericError"), MessageBoxButtons.OK)
    End Sub

    ''' <summary>
    ''' This function reads the selected row from a data grid.
    ''' The row can be selected by highlighting the entire row or a single cell
    ''' within the row.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getSelectedRow(ByRef dataGrid As DataGridView) As DataGridViewRow
        ' Get the selected row
        Dim selectedRow As DataGridViewRow = Nothing
        If (dataGrid.SelectedRows.Count = 1) Then
            Dim rowEnum As IEnumerator = dataGrid.SelectedRows.GetEnumerator
            rowEnum.MoveNext()
            selectedRow = CType(rowEnum.Current, DataGridViewRow)
        ElseIf (dataGrid.SelectedCells.Count = 1) Then
            Dim cellEnum As IEnumerator = dataGrid.SelectedCells.GetEnumerator
            cellEnum.MoveNext()
            Dim selectedCell As DataGridViewCell = CType(cellEnum.Current, DataGridViewCell)
            selectedRow = selectedCell.OwningRow
        Else
            ' Error condition
            DisplayErrorMessage("A row must be selected to perform this action.")
        End If
        Return selectedRow
    End Function

    Private Sub Button_DeleteRelationship_Click(sender As System.Object, e As System.EventArgs) Handles Button_DeleteRelationship.Click
        logger.Debug("Button_DeleteRelationship_Click Entry")

        If ComboBox_Store.SelectedIndex = 0 Then
            MsgBox("Choose a store to show Team/SubTeam relationships for.")
            logger.Info("Choose a store to show Team/SubTeam relationships for.")
            ComboBox_Store.Focus()
            Exit Sub
        End If

        'Remove the relationship.
        Dim dao As StoreSubTeamDAO = New StoreSubTeamDAO()

        If UltraGrid_StoreTeamSubTeam.Selected.Rows.Count = 1 Then
            If UltraGrid_StoreTeamSubTeam.Rows.Count = 1 Then
                If MsgBox(ResourcesItemHosting.GetString("msg_wanring_RemoveVendorItem"), MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    dao.RemoveSubTeamToTeamRelationship(Store_No, UltraGrid_StoreTeamSubTeam.Selected.Rows(0).Cells("SubTeam_No").Value, UltraGrid_StoreTeamSubTeam.Selected.Rows(0).Cells("Team_No").Value)
                End If
            Else
                dao.RemoveSubTeamToTeamRelationship(Store_No, UltraGrid_StoreTeamSubTeam.Selected.Rows(0).Cells("SubTeam_No").Value, UltraGrid_StoreTeamSubTeam.Selected.Rows(0).Cells("Team_No").Value)
            End If
        Else
            '-- No vendor was selected
            MsgBox(ResourcesItemHosting.GetString("VendorHighlight"), MsgBoxStyle.Exclamation, Me.Text)

            logger.Info(ResourcesItemHosting.GetString("VendorHighlight"))

        End If

        LoadSubteamInformation()
        FormatGrid()

        logger.Debug("Button_DeleteRelationship_Click Exit")
    End Sub

    Private Sub SetFieldsStates()
        If InstanceDataDAO.IsFlagActive("UKIPS", glStoreID) Then
            Me.Button_AddRelationship.Enabled = False
        Else
            Me.Button_AddRelationship.Enabled = True
        End If
    End Sub
End Class