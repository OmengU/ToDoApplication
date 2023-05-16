namespace ToDoAPI.Models
{
    public interface IToDoRepository
    {
        IEnumerable<ToDo> All { get; }

        Task<ToDo> CreateToDo(ToDoDto dto);
        void AddToDo(ToDo toDo);
        Task<ToDo> GetByID(Guid id);
        Task<ToDo> SetCompleted (Guid id);
        Task<ToDo> ChangeData (Guid id, ToDoDto dto);
        Task<IEnumerable<ToDo>> GetAll();
        Task DeleteToDo(Guid id);
    }
}
