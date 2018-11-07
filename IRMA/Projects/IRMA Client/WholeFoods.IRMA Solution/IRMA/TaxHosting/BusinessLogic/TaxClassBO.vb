Imports Infragistics.Win.UltraWinGrid

Public Enum TaxClassStatus
    Valid
    Error_ItemsAssociated
    Warning_TaxFlagsAssociated
End Enum

Namespace WholeFoods.IRMA.TaxHosting.BusinessLogic
    Public Class TaxClassBO

#Region "Property Definitions"

        Private _taxClassId As Integer
        Private _taxClassDesc As String
        Private _itemCount As Integer
        Private _taxFlagCount As Integer

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
            If currentRow.Cells("TaxClassID").Value IsNot DBNull.Value Then
                Me.TaxClassId = CType(currentRow.Cells("TaxClassID").Value, Integer)
            End If

            Me.TaxClassDesc = currentRow.Cells("TaxClassDesc").Value.ToString

            If currentRow.Cells("ItemCount").Value IsNot DBNull.Value Then
                Me.ItemCount = CType(currentRow.Cells("ItemCount").Value, Integer)
            End If

            If currentRow.Cells("TaxFlagCount").Value IsNot DBNull.Value Then
                Me.TaxFlagCount = CType(currentRow.Cells("TaxFlagCount").Value, Integer)
            End If
        End Sub

#End Region

#Region "Property Access Methods"

        Public Property TaxClassId() As Integer
            Get
                Return _taxClassId
            End Get
            Set(ByVal value As Integer)
                _taxClassId = value
            End Set
        End Property

        Public Property TaxClassDesc() As String
            Get
                Return _taxClassDesc
            End Get
            Set(ByVal value As String)
                _taxClassDesc = value
            End Set
        End Property

        Public Property ItemCount() As Integer
            Get
                Return _itemCount
            End Get
            Set(ByVal value As Integer)
                _itemCount = value
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

#End Region

#Region "Business Rules"

        ''' <summary>
        ''' Can delete tax class only when no items are associated to this tax class
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateDelete() As TaxClassStatus
            Dim status As TaxClassStatus

            If Me.ItemCount > 0 Then
                status = TaxClassStatus.Error_ItemsAssociated
            ElseIf Me.TaxFlagCount > 0 Then
                status = TaxClassStatus.Warning_TaxFlagsAssociated
            Else
                status = TaxClassStatus.Valid
            End If

            Return status
        End Function

#End Region

    End Class
End Namespace
