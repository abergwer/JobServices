using JobServices.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddSingleton<JobService>();
builder.Services.AddSingleton<JobServices.Services.IJobService, JobServices.Services.JobService>();
builder.Services.AddHostedService<JobServices.Services.JobSchedulerService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure port from arguments
var port = 5001; // default port
var portArg = args.FirstOrDefault(arg => arg.StartsWith("--port="));
if (portArg != null && int.TryParse(portArg.Substring("--port=".Length), out var parsedPort))
{
    port = parsedPort;
}

builder.WebHost.UseUrls($"https://localhost:{port}");

var app = builder.Build();

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
