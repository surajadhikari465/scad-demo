Imports WholeFoods.IRMA.Common.DataAccess

Namespace WholeFoods.IRMA.Pricing.BusinessLogic
    Public Class GlobalPriceManagementBO
        Private _isGlobalPriceManagementEnabled As Boolean
        Public Property IsGlobalPriceManagementEnabled() As Boolean
            Get
                Return _isGlobalPriceManagementEnabled
            End Get
            Set(ByVal value As Boolean)
                _isGlobalPriceManagementEnabled = value
            End Set
        End Property

        Private _areAllStoresGpm As Boolean
        Public Property AreAllStoresGpm() As Boolean
            Get
                Return _areAllStoresGpm
            End Get
            Set(ByVal value As Boolean)
                _areAllStoresGpm = value
            End Set
        End Property

        Public Sub New()
            _isGlobalPriceManagementEnabled = InstanceDataDAO.IsFlagActive("GlobalPriceManagement")
            _areAllStoresGpm = InstanceDataDAO.FlagHasNoStoreOverrides("GlobalPriceManagement")
        End Sub

    End Class
End Namespace
