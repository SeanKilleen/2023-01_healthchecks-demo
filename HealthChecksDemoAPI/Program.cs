using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecksDemoAPI;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
    .AddCheck<DummyHealthCheck>("Dummy")
    // ReSharper disable once NotResolvedInText -- no way to resolve "EventStore Connection String" within this format.

    // NOTE: This is obviously a dummy database for demo purposes. You'd get the connection string from configuration. Don't store plain-text credentials in your code.
    // This was used with a Docker image of SQL Server to show spinning it up and down.
    .AddSqlServer("Server=localhost;Database=MyDBServer;User Id=sa;Password=yourStrong(!)Password;Trust Server Certificate=true")
    .AddApplicationInsightsPublisher();
builder.Services
    .AddHealthChecksUI(setup =>
    {
        setup.MaximumHistoryEntriesPerEndpoint(50);
        setup.AddHealthCheckEndpoint("My APp Health", "/apphealth");
    })
    .AddInMemoryStorage();

var app = builder.Build();

app.MapHealthChecks("/apphealth", new HealthCheckOptions() { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
app.MapHealthChecksUI(options =>
{
    options.AsideMenuOpened = false;
    options.PageTitle = "My App Health Checks UI";
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
