using BuildingBlocks.CQRS.Command;
using FluentValidation;
using TeamTasker.API.Data;
using TeamTasker.API.Models.DTOs;
using TeamTasker.API.Models.Entities;

namespace TeamTasker.API.Services.Teams.CreateTeam
{
    public record CreateTeamCommand
        (TeamRequestDto Team) 
        : ICommand<CreateTeamResult>;

    public record CreateTeamResult(int Id);

    public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
    {
        public CreateTeamCommandValidator()
        {
            RuleFor(x => x.Team).NotEmpty();
            RuleFor(x => x.Team.Name).NotEmpty().Length(2, 40).WithMessage("Name must be between 2-40 characters");
        }
    }


    internal class CreateTeamHandler
        (ApplicationDbContext context) 
        : ICommandHandler<CreateTeamCommand, CreateTeamResult>
    {
        public async Task<CreateTeamResult> Handle(CreateTeamCommand command, CancellationToken cancellationToken)
        {
            var team = new Team
            {
                Name = command.Team.Name,
                Description = command.Team.Description,
                ImageUrl = command.Team.ImageUrl
            };

            context.Teams.Add(team);
            await context.SaveChangesAsync(cancellationToken);

            return new CreateTeamResult(team.Id);
        }
    }
}
