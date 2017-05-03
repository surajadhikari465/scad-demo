Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.Utility
Imports System.Data.SqlClient

Namespace WholeFoods.IRMA.Administration.POSPush.BusinessLogic

    Public Enum StoreScaleConfigStatus
        Valid
        Error_Required_ScaleWriterType
        Error_Required_Store
    End Enum

    Public Class StoreScaleConfigBO

#Region "Property Definitions"
        Private _storeConfig As New StoreBO
        ''' <summary>
        ''' The StoreScaleConfig.ScaleFileWriterKey value for this store.
        ''' </summary>
        ''' <remarks></remarks>
        Private _scaleFileWriterKey As String
        ''' <summary>
        ''' The POSWriter.POSFileWriterCode value for this writer.
        ''' </summary>
        ''' <remarks></remarks>
        Private _scaleFileWriterCode As String
        ''' <summary>
        ''' The POSWriter.ScaleWriterType value for this writer.
        ''' </summary>
        ''' <remarks></remarks>
        Private _scaleWriterType As Integer
        ''' <summary>
        ''' The ScaleWriterType.ScaleWriterTypeDesc for this writer.
        ''' </summary>
        ''' <remarks></remarks>
        Private _scaleWriterTypeDesc As String
#End Region

#Region "Property access methods"
        Public Property StoreConfig() As StoreBO
            Get
                Return _storeConfig
            End Get
            Set(ByVal value As StoreBO)
                _storeConfig = value
            End Set
        End Property

        Public Property ScaleFileWriterKey() As String
            Get
                Return _scaleFileWriterKey
            End Get
            Set(ByVal value As String)
                _scaleFileWriterKey = value
            End Set
        End Property

        Public Property ScaleFileWriterCode() As String
            Get
                Return _scaleFileWriterCode
            End Get
            Set(ByVal value As String)
                _scaleFileWriterCode = value
            End Set
        End Property

        Public Property ScaleWriterType() As Integer
            Get
                Return _scaleWriterType
            End Get
            Set(ByVal value As Integer)
                _scaleWriterType = value
            End Set
        End Property

        Public Property ScaleWriterTypeDesc() As String
            Get
                Return _scaleWriterTypeDesc
            End Get
            Set(ByVal value As String)
                _scaleWriterTypeDesc = value
            End Set
        End Property

#End Region

#Region "constructors and helper methods to initialize the data"
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Create a new instance of the object, populated from the DataGridViewRow
        ''' for a selected row on the UI.
        ''' </summary>
        ''' <param name="selectedRow"></param>
        ''' <remarks></remarks>
        Public Sub New(ByRef selectedRow As DataGridViewRow)
            _storeConfig.StoreNo = CType(selectedRow.Cells("Store_No").Value, Integer)
            _storeConfig.StoreName = CType(selectedRow.Cells("Store_Name").Value, String)
            If selectedRow.Cells("ScaleFileWriterKey").Value IsNot DBNull.Value Then
                _scaleFileWriterKey = CType(selectedRow.Cells("ScaleFileWriterKey").Value, String)
            End If
            If selectedRow.Cells("ScaleFileWriterCode").Value IsNot DBNull.Value Then
                _scaleFileWriterCode = CType(selectedRow.Cells("ScaleFileWriterCode").Value, String)
            End If
            If selectedRow.Cells("ScaleWriterType").Value IsNot DBNull.Value Then
                _scaleWriterType = CType(selectedRow.Cells("ScaleWriterType").Value, Integer)
            End If
            If selectedRow.Cells("ScaleWriterTypeDesc").Value IsNot DBNull.Value Then
                _scaleWriterTypeDesc = CType(selectedRow.Cells("ScaleWriterTypeDesc").Value, String)
            End If
        End Sub

        ''' <summary>
        ''' Create a new instance of the object, populated from the POSGetStoreWriterConfigurations
        ''' result set.
        ''' </summary>
        ''' <param name="results"></param>
        ''' <remarks></remarks>
        Public Sub New(ByRef results As SqlDataReader)
            ' Assign values to the properties
            If (Not results.IsDBNull(results.GetOrdinal("Store_No"))) Then
                _storeConfig.StoreNo = results.GetInt32(results.GetOrdinal("Store_No"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Store_Name"))) Then
                _storeConfig.StoreName = results.GetString(results.GetOrdinal("Store_Name"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ScaleFileWriterKey"))) Then
                _scaleFileWriterKey = results.GetInt32(results.GetOrdinal("ScaleFileWriterKey")).ToString()
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ScaleFileWriterCode"))) Then
                _scaleFileWriterCode = results.GetString(results.GetOrdinal("ScaleFileWriterCode"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ScaleWriterType"))) Then
                _scaleWriterType = results.GetInt32(results.GetOrdinal("ScaleWriterType"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ScaleWriterTypeDesc"))) Then
                _scaleWriterTypeDesc = results.GetString(results.GetOrdinal("ScaleWriterTypeDesc"))
            End If
        End Sub

#End Region

    End Class
End Namespace

