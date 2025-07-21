$Name = $args[0]
If ([string]::IsNullOrEmpty($Name)) { $Name = Read-Host -Prompt 'Name of the migration' }
dotnet ef migrations add $Name --project src/Infrastructure -s src/Api