Imports System.Web.Services
Imports System.Web.Services.Protocols

<AttributeUsage(AttributeTargets.Method)> _
Public Class SoapDisplayExtensionAttribute
    Inherits SoapExtensionAttribute

    Private m_Priority As Integer = 1

    ' Specifies the class of the SOAP
    ' Extension to use with this method
    Public Overrides ReadOnly Property ExtensionType() As Type
        Get
            ExtensionType = GetType(SoapDisplayExtension)
        End Get
    End Property

    ' Member to store the extension's priority
    Public Overrides Property Priority() As Integer
        Get
            Priority = m_Priority
        End Get
        Set(ByVal Value As Integer)
            m_Priority = Value
        End Set
    End Property

End Class


