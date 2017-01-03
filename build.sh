#!/bin/bash
set -ev
dotnet restore
dotnet test ShaverToolsShop/ShaverToolsShop.Test
dotnet build -c Release ShaverToolsShop/src/ShaverToolsShop