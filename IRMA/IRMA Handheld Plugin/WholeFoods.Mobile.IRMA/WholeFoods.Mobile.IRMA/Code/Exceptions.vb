Public Class Exceptions

#Region "Common"
    Public Shared Function UnknownItemCode(ByVal thisItem As StoreItem) As Boolean
        'ToDo unknown itemcode consolidation
    End Function
#End Region

#Region "Order"
    Public Shared Function CanInventory(ByVal thisItem As StoreItem) As Boolean
        If Not thisItem.CanInventory Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Shared Function ValidIdentifier(ByVal thisItem As StoreItem) As Boolean
        If Not thisItem.Identifier Is Nothing Then
            If Not thisItem.Identifier.Length > 0 Then
                Return False
            Else
                Return True
            End If
        Else
            Return False
        End If

    End Function

    Public Shared Function IsOrderable(ByVal thisItem As StoreItem, ByVal thisSession As Session) As Integer
        If Not thisItem.RetailSale Then
            Return 1
            'ElseIf thisSession.DSDVendorID <> Nothing And thisItem.RetailSubteamNo <> thisSession.SubteamKey Then
            '    Return 2
        ElseIf Not thisItem.IsSellable Then
            Return 3
        Else
            Return 0
        End If
    End Function
#End Region

#Region "Receive Order"
    'ToDo move exceptions lookup and checks to this class
#End Region

#Region "Cycle Count"
    Public Shared Function MaximumQty(ByVal qty As Integer) As Boolean
        If qty > 9999 Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Shared Function DateCheck(ByVal thisCycle As CycleCount) As Boolean
        'ToDo Start/End validation
    End Function

    Public Shared Function StoreMismatch(ByVal thisCycle As CycleCount) As Boolean
        'ToDo store cycle store user compare
    End Function

    Public Shared Function SubTeamMismatch(ByVal thisCycle As CycleCount) As Boolean
        'ToDo subteam cycle subteam user compare
    End Function
#End Region

End Class