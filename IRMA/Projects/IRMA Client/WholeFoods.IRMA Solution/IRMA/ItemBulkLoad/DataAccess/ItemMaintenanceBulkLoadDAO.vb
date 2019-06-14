Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ItemBulkLoad.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemBulkLoad.DataAccess

    Public Class ItemMaintenanceBulkLoadDAO

        Public Function UpdateItemAndPrice(ByVal itemBO As ItemMaintenanceBulkLoadBO) As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim itemKeyList As New ArrayList

            Try
                ' setup identifier for stored proc
                currentParam = New DBParam
                currentParam.Name = "Identifier"
                currentParam.Type = DBParamType.String
                If String.IsNullOrEmpty(itemBO.ItemIdentifier) Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = itemBO.ItemIdentifier
                End If
                paramList.Add(currentParam)

                ' setup POS Description for stored proc
                currentParam = New DBParam
                currentParam.Name = "POS_Description"
                currentParam.Type = DBParamType.String
                If String.IsNullOrEmpty(itemBO.PosDescription) Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = itemBO.PosDescription
                End If
                paramList.Add(currentParam)

                ' setup Item Description for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Description"
                currentParam.Type = DBParamType.String
                If String.IsNullOrEmpty(itemBO.ItemDescription) Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = itemBO.ItemDescription
                End If
                paramList.Add(currentParam)

                ' setup Discontinued for stored proc
                currentParam = New DBParam
                currentParam.Name = "Discontinue_Item"
                currentParam.Type = DBParamType.Bit
                If itemBO.DiscontinueItem = -1 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = itemBO.DiscontinueItem
                End If
                paramList.Add(currentParam)

                ' setup Employee Discountable for stored proc
                currentParam = New DBParam
                currentParam.Name = "Discountable"
                currentParam.Type = DBParamType.Bit
                If itemBO.Discountable = -1 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = itemBO.Discountable
                End If
                paramList.Add(currentParam)

                ' setup Food Stamps for stored proc
                currentParam = New DBParam
                currentParam.Name = "Food_Stamps"
                currentParam.Type = DBParamType.Bit
                If itemBO.FoodStamps = -1 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = itemBO.FoodStamps
                End If
                paramList.Add(currentParam)

                ' setup National Class for stored proc
                currentParam = New DBParam
                currentParam.Name = "NatClassID"
                currentParam.Type = DBParamType.Int
                currentParam.Value = itemBO.NationalClassID
                paramList.Add(currentParam)

                ' setup Tax Class for stored proc
                currentParam = New DBParam
                currentParam.Name = "TaxClassID"
                currentParam.Type = DBParamType.Int
                If itemBO.TaxClassId = 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = itemBO.TaxClassId
                End If
                paramList.Add(currentParam)

                ' setup Restricted Hours for stored proc
                currentParam = New DBParam
                currentParam.Name = "Restricted_Hours"
                currentParam.Type = DBParamType.Bit
                If itemBO.RestrictedHours = -1 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = itemBO.RestrictedHours
                End If
                paramList.Add(currentParam)

                ' Get the output value
                currentParam = New DBParam
                currentParam.Name = "ItemKey"
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute Stored Procedure to Create Price Batch Detail records for the price change
                itemKeyList = factory.ExecuteStoredProcedure("UpdateItemInfoForBulkLoad", paramList)

            Catch ex As Exception
                Throw ex
            End Try

            Return CInt(itemKeyList(0))

        End Function

        ' Function to return a count of items for the passed in identifier
        Public Function GetItemFromIdentifier(ByVal Identifier As String) As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim sql As String
            Dim cnt As Integer

            sql = "exec GetItemInfoByIdentifier '" & Identifier & "'"

            cnt = CInt(factory.ExecuteScalar(sql))

            Return cnt
        End Function

        ' Function that returns a list of possible tax class values
        Public Function GetTaxClass() As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim taxClass As TaxClassBO
            Dim taxClassList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetTaxClasses")

                While results.Read
                    taxClass = New TaxClassBO()
                    taxClass.TaxClassID = results.GetInt32(results.GetOrdinal("TaxClassId"))
                    taxClass.TaxClassDesc = results.GetString(results.GetOrdinal("TaxClassDesc"))

                    taxClassList.Add(taxClass)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return taxClassList
        End Function

        ' Function that returns a list of possible national class values
        Public Function GetStoreList() As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim storeList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetRetailStores")

                While results.Read
                    storeList.Add(results.GetInt32(results.GetOrdinal("Store_No")))
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return storeList
        End Function

        ' Function that returns a list of possible national class values
        Public Function GetNationalClass() As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim nationalClass As NationalClassBO
            Dim nationalClassList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetNatClass")

                While results.Read
                    nationalClass = New NationalClassBO()
                    nationalClass.ClassID = results.GetInt32(results.GetOrdinal("ClassId"))
                    nationalClass.ClassName = results.GetString(results.GetOrdinal("ClassName"))

                    nationalClassList.Add(nationalClass)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return nationalClassList
        End Function
        ' Function to return the error message for an error id
        Public Function GetErrorMessages(ByVal id As Integer) As DataSet
            'Dim factory As New DataFactory(DataFactory.ItemCatalog)
            'Dim sql As String
            'Dim errorMsgSet As DataSet

            'sql = "exec GetItemInfoByIdentifier '" & id & "'"

            'errorMsg = CStr(factory.ExecuteScalar(sql))

            Return Nothing
        End Function

        ''' <summary>
        ''' Read complete list of Import Types data and return ArrayList of ImportTypeBO objects
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetImportTypeList() As ArrayList
            Dim importTypeList As New ArrayList
            Dim importTypeBO As ImportTypeBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetItemUploadTypes")

                While results.Read
                    importTypeBO = New ImportTypeBO()
                    importTypeBO.ItemUploadTypeID = results.GetInt32(results.GetOrdinal("ItemUploadType_ID"))
                    importTypeBO.Description = results.GetString(results.GetOrdinal("Description"))
                    importTypeList.Add(importTypeBO)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return importTypeList
        End Function


    ''' <summary>
    ''' Read complete list of Item Admin User data and return ArrayList of ItemAdminUserBO objects
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetItemAdminUserList() As ArrayList
      Dim itemAdminUserList As New ArrayList
      Dim factory As New DataFactory(DataFactory.ItemCatalog)
      Dim results As SqlDataReader = Nothing
      Dim ItemAdminUserBO = Nothing

      Try
        ' Execute the stored procedure 
        results = factory.GetStoredProcedureDataReader("GetItemAdminUsers")

        While results.Read
          ItemAdminUserBO = New ItemAdminUserBO()
          ItemAdminUserBO.UserID = CInt(results("User_ID"))
          ItemAdminUserBO.UserName = If(IsDBNull(results("FullName")), String.Format("Uknown {0}", CInt(results("User_ID")).ToString()), results("FullName"))
          itemAdminUserList.Add(ItemAdminUserBO)
        End While
      Finally
        If results IsNot Nothing Then
          results.Close()
        End If
      End Try

      Return itemAdminUserList
    End Function

    Public Function InsertItemUploadHeader(ByVal header As ItemUploadHeaderBO) As Integer

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim reader As New ArrayList
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim intItemUploadHeaderID As Integer

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ItemUploadType_ID"
                If header.ItemUploadTypeID <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = header.ItemUploadTypeID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ItemsProcessedCount"
                If header.ItemProcessedCount <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = header.ItemProcessedCount
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ItemsLoadedCount"
                If header.ItemLoadedCount <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = header.ItemLoadedCount
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ErrorsCount"
                If header.ErrorsCount <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = header.ErrorsCount
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "EmailToAddress"
                currentParam.Value = header.EmailToAddress
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "User_ID"
                If header.UserID <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = header.UserID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Get the output value
                currentParam = New DBParam
                currentParam.Name = "ItemUploadHeader_ID"
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure
                reader = factory.ExecuteStoredProcedure("InsertItemUploadHeader", paramList)

                intItemUploadHeaderID = CInt(reader(0))

            Catch ex As Exception
                'TODO handle exception
                Throw ex
            End Try

            Return intItemUploadHeaderID

        End Function

    End Class

End Namespace
