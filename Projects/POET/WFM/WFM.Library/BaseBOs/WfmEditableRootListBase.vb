Namespace BaseBusinessObjects
    <Serializable()> _
    Public MustInherit Class WfmEditableRootListBase(Of T As {Csla.Core.IEditableBusinessObject, Csla.Core.IUndoableObject, Csla.Core.ISavable})
        Inherits Csla.EditableRootListBase(Of T)
    End Class

End Namespace
