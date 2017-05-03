Imports System.Data.SqlClient
Imports System.Threading
Imports WholeFoods.Utility.DataAccess

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
    Public Class TransactionManager

#Region "Static Singleton Accessor"

        Private Shared _instance As TransactionManager = Nothing

        Public Shared ReadOnly Property Instance() As TransactionManager
            Get
                If IsNothing(_instance) Then
                    _instance = New TransactionManager()
                End If

                Return _instance
            End Get
        End Property

#End Region

#Region "Fields and Properties"

        Private _transactionsByThread As New Hashtable

        Private Property TransactionsByThread() As Hashtable
            Get
                Return _transactionsByThread
            End Get
            Set(ByVal value As Hashtable)
                _transactionsByThread = value
            End Set
        End Property

        Public Property CurrentTransaction() As Transaction
            Get
                Return CType(_transactionsByThread.Item(Thread.CurrentThread), Transaction)
            End Get
            Set(ByVal value As Transaction)
                _transactionsByThread.Add(Thread.CurrentThread, value)
            End Set
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Public Methods"

        Public Sub RegisterNewTransaction(ByRef inTransaction As Transaction)

            If Not IsNothing(Me.TransactionsByThread.ContainsKey(Thread.CurrentThread)) Then
                ' throw an exception if there is a transaction already for the
                ' current thread
                Throw New NestedTransactionException("A new transaction has been created when there is already an open transaction.")
            Else
                ' add the transaction to the Hashtable
                Me.TransactionsByThread.Add(Thread.CurrentThread, inTransaction)

                Dim factory As New DataFactory(DataFactory.ItemCatalog)
                inTransaction.SqlTransaction = factory.BeginTransaction(IsolationLevel.RepeatableRead)
            End If

        End Sub

        Public Sub UnregisterCurrentTransaction(ByRef inTransaction As Transaction)

            ' remove the transaction from the Hashtable
            Me.TransactionsByThread.Remove(Thread.CurrentThread)

        End Sub

#End Region

#Region "Private Methods"

#End Region

    End Class

End Namespace

