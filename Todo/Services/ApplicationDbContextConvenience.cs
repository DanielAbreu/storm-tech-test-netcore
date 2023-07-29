using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Data.Entities;

namespace Todo.Services
{
    public static class ApplicationDbContextConvenience
    {
        public static IQueryable<TodoList> RelevantTodoLists(this ApplicationDbContext dbContext, string userId)
        {
            return dbContext.TodoLists.Include(tl => tl.Owner)
                .Include(tl => tl.Items)
                .Where(tl => tl.Owner.Id == userId || tl.Items.Any(i => i.ResponsiblePartyId == userId));
        }

        public static TodoList SingleTodoList(this ApplicationDbContext dbContext, int todoListId)
        {
            return dbContext.TodoLists.Include(tl => tl.Owner)
                .Include(tl => tl.Items)
                .ThenInclude(ti => ti.ResponsibleParty)
                .Single(tl => tl.TodoListId == todoListId);
        }

        public static TodoItem SingleTodoItem(this ApplicationDbContext dbContext, int todoItemId)
        {
            return dbContext.TodoItems.Include(ti => ti.TodoList).Single(ti => ti.TodoItemId == todoItemId);
        }

        public async static Task<TodoItem> EditTodoItemRank(this ApplicationDbContext dbContext, int todoItemId, int rank)
        {
            var todoItem = dbContext.TodoItems.Single(ti => ti.TodoItemId == todoItemId);

            todoItem.Rank = rank;

            await dbContext.SaveChangesAsync();

            return todoItem;
        }
    }
}