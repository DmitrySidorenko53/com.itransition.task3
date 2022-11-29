using com.itransition.task3.Models;
using com.itransition.task3.Models.UserModel;

namespace com.itransition.task3.ViewModels.Management;

public class ManagementViewModel
{
    public IList<User> Users { get; }
    public PageViewModel PageViewModel { get;}
    
    public ManagementViewModel(IList<User> users, PageViewModel viewModel)
    {
        Users = users;
        PageViewModel = viewModel;
    }

    public ManagementViewModel()
    {
        Users = new List<User>();
        PageViewModel = new PageViewModel();
    }
}