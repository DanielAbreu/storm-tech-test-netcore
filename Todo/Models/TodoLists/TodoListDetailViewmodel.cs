using System.Collections.Generic;
using Todo.Models.TodoItems;

namespace Todo.Models.TodoLists
{
    public class TodoListDetailViewmodel
    {
        public int TodoListId { get; }
        public string Title { get; }
        public ICollection<TodoItemSummaryViewmodel> Items { get; }
        public TodoItemCreateFields TodoItemCreateFields { get; }

        public TodoListDetailViewmodel(int todoListId, string title, ICollection<TodoItemSummaryViewmodel> items, TodoItemCreateFields todoItemCreateFields)
        {
            Items = items;
            TodoListId = todoListId;
            Title = title;
            TodoItemCreateFields = todoItemCreateFields;
        }
    }
}