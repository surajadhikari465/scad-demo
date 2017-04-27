Public Interface INoTagRule
    ReadOnly Property ExcludedItems As List(Of Integer)

    Sub ApplyRule(items As List(Of Integer), storeNumber As Integer, historyTreshold As Integer)
End Interface
