Imports System.IO
Namespace WholeFoods.IRMA.Replenishment.Common.BusinessLogic
    Public Class FileNameComparerBO
        Implements IComparer
        Public Function Compare(ByVal info1 As Object, ByVal info2 As Object) As Integer Implements IComparer.Compare
            Dim fileInfo1 As FileSystemInfo = CType(info1, FileSystemInfo)
            Dim fileInfo2 As FileSystemInfo = CType(info2, FileSystemInfo)
            Return String.Compare(fileInfo1.Name, fileInfo2.Name)
        End Function
    End Class
End Namespace
