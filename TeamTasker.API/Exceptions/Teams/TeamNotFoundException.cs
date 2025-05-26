using BuildingBlocks.Exceptions;

namespace TeamTasker.API.Exceptions.Teams
{
    public class TeamNotFoundException : NotFoundException
    {
        public TeamNotFoundException(int id) : base("Team", id) { }
    }
}
