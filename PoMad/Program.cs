using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PoMad.Components;
using PoMad.Components.Account;
using PoMad.Data;
using PoMad.Services;
using Radzen;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();






//string? vaultUriString = builder.Configuration["VaultUri"];
//if (string.IsNullOrEmpty(vaultUriString))
//{
//    throw new InvalidOperationException("VaultUri configuration not found.");
//}
//Uri keyVaultEndpoint = new(vaultUriString);
//builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

//// Build a secret client
//string keyVaultUrl = "https://pomad.vault.azure.net/";
//SecretClient client = new(new Uri(keyVaultUrl), new DefaultAzureCredential());

//// Retrieve secrets
//Azure.Response<KeyVaultSecret> clientId = client.GetSecret("GoogleClientId");
//Azure.Response<KeyVaultSecret> clientSecret = client.GetSecret("GoogleClientSecret");

//// Configure Google authentication with secrets from Azure Key Vault
//builder.Services.AddAuthentication().AddGoogle(options =>
//{
//    options.ClientId = clientId.Value.Value;
//    options.ClientSecret = clientSecret.Value.Value;
//    options.SignInScheme = IdentityConstants.ExternalScheme;
//});





//string? vaultUriString = builder.Configuration["VaultUri"];
//if (string.IsNullOrEmpty(vaultUriString))
//{
//    throw new InvalidOperationException("VaultUri configuration not found.");
//}
//Uri keyVaultEndpoint = new Uri(vaultUriString);
//builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

//// Build a secret client
//SecretClient client = new SecretClient(keyVaultEndpoint, new DefaultAzureCredential());

//// Retrieve secrets asynchronously
//var clientId = await client.GetSecretAsync("GoogleClientId");
//var clientSecret = await client.GetSecretAsync("GoogleClientSecret");

//// Configure Google authentication with secrets from Azure Key Vault
//builder.Services.AddAuthentication().AddGoogle(options =>
//{
//    options.ClientId = clientId.Value.Value;
//    options.ClientSecret = clientSecret.Value.Value;
//    options.SignInScheme = IdentityConstants.ExternalScheme;
//});







// Bind Google settings from appsettings.json
builder.Services.Configure<GoogleAuthConfig>(builder.Configuration.GetSection("GoogleAuth"));
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    GoogleAuthConfig googleConfig = builder.Configuration.GetSection("GoogleAuth").Get<GoogleAuthConfig>();
    googleOptions.ClientId = googleConfig.ClientId;
    googleOptions.ClientSecret = googleConfig.ClientSecret;
    googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
});






builder.Services.AddTransient<DailyDataService>();
builder.Services.AddTransient<EmailService>();
builder.Services.AddTransient<DailyDataService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => options.DetailedErrors = true);






builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseMigrationsEndPoint();
}
else
{
    _ = app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _ = app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();



//  az webapp config appsettings set --name PoMad --resource-group PoMad --settings GoogleAuth:ClientId="732386519629-bqh9gqoq1snfcb6j1fh88j5lscj5v4ht.apps.googleusercontent.com" GoogleAuth:ClientSecret="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"
