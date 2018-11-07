

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

    ''' <summary>
    ''' This is not a generated class.
    ''' It has been hand coded to return a DataSet of all
    ''' ItemChain IDs and names.
    ''' Created By:	David Marine
    ''' Created   :	July 4, 2007
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ItemChainDAO

#Region "Static Singleton Accessor"

        Private Shared _instance As ItemChainDAO = Nothing

        Public Shared ReadOnly Property Instance() As ItemChainDAO
            Get
                If IsNothing(_instance) Then
                    _instance = New ItemChainDAO()
                End If

                Return _instance
            End Get
        End Property

#End Region

#Region "Fields and Properties"

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Returns a DataSet of ids and names of all Item Chains.
        ''' </summary>
        ''' <remarks></remarks>
        Public Function GetAllItemChains() As DataSet

            Dim theDataSet As New DataSet()

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing

            ' Execute the stored procedure 
            theDataSet = factory.GetStoredProcedureDataSet("GetItemChains")

            Return theDataSet

        End Function

        ''' <summary>
        ''' Runs the populate procedure specified and fills the given comboBox.
        ''' </summary>
        ''' <remarks></remarks>
        Public Function AddNamesToItemChainIdList(ByVal inItemChainIdList As String) As String

            Dim theItemChainIdNameList As String = ""

            If Not IsNothing(inItemChainIdList) And _
                Not String.IsNullOrEmpty(inItemChainIdList) Then
                Dim factory As New DataFactory(DataFactory.ItemCatalog)
                Dim paramList As New ArrayList()
                Dim currentParam As DBParam
                Dim theOutputList As New ArrayList

                ' ItemChainIdList
                currentParam = New DBParam
                currentParam.Name = "ItemChainIdList"
                currentParam.Type = DBParamType.String
                currentParam.Value = inItemChainIdList
                paramList.Add(currentParam)

                ' output param
                currentParam = New DBParam
                currentParam.Name = "ItemChainIdAndNameList"
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                theOutputList = factory.ExecuteStoredProcedure("EIM_AddChainNamesToIdList", paramList)

                theItemChainIdNameList = CStr(theOutputList(0))
            End If

            Return theItemChainIdNameList

        End Function

        ''' <summary>
        ''' Runs the populate procedure specified and fills the given comboBox.
        ''' </summary>
        ''' <remarks></remarks>
        Public Function GetChainIdsFromNameList(ByVal inItemChainNameList As String) As String

            Dim theItemChainIdList As String = ""

            If Not IsNothing(inItemChainNameList) And _
                Not String.IsNullOrEmpty(inItemChainNameList) Then
                Dim factory As New DataFactory(DataFactory.ItemCatalog)
                Dim paramList As New ArrayList()
                Dim currentParam As DBParam
                Dim theOutputList As New ArrayList

                ' ItemChainIdList
                currentParam = New DBParam
                currentParam.Name = "ItemChainNameList"
                currentParam.Type = DBParamType.String
                currentParam.Value = inItemChainNameList
                paramList.Add(currentParam)

                ' output param
                currentParam = New DBParam
                currentParam.Name = "ItemChainIdList"
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                theOutputList = factory.ExecuteStoredProcedure("EIM_GetChainIdsFromNameList", paramList)

                theItemChainIdList = CStr(theOutputList(0))
            End If

            Return theItemChainIdList

        End Function

        Public Function ExtractChainIdsFromIdAndNameList(ByVal inValue As String) As String
            ' the chains for an item are displayed as
            ' "[id]: [name], [id]: [name]"
            ' we need to translate it to "[id], [id]"
            Dim theChainIdAndNameArray As String() = inValue.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)
            Dim theChainValueArray As String()
            Dim theChainIdList As String = Nothing

            For Each theChainIdAndName As String In theChainIdAndNameArray
                theChainValueArray = theChainIdAndName.Split(New String() {":"}, StringSplitOptions.RemoveEmptyEntries)

                If Not IsNothing(theChainIdList) Then
                    theChainIdList = theChainIdList + ", "
                End If
                theChainIdList = theChainIdList + theChainValueArray(0)
            Next

            Return theChainIdList

        End Function

        Public Function ExtractChainNamesFromIdAndNameList(ByVal inValue As String, ByVal inIsOnlyIds As Boolean) As String
            ' the chains for an item are displayed as
            ' "[id]: [name], [id]: [name]"
            ' we need to translate it to "[name], [name]"

            If inIsOnlyIds Then
                ' add the names if we only have a list of ids
                ' while this *is* extra work, it better reuses existing code
                inValue = AddNamesToItemChainIdList(inValue)
            End If

            Dim theChainIdAndNameArray As String() = inValue.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)
            Dim theChainValueArray As String()
            Dim theChainIdList As String = Nothing

            For Each theChainIdAndName As String In theChainIdAndNameArray
                theChainValueArray = theChainIdAndName.Split(New String() {":"}, StringSplitOptions.RemoveEmptyEntries)

                If Not IsNothing(theChainIdList) Then
                    theChainIdList = theChainIdList + ", "
                End If
                theChainIdList = theChainIdList + theChainValueArray(1)
            Next

            Return theChainIdList

        End Function

#End Region

    End Class

End Namespace
