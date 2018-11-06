Imports log4net
Imports Microsoft.VisualBasic
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient

Public Class BatchReceiveCloseBO

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Property Definitions"
    Private _vendor_ID As Integer
    Private _subteam_No As Integer
    Private _startdate As Date
    Private _enddate As Date
    Private _orderheader_ID As Integer
#End Region

#Region "constructors and helper methods to initialize the data"
    Public Sub New()
    End Sub

#End Region

#Region "Property access methods"

    Public Property BRC_Vendor_ID() As Integer
        Get
            Return _vendor_ID
        End Get
        Set(ByVal value As Integer)
            _vendor_ID = value
        End Set
    End Property
    Public Property BRC_Subteam_No() As Integer
        Get
            Return _subteam_No
        End Get
        Set(ByVal value As Integer)
            _subteam_No = value
        End Set
    End Property

    Public Property BRC_StartDate() As Date
        Get
            Return _startdate
        End Get
        Set(ByVal value As Date)
            _startdate = value
        End Set
    End Property

    Public Property BRC_EndDate() As Date
        Get
            Return _enddate
        End Get
        Set(ByVal value As Date)
            _enddate = value
        End Set
    End Property
    Public Property BRC_OrderHeader_ID() As Integer
        Get
            Return _orderheader_ID
        End Get
        Set(ByVal value As Integer)
            _orderheader_ID = value
        End Set
    End Property
#End Region

End Class
