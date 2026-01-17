using DanaosBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DatabaseService>(); // Register the service

// Add CORS to allow frontend access
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

    //options.AddPolicy("ProductionPolicy", builder =>
    //builder.WithOrigins("https://www.my-react-app.com") // Only the frontend origin
    //       .AllowAnyMethod()
    //       .AllowAnyHeader());
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger middleware
    app.UseSwaggerUI(); // Enable Swagger UI
}

app.UseHttpsRedirection(); // Enforce HTTPS
app.UseCors("AllowAll"); // Handles cross-origin requests
app.UseAuthorization(); // Are you allowed?
app.MapControllers(); // Map controller routes
app.Run();
