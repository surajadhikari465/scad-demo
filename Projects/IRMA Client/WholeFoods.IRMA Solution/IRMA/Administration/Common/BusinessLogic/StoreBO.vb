Imports log4net
Imports System.Data.SqlClient

Namespace WholeFoods.IRMA.Administration.Common.BusinessLogic
    Public Class StoreBO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Property Definitions"
        Private _storeNo As Integer
        Private _storeName As String
        Private _phoneNo As String
        Private _regionalOffice As Boolean
        Private _posSystem As POSSystemType
        Private _zoneID As Integer
        Private _state As String
        Private _BusinessUnitId As Integer?
#End Region

#Region "constructors and helper methods to initialize the data"
        Public Sub New()
        End Sub

        Public Sub New(ByRef results As SqlDataReader)
            Me.New(results, False)
        End Sub

        Public Sub New(ByRef results As SqlDataReader, ByVal retailStoreResult As Boolean)
            logger.Debug("New entry with SQL results")

            If (Not results.IsDBNull(results.GetOrdinal("Store_No"))) Then
                _storeNo = results.GetInt32(results.GetOrdinal("Store_No"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("Store_Name"))) Then
                _storeName = results.GetString(results.GetOrdinal("Store_Name"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("BusinessUnit_Id"))) Then
                _BusinessUnitId = results.GetInt32(results.GetOrdinal("BusinessUnit_id"))
            End If

            If Not retailStoreResult Then
                If Not results.IsDBNull(results.GetOrdinal("POSSystemId")) AndAlso Not results.IsDBNull(results.GetOrdinal("POSSystemType")) Then
                    Dim system As New POSSystemType
                    system.POSSystemID = results.GetInt32(results.GetOrdinal("POSSystemId"))
                    system.POSSystemType = results.GetString(results.GetOrdinal("POSSystemType"))

                    _posSystem = system
                End If
            Else
                ' Populate the zone and state values
                If (Not results.IsDBNull(results.GetOrdinal("Zone_Id"))) Then
                    _zoneID = results.GetInt32(results.GetOrdinal("Zone_Id"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("State"))) Then
                    _state = results.GetString(results.GetOrdinal("State"))
                End If
            End If

            logger.Debug("New exit")
        End Sub

        ''' <summary>
        ''' Create a new instance of the object, populated from the DataGridViewRow
        ''' for a selected row on the UI.
        ''' </summary>
        ''' <param name="selectedRow"></param>
        ''' <remarks></remarks>
        Public Sub New(ByRef selectedRow As DataGridViewRow)
            logger.Debug("New entry with DataGridViewRow")
            _storeNo = CType(selectedRow.Cells("StoreNo").Value, Integer)
            _storeName = CType(selectedRow.Cells("StoreName").Value, String)

            If selectedRow.Cells("POSSystem").Value IsNot Nothing Then
                _posSystem = CType(selectedRow.Cells("POSSystem").Value, POSSystemType)
            End If
            logger.Debug("New exit")
        End Sub
#End Region

#Region "Property access methods"
        Public Property StoreNo() As Integer
            Get
                Return _storeNo
            End Get
            Set(ByVal value As Integer)
                _storeNo = value
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

        Public Property PhoneNo() As String
            Get
                Return _phoneNo
            End Get
            Set(ByVal value As String)
                _phoneNo = value
            End Set
        End Property

        Public Property RegionalOffice() As Boolean
            Get
                Return _regionalOffice
            End Get
            Set(ByVal value As Boolean)
                _regionalOffice = value
            End Set
        End Property

        Public Property POSSystem() As POSSystemType
            Get
                Return _posSystem
            End Get
            Set(ByVal value As POSSystemType)
                _posSystem = value
            End Set
        End Property

        Public Property ZoneId() As Integer
            Get
                Return _zoneID
            End Get
            Set(ByVal value As Integer)
                _zoneID = value
            End Set
        End Property

        Public Property State() As String
            Get
                Return _state
            End Get
            Set(ByVal value As String)
                _state = value
            End Set
        End Property

        Public Property BusinessUnitId() As Integer?
            Get
                Return _BusinessUnitId
            End Get
            Set(ByVal value As Integer?)
                _BusinessUnitId = value
            End Set
        End Property

#End Region

    End Class
End Namespace

