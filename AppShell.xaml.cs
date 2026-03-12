namespace ToDoApplication;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(AppRoutes.SignUpPage, typeof(SignUpPage));
    }
}
