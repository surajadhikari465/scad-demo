Imports WholeFoods.Utility

Public Enum FormAction
    Create
    Edit
End Enum

''' <summary>
''' This is the base form for the IRMA application.  
''' </summary>
''' <remarks></remarks>
Public Class Form_IRMABase

#Region "Error Message Text"

    Public Shared ERROR_DB As String = ResourcesCommon.GetString("msg_dbError")

#End Region

    ''' <summary>
    ''' Display an error message box using the default application error text.
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub DisplayErrorMessage()
        DisplayErrorMessage(ResourcesCommon.GetString("msg_unexpectedError"))
    End Sub

    ''' <summary>
    ''' Display an error message box using the custom error text.
    ''' </summary>
    ''' <param name="message"></param>
    ''' <remarks></remarks>
    Public Shared Sub DisplayErrorMessage(ByVal message As String)
        MessageBox.Show(message, ResourcesCommon.GetString("msg_titleGenericError"), MessageBoxButtons.OK)
    End Sub

End Class