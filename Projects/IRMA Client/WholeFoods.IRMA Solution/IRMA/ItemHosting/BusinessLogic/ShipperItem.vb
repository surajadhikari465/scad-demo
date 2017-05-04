Imports log4net
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Class ShipperItem

#Region "Public Constants"

        ''' <summary>
        ''' Initial unit quantity for an item when it is first added to a Shipper.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SHIPPERITEM_INITIAL_UNIT_QTY As Integer = 1

#End Region

#Region "Private Members"

        ''' <summary>
        ''' Log4Net logger for this class.
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#End Region

#Region "Property Declarations"

        ''' <summary>
        ''' Unique item key for the parent Shipper of this ShipperItem.
        ''' </summary>
        ''' <remarks></remarks>
        Private _iParentItemKey As Integer

        ''' <summary>
        ''' Unique item key of this item.
        ''' </summary>
        ''' <remarks></remarks>
        Private _iItemKey As Integer

        ''' <summary>
        ''' UPC/identifier of this item.
        ''' </summary>
        ''' <remarks></remarks>
        Private _sIdentifier As String

        ''' <summary>
        ''' Description of this item.
        ''' </summary>
        ''' <remarks></remarks>
        Private _sDesc As String

        ''' <summary>
        ''' Total units of this item.
        ''' </summary>
        ''' <remarks></remarks>
        Private _dQty As Integer

        ''' <summary>
        ''' Marks when information within this ShipperItem has been updated.
        ''' </summary>
        ''' <remarks></remarks>
        Private _bHasChanged As Boolean

#End Region

#Region "Property Access Methods"

        ''' <summary>
        ''' Unique item key for the parent/owner of this ShipperItem.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ParentItemKey() As Integer
            Get
                Return _iParentItemKey
            End Get
        End Property

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
        ''' Item identifier for this Shipper.
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
        ''' Get or set the unit qty for this item in a Shipper.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>This will throw exceptions if the input is invalid, so the caller must handle.</remarks>
        Public Property Qty() As Decimal
            Get
                Return _dQty
            End Get
            Set(ByVal value As Decimal)
                ' Qty of an item in a Shipper should be > 0.
                If value <= 0 Then
                    logger.Error(String.Format(ShipperMessages.ERROR_SHIPPERITEM_QTY_LESS_THAN_ONE & "  [ItemKey={0}, ShipperItemKey={1}, BadQty={2}]", Me.ItemKey, Me.ParentItemKey, value))
                    Throw New Exception(ShipperMessages.ERROR_SHIPPERITEM_QTY_LESS_THAN_ONE)
                End If
                If value = _dQty Then Exit Property
                ' Save value and mark as changed.
                _dQty = value
                _bHasChanged = True
            End Set
        End Property

        ''' <summary>
        ''' Returns whether or not this ShipperItem has been updated.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property HasChanged() As Boolean
            Get
                Return _bHasChanged
            End Get
        End Property

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Creates a ShipperItem.
        ''' Exceptions are thrown if rules surrounding the attributes of the ShipperItem are broken and the caller must handle.
        ''' </summary>
        ''' <param name="itemKey">Unique item key for the ShipperItem being created.</param>
        ''' <param name="identifier">UPC or item identifier for the ShipperItem.</param>
        ''' <param name="desc">Item description for the ShipperItem.</param>
        ''' <param name="qty">Number of units of the ShipperItem in the Shipper.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal shipperKey As Integer, ByVal itemKey As Integer, ByVal identifier As String, ByVal desc As String, ByVal qty As Integer)
            ' Validation happens here, rather than in property-set methods, because these attributes are read-only (no set method).
            If Not shipperKey > 0 Then
                logger.Error(String.Format(ShipperMessages.ERROR_SHIPPER_ITEM_KEY_LESS_THAN_ONE & "  [ShipperKey={0}, ItemKey={1}, Identifier={2}, Desc={3}, Qty={4}]", shipperKey, itemKey, identifier, desc, qty))
                Throw New Exception(ShipperMessages.ERROR_SHIPPER_ITEM_KEY_LESS_THAN_ONE)
            End If
            _iParentItemKey = shipperKey

            If Not itemKey > 0 Then
                logger.Error(String.Format(ShipperMessages.ERROR_SHIPPERITEM_ITEM_KEY_LESS_THAN_ONE & "  [ShipperKey={0}, ItemKey={1}, Identifier={2}, Desc={3}, Qty={4}]", shipperKey, itemKey, identifier, desc, qty))
                Throw New Exception(ShipperMessages.ERROR_SHIPPERITEM_ITEM_KEY_LESS_THAN_ONE)
            End If
            _iItemKey = itemKey

            If String.IsNullOrEmpty(identifier) Then
                logger.Error(String.Format(ShipperMessages.ERROR_SHIPPERITEM_INVALID_IDENTIFIER & "  [ShipperKey={0}, ItemKey={1}, Identifier={2}, Desc={3}, Qty={4}]", shipperKey, itemKey, identifier, desc, qty))
                Throw New Exception(ShipperMessages.ERROR_SHIPPERITEM_INVALID_IDENTIFIER)
            End If
            If identifier.Length > ItemBO.ITEM_IDENTIFIER_MAX_LENGTH Then
                logger.Error(String.Format(ShipperMessages.ERROR_SHIPPERITEM_IDENTIFIER_TOO_LONG & "  [ShipperKey={0}, ItemKey={1}, Identifier={2}, Desc={3}, Qty={4}]", shipperKey, itemKey, identifier, desc, qty))
                Throw New Exception(ShipperMessages.ERROR_SHIPPERITEM_IDENTIFIER_TOO_LONG)
            End If
            _sIdentifier = identifier.Trim

            If String.IsNullOrEmpty(desc) Then
                logger.Error(String.Format(ShipperMessages.ERROR_SHIPPERITEM_INVALID_DESC & "  [ShipperKey={0}, ItemKey={1}, Identifier={2}, Desc={3}, Qty={4}]", shipperKey, itemKey, identifier, desc, qty))
                Throw New Exception(ShipperMessages.ERROR_SHIPPERITEM_INVALID_DESC)
            End If
            _sDesc = desc.Trim

            ' Read/Write attributes.

            ' We use the property accessors here because they should contains the validation logic.
            Me.Qty = qty
        End Sub

#End Region

#Region "Public Instance Methods"

        ''' <summary>
        ''' Returns a string containing details about this ShipperItem.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function ToString() As String
            Return String.Format("ShipperItem: ItemKey={0}, Identifier={1}, Description={2}, Quantity={3}", Me.ItemKey, Me.Identifier, Me.Desc, Me.Qty)
        End Function

        Public Sub Save()
            ShipperDAO.UpdateShipperItemQty(_iParentItemKey, _iItemKey, Qty)
            _bHasChanged = False
        End Sub

#End Region

    End Class

End Namespace
