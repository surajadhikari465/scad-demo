Option Strict Off
Option Explicit On

Imports System.Text
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Friend Class frmItemIdentifierList
    Inherits System.Windows.Forms.Form

    Private m_bIsScaleItem As Boolean
    Private m_bIsEXEDistributed As Boolean
    Private mdt As DataTable
    Private isShowOption_DoNotSendToScale As Boolean

    Public ReadOnly Property IsScaleItem() As Boolean
        Get
            IsScaleItem = m_bIsScaleItem
        End Get
    End Property

    Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click
        Dim fItemIdentifierAdd As New frmItemIdentifierAdd
        fItemIdentifierAdd.pbAddToDatabase = True

        'get subteam for this item; all rows have the same SubTeam_No value so grab first row
        fItemIdentifierAdd.piSubTeam_No = ugrdIdentifier.Rows(0).Cells("SubTeam_No").Value

        fItemIdentifierAdd.ShowDialog()
        fItemIdentifierAdd.Close()
        fItemIdentifierAdd.Dispose()

        LoadDataTable()
        RefreshCheckBox()
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click
        Dim temp1, temp2 As String
        '-- Make sure one item was selected
        If ugrdIdentifier.Selected.Rows.Count = 1 Then
            If ugrdIdentifier.Selected.Rows(0).Cells("Default_Identifier").Value = "X" Then

                MsgBox(ResourcesItemHosting.GetString("RemoveDefaultIdentifier"), MsgBoxStyle.Exclamation, Me.Text)

            Else
                temp2 = ugrdIdentifier.Selected.Rows(0).Cells("Identifier").Value
                temp1 = String.Format(ResourcesItemHosting.GetString("DeleteIdentifier"), temp2)
                If MsgBox(temp1, MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                    'If MsgBox(String.Format(ResourcesItemHosting.GetString("DeleteIdentifier"), gridIdentifier.Columns(1).Value), MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                    SQLExecute("EXEC DeleteItemIdentifier " & ugrdIdentifier.Selected.Rows(0).Cells("Identifier_ID").Value, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    LoadDataTable()
                    RefreshCheckBox()
                End If
            End If
        Else
            MsgBox(ResourcesItemHosting.GetString("SelectItem"), MsgBoxStyle.Exclamation, Me.Text)
        End If

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        Me.Close()

    End Sub

    Private Sub cmdApplyCheckBox_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdApplyCheckBox.Click

        Dim sParam As New StringBuilder
        Dim currentIdentifier As String
        Dim skipSave As Boolean
        Dim iUserResponse As Integer
        Dim pluDigits As Integer
        Dim bIsIdentifierOK As Boolean = False

        '-- Make sure one item was selected
        If ugrdIdentifier.Selected.Rows.Count = 1 Then

            currentIdentifier = ugrdIdentifier.Selected.Rows(0).Cells("Identifier").Value
            If Me.RadioButton_SendToScale_Yes.Checked Then
                If Me.RadioButton_NumScaleDigits_4.Checked Then
                    pluDigits = 4
                End If
                If Me.RadioButton_NumScaleDigits_5.Checked Then
                    pluDigits = 5
                End If
            End If

            ' TFS 6744: Ask user to confirm to add a non-type-2 item.
            If (Not (currentIdentifier.Substring(0, 1) = "2")) And _
                RadioButton_SendToScale_Yes.Checked Then
                iUserResponse = MessageBox.Show("Are you sure you want to send this identifier to the scales?", "IRMA", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            Else
                iUserResponse = 6
            End If

            If iUserResponse = 6 Then ' User confirms to update the identifier

                'verify that there are no ScalePLU conflicts with the pending changes
                'only changes to scale items are affected; including changing: do not send to scale, # scale digits

                Do Until bIsIdentifierOK
                    If ugrdIdentifier.Selected.Rows(0).Cells("IsScaleIdentifier").Value Then

                        '--VALIDATION: If the user has selected 4 digits and item is going to the scale, IRMA performs an additional validation 
                        '--first on the scale department selected, then on the four PLU digits (2xXXXX00000)

                        If Me.GroupBox_NumScaleDigits.Enabled AndAlso (Me.RadioButton_NumScaleDigits_4.Checked Or Me.RadioButton_NumScaleDigits_5.Checked) AndAlso _
                            (Me.RadioButton_SendToScale_No.Visible = False Or (Me.RadioButton_SendToScale_No.Visible AndAlso Not Me.RadioButton_SendToScale_No.Checked)) Then

                            'look for any potential conflicts
                            Dim pluConflicts As ArrayList = ScaleDAO.GetScalePluItemConflicts(currentIdentifier, pluDigits, ugrdIdentifier.Selected.Rows(0).Cells("SubTeam_No").Value)

                            If pluConflicts.Count > 0 Then
                                'display conflict error message form
                                Dim errorForm As New ItemAdd_ScalePluConflict
                                errorForm.IsShowOption_DoNotSendToScale = isShowOption_DoNotSendToScale
                                errorForm.PluConflicts = pluConflicts
                                errorForm.IdentifierListAlternateCancelMsg = True
                                If pluDigits = 5 Then
                                    errorForm.RadioButton_Send5Digits.Enabled = False
                                End If
                                errorForm.ShowDialog()

                                Dim conflictOption As Integer = errorForm.SelectedOption

                                errorForm.Dispose()

                                'handle conflict options
                                '(1) Cancel changes to this Identifier  
                                '(2) Keep this PLU but do not send it to the scales 
                                '(3) Send 5 digits for this PLU  
                                Select Case conflictOption
                                    Case 1
                                        'do not save changes  
                                        skipSave = True
                                        bIsIdentifierOK = True
                                        Exit Do
                                    Case 2
                                        Me.RadioButton_SendToScale_No.Checked = True
                                        bIsIdentifierOK = True
                                        Exit Do
                                    Case 3
                                        Me.RadioButton_NumScaleDigits_5.Checked = True
                                End Select
                            Else
                                bIsIdentifierOK = True
                                Exit Do
                            End If
                        Else
                            bIsIdentifierOK = True
                            Exit Do
                        End If
                    Else
                        Dim pluConflicts As ArrayList = ScaleDAO.GetScalePluItemConflicts(currentIdentifier, pluDigits, ugrdIdentifier.Selected.Rows(0).Cells("SubTeam_No").Value)

                        If pluConflicts.Count > 0 Then


                            'display conflict error message form
                            Dim errorForm As New ItemAdd_ScalePluConflict
                            errorForm.IsShowOption_DoNotSendToScale = isShowOption_DoNotSendToScale
                            errorForm.PluConflicts = pluConflicts
                            If pluDigits = 5 Then
                                errorForm.RadioButton_Send5Digits.Enabled = False
                            End If
                            errorForm.ShowDialog()

                            Dim conflictOption As Integer = errorForm.SelectedOption

                            errorForm.Dispose()

                            Select Case conflictOption
                                Case 1
                                    'exit form and return to ItemIdentifierAdd screen
                                    skipSave = True
                                    'keepItemIdentiferAddOpen = True
                                    bIsIdentifierOK = True
                                    Exit Do
                                Case 2
                                    Me.RadioButton_SendToScale_No.Checked = True
                                    bIsIdentifierOK = True
                                    Exit Do
                                Case 3
                                    Me.RadioButton_NumScaleDigits_4.Checked = False
                                    Me.RadioButton_NumScaleDigits_5.Checked = True
                                    'iNumScaleDigits = 5
                            End Select
                        Else
                            bIsIdentifierOK = True
                            Exit Do
                        End If
                    End If
                Loop

                If Not skipSave Then
                    'build SQL parameter string
                    sParam.Append(glItemID)
                    sParam.Append(", ")
                    sParam.Append(ugrdIdentifier.Selected.Rows(0).Cells("Identifier_ID").Value)

                    'if the default ID check box is checked and the grid does not reflect that, check if the ID is a deleted ID
                    If Me.chkDefaultID.CheckState = CheckState.Checked And ugrdIdentifier.Selected.Rows(0).Cells("Default_Identifier").Value = "" Then
                        If ugrdIdentifier.Selected.Rows(0).Cells("Remove_Identifier").Value = "X" Then
                            MsgBox(ResourcesItemHosting.GetString("DeletedIdentifierNotDefault"), MsgBoxStyle.Exclamation, Me.Text)
                            Exit Sub
                        Else
                            sParam.Append(", ")
                            sParam.Append(IIf(Me.chkDefaultID.CheckState = System.Windows.Forms.CheckState.Checked, 1, 0))
                        End If
                    Else
                        sParam.Append(", 0")
                    End If

                    'if the National ID check box is checked and the grid does not reflect that, check if the ID is a deleted ID
                    If Me.chkNatID.CheckState = CheckState.Checked Then
                        If ugrdIdentifier.Selected.Rows(0).Cells("Remove_Identifier").Value = "X" Then
                            MsgBox(ResourcesItemHosting.GetString("DeletedIdentifierNotNationalID"), MsgBoxStyle.Exclamation, Me.Text)
                            Exit Sub
                        Else
                            sParam.Append(", 1")
                        End If
                    Else
                        sParam.Append(", 0")
                    End If

                    'send "# Scale Digits" param if user can/has entered a value
                    If Me.GroupBox_NumScaleDigits.Enabled AndAlso _
                        (Me.RadioButton_NumScaleDigits_4.Checked Or Me.RadioButton_NumScaleDigits_5.Checked) Then
                        Select Case True
                            Case Me.RadioButton_NumScaleDigits_4.Checked
                                sParam.Append(", 4")
                            Case Me.RadioButton_NumScaleDigits_5.Checked
                                sParam.Append(", 5")
                        End Select
                    Else
                        sParam.Append(", NULL")
                    End If

                    'send "Send To Scale" param if user can/has entered a value
                    If Me.GroupBox_SendToScale.Enabled AndAlso _
                        (Me.RadioButton_SendToScale_Yes.Checked Or Me.RadioButton_SendToScale_No.Checked) Then
                        Select Case True
                            Case Me.RadioButton_SendToScale_Yes.Checked
                                sParam.Append(", 1")
                            Case Me.RadioButton_SendToScale_No.Checked
                                sParam.Append(", 0")
                        End Select
                    Else
                        If ugrdIdentifier.Selected.Rows(0).Cells("IsScaleIdentifier").Value Then
                            sParam.Append(", 1")
                        Else
                            sParam.Append(", 0")
                        End If
                    End If

                    SQLExecute("EXEC UpdateItemIdentifier " & sParam.ToString, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                End If

                Call LoadDataTable()
                Call RefreshCheckBox()
            End If
        Else
        MsgBox(ResourcesItemHosting.GetString("SelectItem"), MsgBoxStyle.Exclamation, Me.Text)
        End If

    End Sub

    Private Sub frmItemIdentifierList_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Dim rs As DAO.Recordset = Nothing

        '-- Center the form
        CenterForm(Me)

        SetActive(cmdAdd, (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser)
        SetActive(cmdDelete, (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser)
        SetActive(cmdApplyCheckBox, (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser)
        'SetActive(CheckBox_DoNotSendToScale, (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser)
        SetActive(RadioButton_SendToScale_Yes, (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser)
        SetActive(RadioButton_SendToScale_No, (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser)
        SetActive(chkNatID, (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser)
        SetActive(RadioButton_NumScaleDigits_4, (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser)
        SetActive(RadioButton_NumScaleDigits_5, (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser)

        SetupDataTable()
        LoadDataTable()

        Try
            rs = SQLOpenRecordSet("EXEC CheckIfWarehouseItem " & glItemID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If rs.Fields(0).Value Then
                m_bIsEXEDistributed = True
            Else
                m_bIsEXEDistributed = False
            End If
        Finally
            If rs IsNot Nothing Then
                rs.Close()
                rs = Nothing
            End If
        End Try

        RefreshCheckBox()

    End Sub
    Private Sub LoadDataTable()

        Dim rs As DAO.Recordset = Nothing
        Dim row As DataRow

        mdt.Clear()

        Try
            '-- Set up the databound stuff
            rs = SQLOpenRecordSet("EXEC GetItemIdentifiers " & glItemID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            While Not rs.EOF
                row = mdt.NewRow
                row("Identifier") = rs.Fields("Identifier").Value
                If rs.Fields("Default_Identifier").Value Then
                    row("Default_Identifier") = "X"
                Else
                    row("Default_Identifier") = ""
                End If
                If rs.Fields("Add_Identifier").Value Then
                    row("Add_Identifier") = "X"
                Else
                    row("Add_Identifier") = ""
                End If
                If rs.Fields("Remove_Identifier").Value Then
                    row("Remove_Identifier") = "X"
                Else
                    row("Remove_Identifier") = ""
                End If
                If rs.Fields("National_Identifier").Value Then
                    row("National_Identifier") = "X"
                Else
                    row("National_Identifier") = ""
                End If

                row("Identifier_ID") = rs.Fields("Identifier_ID").Value
                row("IsScaleIdentifier") = rs.Fields("IsScaleIdentifier").Value
                row("SubTeam_No") = rs.Fields("SubTeam_No").Value

                'display "N/A" for non-scale identifiers
                If Not rs.Fields("IsScaleIdentifier").Value Then
                    If rs.Fields("Scale_Identifier").Value And _
                        rs.Fields("NumPluDigitsSentToScale").Value IsNot DBNull.Value Then
                        row("NumPluDigitsSentToScale") = rs.Fields("NumPluDigitsSentToScale").Value.ToString
                    Else
                        row("NumPluDigitsSentToScale") = "N/A"
                    End If
                ElseIf rs.Fields("NumPluDigitsSentToScale").Value IsNot DBNull.Value Then
                    row("NumPluDigitsSentToScale") = rs.Fields("NumPluDigitsSentToScale").Value.ToString
                End If

                'display "N/A" for non-scale identifiers
                'if Scale_Identifier = 1 then display 'X', else display nothing for "Send to Scale" (Allows for Non type 2 scale Identifier)
                If rs.Fields("Scale_Identifier").Value Then
                    row("Scale_Identifier") = "X"
                Else
                    row("Scale_Identifier") = "N/A"
                End If


                row("IdentifierType") = rs.Fields("IdentifierType").Value

                mdt.Rows.Add(row)
                rs.MoveNext()
            End While

        Finally
            If rs IsNot Nothing Then
                rs.Close()
                rs = Nothing
            End If
        End Try

        mdt.AcceptChanges()
        ugrdIdentifier.DataSource = mdt

        If ugrdIdentifier.Rows.Count > 0 Then
            ugrdIdentifier.Rows(0).Selected = True
        End If

    End Sub

    Private Sub RefreshCheckBox()
        'check the Default and National boxes based on the selected row
        Me.chkDefaultID.CheckState = IIf(ugrdIdentifier.Selected.Rows(0).Cells("Default_Identifier").Value = "X", System.Windows.Forms.CheckState.Checked, System.Windows.Forms.CheckState.Unchecked)
        Me.chkNatID.CheckState = IIf(ugrdIdentifier.Selected.Rows(0).Cells("National_Identifier").Value = "X", System.Windows.Forms.CheckState.Checked, System.Windows.Forms.CheckState.Unchecked)

        'is the region sending item level values for # of Scale Digits AND 
        'is the selected row a scale identifier?

        If gsPluDigitsSentToScale.Equals("VARIABLE BY ITEM") AndAlso (ugrdIdentifier.Selected.Rows(0).Cells("IsScaleIdentifier").Value Or ugrdIdentifier.Selected.Rows(0).Cells("Scale_Identifier").Value = "X") Then ' "IsScaleIdentifier"
            'display the "# Scale Digits" box and check the appropriate box

            Me.GroupBox_NumScaleDigits.Enabled = True

            If ugrdIdentifier.Selected.Rows(0).Cells("NumPluDigitsSentToScale").Value Is DBNull.Value Then
                Me.RadioButton_NumScaleDigits_4.Checked = False
                Me.RadioButton_NumScaleDigits_5.Checked = False
            ElseIf ugrdIdentifier.Selected.Rows(0).Cells("NumPluDigitsSentToScale").Value = "4" Then
                Me.RadioButton_NumScaleDigits_4.Checked = True
                Me.RadioButton_NumScaleDigits_5.Checked = False
            ElseIf ugrdIdentifier.Selected.Rows(0).Cells("NumPluDigitsSentToScale").Value = "5" Then
                Me.RadioButton_NumScaleDigits_4.Checked = False
                Me.RadioButton_NumScaleDigits_5.Checked = True
            End If
        Else

            'hide the "# Scale Digits" box 
            If ugrdIdentifier.Selected.Rows(0).Cells("Scale_Identifier").Value = "X" Then
                Me.GroupBox_NumScaleDigits.Enabled = True
            Else
                Me.GroupBox_NumScaleDigits.Enabled = False
            End If
            Me.RadioButton_NumScaleDigits_4.Checked = False
            Me.RadioButton_NumScaleDigits_5.Checked = False

        End If

        'show "Send To Scale" radio button group only if regional flag = TRUE

        isShowOption_DoNotSendToScale = InstanceDataDAO.IsFlagActive("ShowOption_DoNotSendToScale")
        If Not isShowOption_DoNotSendToScale Then
            Me.RadioButton_SendToScale_No.Enabled = False
        Else
            If (ugrdIdentifier.Selected.Rows(0).Cells("IsScaleIdentifier").Value And _
                ugrdIdentifier.Selected.Rows(0).Cells("Scale_Identifier").Value = "X") Or _
                ugrdIdentifier.Selected.Rows(0).Cells("Scale_Identifier").Value = "X" Then
                '''''display the "Send To Scale" radio button group and check if needed
                SetActive(RadioButton_SendToScale_Yes, (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser)
                SetActive(RadioButton_SendToScale_No, (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser)
                Me.RadioButton_SendToScale_Yes.Checked = True
            Else
                '''''Default to "Send To Scale - No" radio button
                Me.RadioButton_SendToScale_No.Checked = True
                Me.RadioButton_SendToScale_Yes.Checked = False
            End If
        End If


        'Me.chkDefaultID.Enabled = Not (Me.chkDefaultID.CheckState = System.Windows.Forms.CheckState.Checked) And Not m_bIsEXEDistributed

    End Sub

    Private Sub ugrdIdentifier_AfterCellActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdIdentifier.AfterCellActivate
        ugrdIdentifier.ActiveRow.Selected = True
        RefreshCheckBox()
    End Sub


    Private Sub SetupDataTable()
        ' Create a data table
        mdt = New DataTable("ItemIdentifier")

        'Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("Identifier", GetType(String)))
        mdt.Columns.Add(New DataColumn("Default_Identifier", GetType(String)))
        mdt.Columns.Add(New DataColumn("Add_Identifier", GetType(String)))
        mdt.Columns.Add(New DataColumn("Remove_Identifier", GetType(String)))
        mdt.Columns.Add(New DataColumn("National_Identifier", GetType(String)))
        mdt.Columns.Add(New DataColumn("NumPluDigitsSentToScale", GetType(String)))
        mdt.Columns.Add(New DataColumn("IsScaleIdentifier", GetType(Boolean)))
        mdt.Columns.Add(New DataColumn("Scale_Identifier", GetType(String))) 'Do Not Send to Scale column based on Scale_Identifier bit field
        mdt.Columns.Add(New DataColumn("SubTeam_No", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("IdentifierType", GetType(String)))

        'Hidden.
        '--------------------
        mdt.Columns.Add(New DataColumn("Identifier_ID", GetType(Integer)))

    End Sub

    Private Sub RadioButton_SendToScale_Yes_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_SendToScale_Yes.CheckedChanged
        If Me.RadioButton_SendToScale_Yes.Checked = True Then
            Me.GroupBox_NumScaleDigits.Enabled = True
        Else
            Me.GroupBox_NumScaleDigits.Enabled = False
            Me.RadioButton_NumScaleDigits_4.Checked = False
            Me.RadioButton_NumScaleDigits_5.Checked = False
        End If
    End Sub

    Private Sub RadioButton_SendToScale_No_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_SendToScale_No.CheckedChanged
        If Me.RadioButton_SendToScale_No.Checked = True Then
            Me.GroupBox_NumScaleDigits.Enabled = False
            Me.RadioButton_NumScaleDigits_4.Checked = False
            Me.RadioButton_NumScaleDigits_5.Checked = False
        Else
            Me.GroupBox_NumScaleDigits.Enabled = True
        End If
    End Sub

    Private Sub ugrdIdentifier_AfterRowActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdIdentifier.AfterRowActivate
        ugrdIdentifier.ActiveRow.Selected = True
        RefreshCheckBox()
    End Sub
End Class

