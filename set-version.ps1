param (
	[string] $filePathAndName,
	[string] $fileVersion,
    [int] $setMode
)

. ".\shared-functions.ps1"

if ( [System.IO.File]::Exists($filePathAndName) -eq $false )
{
	Write-Output $filePathAndName
	throw "File not Found"
}

$versionInfo = get-version $filePathAndName

$version = convert-version $fileVersion

if ($version -eq [System.Version]::new())
{
    $version = set-version -currentVersion $versionInfo.version -setMode $setMode
}

$versionInfo.xmlDocument.Save("$filePathAndName-$currentVersion")

$versionInfo.versionNode.InnerText = $version
$versionInfo.fileVersionNode.InnerText = $version

$versionInfo.xmlDocument.Save($filePathAndName)