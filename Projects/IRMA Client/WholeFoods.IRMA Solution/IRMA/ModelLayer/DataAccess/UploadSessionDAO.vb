
Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common.DataAccess


Namespace WholeFoods.IRMA.ModelLayer.DataAccess

    ''' <summary>
    ''' Generated Data Access Object class for the UploadSession db table.
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
    Public Class UploadSessionDAO
        Inherits UploadSessionDAORegen

#Region "Static Singleton Accessor"

        Private Shared _instance As UploadSessionDAO = Nothing

        Public Shared ReadOnly Property Instance() As UploadSessionDAO
            Get
                If IsNothing(_instance) Then
                    _instance = New UploadSessionDAO()
                End If

                Return _instance
            End Get
        End Property

#End Region

#Region "Public Methods"

        Public Function SessionSearch(ByVal uploadSessionId As Integer, ByVal uploadTypeCode As String,
            ByVal sessionName As String, ByVal userId As Integer, ByVal createdDate As DateTime,
            ByVal savedOnly As Boolean, ByVal uploadedOnly As Boolean, ByVal savedAndUploaded As Boolean) As DataSet

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim theDataSet As DataSet

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "UploadSession_ID"
            currentParam.Type = DBParamType.Int
            If uploadSessionId < 1 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = uploadSessionId
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UploadTypeCode"
            currentParam.Type = DBParamType.String
            If uploadTypeCode.Equals("all", StringComparison.CurrentCultureIgnoreCase) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = uploadTypeCode
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SessionName"
            currentParam.Type = DBParamType.String
            If String.IsNullOrEmpty(sessionName) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = sessionName
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "CreatedByUserId"
            currentParam.Type = DBParamType.Int
            If userId < 1 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = userId
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "CreatedDateTime"
            currentParam.Type = DBParamType.DateTime
            If createdDate.Year = 1980 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = createdDate
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SavedOnly"
            currentParam.Type = DBParamType.Bit
            currentParam.Value = savedOnly
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UploadedOnly"
            currentParam.Type = DBParamType.Bit
            currentParam.Value = uploadedOnly
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SavedAndUploaded"
            currentParam.Type = DBParamType.Bit
            currentParam.Value = savedAndUploaded
            paramList.Add(currentParam)

            theDataSet = factory.GetStoredProcedureDataSet("EIM_SearchUploadSessions", paramList)

            Return theDataSet

        End Function

        Public Function ItemSearch(ByVal identifier As String, ByVal description As String,
            ByVal vendorID As Integer, ByVal brandID As Integer, ByVal storeIDs As String,
            ByVal subteamID As Integer, ByVal categoryID As Integer,
            ByVal level3ID As Integer, ByVal level4ID As Integer,
            ByVal psVendorID As String, ByVal vendorItemID As String,
            ByVal distSubteamID As Integer, ByVal itemChainID As Integer,
            ByVal isDiscontinued As Boolean, ByVal isNotAvailable As Boolean,
            ByVal includeDeletedItems As Boolean,
            ByVal inIsDefaultJurisdiction As Integer, ByVal inStoreJurisdictionId As Integer,
            Optional ByVal isFromSLIM As Boolean = False
            ) As DataSet

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            factory.CommandTimeout = 60 * 10

            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim theDataSet As DataSet = Nothing

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Identifier"
            currentParam.Type = DBParamType.String
            currentParam.Value = identifier
            If String.IsNullOrEmpty(identifier) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = identifier
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Item_Description"
            currentParam.Type = DBParamType.String
            If String.IsNullOrEmpty(description) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = description
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Vendor_ID"
            currentParam.Type = DBParamType.Int
            If vendorID < 1 Then
                currentParam.Value = 0
            Else
                currentParam.Value = vendorID
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Brand_ID"
            currentParam.Type = DBParamType.Int
            If brandID < 1 Then
                currentParam.Value = 0
            Else
                currentParam.Value = brandID
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StoreNos"
            currentParam.Type = DBParamType.String
            currentParam.Value = storeIDs

            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Subteam_No"
            currentParam.Type = DBParamType.Int
            If subteamID < 1 Then
                currentParam.Value = 0
            Else
                currentParam.Value = subteamID
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Category_ID"
            currentParam.Type = DBParamType.Int
            If categoryID < 1 Then
                currentParam.Value = 0
            Else
                currentParam.Value = categoryID
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Level3_ID"
            currentParam.Type = DBParamType.Int
            If level3ID < 1 Then
                currentParam.Value = 0
            Else
                currentParam.Value = level3ID
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Level4_ID"
            currentParam.Type = DBParamType.Int
            If level4ID < 1 Then
                currentParam.Value = 0
            Else
                currentParam.Value = level4ID
            End If
            paramList.Add(currentParam)

            '------------------

            currentParam = New DBParam
            currentParam.Name = "PS_Vendor_ID"
            currentParam.Type = DBParamType.String
            If IsNothing(psVendorID) OrElse String.IsNullOrEmpty(psVendorID) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = psVendorID
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Vendor_Item_ID"
            currentParam.Type = DBParamType.String
            If IsNothing(vendorItemID) OrElse String.IsNullOrEmpty(vendorItemID) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = vendorItemID
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "DistSubTeam_No"
            currentParam.Type = DBParamType.Int
            If distSubteamID < 1 Then
                currentParam.Value = 0
            Else
                currentParam.Value = distSubteamID
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ItemChainID"
            currentParam.Type = DBParamType.Int
            If itemChainID < 1 Then
                currentParam.Value = 0
            Else
                currentParam.Value = itemChainID
            End If
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Discontinue_Item"
            currentParam.Type = DBParamType.Bit
            currentParam.Value = isDiscontinued
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Exclude_Not_Available"
            currentParam.Type = DBParamType.Bit
            currentParam.Value = isNotAvailable
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IncludeDeletedItems"
            currentParam.Type = DBParamType.Bit
            currentParam.Value = includeDeletedItems
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LoadFromSLIM"
            currentParam.Type = DBParamType.Bit
            currentParam.Value = isFromSLIM
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IsDefaultJurisdiction"
            currentParam.Type = DBParamType.Int
            currentParam.Value = inIsDefaultJurisdiction
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "JurisdictionId"
            currentParam.Type = DBParamType.Int
            currentParam.Value = inStoreJurisdictionId
            paramList.Add(currentParam)

            theDataSet = factory.GetStoredProcedureDataSet("EIM_ItemLoadSearch", paramList)

            Return theDataSet

        End Function

        Public Function UploadSession(ByRef inUploadSession As UploadSession,
                ByVal inFromSLIM As Boolean) As Boolean

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim theResultsArray As ArrayList

            Try
                ' up the timeout from 0.5 to 60 to 10 hours
                factory.CommandTimeout = 60 * 60 * 10

                inUploadSession.ProgressComplete = False

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "UploadSession_ID"
                currentParam.Type = DBParamType.Int
                currentParam.Value = inUploadSession.UploadSessionID
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "FromSLIM"
                currentParam.Type = DBParamType.Bit
                currentParam.Value = inFromSLIM
                paramList.Add(currentParam)

                ' Task 2178 - Display Delete Date/By in Item screen
                currentParam = New DBParam
                currentParam.Name = "UserID"
                currentParam.Type = DBParamType.Int
                currentParam.Value = giUserID
                paramList.Add(currentParam)

                ' output param
                currentParam = New DBParam
                currentParam.Name = "Succeded"
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                theResultsArray = factory.ExecuteStoredProcedure("EIM_UploadSession", paramList)
            Finally
                ' this is critical for any progress feedback that may depend on it
                inUploadSession.ProgressComplete = True

            End Try

            Return CBool(theResultsArray.Item(0))

        End Function


#End Region

#Region "Overriden CRUD Methods"

        Public Overridable Function CascadeDeleteUploadSession(ByVal businessObject As UploadSession) As Integer

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As New ArrayList

            ' setup parameters for stored proc

            ' UploadSession_ID
            currentParam = New DBParam
            currentParam.Name = "UploadSession_ID"
            currentParam.Type = DBParamType.Int
            currentParam.Value = businessObject.UploadSessionID

            paramList.Add(currentParam)

            ' Get the output value
            currentParam = New DBParam
            currentParam.Name = "DeleteCount"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute Stored Procedure to Create Price Batch Detail records for the price change
            outputList = factory.ExecuteStoredProcedure("EIM_CascadeDeleteUploadSession", paramList)

            Return CInt(outputList(0))

        End Function

#End Region

    End Class

End Namespace
