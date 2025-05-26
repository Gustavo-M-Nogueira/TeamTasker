using BuildingBlocks.Exceptions;

namespace TeamTasker.API.Exceptions.Tasks
{
    public class TaskNotFoundException : NotFoundException
    {
        public TaskNotFoundException(int id) : base("Task", id) { }
    }
}
