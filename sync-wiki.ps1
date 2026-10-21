<#
.SYNOPSIS
    Synchronizes the documentation from the docs/ directory to the GitHub and GitLab wiki repositories.
.DESCRIPTION
    Pushes Markdown wiki pages to:
      - GitLab: https://www.oldsmarchaos.com/git/cfw34683/honorbuddy-5.4.8-18414.wiki.git
      - GitHub: https://github.com/ripsnortntear/Honorbuddy-MoP-5.4.8-18414.wiki.git
#>

[CmdletBinding()]
param(
    [string]$CommitMessage = "Update wiki documentation"
)

$ErrorActionPreference = "Continue"

$repoRoot = $PSScriptRoot
$docsDir = Join-Path $repoRoot "docs"
$tempWikiDir = Join-Path $env:TEMP "hb_wiki_sync"

$gitlabWikiRemote = "https://oauth2:glpat-2fAhgqd0bsqGFwNW-j1Lem86MQp1OjUH.01.0w0wp70dt@www.oldsmarchaos.com/git/cfw34683/honorbuddy-5.4.8-18414.wiki.git"
$githubWikiRemote = "https://github.com/ripsnortntear/Honorbuddy-MoP-5.4.8-18414.wiki.git"

Write-Host "==> Preparing wiki files from $docsDir..." -ForegroundColor Cyan

if (Test-Path $tempWikiDir) {
    Remove-Item $tempWikiDir -Recurse -Force | Out-Null
}
New-Item -ItemType Directory -Path $tempWikiDir -Force | Out-Null

Set-Location $tempWikiDir
git init -b master | Out-Null
git config user.name "ripsnortntear"
git config user.email "catch@users.noreply.github.com"

git remote add gitlab $gitlabWikiRemote
git remote add origin $githubWikiRemote

# Copy all Markdown docs
Copy-Item (Join-Path $docsDir "*") $tempWikiDir -Force

git add -A
git commit -m $CommitMessage | Out-Null

# Push to GitLab Wiki
Write-Host "==> Pushing to GitLab Wiki..." -ForegroundColor Cyan
git push gitlab master -f
if ($LASTEXITCODE -eq 0) {
    Write-Host " [OK] GitLab wiki updated successfully: https://www.oldsmarchaos.com/git/cfw34683/honorbuddy-5.4.8-18414/-/wikis/home" -ForegroundColor Green
} else {
    Write-Host " [WARN] Failed pushing to GitLab wiki." -ForegroundColor Red
}

# Push to GitHub Wiki
Write-Host "==> Pushing to GitHub Wiki..." -ForegroundColor Cyan
git push origin master -f
if ($LASTEXITCODE -eq 0) {
    Write-Host " [OK] GitHub wiki updated successfully: https://github.com/ripsnortntear/Honorbuddy-MoP-5.4.8-18414/wiki" -ForegroundColor Green
} else {
    Write-Host @"

[NOTICE] GitHub Wiki push failed.
GitHub does not provision the backend wiki git repository until the very first page is created in the web UI.
To enable:
  1. Open your browser and go to: https://github.com/ripsnortntear/Honorbuddy-MoP-5.4.8-18414/wiki
  2. Click 'Create the first page' and save it.
  3. Run .\sync-wiki.ps1 in PowerShell to sync all pages!
"@ -ForegroundColor Yellow
}

Set-Location $repoRoot
