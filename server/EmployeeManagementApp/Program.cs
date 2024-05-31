using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlTypes;
using dotenv.net;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

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

app.MapPost("/register", async (User user, IPasswordHasher passwordHasher, MySqlConnection connection) =>
{
    try
    {

        string hashedPassword = passwordHasher.HashPassword(user.Password);

        var q = "INSERT INTO employee_manager.users (`username`, `password`) VALUES (?, ?)";

        Console.WriteLine(q);

        await connection.OpenAsync();
        using var cmd = new MySqlCommand(q, connection);
        cmd.Parameters.AddWithValue("@Username", user.Username);
        cmd.Parameters.AddWithValue("@Password", hashedPassword);
        await cmd.ExecuteNonQueryAsync();

        return Results.Json(new { StatusCode = 201, Message = "User created successfully" });


    }
    catch (Exception ex)
    {
        return Results.Json(new { StatusCode = 500, Message = $"Error creating user: {ex}" });
    }
    finally
    {
        connection.Close();
    }
});

app.MapPost("/login", async (UserLogin userLogin, IPasswordHasher passwordHasher, MySqlConnection connection, HttpContext httpContext) =>
{
    try
    {
        
        var q = "SELECT `id`, `username`, `password` FROM employee_manager.users WHERE `username` = @Username";
        Console.WriteLine(q);

        await connection.OpenAsync();
        using var cmd = new MySqlCommand(q, connection);
        cmd.Parameters.AddWithValue("@Username", userLogin.Username);
        Console.WriteLine(userLogin.Username);

        using var reader = await cmd.ExecuteReaderAsync();

        if (!reader.HasRows)
        {
            return Results.Json(new { StatusCode = 401, Message = "Invalid username or password" });
        }

        int userId = 0;
        string storedHashedPassword = null;

        while (await reader.ReadAsync())
        {
            userId = reader.GetInt32(0); 
            storedHashedPassword = reader.GetString(2); 

           
            Console.WriteLine(userId);
            Console.WriteLine(storedHashedPassword);
        }

        if (string.IsNullOrEmpty(storedHashedPassword) || userId == 0)
        {
            return Results.Json(new { StatusCode = 401, Message = "Invalid username or password" });
        }

        var passwordMatch = passwordHasher.VerifyPassword(storedHashedPassword, userLogin.Password);
        if (!passwordMatch)
        {
            return Results.Json(new { StatusCode = 401, Message = "Invalid username or password" });
        }

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(1) 
        };

        var cookieValue = $"{userLogin.Username}:{userId}";
         Console.WriteLine(cookieValue);
        httpContext.Response.Cookies.Append("token", cookieValue, cookieOptions);

        return Results.Json(new { StatusCode = 200, Message = "Login successful" });
    }
    catch (Exception ex)
    {
        return Results.Json(new { StatusCode = 500, Message = $"Error logging in: {ex.Message}" });
    }
    finally
    {
        await connection.CloseAsync();
    }
});


app.MapGet("/profile", async () => {
  
});

app.Run();