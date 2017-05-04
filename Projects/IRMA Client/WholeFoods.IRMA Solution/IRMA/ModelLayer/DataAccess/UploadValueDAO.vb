

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

    ''' <summary>
    ''' Generated Data Access Object class for the UploadValue db table.
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
    Public Class UploadValueDAO
        Inherits UploadValueDAORegen

#Region "Static Singleton Accessor"

        Private Shared _instance As UploadValueDAO = Nothing

        Public Shared ReadOnly Property Instance() As UploadValueDAO
            Get
                If IsNothing(_instance) Then
                    _instance = New UploadValueDAO()
                End If

                Return _instance
            End Get
        End Property

#End Region

#Region "Constructors"

        Public Sub New()

            ' strip out the "_regen" from the stored proc names to use the custom versions
            ' that bring in the associated UploadAttribute data to improve performance
            Me.GetAllUploadValuesStoredProcName = Me.GetAllUploadValuesStoredProcName.ToLower().Replace("_regen", "")
            Me.GetUploadValueByPKStoredProcName = Me.GetUploadValueByPKStoredProcName.ToLower().Replace("_regen", "")
            Me.GetUploadValuesByUploadAttributeIDStoredProcName = Me.GetUploadValuesByUploadAttributeIDStoredProcName.ToLower().Replace("_regen", "")
            Me.GetUploadValuesByUploadRowIDStoredProcName = Me.GetUploadValuesByUploadRowIDStoredProcName.ToLower().Replace("_regen", "")

        End Sub

#End Region

        ''' <summary>
        ''' Returns a list of UploadValues from the provided resultset.
        ''' </summary>
        ''' <returns>ArrayList of UploadValues</returns>
        ''' <remarks></remarks>
        Protected Overrides Function GetUploadValuesFromResultSet(ByVal results As SqlDataReader) As BusinessObjectCollection

            Dim businessObjectTable As New BusinessObjectCollection
            Dim businessObject As UploadValue

            While results.Read

                businessObject = New UploadValue()

                businessObject.UploadValueID = results.GetInt32(results.GetOrdinal("UploadValue_ID"))
                businessObject.UploadAttributeID = results.GetInt32(results.GetOrdinal("UploadAttribute_ID"))
                businessObject.UploadRowID = results.GetInt32(results.GetOrdinal("UploadRow_ID"))

                If (results.IsDBNull(results.GetOrdinal("Value"))) Then
                    businessObject.Value = Nothing
                Else
                    businessObject.Value = results.GetString(results.GetOrdinal("Value"))
                End If

                ' bring in denormalized values from the associated UploadAttribute for
                ' performance reasons
                businessObject.Name = results.GetString(results.GetOrdinal("Name"))
                businessObject.TableName = results.GetString(results.GetOrdinal("TableName"))
                businessObject.ColumnNameOrKey = results.GetString(results.GetOrdinal("ColumnNameOrKey"))
                businessObject.ControlType = results.GetString(results.GetOrdinal("ControlType"))
                businessObject.DbDataType = results.GetString(results.GetOrdinal("DbDataType"))

                ' Size can be null so set values appropriately
                If (results.IsDBNull(results.GetOrdinal("Size"))) Then
                    ' the businessObject.Size property cannot be set to null
                    ' so set the businessObject.IsSizeNull flag to true.
                    businessObject.IsSizeNull = True
                Else
                    businessObject.Size = results.GetInt32(results.GetOrdinal("Size"))
                End If
                businessObject.IsRequiredValue = results.GetBoolean(results.GetOrdinal("IsRequiredValue"))
                businessObject.IsActive = results.GetBoolean(results.GetOrdinal("IsActive"))

                ' PopulateProcedure can be null so set values appropriately
                If (results.IsDBNull(results.GetOrdinal("PopulateProcedure"))) Then
                    businessObject.PopulateProcedure = Nothing
                Else
                    businessObject.PopulateProcedure = results.GetString(results.GetOrdinal("PopulateProcedure"))
                End If

                ' PopulateIndexField can be null so set values appropriately
                If (results.IsDBNull(results.GetOrdinal("PopulateIndexField"))) Then
                    businessObject.PopulateIndexField = Nothing
                Else
                    businessObject.PopulateIndexField = results.GetString(results.GetOrdinal("PopulateIndexField"))
                End If

                ' PopulateDescriptionField can be null so set values appropriately
                If (results.IsDBNull(results.GetOrdinal("PopulateDescriptionField"))) Then
                    businessObject.PopulateDescriptionField = Nothing
                Else
                    businessObject.PopulateDescriptionField = results.GetString(results.GetOrdinal("PopulateDescriptionField"))
                End If
                businessObject.SpreadsheetPosition = results.GetInt32(results.GetOrdinal("SpreadsheetPosition"))

                ' OptionalMinValue can be null so set values appropriately
                If (results.IsDBNull(results.GetOrdinal("OptionalMinValue"))) Then
                    businessObject.OptionalMinValue = Nothing
                Else
                    businessObject.OptionalMinValue = results.GetString(results.GetOrdinal("OptionalMinValue"))
                End If

                ' OptionalMaxValue can be null so set values appropriately
                If (results.IsDBNull(results.GetOrdinal("OptionalMaxValue"))) Then
                    businessObject.OptionalMaxValue = Nothing
                Else
                    businessObject.OptionalMaxValue = results.GetString(results.GetOrdinal("OptionalMaxValue"))
                End If

                ' reset all the flags
                businessObject.IsNew = False
                businessObject.IsDirty = False
                businessObject.IsMarkedForDelete = False
                businessObject.IsDeleted = False

                ' add business object to BusinessObjectCollection with PK as key
                businessObjectTable.Add(businessObject.UploadValueID, businessObject)

            End While

            Trace.WriteLine("Loading " + businessObjectTable.Count.ToString() + " UploadValues from the database.")

            Return businessObjectTable

        End Function

    End Class

End Namespace
