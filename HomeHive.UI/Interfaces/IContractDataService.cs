using HomeHive.Application.Features.Contracts.Commands.CreateContract;
using HomeHive.Application.Features.Contracts.Commands.UpdateContract;
using HomeHive.Application.Features.Contracts.Queries.GetAllContractsByOwnerId;
using HomeHive.Application.Features.Contracts.Queries.GetAllContractsByUserId;
using HomeHive.UI.ViewModels.Contracts;

namespace HomeHive.UI.Interfaces;

public interface IContractDataService
{
    Task<CreateContractCommandResponse?> Add(CreateContractViewModel? estateBuyModel);
    Task<GetAllContractsByUserIdResponse?> GetUserContracts();
    Task<GetAllContractsByOwnerIdQueryResponse?> GetAllContractsByOwnerId(Guid ownerId);
    Task<UpdateContractCommandResponse?> UpdateStatus(Guid? contractId, string? status);
}