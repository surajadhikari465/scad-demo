Imports System.Security.Principal

Namespace Security

    Public Interface IWMFIdentity
        Inherits IIdentity

        ReadOnly Property UserID() As Integer
    End Interface

End Namespace