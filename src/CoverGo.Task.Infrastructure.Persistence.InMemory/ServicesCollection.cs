using CoverGo.Task.Application.Interfaces;
using CoverGo.Task.Application.Services;
using CoverGo.Task.Infrastructure.Persistence.InMemory;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServicesCollection
{
    public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        services.AddSingleton<ICompaniesQuery, CompaniesRepository>();
        services.AddSingleton<ICompaniesWriteRepository, CompaniesRepository>();
        services.AddSingleton<IPlansQuery, PlansRepository>();
        services.AddSingleton<IPlansWriteRepository, PlansRepository>();

        services.AddSingleton<IInsuredGroupQuery, InsuredGroupsRepository>();
        services.AddSingleton<IInsuredGroupWriteRepository, InsuredGroupsRepository>();

        services.AddSingleton<IProposalsQuery, ProposalsRepository>();
        services.AddSingleton<IProposalsWriteRepository, ProposalsRepository>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ProposalService>();
        services.AddSingleton<InsuredGroupService>();

        return services;
    }


}
