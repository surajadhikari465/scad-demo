Option Strict Off
Option Explicit On

Imports Infragistics.Shared
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinDataSource
Imports Infragistics.Win.UltraWinGrid
Imports WholeFoods.IRMA.Common.BusinessLogic
Imports WholeFoods.IRMA.Common.DataAccess

Friend Class AvgCostStoreSelector
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean
    Private mbFilling As Boolean

    Private mStoreList As New List(Of Integer)

    Private Sub AvgCostStoreSelector_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        LoadStoresPopulateGrid()

        LoadZone(Me.cmbZones)
        LoadStates(Me.cmbStates)

        SetCombos()
        SetupGrid()

    End Sub

#Region "Properties"


    Public Property StoreList() As List(Of Integer)
        Get
            Return mStoreList
        End Get
        Set(ByVal value As List(Of Integer))
            mStoreList = value
        End Set
    End Property



#End Region

#Region "Grid Code"

    Private Sub SetupGrid()
        'Grid must be setup at run time since it is bound and uses the datasource as the source for columns.

        ugrdStoreList.DisplayLayout.Bands(0).Columns("Store_No").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("Store_Name").Width = 130
        ugrdStoreList.DisplayLayout.Bands(0).Columns("Zone_ID").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("Zone_Name").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("State").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("WFM_Store").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("Mega_Store").Hidden = True
        ugrdStoreList.DisplayLayout.Bands(0).Columns("Region_Id").Hidden = True

    End Sub



    Private Sub LoadStoresPopulateGrid()


        ' bind the grid to the history
        Me.ugrdStoreList.DataSource = AvgCostAdjBO.GetStoreList()

        If Me.ugrdStoreList.Rows.Count > 0 Then

            Me.ugrdStoreList.ActiveRow = Me.ugrdStoreList.Rows(0)
            Me.ugrdStoreList.Rows(0).Selected = True

            ' set security to allow an adjustment
            Me.ugrdStoreList.Enabled = gbCostAdmin OrElse gbSuperUser

        Else
            MessageBox.Show("There are no stores to select from for this item.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

    End Sub

#End Region

#Region "Button Code"

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        Me.Close()

    End Sub

    ''' <summary>
    ''' Select stores to apply the average cost adjustment.
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdSelect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelect.Click

        Dim row As Infragistics.Win.UltraWinGrid.UltraGridRow
        For Each row In ugrdStoreList.Selected.Rows

            mStoreList.Add(CInt(row.GetCellValue("Store_No")))

        Next row

        Me.Close()
    End Sub

#End Region

#Region "Combo Code"

    Private Sub SetCombos()

        mbFilling = True

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

        mbFilling = False

    End Sub

    Private Sub cmbStates_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbStates.SelectedIndexChanged

        If mbFilling Or IsInitializing Then Exit Sub

        mbFilling = True

        Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemString(cmbStates, cmbStates.SelectedIndex))

        mbFilling = False

    End Sub

    Private Sub cmbZones_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbZones.SelectedIndexChanged

        If mbFilling Or IsInitializing Then Exit Sub

        ZoneRadioButton.Checked = True

        Call SelectZone()

    End Sub
  
#End Region

#Region "Option Button Code"

    Private Sub ManualRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManualRadioButton.CheckedChanged
        If mbFilling Or IsInitializing Then Exit Sub

        Call SetCombos()

        mbFilling = True
        ugrdStoreList.Selected.Rows.Clear()
        cmbZones.SelectedIndex = -1
        cmbStates.SelectedIndex = -1

        mbFilling = False

    End Sub

    Private Sub ZoneRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZoneRadioButton.CheckedChanged
        If mbFilling Or IsInitializing Then Exit Sub

        Call SetCombos()
        Call SelectZone()

    End Sub

    Private Sub SelectZone()

        mbFilling = True

        ugrdStoreList.Selected.Rows.Clear()
        If cmbZones.SelectedIndex > -1 Then Call StoreListGridSelectByZone(ugrdStoreList, VB6.GetItemData(cmbZones, cmbZones.SelectedIndex))

        mbFilling = False

    End Sub


    Private Sub StateRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StateRadioButton.CheckedChanged
        If mbFilling Or IsInitializing Then Exit Sub

        Call SetCombos()

        mbFilling = True
        ugrdStoreList.Selected.Rows.Clear()
        If cmbStates.SelectedIndex > -1 Then Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemData(cmbStates, cmbStates.SelectedIndex))

        mbFilling = False

    End Sub

    Private Sub AllRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllRadioButton.CheckedChanged
        If mbFilling Or IsInitializing Then Exit Sub

        Call SetCombos()

        mbFilling = True
        ugrdStoreList.Selected.Rows.Clear()
        Call StoreListGridSelectAll(ugrdStoreList, True)

        mbFilling = False

    End Sub

    Private Sub AllWFMRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllWFMRadioButton.CheckedChanged
        If mbFilling Or IsInitializing Then Exit Sub

        Call SetCombos()

        mbFilling = True
        ugrdStoreList.Selected.Rows.Clear()
        Call StoreListGridSelectAllWFM(ugrdStoreList)

        mbFilling = False

    End Sub
#End Region

End Class
