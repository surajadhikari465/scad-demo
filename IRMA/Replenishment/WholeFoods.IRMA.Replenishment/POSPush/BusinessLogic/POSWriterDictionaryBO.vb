Imports WholeFoods.Utility
Imports WholeFoods.IRMA.Replenishment.POSPush.Controller
Imports System.Data.SqlClient

Namespace WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
    Public Class POSWriterDictionaryBO
#Region "Property Definitions"
        Private _posFileWriterKey As Integer
        Private _fieldID As String
        Private _dataType As String
#End Region

#Region "Methods to populate the object with stored procedure result sets"
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="results"></param>
        ''' <remarks></remarks>
        Public Sub PopulateFromPOSWriterDictionary(ByRef results As SqlDataReader)
            ' Assign values to the properties
            If (Not results.IsDBNull(results.GetOrdinal("POSFileWriterKey"))) Then
                _posFileWriterKey = results.GetInt32(results.GetOrdinal("POSFileWriterKey"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("FieldID"))) Then
                _fieldID = results.GetString(results.GetOrdinal("FieldID"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("DataType"))) Then
                _dataType = results.GetString(results.GetOrdinal("DataType"))
            End If
        End Sub
#End Region

#Region "Property access methods"
        Public Property POSFileWriterKey() As Integer
            Get
                Return _posFileWriterKey
            End Get
            Set(ByVal value As Integer)
                _posFileWriterKey = value
            End Set
        End Property

        Public Property FieldID() As String
            Get
                Return _fieldID
            End Get
            Set(ByVal value As String)
                _fieldID = value
            End Set
        End Property

        Public Property DataType() As String
            Get
                Return _dataType
            End Get
            Set(ByVal value As String)
                _dataType = value
            End Set
        End Property
#End Region

    End Class
End Namespace

