# Move to /src folder and run
# Changes Target Framework Version for all csproj files
Try
{
	Get-ChildItem . -Recurse -Filter *.*proj |ForEach {
    $fileName = $_.Name
    $content = Get-Content $_.FullName
    $content |ForEach {
        $_.Replace("<TargetFrameworkVersion>v4.6.2</TargetFrameworkVersion>", "<TargetFrameworkVersion>v4.7.1</TargetFrameworkVersion>")
    } |Set-Content $_.FullName
    Write-Host "Target framework changed for: "$fileName
}
}
Catch{
	Write-Host "An exception occured: `n" $_.Exception.Message
}

