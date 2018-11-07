Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Collections.ObjectModel

Namespace WholeFoods.Utility.DataAccess

    Public Class DBParamList

        Inherits KeyedCollection(Of String, DBParam)

#Region "Constructors"

        ' The parameterless constructor of the base class creates a 
        ' KeyedCollection with an internal dictionary.
        '
        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Overrides"

        ' This is the only method that absolutely must be overridden,
        ' because without it the KeyedCollection cannot extract the
        ' keys from the items. The input parameter type is the 
        ' second generic type argument, in this case OrderItem, and 
        ' the return value type is the first generic type argument,
        ' in this case Integer.
        '
        Protected Overrides Function GetKeyForItem(ByVal item As DBParam) As String

            ' the key is the parameter's name
            Return item.Name
        End Function

#End Region

#Region "Custom methods"

        ''' <summary>
        ''' Clear all parameter values in the collection; parameters remain with specified name and type.
        ''' Sets DBParam.Value = Nothing for all items in DBParamList
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ClearValues()

            For Each param As DBParam In MyBase.Items
                param.Value = Nothing
            Next

        End Sub

        ''' <summary>
        ''' Copy all parameter values into an ArrayList collection for use with DataFactory parameters
        ''' </summary>
        ''' <remarks></remarks>
        Public Function CopyToArrayList() As System.Collections.ArrayList

            Return New System.Collections.ArrayList(CType(Me.MemberwiseClone, System.Collections.ICollection))

        End Function

#End Region

    End Class

End Namespace
