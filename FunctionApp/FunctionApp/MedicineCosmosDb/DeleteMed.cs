using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Repository.Models;

namespace FunctionApp.MedicineCosmosDb
{
    public static class DeleteMed
    {
        [FunctionName("DeleteMed")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "TestDb",
                collectionName: "Medicine",
                ConnectionStringSetting = "CosmosDBConnection")]
            DocumentClient client,
            ILogger log)
        {
            var res = await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri("TestDb", "Medicine", req.Query["id"][0]));
            if (res.StatusCode == HttpStatusCode.NoContent)
                return new OkObjectResult("Deleted");

            return new BadRequestObjectResult("Wrong");
        }
    }
}
