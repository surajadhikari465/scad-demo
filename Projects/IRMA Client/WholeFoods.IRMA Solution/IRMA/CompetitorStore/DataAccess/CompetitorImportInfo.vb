Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic

Namespace WholeFoods.IRMA.CompetitorStore.DataAccess
    Public Class CompetitorImportInfo

#Region "Helper Methods"

        Private Function CreateInsertImportSessionDataCommand(ByVal factory As DataFactory) As SqlCommand
            Dim insertCommand As SqlCommand = factory.GetDataCommand("InsertCompetitorImportSession", Nothing, True)
            Dim currentParameter As SqlParameter

            insertCommand.Parameters.Add(New SqlParameter("User_ID", SqlDbType.Int, 0, "User_ID"))

            currentParameter = New SqlParameter("CompetitorImportSessionID", SqlDbType.Int, 0, "CompetitorImportSessionID")
            currentParameter.Direction = ParameterDirection.Output
            insertCommand.Parameters.Add(currentParameter)

            insertCommand.Transaction = factory.BeginTransaction(IsolationLevel.ReadCommitted)

            Return insertCommand
        End Function

        Private Function CreateInsertImportSessionInfoCommand(ByVal factory As DataFactory, ByVal transaction As SqlTransaction) As SqlCommand
            Dim command As SqlCommand = factory.GetDataCommand("InsertCompetitorImportInfo", transaction, True)
            Dim currentParameter As SqlParameter = New SqlParameter("CompetitorImportInfoID", SqlDbType.Int, 0, "CompetitorImportInfoID")

            currentParameter.Direction = ParameterDirection.Output

            With command.Parameters
                .Add(currentParameter)
                .Add(New SqlParameter("CompetitorImportSessionID", SqlDbType.Int, 0, "CompetitorImportSessionID"))
                .Add(New SqlParameter("Item_Key", SqlDbType.Int, 0, "Item_Key"))
                .Add(New SqlParameter("CompetitorID", SqlDbType.Int, 0, "CompetitorID"))
                .Add(New SqlParameter("CompetitorLocationID", SqlDbType.Int, 0, "CompetitorLocationID"))
                .Add(New SqlParameter("CompetitorStoreID", SqlDbType.Int, 0, "CompetitorStoreID"))
                .Add(New SqlParameter("FiscalYear", SqlDbType.SmallInt, 0, "FiscalYear"))
                .Add(New SqlParameter("FiscalPeriod", SqlDbType.SmallInt, 0, "FiscalPeriod"))
                .Add(New SqlParameter("PeriodWeek", SqlDbType.SmallInt, 0, "PeriodWeek"))
                .Add(New SqlParameter("Competitor", SqlDbType.VarChar, 50, "Competitor"))
                .Add(New SqlParameter("Location", SqlDbType.VarChar, 50, "Location"))
                .Add(New SqlParameter("CompetitorStore", SqlDbType.VarChar, 50, "CompetitorStore"))
                .Add(New SqlParameter("UPCCode", SqlDbType.VarChar, 50, "UPCCode"))
                .Add(New SqlParameter("Description", SqlDbType.VarChar, 50, "Description"))
                .Add(New SqlParameter("Size", SqlDbType.Decimal, 5, "Size"))
                .Add(New SqlParameter("UnitOfMeasure", SqlDbType.VarChar, 50, "UnitOfMeasure"))
                .Add(New SqlParameter("PriceMultiple", SqlDbType.Int, 0, "PriceMultiple"))
                .Add(New SqlParameter("Price", SqlDbType.Money, 0, "Price"))
                .Add(New SqlParameter("SaleMultiple", SqlDbType.Int, 0, "SaleMultiple"))
                .Add(New SqlParameter("Sale", SqlDbType.Money, 0, "Sale"))
                .Add(New SqlParameter("DateChecked", SqlDbType.DateTime, 0, "DateChecked"))
            End With

            Return command
        End Function

        Private Function CreateUpdateImportSessionInfoCommand(ByVal factory As DataFactory, ByVal transaction As SqlTransaction) As SqlCommand
            Dim command As SqlCommand = factory.GetDataCommand("UpdateCompetitorImportInfo", transaction, True)

            With command.Parameters
                .Add(New SqlParameter("CompetitorImportInfoID", SqlDbType.Int, 0, "CompetitorImportInfoID"))
                .Add(New SqlParameter("CompetitorImportSessionID", SqlDbType.Int, 0, "CompetitorImportSessionID"))
                .Add(New SqlParameter("Item_Key", SqlDbType.Int, 0, "Item_Key"))
                .Add(New SqlParameter("CompetitorID", SqlDbType.Int, 0, "CompetitorID"))
                .Add(New SqlParameter("CompetitorLocationID", SqlDbType.Int, 0, "CompetitorLocationID"))
                .Add(New SqlParameter("CompetitorStoreID", SqlDbType.Int, 0, "CompetitorStoreID"))
                .Add(New SqlParameter("FiscalYear", SqlDbType.SmallInt, 0, "FiscalYear"))
                .Add(New SqlParameter("FiscalPeriod", SqlDbType.SmallInt, 0, "FiscalPeriod"))
                .Add(New SqlParameter("PeriodWeek", SqlDbType.SmallInt, 0, "PeriodWeek"))
                .Add(New SqlParameter("Competitor", SqlDbType.VarChar, 50, "Competitor"))
                .Add(New SqlParameter("Location", SqlDbType.VarChar, 50, "Location"))
                .Add(New SqlParameter("CompetitorStore", SqlDbType.VarChar, 50, "CompetitorStore"))
                .Add(New SqlParameter("UPCCode", SqlDbType.VarChar, 50, "UPCCode"))
                .Add(New SqlParameter("Description", SqlDbType.VarChar, 50, "Description"))
                .Add(New SqlParameter("Size", SqlDbType.Decimal, 5, "Size"))
                .Add(New SqlParameter("Unit_ID", SqlDbType.Int, 0, "Unit_ID"))
                .Add(New SqlParameter("PriceMultiple", SqlDbType.Int, 0, "PriceMultiple"))
                .Add(New SqlParameter("Price", SqlDbType.Money, 0, "Price"))
                .Add(New SqlParameter("SaleMultiple", SqlDbType.Int, 0, "SaleMultiple"))
                .Add(New SqlParameter("Sale", SqlDbType.Money, 0, "Sale"))
                .Add(New SqlParameter("DateChecked", SqlDbType.DateTime, 0, "DateChecked"))
            End With

            Return command
        End Function

        Private Function CreateDeleteImportSessionInfoCommand(ByVal factory As DataFactory, ByVal transaction As SqlTransaction) As SqlCommand
            Dim command As SqlCommand = factory.GetDataCommand("DeleteCompetitorImportInfo", transaction, True)

            command.Parameters.Add(New SqlParameter("CompetitorImportInfoID", SqlDbType.Int, 0, "CompetitorImportInfoID"))

            Return command
        End Function

        Private Function CreateListCommand(ByVal factory As DataFactory, ByVal importSessionID As Integer) As SqlCommand
            Dim command As SqlCommand = factory.GetDataCommand("GetCompetitorImportSessionByCompetitorImportSessionID", Nothing, True)
            Dim param As SqlParameter = New SqlParameter("CompetitorImportSessionID", SqlDbType.Int)

            param.Value = importSessionID

            command.Parameters.Add(param)

            Return command
        End Function

        Private Function GetInfoRow(ByRef dataSet As CompetitorStoreDataSet, ByVal importSession As CompetitorStoreDataSet.CompetitorImportSessionRow, ByVal rowsToUpdate As DataRow(), ByVal i As Integer) As CompetitorStoreDataSet.CompetitorImportInfoRow
            Dim infoRow As CompetitorStoreDataSet.CompetitorImportInfoRow = dataSet.CompetitorImportInfo(i)

            infoRow.CompetitorImportSessionRow = importSession

            Return infoRow
        End Function

        Private Sub ImportCompetitorImportInfoRows(ByRef dataSet As CompetitorStoreDataSet, ByVal factory As DataFactory, _
            ByVal importSession As CompetitorStoreDataSet.CompetitorImportSessionRow, ByVal transaction As SqlTransaction, _
            ByRef success As Boolean, ByVal adapter As SqlDataAdapter, ByVal rowsPerBatch As Integer)

            ' Send one row at a time to show granular update dialog
            Dim rowsToUpdate(rowsPerBatch) As DataRow

            ' Reset the insert commmand
            adapter.InsertCommand = Nothing

            ' Use the update command because the data loaded from the Excel sheet through ODBC
            ' will create rows with a RowState of "Unchanged." When the session row is assigned
            ' the state will change to "Modified," which is resolved with the update command
            adapter.UpdateCommand = CreateInsertImportSessionInfoCommand(factory, transaction)

            ' For now make this an all or nothing operation. This may change.
            Try
                Dim infoRowCount As Integer = dataSet.CompetitorImportInfo.Rows.Count

                For i As Integer = 0 To infoRowCount - 1
                    rowsToUpdate(0) = GetInfoRow(dataSet, importSession, rowsToUpdate, i)

                    For j As Integer = 1 To rowsPerBatch - 1
                        i += 1

                        If i = infoRowCount Then
                            i -= 1
                            Exit For
                        End If

                        rowsToUpdate(j) = GetInfoRow(dataSet, importSession, rowsToUpdate, i)
                    Next

                    adapter.Update(rowsToUpdate)

                    RaiseEvent CompetitorImportInfoRowSaved(i + 1)
                Next
            Catch ex As Exception
                success = False
            End Try
        End Sub

#End Region

#Region "Events and Delegates"
        Public Delegate Sub RowSavedEventHandler(ByVal index As Integer)
        Public Event CompetitorImportInfoRowSaved As RowSavedEventHandler
#End Region

        Public Sub MatchIdentifiers(ByVal competitorImportSessionID As Integer, ByVal dataSet As CompetitorStoreDataSet)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim params As New ArrayList

            params.Add(New DBParam("CompetitorImportSessionID", DBParamType.Int, competitorImportSessionID))

            factory.ExecuteStoredProcedure("UpdateCompetitorImportInfoWithIdentifiers", params)

            factory.OpenConnection()

            ' Update the in-memory rows with the new values
            Using adapter As SqlDataAdapter = factory.GetDataAdapter(CreateListCommand(factory, competitorImportSessionID), Nothing)
                adapter.Fill(dataSet.CompetitorImportInfo)
            End Using
        End Sub

        ''' <summary>
        ''' Imports all competitor data in the given CompetitorStoreDataSet
        ''' </summary>
        ''' <param name="dataSet"></param>
        ''' <param name="userId"></param>
        ''' <returns>Successful import</returns>
        Public Function ImportCompetitorData(ByRef dataSet As CompetitorStoreDataSet, ByVal userId As Integer, ByRef currentSession As CompetitorStoreDataSet.CompetitorImportSessionRow, Optional ByVal rowsPerBatch As Integer = 1) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim transaction As SqlTransaction
            Dim success As Boolean = True

            currentSession = dataSet.CompetitorImportSession.NewCompetitorImportSessionRow()
            currentSession.User_ID = userId
            ' This value will differ from that in the database
            currentSession.StartDateTime = DateTime.Now

            dataSet.CompetitorImportSession.AddCompetitorImportSessionRow(currentSession)

            Using adapter As SqlDataAdapter = factory.GetDataAdapter(String.Empty, Nothing)
                adapter.InsertCommand = CreateInsertImportSessionDataCommand(factory)

                transaction = adapter.InsertCommand.Transaction

                Try
                    success = adapter.Update(dataSet.CompetitorImportSession) = 1
                Catch ex As Exception
                    success = False
                End Try

                If success Then
                    ImportCompetitorImportInfoRows(dataSet, factory, currentSession, transaction, success, adapter, rowsPerBatch)
                End If
            End Using

            If (success) Then
                transaction.Commit()
            Else
                transaction.Rollback()
            End If

            Return success
        End Function

        Public Sub Save(ByRef dataSet As CompetitorStoreDataSet)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim command As SqlCommand = Nothing
            Dim transaction As SqlTransaction = Nothing

            Using adapter As SqlDataAdapter = factory.GetDataAdapter(command, transaction)
                adapter.InsertCommand = CreateInsertImportSessionInfoCommand(factory, transaction)
                adapter.UpdateCommand = CreateUpdateImportSessionInfoCommand(factory, transaction)
                adapter.DeleteCommand = CreateDeleteImportSessionInfoCommand(factory, transaction)

                adapter.Update(dataSet.CompetitorImportInfo)
            End Using
        End Sub

        Public Function GetExistingRowCount(ByVal competitorImportSession As CompetitorStoreDataSet.CompetitorImportSessionRow) As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Return CInt(factory.ExecuteScalar(String.Format("GetCompetitorImportInfoExistingCount @CompetitorImportSessionID = {0}", competitorImportSession.CompetitorImportSessionID)))
        End Function

    End Class
End Namespace