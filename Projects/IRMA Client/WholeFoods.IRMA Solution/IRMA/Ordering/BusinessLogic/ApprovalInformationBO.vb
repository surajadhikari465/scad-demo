



Public Class ApprovalInformationBO
    ' this class is used for PO Approval


    Public Enum ApprovalTypeEnum
        None
        PayByPO
        PayByInvoice
    End Enum


    Private _ApprovalType As ApprovalTypeEnum
    Public Property ApprovalType() As ApprovalTypeEnum
        Get
            Return _ApprovalType
        End Get
        Set(ByVal value As ApprovalTypeEnum)
            _ApprovalType = value
        End Set
    End Property

    Private _resolution As ResolutionCodeBO
    Public Property Resolution() As ResolutionCodeBO
        Get
            Return _resolution
        End Get
        Set(ByVal value As ResolutionCodeBO)
            _resolution = value
        End Set
    End Property

    Public Function IsValid() As Boolean
        Return ((_ApprovalType <> ApprovalTypeEnum.None) And (_resolution IsNot Nothing))
    End Function

    Public Sub Clear()
        Me._ApprovalType = ApprovalTypeEnum.None
        Me._resolution = Nothing

    End Sub

End Class