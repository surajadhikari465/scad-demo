Imports log4net

Public Class GLBO

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private _sDate As String
    Private _eDate As String
    Private _cDate As String
    Private _storeNo As Integer
    Private _type As Integer

    Public Property StartDate() As String
        Get
            Return _sDate
        End Get
        Set(ByVal value As String)
            _sDate = value
        End Set
    End Property

    Public Property EndDate() As String
        Get
            Return _eDate
        End Get
        Set(ByVal value As String)
            _eDate = value
        End Set
    End Property

    Public Property CurrentDate() As String
        Get
            Return _cDate
        End Get
        Set(ByVal value As String)
            _cDate = value
        End Set
    End Property

    Public Property StoreNo() As Integer
        Get
            Return _storeNo
        End Get
        Set(ByVal value As Integer)
            _storeNo = value
        End Set
    End Property

    Public Property TransactionType() As Integer
        Get
            Return _type
        End Get
        Set(ByVal value As Integer)
            _type = value
        End Set
    End Property

End Class
