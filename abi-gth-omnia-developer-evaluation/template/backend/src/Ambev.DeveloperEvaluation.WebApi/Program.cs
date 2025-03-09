using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // API Essentials
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Database
        builder.Services.AddDbContext<DefaultContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
            )
        );

        // AutoMapper
        builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

        // MediatR - Registrando apenas os handlers específicos de venda
        builder.Services.AddScoped<IRequestHandler<CreateSaleCommand, CreateSaleResult>, CreateSaleHandler>();
        builder.Services.AddScoped<IRequestHandler<GetSaleCommand, GetSaleResult>, GetSaleHandler>();
        builder.Services.AddScoped<IRequestHandler<ListSalesCommand, ListSalesResult>, ListSalesHandler>();
        builder.Services.AddScoped<IRequestHandler<UpdateSaleCommand, UpdateSaleResult>, UpdateSaleHandler>();
        builder.Services.AddScoped<IRequestHandler<CancelSaleCommand, CancelSaleResult>, CancelSaleHandler>();

        // Validation
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // IoC
        builder.RegisterDependencies();

        var app = builder.Build();

        // Exception Handling
        app.UseMiddleware<ValidationExceptionMiddleware>();

        // Swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();

        app.Run();
    }
}
