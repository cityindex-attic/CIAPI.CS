For a development workstation:

   * Install via Visual Studio Extension manager - http://docs.nuget.org/docs/start-here/installing-nuget
   * Also complete instructions as for build machine (to get command line version)


For a build machine:

   * Install NuGet command line from : http://nuget.codeplex.com/releases/view/58939 
      - download and install to C:\Program Files\NuGet (or C:\Program Files (x86)\NuGet for x64 machines)
      - run once - it will autoupdate itself
   * Make sure the NuGet API key is registered on the machine by running "nuget setApiKey Your-API-Key" once
   * The "PublishToNugetOrg" target of the build publishes the latest package using "nuget push CIAPI.CS.{version}.nupkg"


