Imports System.ServiceProcess

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ReprintShelfTagsService
    Inherits System.ServiceProcess.ServiceBase

    'UserService overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    ' The main entry point for the process
    <MTAThread()> _
    <System.Diagnostics.DebuggerNonUserCode()> _
    Shared Sub Main(ByVal args() As String)

        If args.Length = 0 Then
            ' no command line arguments were specified; default to a Windows Service application
            Dim ServicesToRun() As System.ServiceProcess.ServiceBase

            ' More than one NT Service may run within the same process. To add
            ' another service to this process, change the following line to
            ' create a second service object. For example,
            '
            '   ServicesToRun = New System.ServiceProcess.ServiceBase () {New Service1, New MySecondUserService}
            '
            ServicesToRun = New System.ServiceProcess.ServiceBase() {New ReprintShelfTagsService}

            System.ServiceProcess.ServiceBase.Run(ServicesToRun)
        Else
            ' command line arguments were specified; run as a Windows Form application
            System.Windows.Forms.Application.Run(New ReprintShelfTagsForm())
        End If

    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    ' NOTE: The following procedure is required by the Component Designer
    ' It can be modified using the Component Designer.  
    ' Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        '
        'ReprintShelfTagsService
        '
        Me.ServiceName = "ReprintShelfTagsService"

    End Sub

End Class
