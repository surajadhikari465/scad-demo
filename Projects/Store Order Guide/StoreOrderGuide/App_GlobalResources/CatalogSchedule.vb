Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Public Class CatalogSchedule

#Region "Private Properties"
    Private _CatalogScheduleID As Integer
    Private _ManagedByID As Integer
    Private _StoreNo As Integer
    Private _SubTeamNo As Integer
    Private _Mon As Boolean
    Private _Tue As Boolean
    Private _Wed As Boolean
    Private _Thu As Boolean
    Private _Fri As Boolean
    Private _Sat As Boolean
    Private _Sun As Boolean
    Private _InsertDate As Date
    Private _UpdateDate As Date
    Private _InsertUser As String
    Private _UpdateUser As String
#End Region

#Region "Public Properties"
    Public Property CatalogScheduleID() As Integer
        Get
            Return _CatalogScheduleID
        End Get
        Friend Set(ByVal value As Integer)
            _CatalogScheduleID = value
        End Set
    End Property

    Public Property ManagedByID() As Integer
        Get
            Return _ManagedByID
        End Get
        Friend Set(ByVal value As Integer)
            _ManagedByID = value
        End Set
    End Property

    Public Property StoreNo() As Integer
        Get
            Return _StoreNo
        End Get
        Friend Set(ByVal value As Integer)
            _StoreNo = value
        End Set
    End Property

    Public Property SubTeamNo() As Integer
        Get
            Return _SubTeamNo
        End Get
        Friend Set(ByVal value As Integer)
            _SubTeamNo = value
        End Set
    End Property

    Public Property Mon() As Boolean
        Get
            Return _Mon
        End Get
        Set(ByVal value As Boolean)
            _Mon = value
        End Set
    End Property

    Public Property Tue() As Boolean
        Get
            Return _Tue
        End Get
        Set(ByVal value As Boolean)
            _Tue = value
        End Set
    End Property

    Public Property Wed() As Boolean
        Get
            Return _Wed
        End Get
        Set(ByVal value As Boolean)
            _Wed = value
        End Set
    End Property

    Public Property Thu() As Boolean
        Get
            Return _Thu
        End Get
        Set(ByVal value As Boolean)
            _Thu = value
        End Set
    End Property

    Public Property Fri() As Boolean
        Get
            Return _Fri
        End Get
        Set(ByVal value As Boolean)
            _Fri = value
        End Set
    End Property

    Public Property Sat() As Boolean
        Get
            Return _Sat
        End Get
        Set(ByVal value As Boolean)
            _Sat = value
        End Set
    End Property

    Public Property Sun() As Boolean
        Get
            Return _Sun
        End Get
        Set(ByVal value As Boolean)
            _Sun = value
        End Set
    End Property

    Public Property InsertDate() As Date
        Get
            Return _InsertDate
        End Get
        Set(ByVal value As Date)
            _InsertDate = value
        End Set
    End Property

    Public Property UpdateDate() As Date
        Get
            Return _UpdateDate
        End Get
        Set(ByVal value As Date)
            _UpdateDate = value
        End Set
    End Property

    Public Property InsertUser() As String
        Get
            Return _InsertUser
        End Get
        Set(ByVal value As String)
            _InsertUser = Trim(value)
        End Set
    End Property

    Public Property UpdateUser() As String
        Get
            Return _UpdateUser
        End Get
        Set(ByVal value As String)
            _UpdateUser = Trim(value)
        End Set
    End Property
#End Region

#Region "Public Methods"
    Public Function GetCatalogSchedules(ByVal CatalogScheduleID As Integer, ByVal ManagedByID As Integer, ByVal StoreNo As Integer, ByVal SubTeamNo As Integer) As Data.DataSet
        Dim Dal As New Dal

        Return Dal.GetCatalogSchedules(Me, CatalogScheduleID, ManagedByID, StoreNo, SubTeamNo)
    End Function

    Public Function AddCatalogSchedule(ByVal CatalogScheduleID As Integer, ByVal ManagedByID As Integer, ByVal StoreNo As Integer, ByVal SubTeamNo As Integer, ByVal Mon As Boolean, ByVal Tue As Boolean, ByVal Wed As Boolean, ByVal Thu As Boolean, ByVal Fri As Boolean, ByVal Sat As Boolean, ByVal Sun As Boolean) As Boolean
        Dim Dal As New Dal

        Me.CatalogScheduleID = CatalogScheduleID
        Me.ManagedByID = ManagedByID
        Me.StoreNo = StoreNo
        Me.SubTeamNo = SubTeamNo
        Me.Mon = Mon
        Me.Tue = Tue
        Me.Wed = Wed
        Me.Thu = Thu
        Me.Fri = Fri
        Me.Sat = Sat
        Me.Sun = Sun
        Me.InsertUser = InsertUser
        Me.UpdateUser = UpdateUser

        Return Dal.AddCatalogSchedule(Me)
    End Function

    Public Function SetCatalogSchedule(ByVal CatalogScheduleID As Integer, ByVal ManagedByID As Integer, ByVal StoreNo As Integer, ByVal SubTeamNo As Integer, ByVal Mon As Boolean, ByVal Tue As Boolean, ByVal Wed As Boolean, ByVal Thu As Boolean, ByVal Fri As Boolean, ByVal Sat As Boolean, ByVal Sun As Boolean) As Boolean
        Dim Dal As New Dal

        Me.CatalogScheduleID = CatalogScheduleID
        Me.ManagedByID = ManagedByID
        Me.StoreNo = StoreNo
        Me.SubTeamNo = SubTeamNo
        Me.Mon = Mon
        Me.Tue = Tue
        Me.Wed = Wed
        Me.Thu = Thu
        Me.Fri = Fri
        Me.Sat = Sat
        Me.Sun = Sun
        Me.InsertUser = InsertUser
        Me.UpdateUser = UpdateUser

        Return Dal.SetCatalogSchedule(Me, CatalogScheduleID)
    End Function

    Public Function DelCatalogSchedule(ByVal CatalogScheduleID As Integer) As Boolean
        Dim Dal As New Dal

        Return Dal.DelCatalogSchedule(CatalogScheduleID)
    End Function
#End Region

End Class
