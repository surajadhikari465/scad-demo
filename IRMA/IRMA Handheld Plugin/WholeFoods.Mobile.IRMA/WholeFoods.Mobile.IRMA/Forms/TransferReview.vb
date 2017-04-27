Imports System.Windows.Forms
Imports System
Imports System.Linq
Imports System.Data

Public Class TransferReview

    Private mySession As Session

    Public Sub New(ByVal session As Session)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.mySession = session
    End Sub

    Private Sub TransferReview_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim fileWriter As TransferFileWriter = New TransferFileWriter(Me.mySession)
        updateBatch(fileWriter)
        Me.lblFromStoreSubteam.Text = mySession.TransferFromStoreName + " " + mySession.TransferFromSubteam
        Me.lblToStoreSubteam.Text = mySession.TransferToStoreName + " " + mySession.TransferToSubteam
        Me.lblExpectedDate.Text = "Expected On: " + mySession.TransferExpectedDate
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim fileWriter As TransferFileWriter = New TransferFileWriter(Me.mySession)

        If dtgInProgressOrder.CurrentRowIndex >= 0 Then
            Dim cCell As DataGridCell = Me.dtgInProgressOrder.CurrentCell
            Dim upcSelected As Integer = cCell.RowNumber

            Dim upc As String = Me.dtgInProgressOrder.Item(upcSelected, 0).ToString()
            Dim qty As String = Me.dtgInProgressOrder.Item(upcSelected, 1).ToString()
            Dim desc As String = Me.dtgInProgressOrder.Item(upcSelected, 2).ToString()
            Dim uom As String = Me.dtgInProgressOrder.Item(upcSelected, 3).ToString()
            Dim totalCost As String = Me.dtgInProgressOrder.Item(upcSelected, 5).ToString()
            Dim vendorCost As String = Me.dtgInProgressOrder.Item(upcSelected, 4).ToString()
            Dim sbw As Boolean = Me.dtgInProgressOrder.Item(upcSelected, 6)

            Dim style = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Critical

            Dim msgText As String
            If sbw Then
                msgText = "Do you want to update the weight for the selected UPC?"
            Else
                msgText = "Do you want to update the quantity for the selected UPC?"
            End If

            Dim response = MsgBox(msgText, MsgBoxStyle.YesNo)
            If (response = MsgBoxResult.Yes) Then
                Dim frmUpdate As UpdateShrink = New UpdateShrink("Update Transfer")
                frmUpdate.UOM = uom
                frmUpdate.UPC = upc
                frmUpdate.Desc = desc
                frmUpdate.LastQtyRecorded = qty
                frmUpdate.VendorCost = vendorCost

                frmUpdate.ShowDialog()

                If frmUpdate.ShrinkQuantity <> String.Empty Then
                    fileWriter.ReplaceQuantity(upc, frmUpdate.ShrinkQuantity)

                    'update datagrid
                    updateBatch(fileWriter)
                End If

                frmUpdate.Close()
                frmUpdate.Dispose()
            Else
                'unselect item
                Me.dtgInProgressOrder.UnSelect(upcSelected)
            End If
        End If
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub mnuAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAdd.Click
        Dim frmTransferScan As TransferScan = New TransferScan(Me.mySession)

        Dim res As DialogResult = frmTransferScan.ShowDialog()
        If res = Windows.Forms.DialogResult.Abort Then
            Me.DialogResult = Windows.Forms.DialogResult.Abort
        ElseIf res = Windows.Forms.DialogResult.OK Or res = Windows.Forms.DialogResult.Cancel Then
            Me.Close()
        End If

        frmTransferScan.Dispose()

    End Sub

    Private Sub mnuSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSend.Click

        Cursor.Current = Cursors.WaitCursor
        Cursor.Show()
        Dim order As Order
        Dim oi As OrderItem

        If Me.mySession.SessionName Is Nothing Or dtgInProgressOrder.CurrentRowIndex < 0 Then
            MessageBox.Show("Transfer order is not available for upload", "Error")
            Cursor.Current = Cursors.Default
            Exit Sub
        End If

        Dim errorLoadFound As Boolean = False
        Dim fileWriter As TransferFileWriter = New TransferFileWriter(Me.mySession)

        Dim sndSession As Session = fileWriter.GetFileSession(fileWriter.MakeFilePath(Me.mySession.SessionName), Me.mySession)
        Dim datalist As List(Of XNode) = New List(Of XNode)

        fileWriter.GetShrinkList(datalist, Me.mySession.SessionName)

        Dim orderHeaderId As Integer = 0
        Dim sUtility As SessionUtility = New SessionUtility
        Dim success As Result = New Result

        order = New Order
        order.CreatedBy = sndSession.UserID
        order.ProductType_ID = sndSession.SelectedProductType
        order.OrderType_Id = 3  'Transfer Order
        order.Vendor_ID = sndSession.transferVendorId
        order.Transfer_SubTeam = sndSession.TransferFromSubteamKey
        order.ReceiveLocation_ID = sndSession.TransferToStoreNo
        order.PurchaseLocation_ID = sndSession.TransferToStoreNo
        If order.ProductType_ID = Enums.ProductType.OtherSupplies Then
            order.Transfer_To_SubTeam = sndSession.SelectedSupplySubteam
            order.SupplyTransferToSubTeam = sndSession.TransferToSubteamKey
        Else
            order.Transfer_To_SubTeam = sndSession.TransferToSubteamKey
            order.SupplyTransferToSubTeam = 0
        End If
        order.Fax_Order = False
        order.Expected_Date = sndSession.TransferExpectedDate
        order.Return_Order = False
        order.FromQueue = False
        order.DSDOrder = False

        Dim oiList As New List(Of OrderItem)

        For Each item As XElement In datalist
            oi = New OrderItem
            oi.QuantityOrdered = item.Attribute("QTY")
            oi.Item_Key = item.Attribute("ITEM_KEY")
            oi.QuantityUnit = item.Attribute("RETAIL_UOMID")
            oi.AdjustedCost = item.Attribute("ADJUSTED_COST")
            oi.ReasonCodeDetailID = item.Attribute("ADJUSTED_REASON")

            oiList.Add(oi)
        Next
        order.OrderItems = oiList.ToArray()

        Try
            success = Me.mySession.WebProxyClient.CreateTransferOrder(order)

            If (success.Status = False) Then
                errorLoadFound = True
                MessageBox.Show(success.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1)
            Else
                MsgBox(String.Format("PO# {0} created successfully", success.IRMA_PONumber), MsgBoxStyle.OkOnly, "PO Created")
                fileWriter.DeleteFile(fileWriter.MakeFilePath(Me.mySession.SessionName))
                If (Not Me.mySession.SessionName = Nothing) Then
                    Me.mySession.SessionName = Nothing
                End If
            End If

        Catch ex As Exception
            errorLoadFound = True
            Messages.ShowException(ex)
        End Try

        mySession.IsLoadedSession = False

        Cursor.Current = Cursors.Default
        Me.Close()
    End Sub

    Private Sub btnViewDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewDetails.Click
        Dim fileWriter As TransferFileWriter = New TransferFileWriter(Me.mySession)

        If dtgInProgressOrder.CurrentRowIndex >= 0 Then
            Dim cCell As DataGridCell = Me.dtgInProgressOrder.CurrentCell
            Dim upcSelected As Integer = cCell.RowNumber

            Dim upc As String = Me.dtgInProgressOrder.Item(upcSelected, 0).ToString()
            Dim qty As String = Me.dtgInProgressOrder.Item(upcSelected, 1).ToString()
            Dim desc As String = Me.dtgInProgressOrder.Item(upcSelected, 2).ToString()
            Dim uom As String = Me.dtgInProgressOrder.Item(upcSelected, 3).ToString()
            Dim vendorCost As String = Me.dtgInProgressOrder.Item(upcSelected, 4).ToString()
            Dim totalCost As String = Me.dtgInProgressOrder.Item(upcSelected, 5).ToString()
            Dim sbw As Boolean = Me.dtgInProgressOrder.Item(upcSelected, 6)
            Dim vendorPack As String = Me.dtgInProgressOrder.Item(upcSelected, 7).ToString()

            If (Not String.IsNullOrEmpty(upc)) Then
                Dim frmTransferScan As TransferScan = New TransferScan(Me.mySession)
                frmTransferScan.txtUpc.Text = upc
                frmTransferScan.txtUpc.Enabled = False
                frmTransferScan.cmdSearch.Enabled = False
                frmTransferScan.lblDescription.Text = desc
                frmTransferScan.txtQty.Text = qty
                frmTransferScan.txtQty.Enabled = False
                frmTransferScan.lblRetailUnitName.Text = uom
                frmTransferScan.lblVendorPackValue.Text = vendorPack
                frmTransferScan.lblItemCost.Text = vendorCost
                frmTransferScan.lblIQueuedQty.Text = "Queued: " + qty

                frmTransferScan.cmdSave.Visible = False
                frmTransferScan.cmdClear.Visible = False
                frmTransferScan.mnuMenu.Enabled = False
                frmTransferScan.mmuReview.Text = "Cancel"

                'frmTransferScan.ShowDialog()
                Dim res As DialogResult = frmTransferScan.ShowDialog()
                If res = Windows.Forms.DialogResult.Abort Then
                    Me.DialogResult = Windows.Forms.DialogResult.Abort
                ElseIf res = Windows.Forms.DialogResult.OK Or res = Windows.Forms.DialogResult.Cancel Then
                    Me.Close()
                End If

                frmTransferScan.Dispose()
            Else
                'unselect item
                Me.dtgInProgressOrder.UnSelect(upcSelected)
            End If
        End If
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Cursor.Current = Cursors.WaitCursor
        Dim fileWriter As TransferFileWriter = New TransferFileWriter(Me.mySession)

        Dim cCell As DataGridCell = Me.dtgInProgressOrder.CurrentCell
        Dim upcSelected As Integer = cCell.RowNumber

        If dtgInProgressOrder.CurrentRowIndex >= 0 Then
            Dim upc As String = Me.dtgInProgressOrder.Item(upcSelected, 0).ToString()

            Dim style = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or _
                MsgBoxStyle.Critical

            Dim response = MsgBox("Do you want to delete selected UPC?", style, "Alert")

            If (response = MsgBoxResult.Yes) Then
                Cursor.Current = Cursors.WaitCursor
                fileWriter.DeleteItem(Me.mySession.SessionName, upc)
                'update datagrid
                updateBatch(fileWriter)
                Cursor.Current = Cursors.Default
            Else
                'unselect item
                Me.dtgInProgressOrder.UnSelect(upcSelected)
            End If
        End If
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub updateBatch(ByVal fileWriter As TransferFileWriter)
        Cursor.Current = Cursors.WaitCursor
        Dim datalist As List(Of XNode) = New List(Of XNode)

        fileWriter.GetShrinkList(datalist, mySession.SessionName)


        Dim _result As New DataSet()
        _result.Tables.Add("results")
        _result.Tables("results").Columns.Add("UPC")
        _result.Tables("results").Columns.Add("QTY")
        _result.Tables("results").Columns.Add("DESC")
        _result.Tables("results").Columns.Add("UOM")
        _result.Tables("results").Columns.Add("UNIT_COST")
        _result.Tables("results").Columns.Add("TOT_COST")
        _result.Tables("results").Columns.Add("SBW")
        _result.Tables("results").Columns.Add("VENDOR_PACK")
        _result.Tables("results").Columns.Add("UOMID")
        _result.Tables("results").Columns.Add("ITEM_KEY")

        Dim tmp As String

        For Each item As XElement In datalist


            Dim newRow As DataRow = _result.Tables("results").NewRow()
            tmp = item.Attribute("UPC")
            newRow("UPC") = tmp
            tmp = item.Attribute("DESC")
            newRow("DESC") = tmp
            tmp = item.Attribute("QTY")
            newRow("QTY") = tmp
            tmp = item.Attribute("RETAIL_UOM")
            newRow("UOM") = tmp
            tmp = item.Attribute("SOLD_BY_WEIGHT")
            newRow("SBW") = tmp
            If Double.Parse(item.Attribute("VENDOR_COST")) > 0 Then
                tmp = item.Attribute("VENDOR_COST")
            Else
                tmp = item.Attribute("ADJUSTED_COST")
            End If
            newRow("UNIT_COST") = tmp
            newRow("TOT_COST") = (Double.Parse(item.Attribute("QTY")) * Double.Parse(tmp)).ToString()

            tmp = item.Attribute("VENDOR_PACK")
            newRow("VENDOR_PACK") = tmp
            tmp = item.Attribute("RETAIL_UOMID")
            newRow("UOMID") = tmp
            tmp = item.Attribute("ITEM_KEY")
            newRow("ITEM_KEY") = tmp

            _result.Tables("results").Rows.Add(newRow)
        Next

        Dim dv As DataView = New DataView(_result.Tables("results"))
        dv.Sort = "UPC"
        Me.dtgInProgressOrder.DataSource = dv

        'Me.DataGrid1.DataSource = New DataView(_result.Tables("results"))

        Me.dtgInProgressOrder.ColumnHeadersVisible = True
        Me.dtgInProgressOrder.RowHeadersVisible = True

        ' Create new Table Style.
        Dim ts As New DataGridTableStyle()
        ts.MappingName = "results"
        dtgInProgressOrder.TableStyles.Clear()
        dtgInProgressOrder.TableStyles.Add(ts)

        Me.dtgInProgressOrder.TableStyles("results").GridColumnStyles("UPC").Width = 142
        Me.dtgInProgressOrder.TableStyles("results").GridColumnStyles("QTY").Width = 60
        Me.dtgInProgressOrder.TableStyles("results").GridColumnStyles("DESC").Width = 218
        Me.dtgInProgressOrder.TableStyles("results").GridColumnStyles("TOT_COST").Width = 120

        'Hide the columns
        Me.dtgInProgressOrder.TableStyles("results").GridColumnStyles("UOM").Width = 0
        Me.dtgInProgressOrder.TableStyles("results").GridColumnStyles("SBW").Width = 0
        Me.dtgInProgressOrder.TableStyles("results").GridColumnStyles("UNIT_COST").Width = 0
        Me.dtgInProgressOrder.TableStyles("results").GridColumnStyles("VENDOR_PACK").Width = 0
        Me.dtgInProgressOrder.TableStyles("results").GridColumnStyles("UOMID").Width = 0
        Me.dtgInProgressOrder.TableStyles("results").GridColumnStyles("ITEM_KEY").Width = 0

        Cursor.Current = Cursors.Default

    End Sub
End Class
