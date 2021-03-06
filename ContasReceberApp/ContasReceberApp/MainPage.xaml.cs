﻿using ContasReceberApp.Models;
using ContasReceberApp.Services;
using ContasReceberApp.ViewModels;
using ContasReceberApp.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ContasReceberApp
{
	public partial class MainPage : ContentPage
	{
        private AccountsViewModel AccountsViewModel;
        public MainPage()
        {
            InitializeComponent();
            BindingContext = AccountsViewModel = new AccountsViewModel();
            GetAccounts();
        }

        private void GetAccounts()
        {
            RestService.GetAsync("financeiro/extrato/dia/1036/" + InputData.Date.ToString("yyyy-MM-dd"),
                    //onSuccess
                    (response, result) =>
                    {
                        String json = JObject.Parse(result)["accountStatement"].ToString();
                        AccountsViewModel.Entity = JsonConvert.DeserializeObject<AccountStatement>(json);
                        AccountsListView.ItemsSource = (AccountsViewModel.Entity as AccountStatement).Accounts;
                        lbAccountsReceivable.Text = (AccountsViewModel.Entity as AccountStatement).AccountsReceivable.ToString("C2", new System.Globalization.CultureInfo("pt-BR"));
                        lbAccountsPayable.Text = (AccountsViewModel.Entity as AccountStatement).AccountsPayable.ToString("C2", new System.Globalization.CultureInfo("pt-BR"));
                    },

                    //onFailure
                    (e) => Task.FromResult(true)
                    );
        }

        private async void InsertAccount_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InsertAccountPage(App.Parametros));
        }

        private void InputData_DateSelected(object sender, DateChangedEventArgs e)
        {
            GetAccounts();
        }

        private void AccountsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new AccountDetailPage((Account)e.Item));
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new InsertAccountPage(App.Parametros));
        }
    }
}
