Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.Utility
Imports System.Data.SqlClient

Namespace WholeFoods.IRMA.Administration.POSPush.BusinessLogic

    Public Enum StorePOSConfigStatus
        Valid
        Error_Required_AcknowledgementType
        Error_Required_FileWriter
        Error_Required_Store
    End Enum

    Public Class StorePOSConfigBO

#Region "Property Definitions"
        Private _storeConfig As New StoreBO
        Private _configType As String
        Private _POSFileWriterKey As Integer
        Private _POSFileWriterCode As String
        Private _FileWriterType As String
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

        Public Property POSFileWriterKey() As Integer
            Get
                Return _POSFileWriterKey
            End Get
            Set(ByVal value As Integer)
                _POSFileWriterKey = value
            End Set
        End Property

        Public Property POSFileWriterCode() As String
            Get
                Return _POSFileWriterCode
            End Get
            Set(ByVal value As String)
                _POSFileWriterCode = value
            End Set
        End Property
        Public Property FileWriterType() As String
            Get
                Return _FileWriterType
            End Get
            Set(ByVal value As String)
                _FileWriterType = value
            End Set
        End Property
        Public Property ConfigType() As String
            Get
                Return _configType
            End Get
            Set(ByVal value As String)
                _configType = value
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
            _POSFileWriterKey = CType(selectedRow.Cells("POSFileWriterKey").Value, Integer)
            _POSFileWriterCode = CType(selectedRow.Cells("POSFileWriterCode").Value, String)
            _FileWriterType = CType(selectedRow.Cells("FileWriterType").Value, String)
            _configType = CType(selectedRow.Cells("ConfigType").Value, String)
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
            If (Not results.IsDBNull(results.GetOrdinal("POSFileWriterKey"))) Then
                _POSFileWriterKey = results.GetInt32(results.GetOrdinal("POSFileWriterKey"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("POSFileWriterCode"))) Then
                _POSFileWriterCode = results.GetString(results.GetOrdinal("POSFileWriterCode"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("FileWriterType"))) Then
                _FileWriterType = results.GetString(results.GetOrdinal("FileWriterType"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ConfigType"))) Then
                _configType = results.GetString(results.GetOrdinal("ConfigType"))
            End If
        End Sub

#End Region

        ''' <summary>
        ''' validates data elements of current instance of StorePOSConfigBO object
        ''' </summary>
        ''' <returns>ArrayList of StorePOSConfigStatus values</returns>
        ''' <remarks></remarks>
        Public Function ValidateStorePOSConfigData() As ArrayList
            Dim statusList As New ArrayList

            'store required
            If Me.StoreConfig.StoreNo < 0 Then
                statusList.Add(StorePOSConfigStatus.Error_Required_Store)
            End If

            'file writer required
            If Me.POSFileWriterKey < 0 Then
                statusList.Add(StorePOSConfigStatus.Error_Required_FileWriter)
            End If

            'acknowledgement type required
            If Me.ConfigType Is Nothing Or (Me.ConfigType IsNot Nothing AndAlso Me.ConfigType.Trim.Equals("")) Then
                statusList.Add(StorePOSConfigStatus.Error_Required_AcknowledgementType)
            End If

            If statusList.Count = 0 Then
                statusList.Add(StorePOSConfigStatus.Valid)
            End If

            Return statusList
        End Function

    End Class
End Namespace

