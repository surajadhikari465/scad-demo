Option Strict Off
Option Explicit On
Friend Class frmCycleCountHistoryEdit
    Inherits System.Windows.Forms.Form

    Private mbNew As Boolean
    Private mbLoading As Boolean
    Private mbChanged As Boolean
    Private mbUpdated As Boolean
    Private mbUserReadOnly As Boolean
    Private mbOpen As Boolean
    Private mbExternal As Boolean

    Private mlCycleCountItemID As Integer
    Private mbWeightedItem As Boolean

    Private mdPD1 As Decimal
    Private mdPD2 As Decimal
    Private mlPDU As Integer

    Public Function LoadForm(ByRef lCycleCountItemID As Integer, ByRef sItemDesc As String, ByRef bOpen As Boolean, ByRef bExternal As Boolean, ByRef PD1 As Decimal, ByRef PD2 As Decimal, ByRef PDU As Integer, ByRef bWeightedItem As Boolean, ByVal dDateTime As Date) As Boolean

        mbLoading = True
        mbOpen = bOpen

        'Determine if the user form level access.
        If gbInventoryAdministrator Or gbBuyer Then
            mbUserReadOnly = False
        Else
            mbUserReadOnly = True
        End If

        mbNew = IIf((CStr(dDateTime) = "12:00:00 AM"), True, False)
        mbUpdated = False
        mlCycleCountItemID = lCycleCountItemID
        mbWeightedItem = bWeightedItem
        mdPD1 = PD1
        mdPD2 = PD2
        mlPDU = PDU
        mbExternal = bExternal

        If mbNew Then
            optCases.Checked = True
            txtDateTime.Text = CStr(SystemDateTime())
        Else
            txtDateTime.Text = CStr(dDateTime)
        End If
        txtDateTime.Text = VB6.Format(txtDateTime.Text, "MM/DD/YY HH:MM:SS")

        txtItemDesc.Text = sItemDesc
        txtPackSize.Value = PD1

        'Show the applicable entry boxes.
        lblUnits.Text = IIf(mbWeightedItem, "Weight :", "Units :")
        lblCases.Text = IIf(mbWeightedItem, "Boxes :", "Cases :")

        optUnits2.Visible = mbWeightedItem
        lblUnits2.Visible = mbWeightedItem
        txtUnits2.Visible = mbWeightedItem

        mbLoading = False

        If Not mbNew Then
            Call LoadHistoryItem()
        Else
            Call SetButtons()
        End If

        Call SetFormReadOnly()

        Me.txtPackSize.Focus()
        Me.txtPackSize.SelectAll()

        Me.ShowDialog()

        'Set the return value.
        LoadForm = mbUpdated

    End Function

    Private Sub frmCycleCountHistoryEdit_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated

        If mbNew Then
            Me.Text = "New Cycle Count Entry"
        Else
            Me.Text = "Cycle Count Entry Edit"
        End If

    End Sub

    Private Sub LoadHistoryItem()

        Dim rsItem As DAO.Recordset = Nothing

        Try
            rsItem = SQLOpenRecordSet("EXEC GetCycleCountHistoryItem " & mlCycleCountItemID & "," & "'" & Me.txtDateTime.Text & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            mbLoading = True

            txtPackSize.Value = IIf(Trim(rsItem.Fields("PackSize").Value) = "", txtPackSize.Value, rsItem.Fields("PackSize").Value)
            txtUnits.Value = IIf(IsDBNull(rsItem.Fields("Count").Value), IIf(IsDBNull(rsItem.Fields("Weight").Value), "", rsItem.Fields("Weight").Value), rsItem.Fields("Count").Value)

            If (rsItem.Fields("IsCaseCnt")).Value Then
                optCases.Checked = True
            Else
                optUnits.Checked = True
            End If

            mbLoading = False

        Finally
            If rsItem IsNot Nothing Then
                rsItem.Close()
                rsItem = Nothing
            End If
        End Try

        Call CalcCases()
        Call CalcUnits2()

        Call SetButtons()

    End Sub

    Private Sub SetButtons()

        Select Case True

            Case optUnits.Checked
                txtUnits.ReadOnly = False
                txtUnits.BackColor = System.Drawing.SystemColors.Window
                txtUnits.TabStop = True

                txtCases.ReadOnly = True
                txtCases.BackColor = COLOR_INACTIVE
                txtCases.TabStop = False

                txtUnits2.ReadOnly = True
                txtUnits2.BackColor = COLOR_INACTIVE
                txtUnits2.TabStop = False

            Case optCases.Checked
                txtUnits.ReadOnly = True
                txtUnits.BackColor = COLOR_INACTIVE
                txtUnits.TabStop = False

                txtUnits2.ReadOnly = True
                txtUnits2.BackColor = COLOR_INACTIVE
                txtUnits2.TabStop = False

                txtCases.ReadOnly = False
                txtCases.BackColor = System.Drawing.SystemColors.Window
                txtCases.TabStop = True

            Case optUnits2.Checked
                txtUnits.ReadOnly = True
                txtUnits.BackColor = COLOR_INACTIVE
                txtUnits.TabStop = False

                txtUnits2.ReadOnly = False
                txtUnits2.BackColor = System.Drawing.SystemColors.Window
                txtUnits2.TabStop = True

                txtCases.ReadOnly = True
                txtCases.BackColor = COLOR_INACTIVE
                txtCases.TabStop = False
        End Select

    End Sub

    Private Sub Changed(Optional ByRef bChanged As Boolean = True)

        If mbUserReadOnly Or Not mbOpen Or mbExternal Then Exit Sub

        If bChanged = True Then
            mbChanged = True
        Else
            mbChanged = False
        End If

    End Sub

    Private Sub CalcUnits()
        mbLoading = True

        If Not IsDBNull(txtCases.Value) And Not IsDBNull(txtPackSize.Value) Then
            If txtCases.Value > 0 And txtPackSize.Value > 0 Then
                If giCase = 0 And giBox > 0 Then
                    txtUnits.Value = System.Math.Round(CostConversion(CDec(txtCases.Value), IIf(mbWeightedItem, mlPDU, giUnit), giBox, CDec(txtPackSize.Value), mdPD2, mlPDU, 0, 0), 2)
                Else
                    txtUnits.Value = System.Math.Round(CostConversion(CDec(txtCases.Value), IIf(mbWeightedItem, mlPDU, giUnit), giCase, CDec(txtPackSize.Value), mdPD2, mlPDU, 0, 0), 2)
                End If

            End If
        End If

        mbLoading = False
    End Sub

    Private Sub CalcUnits2()
        mbLoading = True

        If mbWeightedItem Then
            If Not IsDBNull(txtCases.Value) And Not IsDBNull(txtPackSize.Value) Then
                If txtCases.Value > 0 And txtPackSize.Value > 0 Then
                    txtUnits2.Value = System.Math.Round(CostConversion(CDec(txtCases.Value), giUnit, giCase, CDec(txtPackSize.Value), mdPD2, mlPDU, 0, 0), 2)
                End If
            End If
        End If

        mbLoading = False
    End Sub

    Private Sub CalcCases()
        mbLoading = True

        'Try txtUnits first for display of existing data - everything is saved as units/weight
        If Not IsDBNull(txtUnits.Value) And Not IsDBNull(txtPackSize.Value) Then
            If txtUnits.Value > 0 And txtPackSize.Value > 0 Then
                If giCase = 0 And giBox > 0 Then
                    txtCases.Value = CStr(System.Math.Round(CostConversion(CDec(txtUnits.Value), giBox, IIf(mbWeightedItem, mlPDU, giUnit), CDec(txtPackSize.Value), mdPD2, mlPDU, 0, 0), 2))
                Else
                    txtCases.Value = CStr(System.Math.Round(CostConversion(CDec(txtUnits.Value), giCase, IIf(mbWeightedItem, mlPDU, giUnit), CDec(txtPackSize.Value), mdPD2, mlPDU, 0, 0), 2))
                End If

            End If
        End If

        If optUnits2.Checked Then
            If Not IsDBNull(txtUnits.Value) And Not IsDBNull(txtPackSize.Value) Then
                If txtUnits2.Value > 0 And txtPackSize.Value > 0 Then
                    txtCases.Value = CStr(System.Math.Round(CostConversion(CDec(txtUnits2.Value), giCase, giUnit, CDec(txtPackSize.Value), mdPD2, mlPDU, 0, 0), 2))
                End If
            End If
        End If

        mbLoading = False
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        If Not mbUserReadOnly And Not mbExternal Then

            '-- First validate the form's data.
            If Not ValidateData() Then Exit Sub

            Call Save()

        End If

        Me.Close()

    End Sub

    Private Function Save() As Boolean

        Dim sWeight As String
        Dim sCount As String
        Dim sDate As String

        Save = False

        If mbChanged Then

            sDate = "'" & Trim(txtDateTime.Text) & "'"

            sCount = IIf(mbWeightedItem, "null", "'" & txtUnits.Value & "'")

            If mbWeightedItem Then
                If optUnits2.Checked Then
                    sWeight = "'" & txtUnits2.Value * mdPD2 & "'"
                Else
                    sWeight = "'" & txtUnits.Value & "'"
                End If
            Else
                sWeight = "null"
            End If

            '--Update the record.
            SQLExecute("EXEC UpdateCycleCountHistory " & mlCycleCountItemID & "," & _
                                                                                    sDate & "," & _
                                                                                    sCount & "," & _
                                                                                    sWeight & "," & _
                                                                                    txtPackSize.Value & "," & _
                                                                                    IIf(optCases.Checked Or optUnits2.Checked, 1, 0), DAO.RecordsetOptionEnum.dbSQLPassThrough)

            Save = True
            mbUpdated = True

        End If

    End Function

    Private Function ValidateData() As Boolean

        ValidateData = True

        If IsDBNull(txtPackSize.Value) Then ValidateData = False
        If ValidateData Then
            If txtPackSize.Value = 0 Then ValidateData = False
        End If
        If Not ValidateData Then
            Call MsgBox("Pack Size entry is required.", vbCritical, Me.Text)
            txtPackSize.Focus()
            ValidateData = False
            Exit Function
        End If

        If optUnits.Checked Then
            If IsDBNull(txtUnits.Value) Then ValidateData = False
            If ValidateData Then
                If txtUnits.Value = 0 Then ValidateData = False
            End If
            If Not ValidateData Then
                Call MsgBox("Weight entry is required.", MsgBoxStyle.Critical, Me.Text)
                ValidateData = False
                txtUnits.Focus()
                Exit Function
            End If
        End If

        If optCases.Checked Then
            If IsDBNull(txtCases.Value) Then ValidateData = False
            If ValidateData Then
                If txtCases.Value = 0 Then ValidateData = False
            End If
            If Not ValidateData Then
                Call MsgBox("Cases entry is required.", MsgBoxStyle.Critical, Me.Text)
                ValidateData = False
                txtCases.Focus()
                Exit Function
            End If
        End If

        If optUnits2.Checked Then
            If IsDBNull(txtUnits2.Value) Then ValidateData = False
            If ValidateData Then
                If txtUnits2.Value = 0 Then ValidateData = False
            End If
            If Not ValidateData Then
                Call MsgBox("Units entry is required.", MsgBoxStyle.Critical, Me.Text)
                ValidateData = False
                txtUnits2.Focus()
                Exit Function
            End If
        End If

    End Function

    Private Sub SetFormReadOnly()

        If mbUserReadOnly Or Not mbOpen Or mbExternal Then

            optUnits.Enabled = False
            optCases.Enabled = False
            optUnits2.Enabled = False


            txtPackSize.BackColor = System.Drawing.SystemColors.InactiveCaption
            txtPackSize.ReadOnly = True
            txtUnits.BackColor = System.Drawing.SystemColors.InactiveCaption
            txtUnits.ReadOnly = True
            txtCases.BackColor = System.Drawing.SystemColors.InactiveCaption
            txtCases.ReadOnly = True
            If txtUnits2.Visible Then
                txtUnits2.BackColor = COLOR_INACTIVE
                txtUnits2.ReadOnly = True
            End If
        End If

    End Sub

    Private Sub optUnits2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optUnits2.CheckedChanged

        Call SetButtons()

    End Sub

    Private Sub optUnits_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optUnits.CheckedChanged

        Call SetButtons()

    End Sub

    Private Sub optCases_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optCases.CheckedChanged

        Call SetButtons()

    End Sub

    Private Sub txtUnits2_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUnits2.ValueChanged

        If mbLoading Then Exit Sub

        Call Changed()
        Call CalcCases()
        Call CalcUnits()

    End Sub

    Private Sub txtPackSize_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPackSize.GotFocus

        txtPackSize.SelectAll()

    End Sub

    Private Sub txtUnits_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUnits.GotFocus

        txtUnits.SelectAll()

    End Sub

    Private Sub txtUnits2_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUnits2.GotFocus

        txtUnits2.SelectAll()

    End Sub

    Private Sub txtCases_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCases.GotFocus

        txtCases.SelectAll()

    End Sub

    Private Sub txtPackSize_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPackSize.ValueChanged

        If mbLoading Then Exit Sub

        Call Changed()
        Call CalcCases()

        Call CalcUnits()
        Call CalcUnits2()

    End Sub

    Private Sub txtUnits_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUnits.ValueChanged

        If mbLoading Then Exit Sub

        Call Changed()

        Call CalcCases()
        Call CalcUnits2()

    End Sub

    Private Sub txtCases_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCases.ValueChanged

        If mbLoading Then Exit Sub

        Call Changed()
        Call CalcUnits()
        Call CalcUnits2()

    End Sub
End Class