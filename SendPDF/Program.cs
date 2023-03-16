using Microsoft.EntityFrameworkCore;
using SendMailPDF.Data;
using SendMailPDF.Repo.Interface;
using SendMailPDF.Repo;
using SendMailPDF.Services.Interface;
using SendMailPDF.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SqlDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDbConnectionString")));
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserService, UserServices>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ISendMailService, SendMailService>();
builder.Services.AddScoped<IDataEmailRepo, DataEmailRepo>();
builder.Services.AddScoped<IEmailRepo, EmailRepo>();
builder.Services.AddScoped<ISendPdfService, SendPdfService>();


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    //options.IdleTimeout = TimeSpan.FromSeconds(160);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
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
app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
