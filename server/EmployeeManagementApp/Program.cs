using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlTypes;
using dotenv.net;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalHost3000", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

DotEnv.Load();
var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
var dbDatabase = Environment.GetEnvironmentVariable("DB_DATABASE");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

var connectionString = $"server={dbServer};port={dbPort};database={dbDatabase};user={dbUser};password={dbPassword};";

builder.Services.AddTransient<MySqlConnection>(_ => new MySqlConnection(connectionString));
builder.Services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

var app = builder.Build();

app.UseCors("AllowLocalHost3000");
app.UseRouting();

app.MapGet("/", () => "Hello World fun!");

app.MapPost("/register", async (User user, IPasswordHasher passwordHasher, MySqlConnection connection) => {
try {

    string hashedPassword = passwordHasher.HashPassword(user.Password);

    var q = "INSERT INTO employee_manager.users (`username`, `password`) VALUES (?, ?)";
    
    Console.WriteLine(q);

    await connection.OpenAsync();
    using var cmd = new MySqlCommand(q, connection);
    cmd.Parameters.AddWithValue("@Username", user.Username);
    cmd.Parameters.AddWithValue("@Password", hashedPassword);
    await cmd.ExecuteNonQueryAsync();

   return Results.Json(new { StatusCode = 201, Message = "User created successfully" });


} catch (Exception ex) {
    return Results.Json(new { StatusCode = 500, Message = $"Error creating user: {ex}" });
}
finally {
    connection.Close();
}
});

app.Run();