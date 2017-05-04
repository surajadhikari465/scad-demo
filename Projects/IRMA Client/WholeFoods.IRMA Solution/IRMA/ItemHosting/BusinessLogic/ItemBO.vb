Imports log4net
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Enum ItemStatus
    Valid
    Error_ItemDescriptionRequired
    Error_POSDescriptionRequired
    Error_SignCaptionRequired
    Error_PackageDesc1Required
    Error_PackageDesc1ValueIsZero
    Error_PackageDesc2Required
    Error_PackageDesc2ValueIsZero
    Error_PackageDescUnitRequired
    Error_VendorUnitRequired
    Error_DistributionUnitRequired
    Error_RetailUnitRequired
    Error_AverageUnitWeightRequired
End Enum

Public Enum BatchModificationType
    Update
    Rollback
End Enum

Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic
    Public Class ItemBO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Public Constants"

        Public Const ITEM_IDENTIFIER_MAX_LENGTH = 14

#End Region

#Region "Property Definitions"

        ' Some numeric values will be stored as strings so that DBNull values are handled properly.
        Private _Item_Key As Integer
        Private _identifier As String
        Private _storeJurisdictionID As String
        Private _storeJurisdictionDesc As String
        Private _VATTax As Double
        Private _itemDescription As String
        Private _posDescription As String
        Private _signCaption As String
        Private _packageDesc1 As Decimal
        Private _packageDesc2 As Decimal
        Private _packageUnitID As String
        Private _packageUnitName As String
        Private _costedByWeight As Boolean
        Private _retailUnitID As String
        Private _vendorUnitID As String
        Private _distributionUnitID As String
        Private _manufacturingUnitID As String
        Private _isCatchweightRequired As Boolean
        Private _isCOOL As Boolean
        Private _isBIO As Boolean
        Private _isShipper As Boolean
        Private _shipper As Shipper
        Private _isIngredient As Boolean
        Private _averageUnitWeight As Decimal
        Private _useLastReceivedCost As Boolean
        Private _brandID As String
        Private _originID As String
        Private _countryOfProcID As String
        Private _sustainabilityRankingRequired As Boolean
        Private _sustainabilityRankingID As String
        Private _labelTypeID As String
        Private _recall As Boolean
        Private _lockAuth As Boolean
        Private _notAvailable As Boolean
        Private _notAvailableNote As String
        Private _giftCard As Boolean
        Private _isValidated As Boolean
        Private _hasIngredientIdentifier As Boolean

#End Region

#Region "Constructors"

        ''' <summary>
        ''' empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Create an instance of this object, populating it with the values from the 
        ''' GetItemInfo result set.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByRef itemResult As DAO.Recordset)

            If Not itemResult.Fields("Item_Key").Value.Equals(DBNull.Value) Then
                _Item_Key = CInt(itemResult.Fields("Item_Key").Value)
            End If

            If Not itemResult.Fields("Identifier").Value.Equals(DBNull.Value) Then
                _identifier = itemResult.Fields("Identifier").Value.ToString
            End If

            If Not itemResult.Fields("POS_Description").Value.Equals(DBNull.Value) Then
                _posDescription = itemResult.Fields("POS_Description").Value.ToString
            Else
                _posDescription = Nothing
            End If

            If Not itemResult.Fields("Item_Description").Value.Equals(DBNull.Value) Then
                _itemDescription = itemResult.Fields("Item_Description").Value.ToString
            Else
                _itemDescription = Nothing
            End If

            If Not itemResult.Fields("Sign_Description").Value.Equals(DBNull.Value) Then
                _signCaption = itemResult.Fields("Sign_Description").Value.ToString & ""
            Else
                _signCaption = Nothing
            End If

            If Not itemResult.Fields("Package_Desc1").Value.Equals(DBNull.Value) Then
                _packageDesc1 = Decimal.Parse(itemResult.Fields("Package_Desc1").Value.ToString)
            Else
                _packageDesc1 = Nothing
            End If

            If Not itemResult.Fields("Package_Desc2").Value.Equals(DBNull.Value) Then
                _packageDesc2 = Decimal.Parse(itemResult.Fields("Package_Desc2").Value.ToString)
            Else
                _packageDesc2 = Nothing
            End If

            If Not itemResult.Fields("Package_Desc2").Value.Equals(DBNull.Value) Then
                _packageDesc2 = Decimal.Parse(itemResult.Fields("Package_Desc2").Value.ToString)
            Else
                _packageDesc2 = Nothing
            End If

            If Not itemResult.Fields("Average_Unit_Weight").Value.Equals(DBNull.Value) Then
                _averageUnitWeight = Decimal.Parse(itemResult.Fields("Average_Unit_Weight").Value.ToString)
            Else
                _averageUnitWeight = Nothing
            End If

            If Not itemResult.Fields("Package_Unit_ID").Value.Equals(DBNull.Value) Then
                _packageUnitID = itemResult.Fields("Package_Unit_ID").Value.ToString
            Else
                _packageUnitID = Nothing
            End If

            If Not itemResult.Fields("Retail_Unit_ID").Value.Equals(DBNull.Value) Then
                _retailUnitID = itemResult.Fields("Retail_Unit_ID").Value.ToString
            Else
                _retailUnitID = Nothing
            End If

            If Not itemResult.Fields("Vendor_Unit_ID").Value.Equals(DBNull.Value) Then
                _vendorUnitID = itemResult.Fields("Vendor_Unit_ID").Value.ToString
            Else
                _vendorUnitID = Nothing
            End If

            If Not itemResult.Fields("Distribution_Unit_ID").Value.Equals(DBNull.Value) Then
                _distributionUnitID = itemResult.Fields("Distribution_Unit_ID").Value.ToString
            Else
                _distributionUnitID = Nothing
            End If

            If Not itemResult.Fields("Manufacturing_Unit_ID").Value.Equals(DBNull.Value) Then
                _manufacturingUnitID = itemResult.Fields("Manufacturing_Unit_ID").Value.ToString
            Else
                _manufacturingUnitID = Nothing
            End If

            If Not itemResult.Fields("CostedByWeight").Value.Equals(DBNull.Value) Then
                _costedByWeight = CBool(itemResult.Fields("CostedByWeight").Value.ToString)
            Else
                _costedByWeight = Nothing
            End If

            If Not itemResult.Fields("StoreJurisdictionDesc").Value.Equals(DBNull.Value) Then
                _storeJurisdictionDesc = itemResult.Fields("StoreJurisdictionDesc").Value.ToString
            Else
                _storeJurisdictionDesc = Nothing
            End If

            If Not itemResult.Fields("StoreJurisdictionID").Value.Equals(DBNull.Value) Then
                _storeJurisdictionID = itemResult.Fields("StoreJurisdictionID").Value.ToString
            Else
                _storeJurisdictionID = Nothing
            End If

            If Not itemResult.Fields("Shipper_Item").Value.Equals(DBNull.Value) Then
                _isShipper = CBool(itemResult.Fields("Shipper_Item").Value)
            Else
                _isShipper = False
            End If

            If Not itemResult.Fields("UseLastReceivedCost").Value.Equals(DBNull.Value) Then
                _useLastReceivedCost = CBool(itemResult.Fields("UseLastReceivedCost").Value)
            Else
                _useLastReceivedCost = False
            End If

            If Not itemResult.Fields("LabelType_ID").Value.Equals(DBNull.Value) Then
                _labelTypeID = CStr(itemResult.Fields("LabelType_ID").Value)
            Else
                _labelTypeID = Nothing
            End If

            If Not itemResult.Fields("CountryProc_ID").Value.Equals(DBNull.Value) Then
                _countryOfProcID = CStr(itemResult.Fields("CountryProc_ID").Value)
            Else
                _countryOfProcID = Nothing
            End If

            If Not itemResult.Fields("LockAuth").Value.Equals(DBNull.Value) Then
                _lockAuth = CBool(itemResult.Fields("LockAuth").Value)
            Else
                _lockAuth = False
            End If

            If Not itemResult.Fields("Ingredient").Value.Equals(DBNull.Value) Then
                _isIngredient = CBool(itemResult.Fields("Ingredient").Value)
            Else
                _isIngredient = False
            End If

            If Not itemResult.Fields("Recall_Flag").Value.Equals(DBNull.Value) Then
                _recall = CBool(itemResult.Fields("Recall_Flag").Value)
            Else
                _recall = False
            End If

            If Not itemResult.Fields("Not_Available").Value.Equals(DBNull.Value) Then
                _notAvailable = CBool(itemResult.Fields("Not_Available").Value)
            Else
                _notAvailable = False
            End If

            If Not itemResult.Fields("Not_AvailableNote").Value.Equals(DBNull.Value) Then
                _notAvailableNote = CStr(itemResult.Fields("Not_AvailableNote").Value)
            Else
                _notAvailableNote = Nothing
            End If

            If Not itemResult.Fields("Brand_ID").Value.Equals(DBNull.Value) Then
                _brandID = CStr(itemResult.Fields("Brand_ID").Value)
            Else
                _brandID = Nothing
            End If

            If Not itemResult.Fields("Origin_ID").Value.Equals(DBNull.Value) Then
                _originID = CStr(itemResult.Fields("Origin_ID").Value)
            Else
                _originID = Nothing
            End If

            If Not itemResult.Fields("SustainabilityRankingRequired").Value.Equals(DBNull.Value) Then
                _sustainabilityRankingRequired = CBool(itemResult.Fields("SustainabilityRankingRequired").Value)
            Else
                _sustainabilityRankingRequired = False
            End If

            If Not itemResult.Fields("SustainabilityRankingID").Value.Equals(DBNull.Value) Then
                _sustainabilityRankingID = CStr(itemResult.Fields("SustainabilityRankingID").Value)
            Else
                _sustainabilityRankingID = Nothing
            End If

            If Not itemResult.Fields("GiftCard").Value.Equals(DBNull.Value) Then
                _giftCard = CBool(itemResult.Fields("GiftCard").Value)
            Else
                _giftCard = False
            End If

            If Not itemResult.Fields("IsValidated").Value.Equals(DBNull.Value) Then
                _isValidated = CBool(itemResult.Fields("IsValidated").Value)
            Else
                _isValidated = False
            End If

            If Not itemResult.Fields("HasIngredientIdentifier").Value.Equals(DBNull.Value) Then
                _hasIngredientIdentifier = CBool(itemResult.Fields("HasIngredientIdentifier").Value)
            Else
                _hasIngredientIdentifier = False
            End If
            ' ***** Design Opportunity *****
            ' Not building the Shipper object until it is referenced the first time (via ItemBO.Shipper() property method call)
            ' could potentially save load time for the Item screen when pulling up shipper items.  The extra load time for Shipper items
            ' would be the query to retrieve the data that is loaded into the Shipper object.
            '
            ' For now, it is assumed Shippers will be very rare and the amount of data being retrieved should be very small,
            ' so the extra load time should be okay.

            ' Build Shipper object.  This may throw exceptions, so callers should handle.
            If IsShipper Then
                _shipper = New Shipper(_Item_Key, _identifier, _itemDescription)
            End If
        End Sub

#End Region

#Region "Property Access Methods"

        Public Property Item_Key() As Integer
            Get
                Return _Item_Key
            End Get
            Set(ByVal value As Integer)
                _Item_Key = value
                ' We care about the scenario where our item key is changing from one valid item to another,
                ' meaning this object is being reused and the actual item it is representing is changing.
                ' Unfortunately, we cannot know if the IsShipper property has been set appropriately for the new item,
                ' so we just clear the Shipper object and if it is referenced within this object in the future, it will try to rebuild at that point.
                If _Item_Key > 0 And value > 0 And _Item_Key <> value Then
                    logger.Warn("The ItemBO object is changing from item_key '" & _Item_Key & "' to '" & value & "'.")
                    _shipper = Nothing
                End If
            End Set
        End Property

        Public Property StoreJurisdictionID() As String
            Get
                Return _storeJurisdictionID
            End Get
            Set(ByVal value As String)
                _storeJurisdictionID = value
            End Set
        End Property

        Public Property Identifier() As String
            Get
                Return _identifier
            End Get
            Set(ByVal value As String)
                _identifier = value
            End Set
        End Property

        Public Property StoreJurisdictionDesc() As String
            Get
                Return _storeJurisdictionDesc
            End Get
            Set(ByVal value As String)
                _storeJurisdictionDesc = value
            End Set
        End Property

        Public Property ItemDescription() As String
            Get
                Return _itemDescription
            End Get
            Set(ByVal value As String)
                _itemDescription = value
            End Set
        End Property

        Public Property POSDescription() As String
            Get
                Return _posDescription
            End Get
            Set(ByVal value As String)
                _posDescription = value
            End Set
        End Property

        Public Property SignCaption() As String
            Get
                Return _signCaption
            End Get
            Set(ByVal value As String)
                _signCaption = value
            End Set
        End Property


        Public Property PackageDesc1() As Decimal
            Get
                Return _packageDesc1
            End Get
            Set(ByVal value As Decimal)
                _packageDesc1 = value
            End Set
        End Property

        Public Property PackageDesc2() As Decimal
            Get
                Return _packageDesc2
            End Get
            Set(ByVal value As Decimal)
                _packageDesc2 = value
            End Set
        End Property

        Public Property AverageUnitWeight() As Decimal
            Get
                Return _averageUnitWeight
            End Get
            Set(ByVal value As Decimal)
                _averageUnitWeight = value
            End Set
        End Property

        Public Property PackageUnitName() As String
            Get
                Return _packageUnitName
            End Get
            Set(ByVal value As String)
                _packageUnitName = value
            End Set
        End Property

        Public Property PackageUnitID() As String
            Get
                Return _packageUnitID
            End Get
            Set(ByVal value As String)
                _packageUnitID = value
            End Set
        End Property

        Public Property RetailUnitID() As String
            Get
                Return _retailUnitID
            End Get
            Set(ByVal value As String)
                _retailUnitID = value
            End Set
        End Property

        Public Property VendorUnitID() As String
            Get
                Return _vendorUnitID
            End Get
            Set(ByVal value As String)
                _vendorUnitID = value
            End Set
        End Property

        Public Property DistributionUnitID() As String
            Get
                Return _distributionUnitID
            End Get
            Set(ByVal value As String)
                _distributionUnitID = value
            End Set
        End Property

        Public Property ManufacturingUnitID() As String
            Get
                Return _manufacturingUnitID
            End Get
            Set(ByVal value As String)
                _manufacturingUnitID = value
            End Set
        End Property

        Public Property CostedByWeight() As Boolean
            Get
                Return _costedByWeight
            End Get
            Set(ByVal value As Boolean)
                _costedByWeight = value
            End Set
        End Property

        Public Property CatchweightRequired() As Boolean
            Get
                Return Me._isCatchweightRequired
            End Get
            Set(ByVal value As Boolean)
                Me._isCatchweightRequired = value
            End Set
        End Property

        Public Property UseLastReceivedCost() As Boolean
            Get
                Return Me._useLastReceivedCost
            End Get
            Set(ByVal value As Boolean)
                Me._useLastReceivedCost = value
            End Set
        End Property

        Public Property GiftCard() As Boolean
            Get
                Return Me._giftCard
            End Get
            Set(ByVal value As Boolean)
                Me._giftCard = value
            End Set
        End Property

        Public Property COOL() As Boolean
            Get
                Return Me._isCOOL
            End Get
            Set(ByVal value As Boolean)
                Me._isCOOL = value
            End Set
        End Property

        Public Property BIO() As Boolean
            Get
                Return Me._isBIO
            End Get
            Set(ByVal value As Boolean)
                Me._isBIO = value
            End Set
        End Property

        ''' <summary>
        ''' Returns whether or not this item is a Shipper (an item that contains a group of other items).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsShipper() As Boolean
            Get
                Return _isShipper
            End Get
        End Property

        ''' <summary>
        ''' Returns a Shipper object, if the item represented by this item class is marked as a Shipper.
        ''' If the internal Shipper object needs to be initialized, it will be attempted herein,
        ''' so exceptions may occur and should be handled.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Shipper() As Shipper
            Get
                ' If someone is trying to get the Shipper object, let's try to make sure it has been created.
                ' Some implementations of the ItemBO class do not use the ItemBO constructor that builds the Shipper object.
                If IsShipper And _shipper Is Nothing Then
                    ' This may throw exceptions, but we let them go, so caller must handle.
                    _shipper = New Shipper(Item_Key, Identifier, ItemDescription)
                End If
                Return _shipper
            End Get
        End Property

        Public Property Ingredient() As Boolean
            Get
                Return Me._isIngredient

            End Get
            Set(ByVal value As Boolean)
                Me._isIngredient = value
            End Set
        End Property

        Public Property BrandID() As String
            Get
                Return _brandID
            End Get
            Set(ByVal value As String)
                _brandID = value
            End Set
        End Property

        Public Property OriginID() As String
            Get
                Return _originID
            End Get
            Set(ByVal value As String)
                _originID = value
            End Set
        End Property

        Public Property CountryOfProcID() As String
            Get
                Return _countryOfProcID
            End Get
            Set(ByVal value As String)
                _countryOfProcID = value
            End Set
        End Property

        Public Property SustainabilityRankingRequired() As Boolean
            Get
                Return _sustainabilityRankingRequired
            End Get
            Set(ByVal value As Boolean)
                _sustainabilityRankingRequired = value
            End Set
        End Property

        Public Property SustainabilityRankingID() As String
            Get
                Return _sustainabilityRankingID
            End Get
            Set(ByVal value As String)
                _sustainabilityRankingID = value
            End Set
        End Property

        Public Property LabelTypeID() As String
            Get
                Return _labelTypeID
            End Get
            Set(ByVal value As String)
                _labelTypeID = value
            End Set
        End Property

        Public Property Recall() As Boolean
            Get
                Return _recall
            End Get
            Set(ByVal value As Boolean)
                _recall = value
            End Set
        End Property

        Public Property LockAuth() As Boolean
            Get
                Return _lockAuth
            End Get
            Set(ByVal value As Boolean)
                _lockAuth = value
            End Set
        End Property

        Public Property NotAvailable() As Boolean
            Get
                Return _notAvailable
            End Get
            Set(ByVal value As Boolean)
                _notAvailable = value
            End Set
        End Property

        Public Property NotAvailableNote() As String
            Get
                Return _notAvailableNote
            End Get
            Set(ByVal value As String)
                _notAvailableNote = value
            End Set
        End Property

        Public Property IsValidated() As Boolean
            Get
                Return _isValidated
            End Get
            Set(ByVal value As Boolean)
                _isValidated = value
            End Set
        End Property

        Public Property HasIngredientIdentifier() As Boolean
            Get
                Return _hasIngredientIdentifier
            End Get
            Set(ByVal value As Boolean)
                _hasIngredientIdentifier = value
            End Set
        End Property

#End Region

#Region "Business Rules"
        Public Function ValidateData() As ArrayList
            Return ValidateData(True, True)
        End Function

        Public Function ValidateData(ByVal packageDesc1Required As Boolean, ByVal packageDesc2Required As Boolean) As ArrayList
            Dim statusList As New ArrayList

            ' Validate all of the required fields have been populated
            ' and the data is in the expected format.
            ' -- Item Description
            If _itemDescription = vbNullString Then
                statusList.Add(ItemStatus.Error_ItemDescriptionRequired)
            End If
            ' -- POS Description
            If _posDescription = vbNullString Then
                statusList.Add(ItemStatus.Error_POSDescriptionRequired)
            End If
            ' -- Sign Description
            If _signCaption = vbNullString Then
                statusList.Add(ItemStatus.Error_SignCaptionRequired)
            End If
            ' -- Package Description 1
            If packageDesc1Required Then
                If _packageDesc1.ToString = vbNullString Then
                    statusList.Add(ItemStatus.Error_PackageDesc1Required)
                Else
                    If Val(_packageDesc1) = 0 Then
                        statusList.Add(ItemStatus.Error_PackageDesc1ValueIsZero)
                    End If
                End If
            End If
            ' -- Package Description 2
            If packageDesc2Required Then
                If _packageDesc2.ToString = vbNullString Then
                    statusList.Add(ItemStatus.Error_PackageDesc2Required)
                Else
                    If Val(_packageDesc2) = 0 Then
                        statusList.Add(ItemStatus.Error_PackageDesc2ValueIsZero)
                    End If
                End If
            End If
            ' -- Package Unit ID
            If _packageUnitID Is Nothing Then
                statusList.Add(ItemStatus.Error_PackageDescUnitRequired)
            End If
            ' -- Vendor Unit ID
            If _vendorUnitID Is Nothing Then
                statusList.Add(ItemStatus.Error_VendorUnitRequired)
            End If
            ' -- Distribution Unit ID
            If _distributionUnitID Is Nothing Then
                statusList.Add(ItemStatus.Error_DistributionUnitRequired)
            End If
            ' -- Retail Unit ID
            If _retailUnitID Is Nothing Then
                statusList.Add(ItemStatus.Error_RetailUnitRequired)
            End If

            If statusList.Count = 0 Then
                statusList.Add(ItemStatus.Valid)
            End If

            Return statusList
        End Function

#End Region

#Region "User Interaction"
        ' Tom Lux, 2/9/10, TFS 11978, 3.5.9.

        ''' <summary>
        ''' Ask user if they wish to continue, saving their item modification, and consequently
        ''' update/rollback the batches that contain the item.
        ''' </summary>
        ''' <param name="identifier"></param>
        ''' <param name="batchIdList"></param>
        ''' <param name="modType"></param>
        ''' <remarks></remarks>
        Public Shared Function UserConfirmedSaveChangeAndModifyBatches( _
                    ByVal identifier As String, _
                    ByVal batchIdList As String, _
                    ByVal modType As BatchModificationType _
                ) As Boolean
            Dim response As DialogResult
            Dim msgCaption As String = String.Empty

            Dim msgText As String = "A batch-sensitive item change has been made." & vbCrLf & vbCrLf & _
                    "The batches listed below are in Ready, Printed, or Sent status" & vbCrLf & "and contain identifier " & identifier & "."

            If batchIdList.Length > 0 Then
                If modType = BatchModificationType.Rollback Then
                    msgCaption = "Batch Pull-Back Warning"
                    msgText = msgText & vbCrLf & "These will be rolled back to the Packaged state" & vbCrLf & "if you continue and save your changes." & vbCrLf & vbCrLf & _
                        "Do you want to save your item changes and pull these batches back?"
                ElseIf modType = BatchModificationType.Update Then
                    msgCaption = "Batch Update Warning"
                    msgText = msgText & vbCrLf & "These will be updated with the new item data" & vbCrLf & "if you continue and save your changes." & vbCrLf & vbCrLf & _
                        "Do you want to save your item changes and update these batches?"
                End If
                msgText = msgText & vbCrLf & vbCrLf & batchIdList
                response = MessageBox.Show(msgText, msgCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            End If

            Return (response = Windows.Forms.DialogResult.Yes)
        End Function

        Public Shared Sub UpdateOrRollbackBatches( _
            ByVal identifier As String, _
            ByVal updateIdList As String, _
            ByVal updateInfoList As String, _
            ByVal rollbackIdList As String, _
            ByVal rollbackInfoList As String _
        )
            ' Tom Lux, 2/9/10, TFS 11978, 3.5.9.
            ' To facilitate conditionally rolling-back batches based on a prompt/response from the user,
            ' this sub was split into two parts:
            '  - A function (GetBatchesInSentState) in ItemDAO that builds lists of batches in 'sent' state.
            '  - This sub (UpdateOrRollbackBatches) that actually performs the batch update or rollback, if there are any batches in the lists.

            ' We shouldn't be unconditionally calling this SP if the batch lists are empty,
            ' so I added an IF-wrapper.
            If rollbackIdList.Length > 0 Or updateIdList.Length > 0 Then
                ItemDAO.UpdateOrRollbackBatches(updateIdList, rollbackIdList)

                Dim msgText As String = "The batches listed below were in Ready, Printed, or Sent status" & vbCrLf & "and contain identifier " & identifier & "."
                If rollbackIdList.Length > 0 Then
                    MessageBox.Show(msgText & vbCrLf & "They have been rolled back to Packaged status." & vbCrLf & "Please be sure to promote the batches again." & _
                        vbCrLf & vbCrLf & rollbackInfoList, "Batch Rollback Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If

                If updateIdList.Length > 0 Then
                    MessageBox.Show(msgText & vbCrLf & "They have been updated and left in their current status." & _
                        vbCrLf & vbCrLf & updateInfoList, "Batch Update Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            End If
        End Sub
#End Region

    End Class

End Namespace