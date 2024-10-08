﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Data;

namespace Api;

public class ProductsPost
{
    private readonly IProductData productData;

    public ProductsPost(IProductData productData)
    {
        this.productData = productData;
    }

    [FunctionName("ProductsPost")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "ProductsPost")] HttpRequest req,
        ILogger log)
    {
        var body = await new StreamReader(req.Body).ReadToEndAsync();
        var product = JsonSerializer.Deserialize<Product>(body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        var newProduct = await productData.AddProduct(product);
        return new OkObjectResult(newProduct);
    }
}
