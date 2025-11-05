using CommunityToolkit.Mvvm.Input;
using Group5F25.APP.Models;

namespace Group5F25.APP.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}