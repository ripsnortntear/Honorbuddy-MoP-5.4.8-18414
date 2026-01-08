@echo off
for /f "delims=" %%a in ('dir *.xml /b /a-d') do (
cscript replace.vbs ".\%%a"
echo %%a
)