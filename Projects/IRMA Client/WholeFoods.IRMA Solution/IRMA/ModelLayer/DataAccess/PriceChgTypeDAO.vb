

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

	''' <summary>
	''' Generated Data Access Object class for the PriceChgType db table.
	'''
	''' This class inherits the following CRUD methods from its regenerable base class:
	'''    1. Find method by PK
	'''    2. Find method for each FK
	'''    3. Insert Method
	'''    4. Update Method
	'''    5. Delete Method
	'''
	''' MODIFY THIS CLASS, NOT THE BASE CLASS.
	'''
	''' Created By:	David Marine
	''' Created   :	Mar 21, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class PriceChgTypeDAO
		Inherits PriceChgTypeDAORegen
	
#Region "Static Singleton Accessor"
	
		Private Shared _instance As PriceChgTypeDAO = Nothing
	
        Public Shared ReadOnly Property Instance() As PriceChgTypeDAO
            Get
            	If IsNothing(_instance) Then
            		_instance = New PriceChgTypeDAO()
            	End If
            	
                Return _instance
            End Get
        End Property
    
#End Region

#Region "Fields and Properties"

        Private Shared _cache As BusinessObjectCollection = Nothing

        Public Shared Property Cache() As BusinessObjectCollection
            Get
                Return _cache
            End Get
            Set(ByVal value As BusinessObjectCollection)
                _cache = value
            End Set
        End Property

#End Region

#Region "Overriden Methods"

        ''' <summary>
        ''' Overridden to cache the UploadAttributes.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function GetAllPriceChgTypes() As BusinessObjectCollection

            If IsNothing(PriceChgTypeDAO.Cache) Then
                PriceChgTypeDAO.Cache = MyBase.GetAllPriceChgTypes()
            End If

            Return PriceChgTypeDAO.Cache

        End Function


        ''' <summary>
        ''' Overridden to cache the UploadAttributes.
        ''' </summary>
        ''' <param name="PriceChgTypeID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function GetPriceChgTypeByPK(ByRef PriceChgTypeID As System.Byte) As PriceChgType

            Dim thePriceChgType As PriceChgType = Nothing

            If IsNothing(PriceChgTypeDAO.Cache) Then
                PriceChgTypeDAO.Cache = GetAllPriceChgTypes()
            End If

            thePriceChgType = CType(PriceChgTypeDAO.Cache.ItemByKey(PriceChgTypeID), PriceChgType)

            Return thePriceChgType

        End Function

#End Region
    End Class

End Namespace
