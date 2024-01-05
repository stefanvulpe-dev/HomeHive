using HomeHive.Application.Features.Estates.Commands.CreateEstate;
using HomeHive.Application.Features.Estates.Commands.DeleteEstateById;
using HomeHive.Application.Features.Estates.Commands.UpdateEstate;
using HomeHive.Application.Features.Estates.Queries.GetAllEstates;
using HomeHive.Application.Features.Estates.Queries.GetEstateById;
using HomeHive.UI.ViewModels.Estates;

namespace HomeHive.UI.Interfaces;

public interface IEstateDataService
{
    Task<GetAllEstatesResponse?> GetAll();
    Task<GetEstateByIdResponse?> GetById(Guid id);
    Task<CreateEstateCommandResponse?> Add(CreateEstateModel entity);
    Task<UpdateEstateCommandResponse?> UpdateById(Guid id, CreateEstateModel entity);
    Task<DeleteEstateByIdCommandResponse?> DeleteById(Guid id);
}