Imports System.Runtime.Serialization
Imports System.ComponentModel


Namespace BaseBusinessObjects
    <Serializable()> _
    Public MustInherit Class WfmBusinessBase(Of t As BusinessBase(Of t))
        Inherits BusinessBase(Of t)
        'Implements Csla.Security.IAuthorizeReadWrite



    End Class

End Namespace
