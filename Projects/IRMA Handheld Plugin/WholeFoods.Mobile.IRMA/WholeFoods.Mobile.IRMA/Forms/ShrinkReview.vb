Imports System.Windows.Forms
Imports System
Imports System.Linq
Imports System.Data
Imports System.ServiceModel

Public Class ShrinkReview

    Private mySession As Session
    Private serviceFault As ParsedCFFaultException = Nothing

    Public Sub New(ByVal session As Session)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.mySession = session

    End Sub

    Private Sub ReviewShrink_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim fileWriter As ShrinkFileWriter = New ShrinkFileWriter(Me.mySession)

        updateBatch(fileWriter)
        Me.IrmaShrinkLabel.Text = "IRMA Shrink: " + mySession.ShrinkType
        Me.StoreTeamLabel.Text = mySession.StoreName + " / " + mySession.Subteam
    End Sub

    Private Sub updateBatch(ByVal fileWriter As ShrinkFileWriter)
        Cursor.Current = Cursors.WaitCursor

        Dim datalist As List(Of XNode) = New List(Of XNode)
        fileWriter.GetShrinkList(datalist, mySession.SessionName)

        Dim _result As New DataSet()
        _result.Tables.Add("results")
        _result.Tables("results").Columns.Add("UPC")
        _result.Tables("results").Columns.Add("Desc")
        _result.Tables("results").Columns.Add("Qty")
        _result.Tables("results").Columns.Add("SubType")

        _result.Tables("results").Columns.Add("UOM")
        _result.Tables("results").Columns.Add("CBW")
        _result.Tables("results").Columns.Add("ShrinkSubTypeId")
        _result.Tables("results").Columns.Add("ShrinkAdjId")
        _result.Tables("results").Columns.Add("ShrinkTypeId")

        Dim tmp As String

        For Each item As XElement In datalist
            Dim newRow As DataRow = _result.Tables("results").NewRow()
            tmp = item.Attribute("UPC")
            newRow("UPC") = tmp
            tmp = item.Attribute("DESC")
            newRow("Desc") = tmp
            tmp = item.Attribute("QTY")
            newRow("Qty") = tmp
            tmp = item.Attribute("UOM")
            newRow("UOM") = tmp
            tmp = item.Attribute("COSTED_BY_WEIGHT")
            newRow("CBW") = tmp
            tmp = item.Attribute("SHRINK_SUB_TYPE")
            newRow("SubType") = tmp
            tmp = item.Attribute("SHRINK_SUB_TYPE_ID")
            newRow("ShrinkSubTypeId") = tmp
            tmp = item.Attribute("SHRINK_ADJ_ID")
            newRow("ShrinkAdjId") = tmp
            tmp = item.Attribute("SHRINK_TYPE_ID")
            newRow("ShrinkTypeId") = tmp

            _result.Tables("results").Rows.Add(newRow)
        Next

        Dim dv As DataView = New DataView(_result.Tables("results"))
        dv.Sort = "UPC"

        Me.DataGrid1.DataSource = dv
        Me.DataGrid1.ColumnHeadersVisible = True
        Me.DataGrid1.RowHeadersVisible = True

        Dim ts As New DataGridTableStyle()
        ts.MappingName = "results"
        DataGrid1.TableStyles.Clear()
        DataGrid1.TableStyles.Add(ts)

        Me.DataGrid1.TableStyles("results").GridColumnStyles("UPC").Width = 140
        Me.DataGrid1.TableStyles("results").GridColumnStyles("DESC").Width = 214
        Me.DataGrid1.TableStyles("results").GridColumnStyles("QTY").Width = 57
        Me.DataGrid1.TableStyles("results").GridColumnStyles("SubType").Width = 220

        'Hide the columns
        Me.DataGrid1.TableStyles("results").GridColumnStyles("UOM").Width = 0
        Me.DataGrid1.TableStyles("results").GridColumnStyles("CBW").Width = 0
        Me.DataGrid1.TableStyles("results").GridColumnStyles("ShrinkSubTypeId").Width = 0
        Me.DataGrid1.TableStyles("results").GridColumnStyles("ShrinkAdjId").Width = 0
        Me.DataGrid1.TableStyles("results").GridColumnStyles("ShrinkTypeId").Width = 0

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub DeleteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteButton.Click
        Cursor.Current = Cursors.WaitCursor

        Dim fileWriter As ShrinkFileWriter = New ShrinkFileWriter(Me.mySession)
        Dim cCell As DataGridCell = Me.DataGrid1.CurrentCell
        Dim upcSelected As Integer = cCell.RowNumber

        If (Me.DataGrid1.VisibleRowCount = 0) Then
            MsgBox("There are no shrink items to remove.", MsgBoxStyle.OkOnly, Me.Text)
            Cursor.Current = Cursors.Default
            Exit Sub
        Else
            Dim upc As String = Me.DataGrid1.Item(upcSelected, 0).ToString()
            Dim style = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or _
                MsgBoxStyle.Question
            Dim response = MsgBox("Do you want to remove the selected UPC?", style, "Alert")

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
        End If

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub BackMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackMenuItem.Click
        Me.Close()
    End Sub

    Private Sub UploadMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UploadMenuItem.Click
        Cursor.Current = Cursors.WaitCursor
        Cursor.Show()

        Dim cbw As Boolean

        If Me.mySession.SessionName Is Nothing Then
            MessageBox.Show("Shrink items are not available for upload.", "Shrink Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            Cursor.Current = Cursors.Default
            Exit Sub
        End If

        Dim errorLoadFound As Boolean = False
        Dim fileWriter As ShrinkFileWriter = New ShrinkFileWriter(Me.mySession)
        Dim sndSession As Session = fileWriter.GetFileSession(fileWriter.MakeFilePath(Me.mySession.SessionName), Me.mySession)
        Dim datalist As List(Of XNode) = New List(Of XNode)

        fileWriter.GetShrinkList(datalist, Me.mySession.SessionName)

        Dim shrink As IRMA.Shrink
        Dim uploaded As Boolean = True
        Dim upc As String
        Dim sUtility As SessionUtility = New SessionUtility

        For Each item As XElement In datalist
            shrink = New IRMA.Shrink
            shrink.UserName = sndSession.UserName
            shrink.CreatedByUserID = sndSession.UserID
            shrink.CreatedByUserIDSpecified = True
            shrink.InventoryAdjustmentCodeAbbreviation = item.Attribute("SHRINK_TYPE_ID")
            shrink.StoreNo = sndSession.StoreNo
            shrink.StoreNoSpecified = True
            shrink.SubteamNo = sndSession.SubteamKey
            shrink.SubteamNoSpecified = True
            shrink.ShrinkSubTypeId = item.Attribute("SHRINK_SUB_TYPE_ID")
            shrink.ShrinkSubTypeIdSpecified = True

            cbw = item.Attribute("COSTED_BY_WEIGHT")

            If Not cbw Then
                shrink.Quantity = item.Attribute("QTY")
                shrink.Weight = 0
                shrink.QuantitySpecified = True
            Else
                shrink.Quantity = 0
                shrink.Weight = item.Attribute("QTY")
                shrink.WeightSpecified = True
            End If

            shrink.ItemKey = item.Attribute("ITEM_KEY")
            shrink.ItemKeySpecified = True
            shrink.AdjustmentID = item.Attribute("SHRINK_ADJ_ID")
            shrink.AdjustmentIDSpecified = True
            shrink.AdjustmentReason = item.Attribute("SHRINK_TYPE")

            Try
                uploaded = Me.mySession.WebProxyClient.AddShrinkAdjustment(shrink)
                upc = sUtility.GetValidString(item.Attribute("UPC"))

                If (uploaded = False) Then
                    errorLoadFound = True
                Else
                    fileWriter.DeleteItem(Me.mySession.SessionName, upc)
                    updateBatch(fileWriter)
                End If

                ' Explicitly handle service faults, timeouts, and connection failures.  If this initialization block fails, the user will
                ' fall back to the last form she was on.
            Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
                serviceFault = New ParsedCFFaultException(ex.FaultMessage)
                Dim err As New ErrorHandler(serviceFault)
                err.ShowErrorNotification()
                errorLoadFound = True
                Exit For

            Catch ex As TimeoutException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "UploadMenuItem_Click")
                errorLoadFound = True
                Exit For

            Catch ex As CommunicationException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "UploadMenuItem_Click")
                errorLoadFound = True
                Exit For

            Catch ex As Exception
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(ex.Message, "UploadMenuItem_Click")
                errorLoadFound = True
                Exit For

            End Try
        Next

        Cursor.Current = Cursors.Default

        'remove file session and exit if all items loaded
        If (Not errorLoadFound) Then
            fileWriter.DeleteFile(fileWriter.MakeFilePath(Me.mySession.SessionName))
            If (Not Me.mySession.SessionName = Nothing) Then
                Me.mySession.SessionName = Nothing
            End If

            Me.DialogResult = Windows.Forms.DialogResult.OK
        Else
            Messages.ShrinkItemUploadError()
        End If

    End Sub

    Private Sub UpdateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateButton.Click
        Dim fileWriter As ShrinkFileWriter = New ShrinkFileWriter(Me.mySession)
        Dim cCell As DataGridCell = Me.DataGrid1.CurrentCell
        Dim upcSelected As Integer = cCell.RowNumber

        If (Me.DataGrid1.VisibleRowCount = 0) Then
            MsgBox("There are no shrink items to update.", MsgBoxStyle.OkOnly, Me.Text)
            Exit Sub
        Else
            Dim upc As String = Me.DataGrid1.Item(upcSelected, 0).ToString()
            Dim uom As String = Me.DataGrid1.Item(upcSelected, 4).ToString()
            Dim qty As String = Me.DataGrid1.Item(upcSelected, 2).ToString()
            Dim desc As String = Me.DataGrid1.Item(upcSelected, 1).ToString()
            Dim shrinkType As String = Me.DataGrid1.Item(upcSelected, 3).ToString()
            Dim cbw As Boolean = Me.DataGrid1.Item(upcSelected, 5)
            Dim shrinkSubTypeId As String = Me.DataGrid1.Item(upcSelected, 6)
            Dim style = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Critical
            Dim msgText As String

            If cbw Then
                msgText = "Do you want to update the Weight/Subtype for the selected UPC?"
            Else
                msgText = "Do you want to update the Quantity/Subtype for the selected UPC?"
            End If

            Dim response = MsgBox(msgText, MsgBoxStyle.YesNo + MsgBoxStyle.Question, Me.Text)

            If (response = MsgBoxResult.Yes) Then
                Dim frmUpdate As UpdateShrink = New UpdateShrink("Update Shrink", mySession)
                Dim oldShrinkSubTypeId As Integer = shrinkSubTypeId
                frmUpdate.CostedByWeight = cbw
                frmUpdate.UOM = uom
                frmUpdate.UPC = upc
                frmUpdate.Desc = desc

                frmUpdate.LastQtyRecorded = qty
                frmUpdate.ShrinkSubType = shrinkType
                frmUpdate.ShrinkSubTypeId = shrinkSubTypeId
                frmUpdate.ShowDialog()

                If frmUpdate.ShrinkQuantity <> String.Empty Then

                    Dim shrinkAdjId As Integer
                    Dim shrinkTypeId As String
                    Dim newShrinkType As String

                    For Each shrinkSubType In mySession.ShrinkSubTypes
                        If (shrinkSubType.ShrinkSubTypeID = frmUpdate.GetShrinkSubTypeId) Then
                            shrinkTypeId = shrinkSubType.Abbreviation
                            newShrinkType = shrinkSubType.ShrinkType

                            Dim expr As String = "DisplayMember='" & newShrinkType & "'"
                            Dim dr As DataRow() = mySession.ShrinkAdjustmentIds.Select(expr)
                            shrinkAdjId = dr(0).Item("ValueMember")
                            fileWriter.ReplaceShrinkSubType(upc, frmUpdate.GetShrinkSubType, shrinkAdjId, shrinkTypeId, newShrinkType, oldShrinkSubTypeId.ToString)

                        End If
                    Next

                    fileWriter.ReplaceQuantityBasedOnType(upc, frmUpdate.ShrinkQuantity, oldShrinkSubTypeId.ToString)
                    fileWriter.ReplaceShrinkSubTypeId(upc, frmUpdate.GetShrinkSubTypeId, oldShrinkSubTypeId.ToString)



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
        End If
    End Sub

End Class