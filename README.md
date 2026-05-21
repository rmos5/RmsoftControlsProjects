# RmsoftControls

`RmsoftControls` is a WPF control library and sample application.

## Projects

- `RmsoftControls/` - the reusable control library.
- `RmsoftControlsTestApp/` - sample/test application that demonstrates control usage.

## Getting Started

### Prerequisites

- Windows with .NET Framework developer tooling for WPF.
- Visual Studio (or `msbuild`) capable of building legacy SDK-style `.csproj` projects.

### Build

From repository root:

```bash
msbuild RmsoftControlsProjects.sln /t:Build /p:Configuration=Release
```

### Run sample app

Set `RmsoftControlsTestApp` as startup project in Visual Studio and run.

## Package / release artifacts

- NuGet spec: `RmsoftControls/RmsoftControls.nuspec`
- Build/package helper notes: `readme-nuget.txt`

## Control areas

- Text controls (`RmsoftControls/TextControls`)
- Date/time controls (`RmsoftControls/DateTimeControls`)
- Input capture controls (`RmsoftControls/InputCaptureControls`)
- Dialog controls (`RmsoftControls/Dialogs`)
- Animated controls (`RmsoftControls/AnimatedControls`)
- Behaviors (`RmsoftControls/Behaviors`)
