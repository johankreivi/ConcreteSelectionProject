using ConcreteSelectionWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// The following line enables Application Insights telemetry collection.
builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//register services
builder.Services.AddScoped<IDgmlLabelBasedReaderService, DgmlLabelBasedReaderService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
