Imports System
Imports System.Drawing


' <summary>
' Provides access to resources in this assembly.
' </summary>
Public NotInheritable Class ResourcesPricing

    '
    ' Class fields
    '
    Private Shared resourceServices As WholeFoods.Utility.ResourceServices = New WholeFoods.Utility.ResourceServices("Pricing")

    '
    ' Ctors
    '
    ' <summary>
    ' Default ctor: private to prevent instantiation.
    ' </summary>
    Private Sub New()
        MyBase.New()

    End Sub

    '
    ' Public class methods
    '
    ' <summary>
    ' Returns the value for the specified key from the resources associated with this assembly.
    ' </summary>
    ' <param name="key"></param>
    ' <returns></returns>
    Public Shared Function GetString(ByVal resourceKey As String) As String
        Return resourceServices.GetString(resourceKey)
    End Function

    ' <summary>
    ' Returns the icon for the specified resourceKey.
    ' </summary>
    ' <param name="resourceKey"></param>
    ' <returns></returns>
    Public Shared Function GetIcon(ByVal resourceKey As String) As Icon
        Return resourceServices.GetIcon(resourceKey)
    End Function

    ' <summary>
    ' Returns the image for the specified key from the resources associated with this assembly.
    ' </summary>
    ' <param name="key"></param>
    ' <returns></returns>
    Public Shared Function GetImage(ByVal resourceKey As String) As Image
        Return resourceServices.GetImage(resourceKey)
    End Function

    ' <summary>
    ' Latest date value compatible with the MS-SQL smalldatetime data type
    ' </summary>
    Public Const MaxSmalldatetimeDate As Date = #6/5/2079#
End Class
