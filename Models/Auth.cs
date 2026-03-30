namespace ToDoApplication.Models;

public class AccountClass
{
    public int id { get; set; }
    public string fname { get; set; } = "";
    public string lname { get; set; } = "";
    public string email { get; set; } = "";
    public string timemodified { get; set; } = "";
}

public class SignInRequestClass
{
    public string email { get; set; } = "";
    public string password { get; set; } = "";
}

public class SignUpRequestClass
{
    public string first_name { get; set; } = "";
    public string last_name { get; set; } = "";
    public string email { get; set; } = "";
    public string password { get; set; } = "";
    public string confirm_password { get; set; } = "";
}

public class SignInResponseClass
{
    public int status { get; set; }
    public AccountClass? data { get; set; }
    public string message { get; set; } = "";
}

public class SignUpResponseClass
{
    public int status { get; set; }
    public string message { get; set; } = "";
}
