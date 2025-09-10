using System;
using POSApp.ViewModels;
using POSApp.Data;

namespace POSApp.Factories;

public class PageFactory
{
    private readonly Func<ApplicationPageNames, PageViewModel> pageFactory;
    
    public PageFactory(Func<ApplicationPageNames, PageViewModel> factory)
    {
        pageFactory = factory;
    }
    
    public PageViewModel GetPage(ApplicationPageNames pageName) => pageFactory.Invoke(pageName);
}