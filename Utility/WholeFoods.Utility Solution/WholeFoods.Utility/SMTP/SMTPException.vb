Option Explicit On
Option Strict On

Imports System.Runtime.Serialization

Namespace WholeFoods.Utility.SMTP

    ' Exception Class
    <Serializable()> _
        Public Class SMTPException : Inherits ApplicationException

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(ByVal message As String, ByVal originalException As Exception)
            MyBase.New(message, originalException)
        End Sub

        Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.New(info, context)
        End Sub

        ' Add custom members here
    End Class

End Namespace