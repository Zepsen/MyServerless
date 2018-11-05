using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Repository;
using Repository.Models;

namespace FunctionApp
{
    public static class GetMedicine
    {
        [FunctionName("GetMedicine")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequest req, TraceWriter log)
        {
            var id = req.Query["id"][0];
            var res = await new DocumentDbRepository<MedicineModel>("Medicine")
                .GetItemsAsync(i => i.Id == id);
            return new OkObjectResult(res);
        }
    }
}
