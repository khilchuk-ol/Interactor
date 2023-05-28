using SplitDivider.Application;
using SplitDivider.Infrastructure;
using SplitDivider.Infrastructure.Persistence;
using SplitDivider.WebApp;
using ConfigureServices = SplitDivider.WebApp.ConfigureServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebAppServices();
builder.Services.ConfigureJwt(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        await initialiser.InitialiseAsync().ConfigureAwait(true);
        await initialiser.SeedAsync().ConfigureAwait(true);
    }
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseOpenApi(configure =>
{
    configure.Path = "/api/specification.json";
});
app.UseSwaggerUi3(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.UseRouting();

app.UseCors(x => x
    .WithOrigins("http://localhost:3000")
    .AllowCredentials()
    .AllowAnyHeader()
    .AllowAnyMethod());

//app.UseIdentityServer();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapFallbackToFile("index.html");

var receiver = ConfigureServices.ConfigureEventSubscriptionReceiver(app.Services);
receiver.StartReceiving();
    
app.Lifetime.ApplicationStopped.Register(receiver.Dispose);

app.Run();