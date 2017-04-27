Imports WholeFoods.Utility
Imports WholeFoods.IRMA.Administration.ConfigurationData.DataAccess

Public Class Form_ConfigurationData_Build

    ' Instantiate the data access class
    Private daoConfig As New ConfigurationDataDAO
    Private bIsLoading As Boolean

    Private ReadOnly Property AppID() As Guid
        Get
            Return Me._comboFilterApp.SelectedValue
        End Get
    End Property

    Private ReadOnly Property EnvID() As Guid
        Get
            Return Me._comboFilterEnv.SelectedValue
        End Get
    End Property

    ''' <summary>
    ''' This event is raised whenver form data is changed to indicate that it needs to be saved.
    ''' </summary>
    ''' <remarks></remarks>
    Private Event FormDataChanged()

    ''' <summary>
    ''' Sets the hasChanges form indicator to True and enables the Save button.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FormHasChanges() Handles Me.FormDataChanged

        If Me._radioBuildAll.Checked Then

            Me._groupBuildChoice.Enabled = False
            Me._buttonBuild.Enabled = True
            Me._buttonPreview.Enabled = False

        Else

            If Me._comboFilterEnv.SelectedIndex <> -1 And Me._comboFilterApp.SelectedIndex <> -1 Then

                Me._buttonPreview.Enabled = True
                Me._buttonBuild.Enabled = True

            Else

                Me._buttonPreview.Enabled = False
                Me._buttonBuild.Enabled = False

            End If

        End If

    End Sub

    Private Sub Form_ConfigurationData_Build_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Me._formBuildWorker.IsBusy Then
            MessageBox.Show("A build is in progress and cannot be cancelled. Wait for the build to complete before closing this panel.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            e.Cancel = True
        End If
    End Sub

    Private Sub _radioBuildAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _radioBuildAll.CheckedChanged

        If Me._radioBuildAll.Checked Then

            Me._comboFilterEnv.SelectedIndex = -1
            Me._comboFilterApp.SelectedIndex = -1

            Me._groupBuildChoice.Enabled = False
            Me._buttonBuild.Enabled = True

        Else

            Me._groupBuildChoice.Enabled = True
            Me._buttonBuild.Enabled = False
            Me._buttonPreview.Enabled = False

        End If

    End Sub

    Private Sub Form_ConfigurationData_Build_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.bIsLoading = True

        Dim dtAppConfigEnv As DataTable = daoConfig.GetEnvironmentList()
        Me._comboFilterEnv.DataSource = dtAppConfigEnv
        Me._comboFilterEnv.ValueMember = "EnvironmentID"
        Me._comboFilterEnv.DisplayMember = "Name"
        Me._comboFilterEnv.SelectedIndex = -1

        Me.bIsLoading = False

    End Sub

    Private Sub _comboFilterEnv_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _comboFilterEnv.SelectedIndexChanged

        If Me.bIsLoading Then Exit Sub

        If Me._comboFilterEnv.SelectedIndex > -1 Then

            Me.RefreshApplications()

            Me._comboFilterApp.Enabled = True
            Me._comboFilterApp.Focus()

        Else

            Me._comboFilterApp.Enabled = False

        End If

        RaiseEvent FormDataChanged()

    End Sub

    Private Sub _comboFilterApp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _comboFilterApp.SelectedIndexChanged

        If Me.bIsLoading Then Exit Sub

        RaiseEvent FormDataChanged()

    End Sub

    Private Sub RefreshApplications()

        If Me.bIsLoading Then Exit Sub

        If Not Me._comboFilterEnv.SelectedIndex = -1 Then

            Dim dtAppConfigAppList As DataTable = daoConfig.GetApplicationList(CType(Me._comboFilterEnv.SelectedValue, Guid))
            Me._comboFilterApp.DataSource = dtAppConfigAppList
            Me._comboFilterApp.ValueMember = "ApplicationID"
            Me._comboFilterApp.DisplayMember = "Name"
            Me._comboFilterApp.SelectedIndex = -1

        End If

    End Sub

    Private Sub _buttonBuild_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonBuild.Click

        If ValidateInput() Then

            Me._formErrorProvider.Clear()

            Me.Enabled = False

            Dim _warning As String


            If Me._radioBuildAll.Checked Then

                _warning = "WARNING!!!" & vbCrLf & vbCrLf & _
                                        "THIS ACTION WILL REPLACE THE ACTIVE CONFIGURATION FOR ALL APPLICATIONS AND ENVIRONMENTS!" & vbCrLf & vbCrLf & _
                                        "Once built, the configuration settings for all applications and environments will immediately become active and applications will load the new settings on login or application start, depending on the application type." & vbCrLf & vbCrLf & _
                                        "Users logged in to any application must log out and log back in for settings to take effect." & vbCrLf & vbCrLf & _
                                        "ARE YOU SURE YOU WANT TO DO THIS?"

            Else

                _warning = "WARNING!!!" & vbCrLf & vbCrLf & _
                                        "THIS ACTION WILL REPLACE THE ACTIVE CONFIGURATION FOR THIS APPLICATION AND ENVIRONMENT!" & vbCrLf & vbCrLf & _
                                        "Once built, the configuration settings will immediately become active and the application will load the new settings on login or application start, depending on the application type." & vbCrLf & vbCrLf & _
                                        "Users logged in to this application must log out and log back in for settings to take effect." & vbCrLf & vbCrLf & _
                                        "ARE YOU SURE YOU WANT TO DO THIS?"
            End If



            If MessageBox.Show(_warning, "CONFIRM ACTION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then

                Me.BuildConfiguration()

            End If

            Me.Enabled = True

        End If

    End Sub

    Private Function ValidateInput() As Boolean

        ValidateInput = True

        If Me._radioBuildAll.Checked Then
            Return ValidateInput
            Exit Function
        End If

        If Me._comboFilterApp.SelectedIndex = -1 Then
            Me._formErrorProvider.SetError(Me._comboFilterApp, "Required")
            ValidateInput = False
        End If

        If Me._comboFilterEnv.SelectedIndex = -1 Then
            Me._formErrorProvider.SetError(Me._comboFilterEnv, "Required")
            ValidateInput = False
        End If

        Return ValidateInput

    End Function

    Private Sub _buttonPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonPreview.Click

        Try

            Dim _doc As New Xml.XmlDocument
            _doc.LoadXml(daoConfig.GetConfigKeyList(Me._comboFilterApp.SelectedValue, Me._comboFilterEnv.SelectedValue))

            Dim form As New Form_ConfigurationData_View

            form.Title = "PREVIEW CONFIG: " & Me._comboFilterEnv.Text & " - " & Me._comboFilterApp.Text

            form.DisplayDocument = _doc
            form.ShowDialog()
            form.Dispose()

        Catch ex As Exception

            Logger.LogError("Exception: ", Me.GetType(), ex)
            MessageBox.Show(ex.Message, "IRMA Application Error", MessageBoxButtons.OK)
            Dim args(1) As String
            args(0) = Me.Name & " form event: _buttonPreview_Click"
            ErrorHandler.ProcessError(ErrorType.ConfigurationManagerException, args, SeverityLevel.Warning)

        End Try

    End Sub

    Private Sub BuildConfiguration()

        Dim _appConfigApp As New AppConfigAppBO

        If Me._radioBuildAll.Checked Then
            _appConfigApp.UserID = My.Application.CurrentUserID
            _appConfigApp.EnvironmentID = Nothing
        Else
            _appConfigApp.ApplicationID = Me._comboFilterApp.SelectedValue
            _appConfigApp.EnvironmentID = Me._comboFilterEnv.SelectedValue
            _appConfigApp.UserID = My.Application.CurrentUserID
        End If

        Me._formBuildWorker.RunWorkerAsync(_appConfigApp)
        Me._formProgressBar.Show()

        Me.Text = "Build in progress....please wait."

    End Sub

    Private Sub _formBuildWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles _formBuildWorker.DoWork

        Dim _doc As Xml.XmlDocument
        Dim _navigator As Xml.XPath.XPathNavigator

        Dim dtConfigList As DataTable = daoConfig.GetConfigList()
        Dim drConfigList As DataRow

        Dim dtAppEnvList As DataTable
        Dim drAppEnvRow As DataRow

        Dim dtAppConfigAppList As DataTable
        Dim dtAppConfigAppRow As DataRow

        Dim rows() As DataRow

        ' set the AppConfigApp object = to worker argument
        Dim _appConfigApp As AppConfigAppBO = e.Argument

        ' if the environment is specified, we are working with a single build option
        If _appConfigApp.EnvironmentID <> Nothing Then

            ' filter the config list by the specified environment and application
            rows = Me.FilterBuildList(dtConfigList, _appConfigApp)

            ' only do the build if there are settings for this env/app.
            If rows.Length > 0 Then

                _doc = New Xml.XmlDocument
                _navigator = _doc.CreateNavigator

                _navigator.MoveToRoot()

                ' write the xml configuration for this environment and application
                Using writer As Xml.XmlWriter = Xml.XmlWriter.Create(_navigator.AppendChild)

                    writer.WriteStartDocument()
                    writer.WriteStartElement("configuration")
                    writer.WriteStartElement("appSettings")

                    For Each drConfigList In rows

                        writer.WriteStartElement("add")
                        writer.WriteAttributeString("key", drConfigList.Item("KeyName").ToString)
                        writer.WriteAttributeString("value", drConfigList.Item("Value").ToString)
                        writer.WriteEndElement()

                    Next

                    writer.WriteEndElement()
                    writer.WriteEndElement()

                End Using

                _appConfigApp.Configuration = _doc

                ' save the updated config to the database
                daoConfig.Save(_appConfigApp)

            End If

        Else

            ' the environment was not specified so we are building all environments
            dtAppEnvList = daoConfig.GetEnvironmentList()

            ' loop though each environment
            For Each drAppEnvRow In dtAppEnvList.Rows

                dtAppConfigAppList = daoConfig.GetApplicationList(drAppEnvRow.Item("EnvironmentID"))

                ' loop through the application list and build the app config for the current environment
                For Each dtAppConfigAppRow In dtAppConfigAppList.Rows

                    If drAppEnvRow.Item("EnvironmentID").Equals(dtAppConfigAppRow.Item("EnvironmentID")) Then

                        _appConfigApp.EnvironmentID = dtAppConfigAppRow.Item("EnvironmentID")
                        _appConfigApp.ApplicationID = dtAppConfigAppRow.Item("ApplicationID")

                        ' filter the config list by the specified environment and application
                        rows = Me.FilterBuildList(dtConfigList, _appConfigApp)

                        ' only do the build if there are settings for this env/app.
                        If rows.Length > 0 Then

                            _doc = New Xml.XmlDocument
                            _navigator = _doc.CreateNavigator

                            _navigator.MoveToRoot()

                            ' write the xml configuration for this environment and application
                            Using writer As Xml.XmlWriter = Xml.XmlWriter.Create(_navigator.AppendChild)

                                writer.WriteStartDocument()
                                writer.WriteStartElement("configuration")
                                writer.WriteStartElement("appSettings")

                                For Each drConfigList In rows

                                    writer.WriteStartElement("add")
                                    writer.WriteAttributeString("key", drConfigList.Item("KeyName").ToString)
                                    writer.WriteAttributeString("value", drConfigList.Item("Value").ToString)
                                    writer.WriteEndElement()

                                Next

                                writer.WriteEndElement()
                                writer.WriteEndElement()

                            End Using

                            _appConfigApp.Configuration = _doc

                            ' save the updated config to the database
                            daoConfig.Save(_appConfigApp)

                        End If

                    End If

                Next

            Next

        End If

    End Sub

    Private Sub _formBuildWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles _formBuildWorker.RunWorkerCompleted

        Me._formProgressBar.Hide()
        Me.Text = "Build Complete!"

        MessageBox.Show("Configuration build(s) complete and active." & vbCrLf & vbCrLf & _
                        "Users must restart applications for the new settings to take effect.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)

        Me.Close()

    End Sub

    Private Function FilterBuildList(ByVal DataTableList As DataTable, ByVal AppConfigApp As AppConfigAppBO) As DataRow()

        Dim _expression As String = "ApplicationID = '" & AppConfigApp.ApplicationID.ToString("D") & "' AND EnvironmentID = '" & AppConfigApp.EnvironmentID.ToString("D") & "'"
        Dim _rows() As DataRow = Nothing

        _rows = DataTableList.Select(_expression)

        Return _rows

    End Function

End Class