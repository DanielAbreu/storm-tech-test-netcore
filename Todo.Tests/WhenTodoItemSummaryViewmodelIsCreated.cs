using AutoFixture;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoLists;
using Todo.Models.TodoItems;
using Todo.Services.GravatarServices.Client;
using Todo.Services.GravatarServices.Client.Models;
using Xunit;

namespace Todo.Tests
{
    public class WhenTodoListDetailViewmodelIsCreated
    {
        private readonly Fixture fixture;

        public WhenTodoListDetailViewmodelIsCreated()
        {
            this.fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task WhenTodoListDetailViewmodelIsCreatedmodelWithGravatarDisplayName()
        {
            //Arrange
            var listsLength = 3;
            var todoItemList = fixture.CreateMany<TodoItem>(listsLength).ToList();
            var todoList = fixture.Build<TodoList>()
                .With(tl => tl.Items, todoItemList)
                .Create();
            var todoItemCreateFields = fixture.Create<TodoItemCreateFields>();

            var todoItemListOrdered = todoItemList.OrderBy(ti => ti.TodoItemId);
            var gravatarProfileListOrdered = fixture.CreateMany<GravatarProfileModel>(listsLength).OrderBy(ti => ti.DisplayName);

            var gravatarClientMock = new Mock<IGravatarClient>();
            for(int i = 0; i < listsLength; ++i)
            {
                gravatarClientMock
                .Setup(g => g.GetGravatarProfile(todoItemListOrdered.ElementAt(i).ResponsibleParty.UserName))
                .ReturnsAsync(gravatarProfileListOrdered.ElementAt(i));
            }
            
            var sut = new TodoListDetailViewmodelFactory(gravatarClientMock.Object);

            //Act
            var todoListCreated = await sut.Create(todoList, todoItemCreateFields);

            //Assert
            Assert.Equal(todoList.Title, todoListCreated.Title);
            Assert.Equal(todoList.TodoListId, todoListCreated.TodoListId);
            Assert.Equal(todoItemCreateFields.Title, todoListCreated.TodoItemCreateFields.Title);
            Assert.Equal(todoItemCreateFields.TodoListTitle, todoListCreated.TodoItemCreateFields.TodoListTitle);
            Assert.Equal(todoItemCreateFields.ResponsiblePartyId, todoListCreated.TodoItemCreateFields.ResponsiblePartyId);
            Assert.Equal(todoItemCreateFields.Importance, todoListCreated.TodoItemCreateFields.Importance);
            Assert.Equal(todoItemCreateFields.TodoListId, todoListCreated.TodoItemCreateFields.TodoListId);
            for (int i = 0; i < listsLength; ++i)
            {
                var itemCreated = todoListCreated.Items.Single(ti => ti.TodoItemId == todoItemListOrdered.ElementAt(i).TodoItemId);
                Assert.Equal(todoItemListOrdered.ElementAt(i).TodoItemId, itemCreated.TodoItemId);
                Assert.Equal(todoItemListOrdered.ElementAt(i).Title, itemCreated.Title);
                Assert.Equal(todoItemListOrdered.ElementAt(i).ResponsibleParty.Email, itemCreated.ResponsibleParty.Email);
                Assert.Equal(todoItemListOrdered.ElementAt(i).ResponsibleParty.UserName, itemCreated.ResponsibleParty.UserName);
                Assert.Equal(todoItemListOrdered.ElementAt(i).Rank, itemCreated.Rank);
                Assert.Equal(todoItemListOrdered.ElementAt(i).IsDone, itemCreated.IsDone);
                Assert.Equal(gravatarProfileListOrdered.ElementAt(i).DisplayName, itemCreated.DisplayName);
                gravatarClientMock.Verify(g => g.GetGravatarProfile(todoItemListOrdered.ElementAt(i).ResponsibleParty.UserName), Times.Once());
            }
        }

        [Fact]
        public async Task TodoItemSummaryViewmodelWithGravatarCallReturningNoInfo()
        {
            //Arrange
            var todoItemList = fixture.CreateMany<TodoItem>().ToList();
            var todoList = fixture.Build<TodoList>()
                .With(tl => tl.Items, todoItemList)
                .Create();
            var todoItemCreateFields = fixture.Create<TodoItemCreateFields>();

            var gravatarClientMock = new Mock<IGravatarClient>();
            foreach (var item in todoItemList)
            {
                gravatarClientMock
                .Setup(g => g.GetGravatarProfile(item.ResponsibleParty.UserName))
                .ReturnsAsync(new GravatarProfileModel());
            }
            
            var sut = new TodoListDetailViewmodelFactory(gravatarClientMock.Object);

            //Act
            var todoListCreated = await sut.Create(todoList, todoItemCreateFields);

            //Assert
            Assert.Equal(todoList.Title, todoListCreated.Title);
            Assert.Equal(todoList.TodoListId, todoListCreated.TodoListId);
            Assert.Equal(todoItemCreateFields.Title, todoListCreated.TodoItemCreateFields.Title);
            Assert.Equal(todoItemCreateFields.TodoListTitle, todoListCreated.TodoItemCreateFields.TodoListTitle);
            Assert.Equal(todoItemCreateFields.ResponsiblePartyId, todoListCreated.TodoItemCreateFields.ResponsiblePartyId);
            Assert.Equal(todoItemCreateFields.Importance, todoListCreated.TodoItemCreateFields.Importance);
            Assert.Equal(todoItemCreateFields.TodoListId, todoListCreated.TodoItemCreateFields.TodoListId);
            foreach (var item in todoItemList)
            {
                var itemCreated = todoListCreated.Items.Single(ti => ti.TodoItemId == item.TodoItemId);
                Assert.Equal(item.TodoItemId, itemCreated.TodoItemId);
                Assert.Equal(item.Title, itemCreated.Title);
                Assert.Equal(item.ResponsibleParty.Email, itemCreated.ResponsibleParty.Email);
                Assert.Equal(item.ResponsibleParty.UserName, itemCreated.ResponsibleParty.UserName);
                Assert.Equal(item.Rank, itemCreated.Rank);
                Assert.Equal(item.IsDone, itemCreated.IsDone);
                Assert.Null(itemCreated.DisplayName);
                gravatarClientMock.Verify(g => g.GetGravatarProfile(item.ResponsibleParty.UserName), Times.Once());
            }
        }
    }
}