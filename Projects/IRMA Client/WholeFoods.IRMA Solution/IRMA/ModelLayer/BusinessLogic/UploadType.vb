
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

    ''' <summary>
    ''' Business Object for the UploadType db table.
    '''
    ''' This class inherits persistent properties from the regenerable base class.
    ''' These properties map one-to-one to columns in the UploadType db table.
    '''
    ''' MODIFY THIS CLASS, NOT THE BASE CLASS.
    '''
    ''' Created By:	David Marine
    ''' Created   :	Feb 12, 2007
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UploadType
        Inherits UploadTypeRegen


#Region "Overriden Fields and Properties"


#End Region

#Region "New Fields and Properties"

        Private _uploadTypeAttributeByKey As Hashtable = Nothing


        ''' <summary>
        ''' Gets a Hashtable of UploadTypeAttributes keyed by Key
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UploadTypeAttributeByKey() As Hashtable
            Get
                If IsNothing(_uploadTypeAttributeByKey) Then

                    _uploadTypeAttributeByKey = New Hashtable()

                    For Each theUploadTypeAttribute As UploadTypeAttribute _
                            In Me.UploadTypeAttributeCollection

                        If Not _uploadTypeAttributeByKey.ContainsKey(theUploadTypeAttribute.UploadAttribute.Key) Then
                            _uploadTypeAttributeByKey.Add(theUploadTypeAttribute.UploadAttribute.Key, _
                                theUploadTypeAttribute)
                        End If
                    Next
                End If

                Return _uploadTypeAttributeByKey

            End Get
        End Property

        ''' <summary>
        ''' Returns a value used for sorting the UploadTypes by code.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SortKey() As Integer
            Get

                Dim theSortKey As Integer = 0

                Select Case Me.UploadTypeCode
                    Case EIM_Constants.ITEM_MAINTENANCE_CODE
                        theSortKey = 1
                    Case EIM_Constants.PRICE_UPLOAD_CODE
                        theSortKey = 2
                    Case EIM_Constants.COST_UPLOAD_CODE
                        theSortKey = 3
                End Select

                Return theSortKey

            End Get
        End Property


#End Region

#Region "Constructors"

        Public Sub New()
            MyBase.New()
        End Sub


#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Returns the UpdateTypeAttribute for the provided attribute key.
        ''' </summary>
        ''' <param name="attributeKey"></param>
        ''' <returns>UpdateTypeAttribute</returns>
        ''' <remarks></remarks>
        Public Function FindUploadTypeAttributeByKey(ByVal attributeKey As String) As UploadTypeAttribute

            Dim theUploadTypeAttribute As UploadTypeAttribute = Nothing

            If Not String.IsNullOrEmpty(attributeKey) Then
                theUploadTypeAttribute = CType(Me.UploadTypeAttributeByKey.Item(attributeKey), UploadTypeAttribute)
            End If

            Return theUploadTypeAttribute

        End Function

#End Region

#Region "Private Methods"


#End Region

    End Class

End Namespace

