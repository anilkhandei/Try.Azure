using System;
using System.IO;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace key_vault_console_app
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //How to read config file in console application you ask?
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json",true,true);
            var config = builder.Build();

            const string secretName = "InSightAPIKey";
            var keyVaultName = "insightsecret";
            var kvUri = $"https://{keyVaultName}.vault.azure.net";

            //How to read the config values in console application you ask?
            string tenantId = config.GetSection("AzureAD:tenantID")?.Value??string.Empty;
            string clientId = config.GetSection("AzureAD:clientID")?.Value??string.Empty;
            string clientSecret = config.GetSection("AzureAD:clientSecret")?.Value??string.Empty;

            //how to authenticate with azure entraid for reading secret values?
            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var client = new SecretClient(new Uri(kvUri), credential);
            var secretValue = "SECRET_VALUE";

            Console.WriteLine("Forgetting your secret.");
            secretValue = string.Empty;
            Console.WriteLine($"Your secret is '{secretValue}'.");

            //How to read the secret value from Azure Key vault?
            Console.WriteLine($"Retrieving your secret from {keyVaultName}.");
            var secret = await client.GetSecretAsync(secretName);
            Console.WriteLine($"Your secret is '{secret.Value.Value}'.");
        }
    }
}
