Imports WholeFoods.Utility
Imports System.Data.SqlClient

' The values in this enumeration must match the database POSChangeType.POSChangeTypeKey values
Public Enum POSChangeType
    ItemIDDelete = 1
    ItemIDAdd = 2
    ItemDelete = 3
    ItemChange = 4
    PromotionalData = 5
    VendorAdd = 6
    CorpScaleItemIdAdd = 7
    CorpScaleItemIdDelete = 8
    CorpScaleItemChange = 9
    ZoneScaleItemDelete = 10
    ZoneScalePriceChange = 11
    ShelfTagFile = 12
    NutriFact = 13
    ExtraText = 14
    ZoneScaleSmartXPriceChange = 15
    ElectronicShelfTag = 21
End Enum

Namespace WholeFoods.IRMA.Administration.POSPush.BusinessLogic
    Public Class POSChangeTypeBO
#Region "Property Definitions"
        Private _posChangeTypeKey As Integer
        Private _posDataTypeKey As Integer
        Private _changeTypeDesc As String
#End Region

#Region "Constructors"
        ''' <summary>
        ''' Create a new instance of the object, populated from a result set.
        ''' </summary>
        ''' <param name="results"></param>
        ''' <remarks></remarks>
        Public Sub New(ByRef results As SqlDataReader)
            ' Assign values to the properties
            If (Not results.IsDBNull(results.GetOrdinal("POSChangeTypeKey"))) Then
                _posChangeTypeKey = results.GetInt32(results.GetOrdinal("POSChangeTypeKey"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("POSDataTypeKey"))) Then
                _posDataTypeKey = results.GetInt32(results.GetOrdinal("POSDataTypeKey"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ChangeTypeDesc"))) Then
                _changeTypeDesc = results.GetString(results.GetOrdinal("ChangeTypeDesc"))
            End If
        End Sub
#End Region

#Region "Property access methods"
        Public Property POSChangeTypeKey() As Integer
            Get
                Return _posChangeTypeKey
            End Get
            Set(ByVal value As Integer)
                _posChangeTypeKey = value
            End Set
        End Property

        Public Property POSDataTypeKey() As Integer
            Get
                Return _posDataTypeKey
            End Get
            Set(ByVal value As Integer)
                _posDataTypeKey = value
            End Set
        End Property

        Public Property ChangeTypeDesc() As String
            Get
                Return _changeTypeDesc
            End Get
            Set(ByVal value As String)
                _changeTypeDesc = value
            End Set
        End Property

#End Region
    End Class
End Namespace
