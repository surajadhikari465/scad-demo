Imports Microsoft.VisualBasic
Imports System.Diagnostics

Public Class AD_Access

    Public Shared Sub Main()

    End Sub

#Region "Member Variables"
    Private myUserName As String
    Private myPassWord As String
    Private commonName As String
    Private domain As String = "wfm"
    Private domainUser As String = domain & "\" & myUserName
#End Region

#Region "Constructors"


    Sub New(ByVal name As String, ByVal pass As String)
        myUserName = name
        myPassWord = pass
        commonName = myUserName.Replace(".", " ")
    End Sub

 
#End Region

#Region "Properties"
    Public Property UserName() As String
        Get
            Return myUserName
        End Get
        Set(ByVal value As String)
            myUserName = value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return myPassWord
        End Get
        Set(ByVal value As String)
            myPassWord = value
        End Set
    End Property
#End Region

#Region "Functions"

    Function GetUserInfo() As Collection
        Dim ht As New Collection
        Dim propName As String
        Try
            Dim entry As New DirectoryServices.DirectoryEntry("LDAP://wfm.pvt/DC=wfm,DC=pvt", myUserName, myPassWord)
            Dim mySearcher As New DirectoryServices.DirectorySearcher(entry)
            mySearcher.Filter = ("(&(objectCategory=person)(cn=" & commonName & "))")
            Dim result As DirectoryServices.SearchResult = mySearcher.FindOne()
            entry = Nothing
            mySearcher = Nothing
            For Each propName In result.Properties.PropertyNames
                Dim prop As Object
                For Each prop In result.Properties(propName)
                    ht.Add(prop.ToString)
                Next prop
            Next
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
        Return ht
    End Function

    Function IsValidUser() As Boolean
        Try
            Dim entry As New DirectoryServices.DirectoryEntry("LDAP://wfm.pvt/DC=wfm,DC=pvt", myUserName, myPassWord)
            Dim mySearcher As New DirectoryServices.DirectorySearcher(entry)
            mySearcher.Filter = "(samaccountname=" & myUserName & ")"
            mySearcher.PropertiesToLoad.Add("cn")
            Dim result As DirectoryServices.SearchResult = mySearcher.FindOne()
            entry = Nothing
            mySearcher = Nothing
            If result Is Nothing Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Function

    Function IsValidGroup(ByVal group As String) As Boolean
        Try

            Dim entry As New DirectoryServices.DirectoryEntry("LDAP://wfm.pvt/DC=wfm,DC=pvt", myUserName, myPassWord)
            Dim mySearcher As New DirectoryServices.DirectorySearcher(entry)
            mySearcher.Filter = ("(&(objectCategory=person)(cn=" & commonName & "))")
            Dim result As DirectoryServices.SearchResult = mySearcher.FindOne()
            Dim prop As Object
            entry = Nothing
            mySearcher = Nothing
            For Each prop In result.Properties("memberOf")
                If InStr(prop.ToString, group, CompareMethod.Text) Then
                    Return True
                End If
                'Debug.WriteLine(prop.ToString)
            Next prop
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Function

    Function GetStore() As String
        Try
            Dim entry As New DirectoryServices.DirectoryEntry("LDAP://wfm.pvt/DC=wfm,DC=pvt", myUserName, myPassWord)
            Dim mySearcher As New DirectoryServices.DirectorySearcher(entry)
            mySearcher.Filter = ("(&(objectCategory=person)(cn=" & commonName & "))")
            mySearcher.PropertiesToLoad.Add("extensionattribute2")
            Dim result As DirectoryServices.SearchResult = mySearcher.FindOne()
            Dim prop As Object
            entry = Nothing
            mySearcher = Nothing
            For Each prop In result.Properties("extensionattribute2")
                If Not prop = Nothing Then
                    Return prop.ToString
                Else
                    Return "Empty"
                End If
            Next
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Function
#End Region


End Class
