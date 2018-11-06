

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

    ''' <summary>
    ''' Generated Data Access Object class for the UploadAttribute db table.
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
    Public Class UploadAttributeDAO
        Inherits UploadAttributeDAORegen

#Region "Static Singleton Accessor"

        Private Shared _instance As UploadAttributeDAO = Nothing

        Public Shared ReadOnly Property Instance() As UploadAttributeDAO
            Get
                If IsNothing(_instance) Then
                    _instance = New UploadAttributeDAO()
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
        Public Overrides Function GetAllUploadAttributes() As BusinessObjectCollection

            If IsNothing(UploadAttributeDAO.Cache) Then
                UploadAttributeDAO.Cache = MyBase.GetAllUploadAttributes()
            End If

            Return UploadAttributeDAO.Cache

        End Function


        ''' <summary>
        ''' Overridden to cache the UploadAttributes.
        ''' </summary>
        ''' <param name="uploadAttributeID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function GetUploadAttributeByPK(ByRef uploadAttributeID As System.Int32) As UploadAttribute

            Dim theUploadAttribute As UploadAttribute = Nothing

            If IsNothing(UploadAttributeDAO.Cache) Then
                UploadAttributeDAO.Cache = GetAllUploadAttributes()
            End If

            theUploadAttribute = CType(UploadAttributeDAO.Cache.ItemByKey(uploadAttributeID), UploadAttribute)

            Return theUploadAttribute

        End Function

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Returns a the value list data for the given attribute.
        ''' </summary>
        ''' <remarks></remarks>
        Public Function GetAttributeValueList(ByRef theUploadAttribute As UploadAttribute) As BusinessObjectCollection

            Dim theBusinessObjectCollection As New BusinessObjectCollection()
            Dim populateProcedureName As String = theUploadAttribute.PopulateProcedure
            Dim valueListStaticData As String = theUploadAttribute.ValueListStaticData
            Dim indexField As String = theUploadAttribute.PopulateIndexField
            Dim descriptionField As String = theUploadAttribute.PopulateDescriptionField

            If Not (IsNothing(populateProcedureName) Or String.IsNullOrEmpty(populateProcedureName)) And _
                Not (IsNothing(indexField) Or String.IsNullOrEmpty(indexField)) And _
                Not (IsNothing(descriptionField) Or String.IsNullOrEmpty(descriptionField)) Then

                Dim factory As New DataFactory(DataFactory.ItemCatalog)
                Dim paramList As New ArrayList()
                Dim results As SqlDataReader = Nothing

                Try

                    ' Execute the stored procedure 
                    results = factory.GetStoredProcedureDataReader(populateProcedureName)

                    ' populate the provided comboBox
                    While results.Read

                        If Not results.IsDBNull(results.GetOrdinal(indexField)) And Not results.IsDBNull(results.GetOrdinal(descriptionField)) Then
                            If Not theBusinessObjectCollection.ContainsKey(results.GetValue(results.GetOrdinal(indexField)).ToString()) Then
                                theBusinessObjectCollection.Add(results.GetValue(results.GetOrdinal(indexField)).ToString(), _
                                    New KeyedListItem(results.GetValue(results.GetOrdinal(indexField)).ToString(), results.GetString(results.GetOrdinal(descriptionField))))
                            Else
                                System.Console.Out.WriteLine("Duplicate found in ValueListData: " + results.GetValue(results.GetOrdinal(indexField)).ToString() + _
                                        " for list " + theUploadAttribute.Name)
                            End If
                        End If
                    End While
                Catch ex As Exception

                    Throw New Exception("An error occured while executing the ValueList store procedure """ + populateProcedureName + _
                        " with index and text fields of (" + indexField + ", " + descriptionField + ")." + _
                        "Check that the procedure and field names are correct." + ControlChars.NewLine + ControlChars.NewLine + _
                        "Original Error: " + ControlChars.NewLine + ex.ToString())

                Finally
                    If results IsNot Nothing Then
                        results.Close()
                    End If
                End Try
            ElseIf Not (IsNothing(valueListStaticData) Or String.IsNullOrEmpty(valueListStaticData)) Then

                ' parse the value list static data
                Dim thevalueListStaticDataElements As String() = valueListStaticData.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)
                Dim thevalueListStaticDataElementParts As String()
                Dim theKey As String
                Dim theText As String

                For Each thevalueListStaticDataElement As String In thevalueListStaticDataElements
                    thevalueListStaticDataElementParts = thevalueListStaticDataElement.Split(New String() {"|"}, StringSplitOptions.RemoveEmptyEntries)

                    If thevalueListStaticDataElementParts.Length = 1 Then
                        ' this allows for the value list data as configured
                        ' for flexible attributes - "a,b,c"
                        theKey = thevalueListStaticDataElementParts(0)
                        theText = thevalueListStaticDataElementParts(0)
                    Else
                        ' for all other attributes the format is "[id]|[value],[id]|[value]"
                        theKey = thevalueListStaticDataElementParts(0)
                        theText = thevalueListStaticDataElementParts(1)
                    End If

                    If Not theBusinessObjectCollection.ContainsKey(theKey) Then
                        theBusinessObjectCollection.Add(theKey, _
                             New KeyedListItem(theKey, theText))
                    Else
                        System.Console.Out.WriteLine("Duplicate found in ValueListData: " + theKey + _
                                " for list " + theUploadAttribute.Name)
                    End If
                Next
            End If

            Return theBusinessObjectCollection

        End Function

#End Region

#Region "Constructors"

        Public Sub New()

            ' strip out the "_regen" from the stored proc names to use the custom versions
            Me.GetAllUploadAttributesStoredProcName = Me.GetAllUploadAttributesStoredProcName.ToLower().Replace("_regen", "")
            Me.GetUploadAttributeByPKStoredProcName = Me.GetUploadAttributeByPKStoredProcName.ToLower().Replace("_regen", "")

        End Sub

#End Region

    End Class

End Namespace
