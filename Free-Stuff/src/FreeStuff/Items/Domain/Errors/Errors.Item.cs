using ErrorOr;

namespace FreeStuff.Items.Domain.Errors;

public static partial class Errors
{
    public static class Item
    {
        public static Error NotFound(Guid id)
        {
            return Error.NotFound("Item.NotFoundError", $"Item with id: {id} was not found");
        }
    }
}
