# publish aot binary
dotnet publish -r osx-arm64 -c Release -o bin

Copy-Item ./bin/YoutubeExplode.Demo.Cli /Users/pmario/Local/tools/YoutubeExplode -Force

# $config = '/Users/pmario/Local/tools/YoutubeExplode.json'
# if (!(Test-Path $config)) {
#     Copy-Item ./bin/appsettings.json $config
# }
