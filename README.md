# MCFabricDependencyViewer

1. This program is source only so you need to compile it yourself, I advise to use [VS Code](https://code.visualstudio.com/) or [Visual Studio Community Edition](https://visualstudio.microsoft.com/vs/community/). Also there is [VS Code Web version](https://vscode.dev/).

   The project was initially developped in Visual Studio Community Edition so I don't think the .sln file will work with other than Visual Studio Community Edition but I have no idea.

2. The program won't work by default, you need to go into `Program.cs` and manually set your path where your .jar mods are.

   Right now it's here `const string path = @"";`. You can paste your path directly as the `@` before the `"` means the string doesn't need escape codes and can be read literally, except for `"` of course, so it can't be read literally

3. If the program finds dependencies that are named : `fabric`, `fabricloader`, `minecraft` or `java`, it will ignore them completely. You can configure that in the `Settings.cs` file here `public static readonly List<string> blacklisted_mods`
