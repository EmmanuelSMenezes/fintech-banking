# FinTech Banking - Run All Frontends
# Script para rodar Admin Dashboard e Internet Banking simultaneamente

Write-Host "`n╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║     FinTech Banking - Iniciando Frontends                      ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════════════════════════════╝`n" -ForegroundColor Green

# Verificar se Node.js está instalado
if (-not (Get-Command node -ErrorAction SilentlyContinue)) {
    Write-Host "❌ Node.js não está instalado!" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Node.js encontrado: $(node --version)" -ForegroundColor Green
Write-Host "✅ npm encontrado: $(npm --version)`n" -ForegroundColor Green

# Admin Dashboard
Write-Host "📦 Instalando Admin Dashboard..." -ForegroundColor Cyan
cd admin-dashboard

if (-not (Test-Path "node_modules")) {
    npm install
} else {
    Write-Host "   ✅ Dependências já instaladas" -ForegroundColor Green
}

if (-not (Test-Path ".env.local")) {
    Write-Host "   ⚠️  Criando .env.local..." -ForegroundColor Yellow
    Copy-Item ".env.local.example" ".env.local"
}

Write-Host "   ✅ Admin Dashboard pronto`n" -ForegroundColor Green

# Internet Banking
cd ..
Write-Host "📦 Instalando Internet Banking..." -ForegroundColor Cyan
cd internet-banking

if (-not (Test-Path "node_modules")) {
    npm install
} else {
    Write-Host "   ✅ Dependências já instaladas" -ForegroundColor Green
}

if (-not (Test-Path ".env.local")) {
    Write-Host "   ⚠️  Criando .env.local..." -ForegroundColor Yellow
    Copy-Item ".env.local.example" ".env.local"
}

Write-Host "   ✅ Internet Banking pronto`n" -ForegroundColor Green

# Voltar para FrontEnd
cd ..

Write-Host "🚀 Iniciando servidores...`n" -ForegroundColor Yellow

Write-Host "📍 Admin Dashboard: http://localhost:3000" -ForegroundColor Cyan
Write-Host "📍 Internet Banking: http://localhost:3001`n" -ForegroundColor Cyan

# Iniciar Admin Dashboard em nova janela
Write-Host "Abrindo Admin Dashboard..." -ForegroundColor Green
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD\admin-dashboard'; npm run dev"

# Aguardar um pouco
Start-Sleep -Seconds 3

# Iniciar Internet Banking em nova janela
Write-Host "Abrindo Internet Banking..." -ForegroundColor Green
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD\internet-banking'; npm run dev"

Write-Host "`n✨ Ambos os frontends foram iniciados!" -ForegroundColor Green
Write-Host "   Verifique as janelas do PowerShell abertas`n" -ForegroundColor Green

