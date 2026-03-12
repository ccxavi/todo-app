namespace ToDoApplication;

public static class AppRoutes
{
    public const string AppTabs = "AppTabs";
    public const string SignInPage = nameof(SignInPage);
    public const string SignUpPage = nameof(SignUpPage);
    public const string ToDoPage = nameof(ToDoPage);
    public const string CompletedPage = nameof(CompletedPage);
    public const string ProfilePage = nameof(ProfilePage);

    public const string SignInAbsolute = $"//{SignInPage}";
    public const string ToDoAbsolute = $"//{AppTabs}/{ToDoPage}";
    public const string CompletedAbsolute = $"//{AppTabs}/{CompletedPage}";
    public const string ProfileAbsolute = $"//{AppTabs}/{ProfilePage}";
}
