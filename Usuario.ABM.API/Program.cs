var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 👇 Configuración de CORS (permite cualquier origen, método y header)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy
            .AllowAnyOrigin()    // Permite cualquier dominio
            .AllowAnyMethod()    // Permite cualquier método HTTP (GET, POST, PUT, DELETE, etc.)
            .AllowAnyHeader();   // Permite cualquier encabezado
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 👇 Activar la política de CORS antes de la autorización
app.UseCors("PermitirTodo");

app.UseAuthorization();

app.MapControllers();

app.Run();
