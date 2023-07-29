using AutoFixture;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Services.GravatarServices.Client;
using Todo.Services.GravatarServices.Client.Models;
using Xunit;

namespace Todo.Tests
{
    public class WhenTodoItemSummaryViewmodelIsCreated
    {
        private readonly Fixture fixture;

        public WhenTodoItemSummaryViewmodelIsCreated()
        {
            this.fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task TodoItemSummaryViewmodelWithGravatarDisplayName()
        {
            //Arrange
            var todoItem = fixture.Create<TodoItem>();
            var gravatarProfile = fixture.Create<GravatarProfileModel>();

            var gravatarClientMock = new Mock<IGravatarClient>();
            gravatarClientMock
                .Setup(g => g.GetGravatarProfile(todoItem.ResponsibleParty.UserName))
                .ReturnsAsync(gravatarProfile);
            var sut = new TodoItemSummaryViewmodelFactory(gravatarClientMock.Object);

            //Act
            var itemCreated = await sut.Create(todoItem);

            //Assert
            Assert.Equal(todoItem.TodoItemId, itemCreated.TodoItemId);
            Assert.Equal(todoItem.Title, itemCreated.Title);
            Assert.Equal(todoItem.ResponsibleParty.Email, itemCreated.ResponsibleParty.Email);
            Assert.Equal(todoItem.ResponsibleParty.UserName, itemCreated.ResponsibleParty.UserName);
            Assert.Equal(todoItem.Rank, itemCreated.Rank);
            Assert.Equal(todoItem.IsDone, itemCreated.IsDone);
            Assert.Equal(gravatarProfile.DisplayName, itemCreated.DisplayName);
            gravatarClientMock.Verify(g => g.GetGravatarProfile(todoItem.ResponsibleParty.UserName), Times.Once());
        }

        [Fact]
        public async Task TodoItemSummaryViewmodelWithGravatarCallReturningNoInfo()
        {
            //Arrange
            var todoItem = fixture.Create<TodoItem>();

            var gravatarClientMock = new Mock<IGravatarClient>();
            gravatarClientMock
                .Setup(g => g.GetGravatarProfile(todoItem.ResponsibleParty.UserName))
                .ReturnsAsync(new GravatarProfileModel());
            var sut = new TodoItemSummaryViewmodelFactory(gravatarClientMock.Object);

            //Act
            var itemCreated = await sut.Create(todoItem);

            //Assert
            Assert.Equal(todoItem.TodoItemId, itemCreated.TodoItemId);
            Assert.Equal(todoItem.Title, itemCreated.Title);
            Assert.Equal(todoItem.ResponsibleParty.Email, itemCreated.ResponsibleParty.Email);
            Assert.Equal(todoItem.ResponsibleParty.UserName, itemCreated.ResponsibleParty.UserName);
            Assert.Equal(todoItem.Rank, itemCreated.Rank);
            Assert.Equal(todoItem.IsDone, itemCreated.IsDone);
            Assert.Null(itemCreated.DisplayName);
            gravatarClientMock.Verify(g => g.GetGravatarProfile(todoItem.ResponsibleParty.UserName), Times.Once());
        }
    }
}