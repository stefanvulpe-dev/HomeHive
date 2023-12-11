﻿using HomeHive.Application.Features.Estates.Commands.CreateEstate;
using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Users.Commands.CreateEstate;

public class CreateEstateCommandResponse : BaseResponse
{
    public CreateEstateDto Estate { get; set; } = default!;
}