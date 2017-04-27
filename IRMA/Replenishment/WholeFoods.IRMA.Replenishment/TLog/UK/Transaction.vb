Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports System.Collections.Generic


Public Class Transaction
    Implements IDisposable

    Private _TransactionNumber As Integer
    Private _StoreNumber As Integer
    Private _ResgisterNumber As Integer
    Private _OperatorNumber As Integer
    Private _TransactionDate As DateTime
    Private _StartTime As DateTime
    Private _TenderTime As DateTime
    Private _EndDtime As DateTime
    Private _TransactionItems As List(Of TransactionItem) = New List(Of TransactionItem)
    Private _DiscountItems As List(Of DiscountItem) = New List(Of DiscountItem)
    Private _OfferItems As List(Of OfferItem) = New List(Of OfferItem)
    Private _PaymentItems As List(Of PaymentItem) = New List(Of PaymentItem)
    '  Private _VoidItems As List(Of VoidItem) = New List(Of VoidItem)
    Private _MiscItems As ArrayList = New ArrayList
    Private _TransactionValue As Single
    Private _IsVoided As Boolean = False
    Private _TransactionId As Integer


    Sub New()

    End Sub


#Region "Properties"
    Public Property TransactionNumber() As Integer
        Get
            Return _TransactionNumber
        End Get
        Set(ByVal value As Integer)
            _TransactionNumber = value
        End Set
    End Property

    Public Property StoreNumber() As Integer
        Get
            Return _StoreNumber
        End Get
        Set(ByVal value As Integer)
            _StoreNumber = value
        End Set
    End Property

    Public Property RegisterNumber() As Integer
        Get
            Return _ResgisterNumber
        End Get
        Set(ByVal value As Integer)
            _ResgisterNumber = value
        End Set
    End Property
    Public Property OperatorNumber() As Integer
        Get
            Return _OperatorNumber
        End Get
        Set(ByVal value As Integer)
            _OperatorNumber = value
        End Set
    End Property
    Public Property TransactionDate() As DateTime
        Get
            Return _TransactionDate
        End Get
        Set(ByVal value As DateTime)
            _TransactionDate = value
        End Set
    End Property
    Public Property StartTime() As DateTime
        Get
            Return _StartTime
        End Get
        Set(ByVal value As DateTime)
            _StartTime = value
        End Set
    End Property

    Public Property TenderTime() As DateTime
        Get
            Return _TenderTime
        End Get
        Set(ByVal value As DateTime)
            _TenderTime = value
        End Set
    End Property
    Public Property EndTime() As DateTime
        Get
            Return _EndDtime
        End Get
        Set(ByVal value As DateTime)
            _EndDtime = value
        End Set
    End Property

    Public Property TransactionValue() As Single
        Get
            Return _TransactionValue
        End Get
        Set(ByVal value As Single)
            _TransactionValue = value
        End Set
    End Property
    Public ReadOnly Property TransactionItems() As List(Of TransactionItem)
        Get
            Return _TransactionItems
        End Get
    End Property

    Public ReadOnly Property PaymentItems() As List(Of PaymentItem)
        Get
            Return _PaymentItems
        End Get
    End Property
    Public ReadOnly Property OfferItems() As List(Of OfferItem)
        Get
            Return _OfferItems
        End Get
    End Property



    Public ReadOnly Property DiscountItems() As List(Of DiscountItem)
        Get
            Return _DiscountItems
        End Get
    End Property



    Public Property IsVoided() As Boolean
        Get
            Return _IsVoided
        End Get
        Set(ByVal value As Boolean)
            _IsVoided = value
        End Set
    End Property

    Public Property TransactionId() As Integer
        Get
            Return _TransactionId
        End Get
        Set(ByVal value As Integer)
            _TransactionId = value
        End Set
    End Property


#End Region

    Public Sub AddItem(ByVal Record As String, Optional ByVal RowNo As Integer = 0)
        TransactionItems.Add(New TransactionItem(Record, RowNo))
    End Sub

    
    Public Sub AddDiscount(ByVal Record As String)
        _DiscountItems.Add(New DiscountItem(Record))
    End Sub
    Public Sub AddOffer(ByVal Record As String, ByVal TriggerItem As String)
        _OfferItems.Add(New OfferItem(Record, TriggerItem))
    End Sub
    Public Sub AddPayment(ByVal Record As String, ByVal PaymentCount As Integer)
        _PaymentItems.Add(New PaymentItem(Record, PaymentCount))
    End Sub
    'Public Sub AddVoid(ByVal Record As String)
    '    _VoidItems.Add(New VoidItem(Record))
    'End Sub

    Private Function ParseTime(ByVal t As String) As DateTime
        Return CDate(t.Substring(0, 2) & ":" & t.Substring(2, 2) & ":" & t.Substring(4, 2))
    End Function
    Private Function ParseDate(ByVal d As String) As DateTime
        Return CDate(d.Substring(4, 2) & "/" & d.Substring(6, 2) & "/" & d.Substring(0, 4))
    End Function
    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free unmanaged resources when explicitly called
            End If

            ' TODO: free shared unmanaged resources
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
