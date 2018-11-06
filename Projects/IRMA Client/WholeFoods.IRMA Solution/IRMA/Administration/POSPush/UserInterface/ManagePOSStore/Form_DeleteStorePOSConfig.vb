Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_DeleteStorePOSConfig
#Region "Class Level Vars and Property Definitions"
    ''' <summary>
    ''' Value of the current store configuration for edits
    ''' </summary>
    ''' <remarks></remarks>
    Private _storePOSConfig As StorePOSConfigBO
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
    ''' Load the form, pre-filling with the existing data for the store being deleted.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_DeleteStorePOSConfig_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Logger.LogDebug("Form_DeleteStorePOSConfig_Load entry", Me.GetType())
        'set up button and title bar labels
        LoadText()
        ' Pre-fill the existing values
        Label_StoreVal.Text = String.Format(ResourcesAdministration.GetString("label_storeDesc"), _storePOSConfig.StoreConfig.StoreNo, _storePOSConfig.StoreConfig.StoreName)
        Label_ConfigTypeVal.Text = _storePOSConfig.ConfigType
        Label_FileWriterVal.Text = _storePOSConfig.POSFileWriterCode
        Logger.LogDebug("Form_DeleteStorePOSConfig_Load exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' set localized string text for buttons and title bar
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadText()
        Panel_Instructions.Text = ResourcesAdministration.GetString("msg_confirmDeleteStorePOSConfig")
    End Sub

#End Region

#Region "Delete Button"
    ''' <summary>
    ''' Delete the data from the database and notify the parent form.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ProcessDelete()
        Logger.LogDebug("ProcessDelete entry", Me.GetType())
        Try
            ' Apply the change to the database
            If (_storePOSConfig.FileWriterType = POSWriterBO.WRITER_TYPE_POS) Then
                StoreWriterConfigDAO.DeleteStorePOSConfigRecord(_storePOSConfig)
            Else
                StoreWriterConfigDAO.DeleteStoreShelfTagConfigRecord(_storePOSConfig)
            End If
        Catch ex As DataFactoryException
            Logger.LogError("Exception: ", Me.GetType(), ex)
            'display a message to the user
            DisplayErrorMessage(ERROR_DB)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_DeleteStorePOSConfig form: ProcessDelete sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
        End Try

        ' Raise the delete event - allows the data on the parent form to be refreshed
        RaiseEvent UpdateCallingForm()
        Logger.LogDebug("ProcessDelete exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' The user selected the Delete button - apply the changes.
    ''' Close the form and return focus to the View Stores form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Delete.Click
        Logger.LogDebug("Button_Delete_Click entry", Me.GetType())
        ' Apply the change to the database
        ProcessDelete()
        ' Set the flag so they are not prompted again for delete confirmation
        _skipDeleteConfirm = True
        ' Close the child window
        Me.Close()
        Logger.LogDebug("Button_Delete_Click exit", Me.GetType())
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
    Public Property StorePOSConfig() As StorePOSConfigBO
        Get
            Return _storePOSConfig
        End Get
        Set(ByVal value As StorePOSConfigBO)
            _storePOSConfig = value
        End Set
    End Property
#End Region

End Class
