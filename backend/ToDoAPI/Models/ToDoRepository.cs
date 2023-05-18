using Microsoft.EntityFrameworkCore;

namespace ToDoAPI.Models
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ToDoManagementDbContext _context;

        public ToDoRepository(ToDoManagementDbContext context)
        {
            _context = context;
        }

        public async Task<ToDo> CreateToDo(ToDoDto dto)
        {
            ToDo toDo = new ToDo()
            {
                Title = dto.Title,
                Content = dto.Content,
                IsCompleted = false,
                CreationDate = DateTime.Now.ToUniversalTime(),
            };
            _context.ToDos.Add(toDo);
            await _context.SaveChangesAsync();
            return toDo;
        }

        public async Task<ToDo> GetByID(Guid id)
        {
            return await _context.ToDos.FirstOrDefaultAsync(_ => _.Id == id);
        }

        public async Task<ToDo> SetCompleted(Guid id)
        {
            ToDo newToDo = await _context.ToDos.FirstOrDefaultAsync(p => p.Id == id);

            if (newToDo != null)
            {
                newToDo.IsCompleted = true;

                await _context.SaveChangesAsync();
                return newToDo;
            }
            return null;
        }
        public async Task<ToDo> ChangeData(Guid id, ToDoDto dto)
        {
            ToDo newToDo = await _context.ToDos.FirstOrDefaultAsync(p => p.Id == id);

            if (newToDo != null)
            {
                newToDo.Title = dto.Title;
                newToDo.Content = dto.Content;

                await _context.SaveChangesAsync();
                return newToDo;
            }
            return null;
        }

        public async Task<IEnumerable<ToDo>> GetAll() => await _context.ToDos.OrderByDescending(t => t.CreationDate).ToListAsync();

        public async Task DeleteToDo(Guid id)
        {
            ToDo toDo = await _context.ToDos.FirstOrDefaultAsync(p => p.Id == id);
            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();
        }
    }
}
