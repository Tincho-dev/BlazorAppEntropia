using EntropiaBlazor.Data;
using GeneradorNumerosAleatorios.BLazorMAUI.Services;
using Microsoft.EntityFrameworkCore;
using NumerosAleatoriosIO.Services;
using Radzen;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("entropiadb"));
builder.Services.AddScoped<IFuenteService, FuenteService>();
builder.Services.AddSingleton<IGeneradorService, GeneradorService>();
builder.Services.AddSingleton<IPruebasEstadisticasService, PruebasEstadisticasService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
