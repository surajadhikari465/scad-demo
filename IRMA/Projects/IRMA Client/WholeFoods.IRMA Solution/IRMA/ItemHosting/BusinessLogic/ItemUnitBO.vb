Imports log4net
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Class ItemUnitBO
        ' TFS 13058, v4.0, 8/3/2010, Tom Lux: Merged Administration\ConfigurationData\DataAccess\ItemUnitBO.vb into this class (to remove dup class).

#Region "Private Members"

        ''' <summary>
        ''' Log4Net logger for this class.
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#End Region

#Region "Public Get/Set Properties"

        Private _Unit_Id As Integer
        Public Property UnitId() As Integer
            Get
                Return _Unit_Id
            End Get
            Set(ByVal value As Integer)
                _Unit_Id = value
                _DataChanged = True
            End Set
        End Property

        Private _Unit_Name As String
        Public Property UnitName() As String
            Get
                Return _Unit_Name
            End Get
            Set(ByVal value As String)
                _Unit_Name = value
                _DataChanged = True
            End Set
        End Property

        Private _Weight_Unit As Boolean
        Public Property WeightUnit() As Boolean
            Get
                Return _Weight_Unit
            End Get
            Set(ByVal value As Boolean)
                _Weight_Unit = value
                _DataChanged = True
            End Set
        End Property

        Private _User_Id As Integer
        Public Property UserId() As Integer
            Get
                Return _User_Id
            End Get
            Set(ByVal value As Integer)
                _User_Id = value
                _DataChanged = True
            End Set
        End Property

        Private _Unit_Abbreviation As String
        Public Property UnitAbbreviation() As String
            Get
                Return _Unit_Abbreviation
            End Get
            Set(ByVal value As String)
                _Unit_Abbreviation = value
                _DataChanged = True
            End Set
        End Property

        Private _UnitSysCode As String
        Public Property UnitSysCode() As String
            Get
                Return _UnitSysCode
            End Get
            Set(ByVal value As String)
                _UnitSysCode = value
                _DataChanged = True
            End Set
        End Property

        Private _IsPackageUnit As Boolean
        Public Property IsPackageUnit() As Boolean
            Get
                Return _IsPackageUnit
            End Get
            Set(ByVal value As Boolean)
                _IsPackageUnit = value
                _DataChanged = True
            End Set
        End Property

        Private _PlumUnitAbbr As String
        Public Property PlumUnitAbbr() As String
            Get
                Return _PlumUnitAbbr
            End Get
            Set(ByVal value As String)
                _PlumUnitAbbr = value
                _DataChanged = True
            End Set
        End Property

        Private _EDISysCode As String
        Public Property EDISysCode() As String
            Get
                Return _EDISysCode
            End Get
            Set(ByVal value As String)
                _EDISysCode = value
                _DataChanged = True
            End Set
        End Property

        Private _ErrorMessage As String
        Public Property ErrorMessage() As String
            Get
                Return _ErrorMessage
            End Get
            Set(ByVal value As String)
                _ErrorMessage = value
            End Set
        End Property

        Private _valid As Boolean
        Public Property Valid() As Boolean
            Get
                Return _valid
            End Get
            Set(ByVal value As Boolean)
                _valid = value
            End Set
        End Property

#End Region

#Region "Public Read-Only Properties"

        Private _DataChanged As Boolean
        Public ReadOnly Property DataChanged() As Boolean
            Get
                Return _DataChanged
            End Get
        End Property

#End Region

#Region "Constructors"

        Sub New()
            Me._DataChanged = True
            Me.Valid = True
        End Sub

        Sub New(ByVal UnitId As Integer, ByVal UnitName As String, ByVal WeightUnit As Boolean, ByVal UnitAbbreviation As String, ByVal UnitSysCode As String, ByVal IsPackageUnit As Boolean, ByVal userid As Integer, ByVal PlumUnitAbbr As String, ByVal EDISysCode As String)
            Me._Unit_Id = UnitId
            Me._Unit_Name = UnitName
            Me._Weight_Unit = WeightUnit
            Me._Unit_Abbreviation = UnitAbbreviation
            Me._UnitSysCode = UnitSysCode
            Me._IsPackageUnit = IsPackageUnit
            Me._User_Id = userid
            Me._PlumUnitAbbr = PlumUnitAbbr
            Me._EDISysCode = EDISysCode
            Me.Valid = True
        End Sub

#End Region

#Region "Public Methods"

        Public Sub Save()
            Dim dao As ItemUnitDAO = New ItemUnitDAO()
            dao.SaveItemUnit(Me)
            ' TFS 13036, v4.0, 7/21/2010, Tom Lux: Adding call when ItemUnitDAO is saved so that global UOM IDs are updated and the client doesn't need to be restarted (because that's when the globals are setup).
            ItemUnitDAO.LoadItemUnits()
        End Sub

#End Region

    End Class

End Namespace