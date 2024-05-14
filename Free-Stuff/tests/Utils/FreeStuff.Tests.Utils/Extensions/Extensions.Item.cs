using ErrorOr;
using FluentAssertions;

namespace FreeStuff.Tests.Utils.Extensions;

public static partial class Extensions
{
    public static void ValidateNotFoundError<T>(this ErrorOr<T> actual, Guid id)
    {
        actual.IsError.Should().BeTrue();
        actual.FirstError.Code.Should().Be("Item.NotFoundError");
        actual.FirstError.Description.Should().Be($"Item with id: {id} was not found");
    }
}
