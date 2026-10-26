/*Author: Zenwyn Naicker*/
/*Code Attribution: */
/**************************************************************************************************************
 * Title: HOW TO MAKE AN MVC [CLDV6211]
 * Author: The IIE
 * Date: 2025
 **************************************************************************************************************/
/**************************************************************************************************************
 * Title: MIGRATING A DATABASE TO AZURE [CLDV6211]
 * Author: The IIE
 * Date: 2025
 **************************************************************************************************************/
/**************************************************************************************************************
 * Title: Get started with ASP.NET Core MVC
 * Author: Microsoft
 * Date: 2025
 * Availability: https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-9.0&tabs=visual-studio
 **************************************************************************************************************/
/**************************************************************************************************************
 * Title: Quickstart: Deploy an ASP.NET web app
 * Author: Microsoft
 * Date: 2025
 * Availability: https://learn.microsoft.com/en-us/azure/app-service/quickstart-dotnetcore?tabs=net80&pivots=development-environment-vs
 **************************************************************************************************************/
/**************************************************************************************************************
 * Title: Customizing Navbar In MVC 5 With Bootstrap
 * Author: Hefaz, H
 * Date: 2025
 * Availability: https://www.csharp.com/article/customizing-navbar-in-mvc-5-with-bootstrap/
 **************************************************************************************************************/
/**************************************************************************************************************
 * Title: HTML, CSS, and C# in Web Development
 * Author: Dentistkiller Yusuf
 * Date: 2025
 * Availability: https://github.com/Dentistkiller/CSS
 **************************************************************************************************************/
/**************************************************************************************************************
 * Title: Easy to use, stylish placeholders
 * Author: Picsum
 * Date: 2025
 * Availability: https://picsum.photos/
 **************************************************************************************************************/
/**************************************************************************************************************
 * Title: Venues Pictures Download Free Images on Unsplash
 * Author: Unsplash
 * Date: 2025
 * Availability: https://unsplash.com/s/photos/venues
 **************************************************************************************************************/
/**************************************************************************************************************
 * Title: Integrating Azure Blob Storage with .NET
 * Author: YogeshKumar Hadiya
 * Date: 2024
 * Availability: https://medium.com/c-sharp-programming/integrating-azure-blob-storage-with-net-b4fc16dfde73
 **************************************************************************************************************/
/**************************************************************************************************************
 * Title: Free Venue Photos
 * Author: Pexels
 * Date: 2025
 * Availability: https://www.pexels.com/search/venue/
 **************************************************************************************************************/
/**************************************************************************************************************
 * Title: Tutorial: Add sorting, filtering, and paging with the Entity Framework in an ASP.NET MVC application
 * Author: Microsoft
 * Date: 2023
 * Availability: https://learn.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application
 **************************************************************************************************************/
/**************************************************************************************************************
 * Title: Filters in ASP.NET MVC
 * Author: Nitin Pandit
 * Date: 2024
 * Availability: https://www.c-sharpcorner.com/article/filters-in-Asp-Net-mvc-5-0-part-twelve/
 **************************************************************************************************************/

using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using TheEventEaseWebApp.Models;

var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);

// Add DbContext
builder.Services.AddDbContext<TheEventEaseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Azure Blob Storage service
builder.Services.AddSingleton<BlobServiceClient>(provider =>
    new BlobServiceClient(builder.Configuration.GetConnectionString("ContainerConnection")));

// Add BlobStorageService if you're using it
// builder.Services.AddScoped<BlobStorageService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();