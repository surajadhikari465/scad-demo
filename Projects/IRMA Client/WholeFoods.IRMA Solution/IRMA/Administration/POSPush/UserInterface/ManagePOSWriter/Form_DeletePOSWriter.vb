Imports System.Text
Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_DeletePOSWriter
#Region "Class Level Vars and Property Definitions"
    ''' <summary>
    ''' Value of the current writer configuration for edits
    ''' </summary>
    ''' <remarks></remarks>
    Private _writerConfig As POSWriterBO
#End Region

#Region "Events raised by this form"
    ''' <summary>
    ''' This event is raised when a child form makes a change that requires the
    ''' data on the calling form to be updated.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event UpdateCallingForm()
#End Region

#Region "Events handled by this form"
#Region "Load Form"
    ''' <summary>
    ''' Load the form, pre-filling with the existing data for the writer being deleted.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_DeletePOSWriter_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'set up button and title bar labels
        LoadText()
        ' Pre-fill the existing values
        Me.Label_WriterClassVal.Text = _writerConfig.POSFileWriterClass
        Me.Label_WriterCodeVal.Text = _writerConfig.POSFileWriterCode
    End Sub

    ''' <summary>
    ''' set localized string text for buttons and title bar
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadText()
        Panel_Instructions.Text = ResourcesAdministration.GetString("msg_confirmDisablePOSWriter")
        
        ' Add a message listing the stores that will no longer be configured for POS Push
        Try
            Me.Label_StoreWarnings.Text = String.Format(ResourcesAdministration.GetString("msg_confirmDisablePOSWriterStoreList"), FormatStoresAssignedToWriter(_writerConfig.POSFileWriterKey))
        Catch ex As DataFactoryException
            Logger.LogError("Exception: ", Me.GetType(), ex)
            'display a message to the user
            DisplayErrorMessage(ERROR_DB)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_DisablePOSWriterFileConfig form: Form_DeletePOSWriter_Load sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
        End Try
    End Sub

    ''' <summary>
    ''' Reads the list of Stores assigned to the POSWriter and formats them for display to the user.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function FormatStoresAssignedToWriter(ByVal posFileWriterKey As Integer) As String
        Logger.LogDebug("FormatStoresAssignedToWriter entry", Me.GetType())
        Dim returnStr As New StringBuilder
        ' Read the stores assigned to the writer
        Dim _dataSet As DataSet = POSWriterDAO.GetStoresAssignedToWriter(_writerConfig)

        ' Process each store in the result set
        Dim rowEnum As IEnumerator = _dataSet.Tables(0).Rows.GetEnumerator
        Dim currentRow As DataRow
        Dim currentStoreName As String
        While (rowEnum.MoveNext)
            currentRow = CType(rowEnum.Current, DataRow)
            currentStoreName = CType(currentRow.Item("Store_Name"), String)
            If (currentStoreName IsNot Nothing) Then
                If (returnStr.Length > 0) Then
                    returnStr.Append(", ")
                Else
                    returnStr.Append(Environment.NewLine)
                End If
                returnStr.Append(currentStoreName.ToString)
            End If
        End While

        ' Update the message if no stores were found
        If (returnStr.Length = 0) Then
            returnStr.Append(Environment.NewLine)
            returnStr.Append(ResourcesAdministration.GetString("msg_confirmDeletePOSWriterStoreList_noStores"))
        End If
        Return returnStr.ToString
    End Function
#End Region

#Region "Delete Button"
    ''' <summary>
    ''' Delete the data from the database
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ProcessDelete()
        Logger.LogDebug("ProcessDelete entry", Me.GetType())
        Try
            ' Apply the change to the database
            POSWriterDAO.DeletePOSWriter(_writerConfig)
        Catch ex As DataFactoryException
            Logger.LogError("Exception: ", Me.GetType(), ex)
            'display a message to the user
            DisplayErrorMessage(ERROR_DB)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_DisablePOSWriter form: ProcessDelete sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
        End Try

        ' Raise the delete event - allows the data on the parent form to be refreshed
        RaiseEvent UpdateCallingForm()
        Logger.LogDebug("ProcessDelete exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' The user selected the Delete button - apply the changes.
    ''' Close the form and return focus to the View Writers form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Delete.Click
        ' Apply the change to the database
        ProcessDelete()
        ' Set the flag so they are not prompted again for delete confirmation
        _skipDeleteConfirm = True
        ' Close the child window
        Me.Close()
    End Sub
#End Region

#Region "Close Form"
    ''' <summary>
    ''' Confirm user wants to save any changed data when they are clicking 'X' button in top-right of window
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_DeleteStorePOSConfig_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not _skipDeleteConfirm Then
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmDeleteData"), ResourcesCommon.GetString("msg_titleConfirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = Windows.Forms.DialogResult.Yes Then
                ' Apply the change to the database
                ProcessDelete()
            End If
        End If
    End Sub
#End Region

#End Region

#Region "Property Definitions"
    Public Property WriterConfig() As POSWriterBO
        Get
            Return _writerConfig
        End Get
        Set(ByVal value As POSWriterBO)
            _writerConfig = value
        End Set
    End Property
#End Region

End Class
