using System.Linq;
using System.Threading.Tasks;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoItems;
using Todo.Models.TodoLists;

namespace Todo.EntityModelMappers.TodoLists
{
    public class TodoListDetailViewmodelFactory
    {
        private TodoItemSummaryViewmodelFactory todoItemSummaryViewmodelFactory;
        public TodoListDetailViewmodelFactory(TodoItemSummaryViewmodelFactory todoItemSummaryViewmodelFactory) 
        {
            this.todoItemSummaryViewmodelFactory = todoItemSummaryViewmodelFactory;
        }

        public async Task<TodoListDetailViewmodel> Create(TodoList todoList, TodoItemCreateFields todoItemCreateFields)
        {
            var itemsTasks = todoList.Items.Select(this.todoItemSummaryViewmodelFactory.Create);
            var items = await Task.WhenAll(itemsTasks);
            return new TodoListDetailViewmodel(todoList.TodoListId, todoList.Title, items, todoItemCreateFields);
        }
    }
}