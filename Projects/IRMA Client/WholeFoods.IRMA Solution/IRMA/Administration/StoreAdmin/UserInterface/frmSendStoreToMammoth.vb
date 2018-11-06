Imports WholeFoods.IRMA.Administration.StoreAdmin.DataAccess
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Public Class frmSendStoreToMammoth
    Private _storeBO As StoreBO
    Private Sub frmSendStoreToMammoth_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BindStoreData()
    End Sub
    Private Sub BindStoreData()
        'bind data to store list drop down
        Me.ComboBox_StoreList.DataSource = StoreDAO.GetStores()
        Me.ComboBox_StoreList.DisplayMember = "StoreName"
        Me.ComboBox_StoreList.ValueMember = "StoreNo"
        Me.ComboBox_StoreList.SelectedIndex = -1
    End Sub
    Private Sub ResetUIStatus()
        Button_StartJob.Enabled = False
        Label_JobStatus.Text = "Send Store To Mammoth process is executing."
        Label_ExceptionText.Visible = False
        
        Me.Refresh()

    End Sub
    Private Sub Button_StartJob_Click(sender As Object, e As EventArgs) Handles Button_StartJob.Click
        Dim storeDAO As New StoreDAO
        Dim returnValues As New DataTable

        If Me.ComboBox_StoreList.SelectedIndex < 0 Then
            'prompt user
            MessageBox.Show(ResourcesAdministration.GetString("msg_validation_selectStore"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        Else
            Try
                ' Disable the button while the process is running and update the status on the UI
                Windows.Forms.Cursor.Current = Cursors.WaitCursor
                ' Reset the status messages
                ResetUIStatus()

                Label_JobStatus.Text = "Queuing up Mammoth events for store " + Me.ComboBox_StoreList.GetItemText(ComboBox_StoreList.SelectedItem)
                Me.Refresh()

                _storeBO = New StoreBO()
                _storeBO.StoreNo = CType(Me.ComboBox_StoreList.SelectedValue, Integer)
                returnValues = storeDAO.QueueNewStoreItemMammothEvents(_storeBO)
                Dim successCode As Boolean
                Dim returnMessage As String

                If Not returnValues Is Nothing Then
                    successCode = Convert.ToBoolean(returnValues.Rows(0).Item("successCode").ToString())
                    returnMessage = returnValues.Rows(0).Item("message").ToString()

                    If successCode Then
                        Label_JobStatus.Text = "Successfully completed. " + returnMessage
                        Me.Refresh()
                    Else
                        Label_JobStatus.Text = "Queuing Mammoth events for store " + Me.ComboBox_StoreList.GetItemText(ComboBox_StoreList.SelectedItem) + " failed. "
                        Label_ExceptionText.Text = returnMessage
                        Label_ExceptionText.Visible = True
                        Me.Refresh()
                    End If
                End If
            Catch e1 As Exception
                ' An error occurred during processing - display a message and enable the button
                Label_JobStatus.Text = "Error during Send Store To Mammoth process: " & e1.Message()
                Label_ExceptionText.Text = e1.ToString()
                Label_ExceptionText.Visible = True
            Finally
                Button_StartJob.Enabled = True
                Windows.Forms.Cursor.Current = Cursors.Default
                Me.Refresh()
            End Try
        End If
    End Sub
End Class