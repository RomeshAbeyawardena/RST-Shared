param (
    [string] $outputPath,
    [string] $fileVersion,
    [int] $setMode,
    [switch] $tagBuild = $false
)

. ".\shared-functions.ps1"

$currentPath = Get-Location
$propsFile = "$currentPath/Directory.build.props"
$oldPath = "$outputPath/old"

if ( [System.IO.Directory]::Exists($oldPath) -eq $true )
{
    Remove-item -Path $oldPath -Recurse -Verbose
}

git pull

dotnet build -o $oldPath

if(-not $LastExitCode -eq 0 ) 
{
    throw 'Build was not successful!'
}

.\set-version.ps1 -filePathAndName $propsFile -fileVersion $fileVersion -setMode $setMode 

$versionInfo = get-version $propsFile

$version = $versionInfo.version

dotnet pack -o "$outputPath/$version"

if(-not $LastExitCode -eq 0 ) 
{
    throw 'Pack was not successful!'
}

if ( $tagBuild -eq $true )
{
    $dateNow = [System.DateTime]::UtcNow
    git commit  -a -m "Updated version to $version"
    git tag -a $version -m "Automated build generated on $dateNow"
    git push
}
