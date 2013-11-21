set PATH=%PATH%;%WINDIR%\Microsoft.Net\Framework\v4.0.30319.

rem go to current folder
cd %~dp0

msbuild build_template_store.proj /property:BuildDir=..\Output /property:NugetFolder=..\NugetOutput /property:DeliverFolder=..\Deliver /property:ReleaseNotes="%~1"
msbuild build_template_store_system_web.proj /property:BuildDir=..\Output /property:NugetFolder=..\NugetOutput /property:DeliverFolder=..\Deliver /property:ReleaseNotes="%~2"
msbuild build_template_store_client.proj /property:BuildDir=..\Output /property:NugetFolder=..\NugetOutput /property:DeliverFolder=..\Deliver /property:ReleaseNotes="%~3"

