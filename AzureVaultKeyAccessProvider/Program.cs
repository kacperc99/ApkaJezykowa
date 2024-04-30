using ApkaJezykowa.Keys;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration.AzureKeyVault;

internal class Program
{
    private static void Main(string[] args)
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

        SpeechServiceKey.Instance.Key = client.GetSecret("speechkey").Value.Value.ToString();
        SpeechServiceKey.Instance.Region = client.GetSecret("speechregion").Value.Value.ToString();
        // Configure the HTTP request pipeline.
        /*if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }*/

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}