Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Collections.Generic
Imports AjaxControlToolkit

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<System.Web.Script.Services.ScriptService()> _
Public Class Hierarchy
    Inherits System.Web.Services.WebService

#Region "Helper Methods"

    Private Shared Sub AddDropDownValues(ByVal values As List(Of CascadingDropDownNameValue), ByVal reader As SqlClient.SqlDataReader, ByVal nameColumn As String, ByVal identifierColumn As String, ByVal exclusions As String())
        Dim identifier As String

        While reader.Read()
            identifier = reader.GetInt32(reader.GetOrdinal(identifierColumn)).ToString()

            ' No exclusions or not within the exclusions array
            If exclusions Is Nothing OrElse Array.IndexOf(exclusions, identifier) < 0 Then
                values.Add(New CascadingDropDownNameValue(reader.GetString(reader.GetOrdinal(nameColumn)), identifier))
            End If
        End While

        reader.Close()
    End Sub

    Private Shared Sub AddDropDownValues(ByVal values As List(Of CascadingDropDownNameValue), ByVal reader As SqlClient.SqlDataReader, ByVal nameColumn As String, ByVal identifierColumn As String)
        AddDropDownValues(values, reader, nameColumn, identifierColumn, Nothing)
    End Sub

    Private Shared Function GetParentValue(ByVal categoryValues As StringDictionary, ByRef identifier As Integer, ByVal parentKey As String) As Boolean
        Return categoryValues.ContainsKey(parentKey) AndAlso Integer.TryParse(categoryValues(parentKey), identifier)
    End Function

#End Region

    <WebMethod(), System.Web.Script.Services.ScriptMethod()> _
    Public Function GetHierarchyValues(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As CascadingDropDownNameValue()
        Dim categoryValues As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim values As New List(Of CascadingDropDownNameValue)
        Dim identifier As Integer

        Select Case category
            Case "SubTeam"
                Dim userID As Integer = 0
                Dim reader As SqlClient.SqlDataReader = Nothing
                Dim contextValues As String() = Nothing
                Dim exclusions As String() = Nothing

                If Not String.IsNullOrEmpty(contextKey) Then
                    ' Format UserID:Exclusion,Exclusion,...
                    contextValues = contextKey.Split(":")

                    If contextValues.Length > 0 Then
                        Integer.TryParse(contextValues(0), userID)

                        If contextValues.Length > 1 Then
                            exclusions = contextValues(1).Split(",")
                        End If
                    End If
                End If

                If userID > 0 Then
                    reader = ItemSearch.GetUserSubTeamsReader(userID)
                Else
                    reader = ItemSearch.GetAllSubTeamsReader()
                End If

                AddDropDownValues(values, reader, "SubTeam_Name", "SubTeam_No", exclusions)
            Case "Category"
                If GetParentValue(categoryValues, identifier, "SubTeam") Then
                    AddDropDownValues(values, ItemSearch.GetCategoriesBySubTeamReader(identifier), "Category_Name", "Category_ID")
                End If
            Case "Level 3"
                If GetParentValue(categoryValues, identifier, "Category") Then
                    AddDropDownValues(values, ItemSearch.GetProdHierarchyLevel3sByCategoryReader(identifier), "Description", "ProdHierarchyLevel3_ID")
                End If
            Case "Level 4"
                If GetParentValue(categoryValues, identifier, "Level 3") Then
                    AddDropDownValues(values, ItemSearch.GetProdHierarchyLevel4sByLevel3Reader(identifier), "Description", "ProdHierarchyLevel4_ID")
                End If
        End Select

        Return values.ToArray()
    End Function

End Class
