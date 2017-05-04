Public Class OrderingHistoryRule
    Implements INoTagRule

    Private databaseAccess As NoTagDataAccess

    Private _excludedItems As List(Of Integer)
    Public ReadOnly Property ExcludedItems As List(Of Integer) Implements INoTagRule.ExcludedItems
        Get
            Return _excludedItems
        End Get
    End Property

    Public Sub New(databaseAccess As NoTagDataAccess)
        Me.databaseAccess = databaseAccess
        _excludedItems = New List(Of Integer)
    End Sub

    Public Sub ApplyRule(items As List(Of Integer), storeNumber As Integer, historyThreshold As Integer) Implements INoTagRule.ApplyRule
        _excludedItems = databaseAccess.GetOrderingHistoryExclusions(items, storeNumber, historyThreshold)
    End Sub
End Class
