Imports System.Windows.Forms

Public Class ShrinkType

    Private mySession As Session
    Const btnName As String = "btnReasonCode"

    Public Sub New(ByVal session As Session)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.mySession = session
    End Sub

    Private Sub ShrinkType_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'load all store and subteam selections
        Me.StoreTeamLabel.Text = mySession.Subteam
        Me.StoreTeamLabel2.Text = mySession.StoreName

        Dim count As Integer = 1

        For Each shrinkSubType In mySession.ShrinkSubTypes
            If (count <= 15) Then
                Dim control As Control = getControlByName(btnName + CType(count, String))
                If (control IsNot Nothing) Then
                    CType(control, Button).Text = shrinkSubType.ShrinkSubType1
                    CType(control, Button).Tag = shrinkSubType.Abbreviation + ":" + CType(shrinkSubType.ShrinkSubTypeID, String) + ":" + CType(shrinkSubType.InventoryAdjustmentCodeID, String) + ":" + shrinkSubType.ShrinkType
                    count = count + 1
                End If
            End If
        Next

        CheckForSavedSession()

    End Sub

    Private Function getControlByName(ByVal name As String) As Control

        For Each ctl As Control In Me.Controls
            If (ctl.Name = name) Then
                Return ctl
            End If
        Next

        Return Nothing
    End Function

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
                    newSession.ShrinkSubTypes = Me.mySession.ShrinkSubTypes
                    newSession.ShrinkAdjustmentIds = Me.mySession.ShrinkAdjustmentIds
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

    Private Sub btnReasonCode1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReasonCode1.Click
        goToNextScreen(btnReasonCode1.Text, btnReasonCode1.Tag.ToString())
    End Sub

    Private Sub btnReasonCode2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReasonCode2.Click
        goToNextScreen(btnReasonCode2.Text, btnReasonCode2.Tag.ToString())
    End Sub

    Private Sub btnReasonCode3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReasonCode3.Click
        goToNextScreen(btnReasonCode3.Text, btnReasonCode3.Tag.ToString())
    End Sub

    Private Sub btnReasonCode4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReasonCode4.Click
        goToNextScreen(btnReasonCode4.Text, btnReasonCode4.Tag.ToString())
    End Sub

    Private Sub btnReasonCode5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReasonCode5.Click
        goToNextScreen(btnReasonCode5.Text, btnReasonCode5.Tag.ToString())
    End Sub

    Private Sub btnReasonCode6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReasonCode6.Click
        goToNextScreen(btnReasonCode6.Text, btnReasonCode6.Tag.ToString())
    End Sub

    Private Sub btnReasonCode7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReasonCode7.Click
        goToNextScreen(btnReasonCode7.Text, btnReasonCode7.Tag.ToString())
    End Sub

    Private Sub btnReasonCode8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReasonCode8.Click
        goToNextScreen(btnReasonCode8.Text, btnReasonCode8.Tag.ToString())
    End Sub

    Private Sub btnReasonCode9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReasonCode9.Click
        goToNextScreen(btnReasonCode9.Text, btnReasonCode9.Tag.ToString())
    End Sub

    Private Sub btnReasonCode10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReasonCode10.Click
        goToNextScreen(btnReasonCode10.Text, btnReasonCode10.Tag.ToString())
    End Sub

    Private Sub btnReasonCode11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReasonCode11.Click
        goToNextScreen(btnReasonCode11.Text, btnReasonCode11.Tag.ToString())
    End Sub

    Private Sub btnReasonCode12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReasonCode12.Click
        goToNextScreen(btnReasonCode12.Text, btnReasonCode12.Tag.ToString())
    End Sub

    Private Sub btnReasonCode13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReasonCode13.Click
        goToNextScreen(btnReasonCode13.Text, btnReasonCode13.Tag.ToString())
    End Sub

    Private Sub goToNextScreen(ByVal selectedShrinkSubType As String, ByVal shrinkData As String)
        Dim shrinkInfo() As String

        shrinkInfo = shrinkData.Split(":")
        mySession.SetShrinkType(shrinkInfo(0) & "," & shrinkInfo(3))
        mySession.SetInventoryAdjustmentCodeId(shrinkInfo(2))
        mySession.SetShrinkSubTypeId(shrinkInfo(1))
        mySession.SetShrinkSubType(selectedShrinkSubType)
        mySession.hasSubTypeUpdated = True
        ShowShrinkScan()

    End Sub
    
    Private Sub btnReasonCode14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReasonCode14.Click
        goToNextScreen(btnReasonCode14.Text, btnReasonCode14.Tag.ToString())
    End Sub

    Private Sub btnReasonCode15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReasonCode15.Click
        goToNextScreen(btnReasonCode15.Text, btnReasonCode15.Tag.ToString())
    End Sub
End Class