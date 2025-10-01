using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using LibraryManagement.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<LibraryBranchService>();

// connect to database Data/Library.db
var dataFile = Path.Combine(builder.Environment.ContentRootPath, "Data", "Library.db");
Directory.CreateDirectory(Path.GetDirectoryName(dataFile)!);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dataFile}")
           .EnableSensitiveDataLogging());

var app = builder.Build();

// batch insert from the json files
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    try
    {
        var basePath = Path.Combine(builder.Environment.ContentRootPath, "Data");

        // Clear old data at each startup 
        //to ensure that the JSON file content is the only data source
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

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();