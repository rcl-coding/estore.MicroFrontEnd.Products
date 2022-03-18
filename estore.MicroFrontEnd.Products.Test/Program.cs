using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using RCL.Identity.AAD.Groups;
using RCL.Identity.GraphService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));

builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    var previousOptions = options.Events.OnRedirectToIdentityProvider;
    options.Events.OnRedirectToIdentityProvider = async context =>
    {
        await previousOptions(context);
        context.ProtocolMessage.ResponseType = Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectResponseType.IdToken;
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
        policy.Requirements.Add(new GroupsCheckRequirement(new string[] { "admin" })));
});

builder.Services.AddScoped<IAuthorizationHandler, B2CGroupsCheckHandler>();
builder.Services.AddTransient<IClaimsTransformation, B2CGroupClaimsTransformation>();
builder.Services.AddB2CGraphClientServices(options => builder.Configuration.Bind("AzureAdB2C", options));

builder.Services.AddAzureBlobStorageServices(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("Storage");
});

builder.Services.AddRazorPages()
      .AddMicrosoftIdentityUI();

builder.Services.AddProductsFrontEnd(options => builder.Configuration.Bind("Api", options));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
