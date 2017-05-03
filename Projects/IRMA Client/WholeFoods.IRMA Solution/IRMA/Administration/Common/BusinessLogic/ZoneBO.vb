Imports log4net
Imports System.Data.SqlClient
Imports System.Text

Namespace WholeFoods.IRMA.Administration.Common.BusinessLogic
    Public Class ZoneBO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Private _zoneId As Integer
        Private _zoneName As String
        Private _storeNoList As New ArrayList

#Region "constructors and helper methods to initialize the data"
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Create a new instance of the object, populating it with data from a SQL query.
        ''' </summary>
        ''' <param name="results"></param>
        ''' <remarks></remarks>
        Public Sub New(ByRef results As SqlDataReader)
            logger.Debug("New entry with SQL results")
            If (Not results.IsDBNull(results.GetOrdinal("Zone_Id"))) Then
                _zoneId = results.GetInt32(results.GetOrdinal("Zone_Id"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("Zone_Name"))) Then
                _zoneName = results.GetString(results.GetOrdinal("Zone_Name"))
            End If
            logger.Debug("New exit")
        End Sub
#End Region

        ''' <summary>
        ''' Build a list of the stores assigned to this zone, formatted for input into
        ''' a stored procedure.
        ''' </summary>
        ''' <param name="listSeparator"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function BuildStoreList(ByVal listSeparator As Char) As StringBuilder
            logger.Debug("BuildStoreList entry: _zoneName=" + _zoneName)
            ' Add the store number for each store assigned to the zone to the string list.
            Dim currentStore As StoreBO
            Dim storeList As New StringBuilder
            Dim storeEnum As IEnumerator = _storeNoList.GetEnumerator()
            While (storeEnum.MoveNext())
                currentStore = CType(storeEnum.Current, StoreBO)
                storeList.Append(listSeparator)
                storeList.Append(currentStore.StoreNo.ToString)
            End While
            logger.Debug("BuildStoreList exit")
            Return storeList
        End Function

#Region "Property access methods"
        Public Property ZoneId() As Integer
            Get
                Return _zoneId
            End Get
            Set(ByVal value As Integer)
                _zoneId = value
            End Set
        End Property

        Public Property ZoneName() As String
            Get
                Return _zoneName
            End Get
            Set(ByVal value As String)
                _zoneName = value
            End Set
        End Property

        Public Property StoreNoList() As ArrayList
            Get
                Return _storeNoList
            End Get
            Set(ByVal value As ArrayList)
                _storeNoList = value
            End Set
        End Property
#End Region

    End Class
End Namespace
