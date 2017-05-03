Imports System.IO
Public Interface TlogParserInterface

    ' Parser Events that are thrown during procesing.
    ' These can be used to Update the UI and notify user of progress.
    Event Notfiy(ByVal msg As String)
    Event UpdateProgress(ByVal Value As Integer, ByVal max As Integer)
    Event Start()
    Event Finished()
    Event Failure(ByVal ErrMsg As String, ByVal InnerMsg As String)


    Property IsDebug() As Boolean
    ' insert code to parse tlog data from a text file.
    Sub ParseDataFromFile(ByVal DataFile As String)

    ' insert code to parse tlog data from a memory buffer.
    Sub ParseDataFromMemoryStream(ByRef Buffer As Byte())

    ' class should also implement IDisposable. Use this sub to perform any object cleanup that is needed.
    Sub Dispose()
End Interface

