using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Repository.Models;

namespace FunctionApp.MedicineCosmosDb
{
    public static class PostMed
    {
        [FunctionName("PostMed")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "TestDb",
                collectionName: "Medicine",
                ConnectionStringSetting = "CosmosDBConnection")]out MedicineModel document,
            TraceWriter log)
        {
            var requestBody = new StreamReader(req.Body).ReadToEnd();
            document = JsonConvert.DeserializeObject<MedicineModel>(requestBody);

            return new OkObjectResult(document);
        }
    }
}
