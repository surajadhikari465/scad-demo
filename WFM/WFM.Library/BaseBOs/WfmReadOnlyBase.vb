Namespace BaseBusinessObjects
    <Serializable()> _
    Public MustInherit Class WfmReadOnlyBase(Of T As ReadOnlyBase(Of T))
        Inherits Csla.ReadOnlyBase(Of T)

    End Class

End Namespace
