using Domain.Interface;
using Domain.Services;
using Infra.Repository.Generics.Interface;
using Infra.Repository;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Routing;
using Infra.Repository.Generics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Configure Repositories
ConfigureRepositories();
#endregion

#region Configure Services
ConfigureServices();
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region [Cors]
builder.Services.AddCors();
#endregion
#region [Cors]
app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});
#endregion

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureRepositories()
{
    builder.Services.AddSingleton(typeof(IRestorantiRepositoryGeneric<>), typeof(RestorantiManagerRepository<>));
    builder.Services.AddSingleton<IRTable, RTable>();
    builder.Services.AddSingleton<IRUserInternal, RUserInternal>();
    builder.Services.AddSingleton<IRRequest, RRequest>();
}

void ConfigureServices()
{
    builder.Services.AddSingleton<ITableService, TableService>();
    builder.Services.AddSingleton<IUserInternalService, UserInternalService>();
    builder.Services.AddSingleton<IRequestService, RequestService>();
}
