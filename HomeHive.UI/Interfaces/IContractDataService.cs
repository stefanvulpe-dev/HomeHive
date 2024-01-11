using HomeHive.Application.Features.Contracts.Commands.CreateContract;
using HomeHive.Application.Features.Contracts.Queries.GetAllContractsByUserId;
using HomeHive.UI.ViewModels;
using HomeHive.UI.ViewModels.Estates;

namespace HomeHive.UI.Interfaces;

public interface IContractDataService
{
    Task<CreateContractCommandResponse?> Add(ContractViewModel? estateBuyModel);
    Task<GetAllContractsByUserIdResponse?> GetAllContractsByUserId();
}