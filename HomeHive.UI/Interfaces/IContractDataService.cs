using HomeHive.Application.Features.Contracts.Commands.CreateContract;
using HomeHive.UI.ViewModels.Estates;

namespace HomeHive.UI.Interfaces;

public interface IContractDataService
{
    Task<CreateContractCommandResponse?> Add(EstateBuyModel? estateBuyModel);
}