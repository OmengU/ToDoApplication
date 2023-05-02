using Microsoft.EntityFrameworkCore;

namespace ToDoAPI.Models
{
    public class ToDoRepository : IToDoRepository
    {
        public IEnumerable<ToDo> All => throw new NotImplementedException();
        private readonly ToDoManagementDbContext _context;

        public ToDoRepository(ToDoManagementDbContext context)
        {
            _context = context;
        }

        public void AddToDo(ToDo toDo)
        {
            _context.ToDos.Add(toDo);
        }

        public async Task<ToDo> CreateToDo(ToDoDto dto)
        {
            ToDo toDo = new ToDo()
            {
                Title = dto.Title,
                Content = dto.Content,
                IsCompleted = false,
            };
            AddToDo(toDo);
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

        public async Task<IEnumerable<ToDo>> GetAll() => await _context.ToDos.OrderBy(c => c.Id).ToListAsync();

        public async Task DeleteToDo(Guid id)
        {
            ToDo toDo = await _context.ToDos.FirstOrDefaultAsync(p => p.Id == id);
            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();
        }
    }
}
