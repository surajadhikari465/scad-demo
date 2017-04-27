Imports WholeFoods.IRMA.Pricing.DataAccess
Imports log4net

Public Enum StoreStatus
    Valid
    Error_InvalidEXEWarehouse
    Error_StoreJurisdictionChangeAndBatchExists
End Enum

Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic
    Public Class StoreBO

        Private _storeNo As Integer
        Private _storeName As String
        Private _psiStoreNo As Integer
        Private _storeAbbr As String
        Private _phoneNumber As String
        Private _zoneID As Integer
        Private _businessUnitID As Integer
        Private _unfiStore As String
        Private _exeWarehouse As String 'this is an Int16 value, but made a String so empty data won't default to 0
        Private _internal As Boolean
        Private _regional As Boolean
        Private _megaStore As Boolean
        Private _wfmStore As Boolean
        Private _distributionCenter As Boolean
        Private _manufacturer As Boolean
        Private _taxJurisdictionID As Integer
        Private _originalStoreJurisdictionID As Integer
        Private _updatedStoreJurisdictionID As Integer
        Private _geoCode As String
        Private _PLUMStoreNo As Integer

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


#Region "Property Access Methods"

        Public Property StoreNo() As Integer
            Get
                Return _storeNo
            End Get
            Set(ByVal value As Integer)
                _storeNo = value
            End Set
        End Property
        Public Property PSIStoreNo() As Integer
            Get
                Return _psiStoreNo
            End Get
            Set(ByVal value As Integer)
                _psiStoreNo = value
            End Set
        End Property

        Public Property StoreName() As String
            Get
                Return _storeName
            End Get
            Set(ByVal value As String)
                _storeName = value
            End Set
        End Property

        Public Property StoreAbbr() As String
            Get
                Return _storeAbbr
            End Get
            Set(ByVal value As String)
                _storeAbbr = value
            End Set
        End Property

        Public Property PhoneNumber() As String
            Get
                Return _phoneNumber
            End Get
            Set(ByVal value As String)
                _phoneNumber = value
            End Set
        End Property

        Public Property ZoneID() As Integer
            Get
                Return _zoneID
            End Get
            Set(ByVal value As Integer)
                _zoneID = value
            End Set
        End Property

        Public Property BusinessUnitID() As Integer
            Get
                Return _businessUnitID
            End Get
            Set(ByVal value As Integer)
                _businessUnitID = value
            End Set
        End Property

        Public Property UNFIStore() As String
            Get
                Return _unfiStore
            End Get
            Set(ByVal value As String)
                _unfiStore = value
            End Set
        End Property

        Public Property EXEWarehouse() As String
            Get
                Return _exeWarehouse
            End Get
            Set(ByVal value As String)
                _exeWarehouse = value
            End Set
        End Property

        Public Property IsInternal() As Boolean
            Get
                Return _internal
            End Get
            Set(ByVal value As Boolean)
                _internal = value
            End Set
        End Property

        Public Property IsRegional() As Boolean
            Get
                Return _regional
            End Get
            Set(ByVal value As Boolean)
                _regional = value
            End Set
        End Property

        Public Property IsMegaStore() As Boolean
            Get
                Return _megaStore
            End Get
            Set(ByVal value As Boolean)
                _megaStore = value
            End Set
        End Property

        Public Property IsWFMStore() As Boolean
            Get
                Return _wfmStore
            End Get
            Set(ByVal value As Boolean)
                _wfmStore = value
            End Set
        End Property

        Public Property IsDistributionCenter() As Boolean
            Get
                Return _distributionCenter
            End Get
            Set(ByVal value As Boolean)
                _distributionCenter = value
            End Set
        End Property

        Public Property IsManufacturer() As Boolean
            Get
                Return _manufacturer
            End Get
            Set(ByVal value As Boolean)
                _manufacturer = value
            End Set
        End Property

        Public Property TaxJurisdictionID() As Integer
            Get
                Return _taxJurisdictionID
            End Get
            Set(ByVal value As Integer)
                _taxJurisdictionID = value
            End Set
        End Property

        Public Property OriginalStoreJurisdictionID() As Integer
            Get
                Return _originalStoreJurisdictionID
            End Get
            Set(ByVal value As Integer)
                _originalStoreJurisdictionID = value
            End Set
        End Property

        Public Property UpdatedStoreJurisdictionID() As Integer
            Get
                Return _updatedStoreJurisdictionID
            End Get
            Set(ByVal value As Integer)
                _updatedStoreJurisdictionID = value
            End Set
        End Property
        Public Property GeoCode() As String
            Get
                Return _geoCode
            End Get
            Set(ByVal value As String)
                _geoCode = Trim(value)
            End Set
        End Property
        Public Property PLUMStoreNo() As Integer
            Get
                Return _PLUMStoreNo
            End Get
            Set(value As Integer)
                _PLUMStoreNo = value
            End Set
        End Property
#End Region

#Region "Business Rules"

        ''' <summary>
        ''' validates that EXEWarehouse value entered can be converted to a SmallInt
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ValidateExeWarehouse(ByVal strEXEWarehouse As String) As StoreStatus

            logger.Debug("ValidateExeWarehouse entry with strEXEWarehouse = " + strEXEWarehouse)
            Dim status As StoreStatus
            Dim tempInt16 As Int16

            Try
                If strEXEWarehouse IsNot Nothing AndAlso Not strEXEWarehouse.Equals("") Then
                    'attempt to convert string value to Int16
                    tempInt16 = CType(strEXEWarehouse, Int16)
                    Me.EXEWarehouse = strEXEWarehouse
                Else
                    Me.EXEWarehouse = Nothing
                End If
                status = StoreStatus.Valid
            Catch ex As Exception
                status = StoreStatus.Error_InvalidEXEWarehouse
            End Try

            logger.Debug("ValidateExeWarehouse Exit")

            Return status
        End Function

        ''' <summary>
        ''' validates data elements of current instance of StoreBO object
        ''' </summary>
        ''' <returns>ArrayList of StoreStatus values</returns>
        ''' <remarks></remarks>
        Public Function ValidateData(ByVal strEXEWarehouse As String) As ArrayList

            logger.Debug("ValidateData entry with strEXEWarehouse = " + strEXEWarehouse)

            Dim statusList As New ArrayList

            ' EXE warehouse can be converted to a SmallInt
            Dim exeStatus As StoreStatus = ValidateExeWarehouse(strEXEWarehouse)
            If exeStatus <> StoreStatus.Valid Then
                statusList.Add(exeStatus)
            End If

            ' Store Jurisdiction cannot be changed if there are any unprocessed batches assigned to the store
            If (_originalStoreJurisdictionID <> _updatedStoreJurisdictionID) AndAlso (PriceBatchDetailDAO.CheckForPendingPriceBatchesByStore(_storeNo.ToString, ","c, BatchStatus.AllButProcessed)) Then
                statusList.Add(StoreStatus.Error_StoreJurisdictionChangeAndBatchExists)
            End If

            If statusList.Count = 0 Then
                statusList.Add(StoreStatus.Valid)
            End If

            logger.Debug("ValidateData Exit")

            Return statusList
        End Function
#End Region

    End Class
End Namespace