# Move to /src folder and run
# Changes Tools Version for all csproj files
Try
{
	Get-ChildItem . -Recurse -Filter *.*csproj |ForEach {
    $fileName = $_.Name
    $content = Get-Content $_.FullName
    $content |ForEach {
        $_.Replace('<Project ToolsVersion="12.0"', '<Project ToolsVersion="15.0"')
    } |Set-Content $_.FullName
    Write-Host "Target framework changed for: "$fileName
}
}
Catch{
	Write-Host "An exception occured: `n" $_.Exception.Message
}

