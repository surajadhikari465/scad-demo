Namespace BaseBusinessObjects
    <Serializable()> _
    Public MustInherit Class WfmReadOnlyListBase(Of T As ReadOnlyListBase(Of T, C), C)
        Inherits ReadOnlyListBase(Of T, C)

    End Class
End Namespace
