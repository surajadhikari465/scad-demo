Imports System.Text
Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.Utility

Public Class Form_EditBatchIdValue
    ''' <summary>
    ''' Value of the current writer configuration for edits
    ''' </summary>
    ''' <remarks></remarks>
    Private _inputWriterConfig As POSWriterBO
    ''' <summary>
    ''' Value of the current default batch id for edits
    ''' </summary>
    ''' <remarks></remarks>
    Private _batchIdDefault As BatchIdDefaultBO


#Region "Properties"
    Public Property InputWriterConfig() As POSWriterBO
        Get
            Return _inputWriterConfig
        End Get
        Set(ByVal value As POSWriterBO)
            _inputWriterConfig = value
        End Set
    End Property

    Public Property BatchIdDefaultBO() As BatchIdDefaultBO
        Get
            Return _batchIdDefault
        End Get
        Set(ByVal value As BatchIdDefaultBO)
            _batchIdDefault = value
        End Set
    End Property
#End Region

#Region "Events raised by this form"
    ''' <summary>
    ''' This event is raised when a child form makes a change that requires the
    ''' data on the calling form to be updated.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event UpdateCallingForm()
#End Region

#Region "Form events"
    Private Sub Form_EditBatchIdValue_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Logger.LogDebug("Form_EditBatchIdValue_Load entry", Me.GetType())
        ' Set the text for the read only labels
        Me.Label_FileWriterCode.Text = _inputWriterConfig.POSFileWriterCode
        Me.Label_WriterType.Text = _inputWriterConfig.WriterType

        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ' Hide the scale writer type from other writers
            Me.Label_ScaleWriterTypeHeader.Visible = False
            Me.Label_ScaleWriterType.Visible = False
        Else
            ' Prefill the scale writer values
            Me.Label_ScaleWriterType.Text = _inputWriterConfig.ScaleWriterTypeDesc
        End If

        ' Prefill the batch id values
        Me.Label_ChangeType.Text = _batchIdDefault.ChangeTypeDesc
        Me.TextBox_DefaultBatchId.Text = _batchIdDefault.BatchIdDefault

        ' Disable the OK button until the user makes changes
        Button_OK.Enabled = False
        Logger.LogDebug("Form_EditBatchIdValue_Load exit", Me.GetType())
    End Sub

    Private Sub TextBox_DefaultBatchId_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_DefaultBatchId.TextChanged
        ' Enable the OK button
        Button_OK.Enabled = True
    End Sub

    Private Sub Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_OK.Click
        Logger.LogDebug("Button_OK_Click entry", Me.GetType())
        If SaveChanges() Then
            ' Do not prompt the user to save their changes again
            Button_OK.Enabled = False
            Me.Close()
        End If
        Logger.LogDebug("Button_OK_Click exit", Me.GetType())
    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        Logger.LogDebug("Button_Cancel_Click entry", Me.GetType())
        ' Do not prompt the user to save changes on closing
        Button_OK.Enabled = False
        Me.Close()
        Logger.LogDebug("Button_Cancel_Click exit", Me.GetType())
    End Sub

    Private Sub Form_EditBatchIdValue_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Logger.LogDebug("Form_EditBatchIdValue_FormClosing entry", Me.GetType())
        If Button_OK.Enabled Then
            ' Prompt the user to save their changes
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                If Not SaveChanges() Then
                    ' the save failed - do not close the form
                    e.Cancel = True
                End If
            End If
        End If
        Logger.LogDebug("Form_EditBatchIdValue_FormClosing exit", Me.GetType())
    End Sub
#End Region

    ''' <summary>
    ''' Save the updates to the form to the database.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SaveChanges() As Boolean
        Logger.LogDebug("SaveChanges entry", Me.GetType())
        Dim success As Boolean

        ' Populate the business object with the current values from the UI
        _batchIdDefault.BatchIdDefault = TextBox_DefaultBatchId.Text

        ' Validate the input data
        Dim statusList As ArrayList = _batchIdDefault.ValidateData
        Dim statusEnum As IEnumerator = statusList.GetEnumerator
        Dim message As New StringBuilder
        Dim currentStatus As BatchIdDefaultBOStatus

        ' loop through possible validation erorrs and build message string containing all errors
        While statusEnum.MoveNext
            currentStatus = CType(statusEnum.Current, BatchIdDefaultBOStatus)

            Select Case currentStatus
                Case BatchIdDefaultBOStatus.Error_Integer_BatchIdDefault
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Label_DefaultBatchIdHeader.Text))
                    message.Append(Environment.NewLine)
            End Select
        End While

        If message.Length <= 0 Then
            ' Save the updates to the data, based on the batch id change type
            Dim writerDAO As New POSWriterDAO
            Select Case _batchIdDefault.BatchIdType
                Case BatchIdDefaultType.ItemChange
                    writerDAO.SaveBatchIdDefaultsByItemChangeType(_batchIdDefault)
                Case BatchIdDefaultType.PriceChange
                    writerDAO.SaveBatchIdDefaultsByPriceChangeType(_batchIdDefault)
                Case BatchIdDefaultType.WriterChange
                    writerDAO.SaveBatchIdDefaultsByWriterChangeType(_batchIdDefault)
            End Select
            success = True

            ' Raise the save event - allows the data on the parent form to be refreshed
            RaiseEvent UpdateCallingForm()
        Else
            success = False
            ' Display error msg
            MessageBox.Show(message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
        Logger.LogDebug("SaveChanges exit: success=" & success, Me.GetType())
        Return success
    End Function



End Class