Namespace WholeFoods.IRMA.Common.BusinessLogic
    Public Class ControlGroupBO
        Private _ControlGroupID As Integer
        Private _ControlGroupName As String

        Public Property ControlGroupID() As Integer
            Get
                Return _ControlGroupID
            End Get
            Set(ByVal value As Integer)
                _ControlGroupID = value
            End Set
        End Property


        Public Property ControlGroupName() As String
            Get
                Return _ControlGroupName
            End Get
            Set(ByVal value As String)
                _ControlGroupName = value
            End Set
        End Property

    End Class
End Namespace
