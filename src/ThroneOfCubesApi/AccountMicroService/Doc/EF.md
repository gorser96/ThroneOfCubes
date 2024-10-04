dotnet ef migrations add update_001_ --output-dir .\Infrastructure\Migrations --project AccountMicroService --startup-project AccountMicroService

dotnet ef database update --project AccountMicroService --connection ""