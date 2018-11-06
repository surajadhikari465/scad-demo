Public Class ReferenceList
    Dim m_list_id As Long
    Dim m_list_desc As String
    Public ReadOnly Property ListID() As Long
        Get
            Return m_list_id
        End Get
    End Property
    Public ReadOnly Property ListDesc() As String
        Get
            Return m_list_desc
        End Get
    End Property
    Public Sub New(ByVal lListID As Long, ByVal sListDesc As String)
        m_list_id = lListID
        m_list_desc = sListDesc
    End Sub
End Class
