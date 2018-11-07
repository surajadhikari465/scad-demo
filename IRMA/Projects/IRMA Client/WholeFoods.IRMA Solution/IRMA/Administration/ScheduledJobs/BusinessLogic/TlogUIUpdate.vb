Public Class TlogUIUpdate

    Private _StatusMsg As String
    Public Property StatusMsg() As String
        Get
            Return _StatusMsg
        End Get
        Set(ByVal value As String)
            _StatusMsg = value
        End Set
    End Property


    Private _ProgressMax As Integer
    Public Property ProgressMax() As Integer
        Get
            Return _ProgressMax
        End Get
        Set(ByVal value As Integer)
            _ProgressMax = value
        End Set
    End Property


    Private _ProgressValue As Integer
    Public Property ProgressValue() As Integer
        Get
            Return _ProgressValue
        End Get
        Set(ByVal value As Integer)
            _ProgressValue = value
        End Set
    End Property

    Public Sub ClearValues()
        Me.ProgressMax = -1
        Me.ProgressValue = -1
        Me.StatusMsg = String.Empty
    End Sub

End Class
