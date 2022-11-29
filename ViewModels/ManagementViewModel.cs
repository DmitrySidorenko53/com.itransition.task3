using com.itransition.task3.Models;

namespace com.itransition.task3.ViewModels;

public class ManagementViewModel
{
    public IEnumerable<User> Users { get; }
    public PageViewModel PageViewModel { get;}
    
    public ManagementViewModel(IEnumerable<User> users, PageViewModel viewModel)
    {
        Users = users;
        PageViewModel = viewModel;
    }
}