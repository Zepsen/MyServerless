using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Repository.Models;

namespace FunctionApp.CosmosDbBinding
{
    public static class GetMed
    {
        [FunctionName("GetMed")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "TestDb",
                collectionName: "Medicine",
                ConnectionStringSetting = "CosmosDBConnection",
                Id = "{Query.id}")]MedicineModel medicine,
            ILogger log)
        {
            return new OkObjectResult(medicine);
        }
    }
}
