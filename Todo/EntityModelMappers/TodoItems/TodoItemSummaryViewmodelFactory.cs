using System.Threading.Tasks;
using Todo.Data.Entities;
using Todo.Models.TodoItems;
using Todo.Services.GravatarServices.Client;

namespace Todo.EntityModelMappers.TodoItems
{
    public class TodoItemSummaryViewmodelFactory
    {
        private IGravatarClient gravatarClient;
        public TodoItemSummaryViewmodelFactory(IGravatarClient gravatarClient) 
        {
            this.gravatarClient = gravatarClient;
        }
        public async Task<TodoItemSummaryViewmodel> Create(TodoItem ti)
        {
            var userSummaryModel = UserSummaryViewmodelFactory.Create(ti.ResponsibleParty);
            var gravatarProfile = await this.gravatarClient.GetGravatarProfile(userSummaryModel.UserName);
            return new TodoItemSummaryViewmodel(ti.TodoItemId, ti.Title, ti.IsDone, userSummaryModel, ti.Importance, ti.Rank, gravatarProfile.DisplayName);
        }
    }
}