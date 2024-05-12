var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add CORS middleware 
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAny",
                      policy =>
                      {
                          policy.AllowAnyOrigin();
                          policy.AllowAnyMethod();  
                          policy.AllowAnyHeader();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAny");

app.UseAuthorization();

app.MapControllers();

app.Run();
