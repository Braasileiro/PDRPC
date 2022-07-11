using PDRPC.Core;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

// Info
[assembly: AssemblyTitle(BuildInfo.Description)]
[assembly: AssemblyDescription(BuildInfo.Description)]
[assembly: AssemblyCompany(BuildInfo.Company)]
[assembly: AssemblyProduct(BuildInfo.Name)]
[assembly: AssemblyCopyright(BuildInfo.Author)]
[assembly: AssemblyTrademark(BuildInfo.Company)]

// GUID
[assembly: Guid("55400785-2ebd-4542-9dc5-550e7827d0a3")]

// Version
[assembly: AssemblyVersion(BuildInfo.Version)]
[assembly: AssemblyFileVersion(BuildInfo.Version)]

// Suppress
[assembly: SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "namespaceanddescendants", Target = "~N:PDRPC.Core.Models")]
