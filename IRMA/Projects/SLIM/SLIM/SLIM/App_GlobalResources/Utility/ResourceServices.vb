Imports System.Drawing
Imports System.Reflection
Imports System.Resources

Namespace WholeFoods.Utility

    ''' <summary>
    ''' This class consolidates code that is common to all the resource classes in the application.
    ''' </summary>
    Public NotInheritable Class ResourceServices

        '''
        ''' Instance fields
        '''
        Private resourceManager As ResourceManager

        '''
        ''' Ctors
        '''
        ''' <summary>
        ''' Ctor: Creates a ResourceManager for the specified resource base name. This method assumes that 
        ''' the resources are located in the Executing assembly.
        ''' 
        ''' The error message is not localized because it should only be used as a debugging aid during 
        ''' development.
        ''' </summary>
        ''' <param name="resourceBaseName"></param>
        Public Sub New(ByVal resourceBaseName As String)
            MyBase.New()
            Try
                resourceManager = New ResourceManager(resourceBaseName, Assembly.GetCallingAssembly)
            Catch ex As Exception
                Throw New Exception("An error occurred creating a ResourceManager.", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Gets the resource value for the specified resource key. If the resource key isn't found, the
        ''' resource key itself is returned (this is intended to aid in identifying resources that haven't
        ''' been created yet).
        ''' 
        ''' The error message is not localized because it should only be used as a debugging aid during 
        ''' development.
        ''' </summary>
        ''' <param name="resourceKey"></param>
        ''' <returns>The resource value or an empty string if the resource value could not be found</returns>
        Public Function GetString(ByVal resourceKey As String) As String
            Dim resourceValue As String = String.Empty
            Try
                resourceValue = resourceManager.GetString(resourceKey)
            Catch ex As Exception
                Throw New Exception("An error occurred retrieving a string resource.", ex)
            End Try

            If resourceValue = Nothing Then
                Return resourceKey
            Else
                Return resourceValue
            End If
        End Function

        ''' <summary>
        ''' Gets the icon for the specified resource key.
        ''' 
        ''' The error message is not localized because it should only be used as a debugging aid during 
        ''' development.
        ''' </summary>
        ''' <param name="resourceKey"></param>
        ''' <returns>The resource value or a null if the icon could not be found</returns>
        Public Function GetIcon(ByVal resourceKey As String) As Icon
            Dim resourceValue As Object = Nothing
            Try
                resourceValue = resourceManager.GetObject(resourceKey)
            Catch ex As Exception
                Throw New Exception("An error occurred retrieving an icon resource.", ex)
            End Try
            Return CType(resourceValue, Icon)
        End Function

        ''' <summary>
        ''' Gets the image for the specified resource key.
        ''' 
        ''' The error message is not localized because it should only be used as a debugging aid during 
        ''' development.
        ''' </summary>
        ''' <param name="resourceKey"></param>
        ''' <returns>The resource value or a null if the icon could not be found</returns>
        Public Function GetImage(ByVal resourceKey As String) As Image
            Dim resourceValue As Object = Nothing
            Try
                resourceValue = resourceManager.GetObject(resourceKey)
            Catch ex As Exception
                Throw New Exception("An error occurred retrieving an image resource.", ex)
            End Try
            Return CType(resourceValue, Image)
        End Function

        ''' <summary>
        ''' Gets a generic object related to the specified resource key. Use for resource items
        ''' such as images.
        ''' 
        ''' The error message is not localized because it should only be used as a debugging aid during 
        ''' development.
        ''' </summary>
        ''' <param name="resourceKey"></param>
        ''' <returns>The resource value or a null if the icon could not be found</returns>
        Public Function GetObject(ByVal resourceKey As String) As Object
            Dim resourceValue As Object = Nothing
            Try
                resourceValue = resourceManager.GetObject(resourceKey)
            Catch ex As Exception
                Throw New Exception("An error occurred retrieving an object resource.", ex)
            End Try
            Return resourceValue
        End Function
    End Class

End Namespace