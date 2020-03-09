Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

' General Information about an assembly is controlled through the following
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

' Review the values of the assembly attributes

<Assembly: AssemblyTitle("Wholefoods.Mobile.IRMA")> 
<Assembly: AssemblyDescription("WFM Mobile IRMA Plugin")> 
<Assembly: AssemblyCompany("Whole Foods Market, Inc.")>
<Assembly: AssemblyProduct("WFM Mobile IRMA Plugin")> 
<Assembly: AssemblyCopyright("Copyright © Whole Foods Market, Inc. 2016")> 
<Assembly: AssemblyTrademark("")>

<Assembly: CLSCompliant(False)> 

<Assembly: ComVisible(False)>

'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: Guid("cd5001e9-f81b-47d7-8b8b-c39ef8dd976a")>

' Version information for an assembly consists of the following four values:
'
'      Major Version
'      Minor Version
'      Build Number
'      Revision
'
' You can specify all the values or you can default the Build and Revision Numbers
' by using the '*' as shown below:
' <Assembly: AssemblyVersion("1.0.*")>

<Assembly: AssemblyVersion("10.8.0.0")> 

'Below attribute is to suppress FxCop warning "CA2232 : Microsoft.Usage : Add STAThreadAttribute to assembly"
' as Device app does not support STA thread.
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2232:MarkWindowsFormsEntryPointsWithStaThread")>
