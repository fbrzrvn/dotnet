namespace Identity.Api;

public static class ApiEndpoints
{
    private const string Prefix = "/api";

    public const string Errors = "/errors";

    public static class Identity
    {
        private const string Base = $"{Prefix}/identity";

        public const string Welcome        = $"{Base}/welcome";
        public const string Register       = $"{Base}/register";
        public const string ConfirmEmail   = $"{Base}/confirm-email";
        public const string Login          = $"{Base}/login";
        public const string RefreshToken   = $"{Base}/refresh-token";
        public const string ForgotPassword = $"{Base}/forgot-password";
        public const string ResetPassword  = $"{Base}/reset-password";
        public const string ChangePassword = $"{Base}/change-password";
        public const string Logout         = $"{Base}/logout";

        public static class User
        {
            public const string Create = $"{Base}/user";
            public const string Update = $"{Base}/user/{{id}}";
            public const string Delete = $"{Base}/user/{{id}}";
        }
    }
}
