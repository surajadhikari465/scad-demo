Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports WholeFoods.IRMA.EPromotions.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.EPromotions.DataAccess

    Public Class ItemGroupMemberDAO

        Sub New()
            'Constructor
        End Sub

        ''' <summary>
        '''  Check to see if a Group name already exists.        
        ''' </summary>
        ''' <param name="NewGroupName">String representing the new Item Group name</param>
        ''' <returns>
        ''' FALSE, if a duplicate group already exists.
        ''' TRUE, if no duplicate group name already exists.
        ''' </returns>
        ''' <remarks></remarks>
        Public Function ValidateNewGroupName(ByVal NewGroupName As String, ByVal DeletedGroupIds As String) As Boolean

            Dim retval As Boolean = False
            Dim factory As DataFactory = New DataFactory(DataFactory.ItemCatalog)

            Dim paramList As ArrayList = New ArrayList()
            Dim currentParam As DBParam

            If DeletedGroupIds = "" Then
                DeletedGroupIds = "-999"
            End If

            currentParam = New DBParam
            currentParam.Name = "GroupName"
            currentParam.Value = NewGroupName.Replace("'", "''")
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "DeletedGroupIds"
            currentParam.Value = DeletedGroupIds
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            Dim Result As DataSet = factory.GetStoredProcedureDataSet("EPromotions_ValidateGroupName", paramList)

            If Result.Tables(0).Rows(0)("isValid").ToString().Equals("true") Then retval = True
            Result.Dispose()

            Return retval

        End Function

        Public Sub DeleteItemGroup(ByRef GroupID As Integer)
            Dim factory As DataFactory = New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As ArrayList = New ArrayList()
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "@GroupId"
            currentParam.Value = GroupID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("EPromotions_RemoveItemGroup", paramList)

        End Sub

        Public Function CreateNewGroup(ByRef NewGroupName As String, ByVal GroupLogic As ItemGroup_GroupLogic) As Integer
            Dim factory As DataFactory = New DataFactory(DataFactory.ItemCatalog)
            Dim NewId As Integer
            Dim paramList As ArrayList = New ArrayList()
            Dim currentParam As DBParam
            Dim ds As DataSet = New DataSet


            currentParam = New DBParam
            currentParam.Name = "GroupName"
            currentParam.Value = NewGroupName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "GroupLogic"
            currentParam.Value = GroupLogic
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UserId"
            currentParam.Value = giUserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ds = factory.GetStoredProcedureDataSet("EPromotions_CreateNewItemGroup", paramList)
            NewId = CType(ds.Tables(0).Rows(0)("NewId"), Integer)
            ds.Dispose()
            'NewId = CType(factory.ExecuteScalar("EPromotions_CreateNewItemGroup '" & NewGroupName.Replace("'", "''") & "'," & GroupLogic & "," & giUserID), Integer)
            Return NewId

        End Function

        Public Sub RemoveItemFromGroup(ByVal OfferId As Integer, ByVal GroupId As Integer, ByVal ItemKey As Integer)
            Dim factory As DataFactory = New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As ArrayList = New ArrayList()
            Dim currentParam As DBParam

            Dim SqlTran As SqlTransaction = factory.BeginTransaction(IsolationLevel.ReadCommitted)
            Try
                currentParam = New DBParam
                currentParam.Name = "OfferId"
                currentParam.Value = OfferId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "GroupId"
                currentParam.Value = GroupId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ItemKey"
                currentParam.Value = ItemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "UserId"
                currentParam.Value = giUserID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                factory.ExecuteStoredProcedure("EPromotions_DeleteItemFromGroup", paramList, SqlTran)
                SqlTran.Commit()
            Catch ex As Exception
                SqlTran.Rollback()
                'error 
                Debug.WriteLine(ex.InnerException.Message)
            Finally
                SqlTran.Dispose()
            End Try

        End Sub
        Public Sub AddItemToGroup(ByVal OfferId As Integer, ByVal GroupId As Integer, ByVal ItemKey As Integer)
            Dim factory As DataFactory = New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As ArrayList = New ArrayList()
            Dim currentParam As DBParam

            Dim SqlTran As SqlTransaction = factory.BeginTransaction(IsolationLevel.ReadCommitted)
            Try

                currentParam = New DBParam
                currentParam.Name = "OfferId"
                currentParam.Value = OfferId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "GroupId"
                currentParam.Value = GroupId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ItemKey"
                currentParam.Value = ItemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "UserId"
                currentParam.Value = giUserID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)


                factory.ExecuteStoredProcedure("EPromotions_AddItemToGroup", paramList, SqlTran)
                SqlTran.Commit()
            Catch ex As Exception
                SqlTran.Rollback()
                'error 
                Debug.WriteLine(ex.InnerException.Message)
            Finally
                SqlTran.Dispose()
            End Try

        End Sub

        Public Function IsGroupInCurrentPromotion(ByVal GroupId As Integer, ByVal OfferId As Integer) As Boolean
            Dim factory As DataFactory = New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As ArrayList = New ArrayList()
            Dim currentParam As DBParam
            Dim ds As DataSet = New DataSet
            Dim retval As Boolean

            currentParam = New DBParam
            currentParam.Name = "GroupId"
            currentParam.Value = GroupId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OfferId"
            currentParam.Value = OfferId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ds = factory.GetStoredProcedureDataSet("EPromotions_IsGroupInCurrentPromotion", paramList)
            If ds.Tables(0).Rows(0)("IsGroupInCurrentPromotion").ToString().Equals("True") Then
                retval = True
            Else
                retval = False
            End If
            ds.Dispose()
            Return retval
        End Function
        Public Function GetGroupEditStatus(ByVal GroupId As Integer) As String
            Dim factory As DataFactory = New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As ArrayList = New ArrayList()
            Dim currentParam As DBParam
            Dim ds As DataSet = New DataSet
            Dim retval As String
            Dim EditedUserId As String
            Dim IsEdited As Boolean
            Dim EditedBy As String


            currentParam = New DBParam
            currentParam.Name = "GroupId"
            currentParam.Value = GroupId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ds = factory.GetStoredProcedureDataSet("EPromotions_GetGroupEditStatus", paramList)

            With ds.Tables(0).Rows
                If .Item(0)("IsEdited").ToString().Equals("Yes") Then IsEdited = True Else IsEdited = False
                EditedBy = .Item(0)("EditedBy").ToString()
                If Not .Item(0)("EditUserId").ToString().Equals("") Then EditedUserId = .Item(0)("EditUserId").ToString() Else EditedUserId = ""
            End With
            ds.Dispose()

            If IsEdited Then
                If giUserID <> Int32.Parse(EditedUserId) Then
                    retval = EditedBy
                Else
                    retval = ""
                End If
            Else
                retval = ""
            End If

            Return retval

        End Function


        Public Sub UnlockGroup(ByVal GroupId As Integer)
            Dim factory As DataFactory = New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As ArrayList = New ArrayList()
            Dim currentParam As DBParam
            Dim results As ArrayList = New ArrayList

            currentParam = New DBParam
            currentParam.Name = "GroupId"
            currentParam.Value = GroupId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.ExecuteStoredProcedure("Epromotions_UnlockGroup", paramList)
        End Sub
        Public Sub SetGroupEditStatus(ByVal GroupId As Integer, ByVal IsEdited As Boolean)
            Dim factory As DataFactory = New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As ArrayList = New ArrayList()
            Dim currentParam As DBParam
            Dim ds As DataSet = New DataSet
            Dim Status As Integer = 0
            Dim resutls As ArrayList = New ArrayList


            If IsEdited Then Status = 1 Else Status = 0
            currentParam = New DBParam
            currentParam.Name = "GroupId"
            currentParam.Value = GroupId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)


            currentParam = New DBParam
            currentParam.Name = "Status"
            currentParam.Value = IIf(IsEdited, giUserID, -99)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            resutls = factory.ExecuteStoredProcedure("Epromotions_SetGroupEditStatus", paramList)

        End Sub



        Public Function InsertOrUpdateGroupData(ByVal isInsert As Boolean, ByRef ItemGroup As ItemGroupBO, Optional ByRef transaction As SqlTransaction = Nothing) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim success As Boolean = True

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Groupid"
                currentParam.Type = DBParamType.Int
                If Not isInsert Then
                    ' OUTPUT parameters are indicated by having no value - INSERT populates this with the new row ID
                    currentParam.Value = ItemGroup.GroupID
                End If
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "GroupName"
                currentParam.Type = DBParamType.String
                currentParam.Value = ItemGroup.GroupName
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "GroupLogic"
                currentParam.Value = ItemGroup.GroupLogic
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "CreateDate"
                currentParam.Value = Now
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ModifiedDate"
                currentParam.Value = Now
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "User_id"
                currentParam.Value = giUserID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)


                'Execute the stored procedure 
                If isInsert Then
                    factory.ExecuteStoredProcedure("EPromotions_InsertGroupData", paramList)
                Else
                    factory.ExecuteStoredProcedure("EPromotions_UpdateGroupData", paramList)
                End If
            Catch ex As Exception
                success = False
                MsgBox("Error during " & CStr(IIf(isInsert, "insert", "update")) & " of Group data: " & ex.Message, MsgBoxStyle.Critical, "ItemGroupMemberDAO:InsertOrUpdateGroupData")
            End Try

            Return success

        End Function





        Public Function GetGroupItems(ByRef ItemGroupId As Integer) As BindingList(Of ItemGroupMemberBO)
            Dim factory As DataFactory = New DataFactory(DataFactory.ItemCatalog)
            Dim returnValue As BindingList(Of ItemGroupMemberBO) = New BindingList(Of ItemGroupMemberBO)
            Dim paramList As ArrayList = New ArrayList()
            Dim results As SqlClient.SqlDataReader = Nothing
            Dim currentParam As DBParam

            Try
                currentParam = New DBParam
                currentParam.Name = "GroupId"
                currentParam.Value = ItemGroupId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("EPromotions_GetGroupItems", paramList)

                While results.Read
                    Dim item As ItemGroupMemberBO = New ItemGroupMemberBO()

                    If Not results.GetSqlValue(results.GetOrdinal("Group_Id")) Is Nothing Then
                        item.GroupId = results.GetInt32(results.GetOrdinal("Group_Id"))
                    End If

                    If Not results.GetSqlValue(results.GetOrdinal("Item_key")) Is Nothing Then
                        item.ItemKey = results.GetInt32(results.GetOrdinal("Item_key"))
                    End If
                    If Not results.GetValue(results.GetOrdinal("ModifiedDate")) Is DBNull.Value Then
                        item.ModifiedDate = results.GetDateTime(results.GetOrdinal("ModifiedDate"))
                    End If
                    If Not results.GetValue(results.GetOrdinal("user_id")) Is DBNull.Value Then
                        item.Userid = results.GetInt32(results.GetOrdinal("user_id"))
                    End If
                    If Not results.GetValue(results.GetOrdinal("Identifier")) Is Nothing Then
                        item.ItemIdentifier = results.GetString(results.GetOrdinal("Identifier"))
                    End If
                    If Not results.GetSqlValue(results.GetOrdinal("Item_Description")) Is Nothing Then
                        item.ItemDesc = results.GetString(results.GetOrdinal("Item_Description"))
                    End If

                    If Not results.GetValue(results.GetOrdinal("OfferChgTypeId")) Is Nothing Then
                        Select Case CInt(results.GetValue(results.GetOrdinal("OfferChgTypeId")))
                            Case 1
                                item.Status = BusinessLogic.Common.BO_IRMABase.OfferChangeType.[New]
                            Case 2
                                item.Status = BusinessLogic.Common.BO_IRMABase.OfferChangeType.Add
                            Case 3
                                item.Status = BusinessLogic.Common.BO_IRMABase.OfferChangeType.Delete
                        End Select
                    End If

                    returnValue.Add(item)

                End While

            Catch ex As Exception
                If ex.InnerException Is Nothing Then
                    Debug.WriteLine(ex.Message)
                Else
                    Debug.WriteLine(ex.InnerException.Message)
                End If
            Finally
                If Not results Is Nothing Then
                    results.Close()
                End If
            End Try

            Return returnValue
        End Function
    End Class
End Namespace
