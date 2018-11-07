Imports Microsoft.Build.Utilities
Imports System.IO
Imports System.Xml

Namespace WholeFoods.Build.BuildTask
    ''' <summary>
    ''' The AddFileToBuildOutput defines a custom build task to copy a file from a specified input location to a
    ''' specified output location during a build.  An example of it's use - copying Inventory.MDB (temporary Access database)
    ''' to the build output directory.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class AddFileToBuildOutput
        Inherits Microsoft.Build.Utilities.Task
#Region "Property definitons"
        Private _sourceFilename As String
        Private _outputFilename As String
        Private _outputReadOnly As Boolean = True
#End Region

#Region "Task definitions"
        ''' <summary>
        ''' The Execute method copies a file from a source directory to an output directory.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Execute() As Boolean
            Log().LogMessage("Entering AddFileToBuildOutput.Execute method: source file=" + _sourceFilename)
            Dim returnVal As Boolean = True
            Try
                ' copy the source file to the output file
                File.Copy(_sourceFilename, _outputFilename, True)
                ' set the read-only flag for the file
                If _outputReadOnly Then
                    File.SetAttributes(_outputFilename, FileAttributes.ReadOnly)
                Else
                    File.SetAttributes(_outputFilename, FileAttributes.Normal)
                End If
            Catch ex As Exception
                Log().LogErrorFromException(ex, True)
                returnVal = False
            End Try
            Log().LogMessage("Exiting AddFileToBuildOutput.Execute method: " + returnVal.ToString())
            Return returnVal
        End Function
#End Region

#Region "Property access methods"
        ''' <summary>
        ''' Source filename (including complete path) for the file being moved.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceFilename() As String
            Get
                Return _sourceFilename
            End Get
            Set(ByVal value As String)
                _sourceFilename = value
            End Set
        End Property

        ''' <summary>
        ''' Output filename (including complete path) for the file being moved.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OutputFilename() As String
            Get
                Return _outputFilename
            End Get
            Set(ByVal value As String)
                _outputFilename = value
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
