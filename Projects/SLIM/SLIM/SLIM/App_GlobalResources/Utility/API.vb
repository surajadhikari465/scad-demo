Option Strict Off
Option Explicit On

Imports SLIM.WholeFoods.Utility

Module API

    Public Function GetADUserInfo(ByVal userName As String, ByVal LDAPKey As String) As String

        Dim _writeInfo As Boolean = False
        Dim strPath As String
        Dim strSearchFilter As String = "(&(objectClass=user)(samaccountname={0}))"

        Dim objDirEntry As System.DirectoryServices.DirectoryEntry = Nothing
        Dim objDirSearcher As System.DirectoryServices.DirectorySearcher = Nothing
        Dim objCollSearchResult As System.DirectoryServices.SearchResultCollection = Nothing
        Dim objlSearchResult As System.DirectoryServices.SearchResult = Nothing
        Dim objCollResultProperty As System.DirectoryServices.ResultPropertyCollection = Nothing
        Dim objCollResultPropertyValue As System.DirectoryServices.ResultPropertyValueCollection = Nothing

        GetADUserInfo = String.Empty

        Try

            Try
                strPath = String.Format("LDAP://{0}", HttpContext.Current.Application.Get("LDAP_Server"))
                If strPath = "LDAP://" Then Throw New Exception()
            Catch exLDAP As Exception
                Throw New Exception("LDAP_Server has not been configured")
            End Try



            ' setup the search in AD for the user's details
            objDirEntry = New System.DirectoryServices.DirectoryEntry(strPath)
            objDirSearcher = New System.DirectoryServices.DirectorySearcher(objDirEntry)
            objDirSearcher.Filter = (String.Format(strSearchFilter, userName))

            ' search AD for the user account
            objCollSearchResult = objDirSearcher.FindAll()

            Select Case objCollSearchResult.Count

                Case 1

                    ' pull the AD properties that we can and load the text boxes
                    objlSearchResult = objCollSearchResult.Item(0)
                    objCollResultProperty = objlSearchResult.Properties

                    objCollResultPropertyValue = objCollResultProperty.Item(LDAPKey)
                    If objCollResultPropertyValue.Count > 0 Then
                        GetADUserInfo = objCollResultPropertyValue.Item(0).ToString
                    End If

                Case Else

                    GetADUserInfo = String.Empty

                    Exit Try

            End Select

            ' dispose of all the AD objects
            objDirEntry.Dispose()
            objDirSearcher.Dispose()
            objCollSearchResult.Dispose()
            objlSearchResult = Nothing
            objCollResultProperty = Nothing
            objCollResultPropertyValue = Nothing

            Return GetADUserInfo

        Catch ex As System.Exception


            Dim ex1 As New Exception(String.Format("GetADUserInfo() lookup failed. [ {0} ]", ex.Message), ex)
            Throw ex1


        End Try

    End Function

End Module