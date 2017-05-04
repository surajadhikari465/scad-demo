Imports System.Text
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.IRMA.Administration.Common.DataAccess
Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_EditRegionalSettings
    ''' <summary>
    ''' Flag to keep track of user changes that have not been saved.
    ''' </summary>
    ''' <remarks></remarks>
    Private hasChanges As Boolean
    Private _regionalConfig As RegionBO

#Region "Events raised by this form"
    ''' <summary>
    ''' This event is raised when a child form makes a change that requires the
    ''' data on the calling form to be updated.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event UpdateCallingForm()
#End Region

#Region "Form Load Events"

    Private Sub Form_EditRegionalSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Populate the selections for the corporate scale writers
        Me.ComboBox_CorpWriter.DataSource = POSWriterDAO.GetFileWriters(POSWriterBO.WRITER_TYPE_SCALE, POSWriterBO.SCALE_WRITER_TYPE_CORPORATE).Tables(0)
        Me.ComboBox_CorpWriter.DisplayMember = "POSFileWriterCode"
        Me.ComboBox_CorpWriter.ValueMember = "POSFileWriterKey"
        Me.ComboBox_CorpWriter.SelectedIndex = -1

        ' Populate the selections for the zone scale writers
        Me.ComboBox_ZoneWriter.DataSource = POSWriterDAO.GetFileWriters(POSWriterBO.WRITER_TYPE_SCALE, POSWriterBO.SCALE_WRITER_TYPE_ZONE, POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE).Tables(0)
        Me.ComboBox_ZoneWriter.DisplayMember = "POSFileWriterCode"
        Me.ComboBox_ZoneWriter.ValueMember = "POSFileWriterKey"
        Me.ComboBox_ZoneWriter.SelectedIndex = -1

        'is regional scale file?
        Dim isRegionalScaleFile As Boolean = False
        If InstanceDataDAO.IsFlagActive("UseRegionalScaleFile") Then
            isRegionalScaleFile = True
        End If

        Me.CheckBox_RegionalScale.Checked = isRegionalScaleFile
        If isRegionalScaleFile Then
            If _regionalConfig.CorpScaleWriter.ScaleFileWriterKey IsNot Nothing Then
                Me.ComboBox_CorpWriter.SelectedValue = _regionalConfig.CorpScaleWriter.ScaleFileWriterKey
            End If
            If _regionalConfig.ZoneScaleWriter.ScaleFileWriterKey IsNot Nothing Then
                Me.ComboBox_ZoneWriter.SelectedValue = _regionalConfig.ZoneScaleWriter.ScaleFileWriterKey
            End If
        Else
            ' disable the regional scale options
            Me.ComboBox_CorpWriter.Enabled = False
            Me.ComboBox_ZoneWriter.Enabled = False
        End If

        ' initialze the changes flag to false
        hasChanges = False

        Me.CenterToScreen()
    End Sub
#End Region

#Region "Form Button and Form Closing Events"
    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub

    Private Sub Button_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Save.Click
        'save changes
        If ApplyChanges() Then
            ' Raise event - allows the data on the parent form to be refreshed
            RaiseEvent UpdateCallingForm()

            'close form
            Me.Close()
        End If
    End Sub

    Private Sub Form_EditRegionalSettings_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If hasChanges Then
            'prompt the user to save changes
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                If ApplyChanges() Then
                    ' Raise event - allows the data on the parent form to be refreshed
                    RaiseEvent UpdateCallingForm()

                    'close form
                    Me.Close()
                Else
                    e.Cancel = True
                End If

            End If
        End If
    End Sub

    ''' <summary>
    ''' Save the data entry changes to the database.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function ApplyChanges() As Boolean
        Dim success As Boolean = False
        Dim statusList As ArrayList
        Dim statusEnum As IEnumerator
        Dim message As New StringBuilder
        Dim currentStatus As RegionBOStatus

        'is regional scale file?
        Dim isRegionalScaleFile As Boolean = False
        If CheckBox_RegionalScale.Checked Then
            isRegionalScaleFile = True
        End If

        Try
            ' Populate the business object with the form data and save the change to the database

            _regionalConfig.UseRegionalScaleFlag = Me.CheckBox_RegionalScale.Checked

            If Me.ComboBox_CorpWriter.SelectedValue IsNot Nothing Then
                _regionalConfig.CorpScaleWriter.ScaleFileWriterKey = Me.ComboBox_CorpWriter.SelectedValue.ToString
            Else
                _regionalConfig.CorpScaleWriter.ScaleFileWriterKey = Nothing
            End If
            If Me.ComboBox_ZoneWriter.SelectedValue IsNot Nothing Then
                _regionalConfig.ZoneScaleWriter.ScaleFileWriterKey = Me.ComboBox_ZoneWriter.SelectedValue.ToString
            Else
                _regionalConfig.ZoneScaleWriter.ScaleFileWriterKey = Nothing
            End If

            ''If the checkbox has been unchecked then we do not need to check for these error types
            If CheckBox_RegionalScale.Checked Then
                'validate required data
                statusList = _regionalConfig.ValidateRegionData()
                statusEnum = statusList.GetEnumerator

                'loop through possible validation erorrs and build message string containing all errors
                statusEnum = statusList.GetEnumerator
                While statusEnum.MoveNext
                    currentStatus = CType(statusEnum.Current, RegionBOStatus)

                    Select Case currentStatus
                        Case RegionBOStatus.Error_Required_CorpScaleWriter
                            message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_CorpWriter.Text))
                            message.Append(Environment.NewLine)
                        Case RegionBOStatus.Error_Required_ZoneScaleWriter
                            message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_ZoneWriter.Text))
                            message.Append(Environment.NewLine)
                    End Select
                End While
            End If

            If message.Length <= 0 Then
                'data is valid - perform update
                RegionDAO.UpdateRegionalScaleSettings(_regionalConfig)
                success = True

                'data has been saved - reset the edit flag
                hasChanges = False
            Else
                'display error msg
                MessageBox.Show(message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Catch ex As DataFactoryException
            Logger.LogError("Exception: ", Me.GetType(), ex)
            'display a message to the user
            MessageBox.Show(Form_IRMABase.ERROR_DB, "IRMA Application Error", MessageBoxButtons.OK)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_EditStoreFTPInfo form: ApplyChanges sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
        End Try
        Return success
    End Function
#End Region

#Region "Data Change Events"
    Private Sub CheckBox_RegionalScale_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_RegionalScale.CheckedChanged
        hasChanges = True
        If Me.CheckBox_RegionalScale.Checked Then
            ' enable the regional scale options
            Me.ComboBox_CorpWriter.Enabled = True
            Me.ComboBox_ZoneWriter.Enabled = True
        Else
            ' disable the regional scale options
            Me.ComboBox_CorpWriter.Enabled = False
            Me.ComboBox_CorpWriter.SelectedIndex = -1
            Me.ComboBox_ZoneWriter.Enabled = False
            Me.ComboBox_ZoneWriter.SelectedIndex = -1
        End If

    End Sub

    Private Sub ComboBox_CorpWriter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_CorpWriter.SelectedIndexChanged
        hasChanges = True
    End Sub

    Private Sub ComboBox_ZoneWriter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_ZoneWriter.SelectedIndexChanged
        hasChanges = True
    End Sub
#End Region

#Region "Property Access Methods"
    Public Property RegionalConfig() As RegionBO
        Get
            Return _regionalConfig
        End Get
        Set(ByVal value As RegionBO)
            _regionalConfig = value
        End Set
    End Property

#End Region

End Class