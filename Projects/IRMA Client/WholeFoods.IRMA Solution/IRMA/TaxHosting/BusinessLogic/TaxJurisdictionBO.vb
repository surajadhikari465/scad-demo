Imports Infragistics.Win.UltraWinGrid

Public Enum TaxJurisdictionStatus
    Valid
    Error_StoresAssociated
    Warning_TaxFlagsAssociated
End Enum

Namespace WholeFoods.IRMA.TaxHosting.BusinessLogic
    Public Class TaxJurisdictionBO

#Region "Property Definitions"

        Private _taxJurisdictionId As Integer
        Private _taxJurisdictionDesc As String
        Private _storeCount As Integer
        Private _taxFlagCount As Integer
        Private _regionalJurisdictionID As String

#End Region

#Region "Constructors"

        ''' <summary>
        ''' empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Create a new instance of the object from a current UltraGridRow
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal currentRow As UltraGridRow)
            If currentRow.Cells("TaxJurisdictionID").Value IsNot DBNull.Value Then
                Me.TaxJurisdictionId = CType(currentRow.Cells("TaxJurisdictionID").Value, Integer)
            End If

            Me.TaxJurisdictionDesc = currentRow.Cells("TaxJurisdictionDesc").Value.ToString

            If currentRow.Cells("StoreCount").Value IsNot DBNull.Value Then
                Me.StoreCount = CType(currentRow.Cells("StoreCount").Value, Integer)
            End If

            If currentRow.Cells("TaxFlagCount").Value IsNot DBNull.Value Then
                Me.TaxFlagCount = CType(currentRow.Cells("TaxFlagCount").Value, Integer)
            End If

            If currentRow.Cells("RegionalJurisdictionID").Value IsNot Nothing Then
                Me.RegionalJurisdictionID = currentRow.Cells("RegionalJurisdictionID").Value.ToString
            End If
        End Sub

#End Region

#Region "Property Access Methods"

        Public Property TaxJurisdictionId() As Integer
            Get
                Return _taxJurisdictionId
            End Get
            Set(ByVal value As Integer)
                _taxJurisdictionId = value
            End Set
        End Property

        Public Property TaxJurisdictionDesc() As String
            Get
                Return _taxJurisdictionDesc
            End Get
            Set(ByVal value As String)
                _taxJurisdictionDesc = value
            End Set
        End Property

        Public Property StoreCount() As Integer
            Get
                Return _storeCount
            End Get
            Set(ByVal value As Integer)
                _storeCount = value
            End Set
        End Property

        Public Property TaxFlagCount() As Integer
            Get
                Return _taxFlagCount
            End Get
            Set(ByVal value As Integer)
                _taxFlagCount = value
            End Set
        End Property

        Public Property RegionalJurisdictionID() As String
            Get
                Return _regionalJurisdictionID
            End Get
            Set(ByVal value As String)
                _regionalJurisdictionID = Trim(value)
            End Set
        End Property

#End Region

#Region "Business Rules"

        ''' <summary>
        ''' Can delete tax jurisdiction only when no stores are associated to this tax jurisdiction
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateDelete() As TaxJurisdictionStatus
            Dim status As TaxJurisdictionStatus

            If Me.StoreCount > 0 Then
                status = TaxJurisdictionStatus.Error_StoresAssociated
            ElseIf Me.TaxFlagCount > 0 Then
                status = TaxJurisdictionStatus.Warning_TaxFlagsAssociated
            Else
                status = TaxJurisdictionStatus.Valid
            End If

            Return status
        End Function

#End Region

    End Class
End Namespace
