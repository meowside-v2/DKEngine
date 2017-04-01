$files      = Get-ChildItem -Include *.cs,*.xaml,*.c,*.cpp,*.h,*.hpp,*.html,*.css,*php,*.cshtml -Exclude *.g.cs,*.g.i.cs,*.xr,*.pri -Recurse
$strings    = $files | Select-String .
$count      = $strings.Count

Write-Host $count