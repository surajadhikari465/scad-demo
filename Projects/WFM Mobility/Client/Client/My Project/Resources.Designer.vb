﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.4927
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("WholeFoods.Mobile.Client.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to WFM Mobile.
        '''</summary>
        Friend ReadOnly Property Application_Title() As String
            Get
                Return ResourceManager.GetString("Application_Title", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to \Settings.xml.
        '''</summary>
        Friend ReadOnly Property FileName_Settings() As String
            Get
                Return ResourceManager.GetString("FileName_Settings", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to There are no application plugins available for the {0} Region..
        '''</summary>
        Friend ReadOnly Property LabelText_Error_NoPlugin() As String
            Get
                Return ResourceManager.GetString("LabelText_Error_NoPlugin", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to What do you want to use?.
        '''</summary>
        Friend ReadOnly Property LabelText_Title_MainForm_Default() As String
            Get
                Return ResourceManager.GetString("LabelText_Title_MainForm_Default", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Oops!.
        '''</summary>
        Friend ReadOnly Property LableText_Title_MainForm_NoPlugin() As String
            Get
                Return ResourceManager.GetString("LableText_Title_MainForm_NoPlugin", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Authentication Required.
        '''</summary>
        Friend ReadOnly Property MessageBoxCaption_AuthenticationRequired() As String
            Get
                Return ResourceManager.GetString("MessageBoxCaption_AuthenticationRequired", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Not Found.
        '''</summary>
        Friend ReadOnly Property MessageBoxCaption_MissingPlugin() As String
            Get
                Return ResourceManager.GetString("MessageBoxCaption_MissingPlugin", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to No Plugins.
        '''</summary>
        Friend ReadOnly Property MessageBoxCaption_NoPluginsFound() As String
            Get
                Return ResourceManager.GetString("MessageBoxCaption_NoPluginsFound", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Region Required.
        '''</summary>
        Friend ReadOnly Property MessageBoxCaption_RegionRequired() As String
            Get
                Return ResourceManager.GetString("MessageBoxCaption_RegionRequired", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Oops!.
        '''</summary>
        Friend ReadOnly Property MessageBoxCaption_RegionSelectionBeforeSave() As String
            Get
                Return ResourceManager.GetString("MessageBoxCaption_RegionSelectionBeforeSave", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Error.
        '''</summary>
        Friend ReadOnly Property MessageBoxCaption_UnhandledException() As String
            Get
                Return ResourceManager.GetString("MessageBoxCaption_UnhandledException", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Required Update.
        '''</summary>
        Friend ReadOnly Property MessageBoxCaption_UpdateNeeded() As String
            Get
                Return ResourceManager.GetString("MessageBoxCaption_UpdateNeeded", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to About.
        '''</summary>
        Friend ReadOnly Property MessageBoxCaption_VersionCaption() As String
            Get
                Return ResourceManager.GetString("MessageBoxCaption_VersionCaption", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to {0} requires a valid Whole Foods Login to use..
        '''</summary>
        Friend ReadOnly Property MessageBoxText_AuthenticationRequired() As String
            Get
                Return ResourceManager.GetString("MessageBoxText_AuthenticationRequired", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to No plugins are available for the {0} region..
        '''</summary>
        Friend ReadOnly Property MessageBoxText_NoPluginsFound() As String
            Get
                Return ResourceManager.GetString("MessageBoxText_NoPluginsFound", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Oops! I can&apos;t find that application..
        '''</summary>
        Friend ReadOnly Property MessageBoxText_PluginNotFound() As String
            Get
                Return ResourceManager.GetString("MessageBoxText_PluginNotFound", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to You must set your region before you start using WFM Mobile. Please select one now and tap Save..
        '''</summary>
        Friend ReadOnly Property MessageBoxText_RegionRequired() As String
            Get
                Return ResourceManager.GetString("MessageBoxText_RegionRequired", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Please select a region from the list first..
        '''</summary>
        Friend ReadOnly Property MessageBoxText_RegionSelectionBeforeSave() As String
            Get
                Return ResourceManager.GetString("MessageBoxText_RegionSelectionBeforeSave", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to An error ocurred: {0}.
        '''</summary>
        Friend ReadOnly Property MessageBoxText_UnhandledException() As String
            Get
                Return ResourceManager.GetString("MessageBoxText_UnhandledException", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to An application update is required. Tap OK to begin the update..
        '''</summary>
        Friend ReadOnly Property MessageBoxText_UpdateNeed() As String
            Get
                Return ResourceManager.GetString("MessageBoxText_UpdateNeed", resourceCulture)
            End Get
        End Property
        
        Friend ReadOnly Property Mobile() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("Mobile", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to \AutoUpdate.CAB.
        '''</summary>
        Friend ReadOnly Property Path_AutoUpdateCAB() As String
            Get
                Return ResourceManager.GetString("Path_AutoUpdateCAB", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to \Plugins\.
        '''</summary>
        Friend ReadOnly Property Path_Plugins() As String
            Get
                Return ResourceManager.GetString("Path_Plugins", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to \NETCFv35.Messages.EN.cab.
        '''</summary>
        Friend ReadOnly Property Path_PPCNetCFMsgs() As String
            Get
                Return ResourceManager.GetString("Path_PPCNetCFMsgs", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to \NETCFv35.Messages.EN.wm.cab.
        '''</summary>
        Friend ReadOnly Property Path_WinMobileNetCFMsgs() As String
            Get
                Return ResourceManager.GetString("Path_WinMobileNetCFMsgs", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Contact your Regional IT Support Team for assistance..
        '''</summary>
        Friend ReadOnly Property Text_General_Assistance() As String
            Get
                Return ResourceManager.GetString("Text_General_Assistance", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to {0}, Version {1}.
        '''</summary>
        Friend ReadOnly Property Text_Version() As String
            Get
                Return ResourceManager.GetString("Text_Version", resourceCulture)
            End Get
        End Property
    End Module
End Namespace
