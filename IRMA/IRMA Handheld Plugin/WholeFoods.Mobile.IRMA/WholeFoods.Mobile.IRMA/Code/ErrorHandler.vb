Imports System.Windows.Forms
Imports System
Imports System.Text
Imports System.Linq
Imports System.Data
Imports Microsoft.WindowsCE.Forms

Public Class ErrorHandler

    Private notify As Notification = Nothing
    Private serviceFault As ParsedCFFaultException = Nothing

    Public Sub New()

    End Sub

    Public Sub New(ByVal err As ParsedCFFaultException)
        Me.serviceFault = err
    End Sub

    Public Sub ShowErrorNotification()

        notify = New Notification()
        AddHandler notify.ResponseSubmitted, AddressOf ErrorNotification_ResponseSubmitted
        AddHandler notify.BalloonChanged, AddressOf ErrorNotification_BalloonChanged
        AddHandler notify.Disposed, AddressOf ErrorNotification_Disposed
        notify.Caption = "An error has occured."
        notify.Critical = True
        notify.InitialDuration = 10

        Dim HTMLString As New StringBuilder()

        HTMLString.Append("<html><body>")
        HTMLString.Append("<font color=""#0000FF""><b>An error has been returned from the IRMA handheld service.</b></font>")
        HTMLString.Append("<br/> Click <a href=""View"">here</a> to view error details.")
        HTMLString.Append("</body></html>")
        notify.Text = HTMLString.ToString
        notify.Visible = True

    End Sub

    Public Sub ErrorNotification_Disposed(ByVal obj As Object, ByVal args As EventArgs)
        RemoveHandler notify.ResponseSubmitted, AddressOf ErrorNotification_ResponseSubmitted
        RemoveHandler notify.BalloonChanged, AddressOf ErrorNotification_BalloonChanged
    End Sub

    Public Sub ErrorNotification_BalloonChanged(ByVal obj As Object, ByVal balloonArgs As BalloonChangedEventArgs)
        If balloonArgs.Visible = False Then
            notify.Dispose()
        End If
    End Sub

    Public Sub ErrorNotification_ResponseSubmitted(ByVal obj As Object, ByVal resevent As ResponseSubmittedEventArgs)
        If resevent.Response.ToLower.Equals("view") Then
            Dim errorform As ViewError = New ViewError(serviceFault)
            errorform.ShowDialog()
            errorform.Dispose()
        End If
        notify.Dispose()
    End Sub

    Public Sub DisplayErrorMessage(ByVal errorMessage As String, ByVal errorMethod As String)
        MsgBox(errorMessage + Environment.NewLine + Environment.NewLine + errorMethod + Environment.NewLine + Environment.NewLine + Messages.PLEASE_RETRY, MsgBoxStyle.Critical, "Error")
    End Sub

End Class