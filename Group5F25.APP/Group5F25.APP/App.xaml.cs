using Microsoft.Maui;                // ensure these
using Microsoft.Maui.Controls;

namespace Group5F25.APP
{
    public partial class App : Application
    {
        private readonly AppShell _rootShell;
        public App(AppShell rootShell) { InitializeComponent(); _rootShell = rootShell; }
        protected override Window CreateWindow(IActivationState? s) => new Window(_rootShell);
    }

}
