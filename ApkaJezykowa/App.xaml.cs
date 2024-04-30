using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ApkaJezykowa.Keys;
using ApkaJezykowa.MVVM.View;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ApkaJezykowa
{
  /// <summary>
  /// Logika interakcji dla klasy App.xaml
  /// </summary>
  
  public partial class App : Application
  {
    /*public IServiceProvider ServiceProvider { get; private set; }
    public IConfiguration Configuration { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {

           var applicationBuilder = Host.CreateApplicationBuilder();

            applicationBuilder.Logging.AddConsole();

            applicationBuilder.Configuration.AddJsonFile("AppSettings.json");

            applicationBuilder.Environment.ApplicationName = "ApkaJezykowa";

            var keyVaultUrl = applicationBuilder.Configuration.GetSection("KeyVault:KeyVaultURL");

            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(new AzureServiceTokenProvider().KeyVaultTokenCallback));
            applicationBuilder.Configuration.AddAzureKeyVault(keyVaultUrl.Value.ToString(), new DefaultKeyVaultSecretManager());
            var client = new SecretClient(new Uri(keyVaultUrl.Value.ToString()), new DefaultAzureCredential());

            SpeechServiceKey.Instance.Key = client.GetSecret("speechkey").Value.Value.ToString();
            SpeechServiceKey.Instance.Region = client.GetSecret("speechregion").Value.Value.ToString();
          }*/
  }
}
