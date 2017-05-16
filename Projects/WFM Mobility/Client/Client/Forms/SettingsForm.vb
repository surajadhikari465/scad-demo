Imports Microsoft.Win32

Public Class SettingsForm

#Region " Control Events"

    Private bIsLoading As Boolean = False

    Private Sub SettingsForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = Cursors.WaitCursor
        Cursor.Show()

        bIsLoading = True

        ' contact the universal handheld sevrice and load the available region codes
        cmbRegion.DataSource = Client.GetRegionList

        ' ensure nothing is selected
        cmbRegion.SelectedIndex = -1

        ' get the current region setting
        Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\WFM Mobile", True)
        Dim _region As String = regKey.GetValue("Region")
        regKey.Close()

        If _region IsNot Nothing Then

            ' if one has been set, select it
            Dim index As Integer

            For index = 0 To cmbRegion.Items.Count - 1
                If cmbRegion.Items.Item(index).ToString = _region Then
                    cmbRegion.SelectedIndex = index
                    Exit For
                End If
            Next

        End If

        ' set the service URI text box value
        txtServiceURI.Text = ConfigurationManager.AppSettings("ServiceURI")

        ' show/hide based on configuration setting
        pnlServiceLocation.Visible = CBool(ConfigurationManager.AppSettings("ShowServiceURI"))

        cmbRegion.Focus()

        Me.Text = My.Resources.Application_Title

        bIsLoading = False

        Cursor.Current = Cursors.Default

    End Sub

    Private Sub cmbRegion_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbRegion.SelectedIndexChanged

        If bIsLoading Then Exit Sub

        Cursor.Current = Cursors.WaitCursor
        Cursor.Show()

        ' load the plugin list
        Dim _pluginList() As Plugin = Client.GetPluginList(CType([Enum].Parse(GetType(Region), cmbRegion.SelectedValue, True), Region))

        If _pluginList.Count = 0 Then

            ' tell the user no plugins are available for the selected region code
            MessageBox.Show(String.Format(My.Resources.MessageBoxText_NoPluginsFound, cmbRegion.SelectedText), My.Resources.MessageBoxCaption_NoPluginsFound, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Nothing)

        End If

        Cursor.Current = Cursors.Default

    End Sub

    Private Sub mnuMainMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMainMenu.Click

        If Not ConfigurationManager.AppSettings("Region") = String.Empty Then

            ' if the region setting is set, let them leave and go back

            DialogResult = Windows.Forms.DialogResult.OK

            Me.Close()

            MainForm.Show()

        Else

            ' force them to set a region code if not is set
            MessageBox.Show(My.Resources.MessageBoxText_RegionRequired, My.Resources.MessageBoxCaption_RegionRequired, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Nothing)

        End If

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Cursor.Current = Cursors.WaitCursor
        Cursor.Show()

        If Not cmbRegion.SelectedIndex = -1 Then

            ' if they selected something, save it and close this form
            ConfigurationManager.AppSettings("Region") = cmbRegion.SelectedText
            ConfigurationManager.Save()

            Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\WFM Mobile", True)
            regKey.SetValue("Region", ConfigurationManager.AppSettings("Region"))

            DialogResult = Windows.Forms.DialogResult.OK

        Else

            ' they must set a region code, inform them about it
            MessageBox.Show(My.Resources.MessageBoxText_RegionSelectionBeforeSave, My.Resources.MessageBoxCaption_RegionSelectionBeforeSave, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Nothing)

        End If

    End Sub

#End Region

End Class