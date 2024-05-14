using ErrorOr;

namespace FreeStuff.Categories.Domain.Errors;

public partial class Errors
{
    public static class Category
    {
        public static Error NotFound(string name)
        {
            return Error.NotFound("Category.NotFoundError", $"Category with name: {name} does not exist");
        }

        public static Error DuplicateCategoryName(string name)
        {
            return Error.Conflict("Category.DuplicateNameError", $"Category with name: {name} already exist");
        }
    }
}
