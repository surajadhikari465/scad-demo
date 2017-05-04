Option Explicit On
Option Strict On

Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports WholeFoods.IRMA.EPromotions.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports System.Collections

Namespace WholeFoods.IRMA.EPromotions.DataAccess
    Public Class PromotionOfferDAO

#Region "Enumerations"
        Public Enum EPromotionsUpdateStatus
            Success
            Fail
            ConcurrencyConflict
        End Enum
#End Region

        Public Sub AddStoreToPromotion(ByVal OfferId As Integer, ByVal StoreNo As Integer)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "OfferId"
            currentParam.Value = OfferId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StoreNo"
            currentParam.Value = StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)


            Try
                factory.ExecuteStoredProcedure("EPromotions_AddStoreToPromotion", paramList)
            Catch ex As Exception
                MsgBox("EPromotions_AddStoreToPromotion failed: " & ex.Message)
            End Try

        End Sub

        Public Sub RemoveStoreFromPromotion(ByVal OfferId As Integer, ByVal StoreNo As Integer)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "OfferId"
            currentParam.Value = OfferId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StoreNo"
            currentParam.Value = StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Try
                factory.ExecuteStoredProcedure("EPromotions_RemoveStoreFromPromotion", paramList)
            Catch ex As Exception
                MsgBox("EPromotions_RemoveStoreFromPromotion failed: " & ex.Message)
            End Try

        End Sub

        Public Function GetStoresByPricingMethod(ByVal PricingMethodId As Integer) As DataTable
            Dim StoreData As DataTable = New DataTable("StoreData")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim StoreRow As DataRow
            Dim results As SqlDataReader = Nothing

            StoreData.Columns.Add(New DataColumn("Store_No", GetType(String)))
            StoreData.Columns.Add(New DataColumn("Store_name", GetType(String)))


            currentParam = New DBParam
            currentParam.Name = "PricingMethodId"
            currentParam.Value = PricingMethodId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Try
                results = factory.GetStoredProcedureDataReader("EPromotions_GetStoresByPricingMethod", paramList)

                While results.Read
                    StoreRow = StoreData.NewRow()
                    StoreRow("Store_No") = results("Store_No").ToString()
                    StoreRow("Store_Name") = results("Store_Name").ToString()
                    StoreData.Rows.Add(StoreRow)
                End While
            Catch e As Exception
                MessageBox.Show("Could not load the stores available for this pricing method.")
            Finally
                results.Close()
            End Try

            Return StoreData

        End Function

        Public Function ValidatePromotionName(ByVal PromotionName As String) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As DataSet = New DataSet()
            Dim retval As Boolean = False


            currentParam = New DBParam
            currentParam.Name = "PromotionName"
            currentParam.Value = PromotionName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            results = factory.GetStoredProcedureDataSet("EPromotions_ValidatePromotionName", paramList)
            If results.Tables(0).Rows(0)("IsValid").ToString().Equals("True") Then
                retval = True
            Else
                retval = False
            End If
            results.Dispose()

            Return retval

        End Function

        Public Function GetStoresByPromotionId(ByVal OfferId As Integer) As DataTable
            Dim StoreData As DataTable = New DataTable("StoreData")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim StoreRow As DataRow
            Dim results As SqlDataReader = Nothing

            StoreData.Columns.Add(New DataColumn("Store_No", GetType(String)))
            StoreData.Columns.Add(New DataColumn("Store_name", GetType(String)))
            StoreData.Columns.Add(New DataColumn("IsAssigned", GetType(Boolean)))
            StoreData.Columns.Add(New DataColumn("IsActive", GetType(Boolean)))


            currentParam = New DBParam
            currentParam.Name = "OfferId"
            currentParam.Value = OfferId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Try
                results = factory.GetStoredProcedureDataReader("EPromotions_GetStoresByPromotionId", paramList)

                While results.Read
                    StoreRow = StoreData.NewRow()
                    StoreRow("Store_No") = results("Store_No").ToString()
                    StoreRow("Store_Name") = results("Store_Name").ToString()
                    If results("IsAssigned").ToString().Equals("YES") Then
                        StoreRow("IsAssigned") = True
                        StoreRow("IsActive") = False
                    ElseIf results("IsAssigned").ToString().Equals("ACTIVE") Then
                        StoreRow("IsAssigned") = True
                        StoreRow("IsActive") = True
                    Else
                        StoreRow("IsAssigned") = False
                        StoreRow("IsActive") = False
                    End If
                    StoreData.Rows.Add(StoreRow)
                End While
            Catch e As Exception
                MessageBox.Show("Could not load the stores available for this promotion.")
            Finally
                If Not results Is Nothing Then
                    results.Close()
                End If
            End Try

            Return StoreData

        End Function

        Public Function GetOfferIdFromPriceBatchDetailRecord(ByVal PriceBatchDetailId As String) As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim sql As String
            Dim OfferId As Integer

            sql = "exec EPromotions_GetOfferIdFromPriceBatchDetailId " & PriceBatchDetailId


            OfferId = CInt(factory.ExecuteScalar(sql))
            Return OfferId
        End Function

        Public Function ShowLockedPromotions(ByVal OfferId As Integer) As DataSet
            Dim ds As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "OfferId"
            currentParam.Value = OfferId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ds = factory.GetStoredProcedureDataSet("Epromotions_ShowLockedPromotions", paramList)

            Return ds

        End Function



        Public Function ShowLockedGroups(ByVal OfferId As Integer) As DataSet
            Dim ds As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "OfferId"
            currentParam.Value = OfferId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ds = factory.GetStoredProcedureDataSet("Epromotions_ShowLockedGroups", paramList)
            Return ds
        End Function


        ''' <summary>
        ''' Return a list of Promotional Offer data based on Pricing Method
        ''' </summary>
        ''' <param name="PricingMethodId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPromotionalOfferListByMethod(ByVal PricingMethodId As Integer) As PromotionOfferBOList
            Dim offerList As New PromotionOfferBOList
            Dim offerBO As PromotionOfferBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Try
                ' setup parameters for stored proc;  0 returns all values
                Dim paramList As New ArrayList
                Dim currentParam As DBParam

                currentParam = New DBParam
                currentParam.Name = "PricingMethodId"
                currentParam.Value = PricingMethodId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("EPromotions_GetPromotionalOffersByPricingMethod", paramList)

                While results.Read

                    offerBO = New PromotionOfferBO()
                    With offerBO
                        ' set loading flag to prevent Data State changes
                        .Loading = True

                        If (Not results.IsDBNull(results.GetOrdinal("Offer_ID"))) Then
                            .PromotionOfferID = results.GetInt32(results.GetOrdinal("Offer_ID"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("CreateDate"))) Then
                            .CreateDate = results.GetDateTime(results.GetOrdinal("CreateDate"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("Description"))) Then
                            .Desc = results.GetString(results.GetOrdinal("Description"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("ReferenceCode"))) Then
                            .ReferenceCode = results.GetString(results.GetOrdinal("ReferenceCode"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("TaxClass_ID"))) Then
                            .TaxClassID = results.GetInt32(results.GetOrdinal("TaxClass_ID"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("SubTeam_No"))) Then
                            .SubTeamNo = results.GetInt32(results.GetOrdinal("SubTeam_No"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("EndDate"))) Then
                            .EndDate = results.GetDateTime(results.GetOrdinal("EndDate"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("ModifiedDate"))) Then
                            .ModifiedDate = results.GetDateTime(results.GetOrdinal("ModifiedDate"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("PricingMethod_ID"))) Then
                            .PriceMethodID = results.GetSqlByte(results.GetOrdinal("PricingMethod_ID")).Value
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("RewardAmount"))) Then
                            .RewardAmount = results.GetFloat(results.GetOrdinal("RewardAmount"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("RewardGroupID"))) Then
                            .RewardGroupID = results.GetInt32(results.GetOrdinal("RewardGroupID"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("RewardType"))) Then
                            .RewardID = results.GetInt32(results.GetOrdinal("RewardType"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("RewardQuantity"))) Then
                            .RewardQty = results.GetDecimal(results.GetOrdinal("RewardQuantity"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("StartDate"))) Then
                            .StartDate = results.GetDateTime(results.GetOrdinal("StartDate"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("User_ID"))) Then
                            .UserID = results.GetInt32(results.GetOrdinal("User_ID"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("IsEdited"))) Then
                            .IsEditing = results.GetInt32(results.GetOrdinal("IsEdited"))
                        End If

                        .MarkClean()
                        .Loading = False
                    End With

                    offerList.Add(offerBO)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return offerList


        End Function


        ''' <summary>
        ''' Read complete list of Promotional Offer data and return ArrayList of PromotionOfferBO objects
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPromotionalOfferList(Optional ByVal OfferId As Integer = -1) As PromotionOfferBOList
            Dim offerList As New PromotionOfferBOList
            Dim offerBO As PromotionOfferBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing

            Try
                ' setup parameters for stored proc;  0 returns all values
                Dim paramList As New ArrayList
                Dim currentParam As DBParam

                currentParam = New DBParam
                currentParam.Name = "OfferId"
                currentParam.Value = OfferId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("EPromotions_GetPromotionalOffers", paramList)

                While results.Read

                    offerBO = New PromotionOfferBO()
                    With offerBO
                        ' set loading flag to prevent Data State changes
                        .Loading = True

                        If (Not results.IsDBNull(results.GetOrdinal("Offer_ID"))) Then
                            .PromotionOfferID = results.GetInt32(results.GetOrdinal("Offer_ID"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("CreateDate"))) Then
                            .CreateDate = results.GetDateTime(results.GetOrdinal("CreateDate"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("Description"))) Then
                            .Desc = results.GetString(results.GetOrdinal("Description"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("EndDate"))) Then
                            .EndDate = results.GetDateTime(results.GetOrdinal("EndDate"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("ModifiedDate"))) Then
                            .ModifiedDate = results.GetDateTime(results.GetOrdinal("ModifiedDate"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("PricingMethod_ID"))) Then
                            .PriceMethodID = results.GetSqlByte(results.GetOrdinal("PricingMethod_ID")).Value
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("RewardAmount"))) Then
                            .RewardAmount = results.GetDecimal(results.GetOrdinal("RewardAmount"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("RewardGroupID"))) Then
                            .RewardGroupID = results.GetInt32(results.GetOrdinal("RewardGroupID"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("RewardType"))) Then
                            .RewardID = results.GetInt32(results.GetOrdinal("RewardType"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("RewardQuantity"))) Then
                            .RewardQty = results.GetDecimal(results.GetOrdinal("RewardQuantity"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("StartDate"))) Then
                            .StartDate = results.GetDateTime(results.GetOrdinal("StartDate"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("User_ID"))) Then
                            .UserID = results.GetInt32(results.GetOrdinal("User_ID"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("ReferenceCode"))) Then
                            .ReferenceCode = results.GetString(results.GetOrdinal("ReferenceCode"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("TaxClass_ID"))) Then
                            .TaxClassID = results.GetInt32(results.GetOrdinal("TaxClass_ID"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("SubTeam_No"))) Then
                            .SubTeamNo = results.GetInt32(results.GetOrdinal("SubTeam_No"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("IsEdited"))) Then
                            .IsEditing = results.GetInt32(results.GetOrdinal("IsEdited"))
                        End If

                        .MarkClean()
                        .Loading = False
                    End With

                    offerList.Add(offerBO)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return offerList
        End Function

        ''' <summary>
        ''' Returns datatable of Promotional Offers who include the specified GroupID
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPromotionalOffersByGroupID(ByVal GroupId As Integer) As DataTable
            Dim OfferData As DataTable = New DataTable("OfferData")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim OfferRow As DataRow
            Dim results As SqlDataReader = Nothing


            Try
                OfferData.Columns.Add(New DataColumn("Offer_ID", GetType(Integer)))
                OfferData.Columns.Add(New DataColumn("Description", GetType(String)))
                OfferData.Columns.Add(New DataColumn("PricingMethod_ID", GetType(Integer)))
                OfferData.Columns.Add(New DataColumn("StartDate", GetType(Date)))
                OfferData.Columns.Add(New DataColumn("EndDate", GetType(Date)))
                OfferData.Columns.Add(New DataColumn("RewardType", GetType(Integer)))
                OfferData.Columns.Add(New DataColumn("RewardQuantity", GetType(Integer)))
                OfferData.Columns.Add(New DataColumn("RewardAmount", GetType(Decimal)))
                OfferData.Columns.Add(New DataColumn("RewardGroupID", GetType(Integer)))
                OfferData.Columns.Add(New DataColumn("createdate", GetType(Date)))
                OfferData.Columns.Add(New DataColumn("modifieddate", GetType(Date)))
                OfferData.Columns.Add(New DataColumn("User_ID", GetType(Integer)))
                OfferData.Columns.Add(New DataColumn("ReferenceCode", GetType(String)))
                OfferData.Columns.Add(New DataColumn("TaxClass_ID", GetType(Integer)))
                OfferData.Columns.Add(New DataColumn("SubTeam_No", GetType(Integer)))
                OfferData.Columns.Add(New DataColumn("IsEdited", GetType(Integer)))


                currentParam = New DBParam
                currentParam.Name = "GroupId"
                currentParam.Value = GroupId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("EPromotions_GetOffersByGroupID", paramList)

                While results.Read
                    OfferRow = OfferData.NewRow()
                    OfferRow("Offer_ID") = results("Offer_ID").ToString()
                    OfferRow("Description") = results("Description").ToString()
                    OfferRow("PricingMethod_ID") = results("PricingMethod_ID").ToString()
                    OfferRow("StartDate") = results("StartDate").ToString()
                    OfferRow("EndDate") = results("EndDate").ToString()
                    OfferRow("RewardType") = results("RewardType").ToString()
                    OfferRow("RewardQuantity") = results("RewardQuantity").ToString()
                    OfferRow("RewardAmount") = results("RewardAmount").ToString()
                    OfferRow("RewardGroupID") = results("RewardGroupID").ToString()
                    OfferRow("createdate") = results("createdate").ToString()
                    OfferRow("modifieddate") = results("modifieddate").ToString()
                    If (Not results.IsDBNull(results.GetOrdinal("User_ID"))) Then
                        OfferRow("User_ID") = results("User_ID").ToString()
                    End If
                    OfferRow("ReferenceCode") = results("ReferenceCode").ToString()
                    OfferRow("TaxClass_ID") = results("TaxClass_ID").ToString()
                    OfferRow("SubTeam_No") = results("SubTeam_No").ToString()
                    OfferRow("isEdited") = results("isEdited").ToString()

                    OfferData.Rows.Add(OfferRow)
                End While



            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "PromotionOfferDAO:GetPromotionalOffersByGroupID")
                OfferData = Nothing
            Finally
                If Not results Is Nothing Then
                    results.Close()
                End If
            End Try
            Return OfferData
        End Function

        ''' <summary>
        ''' Returns a dataset populatefd with Promotional Offer information
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPromotionalOffers(Optional ByVal OfferId As Integer = -1) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' setup parameters for stored proc;  0 returns all values
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try

                currentParam = New DBParam
                currentParam.Name = "OfferId"
                currentParam.Value = OfferId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                Return factory.GetStoredProcedureDataSet("EPromotions_GetPromotionalOffers", paramList)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "PromotionOfferDAO:GetPromotionalOffers")
                Return Nothing
            End Try

        End Function

        ''' <summary>
        ''' Returns a dataset populated with pending Price Batch Details associated with the given OfferID
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPendingPriceBatchDetailsByOfferID(ByVal OfferId As Integer) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' setup parameters for stored proc;  0 returns all values
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "OfferId"
            currentParam.Value = OfferId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("EPromotions_GetPendingPriceBatchDetailsByOfferID", paramList)

        End Function

        ''' <summary>
        ''' Read the complete list of Promotional Offer objects, optionally filter by ID.
        ''' </summary>
        ''' <exception cref="DataFactoryException" />
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPromotionalOfferMembersList(Optional ByVal OfferID As Integer = -1, Optional ByVal Logic As Integer = -1, Optional ByVal Reward As Boolean = False) As PromotionOfferMemberBOList
            Dim offerMemberList As New PromotionOfferMemberBOList
            Dim offerMemberBO As PromotionOfferMemberBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing

            Try
                ' setup parameters for stored proc;  0 returns all values
                Dim paramList As New ArrayList
                Dim currentParam As DBParam

                currentParam = New DBParam
                currentParam.Name = "OfferId"
                currentParam.Value = OfferID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("EPromotions_GetPromotionalOfferMembers", paramList)

                While results.Read

                    offerMemberBO = New PromotionOfferMemberBO()
                    With offerMemberBO
                        If (Not results.IsDBNull(results.GetOrdinal("OfferMember_ID"))) Then
                            .OfferMemberID = results.GetInt32(results.GetOrdinal("OfferMember_ID"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("Offer_ID"))) Then
                            .OfferID = results.GetInt32(results.GetOrdinal("Offer_ID"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("GroupID"))) Then
                            .GroupID = results.GetInt32(results.GetOrdinal("GroupID"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("Quantity"))) Then
                            .Quantity = results.GetInt32(results.GetOrdinal("Quantity"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("Purpose"))) Then
                            .Purpose = CType(results.GetByte(results.GetOrdinal("Purpose")), PromotionOfferMemberPurpose)
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("JoinLogic"))) Then
                            .JoinLogic = CType(results.GetByte(results.GetOrdinal("JoinLogic")), PromotionOfferMemberJoinLogic)
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("Modified"))) Then
                            .Modified = results.GetDateTime(results.GetOrdinal("Modified"))
                        End If
                        If (Not results.IsDBNull(results.GetOrdinal("User_ID"))) Then
                            .UserID = results.GetInt32(results.GetOrdinal("User_ID"))
                        End If

                        .MarkClean()
                    End With

                    offerMemberList.Add(offerMemberBO)

                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return offerMemberList
        End Function

        ''' <summary>
        ''' Return all Item Groups that are available to add to a Promotion Offer.
        ''' 
        ''' </summary>
        ''' <param name="Promotion"></param>
        ''' <returns>All Item Groups that are not already included in the Promotion</returns>
        ''' <remarks></remarks>

        Public Function GetAvailableGroupsForPromotion(ByVal Promotion As PromotionOfferBO, ByVal Purpose As PromotionOfferMemberPurpose) As BindingList(Of ItemGroupBO)
            Dim groupList As New BindingList(Of ItemGroupBO)
            Dim groupBO As ItemGroupBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataSet = Nothing

            'Try
            ' setup parameters for stored proc;  0 returns all values
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "OfferId"
            currentParam.Value = Promotion.PromotionOfferID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Purpose"
            currentParam.Value = Purpose
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataSet("EPromotions_GetAvailableItemGroups", paramList)

            For Each dr As DataRow In results.Tables(0).Rows

                groupBO = New ItemGroupBO()
                With groupBO

                    If (Not IsDBNull(dr("Group_ID"))) Then
                        .GroupID = CInt(dr("Group_ID"))
                    End If
                    If (Not IsDBNull(dr("GroupName"))) Then

                        .GroupName = CStr(dr("GroupName"))
                    End If

                    If Not IsDBNull(dr("GroupLogic")) Then
                        .GroupLogic = CType(dr("GroupLogic"), ItemGroup_GroupLogic)
                    End If

                    If Not IsDBNull(dr("CreateDate")) Then
                        .CreateDate = CDate(dr("CreateDate"))
                    End If

                    If Not IsDBNull(dr("ModifiedDate")) Then
                        .ModifiedDate = CDate(dr("ModifiedDate"))
                    End If

                    If Not IsDBNull(dr("User_ID")) Then
                        .UserID = CInt(dr("User_ID"))
                    End If


                    .MarkClean()
                End With

                groupList.Add(groupBO)


            Next

            results.Dispose()

            Return groupList

        End Function

        ''' <summary>
        ''' Returns a dataset populatefd with Promotional Offer information
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAssociatedPriceBatchDetailCount(ByVal OfferId As Integer, Optional ByVal FilterNullHeaderIDs As Boolean = False) As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim outputList As ArrayList

            Try
                ' setup parameters for stored proc;  0 returns all values
                Dim paramList As New ArrayList
                Dim currentParam As DBParam
                Dim outputIDX As Integer

                currentParam = New DBParam
                currentParam.Name = "OfferId"
                currentParam.Value = OfferId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "FilterNullHeaderIDs"
                currentParam.Value = FilterNullHeaderIDs
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Count"
                currentParam.Type = DBParamType.Int
                ' store index of output parameter
                outputIDX = paramList.Add(currentParam)

                ' Execute the stored procedure 
                outputList = factory.ExecuteStoredProcedure("EPromotions_GetPriceBatchDetailCountByOfferID", paramList)

                If outputList.Count > 0 Then
                    Return CType(outputList(0), Integer)
                Else
                    MsgBox("No output value returned from EPromotions_GetPriceBatchDetailCountByOfferID. Returning value of 1.", MsgBoxStyle.Critical, "PromotionalOffer:GetAssociatedPriceBatchDetailCount")
                    Return 1
                End If

            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "PromotionOfferDAO:GetAsscoiatedPriceBatchDetailCount")
                Return 0
            End Try

        End Function


        ''' <summary>
        ''' Returns a dataset populatefd with Promotional Offer information
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPendingPriceBatchDetailCount(ByVal OfferId As Integer, Optional ByVal StoreId As Integer = 0) As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim outputList As ArrayList

            Try
                ' setup parameters for stored proc;  0 returns all values
                Dim paramList As New ArrayList
                Dim currentParam As DBParam
                Dim outputIDX As Integer

                currentParam = New DBParam
                currentParam.Name = "OfferId"
                currentParam.Value = OfferId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Count"
                currentParam.Type = DBParamType.Int
                ' store index of output parameter
                outputIDX = paramList.Add(currentParam)

                ' Execute the stored procedure 
                outputList = factory.ExecuteStoredProcedure("EPromotions_ReturnPendingPriceBatchDetailCount", paramList)

                If outputList.Count > 0 Then
                    Return CType(outputList(0), Integer)
                Else
                    MsgBox("No output value returned from EPromotions_ReturnPendingPriceBatchDetailCount. Returning value of 1.", MsgBoxStyle.Critical, "PromotionalOffer:GetPendingPriceBatchDetailCount")
                    Return 1
                End If

            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "PromotionOfferDAO:GetPendingPriceBatchDetailCount")
                Return 0
            End Try

        End Function

        ''' <summary>
        ''' Returns the value of the active flag for a given offer/store pair
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetStoreActiveFlag(ByVal OfferId As Integer, ByVal StoreId As Integer) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim outputList As ArrayList

            Try
                ' setup parameters for stored proc;  0 returns all values
                Dim paramList As New ArrayList
                Dim currentParam As DBParam
                Dim outputIDX As Integer

                currentParam = New DBParam
                currentParam.Name = "OfferId"
                currentParam.Value = OfferId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreId"
                currentParam.Value = StoreId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Active"
                currentParam.Type = DBParamType.Bit
                ' store index of output parameter
                outputIDX = paramList.Add(currentParam)

                ' Execute the stored procedure 
                outputList = factory.ExecuteStoredProcedure("EPromotions_ReturnStoreActiveFlag", paramList)

                If outputList.Count > 0 Then
                    Return CType(outputList(0), Boolean)
                Else
                    MsgBox("No output value returned from EPromotions_ReturnStoreActiveFlag. Returning value of True.", MsgBoxStyle.Critical, "PromotionalOffer:GetStoreActiveFlag")
                    Return True
                End If

            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "PromotionOfferDAO:GetStoreActiveFlag")
                Return True
            End Try

        End Function

        Public Function GetPromotionalGroupList(Optional ByVal GroupId As Integer = -1) As BindingList(Of ItemGroupBO)
            Dim groupList As New BindingList(Of ItemGroupBO)
            Dim groupBO As ItemGroupBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing

            Try
                ' setup parameters for stored proc;  0 returns all values
                Dim paramList As New ArrayList
                Dim currentParam As DBParam

                currentParam = New DBParam
                currentParam.Name = "GroupId"
                currentParam.Value = GroupId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("EPromotions_GetItemGroups", paramList)

                While results.Read

                    groupBO = New ItemGroupBO()
                    With groupBO
                        groupBO.Loading = True
                        If (Not results.IsDBNull(results.GetOrdinal("Group_ID"))) Then
                            .GroupID = results.GetInt32(results.GetOrdinal("Group_ID"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("GroupName"))) Then
                            .GroupName = Trim(results.GetString(results.GetOrdinal("GroupName")))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("GroupLogic"))) Then
                            Dim Logic As Integer = CType(results.GetSqlBoolean(results.GetOrdinal("GroupLogic")), ItemGroup_GroupLogic)
                            If Logic = -1 Then .GroupLogic = ItemGroup_GroupLogic.And Else .GroupLogic = ItemGroup_GroupLogic.Or
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("createdate"))) Then
                            .CreateDate = results.GetDateTime(results.GetOrdinal("createdate"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("ModifiedDate"))) Then
                            .ModifiedDate = results.GetDateTime(results.GetOrdinal("ModifiedDate"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("User_ID"))) Then
                            .UserID = results.GetInt32(results.GetOrdinal("User_ID"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("PromotionCount"))) Then
                            .PromotionCount = results.GetInt32(results.GetOrdinal("PromotionCount"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("ActivePromotionCount"))) Then
                            .ActivePromotionCount = results.GetInt32(results.GetOrdinal("ActivePromotionCount"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("PendingPromotionCount"))) Then
                            .PendingPromotionCount = results.GetInt32(results.GetOrdinal("PendingPromotionCount"))
                        End If

                        .MarkClean()
                        groupBO.Loading = False
                    End With

                    groupList.Add(groupBO)

                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return groupList
        End Function

        ''' <summary>
        ''' Insert Promotional Offer data
        ''' </summary>
        ''' <param name="offerBO"></param>
        ''' <remarks></remarks>
        Public Function InsertPromotionalOffer(ByRef offerBO As PromotionOfferBO, Optional ByRef transaction As SqlTransaction = Nothing) As EPromotionsUpdateStatus

            InsertPromotionalOffer = InsertOrUpdateOfferData(True, offerBO)
        End Function


        ''' <summary>
        ''' Update Promotional Offer data
        ''' </summary>
        ''' <param name="offerBO"></param>
        ''' <remarks></remarks>
        Public Function UpdatePromotionalOffer(ByRef offerBO As PromotionOfferBO, Optional ByRef transaction As SqlTransaction = Nothing) As EPromotionsUpdateStatus
            UpdatePromotionalOffer = InsertOrUpdateOfferData(False, offerBO)
        End Function

        ''' <summary>
        ''' build DBParams and call stored procedure to insert or update data based on flag
        ''' </summary>
        ''' <param name="isInsert"></param>
        ''' <param name="offerBO"></param>
        ''' <remarks></remarks>
        Private Function InsertOrUpdateOfferData(ByVal isInsert As Boolean, ByRef offerBO As PromotionOfferBO, Optional ByRef transaction As SqlTransaction = Nothing) As EPromotionsUpdateStatus
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim outputparamList As ArrayList
            Dim currentParam As DBParam
            Dim success As EPromotionsUpdateStatus = EPromotionsUpdateStatus.Success

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Description"
                currentParam.Value = offerBO.Desc
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PricingMethodID"
                currentParam.Value = offerBO.PriceMethodID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StartDate"
                currentParam.Value = offerBO.StartDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "EndDate"
                currentParam.Value = offerBO.EndDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "RewardType"
                currentParam.Value = offerBO.RewardID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "RewardQuantity"
                currentParam.Value = offerBO.RewardQty
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "RewardAmount"
                currentParam.Value = offerBO.RewardAmount
                currentParam.Type = DBParamType.Money
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "RewardGroupID"
                currentParam.Value = offerBO.RewardGroupID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "CreateDate"
                currentParam.Value = offerBO.CreateDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ModifiedDate"
                currentParam.Value = offerBO.ModifiedDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "UserID"
                currentParam.Value = offerBO.UserID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ReferenceCode"
                currentParam.Value = offerBO.ReferenceCode
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TaxClass_ID"
                currentParam.Value = offerBO.TaxClassID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "SubTeam_No"
                currentParam.Value = offerBO.SubTeamNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "OfferID"
                currentParam.Type = DBParamType.Int
                ' If Inserting, do not set value to indicate that it is an OUTPUT parameter
                currentParam.Value = IIf(isInsert, Nothing, offerBO.PromotionOfferID)
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                If isInsert Then
                    If transaction Is Nothing Then
                        outputparamList = factory.ExecuteStoredProcedure("EPromotions_InsertPromotionalOffer", paramList)
                    Else
                        outputparamList = factory.ExecuteStoredProcedure("EPromotions_InsertPromotionalOffer", paramList, transaction)
                    End If
                    'retreive newly assigned ID from parameter - assumes currentParam is still "OfferID"
                    offerBO.PromotionOfferID = CInt(CType(outputparamList(0), Integer))
                Else
                    If transaction Is Nothing Then
                        factory.ExecuteStoredProcedure("EPromotions_UpdatePromotionalOffer", paramList)
                    Else
                        factory.ExecuteStoredProcedure("EPromotions_UpdatePromotionalOffer", paramList, transaction)
                    End If
                End If
            Catch ex As Exception

                ' Detremine if it is a Concurrency Error or not
                ' TODO- Determine better way to handle this error trapping
                If Left(ex.InnerException.Message, 17) = "Concurrency Error" Then
                    success = EPromotionsUpdateStatus.ConcurrencyConflict
                    MsgBox(ex.InnerException.Message & " The data will be refreshed to reflect the current state.", MsgBoxStyle.Critical, "PromotionOfferDAO:InsertOrUpdateOfferData")
                Else
                    success = EPromotionsUpdateStatus.Fail
                    MsgBox("Error during " & CStr(IIf(isInsert, "insert", "update")) & " of Promotion Offer data: " & ex.Message, MsgBoxStyle.Critical, "PromotionOfferDAO:InsertOrUpdateOfferData")
                End If
            End Try

            Return success

        End Function

        ''' <summary>
        ''' Insert Promotional Offer Member data
        ''' </summary>
        ''' <param name="offerMemberBO"></param>
        ''' <remarks></remarks>
        Public Function InsertPromotionalOfferMember(ByRef offerMemberBO As PromotionOfferMemberBO, Optional ByRef transaction As SqlTransaction = Nothing) As EPromotionsUpdateStatus
            InsertPromotionalOfferMember = InsertOrUpdateOfferMemberData(True, offerMemberBO)
        End Function

        ''' <summary>
        ''' Update Promotional Offer Member data
        ''' </summary>
        ''' <param name="offerMemberBO"></param>
        ''' <remarks></remarks>
        Public Function UpdatePromotionalOfferMember(ByRef offerMemberBO As PromotionOfferMemberBO, Optional ByRef transaction As SqlTransaction = Nothing) As EPromotionsUpdateStatus
            UpdatePromotionalOfferMember = InsertOrUpdateOfferMemberData(False, offerMemberBO)
        End Function
        ''' <summary>
        ''' build DBParams and call stored procedure to insert or update data based on flag
        ''' </summary>
        ''' <param name="isInsert"></param>
        ''' <param name="offerMemberBO"></param>
        ''' <remarks></remarks>
        Private Function InsertOrUpdateOfferMemberData(ByVal isInsert As Boolean, ByRef offerMemberBO As PromotionOfferMemberBO, Optional ByRef transaction As SqlTransaction = Nothing) As EPromotionsUpdateStatus
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim success As EPromotionsUpdateStatus = EPromotionsUpdateStatus.Success

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "OfferMemberID"
                currentParam.Type = DBParamType.Int
                If Not isInsert Then
                    ' OUTPUT parameters are indicated by having no value - INSERT populates this with the new row ID
                    currentParam.Value = offerMemberBO.OfferMemberID
                End If
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "OfferID"
                currentParam.Type = DBParamType.Int
                currentParam.Value = offerMemberBO.OfferID
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "GroupID"
                currentParam.Value = offerMemberBO.GroupID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Quantity"
                currentParam.Value = offerMemberBO.Quantity
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Purpose"
                currentParam.Value = offerMemberBO.Purpose
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "JoinLogic"
                currentParam.Value = offerMemberBO.JoinLogic
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Modified"
                currentParam.Value = offerMemberBO.Modified
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "UserID"
                currentParam.Value = giUserID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                If isInsert Then
                    factory.ExecuteStoredProcedure("EPromotions_InsertPromotionalOfferMember", paramList)
                Else
                    factory.ExecuteStoredProcedure("EPromotions_UpdatePromotionalOfferMember", paramList)
                End If
            Catch ex As Exception
                ' Detremine if it is a Concurrency Error or not
                ' TODO- Determine better way to handle this error trapping
                If Left(ex.InnerException.Message, 17) = "Concurrency Error" Then
                    success = EPromotionsUpdateStatus.ConcurrencyConflict
                    MsgBox(ex.InnerException.Message & " The data will be refreshed to reflect the current state.", MsgBoxStyle.Critical, "PromotionOfferDAO:InsertOrUpdateOfferData")
                Else
                    success = EPromotionsUpdateStatus.Fail
                    MsgBox("Error during " & CStr(IIf(isInsert, "insert", "update")) & " of Promotion Offer data: " & ex.Message, MsgBoxStyle.Critical, "PromotionOfferDAO:InsertOrUpdateOfferData")
                End If

            End Try

            Return success

        End Function

        Public Sub DeleteOffer(ByVal offerID As Integer, ByRef transaction As SqlTransaction)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "offerID"
                currentParam.Value = offerID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("EPromotions_DeletePromotionalOffer", paramList, transaction)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "PromotionOfferDAO:DeleteOffer")
            End Try
        End Sub

        Public Sub SetOfferEditFlag(ByVal offerID As Integer, ByVal IsEditing As Integer, Optional ByRef transaction As SqlTransaction = Nothing)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "offerID"
                currentParam.Value = offerID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Value"
                currentParam.Value = IsEditing
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("EPromotions_PromotionalOffer_SetEditFlag", paramList, transaction)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "PromotionOfferDAO:SetOfferEditFlag")
            End Try
        End Sub


        Public Function DeleteOfferMember(ByVal offerMemberID As Integer, ByRef transaction As SqlTransaction) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim success As Boolean = True

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "OfferMemberID"
                currentParam.Value = offerMemberID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("EPromotions_DeletePromotionalOfferMembers", paramList, transaction)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "PromotionOfferDAO:DeleteOfferMember")
                success = False
            End Try

            Return success
        End Function

        Public Function GetTransaction() As SqlTransaction
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim transaction As SqlTransaction = factory.BeginTransaction(IsolationLevel.ReadCommitted)

            Return transaction
        End Function


    End Class
End Namespace
