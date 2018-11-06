Option Strict Off
Option Explicit On

Public Class EditResolutionCode

    Private _intResolutionCodeID As Integer
    Private _blnIsEdit As Boolean
    Private _bIsDefault As Boolean = False

    Public Sub New(ByVal ResolutionCodeID As Integer, ByVal IsEdit As Boolean)
        InitializeComponent()

        _intResolutionCodeID = ResolutionCodeID
        _blnIsEdit = IsEdit
    End Sub

    Private Sub EditResolutionCode_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        CenterForm(Me)

        If _blnIsEdit Then
            LoadData(_intResolutionCodeID)
            txtCodeDesc.Enabled = False
        End If

    End Sub

    Private Sub LoadData(ByVal resolutionCodeID As Integer)
        Try
            ' Load all Resolution Codes into combo box
            gRSRecordset = SQLOpenRecordSet("EXEC GetResolutionCode " & resolutionCodeID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            Do While Not gRSRecordset.EOF
                Me.txtCodeDesc.Text = IIf(IsDBNull(gRSRecordset.Fields("Description").Value), "", gRSRecordset.Fields("Description").Value)

                Me.chkDefault.CheckState = IIf(gRSRecordset.Fields("Default").Value = 0, 0, 1)
                _bIsDefault = chkDefault.Checked
                If _bIsDefault = True Then
                    chkDefault.Enabled = False
                    chkActive.Enabled = False
                End If
 
                Me.chkActive.CheckState = IIf(gRSRecordset.Fields("Active").Value = 0, 0, 1)
                gRSRecordset.MoveNext()
            Loop

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

    End Sub


    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click

        Dim sSQL As String

        '' Perform Data change to the ResolutionCodes table
        If _blnIsEdit Then

            sSQL = "EXEC UpdateResolutionCode "
            sSQL = sSQL & _intResolutionCodeID & "," & chkDefault.Checked & "," & chkActive.Checked

            SQLExecute(sSQL, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        Else
            If txtCodeDesc.Text = String.Empty Then
                MsgBox("Please enter a valid description", MsgBoxStyle.Critical)
                Exit Sub
            End If

            If (chkDefault.Checked = True And chkActive.Checked = False) Then
                MsgBox("You can't disable the default resolution code", MsgBoxStyle.Critical)
                chkActive.Checked = False
                Exit Sub
            End If

            sSQL = "EXEC AddResolutionCode "
            sSQL = sSQL & "'" & txtCodeDesc.Text & "'," & chkDefault.Checked & "," & chkActive.Checked

            SQLExecute(sSQL, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End If

  

        If _blnIsEdit Then
            MsgBox("Resolution Code was successfully modified!")
        Else
            MsgBox("Resolution Code was successfully added!")
        End If
        Me.Close()

    End Sub
End Class