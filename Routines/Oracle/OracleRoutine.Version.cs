using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Styx.CommonBot.Routines;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Oracle")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Wulf Uber development co")]
[assembly: AssemblyProduct("Oracle")]
[assembly: AssemblyCopyright("Copyright © Wulf 2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("0eec4efb-571b-42f2-bcd4-c27aef6dcac7")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.315")]
[assembly: AssemblyFileVersion("1.0.0.315")]

namespace Oracle
{
    /// <summary>
	///
	/// Thanks to Singular devs for this method - wulf
	///
    /// Template File:  OracleRoutine.Version.tmpl
    /// Generated File: OracleRoutine.Version.cs
    /// 
    /// These files are the source and output for SubWCRev.exe included
    /// with TortoiseSVN.  The purpose is to provide a real Build #
    /// automatically updated with each release.
    /// 
    /// To make changes, be sure to edit OracleRoutine.Version.tmpl
    /// as the .cs version gets overwritten each build
    /// 
    /// Oracle SVN Information
    /// -------------------------
    /// Revision 315
    /// Date     2013/12/24 07:41:05
    /// Range    304:314
    /// 
    /// </summary>
    public partial class OracleRoutine : CombatRoutine
    {
        // HB Build Process is overwriting AssemblyInfo.cs contents,
        // ... so manage version here instead of reading assembly
        // --------------------------------------------------------

        public static Version GetOracleVersion()
        {
            return new Version("1.0.0.315");
        }
    }
}
