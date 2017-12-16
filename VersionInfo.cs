// Visual Studio: Using external files (without copying them)
// https://stackoverflow.com/questions/19862185/visual-studio-using-external-files-without-copying-them
// file: VersionInfo.cs. Add this file as link to Properties folder, along with local AssemblyInfo.cs

using System.Reflection;

[assembly: AssemblyVersion             ("0.0.0"        )]
[assembly: AssemblyFileVersion         ("0.0.0.1"      )]

[assembly: AssemblyInformationalVersion("0.0.0-alpha"  )]  // Semantic Version 2.0 here: -rc, -beta, -alpha
// https://docs.microsoft.com/en-us/nuget/create-packages/prerelease-packages
// NUGET: https://blog.nuget.org/20140924/supporting-semver-2.0.0.html
// http://semver.org/

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

