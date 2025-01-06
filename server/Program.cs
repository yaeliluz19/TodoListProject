


using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Server=bd2ivxjpmx5a7gpoxc5j-mysql.services.clever-cloud.com;User=uyb9xnwwuyygrbpc;Password=crvceLArTRBtAmA8rhKG;Database=bd2ivxjpmx5a7gpoxc5j;";




// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS service
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Get the connection string from app configuration

// var connectionString = builder.Configuration.GetConnectionString("ToDoDB");



if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Connection string is missing!");
    return;
}
try
{
//     builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql(
//         "Server=bd2ivxjpmx5a7gpoxc5j-mysql.services.clever-cloud.com;User=uyb9xnwwuyygrbpc;Password=crvceLArTRBtAmA8rhKG;Database=bd2ivxjpmx5a7gpoxc5j",
//         Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.40-mysql")
//     )
// );
// Add the DbContext with the connection string
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
}
catch (Exception ex)
{
    Console.WriteLine($"שגיאה: {ex.Message}");
}


// Retrieve the JWT Secret key from the configuration
string secretKey = builder.Configuration["JWT:SecretKey"] ?? throw new ArgumentNullException("JWT:SecretKey is missing");

// Add JWT Authentication
var key = Encoding.ASCII.GetBytes(secretKey); // Secret Key
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// Add Authorization service
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors();
app.UseAuthentication(); // Enable JWT Authentication
app.UseAuthorization();  // Enable Authorization Middleware

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

// Route to Get All Tasks


// Login Endpoint
app.MapPost("/login", async (User loginRequest, ToDoDbContext dbContext) =>
{
    var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Username == loginRequest.Username);
    if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
    {
        return Results.Unauthorized();
    }

    // Generate JWT
    var claims = new[]
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim("UserId", user.Id.ToString())
    };

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddHours(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);

    return Results.Json(new { Token = tokenHandler.WriteToken(token) });
});

// Register Endpoint
app.MapPost("/register", async (User registerRequest, ToDoDbContext dbContext) =>
{

    if (string.IsNullOrEmpty(registerRequest.Username) || string.IsNullOrEmpty(registerRequest.Password))
    {
        return Results.BadRequest("Username and password are required.");
    }

    var existingUser = await dbContext.Users.AnyAsync(u => u.Username == registerRequest.Username);
    if (existingUser)
    {
        return Results.Conflict("Username already exists.");
    }

    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

    var newUser = new User
    {
        Username = registerRequest.Username,
        Password = hashedPassword
    };

    dbContext.Users.Add(newUser);
    await dbContext.SaveChangesAsync();

    return Results.Ok("User registered successfully.");
});
// app.MapGet("/", async (ToDoDbContext dbContext) =>
// {
//     var tasks = await dbContext.Items.ToListAsync();
//     return Results.Json(tasks);
// });

app.MapGet("/", /*[Microsoft.AspNetCore.Authorization.Authorize]*/ async (ToDoDbContext dbContext) =>
{
    // Fetch all items from the database
    var tasks = await dbContext.Items.ToListAsync();

    // Check if the list is empty
    if (tasks == null || tasks.Count == 0)
    {
        return Results.NotFound("No items found in the database.");
    }

    // Return the tasks as JSON
    return Results.Json(tasks);
});

// Route to Add New Task
app.MapPost("/", [Microsoft.AspNetCore.Authorization.Authorize] async (ToDoDbContext dbContext, Item newItem) =>
{
    dbContext.Items.Add(newItem);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/{newItem.Id}", newItem);
});

// Route to Update a Task
app.MapPut("/{id}", [Microsoft.AspNetCore.Authorization.Authorize] async (int id, ToDoDbContext dbContext, Item updatedItem) =>
{
    var item = await dbContext.Items.FindAsync(id);
    if (item == null)
    {
        return Results.NotFound("Item not found.");
    }

    if (!string.IsNullOrEmpty(updatedItem.Name)) item.Name = updatedItem.Name;
    item.IsComplete = updatedItem.IsComplete;
    await dbContext.SaveChangesAsync();
    return Results.Json(item);
});

// Route to Delete a Task
app.MapDelete("/{id}", [Microsoft.AspNetCore.Authorization.Authorize] async (int id, ToDoDbContext dbContext) =>
{
    var item = await dbContext.Items.FindAsync(id);
    if (item == null)
    {
        return Results.NotFound("Item not found.");
    }

    dbContext.Items.Remove(item);
    await dbContext.SaveChangesAsync();
    return Results.Ok("Item deleted successfully.");
});

app.Run();
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
// using TodoApi;

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// // Add CORS service
// builder.Services.AddCors(options =>
// {
//     options.AddDefaultPolicy(policy =>
//     {
//         policy.AllowAnyOrigin()
//               .AllowAnyMethod()
//               .AllowAnyHeader();
//     });
// });

// // Get the connection string from app configuration
// var connectionString = builder.Configuration.GetConnectionString("ToDoDB");
// if (string.IsNullOrEmpty(connectionString))
// {
//     Console.WriteLine("Connection string is missing!");
//     throw new InvalidOperationException("Database connection string is not configured properly.");
// }

// builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// // Retrieve the JWT Secret key from the configuration
// string secretKey = builder.Configuration["JWT:SecretKey"] ?? throw new ArgumentNullException("JWT:SecretKey is missing");

// // Add JWT Authentication
// var key = Encoding.ASCII.GetBytes(secretKey); // Secret Key
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = false,
//             ValidateAudience = false,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(key)
//         };
//     });

// // Add Authorization service
// builder.Services.AddAuthorization();

// var app = builder.Build();

// // Global Exception Handling Middleware
// app.Use(async (context, next) =>
// {
//     try
//     {
//         await next();
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"An error occurred: {ex.Message}");
//         context.Response.StatusCode = 500;
//         await context.Response.WriteAsJsonAsync(new { Error = "An internal server error occurred." });
//     }
// });

// app.UseCors();
// app.UseAuthentication(); // Enable JWT Authentication
// app.UseAuthorization();  // Enable Authorization Middleware

// // Enable Swagger
// app.UseSwagger();
// app.UseSwaggerUI();

// // Route to Get All Tasks
// app.MapGet("/", async (ToDoDbContext dbContext) =>
// {
//     if (dbContext == null)
//     {
//         return Results.Problem("Database context is null. Check the database configuration.");
//     }

//     var tasks = await dbContext.Items.ToListAsync();
//     if (tasks == null || tasks.Count == 0)
//     {
//         return Results.NotFound("No items found in the database.");
//     }

//     return Results.Json(tasks);
// });

// // Login Endpoint
// app.MapPost("/login", async (User loginRequest, ToDoDbContext dbContext) =>
// {
//     if (string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.Password))
//     {
//         return Results.BadRequest("Username and password are required.");
//     }

//     var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Username == loginRequest.Username);
//     if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
//     {
//         return Results.Unauthorized("Invalid username or password.");
//     }

//     var claims = new[]
//     {
//         new Claim(ClaimTypes.Name, user.Username),
//         new Claim("UserId", user.Id.ToString())
//     };

//     var tokenDescriptor = new SecurityTokenDescriptor
//     {
//         Subject = new ClaimsIdentity(claims),
//         Expires = DateTime.UtcNow.AddHours(1),
//         SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//     };

//     var tokenHandler = new JwtSecurityTokenHandler();
//     var token = tokenHandler.CreateToken(tokenDescriptor);

//     return Results.Json(new { Token = tokenHandler.WriteToken(token) });
// });

// // Register Endpoint
// app.MapPost("/register", async (User registerRequest, ToDoDbContext dbContext) =>
// {
//     if (string.IsNullOrWhiteSpace(registerRequest.Username) || string.IsNullOrWhiteSpace(registerRequest.Password))
//     {
//         return Results.BadRequest("Username and password cannot be empty.");
//     }

//     if (registerRequest.Username.Length < 5)
//     {
//         return Results.BadRequest("Username must be at least 5 characters long.");
//     }

//     if (registerRequest.Password.Length < 8)
//     {
//         return Results.BadRequest("Password must be at least 8 characters long.");
//     }

//     var existingUser = await dbContext.Users.AnyAsync(u => u.Username == registerRequest.Username);
//     if (existingUser)
//     {
//         return Results.Conflict("Username already exists.");
//     }

//     var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

//     var newUser = new User
//     {
//         Username = registerRequest.Username,
//         Password = hashedPassword
//     };

//     dbContext.Users.Add(newUser);
//     await dbContext.SaveChangesAsync();

//     return Results.Ok("User registered successfully.");
// });

// // Route to Add New Task
// app.MapPost("/", [Microsoft.AspNetCore.Authorization.Authorize] async (ToDoDbContext dbContext, Item newItem) =>
// {
//     if (string.IsNullOrWhiteSpace(newItem.Name))
//     {
//         return Results.BadRequest("Task name cannot be empty.");
//     }

//     dbContext.Items.Add(newItem);
//     await dbContext.SaveChangesAsync();
//     return Results.Created($"/{newItem.Id}", newItem);
// });

// // Route to Update a Task
// app.MapPut("/{id}", [Microsoft.AspNetCore.Authorization.Authorize] async (int id, ToDoDbContext dbContext, Item updatedItem) =>
// {
//     var item = await dbContext.Items.FindAsync(id);
//     if (item == null)
//     {
//         return Results.NotFound("Item not found.");
//     }

//     if (!string.IsNullOrEmpty(updatedItem.Name)) item.Name = updatedItem.Name;
//     item.IsComplete = updatedItem.IsComplete;
//     await dbContext.SaveChangesAsync();
//     return Results.Json(item);
// });

// // Route to Delete a Task
// app.MapDelete("/{id}", [Microsoft.AspNetCore.Authorization.Authorize] async (int id, ToDoDbContext dbContext) =>
// {
//     var item = await dbContext.Items.FindAsync(id);
//     if (item == null)
//     {
//         return Results.NotFound("Item not found.");
//     }

//     dbContext.Items.Remove(item);
//     await dbContext.SaveChangesAsync();
//     return Results.Ok("Item deleted successfully.");
// });

// app.Run();
