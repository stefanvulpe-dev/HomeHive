﻿using HomeHive.Application.Features.Contracts.Commands.CreateContract;
using HomeHive.Domain.Common.EntitiesUtils.Contracts;

namespace HomeHive.Tests.ApplicationTests.Contracts.Commands.CreateContract;

public class CreateContractCommandTests
{
    [Fact]
    public void CreateContractByIdCommand_SetProperties_ShouldModifyPropertyValues()
    {
        // Arrange
        var originalCommand = new CreateContractCommand(Guid.NewGuid(), new ContractData(
            Guid.NewGuid(), ContractType.Rent.ToString(), DateTime.Now,
            DateTime.Now.AddDays(30), "Test"));

        // Act
        var command = originalCommand with
        {
            UserId = Guid.NewGuid()
        };

        // Assert
        Assert.NotEqual(originalCommand.UserId, command.UserId);
    }
}