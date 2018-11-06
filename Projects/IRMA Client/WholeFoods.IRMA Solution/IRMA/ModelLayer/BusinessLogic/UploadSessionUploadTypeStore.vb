
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

    ''' <summary>
    ''' Business Object for the UploadSessionUploadTypeStore db table.
    '''
    ''' This class inherits persistent properties from the regenerable base class.
    ''' These properties map one-to-one to columns in the UploadSessionUploadTypeStore db table.
    '''
    ''' MODIFY THIS CLASS, NOT THE BASE CLASS.
    '''
    ''' Created By:	David Marine
    ''' Created   :	Feb 12, 2007
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UploadSessionUploadTypeStore
        Inherits UploadSessionUploadTypeStoreRegen


#Region "Overriden Fields and Properties"


#End Region

#Region "New Fields and Properties"


#End Region

#Region "Constructors"

        Public Sub New()
            MyBase.New()
        End Sub

        ''' <summary>
        ''' Construct a UploadSessionUploadTypeStore by passing in
        ''' its parent business objects.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByRef inUploadSessionUploadType As UploadSessionUploadType)
            MyBase.New(inUploadSessionUploadType)
        End Sub

#End Region

#Region "Public Methods"


#End Region

#Region "Private Methods"


#End Region

    End Class

End Namespace

