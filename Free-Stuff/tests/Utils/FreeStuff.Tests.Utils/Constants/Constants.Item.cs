namespace FreeStuff.Tests.Utils.Constants;

public static partial class Constants
{
    public static class Item
    {
        public static readonly Guid UserId = Guid.NewGuid();

        public const string Title       = "Item title";
        public const string Description = "Item description";
        public const string Condition   = "New";

        public const string EditedTitle       = "Edited item title";
        public const string EditedDescription = "Edited item description";
        public const string EditedCondition   = "Has given it all";
    }
}
