Class CreationTimeComparer
    Implements IComparer
    Public Function Compare(ByVal info1 As Object, ByVal info2 As Object) As Integer Implements IComparer.Compare
        Dim fileInfo1 As System.IO.FileSystemInfo = info1
        Dim fileInfo2 As System.IO.FileSystemInfo = info2
        Return Date.Compare(fileInfo1.CreationTime, fileInfo2.CreationTime)
    End Function
End Class
Class NameComparer
    Implements IComparer
    Public Function Compare(ByVal info1 As Object, ByVal info2 As Object) As Integer Implements IComparer.Compare
        Dim fileInfo1 As System.IO.FileSystemInfo = info1
        Dim fileInfo2 As System.IO.FileSystemInfo = info2
        Return String.Compare(fileInfo1.Name, fileInfo2.Name)
    End Function
End Class

