add this manually in the project
OopProject
|-Models
|-Controllers
|-Views
|-other files
|-(add new file) //create this file and name it appsettings.json

--content of the appsettings.json--
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=OpenLibDB;Trusted_Connection=True;TrustServerCertificate=true;" //the Server name must match the Server name you have
  },
  "AllowedHosts": "*"
}

Why need to add?
- the file contains the connection strings. Since it is included in the gitignore, we cannot pull and push the file after committing and so we need to manually add it.
- it is included in the gitignore because every collaborators have different server, so if everyone commit and push, this will not affect our connection string in our clone repository
