<#
.SYNOPSIS
    Synchronizes the local Honorbuddy repository with both GitHub and GitLab remotes.
.DESCRIPTION
    Fetches and pushes code to:
      - GitLab: https://www.oldsmarchaos.com/git/cfw34683/honorbuddy-5.4.8-18414.git (oldsmarchaos)
      - GitHub: https://github.com/ripsnortntear/Honorbuddy-MoP-5.4.8-18414 (origin)
.PARAMETER CommitMessage
    Optional commit message. If specified and there are uncommitted changes, stages and commits them before pushing.
.PARAMETER PullFirst
    If specified, pulls and integrates remote changes before pushing.
#>

[CmdletBinding()]
param(
    [string]$CommitMessage = "",
    [switch]$PullFirst = $true
)

$ErrorActionPreference = "Continue"

$repoRoot = $PSScriptRoot
Set-Location $repoRoot

$currentBranch = (git branch --show-current).Trim()
if ([string]::IsNullOrWhiteSpace($currentBranch)) {
    $currentBranch = "main"
}

Write-Host "==> Starting repository sync on branch [$currentBranch]..." -ForegroundColor Cyan

# 1. Commit uncommitted changes if requested
if (![string]::IsNullOrWhiteSpace($CommitMessage)) {
    $status = git status --porcelain
    if (![string]::IsNullOrWhiteSpace($status)) {
        Write-Host "==> Staging and committing local changes: '$CommitMessage'..." -ForegroundColor Cyan
        git add -A
        git commit -m $CommitMessage
        if ($LASTEXITCODE -eq 0) {
            Write-Host " [OK] Changes committed." -ForegroundColor Green
        } else {
            Write-Host " [WARN] Commit failed or nothing to commit." -ForegroundColor Yellow
        }
    }
}

# 2. Pull / Fetch from remotes if PullFirst is set
if ($PullFirst) {
    Write-Host "==> Fetching latest changes from remotes..." -ForegroundColor Cyan

    # Fetch and integrate origin
    git fetch origin $currentBranch 2>$null
    if ($LASTEXITCODE -eq 0) {
        $behindOrigin = git rev-list --count "$currentBranch..origin/$currentBranch" 2>$null
        if ($behindOrigin -and [int]$behindOrigin -gt 0) {
            Write-Host "==> Integrating $behindOrigin new commit(s) from GitHub (origin)..." -ForegroundColor Cyan
            git merge "origin/$currentBranch" -m "Merge origin/$currentBranch into $currentBranch"
        }
    }

    # Fetch and integrate oldsmarchaos
    git fetch oldsmarchaos $currentBranch 2>$null
    if ($LASTEXITCODE -eq 0) {
        $behindGitlab = git rev-list --count "$currentBranch..oldsmarchaos/$currentBranch" 2>$null
        if ($behindGitlab -and [int]$behindGitlab -gt 0) {
            Write-Host "==> Integrating $behindGitlab new commit(s) from GitLab (oldsmarchaos)..." -ForegroundColor Cyan
            git merge "oldsmarchaos/$currentBranch" -m "Merge oldsmarchaos/$currentBranch into $currentBranch"
        }
    }
}

# 3. Push to GitLab (oldsmarchaos)
Write-Host "==> Pushing to GitLab (oldsmarchaos)..." -ForegroundColor Cyan
git push oldsmarchaos $currentBranch
if ($LASTEXITCODE -eq 0) {
    Write-Host " [OK] GitLab repository updated successfully: https://www.oldsmarchaos.com/git/cfw34683/honorbuddy-5.4.8-18414" -ForegroundColor Green
} else {
    Write-Host " [WARN] Failed pushing to GitLab (oldsmarchaos)." -ForegroundColor Red
}

# 4. Push to GitHub (origin)
Write-Host "==> Pushing to GitHub (origin)..." -ForegroundColor Cyan
git push origin $currentBranch
if ($LASTEXITCODE -eq 0) {
    Write-Host " [OK] GitHub repository updated successfully: https://github.com/ripsnortntear/Honorbuddy-MoP-5.4.8-18414" -ForegroundColor Green
} else {
    Write-Host " [WARN] Failed pushing to GitHub (origin)." -ForegroundColor Red
}

Write-Host "==> Sync complete!" -ForegroundColor Cyan
