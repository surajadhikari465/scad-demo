Public Class clsBcpOutFileNotCreated
    Inherits System.Exception
    Private m_sFileName As String
    Public Sub New(ByVal sFileName As String)
        MyBase.New("BCP output file not created")
        m_sFileName = sFileName
    End Sub
    Public ReadOnly Property FileName() As String
        Get
            Return m_sFileName
        End Get
    End Property
End Class
