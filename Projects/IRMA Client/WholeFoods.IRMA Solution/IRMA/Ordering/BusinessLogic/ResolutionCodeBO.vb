

Public Class ResolutionCodeBO
    Private _resolutionCodeId As Integer
    Public Property ResolutionCodeId() As Integer
        Get
            Return _resolutionCodeId
        End Get
        Set(ByVal value As Integer)
            _resolutionCodeId = value
        End Set
    End Property

    Private _resolutionCode As String
    Public Property ResolutionCode() As String
        Get
            Return _resolutionCode
        End Get
        Set(ByVal value As String)
            _resolutionCode = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return _resolutionCode
    End Function

    Sub New(ByVal ResolutionCodeId As Integer, ByVal ResolutionCode As String)
        _resolutionCodeId = ResolutionCodeId
        _resolutionCode = ResolutionCode

    End Sub


End Class


