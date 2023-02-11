$SET_MODE_MAJOR = 1
$SET_MODE_MINOR = 2
$SET_MODE_BUILD = 3
$SET_MODE_REVISION = 4
$SET_MODE_USE_TIMESTAMP = -1

function get-timestamp-version (
    [System.Version] $currentVersion,
    [System.DateTime] $timeStamp,
    [int] $revision = 0
) {
    $hasDateChanged = $false
    $major = $currentVersion.Major
    if ( $timeStamp.Year -gt $major )
    {
        $hasDateChanged = $true
        $major = $timeStamp.Year
    }

    $minor = $currentVersion.Minor
    if ( $timeStamp.Month -gt $minor )
    {
        $hasDateChanged = $true
        $minor = $timeStamp.Month
    }

    $build = $currentVersion.Build
    if ( $timeStamp.Day -gt $build )
    {
        $hasDateChanged = $true
        $build = $timeStamp.Day
    }
    if ( $hasDateChanged -eq $true ) {
        $revision = 0
    }

    if ( -not $hasDateChanged -eq $true -and $revision -le $currentVersion.Revision) {
        $revision = $currentVersion.Revision + 1
    }

    return [System.Version]::new($major,$minor,$build, $revision)    
}

class VersionInfo {
    [System.Xml.XmlDocument] $xmlDocument
    [System.Xml.XmlNode] $versionNode
    [System.Xml.XmlNode] $fileVersionNode
    [System.Version]$version
    [System.Version]$fileVersion
}

function set-version(
    [System.Version] $currentVersion,
    [int] $setMode
) 
{
    switch($setMode) {
        $SET_MODE_USE_TIMESTAMP
        {
            $now = [System.DateTime]::UtcNow
            return get-timestamp-version -currentVersion $currentVersion -timeStamp $now
        }
        $SET_MODE_MAJOR 
        { 
            return [System.Version]::new(
                $currentVersion.Major + 1, 
                0, 
                0, 
                0 
             )
        }
        $SET_MODE_MINOR 
        { 
            return [System.Version]::new(
                $currentVersion.Major, 
                $currentVersion.Minor + 1, 
                0, 
                0
            ) 
        }
        $SET_MODE_BUILD 
        { 
            return [System.Version]::new(
                $currentVersion.Major, 
                $currentVersion.Minor, 
                $currentVersion.Build + 1, 
                0
            ) 
        }
        $SET_MODE_REVISION 
        { 
            return [System.Version]::new(
                $currentVersion.Major, 
                $currentVersion.Minor, 
                $currentVersion.Build, 
                $currentVersion.Revision + 1
            )
        }
    }
}

function get-xml-document (
    [string] $filePathAndName
) 
{
    $xmlDocument = [System.Xml.XmlDocument]::new()

    $xmlDocument.Load($filePathAndName)

    return $xmlDocument
}

function get-version (
    [string] $filePathAndName
) 
{
    if ( [System.IO.File]::Exists($filePathAndName) -eq $false )
    {
	    throw 'File not Found'
    }

    $xmlDocument = get-xml-document $filePathAndName
    
    $versionInfo = New-Object VersionInfo
    
    $versionNode = $xmlDocument.SelectSingleNode("//Project/PropertyGroup/Version")
    $fileVersionNode = $xmlDocument.SelectSingleNode("//Project/PropertyGroup/FileVersion")
    
    $versionInfo.xmlDocument = $xmlDocument
    $versionInfo.versionNode = $versionNode
    $versionInfo.fileVersionNode = $fileVersionNode
    $versionInfo.version = $versionNode.InnerText
    $versionInfo.fileVersion = $fileVersionNode.InnerText

    return $versionInfo
}

function convert-version (
    [string] $fileVersion
)
{
    $version = [System.Version]::new()

    if ( -not [System.String]::IsNullOrWhiteSpace($fileVersion) -and [System.Version]::TryParse($fileVersion, [ref]$version) -eq $false)
    {
        throw "Invalid version $fileVersion"
    }

    return $version
}