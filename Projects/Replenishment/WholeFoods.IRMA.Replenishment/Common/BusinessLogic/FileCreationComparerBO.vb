Imports System.IO
Namespace WholeFoods.IRMA.Replenishment.Common.BusinessLogic
    Public Class FileCreationComparerBO
        Implements IComparer
        Public Function Compare(ByVal info1 As Object, ByVal info2 As Object) As Integer Implements IComparer.Compare
            Dim fileInfo1 As FileSystemInfo = CType(info1, FileSystemInfo)
            Dim fileInfo2 As FileSystemInfo = CType(info2, FileSystemInfo)
            Return Date.Compare(fileInfo1.CreationTime, fileInfo2.CreationTime)
        End Function

    End Class
End Namespace
