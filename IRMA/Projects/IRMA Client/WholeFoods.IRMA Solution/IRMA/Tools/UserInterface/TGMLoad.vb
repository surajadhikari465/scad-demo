Option Strict Off
Option Explicit On
Imports System.Linq

Friend Class frmTGMLoad
  Inherits Form
  Dim fileName As String = Nothing

  Const PIPE As String = "|"

  Public ReadOnly Property IsCanceled() As Boolean
    Get
      Return String.IsNullOrEmpty(fileName) OrElse Not System.IO.File.Exists(fileName)
    End Get
  End Property

  Private Sub frmTGMLoad_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
    RemoveHandler Me.Activated, AddressOf frmTGMLoad_Activated

    Using dialog = New OpenFileDialog()
      dialog.InitialDirectory = My.Application.Info.DirectoryPath
      dialog.CheckFileExists = True
      dialog.CheckPathExists = True
      dialog.ShowReadOnly = False
      dialog.Filter = "TGM Tool Files (*.tgm)|*.TGM"
      dialog.ShowDialog()

      fileName = dialog.FileName
    End Using

    If Not IsCanceled Then
      Me.Refresh()
      Application.DoEvents()
      InitializeForm()
    Else
      Close()
    End If
  End Sub

  Private Sub InitializeForm()
    Dim bUpdate As Boolean
    Dim bError As Boolean
    Dim sString As String
    Dim bValid(4) As Boolean
    Dim sLastType As String = String.Empty
    Dim rsReport As ADODB.Recordset
    Dim lImportMisses, lTemp As Integer
    Dim lFields As Short

    '-- Make sure that the file exists
    Err.Clear()
    On Error Resume Next
    FileOpen(1, fileName, OpenMode.Input)
    FileClose(1)
    On Error GoTo 0
    If Err.Number <> 0 Then
      MsgBox("Invalid file name!", MsgBoxStyle.Exclamation, "Error")
      Me.Close()
      Exit Sub
    End If

    pbCheck.Value = 0
    pbDownload.Value = 0
    pbDownload.Maximum = 4
    pbImport.Value = 0
    lFields = 0

    '-- Find out if they would like to update information
    bUpdate = MsgBox("Do you want this TGM View to be updated" & vbCrLf & "with IRMA's current retails and costs?", MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.DefaultButton2, "Notice!") = MsgBoxResult.Yes

    '-- Start verifying file format
    FileOpen(1, fileName, OpenMode.Input)
    pbCheck.Minimum = 0
    pbCheck.Maximum = (LOF(1) / 128) + 1

    Do While Not EOF(1)
      sString = LineInput(1)

      Select Case Mid(sString, 1, 1)
        Case "1" : bValid(0) = True
          If Fields(sString) <> 8 Then
            bError = True
            Exit Do
          Else
            glTGMTool.SubTeam_No = ReturnField(sString, 2)
            glTGMTool.StartDate = ReturnField(sString, 3)
            glTGMTool.EndDate = ReturnField(sString, 4)
            glTGMTool.Discontinued = ReturnField(sString, 5)
            glTGMTool.HIAH = ReturnField(sString, 6)
            glTGMTool.Query = ReturnField(sString, 7)
            glTGMTool.value = ReturnField(sString, 8)
          End If
        Case "2" : bValid(1) = True
          If Fields(sString) <> 6 Then
            bError = True
            Exit Do
          End If
        Case "3" : bValid(2) = True
          If Fields(sString) <> 6 Then
            bError = True
            Exit Do
          End If
        Case "4" : bValid(3) = True
          If lFields = 0 Then lFields = Fields(sString)
          If Fields(sString) <> lFields Then
            bError = True
            Exit Do
          End If
        Case Else
          bError = True
          Exit Do
      End Select

      pbCheck.Value = Loc(1)

    Loop
    FileClose(1)
    pbCheck.Value = pbCheck.Maximum

    If lFields = 18 Then
      If MsgBox("File is an older version of TGM.  To use it, you will have to update retails/cost." & vbCrLf & "Continue?", MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.DefaultButton2, "Notice!") <> MsgBoxResult.Yes Then
        Me.Close()
        Exit Sub
      End If

      bUpdate = True
    Else
      If bError Or (Not bValid(0)) Or (Not bValid(1)) Then 'bValid(2) is not required to be true
        MsgBox("File is corrupt, an older TGM File version, or not a TGM File.", MsgBoxStyle.Exclamation, "Error")
        Me.Close()
        Exit Sub
      End If
    End If

    FileOpen(1, fileName, OpenMode.Input)

    pbImport.Minimum = 0
    pbImport.Maximum = (LOF(1) / 128) + 1

    '-- If they wanted to update, put in new data
    If bUpdate Then
      '-- Retrieve new Data
      RetrieveTGMData(pbDownload)

      gDBReport.BeginTrans()

      Do While Not EOF(1)
        sString = LineInput(1)

        If Mid(sString, 1, 1) = "4" Then

          If Not IsDBNull(ReturnField(sString, lFields)) Then
            gDBReport.Execute("Update TGMTool " & "SET NewRetail = " & ReturnField(sString, lFields) & " " & "WHERE Instance = " & glInstance & " AND Item_Key = " & ReturnField(sString, 2) & " AND Store_No = " & ReturnField(sString, 3), lTemp, ADODB.CommandTypeEnum.adCmdText + ADODB.ExecuteOptionEnum.adExecuteNoRecords)
            If lTemp = 0 Then lImportMisses = lImportMisses + 1
          End If

        End If

        pbImport.Value = Loc(1)
      Loop
    Else
      pbDownload.Value = pbDownload.Maximum
      gDBReport.BeginTrans()
      rsReport = New ADODB.Recordset

      '-- Remove existing data
      gDBReport.Execute("DELETE FROM TGMTool WHERE Instance = " & glInstance, ADODB.CommandTypeEnum.adCmdText + ADODB.ExecuteOptionEnum.adExecuteNoRecords)
      gDBReport.Execute("DELETE FROM TGMToolHeader WHERE Instance =  " & glInstance, ADODB.CommandTypeEnum.adCmdText + ADODB.ExecuteOptionEnum.adExecuteNoRecords)
      gDBReport.Execute("DELETE FROM TGMStore WHERE Instance = " & glInstance, ADODB.CommandTypeEnum.adCmdText + ADODB.ExecuteOptionEnum.adExecuteNoRecords)

      Do While Not EOF(1)
        sString = LineInput(1)

        Select Case Mid(sString, 1, 1)
          Case "2"
            If sLastType <> "2" Then
              If sLastType <> "" Then rsReport.Close()
              sLastType = "2"
              rsReport.Open("TGMStore", gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdTable)
            End If

            rsReport.AddNew()
            rsReport.Fields("Instance").Value = glInstance
            rsReport.Fields("Store_Name").Value = ReturnField(sString, 2)
            rsReport.Fields("Store_No").Value = ReturnField(sString, 3)
            rsReport.Fields("Mega_Store").Value = ReturnField(sString, 4)
            rsReport.Fields("WFM_Store").Value = ReturnField(sString, 5)
            rsReport.Fields("Zone_ID").Value = ReturnField(sString, 6)
            rsReport.Update()

          Case "3"
            If sLastType <> "3" Then
              If sLastType <> "" Then rsReport.Close()
              sLastType = "3"
              rsReport.Open("TGMToolHeader", gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdTable)
            End If

            rsReport.AddNew()
            rsReport.Fields("Instance").Value = glInstance
            rsReport.Fields("Store_No").Value = ReturnField(sString, 2)
            rsReport.Fields("TotalActualRetail").Value = ReturnField(sString, 3)
            rsReport.Fields("TotalRetail").Value = ReturnField(sString, 4)
            rsReport.Fields("TotalCost").Value = ReturnField(sString, 5)
            rsReport.Fields("TotalExtCost").Value = ReturnField(sString, 6)
            rsReport.Update()

          Case "4"
            If sLastType <> "4" Then
              If sLastType <> "" Then rsReport.Close()
              sLastType = "4"
              rsReport.Open("TGMTool", gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdTable)
            End If

            rsReport.AddNew()
            rsReport.Fields("Instance").Value = glInstance
            rsReport.Fields("Item_Key").Value = ReturnField(sString, 2)
            rsReport.Fields("Store_No").Value = ReturnField(sString, 3)
            rsReport.Fields("Identifier").Value = ReturnField(sString, 4)
            rsReport.Fields("Item_Description").Value = ReturnField(sString, 5)
            rsReport.Fields("Package_Desc").Value = ReturnField(sString, 6)
            rsReport.Fields("Category_ID").Value = ReturnField(sString, 7)
            rsReport.Fields("CurrentCost").Value = ReturnField(sString, 8)
            rsReport.Fields("CurrentExtCost").Value = ReturnField(sString, 9)
            rsReport.Fields("CurrentRetail").Value = ReturnField(sString, 10)
            rsReport.Fields("Sold_By_weight").Value = ReturnField(sString, 11)
            rsReport.Fields("TotalQuantity").Value = ReturnField(sString, 12)
            rsReport.Fields("TotalActualRetail").Value = ReturnField(sString, 13)
            rsReport.Fields("TotalRetail").Value = ReturnField(sString, 14)
            rsReport.Fields("TotalCost").Value = ReturnField(sString, 15)
            rsReport.Fields("TotalExtCost").Value = ReturnField(sString, 16)
            rsReport.Fields("NewRetail").Value = ReturnField(sString, 17)
            rsReport.Update()

        End Select

        pbImport.Value = Loc(1)
      Loop

      If sLastType <> "" Then rsReport.Close()
    End If

    FileClose(1)
    pbImport.Value = pbImport.Maximum
    rsReport = Nothing
    gDBReport.CommitTrans()
    If gJetFlush IsNot Nothing Then
      gJetFlush.RefreshCache(gDBReport)
    End If

    If lImportMisses > 0 Then
      If MsgBox(lImportMisses & " Record(s) could were not located in the new view." & vbCrLf & "Some TGM data will be lost, continue anyway?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Data has changed.") = MsgBoxResult.No Then
        Me.Close()
        Exit Sub
      End If
    End If

    glTGMTool.FileName = fileName
  End Sub

  Private Function Fields(ByRef value As String) As Integer
    Return value.Count(Function(x) x = PIPE) + 1
  End Function

  Private Function ReturnField(ByRef sString As String, ByVal lField As Integer) As Object
    Dim lLoop As Integer
    Dim sTemp As String

    sTemp = sString

    For lLoop = 2 To lField
      sTemp = Mid(sTemp, InStr(1, sTemp, PIPE) + 1)
    Next lLoop

    If InStr(1, sTemp, PIPE) > 0 Then
      sTemp = Mid(sTemp, 1, InStr(1, sTemp, PIPE) - 1)
    End If

    If sTemp = "" Then
      ReturnField = System.DBNull.Value
    Else
      ReturnField = sTemp
    End If
  End Function
End Class