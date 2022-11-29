using com.itransition.task3.Models;

namespace com.itransition.task3.ViewModels;

public class ManagementViewModel
{
    public IList<User> Users { get; }
    public PageViewModel PageViewModel { get;}
    
    public ManagementViewModel(IList<User> users, PageViewModel viewModel)
    {
        Users = users;
        PageViewModel = viewModel;
    }
}