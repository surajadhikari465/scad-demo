Imports System.Text
Imports WholeFoods.IRMA.Common.DataAccess

Public Class AvgCostAdjBO

#Region " Structs"

    ''' <summary>
    ''' The Average Cost Adjustment Item Structure.
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure AvgCostAdjItem

#Region " Fields"

        Private _bu As Integer
        Private _state As String
        Private _zone As Integer
        Private _allWFMStores As Boolean
        Private _allStores As Boolean
        Private _allHFMStores As Boolean
        Private _subTeam As Integer
        Private _itemKey As Integer
        Private _endDate As DateTime
        Private _startDate As DateTime
        Private _current As Boolean
        Private _avgCost As Decimal
        Private _reason As Integer
        Private _comment As String

#End Region

#Region " Properties"

        Public Property BusinessUnit() As Integer
            Get
                Return Me._bu
            End Get
            Set(ByVal value As Integer)
                Me._bu = value
            End Set
        End Property

        Public Property State() As String
            Get
                Return Me._state
            End Get
            Set(ByVal value As String)
                Me._state = value
            End Set
        End Property

        Public Property Zone() As Integer
            Get
                Return Me._zone
            End Get
            Set(ByVal value As Integer)
                Me._zone = value
            End Set
        End Property

        Public Property AllWFMStores() As Boolean
            Get
                Return Me._allWFMStores
            End Get
            Set(ByVal value As Boolean)
                Me._allWFMStores = value
            End Set
        End Property

        Public Property AllHFMStores() As Boolean
            Get
                Return Me._allHFMStores
            End Get
            Set(ByVal value As Boolean)
                Me._allHFMStores = value
            End Set
        End Property

        Public Property AllStores() As Boolean
            Get
                Return Me._allStores
            End Get
            Set(ByVal value As Boolean)
                Me._allStores = value
            End Set
        End Property

        Public Property SubTeam() As Integer
            Get
                Return Me._subTeam
            End Get
            Set(ByVal value As Integer)
                Me._subTeam = value
            End Set
        End Property

        Public Property ItemKey() As Integer
            Get
                Return Me._itemKey
            End Get
            Set(ByVal value As Integer)
                Me._itemKey = value
            End Set
        End Property

        Public Property StartDate() As DateTime
            Get
                Return Me._startDate
            End Get
            Set(ByVal value As DateTime)
                Me._startDate = value
            End Set
        End Property

        Public Property EndDate() As DateTime
            Get
                Return Me._endDate
            End Get
            Set(ByVal value As DateTime)
                Me._endDate = value
            End Set
        End Property

        Public Property Current() As Boolean
            Get
                Return Me._current
            End Get
            Set(ByVal value As Boolean)
                Me._current = value
            End Set
        End Property

        Public Property AvgCost() As Decimal
            Get
                Return Me._avgCost
            End Get
            Set(ByVal value As Decimal)
                Me._avgCost = value
            End Set
        End Property

        Public Property Reason() As Integer
            Get
                Return Me._reason
            End Get
            Set(ByVal value As Integer)
                Me._reason = value
            End Set
        End Property

        Public Property Comment() As String
            Get
                Return Me._comment
            End Get
            Set(ByVal value As String)
                Me._comment = value
            End Set
        End Property

#End Region

#Region " Constructors"

        ''' <summary>
        ''' Create a new AvgCostAdjItem instance.
        ''' </summary>
        ''' <param name="BusinessUnit">The business unit the item is associated with.</param>
        ''' <param name="SubTeam">The subteam the item is associated with.</param>
        ''' <param name="ItemKey">The item key for the item.</param>
        ''' <param name="AvgCost">The new average cost to apply.</param>
        ''' <param name="Reason">The reason for the average cost adjustment</param>
        ''' <param name="Comment">User Comments regarding the average cost adjustment.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal ItemKey As Integer _
                        , ByVal BusinessUnit As Integer _
                        , ByVal AllWFMStores As Boolean _
                        , ByVal AllHFMStores As Boolean _
                        , ByVal AllStores As Boolean _
                        , ByVal Zone As Integer _
                        , ByVal State As String _
                        , ByVal SubTeam As Integer _
                        , ByVal Current As Boolean _
                        , ByVal StartDate As DateTime _
                        , ByVal EndDate As DateTime _
                        , Optional ByVal AvgCost As Decimal = Nothing _
                        , Optional ByVal Reason As Integer = Nothing _
                        , Optional ByVal Comment As String = Nothing)

            Me._itemKey = ItemKey
            Me._bu = BusinessUnit
            Me._allWFMStores = AllWFMStores
            Me._allHFMStores = AllHFMStores
            Me._allStores = AllStores
            Me._zone = Zone
            Me._state = State
            Me._subTeam = SubTeam
            Me._current = Current
            Me._startDate = StartDate
            Me._endDate = EndDate
            Me._reason = Reason
            Me._comment = Comment
            Me._avgCost = AvgCost

        End Sub

#End Region

    End Structure

#End Region

#Region " Read Methods"

    ''' <summary>
    ''' Returns a data table containing the complete average cost history for the specified item.
    ''' </summary>
    ''' <param name="AvgCostItem"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetHistory(ByVal AvgCostItem As AvgCostAdjItem) As DataTable

        Return AvgCostAdjDO.GetAdjustmentHistory(AvgCostItem)

    End Function

    Public Shared Function GetAvgCostForStores(ByVal AvgCostItem As AvgCostAdjItem) As DataTable

        Return AvgCostAdjDO.GetAvgCostForStores(AvgCostItem)

    End Function

    Public Shared Function GetAdjustmentReasons(ByVal ApplyFilter As Boolean, ByVal Active As Boolean) As DataTable

        Return AvgCostAdjDO.GetAdjustmentReasons(ApplyFilter, Active)

    End Function

    Public Shared Function GetStoreList() As DataTable

        Return StoreListDAO.GetStoresAndDistAdjustments()

    End Function

    Public Shared Function IsAdjustmentReasonActive(ByVal ID As Integer) As Boolean

        Return AvgCostAdjDO.IsAdjustmentReasonActive(ID)

    End Function

#End Region

#Region " Write Methods"

    ''' <summary>
    ''' Saves a valid average cost adjustment to the database.
    ''' </summary>
    ''' <param name="AvgCostItem"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Save(ByVal AvgCostItem As AvgCostAdjItem) As Boolean

        Dim errString As String = ValidateAdjustment(AvgCostItem)

        If errString.Length > 0 Then

            MessageBox.Show("The adjustment data is invalid." & vbCrLf & vbCrLf & errString, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Return False

        Else

            Return AvgCostAdjDO.SaveAdjustment(AvgCostItem)

        End If

    End Function

    Public Shared Function AddAdjustmentReason(ByVal Description As String, ByVal Active As Boolean) As Boolean

        If Description.Length = 0 Then

            MessageBox.Show("The adjustment reason Description is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Return False

        Else

            Return AvgCostAdjDO.AddAdjustmentReason(Description, Active)

        End If

    End Function

    Public Shared Function SetReasonStatus(ByVal ID As Integer, ByVal Active As Boolean) As Boolean

        Return AvgCostAdjDO.SetReasonStatus(ID, Active)

    End Function

#End Region

#Region " Helpers"

    ''' <summary>
    ''' Ensures that all required values to make an average cost adjustment are valid.
    ''' </summary>
    ''' <param name="AvgCostItem"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function ValidateAdjustment(ByVal AvgCostItem As AvgCostAdjItem) As String

        Dim valid As Boolean = True

        Dim retVal As New StringBuilder

        If AvgCostItem.BusinessUnit = Nothing Then
            retVal.AppendLine("Business Unit not provided.")
            valid = False
        End If

        If AvgCostItem.SubTeam = Nothing Then
            retVal.AppendLine("Sub Team not provided.")
            valid = False
        End If

        If AvgCostItem.ItemKey = Nothing Then
            retVal.AppendLine("Item Key not provided.")
            valid = False
        End If

        If Not AvgCostItem.AvgCost > 0 Then
            retVal.AppendLine("Average Cost is not a positive value greater than zero (0).")
            valid = False
        End If

        If AvgCostItem.Reason = Nothing Then
            retVal.AppendLine("Reason not provided.")
            valid = False
        End If

        Return retVal.ToString()

    End Function

#End Region

End Class
