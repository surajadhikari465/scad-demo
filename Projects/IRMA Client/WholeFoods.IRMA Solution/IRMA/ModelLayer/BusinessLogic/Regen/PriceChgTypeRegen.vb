Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

	''' <summary>
	''' Generated Business object base class for the PriceChgType db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Mar 27, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class PriceChgTypeRegen
		Inherits BusinessObjectBase
	
#Region "Persistent Fields and Properties"

		Private _priceChgTypeID As System.Byte
		Private _isPriceChgTypeIDNull As Boolean = True
		Private _priceChgTypeDesc As System.String
		Private _isPriceChgTypeDescNull As Boolean = True
		Private _priority As System.Int16
		Private _isPriorityNull As Boolean = True
		Private _onSale As System.Boolean
		Private _isOnSaleNull As Boolean = True
		Private _mSRPRequired As System.Boolean
		Private _isMSRPRequiredNull As Boolean = True
		Private _lineDrive As System.Boolean
        Private _isLineDriveNull As Boolean = True
        Private _isCompetitive As System.Boolean
        Private _isCompetitiveNull As Boolean = True
        Private _LastUpdateTimestamp As System.DateTime = Today

		Public Overridable Property PriceChgTypeID() As System.Byte
		    Get
				Return _priceChgTypeID
		    End Get
		    Set(ByVal value As System.Byte)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsPriceChgTypeIDNull = False
				If _priceChgTypeID <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "PriceChgTypeID", theOldValue, value)
				End If
				
				' set the abstracted key property
				' which is used to add BusinessObjects to BusinessObjectCollections
				Me.PrimaryKey = value
			
				_priceChgTypeID = value
		    End Set
		End Property

		Public Overridable Property IsPriceChgTypeIDNull() As Boolean
		    Get
				Return _isPriceChgTypeIDNull
		    End Get
		    Set(ByVal value As Boolean)
				_isPriceChgTypeIDNull = value
		    End Set
		End Property

		Public Overridable Property PriceChgTypeDesc() As System.String
		    Get
				Return _priceChgTypeDesc
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsPriceChgTypeDescNull = IsNothing(value)
				If (IsNothing(_priceChgTypeDesc) And Not IsNothing(value)) Or _
						(Not IsNothing(_priceChgTypeDesc) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_priceChgTypeDesc) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _priceChgTypeDesc.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "PriceChgTypeDesc", theOldValue, value)
				End If
				
			
				_priceChgTypeDesc = value
		    End Set
		End Property

		Public Overridable Property IsPriceChgTypeDescNull() As Boolean
		    Get
				Return _isPriceChgTypeDescNull
		    End Get
		    Set(ByVal value As Boolean)
				_isPriceChgTypeDescNull = value
		    End Set
		End Property

		Public Overridable Property Priority() As System.Int16
		    Get
				Return _priority
		    End Get
		    Set(ByVal value As System.Int16)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsPriorityNull = False
				If _priority <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "Priority", theOldValue, value)
				End If
				
			
				_priority = value
		    End Set
		End Property

		Public Overridable Property IsPriorityNull() As Boolean
		    Get
				Return _isPriorityNull
		    End Get
		    Set(ByVal value As Boolean)
				_isPriorityNull = value
		    End Set
		End Property

		Public Overridable Property OnSale() As System.Boolean
		    Get
				Return _onSale
		    End Get
		    Set(ByVal value As System.Boolean)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsOnSaleNull = False
				If _onSale <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "OnSale", theOldValue, value)
				End If
				
			
				_onSale = value
		    End Set
		End Property

		Public Overridable Property IsOnSaleNull() As Boolean
		    Get
				Return _isOnSaleNull
		    End Get
		    Set(ByVal value As Boolean)
				_isOnSaleNull = value
		    End Set
		End Property

		Public Overridable Property MSRPRequired() As System.Boolean
		    Get
				Return _mSRPRequired
		    End Get
		    Set(ByVal value As System.Boolean)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsMSRPRequiredNull = False
				If _mSRPRequired <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "MSRPRequired", theOldValue, value)
				End If
				
			
				_mSRPRequired = value
		    End Set
		End Property

		Public Overridable Property IsMSRPRequiredNull() As Boolean
		    Get
				Return _isMSRPRequiredNull
		    End Get
		    Set(ByVal value As Boolean)
				_isMSRPRequiredNull = value
		    End Set
		End Property

		Public Overridable Property LineDrive() As System.Boolean
		    Get
				Return _lineDrive
		    End Get
		    Set(ByVal value As System.Boolean)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsLineDriveNull = False
				If _lineDrive <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "LineDrive", theOldValue, value)
				End If
				
			
				_lineDrive = value
		    End Set
		End Property

		Public Overridable Property IsLineDriveNull() As Boolean
		    Get
				Return _isLineDriveNull
		    End Get
		    Set(ByVal value As Boolean)
				_isLineDriveNull = value
		    End Set
		End Property

        Public Overridable Property Competetive() As System.Boolean
            Get
                Return _isCompetitive
            End Get
            Set(ByVal value As System.Boolean)

                Dim theOldValue As Object = Nothing

                ' set the IsDirty and is null flags
                Me._isCompetitiveNull = False
                If _isCompetitive <> value Then
                    Me.IsDirty = True
                End If

                If Me.IsDirty Then
                    RaisePropertyChangedEvent(Me, "Competetive", theOldValue, value)
                End If

                _isCompetitive = value
            End Set
        End Property

        Public Overridable Property IsCompetetiveNull() As Boolean
            Get
                Return _isCompetitiveNull
            End Get
            Set(ByVal value As Boolean)
                _isCompetitiveNull = value
            End Set
        End Property

        Public Overridable Property LastUpdateTimestamp() As System.DateTime
            Get
                Return _lastUpdateTimestamp
            End Get
            Set(value As System.DateTime)

                Dim theOldValue As Object = Nothing

                If _LastUpdateTimestamp <> value Then
                    Me.IsDirty = True
                End If

                If Me.IsDirty Then
                    RaisePropertyChangedEvent(Me, "LastUpdateTimestamp", theOldValue, value)
                End If

                _LastUpdateTimestamp = value
            End Set
        End Property
#End Region

#Region "Non-persistent Fields and Properties"

		
#End Region

#Region "Constructors"

		Public Sub New()
		End Sub
		

#End Region

#Region "Just-in-time Instantiated Parents"

#End Region

#Region "Just-in-time Instantiated Children Collections"

#End Region

#Region "CRUD Methods"

       ''' <summary>
        ''' Save this PriceChgType
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Save() As Boolean
		
			If Me.IsDirty And Not Me.IsDeleted Then
				If Me.IsNew Then
					PriceChgTypeDAO.Instance.InsertPriceChgType(CType(Me, PriceChgType))
					Trace.WriteLine("Inserting a new PriceChgType")
				Else
					PriceChgTypeDAO.Instance.UpdatePriceChgType(CType(Me, PriceChgType))
					Trace.WriteLine("Updating an existing PriceChgType")
				End If
				
				Me.IsDirty = False
				Me.IsNew = False
			End If
			
		
		
		End Function

       ''' <summary>
        ''' Delete this PriceChgType
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Delete(ByVal onlyMarked As Boolean) As Boolean
 
			Dim childCollectionIndex As Integer = 0
			
            If Not onlyMarked Or (onlyMarked And Me.IsMarkedForDelete) Then
                If Not Me.IsNew Then
                    PriceChgTypeDAO.Instance.DeletePriceChgType(CType(Me, PriceChgType))
                    Trace.WriteLine("Deleting a PriceChgType.")
                Else
                    Trace.WriteLine("Removing a new unsaved PriceChgType.")
                End If

                Me.IsDeleted = True
                Me.IsMarkedForDelete = False

            End If
		

		End Function

#End Region

#Region "Public Methods"

       ''' <summary>
        ''' Make this PriceChgType  and all of its
		''' decedents new so it
		''' will be inserted into the database when it is saved.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub MakeNew()
			
			Me.IsDirty = True
			Me.IsNew = True
		
		End Sub

#End Region

#Region "Private Methods"

        Protected Overrides Function GetNewInstance() As BusinessObjectBase
            Return New PriceChgType()
        End Function

#End Region

	End Class

End Namespace

