
#
# Copyright © 2018. TIBCO Software Inc.
# This file is subject to the license terms contained
# in the license file that is distributed with this file.
#

$env:VERSION="1.0"

$now = Get-Date
$today = (Get-Date -Year $now.Year -Month $now.Month -Day $now.Day -Hour 0 -Minute 0 -Second 0)
$then = (Get-Date -Year 2017 -Month 01 -Day 01)
$part3=100*(($now.Month - $then.Month) + 12 * ($now.Year - $then.Year))+$now.Day
$part4=[int]((($now-$today).totalSeconds)/10)
$version=$env:VERSION + '.' + $part3 + '.' + $part4

$env:MSBUILDPATH="C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\"
$env:SPOTFIREVERSION="10.0.0"
$env:SPOTFIREPACKAGEBUILDERCONSOLEPATH="C:\Users\lpautet\TIBCO Spotfire SDK\$env:SPOTFIREVERSION\SDK\Package Builder\"
$env:BUILDHOME="C:\Users\lpautet\source\repos\LMIDataSource"

$env:PATH="$env:SPOTFIREPACKAGEBUILDERCONSOLEPATH;$env:MSBUILDPATH;$env:PATH"
Write-Host "Unblocking files"
Unblock-File  -Path "$env:SPOTFIREPACKAGEBUILDERCONSOLEPATH\*.exe"
Unblock-File  -Path "$env:SPOTFIREPACKAGEBUILDERCONSOLEPATH\bin\*.dll"
#Write-Host $env:PATH
Write-Host "Building MS VS solution"
msbuild "$env:BUILDHOME\LmiDataSource.sln" /t:Clean,Build 
Write-Host "All DLL builds completed." 
Write-Host "Building Spotfire Desktop ZIP files"
Compress-Archive -LiteralPath "$env:BUILDHOME\LMIDataSource\bin\debug\LMIDataSource.dll", "$env:BUILDHOME\LMIDataSource\bin\debug\module.xml" -DestinationPath "$env:BUILDHOME\target\LMIDataSource_$version.zip"
Compress-Archive -LiteralPath "$env:BUILDHOME\LMIDataSourceForms\bin\debug\LMIDataSourceForms.dll", "$env:BUILDHOME\LMIDataSourceForms\bin\debug\module.xml" -DestinationPath "$env:BUILDHOME\target\LMIDataSourceForms_$version.zip"
Write-Host "Building release $version package files"
Spotfire.Dxp.PackageBuilder-Console.exe /packageversion:"$version" /pkdesc:"$env:BUILDHOME\LMIDataSource\package.pkdesc" /basefolder:"$env:BUILDHOME\LMIDataSource"  /target:"$env:BUILDHOME\target\LmiDataSource.spk"
Spotfire.Dxp.PackageBuilder-Console.exe /packageversion:"$version" /pkdesc:"$env:BUILDHOME\LMIDataSourceForms\package.pkdesc" /basefolder:"$env:BUILDHOME\LMIDataSourceForms"  /target:"$env:BUILDHOME\target\LmiDataSourceForms.spk" /refpath:"$env:BUILDHOME\LMIDataSourceForms\bin\debug"
Spotfire.Dxp.PackageBuilder-Console.exe /packageversion:"$version" /pkdesc:"$env:BUILDHOME\LMIDataSourceWeb\package.pkdesc" /basefolder:"$env:BUILDHOME\LMIDataSourceWeb"  /target:"$env:BUILDHOME\target\LmiDataSourceWeb.spk" /refpath:"$env:BUILDHOME\LMIDataSourceWeb\bin\debug"
Write-Host "Building release $version distribution file"
Spotfire.Dxp.PackageBuilder-Console.exe /targettype:Distribution /basefolder:"$env:BUILDHOME" /distdesc:"$env:BUILDHOME\bundle.xml"  /target:"$env:BUILDHOME\target\LmiDataSource-$version-sf$env:SPOTFIREVERSION.sdn"




