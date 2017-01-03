#!/bin/bash
set -ev
dotnet restore
dotnet test ShaverToolsShop/ShaverToolsShop.Tests
dotnet build -c Release ShaverToolsShop/src/ShaverToolsShop