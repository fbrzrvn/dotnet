namespace FreeStuff.Api;

public static class ApiEndpoints
{
    private const string Prefix = "api";

    public const string Errors = "/errors";
    public const string Health = $"{Prefix}/_health";

    public static class Items
    {
        public const string Base   = $"{Prefix}/items";
        public const string Create = Base;
        public const string Get    = $"{Base}/{{id}}";
        public const string GetAll = Base;
        public const string Search = $"{Base}/search";
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
    }

    public static class Category
    {
        public const string Base   = $"{Prefix}/categories";
        public const string Create = Base;
        public const string Get    = $"{Base}/{{name}}";
        public const string GetAll = Base;
        public const string Update = Base;
    }
}
