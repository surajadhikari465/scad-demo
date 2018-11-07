Imports Infragistics.Win.UltraWinGrid
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.Common.BusinessLogic

    Public Enum StoreFTPConfigStatus
        Valid
        Error_Required_FileWriterType
        Error_Required_IPAddress
        Error_Required_FTPUser
        Error_Required_FTPPassword
        Error_Port_Numeric
    End Enum

    Public Class StoreFTPConfigBO

        Private _storeNo As Integer
        Private _fileWriterType As String
        Private _ipAddress As String
        Private _ftpUser As String
        Private _ftpPassword As String
        Private _changeDirectory As String
        Private _port As String  'port set as String so as not to default to 0
        Private _isSecureTransfer As Boolean
        Private _posSystemType As String

#Region "Property Access Methods"

        Public Property StoreNo() As Integer
            Get
                Return _storeNo
            End Get
            Set(ByVal value As Integer)
                _storeNo = value
            End Set
        End Property

        Public Property FileWriterType() As String
            Get
                Return _fileWriterType
            End Get
            Set(ByVal value As String)
                _fileWriterType = value
            End Set
        End Property

        Public Property IPAddress() As String
            Get
                Return _ipAddress
            End Get
            Set(ByVal value As String)
                _ipAddress = value
            End Set
        End Property

        Public Property FTPUser() As String
            Get
                Return _ftpUser
            End Get
            Set(ByVal value As String)
                _ftpUser = value
            End Set
        End Property

        Public Property FTPPassword() As String
            Get
                Return _ftpPassword
            End Get
            Set(ByVal value As String)
                _ftpPassword = value
            End Set
        End Property

        Public Property ChangeDirectory() As String
            Get
                Return _changeDirectory
            End Get
            Set(ByVal value As String)
                _changeDirectory = value
            End Set
        End Property

        Public Property Port() As String
            Get
                Return _port
            End Get
            Set(ByVal value As String)
                _port = value
            End Set
        End Property

        Public Property IsSecureTransfer() As Boolean
            Get
                Return _isSecureTransfer
            End Get
            Set(ByVal value As Boolean)
                _isSecureTransfer = value
            End Set
        End Property

        Public Property POSSystemType() As String
            Get
                Return _posSystemType
            End Get
            Set(ByVal value As String)
                _posSystemType = value
            End Set
        End Property

        Public Property BusinessUnitID As Int32
#End Region

#Region "Constructors"

        Public Sub New()

        End Sub

        Public Sub New(ByRef results As SqlDataReader)
            Logger.LogDebug("New entry", Me.GetType())

            Try
                If (Not results.IsDBNull(results.GetOrdinal("Store_No"))) Then
                    _storeNo = results.GetInt32(results.GetOrdinal("Store_No"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("FileWriterType"))) Then
                    _fileWriterType = results.GetString(results.GetOrdinal("FileWriterType"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("IP_Address"))) Then
                    _ipAddress = results.GetString(results.GetOrdinal("IP_Address"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("FTP_User"))) Then
                    _ftpUser = results.GetString(results.GetOrdinal("FTP_User"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("FTP_Password"))) Then
                    _ftpPassword = results.GetString(results.GetOrdinal("FTP_Password"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("ChangeDirectory"))) Then
                    _changeDirectory = results.GetString(results.GetOrdinal("ChangeDirectory"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("Port"))) Then
                    _port = results.GetInt32(results.GetOrdinal("Port")).ToString
                End If

                If (Not results.IsDBNull(results.GetOrdinal("IsSecureTransfer"))) Then
                    _isSecureTransfer = results.GetBoolean(results.GetOrdinal("IsSecureTransfer"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("POSSystemType"))) Then
                    _posSystemType = results.GetString(results.GetOrdinal("POSSystemType"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("BusinessUnit_ID"))) Then
                    BusinessUnitID = results.GetInt32(results.GetOrdinal("BusinessUnit_ID"))
                End If
            Catch ex As Exception
                Throw ex
            End Try

            Logger.LogDebug("New exit", Me.GetType())
        End Sub

        Public Sub New(ByVal selectedRow As UltraGridRow)
            If selectedRow IsNot Nothing Then
                If selectedRow.Cells("StoreNo").Value IsNot Nothing Then
                    _storeNo = CType(selectedRow.Cells("StoreNo").Value, Integer)
                End If
                If selectedRow.Cells("FileWriterType").Value IsNot Nothing Then
                    _fileWriterType = selectedRow.Cells("FileWriterType").Value.ToString
                End If
                If selectedRow.Cells("IPAddress").Value IsNot Nothing Then
                    _ipAddress = selectedRow.Cells("IPAddress").Value.ToString
                End If
                If selectedRow.Cells("FTPUser").Value IsNot Nothing Then
                    _ftpUser = selectedRow.Cells("FTPUser").Value.ToString
                End If
                If selectedRow.Cells("FTPPassword").Value IsNot Nothing Then
                    _ftpPassword = selectedRow.Cells("FTPPassword").Value.ToString
                End If
                If selectedRow.Cells("ChangeDirectory").Value IsNot Nothing Then
                    _changeDirectory = selectedRow.Cells("ChangeDirectory").Value.ToString
                End If
                If selectedRow.Cells("Port").Value IsNot Nothing Then
                    _port = selectedRow.Cells("Port").Value.ToString
                End If
                If selectedRow.Cells("IsSecureTransfer").Value IsNot Nothing Then
                    _isSecureTransfer = CType(selectedRow.Cells("IsSecureTransfer").Value, Boolean)
                End If
            End If
        End Sub

#End Region

#Region "Business Rules"

        Public Function ValidateFTPData() As ArrayList
            Dim statusList As New ArrayList

            'file writer type required
            If _fileWriterType Is Nothing Or (_fileWriterType IsNot Nothing AndAlso _fileWriterType.Trim.Equals("")) Then
                statusList.Add(StoreFTPConfigStatus.Error_Required_FileWriterType)
            End If

            'ip address required
            If _ipAddress Is Nothing Or (_ipAddress IsNot Nothing AndAlso _ipAddress.Trim.Equals("")) Then
                statusList.Add(StoreFTPConfigStatus.Error_Required_IPAddress)
            End If

            'username required
            If _ftpUser Is Nothing Or (_ftpUser IsNot Nothing AndAlso _ftpUser.Trim.Equals("")) Then
                statusList.Add(StoreFTPConfigStatus.Error_Required_FTPUser)
            End If

            'password required
            If _ftpPassword Is Nothing Or (_ftpPassword IsNot Nothing AndAlso _ftpPassword.Trim.Equals("")) Then
                statusList.Add(StoreFTPConfigStatus.Error_Required_FTPPassword)
            End If

            'port must be a numeric value
            If _port IsNot Nothing AndAlso Not _port.Trim.Equals("") AndAlso Not IsNumeric(_port) Then
                statusList.Add(StoreFTPConfigStatus.Error_Port_Numeric)
            End If

            If statusList.Count = 0 Then
                statusList.Add(StoreFTPConfigStatus.Valid)
            End If

            Return statusList
        End Function

#End Region

    End Class

End Namespace