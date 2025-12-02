using System.Windows.Input;

namespace Group5F25.APP.PageModels
{
    public class RegistrationSuccessViewModel
    {
        public ICommand ReturnToLoginCommand { get; }

        public RegistrationSuccessViewModel()
        {
            ReturnToLoginCommand = new Command(async () =>
            {
                // Adjust route to whatever your Login route is
                await Shell.Current.GoToAsync("//login");
            });
        }
    }
}
