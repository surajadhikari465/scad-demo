using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("WholeFoods.Mobile.Interop.Scanner")]
[assembly: AssemblyDescription("Provides handheld scanner interoperability for all supported WFM handheld devices.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Whole Foods Market, Inc.")]
[assembly: AssemblyProduct("WholeFoods.Mobile.Interop.Scanner")]
[assembly: AssemblyCopyright("Copyright © Whole Foods Market, Inc. 2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("33ae784d-6709-45b5-a8b4-b192c1a43ec0")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("1.0.0.0")]

// Below attribute is to suppress FxCop warning "CA2232 : Microsoft.Usage : Add STAThreadAttribute to assembly"
// as Device app does not support STA thread.
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2232:MarkWindowsFormsEntryPointsWithStaThread")]
