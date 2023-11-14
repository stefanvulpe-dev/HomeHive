using FluentValidation;

namespace HomeHive.Application.Features.Commands.UpdateUser;

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleSet("UpdateUser", () =>
            {
                RuleFor(p => p.UserId)
                    .NotEmpty().WithMessage("UserId is required.");

                RuleFor(p => p.UserData)
                    .NotNull().WithMessage("UserData is required.")
                    .Custom((userData, context) =>
                    {
                        if (string.IsNullOrWhiteSpace(userData.FirstName) &&
                            string.IsNullOrWhiteSpace(userData.LastName) &&
                            string.IsNullOrWhiteSpace(userData.Email) &&
                            string.IsNullOrWhiteSpace(userData.Password) &&
                            string.IsNullOrWhiteSpace(userData.PhoneNumber) &&
                            string.IsNullOrWhiteSpace(userData.ProfilePicture))
                        {
                            context.AddFailure("At least one field in UserData must be provided.");
                        }
                    });
            });
        }
    }
