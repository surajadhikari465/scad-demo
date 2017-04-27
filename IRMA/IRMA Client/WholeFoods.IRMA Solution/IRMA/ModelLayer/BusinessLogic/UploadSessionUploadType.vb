
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

    ''' <summary>
    ''' Business Object for the UploadSessionUploadType db table.
    '''
    ''' This class inherits persistent properties from the regenerable base class.
    ''' These properties map one-to-one to columns in the UploadSessionUploadType db table.
    '''
    ''' MODIFY THIS CLASS, NOT THE BASE CLASS.
    '''
    ''' Created By:	David Marine
    ''' Created   :	Feb 12, 2007
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UploadSessionUploadType
        Inherits UploadSessionUploadTypeRegen


#Region "Overriden Fields and Properties"


#End Region

#Region "New Fields and Properties"


#End Region

#Region "Constructors"

        Public Sub New()
            MyBase.New()
        End Sub

        ''' <summary>
        ''' Construct a UploadSessionUploadType by passing in
        ''' its parent business objects.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByRef inUploadTypeTemplate As UploadTypeTemplate, ByRef inUploadType As UploadType, ByRef inUploadSession As UploadSession)
            MyBase.New(inUploadTypeTemplate, inUploadType, inUploadSession)
        End Sub

#End Region

#Region "Public Methods"


#End Region

#Region "Private Methods"


#End Region

    End Class

End Namespace

