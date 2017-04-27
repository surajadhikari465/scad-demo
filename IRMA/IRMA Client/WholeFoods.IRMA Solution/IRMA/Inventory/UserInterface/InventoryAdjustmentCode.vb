Option Strict Off
Option Explicit On

Public Class InventoryAdjustmentCode

    Private _intInventoryAdjustmentCodeID As Integer
    Private _blnIsEdit As Boolean

    Private _blnIsAllowDecrease As Boolean
    Private _blnIsAllowIncrease As Boolean
    Private _blnIsAllowReset As Boolean
    Private _intAdjustmentType As Integer
    Private _intIndex As Integer

    Public Sub New(ByVal InventoryAdjustmentCodeID As Integer, ByVal IsEdit As Boolean)
        InitializeComponent()

        _intInventoryAdjustmentCodeID = InventoryAdjustmentCodeID
        _blnIsEdit = IsEdit
    End Sub

    Private Sub InventoryAdjustmentCode_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        CenterForm(Me)
        LoadInventoryEffectCombo()
        InitInventoryEffectFlags()

        If _blnIsEdit Then
            SetActive(txtCodeAbbrev(), False)
            LoadData(_intInventoryAdjustmentCodeID)
        End If
       
    End Sub

    Private Sub LoadData(ByVal iInventoryAdjustmentCodeID As Integer)
        Try
            ' Load all Inventory Adjustment Codes into combo box
            gRSRecordset = SQLOpenRecordSet("EXEC GetInventoryAdjustmentCode " & iInventoryAdjustmentCodeID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            Do While Not gRSRecordset.EOF
                Me.txtCodeAbbrev.Text = IIf(IsDBNull(gRSRecordset.Fields("Abbreviation").Value), "", gRSRecordset.Fields("Abbreviation").Value)
                Me.txtCodeDesc.Text = IIf(IsDBNull(gRSRecordset.Fields("AdjustmentDescription").Value), "", gRSRecordset.Fields("AdjustmentDescription").Value)
                Me.txtGLCode.Text = IIf(IsDBNull(gRSRecordset.Fields("GLAccount").Value), "", gRSRecordset.Fields("GLAccount").Value)
                If (gRSRecordset.Fields("AllowsInventoryAdd").Value) And (gRSRecordset.Fields("AllowsInventoryDelete").Value) Then
                    _intIndex = 2                                       ' Variable Inventory 
                End If
                If (gRSRecordset.Fields("AllowsInventoryAdd").Value) And Not (gRSRecordset.Fields("AllowsInventoryDelete").Value) Then
                    _intIndex = 1                                       ' Increase Inventory 
                End If
                If Not (gRSRecordset.Fields("AllowsInventoryAdd").Value) And (gRSRecordset.Fields("AllowsInventoryDelete").Value) Then
                    _intIndex = 0                                       ' Decrease Inventory
                End If

                cmbInventoryEffect.SelectedIndex = _intIndex

                Me.chkEXEWarehouse.CheckState = IIf(gRSRecordset.Fields("ActiveForWarehouse").Value = 0, 0, 1)
                Me.chkStore.CheckState = IIf(gRSRecordset.Fields("ActiveForStore").Value = 0, 0, 1)
                gRSRecordset.MoveNext()
            Loop

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

    End Sub

    Private Sub LoadInventoryEffectCombo()
        cmbInventoryEffect.Items.Add("Decrease")
        cmbInventoryEffect.Items.Add("Increase")
        cmbInventoryEffect.Items.Add("Variable")
    End Sub

    Private Sub InitInventoryEffectFlags()
        _blnIsAllowDecrease = False
        _blnIsAllowIncrease = False
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click

        ' Validate Input data
        If Trim(txtCodeAbbrev.Text) = "" Then
            MsgBox("Blank Inventory Code Abbreviation found!")
            txtCodeAbbrev.Focus()
            Exit Sub
        End If

        If txtCodeDesc.Text = "" Then
            MsgBox("Blank Inventory Code Description found!")
            txtCodeDesc.Focus()
            Exit Sub
        End If

        If txtCodeAbbrev.Text.Length > 2 Then
            MsgBox("Inventory Adjustment Code must be limited to two characters!")
            txtCodeAbbrev.Focus()
            Exit Sub
        End If

        If txtGLCode.Text.Length > 0 Then
            If Not IsNumeric(txtGLCode.Text) Then
                MsgBox("GL Code must be numeric!")
                txtGLCode.Focus()
                Exit Sub
            End If
        End If

        Dim sSQL As String

        ' Check if Inventory Abrreviation already exists in database and DataChange is "Add"
        If Not (_blnIsEdit) Then
            Try
                sSQL = "EXEC CheckInventoryAdjustmentAbbreviation '" & Trim(txtCodeAbbrev.Text) & "'"
                gRSRecordset = SQLOpenRecordSet("EXEC CheckInventoryAdjustmentAbbreviation '" & Trim(UCase(txtCodeAbbrev.Text)) & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

                If Not (gRSRecordset.EOF) Then
                    MsgBox("Please change to a different code, duplicate codes not allowed!")
                    If gRSRecordset IsNot Nothing Then
                        gRSRecordset.Close()
                        gRSRecordset = Nothing
                    End If
                    Exit Sub
                End If
            Finally
                If gRSRecordset IsNot Nothing Then
                    gRSRecordset.Close()
                    gRSRecordset = Nothing
                End If
            End Try
        End If

        ' These two fields are hard-coded for now as per Russell M.
        _intAdjustmentType = 8
        _blnIsAllowReset = False


        ' Perform Data change to the InventoryAdjustmentCode table

        If _blnIsEdit Then
            sSQL = "EXEC UpdateInventoryAdjustmentCode "
        Else
            sSQL = "EXEC InsertInventoryAdjustmentCode "
        End If

        sSQL = sSQL & _
                        "'" & Trim(UCase(txtCodeAbbrev.Text)) & "'" & _
                        ", '" & Trim(txtCodeDesc.Text) & "'" & _
                        ", " & _blnIsAllowIncrease & _
                        ", " & _blnIsAllowDecrease & _
                        ", " & _blnIsAllowReset & _
                        ", " & txtGLCode.Text & _
                        ", " & chkEXEWarehouse.CheckState & _
                        ", " & chkStore.CheckState & _
                        ", " & _intAdjustmentType & _
                        ", " & giUserID & _
                        ", '" & VB6.Format(SystemDateTime, "MM/DD/YYYY HH:MM:SS") & "'"

        SQLExecute(sSQL, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        If _blnIsEdit Then
            MsgBox("Inventory Adjustment data was successfully modified!")
        Else
            MsgBox("Inventory Adjustment data was successfully added!")
        End If
        Me.Close()

    End Sub

    Private Sub cmbInventoryEffect_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbInventoryEffect.SelectedIndexChanged

        ' Get Inventory Effect
        Select Case cmbInventoryEffect.SelectedIndex
            Case 0                                      ' Decrease Inventory
                _blnIsAllowDecrease = True
                _blnIsAllowIncrease = False
            Case 1                                      ' Increase Inventory
                _blnIsAllowIncrease = True
                _blnIsAllowDecrease = False
            Case 2                                      ' Variable Inventory
                _blnIsAllowDecrease = True
                _blnIsAllowIncrease = True
        End Select
    End Sub

End Class