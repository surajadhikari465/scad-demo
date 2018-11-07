Imports log4net

Namespace WholeFoods.IRMA.Administration.BatchMgmt.DataAccess
    Public Class BatchSearchBO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Private _storeList As String
        Private _batchStatusList As String
        Private _listSeparator As Char = "|"c
        Private _batchDesc As String

#Region "Property Accessor Methods"
        Public Property StoreList() As String
            Get
                Return _storeList
            End Get
            Set(ByVal value As String)
                _storeList = value
            End Set
        End Property

        Public Property BatchStatusList() As String
            Get
                Return _batchStatusList
            End Get
            Set(ByVal value As String)
                _batchStatusList = value
            End Set
        End Property

        Public ReadOnly Property ListSeparator() As Char
            Get
                Return _listSeparator
            End Get
        End Property

        Public Property BatchDesc() As String
            Get
                Return _batchDesc
            End Get
            Set(ByVal value As String)
                _batchDesc = value
            End Set
        End Property

#End Region

    End Class
End Namespace
