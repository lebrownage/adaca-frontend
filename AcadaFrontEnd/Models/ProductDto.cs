namespace Acada.FrontEnd.Models
{
    //For context lets assume that the front end its a microservice that connects to our WebAPI
    //this class for structuring the JSON Return from the API
    public class ProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public float Price { get; set; }
    }
}
