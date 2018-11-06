Imports log4net
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports System.Data.SqlClient


Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    ''' <summary>
    ''' A Shipper is a group of two or more items sold as a single item.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Shipper

#Region "Private Members"

        ''' <summary>
        ''' Log4Net logger for this class.
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        ''' <summary>
        ''' Key/Value structure to hold the items within this Shipper: the key is an item key and value is a ShipperItem object.
        ''' </summary>
        ''' <remarks></remarks>
        Private _htItemList As Hashtable = New Hashtable

#End Region

#Region "Public Constants"

        ''' <summary>
        ''' Initial pack qty (package_desc1) after pack info reset when an item is marked as a Shipper.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SHIPPER_INITIAL_PACK_QTY As Integer = 0

        ''' <summary>
        ''' Initial pack size (package_desc2) after pack info reset when an item is marked as a Shipper.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SHIPPER_INITIAL_PACK_SIZE As Integer = 1

        ''' <summary>
        ''' Number of output parameters expected from the add-item DB call (identifier, item desc).
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ADD_ITEM_OUTPUT_PARAM_COUNT As Integer = 2

#End Region

#Region "Property Declarations"

        Private _iItemKey As Integer ' A Shipper is an item.  This is the IRMA Item Key.
        Private _sIdentifier As String ' Item identifier (UPC) for this Shipper.
        Private _sDesc As String ' Item description for this Shipper.

        Private _iUnitCount As Integer ' Total units in this Shipper.

#End Region

#Region "Public Shared Methods"

        ''' <summary>
        ''' Used to validate whether or not an item can be marked as a Shipper.
        ''' Can be called before actually creating a Shipper object.
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <remarks>
        ''' Throws exceptions when Shipper rules are broken, so these need to be handled by the caller.
        ''' </remarks>
        Public Shared Sub ValidateConvertItemToShipper(ByVal itemKey As Integer)
            ' We need to make sure the "Shipper" and "Unit" item units exist, because Shipper functionality depends on them.
            ' First we check for the Shipper UOM.
            If UOM = -1 Then
                logger.Error(String.Format(ShipperMessages.ERROR_SHIPPER_CANNOT_BUILD_MISSING_SHIPPER_UOM & "  [ItemKey={0}]", itemKey))
                Throw New Exception(ShipperMessages.ERROR_SHIPPER_CANNOT_BUILD_MISSING_SHIPPER_UOM)
            End If

            ' Next we check for the Unit UOM.
            ' This Global-module reference should be moved into the ItemUnitDAO or appropriate object.
            If ReturnUnitID(ItemUnitDAO.UOM_NAME_UNIT) = -1 Then
                logger.Error(String.Format(ShipperMessages.ERROR_SHIPPER_CANNOT_BUILD_MISSING_UNIT_UOM & "  [ItemKey={0}]", itemKey))
                Throw New Exception(ShipperMessages.ERROR_SHIPPER_CANNOT_BUILD_MISSING_UNIT_UOM)
            End If

            ' If an item exists in any Shipper, it cannot be converted to a Shipper itself.
            If ShipperDAO.ItemExistsInAShipper(itemKey) Then
                logger.Error(String.Format(ShipperMessages.ERROR_SHIPPER_ITEM_EXISTS_IN_A_SHIPPER & "  [ItemKey={0}]", itemKey))
                Throw New Exception(ShipperMessages.ERROR_SHIPPER_ITEM_EXISTS_IN_A_SHIPPER)
            End If

        End Sub

        ''' <summary>
        ''' Used to validate whether or not a Shipper can be returned to a normal item.
        ''' </summary>
        ''' <param name="shp">Existing Shipper object for the shipper-item that is being converted back to an item.</param>
        ''' <remarks>
        ''' Throws exceptions when Shipper rules are broken, so these need to be handled by the caller.
        ''' </remarks>
        Public Shared Sub ValidateConvertShipperToItem(ByRef shp As Shipper)
            If shp.ItemCount > 0 Then
                logger.Error(String.Format(ShipperMessages.ERROR_SHIPPER_CONVERT_TO_ITEM_SHIPPER_CONTAINS_ITEMS & "  [ItemKey={0}, Identifier={1}, Desc={2}]", shp.ItemKey, shp.Identifier, shp.Desc))
                Throw New Exception(ShipperMessages.ERROR_SHIPPER_CONVERT_TO_ITEM_SHIPPER_CONTAINS_ITEMS)
            End If
        End Sub

        Public Shared Function IsItemAShipper(ByVal itemKey As Integer) As Boolean
            Return ShipperDAO.IsItemAShipper(itemKey)
        End Function

#End Region

#Region "Property Access Methods"

        ''' <summary>
        ''' Unique item key for this Shipper.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ItemKey() As Integer
            Get
                Return _iItemKey
            End Get
        End Property

        ''' <summary>
        ''' UPC or external item identifier for this Shipper.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Identifier() As String
            Get
                Return _sIdentifier
            End Get
        End Property

        ''' <summary>
        ''' Item description for this Shipper.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Desc() As String
            Get
                Return _sDesc
            End Get
        End Property

        ''' <summary>
        ''' Total items in this Shipper.
        ''' </summary>
        ''' <value></value>
        ''' <returns>Number of items in an internal list object.</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ItemCount() As Integer
            Get
                Return _htItemList.Count
            End Get
        End Property

        ''' <summary>
        ''' Total number of units across all items in this Shipper.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UnitCount() As Integer
            Get
                Return _iUnitCount
            End Get
        End Property

        ''' <summary>
        ''' Total items in the Shipper.ReturnUnitID("Unit")
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property UOM() As Integer
            Get
                ' Currently using existing Global-module function here, but this could be redesigned.
                Return ReturnUnitID(ShipperDAO.SHIPPER_UOM_NAME)
            End Get
        End Property

#End Region

#Region "Constructors"

        Public Sub New()

        End Sub


        ''' <summary>
        ''' Builds Shipper object, provided the passed item key is marked as a Shipper.
        ''' </summary>
        ''' <param name="itemKey">Item key for this Shipper.</param>
        ''' <param name="identifier">Identifier for this Shipper.</param>
        ''' <param name="desc">Item description for this Shipper.</param>
        ''' <remarks>
        ''' Throws exceptions if the passed item key is invalid or if the item is not a Shipper.
        ''' </remarks>
        Public Sub New(ByVal itemKey As Integer, ByVal identifier As String, ByVal desc As String)
            ' We don't want validation logic here, because the BuildShipper() method could be used by a
            ' caller, so all validiation should be there.
            ' Try to retrieve Shipper data.
            BuildShipper(itemKey, identifier, desc)
        End Sub

#End Region

#Region "Public Instance Methods"

        ''' <summary>
        ''' Adds an item to this Shipper.
        ''' </summary>
        ''' <param name="itemKey">New item key of item to be added.</param>
        ''' <remarks>This will throw exceptions, so the caller must handle.</remarks>
        Public Sub AddItem(ByVal itemKey As Integer)
            Dim attributeList As ArrayList
            Try
                ' We use a default initial unit qty for the new item in this Shipper.
                attributeList = ShipperDAO.AddItemToShipper(Me.ItemKey, itemKey, ShipperItem.SHIPPERITEM_INITIAL_UNIT_QTY)
            Catch ex As Exception
                logger.Error(String.Format(ShipperMessages.ERROR_SHIPPER_ADD_ITEM_DB_ERROR & "  [NewItemKey={0}, ShipperItemKey={1}]", itemKey, Me.ItemKey))
                Throw New Exception(ShipperMessages.ERROR_SHIPPER_ADD_ITEM_DB_ERROR, ex)
            End Try
            ' Order of attributes in the list should be identifier, then item desc.
            ' We expect two output items from the item-add call.
            If attributeList Is Nothing Or attributeList.Count <> ADD_ITEM_OUTPUT_PARAM_COUNT Then
                logger.Error(String.Format(ShipperMessages.ERROR_SHIPPER_ADD_ITEM_OUTPUT & "  [NewItemKey={0}, ShipperItemKey={1}, attributeList.Count={2}, expectedCount={3}]", itemKey, Me.ItemKey, attributeList.Count, ADD_ITEM_OUTPUT_PARAM_COUNT))
                Throw New Exception(ShipperMessages.ERROR_SHIPPER_ADD_ITEM_OUTPUT)
            End If
            ' Add item to internal list.  Exceptions thrown by ShipperItem constructor will bubble up.
            _htItemList.Add(itemKey, New ShipperItem(Me.ItemKey, itemKey, attributeList.Item(0), attributeList.Item(1), ShipperItem.SHIPPERITEM_INITIAL_UNIT_QTY))
        End Sub

        ''' <summary>
        ''' Assigns a new unit qty to an item in this Shipper.
        ''' </summary>
        ''' <param name="itemKey">Item key of item being updated.</param>
        ''' <param name="newUnitQty">New number of units of this item in this Shipper.</param>
        ''' <remarks></remarks>
        Public Sub UpdateItemUnitQty(ByVal itemKey As Integer, ByVal newUnitQty As Integer)
            GetItem(itemKey).Qty = newUnitQty
        End Sub

        ''' <summary>
        ''' Removes an item from this Shipper.
        ''' </summary>
        ''' <param name="itemKey">Item key of the item to be removed from this Shipper.</param>
        ''' <remarks></remarks>
        Public Sub DeleteItem(ByVal itemKey As Integer)
            ShipperDAO.DeleteItemFromShipper(Me.ItemKey, itemKey)
            If ItemExists(itemKey) Then
                _htItemList.Remove(itemKey)
            Else
                logger.Error(String.Format(ShipperMessages.ERROR_SHIPPER_DELETE_ITEM_NOT_IN_SHIPPER & "  [DelItemKey={0}, ShipperItemKey={1}]", itemKey, Me.ItemKey))
                Throw New Exception(ShipperMessages.ERROR_SHIPPER_DELETE_ITEM_NOT_IN_SHIPPER)
            End If
        End Sub

        Public Function ItemExists(ByVal itemKey As Integer) As Boolean
            Return _htItemList.Contains(itemKey)
        End Function

        Public Function GetItem(ByVal itemKey As Integer) As ShipperItem
            Return _htItemList.Item(itemKey)
        End Function

        ''' <summary>
        ''' Returns a collection representing the items in this Shipper.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemList() As ICollection
            Return _htItemList.Values

        End Function

        ''' <summary>
        ''' Retrieve Shipper data and populate the internal objects in this Shipper object.
        ''' 
        ''' </summary>
        ''' <param name="itemKey">Item key for this Shipper.</param>
        ''' <param name="identifier">Identifier for this Shipper.</param>
        ''' <param name="desc">Item description for this Shipper.</param>
        ''' <remarks>
        ''' This method is public to provide a way to "build" or populate a Shipper object when
        ''' the default constructor is used.
        ''' </remarks>
        Public Sub BuildShipper(ByVal itemKey As Integer, ByVal identifier As String, ByVal desc As String)
            If String.IsNullOrEmpty(identifier) Then
                logger.Error(String.Format(ShipperMessages.ERROR_SHIPPER_CANNOT_BUILD_INVALID_IDENTIFIER & "  [ItemKey={0}, Identifier={1}, Desc={2}]", itemKey, identifier, desc))
                Throw New Exception(ShipperMessages.ERROR_SHIPPER_CANNOT_BUILD_INVALID_IDENTIFIER)
            End If

            If String.IsNullOrEmpty(desc) Then
                logger.Error(String.Format(ShipperMessages.ERROR_SHIPPER_CANNOT_BUILD_INVALID_DESC & "  [ItemKey={0}, Identifier={1}, Desc={2}]", itemKey, identifier, desc))
                Throw New Exception(ShipperMessages.ERROR_SHIPPER_CANNOT_BUILD_INVALID_DESC)
            End If

            If itemKey = 0 Then
                logger.Error(String.Format(ShipperMessages.ERROR_SHIPPER_ITEM_KEY_LESS_THAN_ONE & "  [ItemKey={0}, Identifier={1}, Desc={2}]", itemKey, identifier, desc))
                Throw New Exception(ShipperMessages.ERROR_SHIPPER_ITEM_KEY_LESS_THAN_ONE)
            End If
            ' Check rules for converting an item to a Shipper.
            ' Exceptions thrown by this call should bubble up to original caller because this is part of constructing a Shipper.
            logger.Debug("Validating item can be marked as Shipper.")
            ValidateConvertItemToShipper(itemKey)

            ' Save Shipper info.
            _iItemKey = itemKey
            _sIdentifier = identifier.Trim
            _sDesc = desc.Trim

            ' Get data for this Shipper and the items it contains.
            Dim shipperDataReader As SqlDataReader = ShipperDAO.GetShipperInfo(itemKey)
            Try
                ' Populate Shipper attributes and build list of ShipperItem objects.
                ' It is okay if no rows are returned, because when an item is first marked as a Shipper,
                ' it will contain no items, so the Shipper-info query will return no rows.
                ' Example Data from GetShipperInfo SP:
                '    Shipper_Key	ShipperIdentifier	ShipperDesc	Item_Key	Identifier	Item_Description	Quantity	Subteam_No	SubteamName	Brand_ID	BrandName	RetailPackQty	RetailPackSize
                '    358613	8675309	Lux Tea Organic Shipper	188085	951	DVT ORG BONASPATI ASSAM TEA   	5.0000	320	320 COFFEE/TEA	6405	Divinitea	1.0000	16.0000
                '    358613	8675309	Lux Tea Organic Shipper	188092	1007	ROT GINGER PEACH TEABAGS	5.0000	320	320 COFFEE/TEA	606	Republic of Tea	1.0000	1.0000
                With shipperDataReader
                    ' Let all exceptions bubble up?
                    If .HasRows Then
                        Do While .Read()
                            ' Create a ShipperItem, passing in the parent/Shipper item key so it knows who it belongs to.
                            Dim _shipperItem As ShipperItem = New ShipperItem(_iItemKey, _
                                .GetInt32(.GetOrdinal("Item_Key")), _
                                .GetString(.GetOrdinal("Identifier")), _
                                .GetString(.GetOrdinal("Item_Description")), _
                                .GetDecimal(.GetOrdinal("Quantity")))
                            ' Add it to internal list.
                            _htItemList.Add(_shipperItem.ItemKey, _shipperItem)
                            logger.Debug("Shipper internal hashtable count: " & _htItemList.Count)
                            logger.Debug("ShipperItem obj: " & _shipperItem.ToString)
                        Loop
                    Else
                        ' No rows returned from query, but if we are here, there were no errors either, so we assume our Shipper is empty
                    End If
                End With
            Catch ex As Exception
                logger.Error(String.Format(ShipperMessages.ERROR_SHIPPER_CANNOT_BUILD_DB_ERROR & "  [ItemKey={0}, Identifier={1}, Desc={2}]", itemKey, identifier, desc), ex)
                Throw New Exception(ShipperMessages.ERROR_SHIPPER_CANNOT_BUILD_DB_ERROR, ex)
            Finally
                shipperDataReader.Close()
            End Try

        End Sub

        ''' <summary>
        ''' Returns a string representation of this Shipper.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function ToString() As String
            '  Dim shipperInfo As String
            ' Build complete string by appending info for each ShipperItem (call ToString).
            Return String.Format("Shipper: ItemKey={0}, Identifier={1}, Description={2}", Me.ItemKey, Me.Identifier, Me.Desc)
        End Function

        ''' <summary>
        ''' Updates pack qty and size for this Shipper, if necessary.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UpdateInfo(ByVal itemKey As Integer)
            ' TODO: Loop through all ShipperItem objects and check if qty for any of them has changed?  This would save a DB hit.
            ShipperDAO.UpdatePackInfo(itemKey)
        End Sub

        ''' <summary>
        ''' Checks rules for adding the specified item to this Shipper.
        ''' </summary>
        ''' <param name="itemKey">Item key of item to be added.</param>
        ''' <remarks>This will throw exceptions if rules are violated, so caller must handle.</remarks>
        Public Sub ValidateAddItem(ByVal itemKey As Integer)
            ' Make sure the item isn't already in the Shipper.
            If ItemExists(itemKey) Then
                Throw New Exception(ShipperMessages.ERROR_SHIPPER_ALREADY_CONTAINS_ITEM)
            End If

            ' Make sure the selected item is not a Shipper.
            If IsItemAShipper(itemKey) Then
                Throw New Exception(ShipperMessages.ERROR_SHIPPER_CANNOT_ADD_ITEM_IS_SHIPPER)
            End If
        End Sub

#End Region

#Region "Private Methods"


#End Region

    End Class

End Namespace
