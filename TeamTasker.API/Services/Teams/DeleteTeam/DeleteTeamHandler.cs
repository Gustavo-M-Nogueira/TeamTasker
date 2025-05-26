using BuildingBlocks.CQRS.Command;
using FluentValidation;
using TeamTasker.API.Data;
using TeamTasker.API.Exceptions.Teams;

namespace TeamTasker.API.Services.Teams.DeleteTeam
{
    public record DeleteTeamCommand(int Id) : ICommand<DeleteTeamResult>;
    public record DeleteTeamResult(bool IsSuccess);

    public class DeleteTeamCommandValidator : AbstractValidator<DeleteTeamCommand>
    {
        public DeleteTeamCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Team ID is required");
        }
    }

    internal class DeleteTeamHandler
        (ApplicationDbContext context)
        : ICommandHandler<DeleteTeamCommand, DeleteTeamResult>
    {
        public async Task<DeleteTeamResult> Handle(DeleteTeamCommand command, CancellationToken cancellationToken)
        {
            var team = await context.Teams.FindAsync(command.Id, cancellationToken);

            if (team is null)
            {
                throw new TeamNotFoundException(command.Id);
            }

            context.Teams.Remove(team);
            await context.SaveChangesAsync(cancellationToken);

            return new DeleteTeamResult(true);
        }
    }
}
