

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

    ''' <summary>
    ''' This is not a generated class.
    ''' Created By:	David Marine
    ''' Created   :	July 4, 2007
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ItemIdentifierDAO

#Region "Static Singleton Accessor"

        Private Shared _instance As ItemIdentifierDAO = Nothing

        Public Shared ReadOnly Property Instance() As ItemIdentifierDAO
            Get
                If IsNothing(_instance) Then
                    _instance = New ItemIdentifierDAO()
                End If

                Return _instance
            End Get
        End Property

#End Region

#Region "Fields and Properties"

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Runs the populate procedure specified and fills the given comboBox.
        ''' </summary>
        ''' <remarks></remarks>
        Public Function IsScaleIdentifier(ByVal inIdentifier As String) As Boolean

            Dim isValueScaleIdentifier As Boolean = False

            If Not IsNothing(inIdentifier) And _
                Not String.IsNullOrEmpty(inIdentifier) Then

                Dim factory As New DataFactory(DataFactory.ItemCatalog)
                Dim paramList As New ArrayList()
                Dim currentParam As DBParam
                Dim theOutputList As New ArrayList

                ' Identifier
                currentParam = New DBParam
                currentParam.Name = "Identifier"
                currentParam.Type = DBParamType.String
                currentParam.Value = inIdentifier
                paramList.Add(currentParam)

                ' output param
                currentParam = New DBParam
                currentParam.Name = "IsScaleIdentifier"
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                theOutputList = factory.ExecuteStoredProcedure("IsScaleIdentifier", paramList)

                isValueScaleIdentifier = CBool(theOutputList(0))
            End If

            Return isValueScaleIdentifier

        End Function

        Public Function IsValidatedItemInIcon(ByVal itemKey As Integer) As Boolean

            Dim validated As Boolean = False

            If Not IsNothing(itemKey) Then

                Dim factory As New DataFactory(DataFactory.ItemCatalog)
                Dim paramList As New ArrayList()
                Dim currentParam As DBParam
                Dim theOutputList As New ArrayList

                currentParam = New DBParam
                currentParam.Name = "ItemKey"
                currentParam.Type = DBParamType.Int
                currentParam.Value = itemKey
                paramList.Add(currentParam)

                ' output param
                currentParam = New DBParam
                currentParam.Name = "IsValidatedInIcon"
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                theOutputList = factory.ExecuteStoredProcedure("IsValidatedItemInIcon", paramList)

                validated = CBool(theOutputList(0))
            End If

            Return validated

        End Function

        Public Function HasAlignedSubteam(ByVal itemKey As Integer) As Boolean

            Dim aligned As Boolean = False

            If Not IsNothing(itemKey) Then

                Dim factory As New DataFactory(DataFactory.ItemCatalog)
                Dim paramList As New ArrayList()
                Dim currentParam As DBParam
                'Dim theOutputList As New ArrayList

                currentParam = New DBParam
                currentParam.Name = "ItemKey"
                currentParam.Type = DBParamType.Int
                currentParam.Value = itemKey
                paramList.Add(currentParam)

                ' output param
                currentParam = New DBParam
                currentParam.Name = "HasAlignedSubteam"
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                aligned = factory.ExecuteScalar(String.Format("select dbo.fn_HasAlignedSubteam({0})", itemKey))
            End If

            Return aligned

        End Function

        Public Function IsRetailSaleItem(ByVal itemKey As Integer) As Boolean

            Dim retailSale As Boolean = False

            If Not IsNothing(itemKey) Then

                Dim factory As New DataFactory(DataFactory.ItemCatalog)
                Dim paramList As New ArrayList()
                Dim currentParam As DBParam
                Dim theOutputList As New ArrayList

                currentParam = New DBParam
                currentParam.Name = "ItemKey"
                currentParam.Type = DBParamType.Int
                currentParam.Value = itemKey
                paramList.Add(currentParam)

                ' output param
                currentParam = New DBParam
                currentParam.Name = "IsRetailSaleItem"
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                theOutputList = factory.ExecuteStoredProcedure("IsRetailSaleItem", paramList)

                retailSale = CBool(theOutputList(0))
            End If

            Return retailSale

        End Function
#End Region

    End Class

End Namespace
