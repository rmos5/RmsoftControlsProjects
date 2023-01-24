/* Sample nuget CLI */
cd RmsoftControls
nuget update -Self
nuget pack -Build -Symbols -Properties Configuration=Release
nuget push RmsoftControls.2.9.0.nupkg -Source PhdTfs01
