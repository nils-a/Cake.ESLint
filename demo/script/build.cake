#addin nuget:?package=Cake.Npm&version=1.0.0
#r "..\..\src\Cake.ESLint\bin\Debug\netstandard2.0\Cake.ESLint.dll"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("ensure-eslint-tool")
.Does(() => 
{
   var tools = Directory("tools");
   CreateDirectory(tools);
   var settings = new NpmInstallSettings 
   {
      WorkingDirectory = tools
   };
   settings.Packages.Add("eslint");

   NpmInstall(settings);
});

Task("manual-installation")
.IsDependentOn("ensure-eslint-tool")
.Does(() => {
   var settings = new ESLintSettings {
      ContinueOnErrors = true
   };
   settings.Directories.Add("src1");
   ESLint(settings);
});

Task("local-project")
.Does(() => {
   NpmInstall(new NpmInstallSettings {
      WorkingDirectory = "src2"
   });

   var settings = new ESLintSettings {
      WorkingDirectory = "src2",
      Output = "../output.json",
      OutputFormat = ESLintOutputFormat.Json,
      ContinueOnErrors = true
   };
   settings.Directories.Add(".");
   ESLint(settings);
});

Task("Default")
 .IsDependentOn("local-project");
 //.IsDependentOn("manual-installation")

RunTarget(target);