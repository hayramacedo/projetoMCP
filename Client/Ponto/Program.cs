using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ponto;

var builder = WebApplication.CreateBuilder(args);

// Adicione a classe Routes como um serviço
builder.Services.AddSingleton<Routes>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Home}/{action=Index}/{id?}");
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "empresa",
    pattern: "api/{controller=Empresa}/{action=Empresa}/{id?}"
    );

app.MapControllerRoute(
    name: "departamento",
    pattern: "api/{controller=Departamento}/{action=Departamento}/{id?}"
    );

app.MapControllerRoute(
    name: "cargo",
    pattern: "api/{controller=Cargo}/{action=Cargo}/{id?}"
    );

app.MapControllerRoute(
    name: "folha",
    pattern: "api/{controller=Folha}/{action=PesquisarFolha}/{id?}/{id_funcionario?}/{data_inicio?}/{data_fim?}");

app.MapControllerRoute(
    name: "espelhoPonto",
    pattern: "api/{controller=Folha}/{action=EspelhoPonto}/{id?}");

app.MapControllerRoute(
    name: "usuariofuncionario",
    pattern: "api/{controller=UsuarioFuncionario}/{action=UsuarioFuncionario}/{id?}"
    );

app.MapControllerRoute(
    name: "solicitacaoAjuste",
    pattern: "api/{controller=Solicitacao}/{action=SolicitacaoAJuste}/{id?}"
    );

app.MapControllerRoute(
    name: "folhaAjuste",
    pattern: "api/{controller=Solicitacao}/{action=FolhaAjuste}/{id?}"
    );


app.Run();
