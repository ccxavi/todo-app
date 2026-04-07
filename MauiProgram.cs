using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ToDoApplication.Services;

namespace ToDoApplication;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Inter-Regular.ttf", "InterRegular");
                fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
            });

        builder.Services.AddSingleton<ISessionService, SessionService>();

        builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
        {
            client.BaseAddress = new Uri("https://todo-list.dcism.org");
            client.Timeout = TimeSpan.FromSeconds(60);
        });

        builder.Services.AddHttpClient<IToDoService, ToDoService>(client =>
        {
            client.BaseAddress = new Uri("https://todo-list.dcism.org");
            client.Timeout = TimeSpan.FromSeconds(60);
        });

        // Register Pages
        builder.Services.AddTransient<SignInPage>();
        builder.Services.AddTransient<SignUpPage>();
        builder.Services.AddTransient<ToDoPage>();
        builder.Services.AddTransient<CompletedPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<AddToDoPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
