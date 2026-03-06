var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("smhi", c =>
{
    c.BaseAddress = new Uri("https://opendata-download-metfcst.smhi.se/");
    c.DefaultRequestHeaders.Add("Accept", "application/json");
 
    c.DefaultRequestHeaders.Add("User-Agent", "KladerEfterVaderApp");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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