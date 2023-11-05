using GoogleCalendarAPI.Services.AuthCalendarService;
using GoogleCalendarAPI.Services.EventsService;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions;
using GoogleCalendarAPI.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Google Calendar API",
        Version = "v1",
        Description = "Google Calendar API",
    });
});
builder.Services.Configure<GoogleApiSettings>(builder.Configuration.GetSection(nameof(GoogleApiSettings)));
builder.Services.AddScoped<IEventsService, EventsService>();
builder.Services.AddScoped<IAuthCalendarService, AuthCalendarService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Calendar API");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
