
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

	''' <summary>
	''' Business Object for the PriceChgType db table.
	'''
	''' This class inherits persistent properties from the regenerable base class.
	''' These properties map one-to-one to columns in the PriceChgType db table.
	'''
	''' MODIFY THIS CLASS, NOT THE BASE CLASS.
	'''
	''' Created By:	David Marine
	''' Created   :	Mar 21, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class PriceChgType
		Inherits PriceChgTypeRegen


#Region "Overriden Fields and Properties"


#End Region

#Region "New Fields and Properties"

#End Region

#Region "Constructors"

        Public Sub New()
            MyBase.New()
        End Sub


#End Region

#Region "Public Methods"

        Public Shared Function IsPromoPriceChgType(ByVal inPriceChgTypeId As Integer) As Boolean

            Dim thePriceChangeType As PriceChgType = PriceChgTypeDAO.Instance.GetPriceChgTypeByPK(CByte(inPriceChgTypeId))
            Return Not IsNothing(thePriceChangeType) AndAlso Not thePriceChangeType.IsOnSaleNull AndAlso thePriceChangeType.OnSale
        End Function

        Public Shared Function IsDiscontinuedPriceChgType(ByVal inPriceChgTypeId As Integer) As Boolean

            Dim thePriceChangeType As PriceChgType = PriceChgTypeDAO.Instance.GetPriceChgTypeByPK(CByte(inPriceChgTypeId))
            Return Not IsNothing(thePriceChangeType) AndAlso thePriceChangeType.PriceChgTypeDesc.ToLower().Equals("dis")
        End Function

        Public Shared Function IsEDVPriceChgType(ByVal inPriceChgTypeId As Integer) As Boolean
            Dim thePriceChangeType As PriceChgType = PriceChgTypeDAO.Instance.GetPriceChgTypeByPK(CByte(inPriceChgTypeId))
            Return Not IsNothing(thePriceChangeType) AndAlso Not thePriceChangeType.IsMSRPRequiredNull AndAlso thePriceChangeType.MSRPRequired
        End Function

#End Region

#Region "Private Methods"


#End Region

    End Class

End Namespace

