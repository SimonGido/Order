using OrderAPI.Models;
using OrderAPI.Services;
using OrderAPI.DTO;

namespace OrderAPI
{
    public static class TestSetupShop // NOTE: Testing purposes
    {
        public static void CreateTestProducts(IOrderService orderService)
        {
            Product computerProduct = new();
            computerProduct.ProductName = "Computer";
            computerProduct.PricePerUnit = 1000.95m;


            Product keyboardProduct = new();
            keyboardProduct.ProductName = "Keyboard";
            keyboardProduct.PricePerUnit = 100.99m;

            orderService.CreateProductAsync(computerProduct).Wait();
            orderService.CreateProductAsync(keyboardProduct).Wait();
        }
    }
}
