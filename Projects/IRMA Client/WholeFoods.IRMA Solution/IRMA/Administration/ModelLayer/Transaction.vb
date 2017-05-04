Imports System.Data.SqlClient

Namespace WholeFoods.IRMA.ModelLayer

	''' <summary>
	''' 
    ''' This class registers and hands out transactions
    ''' by current thread.
	'''
    ''' DO NOT MODIFY THIS CLASS.
	'''
	''' Created By:	David Marine
	''' Created   :	Feb 12, 2007
	''' </summary>
	''' <remarks></remarks>
    Public Class Transaction

#Region "Fields and Properties"

        Private _transactionManager As TransactionManager
        Private _sqlTransaction As SqlTransaction

        Public Property SqlTransaction() As SqlTransaction
            Get
                Return _sqlTransaction
            End Get
            Set(ByVal value As SqlTransaction)
                _sqlTransaction = value
            End Set
        End Property

        Public Property TransactionManager() As TransactionManager
            Get
                Return _transactionManager
            End Get
            Set(ByVal value As TransactionManager)
                _transactionManager = value
            End Set
        End Property

#End Region

#Region "Constructors"

        Public Sub New(ByRef inTransactionManager As TransactionManager)
            MyBase.New()

            Me.TransactionManager = inTransactionManager
            Me.TransactionManager.RegisterNewTransaction(Me)

        End Sub

        Public Sub Commit()

            If Not IsNothing(Me.SqlTransaction) Then
                Me.SqlTransaction.Commit()
                Me.TransactionManager.UnregisterCurrentTransaction(Me)
            End If

        End Sub

        Public Sub Rollback()

            If Not IsNothing(Me.SqlTransaction) Then
                Me.SqlTransaction.Rollback()
                Me.TransactionManager.UnregisterCurrentTransaction(Me)
            End If

        End Sub

#End Region

#Region "Public Methods"

#End Region

#Region "Private Methods"

#End Region

    End Class

End Namespace

