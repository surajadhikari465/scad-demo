Namespace WholeFoods.IRMA.ModelLayer

    ''' <summary>
    ''' Thrown if there is an attempt to start a new transaction
    ''' for a thread when there is already one open.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NestedTransactionException
        Inherits Exception

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal inMessage As String)
            MyBase.New(inMessage)
        End Sub

    End Class

End Namespace
