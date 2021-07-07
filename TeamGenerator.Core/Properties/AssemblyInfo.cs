using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("aa8e144c-a668-48d2-91e8-7ed620d806d3")]

//Internal classes are accessible in Test assembly.
[assembly: InternalsVisibleTo("TeamGenerator.Core.Tests")]
