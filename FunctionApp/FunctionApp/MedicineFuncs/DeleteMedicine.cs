using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Repository;
using Repository.Models;

namespace FunctionApp.MedicineFuncs
{
    public static class DeleteMedicine
    {
        [FunctionName("DeleteMedicine")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)] HttpRequest req,
            ILogger log)
        {
            var id = req.Query["id"][0];
            var res = await new DocumentDbRepository<MedicineModel>("Medicine")
                .DeleteAsync(id);

            if(res.StatusCode == HttpStatusCode.OK)
                return new OkObjectResult(res);

            return new InternalServerErrorResult();
        }
    }
}
