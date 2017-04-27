Imports System.Data.SqlClient
Imports System.Linq
Imports log4net
Imports WholeFoods.IRMA.Planogram.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Planogram.DataAccess

    Public Class PlanogramDAO

        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Shared Function GetItemDataset(ByRef ItemDataset As DataSet, ByVal strFillTableName As String) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim reader As DataSet = Nothing

            Try
                reader = factory.GetDataSet("Exec Replenishment_POSPull_GetIdentifier", ItemDataset, strFillTableName)
            Finally
                factory = Nothing
            End Try

            Return reader
        End Function

        Public Shared Function GetPlanogramItems(ByVal store_no As Integer, ByVal subteam_no As Integer,
                        ByVal setList As String, ByVal isRegular As Boolean, ByVal startDate As Date) As Integer()
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing
            Dim itemList() As Integer
            Dim itemArrayList As New ArrayList
            Dim index As Integer = 0

            Try
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = store_no
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Subteam_No"
                If subteam_no <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = subteam_no
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "SetNoList"
                If setList.Length = 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = setList
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ListSeparator"
                currentParam.Value = "|"
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                If isRegular Then
                    results = factory.GetStoredProcedureDataReader("Planogram_GetRegPlanogramItems", paramList)
                Else
                    currentParam = New DBParam
                    currentParam.Name = "StartDate"
                    currentParam.Value = startDate
                    currentParam.Type = DBParamType.DateTime
                    paramList.Add(currentParam)

                    results = factory.GetStoredProcedureDataReader("Planogram_GetNonRegPlanogramItems", paramList)
                End If

                While results.Read
                    itemArrayList.Add(results.GetInt32(results.GetOrdinal("Item_Key")))
                End While
            Finally
                factory = Nothing
            End Try

            ReDim itemList(itemArrayList.Count - 1)
            For index = 0 To itemArrayList.Count - 1
                itemList(index) = CInt(itemArrayList.Item(index))
            Next

            Return itemList
        End Function

        Public Shared Function GetValidPlanogramIdentifiers(planogramItems As List(Of PlanogramItemBO)) As List(Of String)
            Dim tableType As DataTable = New DataTable()

            With tableType.Columns
                .Add("Identifier", GetType(String))
            End With

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim connection As SqlConnection = New SqlConnection()
            Dim results As SqlDataReader
            Dim validIdentifiers As List(Of String) = New List(Of String)
            Dim distinctStoreNumbers As List(Of Integer) = planogramItems.Select(Function(pi) pi.StoreNo).Distinct().ToList()

            For Each storeNumber As Integer In distinctStoreNumbers
                Dim itemsForStore As List(Of String) = planogramItems.Where(Function(pi) pi.StoreNo = storeNumber).Select(Function(pi) pi.Identifier).ToList()

                For Each identifier As String In itemsForStore
                    tableType.Rows.Add({identifier})
                Next

                Try
                    connection.ConnectionString = factory.ConnectString
                    connection.Open()

                    Dim command As New SqlCommand("GetValidPlanogramIdentifiers", connection) With {.CommandType = CommandType.StoredProcedure, .CommandTimeout = factory.CommandTimeout}

                    Dim identifiersParameter As SqlParameter = command.Parameters.Add("@Identifiers", SqlDbType.Structured)
                    identifiersParameter.Value = tableType

                    Dim storeNumberParameter As SqlParameter = command.Parameters.Add("@StoreNumber", SqlDbType.Int)
                    storeNumberParameter.Value = storeNumber

                    results = command.ExecuteReader()

                    While results.Read
                        validIdentifiers.Add(results.GetString(results.GetOrdinal("Identifier")))
                    End While

                    Dim invalidIdentifiers As List(Of String) = itemsForStore.Except(validIdentifiers).ToList()

                    If invalidIdentifiers.Count > 0 Then
                        logger.Info(String.Format("The following identifiers will be excluded from the planogram print request for store {0}. Check authorization, validation, and deleted status: {1}",
                                                  storeNumber, String.Join(", ", invalidIdentifiers)))
                    End If
                Finally
                    connection.Close()
                    tableType.Clear()
                End Try
            Next

            Return validIdentifiers
        End Function

        Public Shared Sub InsertPlanogramItems(ByVal storeNo As Integer, ByVal pathFileName As String)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                If storeNo <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = storeNo
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PathFileName"
                If pathFileName Is Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = pathFileName
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                factory.ExecuteStoredProcedure("Planogram_InsertPlanogramItem", paramList)
            Finally
                factory = Nothing
                paramList = Nothing
                currentParam = Nothing
            End Try
        End Sub

        Public Shared Function GetSetNumbersComboList(ByVal storeNo As Integer, Optional ByVal subTeam_No As Integer = 0) As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim planogramSet As PlanogramSetBO
            Dim planogramSetList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = storeNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Subteam_No"
                If subTeam_No > 0 Then
                    currentParam.Value = subTeam_No
                Else
                    currentParam.Value = DBNull.Value
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("Planogram_GetSetNumbers", paramList)

                While results.Read
                    planogramSet = New PlanogramSetBO()
                    planogramSet.Description = results.GetString(results.GetOrdinal("ProductPlanogramCode"))

                    planogramSetList.Add(planogramSet)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If

                factory = Nothing
                paramList = Nothing
                currentParam = Nothing
            End Try

            Return planogramSetList
        End Function
    End Class
End Namespace