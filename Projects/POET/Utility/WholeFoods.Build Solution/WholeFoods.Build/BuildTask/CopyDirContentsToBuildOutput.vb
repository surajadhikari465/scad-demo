Imports Microsoft.Build.Utilities
Imports System.IO
Imports System.Xml

Namespace WholeFoods.Build.BuildTask
    ''' <summary>
    ''' The CopyDirContentsToBuildOutput defines a custom build task to copy all the files from a specified input directory to a
    ''' specified output directory during a build.  An example of it's use - copying Crystal Report *.rpt files
    ''' to the reports directory for the build.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CopyDirContentsToBuildOutput
        Inherits Microsoft.Build.Utilities.Task
#Region "Property definitons"
        Private _sourceDir As String
        Private _outputDir As String
        Private _searchPattern As String
        Private _outputReadOnly As Boolean = True
#End Region

#Region "Task definitions"
        ''' <summary>
        ''' The Execute method copies a file from a source directory to an output directory.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Execute() As Boolean
            Log().LogMessage("Entering CopyDirContentsToBuildOutput.Execute method: source dir=" + _sourceDir)
            Dim returnVal As Boolean = True
            Try
                ' create a clean version of the output directory
                If Directory.Exists(_outputDir) Then
                    ' delete the contents of the directory
                    Directory.Delete(_outputDir, True)
                End If
                Directory.CreateDirectory(_outputDir)

                ' copy all the files from the source directory to the output directory
                Dim fileList As IEnumerator
                If (_searchPattern IsNot Nothing) AndAlso (_searchPattern IsNot "") Then
                    fileList = Directory.GetFiles(_sourceDir, _searchPattern).GetEnumerator()
                Else
                    fileList = Directory.GetFiles(_sourceDir).GetEnumerator()
                End If

                Dim currentFile As FileInfo
                Dim sourceFile As String
                Dim outputFile As String
                While fileList.MoveNext
                    currentFile = New FileInfo(fileList.Current.ToString)

                    ' copy the source file to the output file
                    sourceFile = _sourceDir & "\" & currentFile.Name
                    outputFile = _outputDir & "\" & currentFile.Name
                    File.Copy(sourceFile, outputFile, True)

                    ' set the read-only flag for the file
                    If _outputReadOnly Then
                        File.SetAttributes(outputFile, FileAttributes.ReadOnly)
                    Else
                        File.SetAttributes(outputFile, FileAttributes.Normal)
                    End If
                End While

            Catch ex As Exception
                Log().LogErrorFromException(ex, True)
                returnVal = False
            End Try
            Log().LogMessage("Exiting CopyDirContentsToBuildOutput.Execute method: " + returnVal.ToString())
            Return returnVal
        End Function
#End Region

#Region "Property access methods"
        ''' <summary>
        ''' Source directory (including complete path) for the files being moved.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceDir() As String
            Get
                Return _sourceDir
            End Get
            Set(ByVal value As String)
                _sourceDir = value
            End Set
        End Property

        ''' <summary>
        ''' Output directory (including complete path) for the files being moved.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OutputDir() As String
            Get
                Return _outputDir
            End Get
            Set(ByVal value As String)
                _outputDir = value
            End Set
        End Property

        ''' <summary>
        ''' Search pattern for files in the source directory.  Only moves files that match the pattern.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Optional parameter</remarks>
        Public Property SearchPattern() As String
            Get
                Return _searchPattern
            End Get
            Set(ByVal value As String)
                _searchPattern = value
            End Set
        End Property

        ''' <summary>
        ''' Flag that indicates if the output file should be read only.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Optional parameter - defaults to True</remarks>
        Public Property OutputReadOnly() As Boolean
            Get
                Return _outputReadOnly
            End Get
            Set(ByVal value As Boolean)
                _outputReadOnly = value
            End Set
        End Property
#End Region

    End Class
End Namespace
