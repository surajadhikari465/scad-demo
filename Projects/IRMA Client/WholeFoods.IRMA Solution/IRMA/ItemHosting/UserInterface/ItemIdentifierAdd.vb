Option Strict Off
Option Explicit On

Imports VB = Microsoft.VisualBasic
Imports WholeFoods.IRMA.Common.BusinessLogic    ' for Instance data flags
Imports WholeFoods.IRMA.Common.DataAccess       ' for Instance data flags
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Friend Class frmItemIdentifierAdd
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean
    Public pbAddToDatabase As Boolean 'Set by the caller to determine if this window gathers and validates only or actually saves as well.  This window is used for new Item add as well as existing Item maintenance is the reason for this flag.
    Public piSubTeam_No As Integer 'set by caller if this is an existing item with a subteam tied to it already

    Private Sub frmItemIdentifierAdd_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        CenterForm(Me)

        SetActive(txtIdentifier, False)
        SetActive(txtCheckDigit, False)

        If Not InstanceDataDAO.IsFlagActive("ValidateCheckDigit") Then

            Me.txtCheckDigit.Visible = False
            Me.lblCheckDigit.Visible = False

        End If
    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click

        Dim iUserResponse As Integer
        Dim bIdentifierOK As Boolean = False

        ' TFS 6744: Ask user to confirm to add a non-type-2 item.

        If (Me.txtIdentifier.Text.Substring(0, 1) <> "2") And Me.RadioButton_SendToScale_Yes.Checked Then

            iUserResponse = MessageBox.Show("Are you sure you want to send this identifier to the scales?", "IRMA", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        Else
            iUserResponse = 6
        End If

        If iUserResponse = 6 Then ' User confirms to add the identifier 
            Dim rs As DAO.Recordset = Nothing
            Dim iNationalId As Int16 = 0
            Dim keepFormOpen As Boolean
            Dim sIdentifierType As String
            Dim numPluDigitsSentToScale As String = "NULL"
            Dim isScaleIdentifier As String = "0"
            Dim skipSave As Boolean
            Dim reNumber As New System.Text.RegularExpressions.Regex("^[0-9]+$")
            Dim isRetailSaleItem As Boolean

            sIdentifierType = String.Empty


            '-- Validate the Identifier Code --------------------------------------------------------'
            txtIdentifier.Text = Trim(txtIdentifier.Text)

            If Len(txtIdentifier.Text) = 0 Then
                MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblIdentifier.Text.Replace(":", "")), MsgBoxStyle.Critical, Me.Text)
                txtIdentifier.Focus()
                Exit Sub
            End If

            '-- check for numbers only bug #8141 was using IsNumeric insted of RegEx
            If Not reNumber.IsMatch(txtIdentifier.Text) Then
                MsgBox(ResourcesItemHosting.GetString("IdentifierNumeric"), MsgBoxStyle.Critical, Me.Text)
                txtIdentifier.Focus()
                Exit Sub
            End If

            '-- Get rid of leading zeros
            If VB.Left(txtIdentifier.Text, 1) = "0" Then
                MsgBox(String.Format(ResourcesItemHosting.GetString("IdentifierZero"), Chr(13)), MsgBoxStyle.Critical, Me.Text)
                txtIdentifier.Focus()
                Exit Sub
            End If

            '-- Check to see if a non-retail ingredient identifier is to be added to a retail-sale item
            Try
                rs = SQLOpenRecordSet("SELECT dbo.fn_IsRetailSaleItem(" & glItemID.ToString() & ")", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                isRetailSaleItem = rs.Fields(0).Value
            Finally
                If rs IsNot Nothing Then
                    rs.Close()
                    rs = Nothing
                End If
            End Try

            'If the identifier falls into the specified range, it's a non-retail ingredient item, so it can't be assigned to a retail sale item as an alternate identifier
            Dim sIdentifier As String = txtIdentifier.Text
            If (isRetailSaleItem = True And ((sIdentifier >= 46000000000 And sIdentifier <= 46999999999) Or (sIdentifier >= 48000000000 And sIdentifier <= 48999999999))) Then
                MsgBox(ResourcesItemHosting.GetString("NonRetailIngredientItem"), MsgBoxStyle.Critical, Me.Text)
                txtIdentifier.Focus()
                Exit Sub
            End If

            '-- Check Digit for barcodes
            If Me.lblCheckDigit.Visible = True Then
                Dim sCalcCheckDigit As String
                If optType(0).Checked Then
                    If Len(txtCheckDigit.Text) <> 1 Then
                        MsgBox(ResourcesItemHosting.GetString("CheckDigitRequired"), MsgBoxStyle.Critical, Me.Text)
                        txtCheckDigit.Focus()
                        Exit Sub
                    End If

                    Try
                        rs = SQLOpenRecordSet("SELECT dbo.fn_CalcBarcodeCheckDigit('" & txtIdentifier.Text & "')", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                        sCalcCheckDigit = rs.Fields(0).Value & ""
                    Finally
                        If rs IsNot Nothing Then
                            rs.Close()
                            rs = Nothing
                        End If
                    End Try

                    If sCalcCheckDigit <> txtCheckDigit.Text Then
                        MsgBox(ResourcesItemHosting.GetString("CalculatedCheckDigit"), MsgBoxStyle.Critical, Me.Text)
                        txtIdentifier.Focus()
                        Exit Sub
                    End If
                End If
            End If

            Try
                '-- Check to see if the name already exists
                rs = SQLOpenRecordSet("EXEC CheckForDuplicateIdentifier '" & txtIdentifier.Text & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                If rs.Fields("IdentifierCount").Value > 0 Then
                    rs.Close()
                    rs = Nothing
                    MsgBox(ResourcesItemHosting.GetString("IdentifierExists"), MsgBoxStyle.Critical, Me.Text)
                    txtIdentifier.Focus()
                    Exit Sub
                End If
            Finally
                If rs IsNot Nothing Then
                    rs.Close()
                    rs = Nothing
                End If
            End Try

            Select Case True
                Case optType(0).Checked : sIdentifierType = "B"
                Case optType(1).Checked : sIdentifierType = "P"
                Case optType(2).Checked : sIdentifierType = "S"
                Case optType(3).Checked : sIdentifierType = "O"
            End Select

            'is National identifier?
            If Me.CheckBox_National.Checked Then
                iNationalId = 1
            End If

            'is Scale Item (Applies for Non Type 2 Identifiers)
            If Me.RadioButton_SendToScale_Yes.Checked Then
                isScaleIdentifier = "1"
            End If

            If Me.RadioButton_SendToScale_Yes.Checked Then
                If Me.RadioButton_NumScaleDigits_4.Checked Then
                    numPluDigitsSentToScale = "4"
                End If
                If Me.RadioButton_NumScaleDigits_5.Checked Then
                    numPluDigitsSentToScale = "5"
                End If
            End If

            'validate scale info ONLY if this is adding an identifier to an existing item; 
            'NOT for new item creation because validation takes place on the ItemAdd.vb form
            If Me.pbAddToDatabase Then
                'SCALE ITEMS ONLY -- validate that new identifier will not conflict with existing ScalePLU values
                Do Until bIdentifierOK
                    If sIdentifierType.Equals("O") AndAlso ScaleDAO.IsScaleIdentifier(txtIdentifier.Text) AndAlso isScaleIdentifier <> "1" Then
                        'default values for new Type 2 scale items (pending validation below)
                        isScaleIdentifier = "1"

                        'if regional option is set to always send 5 digit PLUs set to 5, otherwise default to 4
                        'If gsPluDigitsSentToScale.Equals("ALWAYS 5") Then
                        '    numPluDigitsSentToScale = "5"
                        'Else
                        '    numPluDigitsSentToScale = "4"

                        'VALIDATE that 4 or 5 digit ScalePLU will not conflict with existing items



                        Dim pluConflicts As ArrayList = ScaleDAO.GetScalePluItemConflicts(txtIdentifier.Text, numPluDigitsSentToScale, piSubTeam_No)

                        If pluConflicts.Count > 0 Then
                            'display conflict error message form
                            Dim errorForm As New ItemAdd_ScalePluConflict
                            errorForm.IsShowOption_DoNotSendToScale = InstanceDataDAO.IsFlagActive("ShowOption_DoNotSendToScale")
                            errorForm.PluConflicts = pluConflicts
                            If numPluDigitsSentToScale = 5 Then
                                errorForm.RadioButton_Send5Digits.Enabled = False
                            End If
                            errorForm.ShowDialog()

                            Dim conflictOption As Integer = errorForm.SelectedOption

                            errorForm.Dispose()

                            'handle conflict options
                            '(1) Cancel and enter a new PLU  
                            '(2) Keep this PLU but do not send it to the scales 
                            '(3) Send 5 digits for this PLU  
                            Select Case conflictOption
                                Case 1
                                    'exit form and return to ItemIdentifierAdd screen
                                    skipSave = True
                                    keepFormOpen = True
                                    bIdentifierOK = True
                                    Exit Do
                                Case 2
                                    'MessageBox.Show("IsItemIdentifier=0")
                                    isScaleIdentifier = "0"
                                    bIdentifierOK = True
                                    Exit Do
                                Case 3
                                    'MessageBox.Show("numPluDigitsSentToScale=5")
                                    numPluDigitsSentToScale = "5"
                            End Select
                        Else
                            bIdentifierOK = True
                            Exit Do
                        End If
                        'End If
                    Else
                        If isScaleIdentifier = 1 Then

                            Dim pluConflicts As ArrayList = ScaleDAO.GetScalePluItemConflicts(txtIdentifier.Text, numPluDigitsSentToScale, piSubTeam_No)

                            If pluConflicts.Count > 0 Then
                                ''display conflict error message

                                Dim errorForm As New ItemAdd_ScalePluConflict
                                errorForm.IsShowOption_DoNotSendToScale = InstanceDataDAO.IsFlagActive("ShowOption_DoNotSendToScale")
                                errorForm.PluConflicts = pluConflicts
                                If numPluDigitsSentToScale = 5 Then
                                    errorForm.RadioButton_Send5Digits.Enabled = False
                                End If
                                errorForm.ShowDialog()

                                Dim conflictOption As Integer = errorForm.SelectedOption

                                errorForm.Dispose()

                                'handle conflict options
                                '(1) Cancel and enter a new PLU  
                                '(2) Keep this PLU but do not send it to the scales 
                                '(3) Send 5 digits for this PLU  
                                Select Case conflictOption
                                    Case 1
                                        'exit form and return to ItemIdentifierAdd screen
                                        skipSave = True
                                        keepFormOpen = True
                                        bIdentifierOK = True
                                        Exit Do
                                    Case 2
                                        'MessageBox.Show("IsItemIdentifier=0")
                                        isScaleIdentifier = "0"
                                        bIdentifierOK = True
                                        Exit Do
                                    Case 3
                                        'MessageBox.Show("numPluDigitsSentToScale=5")
                                        numPluDigitsSentToScale = "5"
                                End Select
                            Else
                                bIdentifierOK = True
                                Exit Do
                            End If
                        Else
                            bIdentifierOK = True
                            Exit Do
                        End If
                    End If
                Loop
            End If

            If Not skipSave Then
                If Me.pbAddToDatabase Then
                    '-- Add the new record
                    SQLExecute("EXEC InsertItemIdentifier " & glItemID & ", '" & sIdentifierType & "','" & txtIdentifier.Text & "','" & txtCheckDigit.Text & "'," & iNationalId & "," & numPluDigitsSentToScale & "," & isScaleIdentifier, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                Else
                    '-- Call the adding form
                    Dim fItemAdd As New frmItemAdd
                    fItemAdd.psIdentifierType = sIdentifierType
                    fItemAdd.psIdentifier = txtIdentifier.Text
                    fItemAdd.psCheckDigit = txtCheckDigit.Text
                    fItemAdd.piNationalID = iNationalId
                    If isScaleIdentifier = "1" Then
                        fItemAdd.isScaleIdentifier = True
                        fItemAdd.RadioButton_SendToScale_Yes.Checked = True
                        If numPluDigitsSentToScale = "4" Then
                            fItemAdd.RadioButton_NumScaleDigits_4.Checked = True
                        End If
                        If numPluDigitsSentToScale = "5" Then
                            fItemAdd.RadioButton_NumScaleDigits_5.Checked = True
                        End If
                    Else
                        If Me.RadioButton_SendToScale_Yes.Checked Then
                            fItemAdd.isScaleIdentifier = True
                            If numPluDigitsSentToScale = "4" Then
                                fItemAdd.RadioButton_NumScaleDigits_4.Checked = True
                            End If
                            If numPluDigitsSentToScale = "5" Then
                                fItemAdd.RadioButton_NumScaleDigits_5.Checked = True
                            End If
                        Else
                            fItemAdd.isScaleIdentifier = False
                            fItemAdd.RadioButton_SendToScale_No.Checked = True
                            fItemAdd.RadioButton_NumScaleDigits_4.Checked = False
                            fItemAdd.RadioButton_NumScaleDigits_5.Checked = False
                        End If
                        
                    End If

                    fItemAdd.ShowDialog()

                    keepFormOpen = fItemAdd.keepItemIdentiferAddOpen

                    fItemAdd.Close()
                    fItemAdd.Dispose()
                End If
            Else
               
            End If

            If Not keepFormOpen Then
                Me.Close()
            Else
                'empty identifier entry box
                txtIdentifier.Text = ""
                Me.RadioButton_SendToScale_No.Checked = True
                Me.GroupBox_NumScaleDigits.Enabled = False
            End If
        End If
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        Me.Close()

    End Sub

    Private Sub optType_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optType.CheckedChanged
        If Me.IsInitializing Then Exit Sub
        If eventSender.Checked Then
            Dim Index As Short = optType.GetIndex(eventSender)

            SetActive(txtIdentifier, True)

            If Me.lblCheckDigit.Visible = True Then
                If Index = 0 Then
                    SetActive(txtCheckDigit, True)
                Else
                    SetActive(txtCheckDigit, False)
                End If
            End If
            txtIdentifier.Focus()
        End If
    End Sub

    Private Sub txtCheckDigit_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtCheckDigit.Enter

        HighlightText(txtCheckDigit)

    End Sub


    Private Sub txtCheckDigit_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtCheckDigit.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtCheckDigit.Tag), txtCheckDigit, 0, 0, 0)

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub txtIdentifier_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtIdentifier.Enter

        HighlightText(txtIdentifier)

    End Sub

    Private Sub txtIdentifier_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtIdentifier.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, (txtIdentifier.Tag), txtIdentifier, 0, 0, 0)

        If Chr(KeyAscii) = "0" And Len(Trim(txtIdentifier.Text)) = 0 Then KeyAscii = 0

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub RadioButton_SendToScale_Yes_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_SendToScale_Yes.CheckedChanged
        ' TFS 6744: Add a warning message for the non-type-2 identifiers.
        If Me.RadioButton_SendToScale_Yes.Checked = True Then
            Me.GroupBox_NumScaleDigits.Enabled = True
            If Me.txtIdentifier.Text <> "" Then
                If Me.txtIdentifier.Text.Substring(0, 1) = "2" Then
                    If Trim(Me.txtIdentifier.Text).Length = 11 Then
                        If Me.txtIdentifier.Text.Substring(Trim(Me.txtIdentifier.Text).Length - 5, 5) = "00000" Then
                            Me.lblNonType2Message.Text = ""
                        Else
                            Me.lblNonType2Message.Text = "This is a non type-2 scale item."
                        End If
                    Else
                        Me.lblNonType2Message.Text = "This is a non type-2 scale item."
                    End If
                Else
                    Me.lblNonType2Message.Text = "This is a non type-2 scale item."
                End If
            End If
        Else
            Me.lblNonType2Message.Text = ""
            Me.GroupBox_NumScaleDigits.Enabled = False
        End If
    End Sub

    Private Sub txtIdentifier_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtIdentifier.LostFocus
        Me.txtIdentifier.Text = Trim(Me.txtIdentifier.Text)
        If Me.txtIdentifier.Text <> "" Then
            If Me.txtIdentifier.Text.Substring(0, 1) = "2" Then
                If Trim(Me.txtIdentifier.Text).Length = 11 Then
                    If Me.txtIdentifier.Text.Substring(Trim(Me.txtIdentifier.Text).Length - 5, 5) = "00000" Then
                        Me.RadioButton_SendToScale_Yes.Checked = True
                    Else
                        Me.RadioButton_SendToScale_No.Checked = True
                    End If
                Else
                    Me.RadioButton_SendToScale_No.Checked = True
                End If
            Else
                Me.RadioButton_SendToScale_No.Checked = True
            End If
        End If
    End Sub
End Class
