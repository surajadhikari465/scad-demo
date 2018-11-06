

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

    ''' <summary>
    ''' Generated Data Access Object class for the UploadTypeTemplateAttribute db table.
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
    ''' Created   :	Feb 15, 2007
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UploadTypeTemplateAttributeDAO
        Inherits UploadTypeTemplateAttributeDAORegen

#Region "Static Singleton Accessor"

        Private Shared _instance As UploadTypeTemplateAttributeDAO = Nothing

        Public Shared ReadOnly Property Instance() As UploadTypeTemplateAttributeDAO
            Get
                If IsNothing(_instance) Then
                    _instance = New UploadTypeTemplateAttributeDAO()
                End If

                Return _instance
            End Get
        End Property

#End Region

    End Class

End Namespace
