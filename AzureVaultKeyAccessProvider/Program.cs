//using ApkaJezykowa.Keys;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
//using AzureVaultKeyAccessProvider.Keys;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using System.IO.Pipes;

public class Program
{
    public Program() { }
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Logging.AddConsole();
        builder.Configuration.AddJsonFile("appsettings.json");
        var keyVaultUrl = builder.Configuration.GetSection("KeyVault:KeyVaultURL");
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        //builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();

        var app = builder.Build();
        var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(new AzureServiceTokenProvider().KeyVaultTokenCallback));
        builder.Configuration.AddAzureKeyVault(keyVaultUrl.Value.ToString(), new DefaultKeyVaultSecretManager());
        var client = new SecretClient(new Uri(keyVaultUrl.Value.ToString()), new DefaultAzureCredential());

        var key = client.GetSecret("speechkey").Value.Value.ToString();
        var region = client.GetSecret("speechregion").Value.Value.ToString();
        var userBase = client.GetSecret("mssqluserconnectionstring").Value.Value.ToString();
        var courseBase = client.GetSecret("mssqlcourseconnectionstring").Value.Value.ToString();
        // Configure the HTTP request pipeline.
        /*if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }*/
        var server = new NamedPipeServerStream("DataPasser");
        server.WaitForConnection();
        using(var writer = new StreamWriter(server))
        {
            writer.AutoFlush = true;
            writer.WriteLine(key);
            writer.WriteLine(region);
            writer.WriteLine(userBase);
            writer.WriteLine(courseBase);
            server.WaitForPipeDrain();
        }


        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}