﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.1
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace IRMAFileMonitor
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ServiceModel.ServiceContractAttribute(ConfigurationName:="IRMAFileMonitor.IFileMonitor")>  _
    Public Interface IFileMonitor
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IFileMonitor/GetUnprocessedPushFiles", ReplyAction:="http://tempuri.org/IFileMonitor/GetUnprocessedPushFilesResponse")>  _
        Function GetUnprocessedPushFiles(ByVal region As String) As System.Data.DataTable
    End Interface
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Public Interface IFileMonitorChannel
        Inherits IRMAFileMonitor.IFileMonitor, System.ServiceModel.IClientChannel
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Partial Public Class FileMonitorClient
        Inherits System.ServiceModel.ClientBase(Of IRMAFileMonitor.IFileMonitor)
        Implements IRMAFileMonitor.IFileMonitor
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String)
            MyBase.New(endpointConfigurationName)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As String)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal binding As System.ServiceModel.Channels.Binding, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(binding, remoteAddress)
        End Sub
        
        Public Function GetUnprocessedPushFiles(ByVal region As String) As System.Data.DataTable Implements IRMAFileMonitor.IFileMonitor.GetUnprocessedPushFiles
            Return MyBase.Channel.GetUnprocessedPushFiles(region)
        End Function
    End Class
End Namespace
