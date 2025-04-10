using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sustainsys.Saml2.AspNetCore2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddSaml2(options =>
{
    options.SPOptions.EntityId = new EntityId("https://saml.pubstrat.com/Saml2");
    options.IdentityProviders.Add(
        new IdentityProvider(
            new EntityId("https://login.microsoftonline.com/<TENANT-ID>/federationmetadata/2007-06/federationmetadata.xml"),
            options.SPOptions)
        {
            LoadMetadata = true
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("SamlProxyApp is running.");
    });
});

app.Run();