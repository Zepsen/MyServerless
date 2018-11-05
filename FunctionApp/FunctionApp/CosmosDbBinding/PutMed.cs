using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Repository.Models;

namespace FunctionApp.CosmosDbBinding
{
    public static class PutMed
    {
        [FunctionName("PutMed")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "TestDb",
                collectionName: "Medicine",
                ConnectionStringSetting = "CosmosDBConnection")]
            DocumentClient client,
            ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<MedicineModel>(requestBody);

            //var res = await client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri("TestDb", "Medicine"), data);
            var res = await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri("TestDb", "Medicine", data.Id), data);
            if (res.StatusCode == HttpStatusCode.OK)
                return new OkObjectResult("Puted");

            return new BadRequestObjectResult("Wrong");
        }
    }
}
