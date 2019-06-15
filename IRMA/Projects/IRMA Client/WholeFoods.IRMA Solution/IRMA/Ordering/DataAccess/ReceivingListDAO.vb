Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility
Imports log4net

Namespace WholeFoods.IRMA.Ordering.DataAccess

    Public Class ReceivingListDAO
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


        Public Function GetOrderReceivingDisplayInfo(ByRef glOrderHeaderID As Integer) As DataTable
            logger.Debug("GetOrderReceivingDisplayInfo entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = glOrderHeaderID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.CommandTimeout = 1200
                results = factory.GetStoredProcedureDataTable("GetOrderReceivingDisplayInfo", paramList)

            Catch ex As Exception
                Throw ex
            End Try

            Return results

        End Function
        Public Function GetRefusedItemList(ByRef glOrderHeaderID As Integer) As DataTable
            logger.Debug("GetRefusedItemList entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = glOrderHeaderID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.CommandTimeout = 1200
                results = factory.GetStoredProcedureDataTable("GetOrderItemsRefused", paramList)

            Catch ex As Exception
                Throw ex
            End Try

            Return results

        End Function

        Public Function GetReceivingList(ByRef glOrderHeaderID As Integer) As DataTable
            logger.Debug("GetReceivingList entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = glOrderHeaderID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.CommandTimeout = 1200
                results = factory.GetStoredProcedureDataTable("GetReceivingList", paramList)

            Catch ex As Exception
                Throw ex
            End Try

            Return results

        End Function

        Public Function GetReceivingListForNOIDNORD(ByRef glOrderHeaderID As Integer) As DataTable
            logger.Debug("GetReceivingListForNOIDNORD entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = glOrderHeaderID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.CommandTimeout = 1200
                results = factory.GetStoredProcedureDataTable("GetReceivingListForNOIDNORD", paramList)

            Catch ex As Exception
                Throw ex
            End Try

            Return results

        End Function

        Public Sub ReceiveOrderItem(ByRef OrderLineID As Integer, ByRef Quantity As String, ByRef Weight As String, ByVal RecvDiscrepancyReasonId As Integer)
            logger.Debug("ReceiveOrderItem entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As ArrayList = Nothing
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "OrderItem_ID"
                currentParam.Value = OrderLineID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "DateReceived"
                currentParam.Value = SystemDateTime()
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Quantity"
                If Quantity.ToString = "NULL" Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = Quantity.ToString
                End If
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Weight"
                If Weight.ToString = "NULL" Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = Weight.ToString
                End If
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "RecvDiscrepancyReasonId"
                If RecvDiscrepancyReasonId = -1 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = RecvDiscrepancyReasonId
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = giUserID
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                'factory.CommandTimeout = 1200
                results = factory.ExecuteStoredProcedure("ReceiveOrderItem4", paramList)

            Catch ex As Exception
                Throw ex
            End Try

        End Sub

		Public Sub SavePastReceiptDate(ByRef glOrderHeaderID As Integer, ByVal pastReceiptDate As DateTime)
			logger.Debug("SavePastReceiptDate entry")
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim results As ArrayList = Nothing
			Dim currentParam As DBParam
			Dim paramList As New ArrayList

			Try
				' setup parameters for stored proc
				currentParam = New DBParam
				currentParam.Name = "OrderHeader_ID"
				currentParam.Value = glOrderHeaderID
				currentParam.Type = DBParamType.Int
				paramList.Add(currentParam)

				currentParam = New DBParam
				currentParam.Name = "PastReceiptDate"
				currentParam.Value = pastReceiptDate
				currentParam.Type = DBParamType.DateTime
				paramList.Add(currentParam)

				' Execute the stored procedure 
				'factory.CommandTimeout = 1200
				results = factory.ExecuteStoredProcedure("SavePastReceiptDate", paramList)

			Catch ex As Exception
				Throw ex
			End Try

		End Sub
	End Class

End Namespace