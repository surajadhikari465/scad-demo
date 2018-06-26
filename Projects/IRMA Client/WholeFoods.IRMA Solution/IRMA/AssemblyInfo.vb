Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

' General Information about an assembly is controlled through the following
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.


' TODO: Review the values of the assembly attributes


<Assembly: AssemblyTitle("IRMA")> 
<Assembly: AssemblyDescription("Inventory Replenishment Merchandizing Application")> 
<Assembly: AssemblyCompany("Whole Foods Market")> 
<Assembly: AssemblyProduct("WFM IRMA Client")> 
<Assembly: AssemblyCopyright("© Whole Foods Market, Inc. 2006-2013")> 
<Assembly: AssemblyTrademark("")>
<Assembly: AssemblyCulture("")>

' Version information for an assembly consists of the following four values:

'	Major version
'	Minor Version
'	Build Number
'	Revision

' You can specify all the values or you can default the Build and Revision Numbers
' by using the '*' as shown below:

<Assembly: AssemblyVersion("10.2.0.0")>


<Assembly: AssemblyFileVersionAttribute("10.2.0.0")>

' Specify that the logger is configured using the app.config file 
<Assembly: log4net.Config.XmlConfigurator()> 
