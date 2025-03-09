using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class ApplicationModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        // Register FluentValidation apenas para vendas
        builder.Services.AddValidatorsFromAssemblyContaining<SaleValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<SaleItemValidator>();
        builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // Register validators apenas para vendas
        builder.Services.AddScoped<IValidator<Sale>, SaleValidator>();
        builder.Services.AddScoped<IValidator<SaleItem>, SaleItemValidator>();
    }
}