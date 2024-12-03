using PaltineanuVladLab7.Models;

namespace PaltineanuVladLab7
{
    public partial class ListPage : ContentPage
    {
        public ListPage()
        {
            InitializeComponent();
        }
        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var slist = (ShopList)BindingContext;
            slist.Date = DateTime.UtcNow;
            await App.Database.SaveShopListAsync(slist);
            await Navigation.PopAsync();
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var slist = (ShopList)BindingContext;
            await App.Database.DeleteShopListAsync(slist);
            await Navigation.PopAsync();
        }

        async void OnChooseButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductPage((ShopList)this.BindingContext)
            {
                BindingContext = new Product()
            });
        }

        async void OnDeleteItemButtonClicked(object sender, EventArgs e)
        {
            var selectedItem = listView.SelectedItem as Product;  

            if (selectedItem != null)
            {
                var shopList = (ShopList)BindingContext;

                var listProduct = await App.Database.GetListProductForProductAsync(shopList.ID, selectedItem.ID);

                if (listProduct != null)
                {
                    await App.Database.DeleteListProductAsync(listProduct);
                    listView.ItemsSource = await App.Database.GetListProductsAsync(shopList.ID);
                }
                else
                {
                    await DisplayAlert("Error", "The selected product is not part of the current shopping list", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Please select an item to delete", "OK");
            }
        }



        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var shopList = (ShopList)BindingContext;
            listView.ItemsSource = await App.Database.GetListProductsAsync(shopList.ID);
        }
    }
}
