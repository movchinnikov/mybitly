param([string]$DeployPath)
MSBuild "$PSScriptRoot\MyBitly.sln" /p:Configuration=Release /p:DeployOnBuild=true /p:PublishProfile=front-release /p:PublishUrl="$DeployPath\"