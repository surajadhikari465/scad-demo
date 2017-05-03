Imports System.Windows.Forms
Imports System.Linq
Imports System.Data
Imports System.IO
Imports System.ServiceModel

Public Class ReceiveDocumentMain

    Private mySession As Session
    Private serviceFault As ParsedCFFaultException = Nothing
    Private serviceCallSuccess As Boolean

    Public Sub New(ByVal session As Session)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.mySession = session
        AlignText()

    End Sub

    Private Sub ReceiveDocumentMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            ' Initialize the form controls.  All service calls needed for this form are included in this block.
            If Me.mySession.Subteam = String.Empty Then
                MsgBox("Please select a subteam.", MsgBoxStyle.Information, Me.Text)
                Me.Close()
                Exit Sub
            ElseIf CheckSubteamExistWithStore() <> True Then
                MsgBox("This subteam does not exist for this store.", MsgBoxStyle.Information, Me.Text)
                Me.Close()
                Exit Sub
            End If

            LabelSelectedStore.Text = Me.mySession.StoreName
            LabelSelectedSubteam.Text = Me.mySession.Subteam
            invoiceNum.Text = String.Empty

            GetVendorList()
            DisplayReturnCombo()

            ' Explicitly handle service faults, timeouts, and connection failures.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            Me.DialogResult = Windows.Forms.DialogResult.Cancel

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "ReceiveDocumentMain_Load")
            Me.DialogResult = Windows.Forms.DialogResult.Cancel

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "ReceiveDocumentMain_Load")
            Me.DialogResult = Windows.Forms.DialogResult.Cancel

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "ReceiveDocumentMain_Load")
            Me.DialogResult = Windows.Forms.DialogResult.Cancel

        Finally
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    Private Sub GetVendorList()
        Dim vendorListResult As DSDVendor() = Me.mySession.WebProxyClient.GetDSDVendors((mySession.StoreNo))
        Dim dtsTypes As DataTable
        Dim dr As DataRow

        dtsTypes = New DataTable
        dtsTypes.Columns.Add(New DataColumn("DisplayMember"))
        dtsTypes.Columns.Add(New DataColumn("ValueMember"))
        dr = dtsTypes.NewRow()
        dr.Item("DisplayMember") = "Select Vendor..."
        dr.Item("ValueMember") = "NN"
        dtsTypes.Rows.Add(dr)
        If vendorListResult.Length > 0 Then
            For Each item As DSDVendor In vendorListResult
                dr = dtsTypes.NewRow()
                dr.Item("DisplayMember") = item.VendorName
                dr.Item("ValueMember") = item.VendorID
                dtsTypes.Rows.Add(dr)
            Next
        Else
            MsgBox("This store does not have any Receiving Document vendors.", MsgBoxStyle.Information, Me.Text)
            Me.Close()
            Exit Sub
        End If
        Me.vendorCombo.DataSource = dtsTypes
        Me.vendorCombo.DisplayMember = "DisplayMember"
        Me.vendorCombo.ValueMember = "ValueMember"
        If dtsTypes.Rows.Count > 1 Then
            Me.vendorCombo.Enabled = True
        Else
            Me.vendorCombo.Enabled = False
        End If
    End Sub

    Private Sub DisplayReturnCombo()
        Dim vendorListResult As DSDVendor() = Me.mySession.WebProxyClient.GetDSDVendors(mySession.StoreNo)
        Dim dtsTypes As DataTable
        Dim dr As DataRow

        dtsTypes = New DataTable
        dtsTypes.Columns.Add(New DataColumn("DisplayMember"))
        dtsTypes.Columns.Add(New DataColumn("ValueMember"))
        dr = dtsTypes.NewRow()
        dr.Item("DisplayMember") = "Select Type..."
        dr.Item("ValueMember") = "NN"
        dtsTypes.Rows.Add(dr)

        dr = dtsTypes.NewRow()
        dr.Item("DisplayMember") = "Invoice"
        dr.Item("ValueMember") = 0
        dtsTypes.Rows.Add(dr)

        dr = dtsTypes.NewRow()
        dr.Item("DisplayMember") = "Credit"
        dr.Item("ValueMember") = 1
        dtsTypes.Rows.Add(dr)

        Me.returnCombo.DataSource = dtsTypes
        Me.returnCombo.DisplayMember = "DisplayMember"
        Me.returnCombo.ValueMember = "ValueMember"
    End Sub

    Private Sub mnuReceiveItems_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReceiveItem.Click

        If Me.vendorCombo.SelectedValue = "NN" Then
            MsgBox("Please select a vendor before creating the Receiving Document.", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        If Me.returnCombo.SelectedValue = "NN" Then
            MsgBox("Please choose invoice or credit before continuing.", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        mySession.DSDInvoice = Me.invoiceNum.Text.Trim()
        mySession.DSDVendorName = Me.vendorCombo.Text
        mySession.DSDVendorID = Me.vendorCombo.SelectedValue

        If Not String.IsNullOrEmpty(mySession.DSDInvoice) Then

            Try
                Cursor.Current = Cursors.WaitCursor

                If Me.mySession.WebProxyClient.IsDuplicateReceivingDocumentInvoiceNumber(mySession.DSDInvoice, mySession.DSDVendorID) Then
                    MessageBox.Show(String.Format("Invoice number {0} has already been used for this vendor.  Please update the invoice number and try again.", mySession.DSDInvoice), _
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
                    Exit Sub
                End If

                ' Explicitly handle service faults, timeouts, and connection failures.  
            Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
                serviceFault = New ParsedCFFaultException(ex.FaultMessage)
                Dim err As New ErrorHandler(serviceFault)
                err.ShowErrorNotification()
                Exit Sub

            Catch ex As TimeoutException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "mnuReceiveItems_Click")
                Exit Sub

            Catch ex As CommunicationException
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "mnuReceiveItems_Click")
                Exit Sub

            Catch ex As Exception
                Dim err As New ErrorHandler()
                err.DisplayErrorMessage(ex.Message, "mnuReceiveItems_Click")
                Exit Sub
            Finally
                Cursor.Current = Cursors.Default
            End Try
            
        End If

        If (Me.returnCombo.SelectedValue = 0) Then
            mySession.ActionType = Enums.ActionType.ReceiveDocument
        Else
            mySession.ActionType = Enums.ActionType.ReceiveDocumentCredit
        End If

        ShowReceiveDocumentScan()
    End Sub

    Private Sub ShowReceiveDocumentScan()
        Cursor.Current = Cursors.WaitCursor

        Dim rdScan As ReceiveDocumentScan = New ReceiveDocumentScan(Me.mySession)
        Dim res As DialogResult = rdScan.ShowDialog()

        If res = Windows.Forms.DialogResult.Abort Then
            Me.DialogResult = Windows.Forms.DialogResult.Abort
        ElseIf res = Windows.Forms.DialogResult.OK Then
            Me.Close()
        End If

        rdScan.Close()
        rdScan.Dispose()

        ClearScreen()
    End Sub

    Private Function SavedSessionExist() As Boolean
        Dim path As String = Environment.GetFolderPath(Environment.SpecialFolder.Personal) & "\ReceiveDocument"
        If Directory.Exists(path) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub MenuItemViewSessions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemViewSessions.Click

        Try
            Dim openFileDialog1 As OpenFileDialog = New OpenFileDialog()

            openFileDialog1.InitialDirectory = "ReceiveDocument"
            openFileDialog1.Filter = "UnSent files (*.txt)|*.txt"

            If SavedSessionExist() = True Then
                Dim rs As DialogResult = openFileDialog1.ShowDialog()
                Dim validChoice As Boolean = False
                Dim selectedSomething As Boolean = False

                If (rs = DialogResult.OK) Then
                    If (String.IsNullOrEmpty(openFileDialog1.FileName) = False) Then

                        'set session name
                        Dim param As Char() = {"\\", "."}
                        Dim myValues As String() = openFileDialog1.FileName.Split(param)

                        For i As Integer = 0 To myValues.Length - 1

                            If (myValues(i).EndsWith("txt")) Then
                                If Not (myValues(3).Contains(mySession.Subteam.Replace("/"c, "_"c))) Then
                                    'not matching the current subteam don't allow it
                                    MsgBox("The selected file's subteam does not match the current session's subteam.  " _
                                           & "Please choose another file or change the current session's subteam.", _
                                           MsgBoxStyle.Information, "Subteam Mismatch")
                                    Exit For
                                End If

                                Cursor.Current = Cursors.WaitCursor

                                Dim newSession As Session = New Session(mySession.ServiceURI)
                                newSession.Region = mySession.Region
                                newSession.MyScanner = Me.mySession.MyScanner

                                Dim fileWriter As ReceiveDocumentFileWriter = New ReceiveDocumentFileWriter(Me.mySession)
                                Me.mySession = fileWriter.GetFileSession(openFileDialog1.FileName, newSession)

                                validChoice = True
                                selectedSomething = True
                                mySession.IsLoadedSession = True
                                Cursor.Current = Cursors.Default
                                Exit For
                            End If
                        Next

                    End If

                    openFileDialog1 = Nothing

                    If (validChoice) Then
                        ShowShrinkScan()
                    ElseIf (Not validChoice And selectedSomething) Then
                        Messages.invalidSavedOrderChoiceException()
                    End If
                End If
            Else
                MsgBox("No saved session exists.", MsgBoxStyle.Information, Me.Text)
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox("Error when trying to edit the saved order: " + ex.Message, MsgBoxStyle.Exclamation, Me.Text)
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    Private Sub ShowShrinkScan()

        Dim scanReceiveDocument As ReceiveDocumentScan = New ReceiveDocumentScan(Me.mySession)
        Dim res As DialogResult = scanReceiveDocument.ShowDialog()

        If res = Windows.Forms.DialogResult.Abort Then
            Me.DialogResult = Windows.Forms.DialogResult.Abort
        ElseIf res = Windows.Forms.DialogResult.OK Then
            Me.Close()
        End If

        scanReceiveDocument.Dispose()

    End Sub

    Private Sub MenuItemNewSession_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemNewSession.Click
        mySession.IsLoadedSession = False
        mySession.StartTime = DateTime.Now
        ClearScreen()
        MessageBox.Show("A new session has been loaded.", "New Session", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
    End Sub

    Private Sub MenuItemClearSession_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemClearSession.Click
        Cursor.Current = Cursors.WaitCursor

        Try
            'delete session
            Dim resp As MsgBoxResult = Messages.DeleteSession()

            If (resp = MsgBoxResult.Yes) Then
                Dim fileWriter As ReceiveDocumentFileWriter = New ReceiveDocumentFileWriter(Me.mySession)

                fileWriter.DeleteFile(fileWriter.MakeFilePath(Me.mySession.SessionName))
                If (Not Me.mySession.SessionName = Nothing) Then
                    Me.mySession.SessionName = Nothing
                End If

                Me.Close()
            End If

        Catch ex As NullReferenceException
            MessageBox.Show("No session file is loaded.  Nothing has been deleted.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
        Catch ex As Exception
            MessageBox.Show("An unknown error has occured while clearing the session.  Please launch the Receiving Document module and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            Me.Close()
        Finally
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    Private Sub MenuItemExitReceiveDocument_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemExitReceiveDocument.Click
        If MessageBox.Show("Return to the Main Menu?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            ExitReceiveDocument()
            Me.Close()
        End If
    End Sub

    Private Sub MenuItemExitIrmaMobile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemExitIrmaMobile.Click
        If MessageBox.Show("Exit IRMA Mobile?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            ExitReceiveDocument()
            Me.Close()
            Me.DialogResult = Windows.Forms.DialogResult.Abort
        End If
    End Sub

    Private Sub ClearScreen()
        invoiceNum.Text = String.Empty
        Me.vendorCombo.SelectedValue = "NN"
        Me.returnCombo.SelectedValue = "NN"
    End Sub

    Private Function CheckSubteamExistWithStore() As Boolean
        Return Me.mySession.WebProxyClient.StoreSubTeamRelationshipExists(mySession.StoreNo, mySession.SubteamKey)
    End Function

    Private Sub ExitReceiveDocument()
        mySession.IsLoadedSession = False
        mySession.DSDInvoice = Nothing
        mySession.DSDVendorName = Nothing
        mySession.DSDVendorID = Nothing
        mySession.ActionType = Nothing
    End Sub

    Private Sub AlignText()
        LabelInvoiceNumber.TextAlign = ContentAlignment.TopRight
        LabelInvoiceCredit.TextAlign = ContentAlignment.TopRight
    End Sub

End Class