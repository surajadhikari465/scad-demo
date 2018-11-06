Imports WholeFoods.IRMA.EPromotions.BusinessLogic.Common
Imports WholeFoods.IRMA.EPromotions.DataAccess
Imports System.Text
Imports System.Collections.Generic
Imports System.ComponentModel   ' Need for BindingList 
Imports Infragistics.Win.UltraWinGrid

#Region "Enumerations"

Public Enum PromotionOfferStatus
    Valid
    Error_Required_Description
    Error_Required_PricingMethod
    Error_Required_RewardType
    Error_Required_ReferenceCode
    Error_Required_LossVatType
    Error_Required_LossDeptCode
    Error_Invalid_StartEndDate
    Error_ItemAssociated        ' only set when date range is current
    Error_ThreeItemsRequired    ' used for Three of a Group Pricing Method; requiremnets have to add to 3 items
    Error_TwoGroupsRequired     ' for two group offer
    Error_ThreeGroupsRequired   ' for three group offer
    Error_Unspecified           ' run-time error thrown 
    Error_Required_OfferRequirements
    Error_Duplicate_PromotionName
    Error_Invalid_StartDate
    Error_Delete_AssociatedStores
    Error_Delete_UnprocessedBatchDetails
    Error_RewardValue
End Enum

Public Enum PricingMethods As Short
    RegularPromo = 0
    BOGO = 1
    BuyTwoGetOneFree = 2
    ThreeOfARange = 3
    TwoGroup = 4
    ThreeGroup = 5
    BuyThreeGetCheapestFree = 6
End Enum


#End Region

Namespace WholeFoods.IRMA.EPromotions.BusinessLogic
    Public Class PromotionOfferBO
        Inherits BO_IRMABase
        ' Class: PromotionOfferBO
        ' Description: Provides storage and business functions for Promotional Offer data.
        ' Created: 17 April 06

#Region "Constructors"

        ''' <summary>
        ''' constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            With Me

                .PromotionOfferID = 0
                .StartDate = Now
                .EndDate = Now
                .CreateDate = Now
                .ModifiedDate = Now
                .RewardAmount = 0

                .RewardQty = 0
                .MarkNew()
            End With
        End Sub

        ''' <summary>
        ''' Create a new instance of the object from a current UltraGridRow
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal currentRow As UltraGridRow)

            Me.Desc = currentRow.Cells("Desc").Value.ToString

            If currentRow.Cells("PriceMethodID").Value IsNot DBNull.Value Then
                Me.PriceMethodID = CType(currentRow.Cells("PriceMethodID").Value, Byte)
            End If

            If currentRow.Cells("StartDate").Value IsNot DBNull.Value Then
                Me.StartDate = CType(currentRow.Cells("StartDate").Value, Date)
            End If

            If currentRow.Cells("EndDate").Value IsNot DBNull.Value Then
                Me.EndDate = CType(currentRow.Cells("EndDate").Value, Date)
            End If

            If currentRow.Cells("RewardID").Value IsNot DBNull.Value Then
                Me.RewardID = CType(currentRow.Cells("RewardID").Value, Integer)
            End If

            If currentRow.Cells("RewardQty").Value IsNot DBNull.Value Then
                Me.RewardQty = CType(currentRow.Cells("RewardQty").Value, Decimal)
            End If

            If currentRow.Cells("RewardAmount").Value IsNot DBNull.Value Then
                Me.RewardAmount = CType(currentRow.Cells("RewardAmount").Value, Decimal)
            End If

            If currentRow.Cells("RewardGroupID").Value IsNot DBNull.Value Then
                Me.RewardGroupID = CType(currentRow.Cells("RewardGroupID").Value, Integer)
            End If

            If currentRow.Cells("CreateDate").Value IsNot DBNull.Value Then
                Me.CreateDate = CType(currentRow.Cells("CreateDate").Value, Date)
            End If

            If currentRow.Cells("ModifiedDate").Value IsNot DBNull.Value Then
                Me.ModifiedDate = CType(currentRow.Cells("ModifiedDate").Value, Date)
            End If

            If currentRow.Cells("UserID").Value IsNot DBNull.Value Then
                Me.UserID = CType(currentRow.Cells("UserID").Value, Integer)
            End If

        End Sub


#End Region

#Region "Property Definitions"
        Private _offerID As Integer
        Public Property PromotionOfferID() As Integer
            Get
                Return _offerID
            End Get
            Set(ByVal value As Integer)
                _offerID = value
            End Set
        End Property

        Private _offerDescription As String = ""
        Public Property Desc() As String
            Get
                Return _offerDescription.TrimEnd
            End Get
            Set(ByVal value As String)
                If value <> _offerDescription Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "Desc")
                    _offerDescription = value
                End If
            End Set
        End Property

        Private _offerPriceMethodID As Integer = 0
        Public Property PriceMethodID() As Integer
            Get
                Return _offerPriceMethodID
            End Get
            Set(ByVal value As Integer)
                If value <> _offerPriceMethodID Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "PriceMethodID")
                    _offerPriceMethodID = value
                End If
            End Set
        End Property

        Private _offerStartDate As Date
        Public Property StartDate() As Date
            Get
                Return _offerStartDate
            End Get
            Set(ByVal value As Date)
                If value <> _offerStartDate Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "StartDate")
                    _offerStartDate = value.Date
                End If
            End Set
        End Property

        Private _offerEndDate As Date
        Public Property EndDate() As Date
            Get
                Return _offerEndDate
            End Get
            Set(ByVal value As Date)
                If value <> _offerEndDate Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "EndDate")
                    _offerEndDate = value.Date
                End If
            End Set
        End Property

        Private _offerRewardID As Integer = 0
        Public Property RewardID() As Integer
            Get
                Return _offerRewardID
            End Get
            Set(ByVal value As Integer)
                If value <> _offerRewardID Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "RewardID")
                    _offerRewardID = value
                End If
            End Set
        End Property

        Private _offerRewardQty As Decimal = 0
        Public Property RewardQty() As Decimal
            Get
                Return _offerRewardQty
            End Get
            Set(ByVal value As Decimal)
                If value <> _offerRewardQty Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "RewardQty")
                    _offerRewardQty = value
                End If
            End Set
        End Property

        Private _offerRewardAmount As Single = 0
        Public Property RewardAmount() As Single
            Get
                Return _offerRewardAmount
            End Get
            Set(ByVal value As Single)
                If value <> _offerRewardAmount Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "RewardAmount")
                    _offerRewardAmount = value
                End If
            End Set
        End Property

        Private _offerRewardGroupID As Integer = 0
        Public Property RewardGroupID() As Integer
            Get
                Return _offerRewardGroupID
            End Get
            Set(ByVal value As Integer)
                If value <> _offerRewardGroupID Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "RewardGroupID")
                    _offerRewardGroupID = value
                End If
            End Set
        End Property

        ''' <summary>
        ''' Create Date should only be set when communicating with the database - this value is used for concurrency checks
        ''' </summary>
        ''' <remarks></remarks>
        Private _offerCreateDate As Date
        Public Property CreateDate() As Date
            Get
                Return _offerCreateDate
            End Get
            Set(ByVal value As Date)
                If value <> _offerCreateDate Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "CreateDate")
                    _offerCreateDate = value
                End If
            End Set
        End Property

        Private _DescriptionChanged As Boolean = False
        Public Property DescriptionChanged() As Boolean
            Get
                Return _DescriptionChanged
            End Get
            Set(ByVal value As Boolean)
                _DescriptionChanged = value
            End Set
        End Property

        ''' <summary>
        ''' Modified Date should only be set when communicating with the database - this value is used for concurrency checks
        ''' </summary>
        ''' <remarks></remarks>
        Private _offerModifiedDate As Date
        Public Property ModifiedDate() As Date
            Get
                Return _offerModifiedDate
            End Get
            Set(ByVal value As Date)
                If value <> _offerModifiedDate Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "ModifiedDate")
                    _offerModifiedDate = value
                End If
            End Set
        End Property

        Private _offerUserID As Integer = 0
        Public Property UserID() As Integer
            Get
                Return _offerUserID
            End Get
            Set(ByVal value As Integer)
                If value <> _offerUserID Then
                    MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "UserID")
                    _offerUserID = value
                End If
            End Set
        End Property

        Private _SubTeam_No As Integer
        Public Property SubTeamNo() As Integer
            Get
                Return _SubTeam_No
            End Get
            Set(ByVal value As Integer)
                MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "SubTeamNo")
                _SubTeam_No = value
            End Set
        End Property


        Private _TaxClass_Id As Integer
        Public Property TaxClassID() As Integer
            Get
                Return _TaxClass_Id
            End Get
            Set(ByVal value As Integer)
                MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "TaxClassId")
                _TaxClass_Id = value
            End Set
        End Property


        Private _ReferenceCode As String
        Public Property ReferenceCode() As String
            Get
                Return _ReferenceCode
            End Get
            Set(ByVal value As String)
                MyBase.DataStateChanged(EntityStateEnum.MODIFIED, "ReferenceCode")
                _ReferenceCode = value
            End Set
        End Property


        Private _isEditing As Integer
        Public Property IsEditing() As Integer
            Get
                Return _isEditing
            End Get
            Set(ByVal value As Integer)
                _isEditing = value
            End Set
        End Property

#End Region

#Region "Business Rules"
        ''' <summary>
        ''' validates data elements of current instance of PromotionOfferBO object
        ''' </summary>
        ''' <returns>ArrayList of PromotionOfferStatus values</returns>
        ''' <remarks></remarks>
        ''' 

        Public Function ValidatePromotionName(ByVal PromotionName As String) As Boolean
            Dim OfferDAO As PromotionOfferDAO = New PromotionOfferDAO
            Return OfferDAO.ValidatePromotionName(PromotionName)


        End Function


        Public Function IsPromotionLocked(ByVal OfferId As Integer, ByVal OfferDescription As String) As String
            Dim retval As StringBuilder = New StringBuilder()
            Dim OfferDAO As PromotionOfferDAO = New PromotionOfferDAO
            Dim NamesList As List(Of String) = New List(Of String)

            Dim ds As DataSet

            ds = OfferDAO.ShowLockedPromotions(OfferId)

            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    If Not NamesList.Contains(dr("FullName").ToString) Then
                        NamesList.Add(dr("FullName").ToString)
                    End If
                Next
            End If

            ds.Dispose()


            ds = OfferDAO.ShowLockedGroups(OfferId)

            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    If Not NamesList.Contains(dr("FullName").ToString) Then
                        NamesList.Add(dr("FullName").ToString)
                    End If
                Next
            End If

            If NamesList.Count > 0 Then
                retval.Append("""" & OfferDescription & """ or one of its groups is being edited by: " & vbCrLf)
                For Each User As String In NamesList
                    retval.Append(vbTab & User)
                Next
                retval.Append(vbCrLf)
            End If




            Return retval.ToString
        End Function

        Public Function GetOfferIdsFromPriceBatchDetail(ByVal PriceBatchDetailIdList As String, ByVal PriceBatchDetailIdSeperator As Char) As String
            Dim PBDList() As String = Split(PriceBatchDetailIdList, PriceBatchDetailIdSeperator)
            Dim PBDListEnumerator As IEnumerator = PBDList.GetEnumerator
            Dim OfferIdList As List(Of Integer) = New List(Of Integer)
            Dim retval As New StringBuilder


            Dim CurrentOfferId As Integer
            Dim OfferDAO As PromotionOfferDAO = New PromotionOfferDAO

            While PBDListEnumerator.MoveNext
                CurrentOfferId = OfferDAO.GetOfferIdFromPriceBatchDetailRecord(PBDListEnumerator.Current.ToString)
                If Not OfferIdList.Contains(CurrentOfferId) Then
                    OfferIdList.Add(CurrentOfferId)
                End If
            End While

            For Each offerid As Integer In OfferIdList
                retval.Append(offerid.ToString & ",")
            Next
            If retval.Length > 0 Then
                retval.Remove(retval.Length - 1, 1)
            End If
            Return retval.ToString
        End Function

        Public Function ValidateData() As ArrayList
            Dim statusList As New ArrayList

            ' Make sure all required fields are present
            If Me.Desc Is Nothing OrElse Len(Me.Desc.TrimEnd) = 0 Then
                statusList.Add(PromotionOfferStatus.Error_Required_Description)
            End If

            If Me.PriceMethodID = 0 Then
                statusList.Add(PromotionOfferStatus.Error_Required_PricingMethod)
            End If

            If Me.RewardID = 0 Then
                statusList.Add(PromotionOfferStatus.Error_Required_RewardType)
            End If

            If Me.SubTeamNo = 0 Then
                statusList.Add(PromotionOfferStatus.Error_Required_LossDeptCode)
            End If

            If Me.TaxClassID = 0 Then
                statusList.Add(PromotionOfferStatus.Error_Required_LossVatType)
            End If

            If Me.ReferenceCode Is Nothing OrElse Len(Me.ReferenceCode.TrimEnd) = 0 Then
                statusList.Add(PromotionOfferStatus.Error_Required_ReferenceCode)
            End If

            If _DescriptionChanged Then
                If Not ValidatePromotionName(Me.Desc) Then
                    statusList.Add(PromotionOfferStatus.Error_Duplicate_PromotionName)
                End If
            End If

            ' Start Date must not be later than End Date
            If Me.StartDate >= Me.EndDate Then
                statusList.Add(PromotionOfferStatus.Error_Invalid_StartEndDate)
            End If

            If Me.StartDate < DateTime.Now.Date Then
                statusList.Add(PromotionOfferStatus.Error_Invalid_StartDate)
            End If

            ' Validate Reward values
            Select Case Me.RewardID
                Case 1 ' Allowance
                    ' Amount must have value, no group is allowed
                    If Me.RewardAmount <= 0.0 Then
                        statusList.Add(PromotionOfferStatus.Error_RewardValue)
                    End If

                Case 2 ' Discount
                    ' Amount must have a value, no group is allowed
                    If Me.RewardAmount <= 0.0 Then
                        statusList.Add(PromotionOfferStatus.Error_RewardValue)
                    End If


                Case 3 ' Item
                    ' group must be defined

            End Select

            ' set valid status if no other status has ben assigned
            If statusList.Count = 0 Then
                statusList.Add(PromotionOfferStatus.Valid)
            End If

            Return statusList
        End Function

   
        Public Function ValidateDelete(ByVal OfferID As Integer, ByVal Unbatcheddetails As Boolean, ByVal UnprocessedDetails As Boolean, ByVal AssociatedStores As Boolean, ByVal ActiveStores As Integer) As PromotionOfferStatus
            Dim offerDAO As New PromotionOfferDAO
            Dim status As PromotionOfferStatus

            ' set default
            status = PromotionOfferStatus.Valid

            Try
                ' if there are batched but unprocessed details or unbatched details for stores who are active, disallow
                If (Unbatcheddetails And ActiveStores > 0) Or UnprocessedDetails Then
                    status = PromotionOfferStatus.Error_Delete_UnprocessedBatchDetails
                End If

                If status = PromotionOfferStatus.Valid Then
                    ' if there are associated stores, cannot delete
                    If AssociatedStores And (ActiveStores > 0) Then
                        status = PromotionOfferStatus.Error_Delete_AssociatedStores
                    End If
                End If


            Catch ex As Exception
                status = PromotionOfferStatus.Error_ItemAssociated
                MessageBox.Show(ex.Message, "PromotionOfferBO.ValidateDelete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try

            Return status
        End Function

        ''' <summary>
        ''' indicates if the Offer is associated with pending Price Batch detail records
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function BatchPending(ByVal OfferId As Integer) As Boolean
            Dim offerDAO As New PromotionOfferDAO
            Dim detailCount As Integer

            Try
                detailCount = offerDAO.GetPendingPriceBatchDetailCount(OfferId)

                ' If there are any rows returned, return true
                Return detailCount > 0
            Catch ex As Exception
                MessageBox.Show(ex.Message, "PromotionOfferBO.BatchPending", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return True
            End Try

        End Function

        ''' <summary>
        ''' indicates if there are any unbatched price details
        ''' </summary>
        ''' <param name="OfferId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function HasUnbatchedDetails(ByVal OfferId As Integer) As Boolean
            Dim offerDAO As New PromotionOfferDAO
            Dim detailCount As Integer

            Try
                detailCount = offerDAO.GetPendingPriceBatchDetailCount(OfferId)

                ' If there are any rows returned, return true
                Return detailCount > 0
            Catch ex As Exception
                MessageBox.Show(ex.Message, "PromotionOfferBO.BatchPending", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return True
            End Try

        End Function

        ''' <summary>
        ''' validates data elements of current instance of PromotionOfferBO object based on selected Pricing Method.
        ''' *** DO NOT USE*** Currently, Pricing Types may vary from region to region so no standard validation can
        ''' be performed.
        ''' </summary>
        ''' <returns>ArrayList of PromotionOfferStatus values</returns>
        ''' <remarks></remarks>
        Public Function ValidateForPricingMethod(ByVal priceMethod As PricingMethods, ByVal listRequirements As BindingList(Of PromotionOfferMemberBO), ByVal listRewards As BindingList(Of PromotionOfferMemberBO)) As ArrayList
            Dim succes As Boolean = True
            Dim statusList As New ArrayList
            Dim meetoneQuantity As Integer = 0
            Dim mandatoryQuantity As Integer = 0
            Dim meetoneCount As Integer = 0
            Dim mandatoryCount As Integer = 0
            Dim memberBO As PromotionOfferMemberBO
            Dim offerQuantityAlwaysThree As Boolean = True
            Dim mandatoryQuantityEqualsThree As Boolean = True
            Dim meetoneLowestQuantity As Integer = 0

            Try

                ' Calculate Quantities
                For Each memberBO In listRequirements
                    If memberBO.JoinLogic = PromotionOfferMemberJoinLogic.MeetOne Then
                        meetoneQuantity += memberBO.Quantity
                        meetoneCount += 1

                        ' check if this is the lowest quantity among Meet One requirements
                        ' if the current lowest quantity is zero, it means it has not been set
                        If (memberBO.Quantity < meetoneLowestQuantity) Or (meetoneLowestQuantity = 0) Then
                            meetoneLowestQuantity = 0
                        End If

                    ElseIf memberBO.JoinLogic = PromotionOfferMemberJoinLogic.Mandatory Then
                        mandatoryQuantity += memberBO.Quantity
                        mandatoryCount += 1
                    End If

                Next

                ' Check if three of a group requirement is met. We are checking to make sure that 
                ' the requirements as configured demand that three items be purchased.
                If (mandatoryQuantity + meetoneLowestQuantity) < 3 Then
                    offerQuantityAlwaysThree = False
                End If

                ' Perform validations based on Price Method
                Select Case priceMethod

                    Case PricingMethods.ThreeOfARange
                        ' Quantity must be at least three, larger numbers are allowed
                        If Not offerQuantityAlwaysThree Then
                            statusList.Add(PromotionOfferStatus.Error_ThreeItemsRequired)
                        End If

                    Case PricingMethods.TwoGroup
                        ' At Least one mandatory Group + at least on Meet One OR two Mandatory groups
                        If Not ((mandatoryCount >= 1 And meetoneCount >= 1) Or (mandatoryCount >= 2)) Then
                            statusList.Add(PromotionOfferStatus.Error_TwoGroupsRequired)
                        End If

                    Case PricingMethods.ThreeGroup
                        ' At Least two mandatory Group + at least on Meet One OR three Mandatory groups
                        If Not ((mandatoryCount >= 2 And meetoneCount >= 1) Or (mandatoryCount >= 3)) Then
                            statusList.Add(PromotionOfferStatus.Error_ThreeGroupsRequired)
                        End If

                    Case PricingMethods.BuyThreeGetCheapestFree
                        ' any reward limit? - make sure quantity is three (or mandatory's add to >= three)
                        If Not offerQuantityAlwaysThree Then
                            statusList.Add(PromotionOfferStatus.Error_ThreeItemsRequired)
                        End If

                    Case PricingMethods.RegularPromo, PricingMethods.BOGO, PricingMethods.BuyTwoGetOneFree
                        ' No specific validation 
                End Select

            Catch ex As Exception
                statusList.Add(PromotionOfferStatus.Error_Unspecified)

            End Try

            Return statusList

        End Function

#End Region

    End Class
End Namespace
