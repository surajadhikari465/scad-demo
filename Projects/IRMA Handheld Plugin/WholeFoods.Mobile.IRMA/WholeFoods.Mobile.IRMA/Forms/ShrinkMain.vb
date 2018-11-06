Imports System.Windows.Forms

Public Class ShrinkMain

    Private mySession As Session

    Public Sub New(ByVal session As Session)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.mySession = session
        AlignText()
    End Sub

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'load all store and subteam selections
        Me.StoreTeamLabel.Text = mySession.Subteam
        Me.StoreTeamLabel2.Text = mySession.StoreName

        Me.ShrinkComboBox.DataSource = mySession.ShrinkTypes
        Me.ShrinkComboBox.DisplayMember = "DisplayMember"
        Me.ShrinkComboBox.ValueMember = "ValueMember"

        If Me.mySession.ShrinkTypes.Rows.Count > 1 Then
            Me.ShrinkComboBox.Enabled = True
        Else
            Me.ShrinkComboBox.Enabled = False
        End If

        ' if one has been set, select it
        Dim index As Integer
        For index = 0 To ShrinkComboBox.Items.Count - 1
            If ShrinkComboBox.Items.Item(index).ToString.StartsWith("spoil") Then
                ShrinkComboBox.SelectedIndex = index
                Exit For
            End If
        Next
        CheckForSavedSession()
    End Sub

    Private Sub CheckForSavedSession()
        Try
            Dim fileWriter As ShrinkFileWriter = New ShrinkFileWriter(Me.mySession)

            If fileWriter.SavedSessionExists Then

                Dim myValues As String() = fileWriter.PREVIOUS_SESSION.Split("_")
                If MessageBox.Show("Would you like to reload your previous Session? (" & myValues(2) & " for " & myValues(1) & ")" & vbCrLf & _
                                   "Clicking No will delete the old session.", _
                                   "Previous Session Exists", MessageBoxButtons.YesNo, _
                                   MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.No Then
                    If MessageBox.Show("Are you sure you want to delete your saved Session?" & vbCrLf & _
                                      "(" & myValues(2) & " for " & myValues(1) & ")", "Delete Session?", _
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Hand, _
                                      MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then

                        fileWriter.DeleteFile(fileWriter.MakeFilePath(fileWriter.PREVIOUS_SESSION))
                        If (Not Me.mySession.SessionName = Nothing) Then
                            Me.mySession.SessionName = Nothing
                        End If
                        Me.Close()
                    Else
                        Me.Close()
                    End If
                Else
                    Cursor.Current = Cursors.WaitCursor
                    Dim newSession As Session = New Session(mySession.ServiceURI)
                    newSession.Region = mySession.Region
                    newSession.MyScanner = Me.mySession.MyScanner
                    Me.mySession = fileWriter.GetFileSession(fileWriter.MakeFilePath(fileWriter.PREVIOUS_SESSION), newSession)
                    mySession.IsLoadedSession = True
                    Cursor.Current = Cursors.Default
                    ShowShrinkScan()
                End If
            End If

        Catch ex As Exception
            MessageBox.Show("Could not edit saved Shrink Session: " + ex.Message)
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    Private Sub mnuCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCancel.Click
        Me.Close()
    End Sub

    Private Sub mnuNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNext.Click
        'UPDATE COMBOBOX SELECTIONS 

        'TFS 5498, Faisal Ahmed
        If Me.ShrinkComboBox.SelectedValue = "NN" Then
            MessageBox.Show("You must select a Shrink Type before recording Shrink", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            Exit Sub
        End If

        mySession.SetShrinkType(Me.ShrinkComboBox.SelectedValue & "," & Me.ShrinkComboBox.Text)

        ShowShrinkScan()

    End Sub

    Private Sub ShowShrinkScan()

        Dim scanShrink As ShrinkScan = New ShrinkScan(Me.mySession)

        Dim res As DialogResult = scanShrink.ShowDialog()

        If res = Windows.Forms.DialogResult.Abort Then
            Me.DialogResult = Windows.Forms.DialogResult.Abort
        ElseIf res = Windows.Forms.DialogResult.OK Then
            Me.Close()
        End If

        scanShrink.Dispose()

    End Sub

    Private Sub AlignText()
        Label1.TextAlign = ContentAlignment.TopRight
        Label2.TextAlign = ContentAlignment.TopRight
        Label3.TextAlign = ContentAlignment.TopRight
    End Sub

End Class