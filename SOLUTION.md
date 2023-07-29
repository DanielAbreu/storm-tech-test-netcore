**Observations**
  * The ```TodoListDetailViewmodelFactory``` and ```TodoItemSummaryViewmodelFactory``` were changed from static to instace because ```TodoItemSummaryViewmodelFactory``` had to receive an instance of the ```IGravatarClient``` to request data from Gravatar
  * The ```Autofixture``` and ```Moq``` libraries were added to the Tests project in order to assist the tests creation
  * I also want to leave a note referring the liberty that I took designing the UX, since it was not mentioned how the UI and UX supposed to be, so I take some liberties