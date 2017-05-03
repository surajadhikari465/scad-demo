Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemChaining.DataAccess
    Public Class ItemChain
        Inherits WholeFoods.IRMA.ItemChaining.BusinessLogic.ItemChain

        Private Sub LoadItems()
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim results As SqlDataReader = Nothing
            Dim listViewItem As ListViewItem
            Dim currentOrdinal As Integer

            Try
                paramList.Add(New DBParam("Chain_ID", DBParamType.Int, Convert.ToInt32(ID)))

                results = factory.GetStoredProcedureDataReader("GetItemChainItems_ByItemChainID", paramList)

                ChainedItems.Clear()

                While results.Read
                    listViewItem = New ListViewItem

                    listViewItem.SubItems.Add(results.GetString(results.GetOrdinal("Identifier")))
                    listViewItem.SubItems.Add(results.GetInt32(results.GetOrdinal("Item_Key")).ToString())
                    listViewItem.Text = results.GetString(results.GetOrdinal("Item_Description")).Trim()

                    currentOrdinal = results.GetOrdinal("Chain_ID")

                    If results.IsDBNull(currentOrdinal) Then
                        listViewItem.StateImageIndex = 0
                    Else
                        listViewItem.SubItems.Add(results.GetInt32(currentOrdinal).ToString())
                        listViewItem.StateImageIndex = 1
                    End If

                    ChainedItems.Add(listViewItem)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try
        End Sub

        Private Sub CreateChainItem(ByRef factory As DataFactory, ByVal itemId As Integer, ByRef transaction As SqlTransaction)
            Dim paramList As New ArrayList

            paramList.Add(New DBParam("Item_Key", DBParamType.Int, itemId))
            paramList.Add(New DBParam("Chain_ID", DBParamType.Int, Convert.ToInt32(ID)))

            factory.ExecuteStoredProcedure("InsertItemChainItem", paramList, transaction)
        End Sub

        Public Overrides Function CreateChain() As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim transaction As SqlTransaction = factory.BeginTransaction(IsolationLevel.ReadCommitted)
            Dim success As Boolean = True

            Try
                ' Delete previous chain
                If ID <> "" AndAlso Not Me.DeleteChain(transaction) Then
                    ' If the attempt to delete failed, exit operation as failure
                    success = False
                Else
                    paramList.Add(New DBParam("Chain_Name", DBParamType.String, ChainName))

                    ' Output parameter for the generated ID
                    paramList.Add(New DBParam("Chain_ID", DBParamType.Int))

                    paramList = factory.ExecuteStoredProcedure("InsertItemChain", paramList, transaction)

                    If paramList.Count = 1 AndAlso TypeOf paramList(0) Is Integer Then
                        ID = CType(paramList(0), Integer).ToString()

                        ' Insert all items
                        For Each item As String In ChainedItems
                            CreateChainItem(factory, Convert.ToInt32(item), transaction)
                        Next
                    End If
                End If
            Catch
                success = False
            Finally
                If (success) Then
                    transaction.Commit()
                Else
                    transaction.Rollback()
                End If
            End Try

            Return success
        End Function

        Public Overloads Function DeleteChain(ByRef transaction As SqlTransaction) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim success As Boolean = True

            paramList.Add(New DBParam("Chain_ID", DBParamType.Int, Convert.ToInt32(ID)))

            Try
                factory.ExecuteStoredProcedure("DeleteItemChain", paramList, transaction)
            Catch ex As Exception
                success = False
            End Try

            Return success
        End Function

        Public Overrides Function DeleteChain() As Boolean
            Return DeleteChain(Nothing)
        End Function

        Public Overrides Function GetChain(ByVal withItems As Boolean) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim reader As SqlDataReader

            paramList.Add(New DBParam("Chain_ID", DBParamType.Int, Convert.ToInt32(ID)))

            reader = factory.GetStoredProcedureDataReader("GetItemChain_ByItemChainID", paramList)

            If (reader.Read) Then
                ChainName = reader.GetString(reader.GetOrdinal("Value"))
            End If

            If withItems Then
                LoadItems()
            End If
        End Function


        Public Overrides Function ListChains() As System.Data.DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New DBParamList

            paramList.Add(New DBParam("Start", DBParamType.String, "%"))

            Return factory.GetStoredProcedureDataSet("GetItemChains_ByDescriptionStartsWith", paramList)
        End Function

        Public Overrides Function ListItemChains(ByVal ItemID As String) As System.Data.DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New DBParamList

            paramList.Add(New DBParam("ItemID", DBParamType.String, ItemID))

            Return factory.GetStoredProcedureDataSet("GetItemChains_ByItemKey", paramList)
        End Function

        Public Overrides Function IsREGPriceDifference(ByVal ItemIDs As String, ByVal StoreIDs As String) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New DBParamList
            Dim results As DataSet

            paramList.Add(New DBParam("Items", DBParamType.String, ItemIDs))
            paramList.Add(New DBParam("Stores", DBParamType.String, StoreIDs))

            results = factory.GetStoredProcedureDataSet("CheckREGPriceDifference", paramList)

            For Each row As Data.DataRow In results.Tables(0).Rows
                If row("Found").ToString <> "1" Then
                    Return True
                End If
            Next

            Return False
        End Function

        Public Overrides Function ItemPriceListByItemAndStore(ByVal ItemIDs As String, ByVal StoreIDs As String) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New DBParamList

            paramList.Add(New DBParam("Items", DBParamType.String, ItemIDs))
            paramList.Add(New DBParam("Stores", DBParamType.String, StoreIDs))

            Return factory.GetStoredProcedureDataSet("ItemPriceListByItemAndStore", paramList)
        End Function

    End Class
End Namespace