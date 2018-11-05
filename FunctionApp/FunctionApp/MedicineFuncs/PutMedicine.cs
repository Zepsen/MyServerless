using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Repository;
using Repository.Models;

namespace FunctionApp.MedicineFuncs
{
    public static class PutMedicine
    {
        [FunctionName("PutMedicine")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)] HttpRequest req,
            ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<MedicineModel>(requestBody);

            try
            {
                var res = await new DocumentDbRepository<MedicineModel>("Medicine")
                        .UpsertAsync(data);

                if (res.StatusCode == HttpStatusCode.OK)
                    return new OkResult();
            }
            catch (System.Exception)
            {
                return new InternalServerErrorResult();
            }


            return new BadRequestResult();
        }
    }
}
