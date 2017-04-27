Imports System.Data.SqlClient
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Administration.Common.BusinessLogic
    Public Class PricingMethodsBO

#Region "Property Definitions"
        Private _pricingMethodId As Integer
        Private _pricingMethodName As String
        Private _pricingMethodKey As Integer
        Private _pricingMethodFileWriterKey As Integer
#End Region

#Region "constructors and helper methods to initialize the data"
        Public Sub New()
        End Sub

        Public Sub New(ByRef results As SqlDataReader)
            Logger.LogDebug("New entry", Me.GetType())

            Try
                If (Not results.IsDBNull(results.GetOrdinal("PricingMethod_ID"))) Then
                    _pricingMethodId = results.GetInt32(results.GetOrdinal("PricingMethod_ID"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("PricingMethod_Name"))) Then
                    _pricingMethodName = results.GetString(results.GetOrdinal("PricingMethod_Name"))
                End If

            Catch ex As Exception
                Throw ex
            End Try

            Logger.LogDebug("New exit", Me.GetType())
        End Sub
#End Region

#Region "Property access methods"
        Public Property PricingMethodID() As Integer
            Get
                Return _pricingMethodId
            End Get
            Set(ByVal value As Integer)
                _pricingMethodId = value
            End Set
        End Property

        Public Property PricingMethodName() As String
            Get
                Return _pricingMethodName
            End Get
            Set(ByVal value As String)
                _pricingMethodName = value
            End Set
        End Property

        Public Property PricingMethodKey() As Integer
            Get
                Return _pricingMethodKey
            End Get
            Set(ByVal value As Integer)
                _pricingMethodKey = value
            End Set
        End Property

        Public Property PricingMethodFileWriterKey() As Integer
            Get
                Return _pricingMethodFileWriterKey
            End Get
            Set(ByVal value As Integer)
                _pricingMethodFileWriterKey = value
            End Set
        End Property
#End Region

    End Class
End Namespace

