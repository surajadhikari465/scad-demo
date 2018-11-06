Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.Writers.Constants


Namespace WholeFoods.IRMA.Common.DataAccess
    Public Class ConfigurationDataDAO
        ' ************************************************************
        ' THIS CLASS IS DUPLICATED in the IRMA ADMINISTRATION project.
        ' until we have a repository for common code.
        ' ************************************************************

        ' The purpose of this class is to manage the ConfigurationData table
        ' The table is made to take data of various types and store them in the same sql_variant field
        ' to be available as values in the application

        ' Rick Kelleher

        Private factory As DataFactory

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            CreateDatabaseConnection()
        End Sub

        ''' <summary>
        ''' CreateDatabaseConnection from the data factory
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CreateDatabaseConnection()
            If factory Is Nothing Then
                factory = New DataFactory(DataFactory.ItemCatalog)
            End If
        End Sub

        ''' <summary>
        ''' GetConfigInfo() returns all configuration data records from the ConfigurationData table
        ''' </summary>
        ''' <returns>Returns a dataset containing a single table</returns>
        ''' <remarks></remarks>
        Public Function GetConfigInfo() As DataSet
            Dim ds As DataSet = factory.GetStoredProcedureDataSet("GetConfigurationData")
            Return ds
        End Function

        ''' <summary>
        ''' GetConfigValue() returns a single configuration data record from the ConfigurationData table
        ''' </summary>
        ''' <param name="sConfigKey">The key (a string) defining the value to retrieve</param>
        ''' <returns>Returns an object of elemental data type (actaully a value of type SQL_VARIANT)</returns>
        ''' <remarks></remarks>
        Public Function GetConfigValue(ByVal sConfigKey As String) As Object
            Dim retval As Object = factory.ExecuteScalar("EXEC GetConfigurationValue '" & sConfigKey & "'")
            Return retval
        End Function

        ''' <summary>
        ''' UpdateConfigInfo is for updating the dataset created by GetConfigInfo(). 
        ''' </summary>
        ''' <param name="ds">The dataset containing a single table containing the modified data to be updated.</param>
        ''' <param name="TblName">The name of the table in the dataset</param>
        ''' <returns></returns>
        ''' <remarks>It updates the value of all records marked as modified back into the ConfigurationData table.
        ''' The table must have the same structure as the one created by GetConfigInfo().
        ''' It attempts to keep the base data type intact of the values being saved back into the SQL_VARIANT field.
        ''' </remarks>
        Public Function UpdateConfigInfo(ByVal ds As DataSet, ByVal TblName As String) As Boolean
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            UpdateConfigInfo = False
            If ds.HasChanges Then
                ' get only the changed rows for updating
                Dim dv As DataView = New DataView(ds.Tables(TblName), "", "", DataViewRowState.ModifiedCurrent)
                Dim rowView As DataRowView

                For Each rowView In dv
                    paramList = New ArrayList

                    ' setup parameters for stored proc
                    currentParam = New DBParam
                    currentParam.Name = "iConfigKey"
                    currentParam.Value = rowView.Item("ConfigKey").ToString
                    currentParam.Type = DBParamType.String
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "iConfigValue"
                    currentParam.Value = rowView.Item("ConfigValue")
                    Select Case rowView.Item("BaseType").ToString
                        Case "bit"
                            currentParam.Type = DBParamType.Bit
                        Case "char"
                            currentParam.Type = DBParamType.Char
                        Case "datetime"
                            currentParam.Type = DBParamType.DateTime
                        Case "decimal", "real"
                            currentParam.Type = DBParamType.Decimal
                        Case "float"
                            currentParam.Type = DBParamType.Float
                        Case "int"
                            currentParam.Type = DBParamType.Int
                        Case "money"
                            currentParam.Type = DBParamType.Money
                        Case "smallint"
                            currentParam.Type = DBParamType.SmallInt
                        Case "varchar"
                            currentParam.Type = DBParamType.String
                    End Select
                    paramList.Add(currentParam)
                    ' Execute the stored procedure 
                    factory.ExecuteStoredProcedure("UpdateConfigurationData", paramList)
                Next
            End If
            UpdateConfigInfo = True
        End Function

    End Class

End Namespace
