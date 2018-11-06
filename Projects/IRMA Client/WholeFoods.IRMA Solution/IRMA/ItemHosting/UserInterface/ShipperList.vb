Option Strict Off
Option Explicit On

Imports log4net
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic

''' <summary>
''' Displays the contents of a Shipper and allows editing of its items.
''' </summary>
''' <remarks>Form classes that use Shipper functionality (get/set data) should use the Shipper class.</remarks>
Friend Class frmShipperList
    Inherits System.Windows.Forms.Form

#Region "Private Members"

    ''' <summary>
    ''' Log4Net logger for this class.
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private mdtShipperItems As DataTable

    ''' <summary>
    ''' Holds reference to the Shipper object containing the data needed to display this Shipper item list form.
    ''' </summary>
    ''' <remarks></remarks>
    Private _shipper As Shipper

#End Region

#Region "Constructors"

    ''' <summary>
    ''' Constructs a Shipper-List screen from a Shipper object.
    ''' </summary>
    ''' <param name="shp">Shipper object containing details for the Shipper itself and all the items it contains.</param>
    ''' <remarks></remarks>
    Public Sub New(ByRef shp As Shipper)
        _shipper = shp
        InitializeComponent()
    End Sub

#End Region

#Region "Private Form Event Handlers"

    Private Sub frmShipperList_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        CenterForm(Me)
        SetupDataTable()
        PopulateGrid()
    End Sub

    Private Sub cmdAddItem_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAddItem.Click
        ' TODO: Use event fired from item-search screen.  Something like this: AddHandler frmItemSearch.ItemSelected, AddressOf HandleItemSelected
        ' Show item-search screen.
        frmItemSearch.ShowDialog()

        If glItemID <> 0 Then
            Try
                _shipper.ValidateAddItem(glItemID)
            Catch ex As Exception
                ' If the item is not valid for addition, show msg and exit.
                MessageBox.Show(ex.Message, ShipperMessages.CAPTION_SHIPPER_ADD_ITEM, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End Try

            Dim addOK As Boolean = False
            ' This adds the item to the Shipper with a qty of 1, then displays the Shipper-Item edit screen to update the qty.
            ' This is two separate calls to DB that could be made more efficient.
            Try
                _shipper.AddItem(glItemID)
                addOK = True
            Catch ex As Exception
                ' Errors may vary, so we don't exit here because we want to refresh our grid as the last step.
                MessageBox.Show(ex.Message, ShipperMessages.CAPTION_SHIPPER_ADD_ITEM, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            ' -------------------------------------------------------
            ' Show Shipper-item edit screen.
            ' -------------------------------------------------------
            If addOK Then
                ShowEditScreen(glItemID, ShipperMessages.CAPTION_SHIPPER_ADD_ITEM)
            End If

            ' Refresh grid.
            PopulateGrid()
        End If
        frmItemSearch.Close()
    End Sub

    Private Sub cmdDeleteItem_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDeleteItem.Click
        If ugrdShipper.Selected.Rows.Count = 1 Then
            Dim deleteItemID As Integer = ugrdShipper.Selected.Rows(0).Cells("Item_Key").Value

            ' Confirm item removal.
            If MessageBox.Show(String.Format(ShipperMessages.CONFIRM_SHIPPER_DELETE_ITEM, _shipper.GetItem(deleteItemID).Desc, _shipper.GetItem(deleteItemID).Identifier), _
                    ShipperMessages.CAPTION_SHIPPER_DELETE_ITEM, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) _
                    = Windows.Forms.DialogResult.No Then
                Exit Sub
            End If

            ' Remove item.
            Try
                _shipper.DeleteItem(deleteItemID)
            Catch ex As Exception
                ' Errors may vary, so we don't exit here because we want to refresh our grid as the last step.
                MessageBox.Show(ex.Message, ShipperMessages.CAPTION_SHIPPER_DELETE_ITEM, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            ' Refresh grid.
            PopulateGrid()

        Else
            ' No item selected.
            MessageBox.Show(ShipperMessages.INFO_SCREEN_ACTION_SELECT_ONE_ITEM, ShipperMessages.CAPTION_SHIPPER_DELETE_ITEM, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Private Sub cmdEditItem_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEditItem.Click
        If ugrdShipper.Selected.Rows.Count = 1 Then
            ' Show Shipper-Item edit screen.
            ShowEditScreen(ugrdShipper.Selected.Rows(0).Cells("Item_Key").Value, ShipperMessages.CAPTION_SHIPPER_EDIT_ITEM)
            ' Refresh grid.
            PopulateGrid()
        Else
            ' Only one item can be selected.
            MessageBox.Show(ShipperMessages.INFO_SCREEN_ACTION_SELECT_ONE_ITEM, ShipperMessages.CAPTION_SHIPPER_DELETE_ITEM, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Private Sub ShowEditScreen(ByVal itemKey As Integer, ByVal msgCaption As String)
        ' Need to pass a ShipperItem object to the Shipper-item edit screen because it is the source of the info it displays.
        Dim fShipperEdit As frmShipperEdit
        Try
            ' Get item from Shipper to pass to edit screen.
            fShipperEdit = New frmShipperEdit(_shipper.GetItem(itemKey))
            fShipperEdit.ShowDialog()
            fShipperEdit.Dispose()
        Catch ex As Exception
            MessageBox.Show(ShipperMessages.ERROR_SHIPPERITEM_SCREEN_LOAD & vbCrLf & vbCrLf & ex.Message, msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ugrdShipper_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdShipper.DoubleClickRow
        If cmdEditItem.Enabled Then
            cmdEditItem.PerformClick()
        End If
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

#End Region

#Region "Private Instance Methods"

    Private Sub PopulateGrid()
        UpdateWindowTitle()

        Dim row As DataRow
        mdtShipperItems.Clear()
        logger.Debug("Populating ShipperList screen data grid with " & _shipper.ItemCount & " item(s)...")

        ' Get enum to iterate through Shipper items.
        Dim shipperList As ICollection = _shipper.GetItemList
        If shipperList.Count > 0 Then
            Dim shipperEnum As IEnumerator = shipperList.GetEnumerator()
            Dim item As ShipperItem

            While shipperEnum.MoveNext()

                ' Convert item to ShipperItem object.
                item = CType(shipperEnum.Current, ShipperItem)
                row = mdtShipperItems.NewRow
                row("Item_Key") = item.ItemKey
                row("Identifier") = item.Identifier
                row("Item_Description") = item.Desc
                row("Quantity") = item.Qty.ToString("###0.##") & " Units"
                mdtShipperItems.Rows.Add(row)
            End While

            mdtShipperItems.AcceptChanges()
            ugrdShipper.DataSource = mdtShipperItems

            If ugrdShipper.Rows.Count > 0 Then
                ugrdShipper.Rows(0).Selected = True
                ugrdShipper.DisplayLayout.Bands(0).Columns("Item_Description").PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand)
            End If
        End If

    End Sub

    Private Sub SetupDataTable()
        mdtShipperItems = New DataTable("ShipperItems")

        mdtShipperItems.Columns.Add(New DataColumn("Item_Key", GetType(Integer)))
        mdtShipperItems.Columns.Add(New DataColumn("Identifier", GetType(String)))
        mdtShipperItems.Columns.Add(New DataColumn("Item_Description", GetType(String)))
        mdtShipperItems.Columns.Add(New DataColumn("Quantity", GetType(String)))
    End Sub

    Private Sub UpdateWindowTitle()
        ' Add item desc of this Shipper to window title.
        Dim suffix As String = "s"
        If _shipper.ItemCount = 1 Then suffix = ""
        Me.Text = ShipperMessages.SHIPPER_LIST_WINDOW_TITLE & " - " & _shipper.Desc & " - " & _shipper.ItemCount & " Item" & suffix
    End Sub

#End Region

End Class