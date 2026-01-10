using Evently.Common.Application.Messaging;
using Evently.Modules.Events.Application.Categories.GetCategory;

namespace Evently.Modules.Events.Application.Categories.CreateCategory;

public sealed record CreateCategoryCommand(string Name) : ICommand<CategoryResponse>;
