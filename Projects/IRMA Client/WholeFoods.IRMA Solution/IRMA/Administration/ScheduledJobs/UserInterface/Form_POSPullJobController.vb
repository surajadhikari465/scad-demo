Imports System.Configuration
Imports WholeFoods.IRMA.Replenishment.Jobs
Imports WholeFoods.Utility


Public Class Form_POSPullJobController
    Private storeNo As Integer
    Private storeInformation As List(Of StoreInfo) = New List(Of StoreInfo)

    ''' <summary>
    ''' Form Load functionality
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_POSPullJobController_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Gets the IBM and NCR filenames from the config file
        GetFilenames()
        ' Loads the stores from the database and populates the store combobox
        LoadStores()

    End Sub

    ''' <summary>
    ''' Populates the combobox with stores from the database
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadStores()
        Dim storeDAO As New StoreInfoDAO
        storeInformation.Clear()
        storeInformation = storeDAO.GetStorePOSPullInfo

        For Each store As StoreInfo In storeInformation
            ComboBox_Stores.Items.Add(store)
        Next

        If ComboBox_Stores.Items.Count > 0 Then
            Dim AllStores As New StoreInfo
            AllStores.StoreNo = -1
            AllStores.StoreName = "All Stores"
            AllStores.StoreAbbr = "All"
            ComboBox_Stores.Items.Insert(0, AllStores)
        Else
            Dim NoStores As New StoreInfo
            NoStores.StoreNo = 0
            NoStores.StoreName = "No Stores"
            NoStores.StoreAbbr = "None"
            ComboBox_Stores.Items.Insert(0, NoStores)
        End If
        ComboBox_Stores.DisplayMember = "StoreName"
        ComboBox_Stores.ValueMember = "StoreNo"

        ' Default to all stores
        ComboBox_Stores.SelectedIndex = 0

    End Sub

    ''' <summary>
    ''' This is the main function that calls the POSPullJob's main function and kicks off the process.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PullCatalogsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PullCatalogsButton.Click
        Try
            Dim currentStore As StoreInfo
            Dim jobStatus As Integer

            If ComboBox_Stores.SelectedItem Is Nothing Then
                MessageBox.Show("You must select a store.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            Else
                currentStore = CType(ComboBox_Stores.SelectedItem, StoreInfo)
            End If

            If IBMFileNameTextBox.Text = String.Empty Or NCRFileNameTextBox.Text = String.Empty Then
                If MsgBox("Filenames cannot be blank, reset to default?", CType(MsgBoxStyle.YesNo + MsgBoxStyle.Question, MsgBoxStyle), Me.Text) = MsgBoxResult.Yes Then
                    GetFilenames()
                Else
                    Exit Sub
                End If
            End If

            ' Disable the button while the process is running and update the status on the UI
            PullCatalogsButton.Enabled = False
            StatusStrip.Text = "Pulling POS data..."
            ResetScreen()

            ' Start the job
            Dim pullJob As POSPullJob = New POSPullJob

            ' THIS IS WHERE ALL THE MAGIC HAPPENS
            jobStatus = pullJob.Main(currentStore.StoreNo)

            ' The job finished executing - update the status and enable the button
            If jobStatus = 0 Then
                StatusStrip.Text = "Error during POS Pull process."

                ' Display the error status, including the error message if there is one.
                If pullJob.ErrorMessage IsNot Nothing Then
                    ExceptionLabel.Text = String.Format("Error: {0}", pullJob.ErrorMessage())
                End If

                ' Show the exception stack trace if it's available.
                If pullJob.ErrorException IsNot Nothing Then
                    If ExceptionLabel.Text <> String.Empty Then
                        ExceptionLabel.Text = String.Format("{0} Exception: {1}", ExceptionLabel.Text, pullJob.ErrorException.ToString())
                    Else
                        ExceptionLabel.Text = String.Format("Exception: {0}", pullJob.ErrorException.ToString())
                    End If
                End If
                If ExceptionLabel.Text.Length > 0 Then DisplayMessage()
            Else
                If jobStatus = 1 Then
                    StatusStrip.Text = "POS Pull process completed successfully."
                ElseIf jobStatus = -1 Then
                    StatusStrip.Text = "POS Pull process could not complete."
                End If
                If pullJob.StatusMessage.Length > 0 Then
                    ExceptionLabel.Text = String.Format("{0}{1}{0}{0}", vbLf, pullJob.StatusMessage)
                    DisplayMessage()
                End If
            End If
        Catch e1 As Exception
            ' An error occurred during processing - display a message and enable the button
            StatusStrip.Text = "Error during POS Pull process."
            ExceptionLabel.Text = e1.ToString()
            DisplayMessage()
        Finally
            PullCatalogsButton.Enabled = True
            Me.Refresh()
        End Try

    End Sub
    Private Sub DisplayMessage()
        If ExceptionLabel.Visible = True Then Exit Sub
        Dim labelHeight As Integer

        ExceptionLabel.Visible = True
        labelHeight = ExceptionLabel.Height

        Me.Height = Me.Height + labelHeight + 20
    End Sub
    Private Sub ResetScreen()
        Me.Refresh()
        ExceptionLabel.Text = String.Empty
        ExceptionLabel.Visible = False
        Me.Height = 247
    End Sub

    Private Sub ExitButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitButton.Click
        Me.Close()
    End Sub

    Private Sub GetFilenames()
        'populate the filenames from the config file
        IBMFileNameTextBox.Text = ConfigurationServices.AppSettings("POSPullIBMFileName")
        NCRFileNameTextBox.Text = ConfigurationServices.AppSettings("POSPullNCRFileName")
    End Sub
   
    Private Sub DefaultFileNamesButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DefaultFileNamesButton.Click
        GetFilenames()
    End Sub

    Private Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim reportserver As String
        reportserver = ConfigurationServices.AppSettings("reportingServicesURL")

        Dim sReportURL As New System.Text.StringBuilder
        'report name
        sReportURL.Append("IRMAvsPOSAudit")

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=True")

        Dim Browser As New WebBrowser

        Browser.Navigate(reportserver & sReportURL.ToString, True)
        Browser.Visible = True
    End Sub
End Class