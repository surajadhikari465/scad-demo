Public Class ProgressDialog

#Region "Fields and Properties"

    Private _message As String
    Private _owningForm As Form
    Private _itemsName As String

    Public Property SubMessage() As String
        Get
            Return Me.LabelSubmessage.Text
        End Get
        Set(ByVal value As String)
            Me.LabelSubmessage.Text = value
        End Set
    End Property

    Public Property Message() As String
        Get
            Return Me.LabelProgressMessage.Text
        End Get
        Set(ByVal value As String)
            Me.LabelProgressMessage.Text = value
        End Set
    End Property

    Public Property ProgressBarStyle() As ProgressBarStyle
        Get
            Return Me.ProgressBarControl.Style
        End Get
        Set(ByVal value As ProgressBarStyle)
            Me.ProgressBarControl.Style = value
        End Set
    End Property

    Public Property OwningForm() As Form
        Get
            Return _owningForm
        End Get
        Set(ByVal value As Form)
            _owningForm = value
        End Set
    End Property

    Public Property ItemsName() As String
        Get
            Return _itemsName
        End Get
        Set(ByVal value As String)
            _itemsName = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Opens the dialog and reenables the owning form.
    ''' </summary>
    ''' <param name="owningForm">The form that calls this function.</param>
    ''' <param name="message">The message to display in the dialog.</param>
    ''' <param name="progressMaximunCount">The maximum value for the progress bar.</param>
    ''' <param name="showDialogThreshold">The value the maximum value needs to be larger than
    ''' to show this progress dialog.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function OpenProgressDialog(ByRef owningForm As Form, ByVal message As String, ByVal progressMaximunCount As Integer, ByVal showDialogThreshold As Integer) As ProgressDialog

        Dim subMessage As String = "Please Stand By..."
        Return OpenProgressDialog(owningForm, message, subMessage, "", progressMaximunCount, showDialogThreshold)

    End Function

    ''' <summary>
    ''' Opens the dialog and disables the owning form.
    ''' </summary>
    ''' <param name="owningForm">The form that calls this function.</param>
    ''' <param name="message">The main message to display in the dialog.</param>
    ''' <param name="subMessage">A submessage to display in the dialog.</param>
    ''' <param name="progressMaximunCount">The maximum value for the progress bar.</param>
    ''' <param name="showDialogThreshold">The value the maximum value needs to be larger than
    ''' to show this progress dialog.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function OpenProgressDialog(ByRef owningForm As Form, ByVal message As String, ByVal subMessage As String, ByVal inItemsName As String, ByVal progressMaximunCount As Integer, ByVal showDialogThreshold As Integer) As ProgressDialog

        Dim theProgressBarStyle As ProgressBarStyle = ProgressBarStyle.Blocks
        Return OpenProgressDialog(owningForm, message, subMessage, inItemsName, theProgressBarStyle, progressMaximunCount, showDialogThreshold)
    End Function

    ''' <summary>
    ''' Opens the dialog and disables the owning form.
    ''' </summary>
    ''' <param name="owningForm">The form that calls this function.</param>
    ''' <param name="message">The main message to display in the dialog.</param>
    ''' <param name="subMessage">A submessage to display in the dialog.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function OpenProgressDialog(ByRef owningForm As Form, ByVal message As String, ByVal subMessage As String, _
            ByVal inItemsName As String, ByVal inProgressBarStyle As ProgressBarStyle) As ProgressDialog

        Return OpenProgressDialog(owningForm, message, subMessage, inItemsName, inProgressBarStyle, 0, 0)
    End Function

    ''' <summary>
    ''' 
    ''' Opens the dialog and disables the owning form.
    ''' </summary>
    ''' <param name="owningForm">The form that calls this function.</param>
    ''' <param name="message">The main message to display in the dialog.</param>
    ''' <param name="subMessage">A submessage to display in the dialog.</param>
    ''' <param name="progressMaximunCount">The maximum value for the progress bar.</param>
    ''' <param name="showDialogThreshold">The value the maximum value needs to be larger than
    ''' to show this progress dialog.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function OpenProgressDialog(ByRef owningForm As Form, _
        ByVal message As String, ByVal subMessage As String, ByVal inItemsName As String, _
        ByVal inProgressBarStyle As ProgressBarStyle, ByVal progressMaximunCount As Integer, _
        ByVal showDialogThreshold As Integer) As ProgressDialog

        Dim theCurrentProgressDialog As ProgressDialog = Nothing

        owningForm.Enabled = False
        owningForm.Cursor = Cursors.WaitCursor

        If progressMaximunCount >= showDialogThreshold Then

            ' disable the parent form, create, show, and return a new progress dialog
            theCurrentProgressDialog = New ProgressDialog()
            theCurrentProgressDialog.OwningForm = owningForm
            theCurrentProgressDialog.ItemsName = inItemsName
            theCurrentProgressDialog.ProgressBarStyle = inProgressBarStyle

            theCurrentProgressDialog.OwningForm.AddOwnedForm(theCurrentProgressDialog)
            theCurrentProgressDialog.Cursor = Cursors.WaitCursor

            theCurrentProgressDialog.TopLevel = True
            theCurrentProgressDialog.Message = message

            If String.IsNullOrEmpty(subMessage) Then
                subMessage = "Please Stand By..."
            End If

            theCurrentProgressDialog.SubMessage = subMessage
            theCurrentProgressDialog.ProgressBarControl.Minimum = 0
            theCurrentProgressDialog.ProgressBarControl.Maximum = progressMaximunCount
            theCurrentProgressDialog.Show()
        End If

        Return theCurrentProgressDialog

    End Function

    ''' <summary>
    ''' Updates the progress bar value and
    ''' calls Application.DoEvents() for you.
    ''' </summary>
    ''' <param name="currentProgressValue"></param>
    ''' <remarks></remarks>
    Public Sub UpdateProgressDialogValue(ByVal currentProgressValue As Integer)

        If Me.ProgressBarStyle = Windows.Forms.ProgressBarStyle.Marquee Then
            Me.LabelTextProgress.Text = ""
        Else

            If currentProgressValue < Me.ProgressBarControl.Minimum Then
                currentProgressValue = Me.ProgressBarControl.Minimum
            ElseIf currentProgressValue > Me.ProgressBarControl.Maximum Then
                currentProgressValue = Me.ProgressBarControl.Maximum
            End If

            Me.ProgressBarControl.Value = currentProgressValue

            If IsNothing(Me.ItemsName) OrElse String.IsNullOrEmpty(Me.ItemsName) Then
                Me.LabelTextProgress.Text = ""
            Else
                Me.LabelTextProgress.Text = Me.ProgressBarControl.Value.ToString("###,###,##0") + " of " + Me.ProgressBarControl.Maximum.ToString("###,###,###") + _
                    " " + Me.ItemsName
            End If

        End If

        Application.DoEvents()
    End Sub

    ''' <summary>
    ''' Closes the dialog and reenables the owning form.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CloseProgressDialog()
        Me.LabelProgressMessage.Text = ""
        Me.TopLevel = False
        Me.Close()
        Me.Dispose()
        Me.OwningForm.Cursor = Cursors.Default
        Me.OwningForm.Enabled = True
    End Sub

#End Region

End Class