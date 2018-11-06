Imports System.Windows.Forms
Imports WholeFoods.Utility
Imports System.Collections.Generic
Imports System.Collections.ObjectModel


Public Class ErrorDialog

#Region "Public Enums"

    Public Enum NotificationTypes
        DialogOnly
        EmailOnly
        DialogAndEmail
    End Enum

#End Region

#Region "Shared Methods"

    Public Shared Sub HandleError(ByVal inSubject As String, _
        ByVal inShortMessage As String, ByVal inLongMessage As String, ByVal inNotificationType As NotificationTypes, _
        ByVal inErrorCategoryCode As String)

        _handleError(Nothing, inSubject, inShortMessage, inLongMessage, inNotificationType, inErrorCategoryCode)

    End Sub

    Public Shared Sub HandleError(ByVal ex As System.Exception, ByVal inNotificationType As NotificationTypes)

        Call HandleError(Nothing, ex, inNotificationType, Nothing)

    End Sub

    Public Shared Sub HandleError(ByVal inSubject As String, ByVal ex As System.Exception, ByVal inNotificationType As NotificationTypes, ByVal inErrorCategoryCode As String)

        Dim shortMessage As String = String.Empty
        Dim longMessage As String = String.Empty

        If ex.InnerException IsNot Nothing Then
            shortMessage = String.Format("{0}{1}{1}{2}", ex.Message, ControlChars.NewLine, ex.InnerException.Message)
        Else
            shortMessage = ex.Message
        End If

        longMessage = ErrorHandler.GenerateRTF(ex)

        _handleError(ex, inSubject, shortMessage, longMessage, inNotificationType, inErrorCategoryCode)

    End Sub

    Private Shared Sub _handleError(ByVal ex As System.Exception, ByVal inSubject As String, _
        ByVal inShortMessage As String, ByVal inLongMessage As String, ByVal inNotificationType As NotificationTypes, _
        ByVal inErrorCategoryCode As String)

        Dim theErrorDialog As New ErrorDialog()
        Dim emailStatus As String = "An admin has been notified by email."

        If IsNothing(inSubject) OrElse String.IsNullOrEmpty(inSubject) Then
            inSubject = String.Format("{0} error", theErrorDialog.ApplicationInfo)
        End If

        With theErrorDialog
            .Exception = ex
            .Subject = inSubject
            .ShortMessage = inShortMessage
            .LongMessage = inLongMessage
        End With

        If inNotificationType <> NotificationTypes.DialogOnly Then
            emailStatus = theErrorDialog.SendErrorEmail(inErrorCategoryCode)
            theErrorDialog.LabelEmailHasBeenSent.Text = emailStatus
        End If

        If inNotificationType <> NotificationTypes.EmailOnly Then
            theErrorDialog.ShowDialog()
        End If

        If theErrorDialog IsNot Nothing Then
            theErrorDialog.Dispose()
            theErrorDialog = Nothing
        End If

    End Sub

#End Region

#Region "Fields and Properties"

    Private _exception As System.Exception

    Private Property Exception() As System.Exception
        Get
            Return _exception
        End Get
        Set(ByVal value As System.Exception)
            _exception = value
        End Set
    End Property

    Private ReadOnly Property ApplicationInfo() As String
        Get
            Dim version As String = String.Empty
            With My.Application.Info
                If .Version.Revision = 0 Then
                    version = String.Format("{0} v{1}.{2}.{3}", .ProductName, .Version.Major, .Version.Minor, .Version.Build)
                Else
                    version = String.Format("{0} v{1}.{2}.{3}.{4}", .ProductName, .Version.Major, .Version.Minor, .Version.Build, .Version.Revision)
                End If
                Return version
            End With
        End Get
    End Property

    Private Property Subject() As String
        Get
            Return Me.Text
        End Get
        Set(ByVal value As String)
            Me.Text = value
        End Set
    End Property

    Private Property ShortMessage() As String
        Get
            Return Me.TextBoxShortMessage.Text
        End Get
        Set(ByVal value As String)
            Me.TextBoxShortMessage.Text = value
        End Set
    End Property

    Private Property LongMessage() As String
        Get
            Return Me.RichTextBoxFullError.Text
        End Get
        Set(ByVal value As String)
            If value.StartsWith("{\rtf", StringComparison.OrdinalIgnoreCase) Then
                Me.RichTextBoxFullError.Rtf = value
            Else
                Me.RichTextBoxFullError.Text = value
            End If
        End Set
    End Property

    Private ReadOnly Property FullMessage() As String
        Get
            Dim theFullMessage As New System.Text.StringBuilder()

            If Me.Exception IsNot Nothing Then
                theFullMessage.Append(ErrorHandler.GenerateHTML(Me.Exception))
            Else
                With theFullMessage
                    .AppendLine(Me.ShortMessage)
                    .Append("_"c, 40)
                    .AppendLine()
                    .AppendLine()
                    .AppendFormat("Application: {0}", Me.ApplicationInfo)
                    .AppendLine()
                    If gsUserName.Trim.Length <> 0 Then
                        .AppendFormat("User:        {0}", gsUserName)
                    Else
                        .AppendFormat("User:        {0}", Environment.UserName)
                    End If
                    .AppendLine()
                    .AppendFormat("Machine:     {0}", Environment.MachineName)
                    .AppendLine()
                    .AppendFormat("Date:        {0}", Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    .AppendLine()
                    .Append("_"c, 40)
                    .AppendLine()
                    .AppendLine()
                    .AppendLine("Full Error:")
                    .AppendLine("------------")
                    .AppendLine(Me.LongMessage)
                End With
            End If

            Return theFullMessage.ToString
        End Get
    End Property

#End Region

#Region "Event Handlers"

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ButtonCopyToClipboard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCopyToClipboard.Click

        Me.Exception = Nothing
        Clipboard.SetText(Me.FullMessage)

        ' give the user come visual feedback
        Me.Enabled = False
        Thread.Sleep(500)
        Me.Enabled = True

    End Sub

    Private Sub lnkDetailToggle_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkDetailToggle.LinkClicked

        Call ToggleDetails()

    End Sub

    Private Sub ErrorDialog_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Height = Me.MinimumSize.Height

    End Sub

    Private Sub tbScroll(ByVal sender As Object, ByVal e As EventArgs) Handles TextBoxShortMessage.ClientSizeChanged, TextBoxShortMessage.TextChanged

        Static busy As Boolean = False

        If busy Then
            Exit Sub
        End If

        busy = True

        Try
            Dim txt As TextBox = CType(sender, TextBox)

            Dim tS As Size = TextRenderer.MeasureText(txt.Text, txt.Font, txt.MaximumSize, TextFormatFlags.WordBreak)

            Dim hsb As Boolean = False ' txt.ClientSize.Width < tS.Width

            Dim vsb As Boolean = txt.ClientSize.Height < tS.Height + txt.Font.Size

            If hsb AndAlso vsb Then
                txt.ScrollBars = ScrollBars.Both
            ElseIf hsb Then
                txt.ScrollBars = ScrollBars.Horizontal
            ElseIf vsb Then
                txt.ScrollBars = ScrollBars.Vertical
            Else
                txt.ScrollBars = ScrollBars.None
            End If

        Catch ex As Exception

        Finally
            busy = False

        End Try

    End Sub

#End Region

#Region "Private Methods"

    Private Function SendErrorEmail(ByVal inErrorCategoryCode As String) As String

        Dim status As String = String.Empty

        If IsNothing(inErrorCategoryCode) OrElse String.IsNullOrEmpty(inErrorCategoryCode) Then
            inErrorCategoryCode = ""
        End If

        If IsNothing(Me.Subject) OrElse String.IsNullOrEmpty(Me.Subject) Then
            Me.Subject = String.Format("{0} error", Me.ApplicationInfo)
        End If

        'Call Global_Renamed.(Environment.UserName, userDisplayName, userEmailAddress)

        Dim emailClient As WholeFoods.Utility.SMTP.SMTP = Nothing
        Dim emailFrom As String = ConfigurationServices.AppSettings("Error_FromEmailAddress")
        Dim emailTo As String = ConfigurationServices.AppSettings(inErrorCategoryCode + "Error_ToEmailAddress")
        Dim emailCC As String = ConfigurationServices.AppSettings(inErrorCategoryCode + "Error_CCEmailAddress")
        Dim emailUser As String = String.Empty
        Dim isUserCopied As Boolean = System.Convert.ToBoolean(ConfigurationServices.AppSettings("CopyUserOnErrorNotifications"))
        Try

            emailUser = API.GetADUserInfo(Environment.UserName, "mail")

            ' The emailCC var cannot be empty when passed to the SMTP class because it will cause an exception. 
            If Not emailCC Is Nothing Then
                emailCC = emailCC.Trim
                If emailCC.Equals(String.Empty) Then
                    emailCC = Nothing
                End If
            End If
            If emailCC Is Nothing Then
                If isUserCopied Then
                    emailCC = emailUser
                End If
                status = emailTo
            Else
                If isUserCopied Then
                    'add current user if not already included in the Cc list
                    If Not emailCC.Contains(emailUser) Then
                        emailCC = String.Concat(emailCC, ";", emailUser)
                    End If
                End If
                status = String.Format("{0}; {1}", emailTo, emailCC)
            End If

            'TODO: add custom error information to the output
            'status = String.Empty

            'Dim errEnumerator As IEnumerator = Me.Exception.Data.GetEnumerator
            'Dim currentError As String

            'While errEnumerator.MoveNext
            '    currentError = CType(errEnumerator.Current, String)

            '    status = String.Concat(status, "; ", currentError)
            'End While


            emailClient = New WholeFoods.Utility.SMTP.SMTP(ConfigurationServices.AppSettings("EmailSMTPServer"))

            emailClient.send(Me.FullMessage, emailTo, emailCC, emailFrom, Me.Subject)

            status = String.Format("An e-mail notification was sent to: {0}.", status)

        Catch ex As Exception
            'e-mail confirmation shouldn't be a fatal error!
            status = String.Format("An error occurred sending an e-mail notification to: {0}.", status)

            ErrorHandler.ProcessError(WholeFoods.Utility.ErrorType.GeneralApplicationError, SeverityLevel.Warning, ex)

            Dim msg As String = "Warning!  No error alert e-mail will be sent due to the following error:"
            If ex.InnerException IsNot Nothing Then
                msg = String.Format("{0}{1}{1} {2} {3}{1} {2} {4}{1}{1}{5}", msg, vbCrLf, Chr(149), ex.Message, ex.InnerException.Message, status)
                Logger.LogWarning(ex.Message, Me.GetType(), ex.InnerException)
            Else
                msg = String.Format("{0}{1}{1} {2} {3}{1}{1}{4}", msg, vbCrLf, Chr(149), ex.Message, status)
                Logger.LogWarning(ex.Message, Me.GetType())
            End If

            MessageBox.Show(msg, "Error E-mail Notification Failure", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Finally
            emailClient = Nothing

        End Try

        Return status

    End Function

    Private Sub ToggleDetails()

        If Me.RichTextBoxFullError.Visible Then
            Me.Height = Me.MinimumSize.Height
            Me.RichTextBoxFullError.Visible = False
            Me.lnkDetailToggle.Text = "Show Exception Details"
        Else
            Me.Height = Me.MaximumSize.Height
            Me.RichTextBoxFullError.Visible = True
            Me.lnkDetailToggle.Text = "Hide Exception Details"
        End If

    End Sub

#End Region

End Class
