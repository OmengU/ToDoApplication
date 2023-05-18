namespace ToDoAPI.Models
{
    public interface IToDoRepository
    {
        Task<ToDo> CreateToDo(ToDoDto dto);
        Task<ToDo> GetByID(Guid id);
        Task<ToDo> SetCompleted (Guid id);
        Task<ToDo> ChangeData (Guid id, ToDoDto dto);
        Task<IEnumerable<ToDo>> GetAll();
        Task DeleteToDo(Guid id);
    }
}
