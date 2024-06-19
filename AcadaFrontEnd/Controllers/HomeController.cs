using Acada.FrontEnd.Models;
using AcadaFrontEnd.HelpersClasses;
using AcadaFrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;

namespace AcadaFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static string urlDomain = "https://localhost:44334/api/products";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                //get the products from the API
                using HttpClient httpClient = new();
                HttpResponseMessage response = await httpClient.GetAsync(urlDomain);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                List<ProductDto>? product = JsonConvert.DeserializeObject<List<ProductDto>?>(responseBody);
              

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on Getting Products");
                return View(new List<ProductDto>());
            }
        }
        [HttpGet("/home/get/{id}")]
        public async Task<IActionResult> GetProduct(long id)
        {
            try
            {
                //get the products from the API
                using HttpClient httpClient = new();
                HttpResponseMessage response = await httpClient.GetAsync(urlDomain + "/" + id);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                ProductDto? product = JsonConvert.DeserializeObject<ProductDto?>(responseBody);
                if(product == null)
                {
                    return RedirectToAction("NotFoundPage", "Home");
                }
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on Getting Products");
                return View(new List<ProductDto>());
            }
        }


        [HttpPost("/home/add")]
        public async Task<IActionResult> AddProduct(ProductDto data)
        {
            try
            {
                //check if data is valid
                if (Helper.IsInvalidProduct(data))
                {
                    return BadRequest("Missing or Invalid Values");
                }

                string json = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                using HttpClient httpClient = new();
                HttpResponseMessage response = await httpClient.PostAsync(urlDomain, content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                ProductDto? createdProduct = JsonConvert.DeserializeObject<ProductDto?>(responseBody);
                //return product
                return Ok(createdProduct);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error on Adding Products");
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry something went wrong");
            }
        }

        [HttpPut("/home/update/{id}")]
        public async Task<IActionResult> UpdateProduct(ProductDto data)
        {
            try
            {
                //check if data is valid
                if (Helper.IsInvalidProduct(data))
                {
                    return BadRequest("Missing or Invalid Values");
                }

                string json = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                using HttpClient httpClient = new();
                HttpResponseMessage response = await httpClient.PutAsync(urlDomain + "/" + data.Id, content);
                response.EnsureSuccessStatusCode();

                return Ok();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error on Updating Products");
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry something went wrong");
            }
        }

        [HttpDelete("/home/delete/{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            try
            {
                if(id == 0)
                {
                    return BadRequest("Missing ID Parameter");
                }

                HttpContent content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                
                using HttpClient httpClient = new();
                HttpResponseMessage response = await httpClient.DeleteAsync(urlDomain + "/" + id);
                response.EnsureSuccessStatusCode();

                return Ok();
            }
            catch(Exception ex) 
            { 
            
                _logger.LogError(ex, "Error on Deleting Products");
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry something went wrong");
            }
        }

        public IActionResult NotFoundPage()
        {
            return View();
        }


    }
}