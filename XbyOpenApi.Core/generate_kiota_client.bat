@REM generate kiota client
dotnet kiota generate -l CSharp -c XClient -n XbyOpenApi.Core.Client -d https://api.twitter.com/2/openapi.json -o ./Client --exclude-backward-compatible

@REM apply whitespace formatting according to .editorconfig afterwards
dotnet format whitespace --include-generated

@pause