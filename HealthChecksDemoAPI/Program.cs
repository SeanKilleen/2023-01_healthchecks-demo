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
    .AddSqlServer("Server=localhost;Database=operation-preparation;User Id=sa;Password=yourStrong(!)Password;Trust Server Certificate=true")
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
