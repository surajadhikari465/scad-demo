Imports WholeFoods.Utility
Imports WholeFoods.IRMA.Administration.Common.DataAccess

Namespace WholeFoods.IRMA.Administration.Common.BusinessLogic
    Public Enum ArchiveConfigStatus
        Valid
        Error_Required_TableName
        Error_Required_FilterSQL
    End Enum

    Public Class DataArchiveBO

#Region "Property Definitions"

        Private _dataArchiveID As Integer
        Private _tableName As String
        Private _storedProcName As String
        Private _changeTypeID As Integer
        Private _jobFrequencyID As Integer
        Private _retentionDays As Integer
        Private _isEnabled As Boolean
        Private _processStatus As Integer
        Private _processDate As Date
        Private _lastUpdate As Date
        Private _lastUpdateUserID As Integer
        Private _changeTypeName As String
        Private _jobFrequencyName As String
        Private _fullName As String

#End Region

#Region "constructors and helper methods to initialize the data"
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Create a new instance of the object from the database using the DataArchiveID.
        ''' </summary>
        ''' <param name="DataArchiveID"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal DataArchiveID As Integer)

            Dim dt As DataTable = Nothing

            Try

                dt = New DataTable
                dt = DataArchiveDAO.GetArchiveTable(DataArchiveID)

                If dt.Rows.Count > 0 Then
                    _dataArchiveID = dt.Rows(0).Item("DataArchiveID")

                    If dt.Rows(0).Item("TableName") IsNot DBNull.Value Then
                        _tableName = dt.Rows(0).Item("TableName")
                    End If
                    If dt.Rows(0).Item("StoredProcName") IsNot DBNull.Value Then
                        _storedProcName = dt.Rows(0).Item("StoredProcName")
                    End If
                    If dt.Rows(0).Item("ChangeTypeID") IsNot DBNull.Value Then
                        _changeTypeID = dt.Rows(0).Item("ChangeTypeID")
                    End If
                    If dt.Rows(0).Item("JobFrequencyID") IsNot DBNull.Value Then
                        _jobFrequencyID = dt.Rows(0).Item("JobFrequencyID")
                    End If
                    If dt.Rows(0).Item("RetentionDays") IsNot DBNull.Value Then
                        _retentionDays = dt.Rows(0).Item("RetentionDays")
                    End If
                    If dt.Rows(0).Item("IsEnabled") IsNot DBNull.Value Then
                        _isEnabled = dt.Rows(0).Item("IsEnabled")
                    End If
                    If dt.Rows(0).Item("ProcessStatus") IsNot DBNull.Value Then
                        _processStatus = dt.Rows(0).Item("ProcessStatus")
                    End If
                    If dt.Rows(0).Item("ProcessDate") IsNot DBNull.Value Then
                        _lastUpdate = dt.Rows(0).Item("ProcessDate")
                    End If
                    If dt.Rows(0).Item("LastUpdate") IsNot DBNull.Value Then
                        _lastUpdate = dt.Rows(0).Item("LastUpdate")
                    End If
                    If dt.Rows(0).Item("LastUpdateUserID") IsNot DBNull.Value Then
                        _lastUpdateUserID = dt.Rows(0).Item("LastUpdateUserID")
                    End If
                    If dt.Rows(0).Item("ChangeTypeName") IsNot DBNull.Value Then
                        _changeTypeName = dt.Rows(0).Item("ChangeTypeName")
                    End If
                    If dt.Rows(0).Item("JobFrequencyName") IsNot DBNull.Value Then
                        _jobFrequencyName = dt.Rows(0).Item("JobFrequencyName")
                    End If
                    If dt.Rows(0).Item("FullName") IsNot DBNull.Value Then
                        _fullName = dt.Rows(0).Item("FullName")
                    End If

                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try

        End Sub

        ''' <summary>
        ''' Create a new instance of the object, populated from the DataGridViewRow
        ''' for a selected row on the UI.
        ''' </summary>
        ''' <param name="selectedRow"></param>
        ''' <remarks></remarks>
        Public Sub New(ByRef selectedRow As DataGridViewRow)

            If selectedRow.Cells("DataArchiveID").Value IsNot DBNull.Value Then
                _dataArchiveID = CType(selectedRow.Cells("DataArchiveID").Value, Integer)
            End If
            If selectedRow.Cells("TableName").Value IsNot DBNull.Value Then
                _tableName = CType(selectedRow.Cells("TableName").Value, String)
            End If
            If selectedRow.Cells("StoredProcName").Value IsNot DBNull.Value Then
                _storedProcName = CType(selectedRow.Cells("StoreProcName").Value, String)
            End If
            If selectedRow.Cells("ChangeTypeID").Value IsNot DBNull.Value Then
                _changeTypeID = CType(selectedRow.Cells("ChangeTypeID").Value, Integer)
            End If
            If selectedRow.Cells("JobFrequencyID").Value IsNot DBNull.Value Then
                _jobFrequencyID = CType(selectedRow.Cells("JobFrequencyID").Value, Integer)
            End If
            If selectedRow.Cells("RetentionDays").Value IsNot DBNull.Value Then
                _retentionDays = CType(selectedRow.Cells("RetentionDays").Value, Integer)
            End If
            If selectedRow.Cells("IsEnabled").Value IsNot DBNull.Value Then
                _isEnabled = CType(selectedRow.Cells("IsEnabled").Value, Boolean)
            End If
            If selectedRow.Cells("ProcessStatus").Value IsNot DBNull.Value Then
                _processStatus = CType(selectedRow.Cells("ProcessStatus").Value, Integer)
            End If
            If selectedRow.Cells("ProcessDate").Value IsNot DBNull.Value Then
                _processDate = CType(selectedRow.Cells("ProcessDate").Value, Date)
            End If
            If selectedRow.Cells("LastUpdate").Value IsNot DBNull.Value Then
                _lastUpdate = CType(selectedRow.Cells("LastUpdate").Value, Date)
            End If
            If selectedRow.Cells("LastUpdateUserID").Value IsNot DBNull.Value Then
                _lastUpdateUserID = CType(selectedRow.Cells("LastUpdateUserID").Value, Integer)
            End If
            If selectedRow.Cells("ChangeTypeName").Value IsNot DBNull.Value Then
                _changeTypeName = CType(selectedRow.Cells("ChangeTypeName").Value, String)
            End If
            If selectedRow.Cells("JobFrequencyName").Value IsNot DBNull.Value Then
                _jobFrequencyName = CType(selectedRow.Cells("JobFrequencyName").Value, String)
            End If
            If selectedRow.Cells("FullName").Value IsNot DBNull.Value Then
                _fullName = CType(selectedRow.Cells("FullName").Value, String)
            End If

        End Sub
#End Region

#Region "Property access methods"

        Public Property DataArchiveID() As Integer
            Get
                Return _dataArchiveID
            End Get
            Set(ByVal value As Integer)
                _dataArchiveID = value
            End Set
        End Property

        Public Property TableName() As String
            Get
                Return _tableName
            End Get
            Set(ByVal value As String)
                _tableName = value
            End Set
        End Property

        Public Property StoredProcName() As String
            Get
                Return _storedProcName
            End Get
            Set(ByVal value As String)
                _storedProcName = value
            End Set
        End Property

        Public Property ChangeTypeID() As Integer
            Get
                Return _changeTypeID
            End Get
            Set(ByVal value As Integer)
                _changeTypeID = value
            End Set
        End Property

        Public Property JobFrequencyID() As Integer
            Get
                Return _jobFrequencyID
            End Get
            Set(ByVal value As Integer)
                _jobFrequencyID = value
            End Set
        End Property

        Public Property RetentionDays() As Integer
            Get
                Return _retentionDays
            End Get
            Set(ByVal value As Integer)
                _retentionDays = value
            End Set
        End Property

        Public Property IsEnabled() As Boolean
            Get
                Return _isEnabled
            End Get
            Set(ByVal value As Boolean)
                _isEnabled = value
            End Set
        End Property

        Public Property ProcessStatus() As Integer
            Get
                Return _processStatus
            End Get
            Set(ByVal value As Integer)
                _processStatus = value
            End Set
        End Property

        Public Property ProcessDate() As Date
            Get
                Return _processDate
            End Get
            Set(ByVal value As Date)
                _processDate = value
            End Set
        End Property

        Public Property LastUpdate() As Date
            Get
                Return _lastUpdate
            End Get
            Set(ByVal value As Date)
                _lastUpdate = value
            End Set
        End Property

        Public Property LastUpdateUserID() As Integer
            Get
                Return _lastUpdateUserID
            End Get
            Set(ByVal value As Integer)
                _lastUpdateUserID = value
            End Set
        End Property

        Public Property ChangeTypeName() As String
            Get
                Return _changeTypeName
            End Get
            Set(ByVal value As String)
                _changeTypeName = value
            End Set
        End Property

        Public Property JobFrequencyName() As String
            Get
                Return _jobFrequencyName
            End Get
            Set(ByVal value As String)
                _jobFrequencyName = value
            End Set
        End Property

        Public Property FullName() As String
            Get
                Return _fullName
            End Get
            Set(ByVal value As String)
                _fullName = value
            End Set
        End Property

#End Region

#Region "Business rules"
        ''' <summary>
        ''' validates data elements of current instance of DataArchiveBO object
        ''' </summary>
        ''' <returns>ArrayList of ArchiveConfigStatus values</returns>
        ''' <remarks></remarks>
        Public Function ValidateArchiveConfigData() As ArrayList

            Dim errorList As New ArrayList

            ' required fields
            If _tableName.Equals("") Then
                errorList.Add(ArchiveConfigStatus.Error_Required_TableName)
            End If

            'no errors - return Valid status
            If errorList.Count = 0 Then
                errorList.Add(ArchiveConfigStatus.Valid)
            End If

            Return errorList
        End Function
#End Region

    End Class
End Namespace
