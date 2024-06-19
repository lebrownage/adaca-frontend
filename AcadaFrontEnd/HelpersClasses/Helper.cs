using Acada.FrontEnd.Models;

namespace AcadaFrontEnd.HelpersClasses
{
    public static class Helper
    {
        public static bool IsInvalidProduct(ProductDto data)
        {

            return  //check if Data is Null
                    data == null
                    //check if name is null or empty and if character exceeded
                    || string.IsNullOrWhiteSpace(data.Name) || data.Name.Length > 500
                    //check if Description is null or empty and if character exceeded
                    || string.IsNullOrWhiteSpace(data.Description) || data.Description.Length > 2000
                    //check if price is 0 and if price is negative
                    || data.Price == 0 || data.Price < 0; 
        }
    }
}
