Namespace WholeFoods.IRMA.Common.BusinessLogic
    Public Class ControlGroupStatusBO
        Private _ControlGroupStatusID As Integer
        Private _ControlGroupStatusName As String

        Public Property ControlGroupStatusID() As Integer
            Get
                Return _ControlGroupStatusID
            End Get
            Set(ByVal value As Integer)
                _ControlGroupStatusID = value
            End Set
        End Property


        Public Property ControlGroupStatusName() As String
            Get
                Return _ControlGroupStatusName
            End Get
            Set(ByVal value As String)
                _ControlGroupStatusName = value
            End Set
        End Property


    End Class
End Namespace
