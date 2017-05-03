Imports Infragistics.Win.UltraWinGrid
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess


Namespace WholeFoods.IRMA.Administration.Common.BusinessLogic
    Public Class MenuAccessBO

#Region "Property Definitions"
        Private _menuAccessKey As Integer
        Private _menuAccessVisible As Boolean
#End Region

#Region "constructors and helper methods to initialize the data"
        Public Sub New()
        End Sub

#End Region

#Region "Property access methods"

        Public Property MenuAccessKey() As Integer
            Get
                Return _menuAccessKey
            End Get
            Set(ByVal value As Integer)
                _menuAccessKey = value
            End Set
        End Property

        Public Property MenuAccessVisible() As Boolean
            Get
                Return _menuAccessVisible
            End Get
            Set(ByVal value As Boolean)
                _menuAccessVisible = value
            End Set
        End Property
#End Region

    End Class
End Namespace