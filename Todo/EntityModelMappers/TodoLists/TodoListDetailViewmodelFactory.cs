using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoItems;
using Todo.Models.TodoLists;
using Todo.Services.GravatarServices.Client;
using Todo.Services.GravatarServices.Client.Models;

namespace Todo.EntityModelMappers.TodoLists
{
    public class TodoListDetailViewmodelFactory
    {
        private IGravatarClient gravatarClient;
        public TodoListDetailViewmodelFactory(IGravatarClient gravatarClient) 
        {
            this.gravatarClient = gravatarClient;
        }

        public async Task<TodoListDetailViewmodel> Create(TodoList todoList, TodoItemCreateFields todoItemCreateFields)
        {
            var itemsGravatarProfilesDict = await GetItemsGravatarProfiles(todoList.Items);
            var items = todoList.Items.Select(ti => TodoItemSummaryViewmodelFactory.Create(ti, itemsGravatarProfilesDict)).ToList();
            return new TodoListDetailViewmodel(todoList.TodoListId, todoList.Title, items, todoItemCreateFields);
        }

        private async Task<Dictionary<string, GravatarProfileModel>> GetItemsGravatarProfiles(ICollection<TodoItem> items)
        {
            var emailListDistinct = items.Select(ti => ti.ResponsibleParty.UserName).Distinct();
            var gravatarProfilesDict = new Dictionary<string, GravatarProfileModel>();

            foreach(var email in emailListDistinct)
            {
                var gravatarProfile = await this.gravatarClient.GetGravatarProfile(email);
                gravatarProfilesDict[email] = gravatarProfile;
            }

            return gravatarProfilesDict;
        }
    }
}