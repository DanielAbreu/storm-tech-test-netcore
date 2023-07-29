using System.Collections.Generic;
using Todo.Data.Entities;
using Todo.Models.TodoItems;
using Todo.Services.GravatarServices.Client;
using Todo.Services.GravatarServices.Client.Models;

namespace Todo.EntityModelMappers.TodoItems
{
    public static class TodoItemSummaryViewmodelFactory
    {
        public static TodoItemSummaryViewmodel Create(TodoItem ti, Dictionary<string, GravatarProfileModel> gravatarProfilesByEmail)
        {
            return new TodoItemSummaryViewmodel(ti.TodoItemId, ti.Title, ti.IsDone, UserSummaryViewmodelFactory.Create(ti.ResponsibleParty), ti.Importance, ti.Rank, gravatarProfilesByEmail[ti.ResponsibleParty.UserName].DisplayName);
        }
    }
}