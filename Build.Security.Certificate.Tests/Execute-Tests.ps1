function Execute-WithMsbuild
{
  Param(
    [String]
    [ValidateScript({Test-Path $_ -PathType 'Leaf'})] 
    $project,

    [String]
    $targets
  )

  $msbuild = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"

  & $msbuild $project /t:$targets  
}

# Act
Execute-WithMsbuild -Project .\Targets\SignFile.targets -Targets "SignFromCurrentUserMy;SignFromCurrentUserTrustedPublisher;SignFromLocalMachineMy;SignFromLocalMachineTrustedPublisher"

# Assert
$signatures = Get-ChildItem .\Output | Get-AuthenticodeSignature

foreach ($file in $signatures)
{
  $status = $file.Status
  $statusFile = $file.Path 
  $status | Out-File "$statusFile.$status" -Encoding ASCII

  if ($file.Status -ne "Valid")
  {
    Write-Host -ForegroundColor Red Invalid Signature on (Split-Path -Leaf $file.Path)
  } 
  else
  {
    Write-Host -ForegroundColor Green Valid Signature on (Split-Path -Leaf $file.Path)
  }
}
