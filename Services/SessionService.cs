using ToDoApplication.Models;

namespace ToDoApplication.Services;

public interface ISessionService
{
    AccountClass? CurrentUser { get; set; }
}

public class SessionService : ISessionService
{
    public AccountClass? CurrentUser { get; set; }
}
