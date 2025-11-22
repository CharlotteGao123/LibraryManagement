using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Facebook;
using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register your application services
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<LibraryBranchService>();

// Setup SQLite database
var dataFile = Path.Combine(builder.Environment.ContentRootPath, "Data", "Library.db");
Directory.CreateDirectory(Path.GetDirectoryName(dataFile)!);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dataFile}")
           .EnableSensitiveDataLogging());

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "ThisIsAVeryLongSecretKeyForJWTAuthenticationPurposeWithMinimumLength32Characters";
var key = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Setup Identity with default UI
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;

    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultUI();

// Google Authentication
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    var rawClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientId = rawClientId?.Replace("<", "").Replace(">", "");
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});

builder.Services.AddAuthentication()
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
        options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    });

// Configure authentication cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Migrate database and batch insert JSON data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    try
    {
        var basePath = Path.Combine(builder.Environment.ContentRootPath, "Data");

        // Clear old data
        db.Books.RemoveRange(db.Books);
        db.Customers.RemoveRange(db.Customers);
        db.Authors.RemoveRange(db.Authors);
        db.LibraryBranches.RemoveRange(db.LibraryBranches);
        db.SaveChanges();

        // 1. LibraryBranches
        var branchesFile = Path.Combine(basePath, "librarybranches.json");
        if (File.Exists(branchesFile))
        {
            var branches = JsonSerializer.Deserialize<List<LibraryBranch>>(File.ReadAllText(branchesFile));
            if (branches != null)
            {
                db.LibraryBranches.AddRange(branches);
                db.SaveChanges();
                Console.WriteLine($"Inserted {branches.Count} LibraryBranches.");
            }
        }

        // 2. Authors
        var authorsFile = Path.Combine(basePath, "authors.json");
        if (File.Exists(authorsFile))
        {
            var authors = JsonSerializer.Deserialize<List<Author>>(File.ReadAllText(authorsFile));
            if (authors != null)
            {
                db.Authors.AddRange(authors);
                db.SaveChanges();
                Console.WriteLine($"Inserted {authors.Count} Authors.");
            }
        }

        // 3. Books
        var booksFile = Path.Combine(basePath, "books.json");
        if (File.Exists(booksFile))
        {
            var books = JsonSerializer.Deserialize<List<Book>>(File.ReadAllText(booksFile));
            if (books != null)
            {
                var validBooks = books.Where(b =>
                    db.Authors.Any(a => a.AuthorId == b.AuthorId) &&
                    db.LibraryBranches.Any(lb => lb.LibraryBranchId == b.LibraryBranchId)).ToList();

                db.Books.AddRange(validBooks);
                db.SaveChanges();
                Console.WriteLine($"Inserted {validBooks.Count} Books.");
            }
        }

        // 4. Customers
        var customersFile = Path.Combine(basePath, "customers.json");
        if (File.Exists(customersFile))
        {
            var customers = JsonSerializer.Deserialize<List<Customer>>(File.ReadAllText(customersFile));
            if (customers != null)
            {
                var validCustomers = customers.Where(c =>
                    db.LibraryBranches.Any(lb => lb.LibraryBranchId == c.LibraryBranchId)).ToList();

                db.Customers.AddRange(validCustomers);
                db.SaveChanges();
                Console.WriteLine($"Inserted {validCustomers.Count} Customers.");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Batch insert failed: " + ex.Message);
    }
}

// // Configure middleware - lambda style error handling
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler(exceptionHandlerApp =>
//     {
//         exceptionHandlerApp.Run(async context =>
//         {
//             context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//             context.Response.ContentType = "text/html";

//             await context.Response.WriteAsync("An exception was thrown.");

//             var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

//             if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
//             {
//                 await context.Response.WriteAsync(" The file was not found.");
//             }

//             if (exceptionHandlerPathFeature?.Path == "/")
//             {
//                 await context.Response.WriteAsync(" Page: Home.");
//             }
//         });
//     });
// }


// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    // Enable Swagger in Development environment
    app.UseSwagger();
    app.UseSwaggerUI();
}
// app.UseStatusCodePages(); //Status Code: 404; Not Found

// using static System.Net.Mime.MediaTypeNames;
//app.UseStatusCodePages("text/html", "Status Code Page: {0}"); //Status Code Page: 404

// UseStatusCodePages with lambda
// app.UseStatusCodePages(async statusCodeContext =>
// {
//    // using static System.Net.Mime.MediaTypeNames;
//   statusCodeContext.HttpContext.Response.ContentType = "text/html";
//   await statusCodeContext.HttpContext.Response.WriteAsync(
//     $"Status Code Page: {statusCodeContext.HttpContext.Response.StatusCode}");
// });

//app.UseStatusCodePagesWithRedirects("/StatusCode/{0}");

//UseStatusCodePagesWithReExecute
app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


// Map routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();


