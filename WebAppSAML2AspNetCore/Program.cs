using Microsoft.AspNetCore.Authentication.Cookies;
using Sustainsys.Saml2.Metadata;
using Sustainsys.Saml2;
using Sustainsys.Saml2.AspNetCore2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var services = builder.Services;
services.AddAuthentication(sharedOptions =>
{
    sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    sharedOptions.DefaultChallengeScheme = Saml2Defaults.Scheme;
})
.AddSaml2(options =>
{
    options.SPOptions.EntityId = new EntityId("https://hoekstra.dev/SAMLBlogPost");
    options.IdentityProviders.Add(
      new IdentityProvider(
        new EntityId("https://sts.windows.net/5bc29b3a-acdf-4b00-a873-69c09bc3fbec/"), options.SPOptions)
      {
          MetadataLocation = "https://login.microsoftonline.com/5bc29b3a-acdf-4b00-a873-69c09bc3fbec/federationmetadata/2007-06/federationmetadata.xml?appid=7408fc7e-4358-4c7d-9e66-d2ee1b8a516a"
      });
})
.AddCookie();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
