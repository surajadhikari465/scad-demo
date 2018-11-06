Option Explicit On
Option Strict On

Imports System
Imports System.Net
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient
Imports System.Net.Mail.MailMessage

'smtp.wholefoods.com

Namespace WholeFoods.Utility.SMTP

    Public Class SMTP

#Region "Class Variables"

        Dim mailer As New SmtpClient

        Private _hostname As String
        Private _username As String
        Private _password As String

#End Region

#Region "Constructors"
        ''' <summary>
        ''' Blank constructor
        ''' </summary>
        ''' <remarks>Hostname, message body, mail recipients and subject must be set manually</remarks>
        Sub New()
        End Sub

        ''' <summary>
        ''' Constructor just taking the hostname
        ''' </summary>
        ''' <param name="Hostname">in smtp.blah.com form</param>
        ''' <remarks>This will use a default L/P</remarks>
        Sub New(ByVal Hostname As String)
            _hostname = Hostname
            mailer.Host = Hostname
        End Sub

        ''' <summary>
        ''' Constructor just taking the hostname, and L/P
        ''' </summary>
        ''' <param name="Hostname">in smtp.blah.com form</param>
        ''' <param name="Username"></param>
        ''' <param name="Password"></param>
        ''' <remarks></remarks>
        Sub New(ByVal Hostname As String, ByVal Username As String, ByVal Password As String)
            _hostname = Hostname
            _username = Username
            _password = Password
            mailer.Host = Hostname
        End Sub

#End Region

#Region "Properties"
        'hostname of the smtp server
        Public Property Hostname() As String
            Get
                Return _hostname
            End Get
            Set(ByVal value As String)
                _hostname = value
            End Set
        End Property
        'username to log into the smtp server
        Public Property Username() As String
            Get
                Return _username
            End Get
            Set(ByVal value As String)
                _username = value
            End Set
        End Property
        'password to login to the smtp server
        Public Property Password() As String
            Get
                Return _password
            End Get
            Set(ByVal value As String)
                _password = value
            End Set
        End Property

#End Region

#Region "Public Methods"


        ''' <summary>
        ''' Send Method, used to send messages to the SMTP server
        ''' </summary>
        ''' <param name="msgBody"></param>
        ''' <param name="msgTo"></param>
        ''' <param name="msgFrom"></param>
        ''' <param name="msgSubject"></param>
        ''' <remarks>pass in params to send the message to the smtp client</remarks>
        Public Sub send(ByVal msgBody As String, ByVal msgTo As String, ByVal msgCC As String, ByVal msgFrom As String, ByVal msgSubject As String)

            Dim message As New Mail.MailMessage
            Dim addyTo As String()
            Dim addyCC As String()
            Dim subString As String
            Dim addyFrom As Mail.MailAddress

            'set from address
            addyFrom = New MailAddress(msgFrom)
            message.From = addyFrom

            'grab "To" Addresses
            addyTo = GetAddress(msgTo)

            For Each subString In addyTo
                'spins through each substring in the array and stores in the "To" address line in the mail message object
                message.To.Add(subString)
            Next subString

            'check to see if a CC string is passed in and add them to the message object
            If Trim(msgCC).Length > 0 Then
                'grab "CC" addresses
                addyCC = GetAddress(msgCC)

                For Each subString In addyCC
                    'spins through each substring in the array and stores in the "CC" address line in the mail message object
                    message.CC.Add(subString)
                Next subString
            End If


            'set body, subject, and default login
            If msgSubject IsNot Nothing Then
                message.Subject = msgSubject
            Else
                message.Subject = "System Message"
            End If

            message.Body = msgBody
            'check the message to see if it is an HTML message
            If msgBody.StartsWith("<HTML>") Then
                message.IsBodyHtml = True
            Else
                message.IsBodyHtml = False
            End If

            mailer.Credentials = CredentialCache.DefaultNetworkCredentials
            'send the email
            Try
                mailer.Send(message)
            Catch ex As SMTPException
                throwException("The message was unable to send", ex)

            End Try

            'this is old, no longer using a address collection object, I am now storing the addresses in a string array
            'Dim i As Integer
            ''sets i to the total number of items in the address collection
            'i = addyTo.Count
            ''adds every item from the address collection into the To address in the smtp client
            'Do Until i = -1
            '    message.To.Add(CType(addyTo.Item(i), MailAddress))
            '    i = i - 1
            'Loop

        End Sub

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' Grab the credentials from username/password
        ''' </summary>
        Private Function GetCredentials() As ICredentialsByHost
            Return New NetworkCredential(Username, Password)
        End Function

        ''' <summary>
        ''' Split the address names up into an array
        ''' </summary>
        Private Function GetAddress(ByVal address As String) As String()
            Const Semicolon As Char = ";"c
            Dim delimiter As Char = Semicolon
            'splits the addresses up based on their delimiter being a ; and then returns an array with the addresses split up
            Dim resultArray As String() = address.Split(delimiter)
            Return resultArray
        End Function

        ''' <summary>
        ''' Log an error and throw a new SMTPException.
        ''' </summary>
        ''' <param name="message"></param>
        ''' <param name="innerException"></param>
        ''' <remarks></remarks>
        Protected Sub throwException(ByVal message As String, Optional ByVal innerException As Exception = Nothing)
            Dim newException As SMTPException
            If innerException IsNot Nothing Then
                Logger.LogError(message, Me.GetType(), innerException)
                newException = New SMTPException(message, innerException)
            Else
                Logger.LogError(message, Me.GetType())
                newException = New SMTPException(message)
            End If

            ' Throw the exception
            Throw newException
        End Sub

#End Region

    End Class

End Namespace