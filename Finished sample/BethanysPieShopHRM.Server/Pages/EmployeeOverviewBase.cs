using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BethanysPieShopHRM.Server.Components;
using BethanysPieShopHRM.Server.Services;
using BethanysPieShopHRM.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BethanysPieShopHRM.Server.Pages
{
    public class EmployeeOverviewBase: ComponentBase
    {
        [CascadingParameter]
        Task<AuthenticationState> authenticationStateTask { get; set; }

        [Inject]
        public IEmployeeDataService EmployeeDataService { get; set; }

        public List<Employee> Employees { get; set; }

        protected AddEmployeeDialog AddEmployeeDialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Employees = (await EmployeeDataService.GetAllEmployees()).ToList();
        }

        public async void AddEmployeeDialog_OnDialogClose()
        {
            Employees = (await EmployeeDataService.GetAllEmployees()).ToList();
            StateHasChanged();
        }

        protected async Task QuickAddEmployee()
        {
            var authenticationState = await authenticationStateTask;
            if (authenticationState.User.Identity.Name == "Kevin")
            {
                AddEmployeeDialog.Show();
            }
        }
    }
}
