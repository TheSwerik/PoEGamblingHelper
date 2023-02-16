$Name = Read-Host -Prompt 'Name of the migration'
dotnet ef migrations add $Name --project src/Infrastructure -s src/Api

#TODO automate major minor version updates