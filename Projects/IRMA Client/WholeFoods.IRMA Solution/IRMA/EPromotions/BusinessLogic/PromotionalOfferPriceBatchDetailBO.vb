Option Explicit On
Option Strict On

Imports System.ComponentModel   ' Need for BindingList 
Imports WholeFoods.IRMA.EPromotions.DataAccess
Imports WholeFoods.IRMA.Pricing.BusinessLogic
Imports WholeFoods.IRMA.Pricing.DataAccess
Imports WholeFoods.IRMA.EPromotions.BusinessLogic
Imports WholeFoods.IRMA.EPromotions.BusinessLogic.Common
Imports WholeFoods.Utility
Imports System.Data.SqlClient

Namespace WholeFoods.IRMA.EPromotions.BusinessLogic
    ''' <summary>
    ''' Manages updates to Price Batch Detail table based on changes to an offer
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PromotionalOfferPriceBatchDetailBO
        Inherits BO_IRMABase

#Region " Enums"

        Public Enum InitializeType
            SingleOffer
            GroupOwners
        End Enum

#End Region

#Region "Globals"
        Private _listOffer As ArrayList   ' array of PromotionalOfferUpdateBO, with one element for each Offer that needs to be processed
#End Region

#Region "Property Definitions"

#End Region

#Region "Constructors"
        ''' <summary>
        ''' empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub


#End Region

#Region "Private Methods"
        ''' <summary>
        ''' Insert IDs for every nonexpired Offer that uses the specified GroupID into the classes list of offers
        ''' </summary>
        ''' <param name="groupID"></param>
        ''' <remarks></remarks>
        Private Sub BuildOfferListByGroup(ByVal groupID As Integer)
            Dim tableOffers As DataTable
            Dim rowOffer As DataRow
            Dim offerDAO As New PromotionOfferDAO
            Dim offerBO As PromotionalOfferUpdateBO

            ' Get datatable of all Offers who have the specified GroupId as a member 
            tableOffers = offerDAO.GetPromotionalOffersByGroupID(groupID)

            ' Insert IDs for every Offer that uses the specified GroupID 
            For Each rowOffer In tableOffers.Rows

                ' add offer to list if not expired 
                If CType(rowOffer("EndDate"), Date) >= CType(Now(), Date) Then
                    offerBO = New PromotionalOfferUpdateBO
                    offerBO.OfferID = CType(rowOffer("Offer_ID"), Integer)
                    _listOffer.Add(offerBO)
                End If

            Next
        End Sub

        ''' <summary>
        ''' returns a arrayList of unique ItemGroupMemberBOs, on efor each unique item 
        ''' conatined in the current offer
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function BuildOfferItemList(ByVal OfferID As Integer) As BindingList(Of ItemGroupMemberBO)
            Dim offerMemberBO As PromotionOfferMemberBO
            Dim listItemGroupMemberBO As BindingList(Of ItemGroupMemberBO)
            Dim itemgroupMemberDAO As New ItemGroupMemberDAO
            Dim itemgroupMemberBO As New ItemGroupMemberBO
            Dim listAllGroupItems As New BindingList(Of ItemGroupMemberBO)
            Dim offerDAO As New PromotionOfferDAO
            Dim listMembers As PromotionOfferMemberBOList

            Try
                ' Get arraylist of members for the offer
                listMembers = offerDAO.GetPromotionalOfferMembersList(OfferID)

                ' Get Items contained within groups who are members of the offer
                For Each offerMemberBO In listMembers
                    listItemGroupMemberBO = itemgroupMemberDAO.GetGroupItems(offerMemberBO.GroupID)

                    ' Add each item in te current member group to the list of offer items (if it doesn't already exist in the list)
                    ' EXCLUDE items who have a DELETED status
                    For Each itemgroupMemberBO In listItemGroupMemberBO
                        If Not listAllGroupItems.Contains(itemgroupMemberBO) And Not offerMemberBO.isDeleted Then
                            listAllGroupItems.Add(itemgroupMemberBO)
                        End If
                    Next
                Next
            Catch ex As Exception
                listAllGroupItems = Nothing

                'display error msg
                MessageBox.Show(ex.Message, "PromotionalOfferPriceBatchDetail:BuildOfferItemList", MessageBoxButtons.OK, MessageBoxIcon.Warning)

            End Try

            Return listAllGroupItems

        End Function

        ''' <summary>
        ''' returns an arrayList of the StoreIDs of stores currently associated with the offer
        ''' </summary>
        ''' <returns>arrayllist of Store_Nos (integers)</returns>
        ''' <remarks></remarks>
        Private Function BuildOfferStoreList(ByVal OfferID As Integer) As ArrayList
            Dim PromotionDAO As PromotionOfferDAO = New PromotionOfferDAO
            Dim StoreData As DataTable = PromotionDAO.GetStoresByPromotionId(OfferId)
            Dim listStores As New ArrayList

            ' create an element in the array list for every associated store
            For Each dr As DataRow In StoreData.Rows
                If CBool(dr("IsAssigned")) And Not CBool(dr("IsActive")) Then
                    listStores.Add(CInt(dr("Store_No")))
                End If
            Next

            StoreData.Dispose()

            Return listStores

        End Function

        ''' <summary>
        ''' Builds an array of ItemGroupMemberBOs with each eleemnt assigned the appropriate Status (OfferChangeType)
        ''' based on edits that have been made to the offer.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function BuildItemDetailArray(ByRef offerBO As PromotionalOfferUpdateBO) As Boolean
            Dim found As Boolean
            Dim success As Boolean = True

            Try

                ' Get all unique Items contained in the offer
                offerBO.UpdatedItems = BuildOfferItemList(offerBO.OfferID)

                ' loop through all Updated Itemsto dtermine which are new
                For Each item As ItemGroupMemberBO In offerBO.UpdatedItems

                    ' See if item Id exists in list or original items
                    found = False
                    For Each testItem As ItemGroupMemberBO In offerBO.OriginalItems
                        If testItem.ItemKey = item.ItemKey Then
                            found = True
                            Exit For
                        End If
                    Next

                    If Not found Then
                        ' Items not in original offer are give OfferChgType ADD 
                        ' Existing Stores will have an ADD detail written for this item
                        ' Newly associated stores will have a NEW detail written for this item
                        item.Status = BO_IRMABase.OfferChangeType.Add
                    Else
                        'Items in original offer are give OfferChgType NEW 
                        ' Existing Stores will not build a detail for this item
                        ' Newly associated stores will have a NEW detail written for this item
                        item.Status = BO_IRMABase.OfferChangeType.[New]
                    End If
                    offerBO.ItemDetails.Add(item)

                Next

                ' loop through all Original itemsto determine which are deleted
                For Each item As ItemGroupMemberBO In offerBO.OriginalItems

                    ' See if item Id exists in list or original items
                    found = False
                    For Each testItem As ItemGroupMemberBO In offerBO.UpdatedItems
                        If testItem.ItemKey = item.ItemKey Then
                            found = True
                            Exit For
                        End If
                    Next

                    If Not found Then
                        ' Item was deleted during editing
                        ' Existing Stores will build a DELETE detail for this item
                        ' Newly associated stores will n ot build a detail
                        item.Status = BO_IRMABase.OfferChangeType.Delete
                        offerBO.ItemDetails.Add(item)
                    End If

                Next
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "PromotionalOfferPriceBatchDetail:BuildItemDetailArray")
                success = False
            End Try

            Return success
        End Function

        Private Function HasUnbatchedAdd(ByVal OfferID As Integer, ByVal ItemID As Integer) As Boolean
            Dim pricebatchDAO As New PriceBatchDetailDAO
            Dim tableDetails As DataTable
            Dim found As Boolean = False

            tableDetails = pricebatchDAO.GetPriceBatchDetailsByOfferItem(OfferID, ItemID, True)

            ' see if any rows are ADDs
            Dim row As DataRow
            For Each row In tableDetails.Rows
                If CType(row("OfferChgTYpeID"), Integer) = 2 Then
                    found = True
                    Exit For
                End If
            Next

            Return found
        End Function

        Private Function ProcessOffer(ByRef offerBO As PromotionalOfferUpdateBO) As Boolean
            Dim success As Boolean = True
            Dim offerDAO As New PromotionOfferDAO
            Dim batchdetailDAO As New PriceBatchDetailDAO
            Dim itemDAO As New WholeFoods.IRMA.ItemHosting.DataAccess.ItemDAO
            Dim itemgroupMemberDAO As New ItemGroupMemberDAO
            Dim itemgroupMemberBO As New ItemGroupMemberBO
            Dim NoItemsChanged As Boolean = True
            Dim HasUnbatchedDetails As Boolean = False
            Dim OfferPushed As Boolean = False
            Dim UnbatchedAdd As Boolean = False

            ' Build the list of items covered by the offer, along with the status we should write to the Price Batch detail file
            success = BuildItemDetailArray(offerBO)

            ' Get updated list of associated Stores
            offerBO.UpdatedStores = BuildOfferStoreList(offerBO.OfferID)

            ' check to see if Offer has changed but no items have
            For Each itemgroupMemberBO In offerBO.ItemDetails

                With itemgroupMemberBO
                    ' a NEW status indicates that the offer is new, or this is an item who existed in the offer and has
                    ' changed during the current editing
                    If itemgroupMemberBO.Status <> BO_IRMABase.OfferChangeType.[New] Then
                        NoItemsChanged = False
                        Exit For
                    End If
                End With

            Next

            ' Create ADD & NEW PriceBatchDetails for associated stores 
            ' create/update the PriceBatchDetail for each unique item in offer
            ' all operations should be handled within a transaction
            Dim transact As SqlTransaction = offerDAO.GetTransaction()
            Try
                For Each StoreID As Integer In offerBO.UpdatedStores
                    ' Find out if there are pending batch records for this offer/store pair
                    HasUnbatchedDetails = offerBO.HasUnbatchedDetails(StoreID)

                    ' See if offer has been pushed to this store
                    OfferPushed = offerBO.IsPushed(StoreID)

                    ' Process each item in the detail list
                    For Each itemgroupMemberBO In offerBO.ItemDetails

                        With itemgroupMemberBO
                            If Not offerBO.OriginalStores.Contains(StoreID) Then

                                ' If store is newly added, process ADDs as NEWs
                                Select Case itemgroupMemberBO.Status
                                    Case BO_IRMABase.OfferChangeType.Add, BO_IRMABase.OfferChangeType.[New]
                                        ' Create a NEW detail for items witn ADD or NEW status - force 
                                        ' NEW status in function call
                                        success = batchdetailDAO.UpdatePromotionPriceBatchDetails(.ItemKey, StoreID, offerBO.OfferID, itemgroupMemberBO.OfferChangeType.[New], transact)
                                    Case BO_IRMABase.OfferChangeType.Delete
                                        ' Do Nothing
                                End Select

                            Else

                                ' This Offer existed in the store previously
                                Select Case itemgroupMemberBO.Status
                                    Case BO_IRMABase.OfferChangeType.Add
                                        ' Create PriceBatchdetail with ADD status
                                        If OfferPushed Then
                                            success = batchdetailDAO.UpdatePromotionPriceBatchDetails(.ItemKey, StoreID, offerBO.OfferID, .Status, transact)
                                        Else
                                            ' if offer has never been pushed, send with Status NEW
                                            success = batchdetailDAO.UpdatePromotionPriceBatchDetails(.ItemKey, StoreID, offerBO.OfferID, BO_IRMABase.OfferChangeType.[New], transact)
                                        End If

                                    Case BO_IRMABase.OfferChangeType.Delete
                                        ' Is this item in the offer by virtue of an unbatched ADD detail?
                                        UnbatchedAdd = HasUnbatchedAdd(offerBO.OfferID, .ItemKey)

                                        ' delete unpushed PBDs for this item/store, replacing them with the DELETE PBD
                                        success = batchdetailDAO.DeleteUnbatchedPriceDetails(offerBO.OfferID, .ItemKey, StoreID, transact)

                                        ' Create PriceBatchdetail with DELETE status if the offer has been pushed to the POS before
                                        If OfferPushed And Not UnbatchedAdd Then
                                            success = batchdetailDAO.UpdatePromotionPriceBatchDetails(.ItemKey, StoreID, offerBO.OfferID, .Status, transact)
                                        End If

                                    Case BO_IRMABase.OfferChangeType.[New]
                                        ' Offer already exists - NEW status indicates the item was already a part of the
                                        ' offer prior to this editing session
                                        ' If there are Unbatched details for the offer/store pair , we assume that there are already
                                        ' NEW records for the unchanged items and there is no reason to rewrite them
                                        If Not HasUnbatchedDetails Then
                                            success = batchdetailDAO.UpdatePromotionPriceBatchDetails(.ItemKey, StoreID, offerBO.OfferID, OfferChangeType.[New], transact)
                                        End If

                                End Select

                            End If
                        End With

                        ' if a failure was detected, exit loop
                        If Not success Then Exit For
                    Next

                    ' if a failure was detected, exit loop
                    If Not success Then Exit For
                Next

                ' Rollback if failure is detected
                If Not success Then
                    transact.Rollback()
                End If

            Catch ex As Exception
                success = False
                transact.Rollback()
            End Try

            If success Then
                transact.Commit()
            End If

            Return success
        End Function

#End Region

#Region "Public Methods"
        Public Function PreEditInitialization(ByVal ID As Integer, ByVal initType As InitializeType) As Boolean
            Dim offerBO As PromotionalOfferUpdateBO
            Dim success As Boolean = True

            _listOffer = New ArrayList

            Try
                ' Get Offer IDs to process
                Select Case initType
                    Case InitializeType.SingleOffer
                        ' Insert single BO into Offer list to be processed for the specified OfferId
                        offerBO = New PromotionalOfferUpdateBO
                        offerBO.OfferID = ID
                        _listOffer.Add(offerBO)

                    Case InitializeType.GroupOwners
                        BuildOfferListByGroup(ID)

                End Select

                ' Get array of Original (pre-edit) Items for each offer in list
                For Each offerBO In _listOffer
                    With offerBO
                        .OriginalItems = BuildOfferItemList(.OfferID)
                        .OriginalStores = BuildOfferStoreList(.OfferID)
                    End With
                Next

            Catch ex As Exception
                success = False
            End Try

            Return success

        End Function

        Public Function MaintainPriceBatchDetail() As Boolean
            Dim offerBO As PromotionalOfferUpdateBO

            For Each offerBO In _listOffer
                ProcessOffer(offerBO)
            Next

        End Function

        ''' <summary>
        ''' Replaces any elements in the list of offerIDs whose value is zero with the specified ID. 
        ''' Should be called when a new offer has been created and the PreditInitalization process picked up a 0
        ''' because no offerID was yet assigned.
        ''' </summary>
        ''' <param name="newOfferID"></param>
        ''' <remarks></remarks>
        Public Sub UpdateZeroID(ByVal newOfferID As Integer)

            ' search list of OfferIDs for a zero value
            For i As Integer = 0 To _listOffer.Count - 1
                If CType(_listOffer(i), PromotionalOfferUpdateBO).OfferID = 0 Then
                    CType(_listOffer(i), PromotionalOfferUpdateBO).OfferID = newOfferID

                    ' since it should only be possible to have a single 0 value in the list, exit
                    Exit For
                End If
            Next


        End Sub

#End Region

    End Class
End Namespace
