using HomeHive.Application.Features.Estates.Commands.DeleteEstateById;
using HomeHive.Application.Features.Estates.Commands.UpdateEstate;
using HomeHive.Application.Features.Estates.Queries.GetAllEstates;
using HomeHive.Application.Features.Estates.Queries.GetEstateById;
using HomeHive.Application.Features.Users.Commands.CreateEstate;
using HomeHive.UI.ViewModels.Estates;

namespace HomeHive.UI.Utils.Interfaces;

public interface IEstateDataService
{
    void SetAccessToken(string token);
    Task<GetAllEstatesResponse?> GetAll();
    Task<GetEstateByIdResponse?> GetById(Guid id);
    Task<CreateEstateCommandResponse?> Add(EstateModel entity);
    Task<UpdateEstateCommandResponse?> UpdateById(Guid id, EstateModel entity);
    Task<DeleteEstateByIdCommandResponse?> DeleteById(Guid id);
}