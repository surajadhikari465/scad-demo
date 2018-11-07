Namespace BaseBusinessObjects
    <Serializable()> _
    Public MustInherit Class WfmBusinessListBase(Of T As BusinessListBase(Of T, C), C As {Csla.Core.IEditableBusinessObject})
        Inherits BusinessListBase(Of T, C)
    End Class

End Namespace
