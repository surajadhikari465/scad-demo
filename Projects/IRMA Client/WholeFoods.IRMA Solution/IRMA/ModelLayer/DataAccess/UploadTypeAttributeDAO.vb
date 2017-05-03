

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

    ''' <summary>
    ''' Generated Data Access Object class for the UploadTypeAttribute db table.
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
    ''' Created   :	Feb 12, 2007
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UploadTypeAttributeDAO
        Inherits UploadTypeAttributeDAORegen

#Region "Static Singleton Accessor"

        Private Shared _instance As UploadTypeAttributeDAO = Nothing

        Public Shared ReadOnly Property Instance() As UploadTypeAttributeDAO
            Get
                If IsNothing(_instance) Then
                    _instance = New UploadTypeAttributeDAO()
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
        ''' Overridden to cache the UploadTypeAttributes.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function GetAllUploadTypeAttributes() As BusinessObjectCollection

            If IsNothing(UploadTypeAttributeDAO.Cache) Then
                UploadTypeAttributeDAO.Cache = MyBase.GetAllUploadTypeAttributes()
            End If

            Return UploadTypeAttributeDAO.Cache

        End Function

        ''' <summary>
        ''' Overridden to cache the UploadTypeAttributes.
        ''' </summary>
        ''' <param name="uploadTypeAttributeID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function GetUploadTypeAttributeByPK(ByRef uploadTypeAttributeID As System.Int32) As UploadTypeAttribute

            Dim theUploadTypeAttribute As UploadTypeAttribute = Nothing

            If IsNothing(UploadTypeAttributeDAO.Cache) Then
                UploadTypeAttributeDAO.Cache = GetAllUploadTypeAttributes()
            End If

            theUploadTypeAttribute = CType(UploadTypeAttributeDAO.Cache.ItemByKey(uploadTypeAttributeID), UploadTypeAttribute)

            Return theUploadTypeAttribute

        End Function

#End Region

#Region "Constructors"

        Public Sub New()

            ' strip out the "_regen" from the stored proc names to use the custom versions
            ' that filter out UploadTypeAttributes for inactive UploadAttributes
            Me.GetAllUploadTypeAttributesStoredProcName = Me.GetAllUploadTypeAttributesStoredProcName.ToLower().Replace("_regen", "")
            Me.GetUploadTypeAttributeByPKStoredProcName = Me.GetUploadTypeAttributeByPKStoredProcName.ToLower().Replace("_regen", "")
            Me.GetUploadTypeAttributesByUploadTypeCodeStoredProcName = Me.GetUploadTypeAttributesByUploadTypeCodeStoredProcName.ToLower().Replace("_regen", "")
            Me.GetUploadTypeAttributesByUploadAttributeIDStoredProcName = Me.GetUploadTypeAttributesByUploadAttributeIDStoredProcName.ToLower().Replace("_regen", "")

        End Sub

#End Region

    End Class

End Namespace
