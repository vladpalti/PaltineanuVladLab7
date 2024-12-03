namespace PaltineanuVladLab7;
using PaltineanuVladLab7.Models;

public partial class ProductPage : ContentPage
{
    ShopList sl;

    public ProductPage(ShopList slist)
    {
        InitializeComponent();
        sl = slist;
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var product = (Product)BindingContext;
        await App.Database.SaveProductAsync(product);
        listView.ItemsSource = await App.Database.GetProductsAsync();
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var product = listView.SelectedItem as Product;
        await App.Database.DeleteProductAsync(product);
        listView.ItemsSource = await App.Database.GetProductsAsync();
    }

    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        var productPage = new ProductPage(sl); 
        productPage.BindingContext = new Product(); 
        await Navigation.PushAsync(productPage);
    }

    async void OnAddButtonClicked(object sender, EventArgs e)
    {
        var product = (Product)BindingContext;
        var listProduct = new ListProduct
        {
            ProductID = product.ID,
            ShopListID = sl.ID
        };

        await App.Database.SaveListProductAsync(listProduct);
        listView.ItemsSource = await App.Database.GetListProductsAsync(sl.ID);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        listView.ItemsSource = await App.Database.GetProductsAsync();
    }
}
