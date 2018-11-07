Imports System.Windows.Forms
Imports System
Imports System.Linq
Imports System.Data
Imports System.ServiceModel

Public Class ReceiveDocumentReview
    Private mySession As Session
    Private serviceFault As ParsedCFFaultException = Nothing
    Private serviceCallSuccess As Boolean
    Private itemCount As Integer = 0

    Public Sub New(ByVal session As Session)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.mySession = session

    End Sub

    Private Sub ReviewShrink_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim fileWriter As ReceiveDocumentFileWriter = New ReceiveDocumentFileWriter(Me.mySession)
        updateBatch(fileWriter)

        Me.StoreTeamLabel.Text = mySession.StoreName.Trim() + " / " + mySession.Subteam
        Me.Count.Text = itemCount
    End Sub

    Private Sub updateBatch(ByVal fileWriter As ReceiveDocumentFileWriter)
        Cursor.Current = Cursors.WaitCursor

        Dim datalist As List(Of XNode) = New List(Of XNode)
        fileWriter.GetShrinkList(datalist, mySession.SessionName)

        Dim _result As New DataSet()
        _result.Tables.Add("results")
        _result.Tables("results").Columns.Add("UPC")
        _result.Tables("results").Columns.Add("Desc")
        _result.Tables("results").Columns.Add("Qty")
        _result.Tables("results").Columns.Add("UOM")
        _result.Tables("results").Columns.Add("CBW")

        Dim tmp As String

        itemCount = 0

        For Each item As XElement In datalist
            Dim newRow As DataRow = _result.Tables("results").NewRow()
            tmp = item.Attribute("UPC")
            newRow("UPC") = tmp
            tmp = item.Attribute("DESC")
            newRow("DESC") = tmp
            tmp = item.Attribute("QTY")
            newRow("QTY") = tmp
            tmp = item.Attribute("UOM")
            newRow("UOM") = tmp
            tmp = item.Attribute("COSTED_BY_WEIGHT")
            newRow("CBW") = tmp

            itemCount = itemCount + CInt(item.Attribute("QTY"))
            _result.Tables("results").Rows.Add(newRow)
        Next

        Me.Count.Text = itemCount

        Dim dv As DataView = New DataView(_result.Tables("results"))
        dv.Sort = "UPC"

        Me.DataGrid1.DataSource = dv
        Me.DataGrid1.ColumnHeadersVisible = True
        Me.DataGrid1.RowHeadersVisible = True

        ' Create new Table Style.
        Dim ts As New DataGridTableStyle()
        ts.MappingName = "results"
        DataGrid1.TableStyles.Clear()
        DataGrid1.TableStyles.Add(ts)

        Me.DataGrid1.TableStyles("results").GridColumnStyles("UPC").Width = 141
        Me.DataGrid1.TableStyles("results").GridColumnStyles("DESC").Width = 217
        Me.DataGrid1.TableStyles("results").GridColumnStyles("QTY").Width = 55

        'Hide the columns
        Me.DataGrid1.TableStyles("results").GridColumnStyles("UOM").Width = 0
        Me.DataGrid1.TableStyles("results").GridColumnStyles("CBW").Width = 0

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub DeleteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteButton.Click

        If Me.DataGrid1.VisibleRowCount < 1 Then Exit Sub

        Cursor.Current = Cursors.WaitCursor

        Dim fileWriter As ReceiveDocumentFileWriter = New ReceiveDocumentFileWriter(Me.mySession)
        Dim cCell As DataGridCell = Me.DataGrid1.CurrentCell
        Dim upcSelected As Integer = cCell.RowNumber
        Dim upc As String = Me.DataGrid1.Item(upcSelected, 0).ToString()
        Dim style = MsgBoxStyle.YesNo Or MsgBoxStyle.Question Or MsgBoxStyle.DefaultButton2

        Dim response = MsgBox("Do you want to delete selected UPC?", style, "Confirm Deletion")

        If (response = MsgBoxResult.Yes) Then
            Cursor.Current = Cursors.WaitCursor
            fileWriter.DeleteItem(Me.mySession.SessionName, upc)
            'update datagrid
            updateBatch(fileWriter)
            Cursor.Current = Cursors.Default
        Else
            'unselect item
            Me.DataGrid1.UnSelect(upcSelected)
        End If

        Cursor.Current = Cursors.Default

    End Sub

    Private Sub BackMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackMenuItem.Click
        Me.Close()
    End Sub

    Private Sub UploadMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UploadMenuItem.Click

        If Me.DataGrid1.VisibleRowCount < 1 Then Exit Sub

        Cursor.Current = Cursors.WaitCursor

        Dim order As IRMA.Order
        Dim oi As WholeFoods.Mobile.IRMA.OrderItem
        Dim cbw As Boolean

        If Me.mySession.SessionName Is Nothing Then
            MsgBox("No Receiving Document items are available for upload.", MsgBoxStyle.Information, Me.Text)
            Cursor.Current = Cursors.Default
            Exit Sub
        End If

        Dim fileWriter As ReceiveDocumentFileWriter = New ReceiveDocumentFileWriter(Me.mySession)
        Dim sndSession As Session = fileWriter.GetFileSession(fileWriter.GetFilePath(), Me.mySession)
        Dim datalist As List(Of XNode) = New List(Of XNode)

        fileWriter.GetShrinkList(datalist, Me.mySession.SessionName)

        Dim uploaded As Boolean = True
        Dim sUtility As SessionUtility = New SessionUtility

        order = New IRMA.Order

        order.CreatedBy = sndSession.UserID
        order.ProductType_ID = 1
        order.Vendor_ID = sndSession.DSDVendorID
        order.ReceiveLocation_ID = sndSession.StoreVendorID
        order.PurchaseLocation_ID = sndSession.StoreVendorID
        order.Transfer_To_SubTeam = sndSession.SubteamKey
        order.InvoiceNumber = sndSession.DSDInvoice

        If mySession.ActionType = Enums.ActionType.ReceiveDocument Then
            order.Return_Order = False
        ElseIf mySession.ActionType = Enums.ActionType.ReceiveDocumentCredit Then
            order.Return_Order = True
        End If

        Dim oiList As List(Of OrderItem) = New List(Of OrderItem)

        For Each item As XElement In datalist
            oi = New WholeFoods.Mobile.IRMA.OrderItem
            cbw = item.Attribute("COSTED_BY_WEIGHT")
            oi.QuantityOrdered = item.Attribute("QTY")
            oi.Item_Key = item.Attribute("ITEM_KEY")
            oi.QuantityUnit = item.Attribute("QUANTITY_UNIT")
            oi.DiscountType = item.Attribute("DISCOUNT_TYPE")
            oi.QuantityDiscount = item.Attribute("DISCOUNT_AMOUNT")
            oi.ReasonCodeDetailID = item.Attribute("DISCOUNT_REASON_CODE")

            oiList.Add(oi)
        Next

        order.OrderItems = oiList.ToArray()

        Try
            ' Attempt a service call to create the order.  Check that the invoice number is not a duplicate.
            serviceCallSuccess = True

            If Not String.IsNullOrEmpty(order.InvoiceNumber) Then
                If Me.mySession.WebProxyClient.IsDuplicateReceivingDocumentInvoiceNumber(order.InvoiceNumber, order.Vendor_ID) Then
                    MessageBox.Show(String.Format("Invoice number {0} has already been used for this vendor.  Please go back to update the invoice number and try again.", order.InvoiceNumber), _
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
                    Exit Sub
                End If
            End If
            
            Dim success As Result = Me.mySession.WebProxyClient.CreateDSDOrder(order)

            If (success.Status = False) Then
                MessageBox.Show(success.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1)
            Else
                If Not success.DVO_PONumber = 0 Then
                    MsgBox(String.Format("IRMA PO# {0} and DVO PO# {1} have been created.", success.IRMA_PONumber, success.DVO_PONumber), MsgBoxStyle.Information, "PO Created")
                Else
                    MsgBox(String.Format("IRMA PO# {0} has been created.", success.IRMA_PONumber), MsgBoxStyle.Information, "PO Created")
                End If

                fileWriter.DeleteFile(fileWriter.MakeFilePath(Me.mySession.SessionName))

                If (Not Me.mySession.SessionName = Nothing) Then
                    Me.mySession.SessionName = Nothing
                End If
            End If

            ' Explicitly handle service faults, timeouts, and connection failures.  
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            serviceCallSuccess = False

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "UploadMenuItem_Click")
            serviceCallSuccess = False

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "UploadMenuItem_Click")
            serviceCallSuccess = False

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "UploadMenuItem_Click")
            serviceCallSuccess = False
        Finally
            Cursor.Current = Cursors.Default
        End Try

        If Not serviceCallSuccess Then
            ' Order creation enountered an error.  End the method.'
            Exit Sub
        End If

        mySession.IsLoadedSession = False

        Me.DialogResult = Windows.Forms.DialogResult.OK

    End Sub

    Private Sub UpdateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateButton.Click
        Dim fileWriter As ReceiveDocumentFileWriter = New ReceiveDocumentFileWriter(Me.mySession)
        Dim cCell As DataGridCell = Me.DataGrid1.CurrentCell
        Dim upcSelected As Integer = cCell.RowNumber
        Dim upc As String = Me.DataGrid1.Item(upcSelected, 0).ToString()
        Dim uom As String = Me.DataGrid1.Item(upcSelected, 3).ToString()
        Dim qty As String = Me.DataGrid1.Item(upcSelected, 2).ToString()
        Dim desc As String = Me.DataGrid1.Item(upcSelected, 1).ToString()
        Dim cbw As Boolean = Me.DataGrid1.Item(upcSelected, 4)

        Dim style = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question
        Dim msgText As String

        If cbw Then
            msgText = "Do you want to update the weight for the selected UPC?"
        Else
            msgText = "Do you want to update the quantity for the selected UPC?"
        End If

        Dim response = MsgBox(msgText, style, Me.Text)
        If (response = MsgBoxResult.Yes) Then
            Dim frmUpdate As UpdateShrink = New UpdateShrink("Receiving Document")
            frmUpdate.CostedByWeight = cbw
            frmUpdate.UOM = uom
            frmUpdate.UPC = upc
            frmUpdate.Desc = desc
            frmUpdate.LastQtyRecorded = qty

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
            Me.DataGrid1.UnSelect(upcSelected)
        End If

        Cursor.Current = Cursors.Default
    End Sub

End Class