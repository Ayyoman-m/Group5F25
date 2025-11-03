using System.Text.RegularExpressions;
using System.Windows.Input;
using Group5F25.APP.Services;

namespace Group5F25.APP.ViewModels;

public class RegisterViewModel : BindableObject
{
    private readonly IAuthApiClient _api;

    public RegisterViewModel(IAuthApiClient api)
    {
        _api = api;
        RegisterCommand = new Command(async () => await RegisterAsync(), () => IsValid);
    }

    private string _name = "";
    public string Name { get => _name; set { _name = value; OnPropertyChanged(); Raise(); } }

    private string _email = "";
    public string Email { get => _email; set { _email = value; OnPropertyChanged(); Raise(); } }

    private string _password = "";
    public string Password { get => _password; set { _password = value; OnPropertyChanged(); Raise(); } }

    private string _confirm = "";
    public string Confirm { get => _confirm; set { _confirm = value; OnPropertyChanged(); Raise(); } }

    private string _status = "";
    public string Status { get => _status; set { _status = value; OnPropertyChanged(); } }

    public ICommand RegisterCommand { get; }

    public bool EmailValid =>
        !string.IsNullOrWhiteSpace(Email) &&
        Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    public bool PasswordsMatch =>
        !string.IsNullOrEmpty(Password) && Password == Confirm;

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Name) &&
        EmailValid &&
        PasswordsMatch &&
        Password.Length >= 6;

    private void Raise() => (RegisterCommand as Command)?.ChangeCanExecute();

    private async Task RegisterAsync()
    {
        if (!IsValid) { Status = "Fix validation errors."; return; }
        Status = "";
        var res = await _api.RegisterAsync(Name.Trim(), Email.Trim(), Password);
        Status = res.IsSuccess ? "Registration successful." : res.Error ?? "Registration failed.";
    }
}
