using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Repository;
using Repository.Models;

namespace FunctionApp.MedicineFuncs
{
    public static class PostMedicine
    {
        [FunctionName("PostMedicine")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<MedicineModel>(requestBody);

            try
            {
                var res = await new DocumentDbRepository<MedicineModel>("Medicine")
                        .CreateAsync(data);

                if (res.StatusCode == HttpStatusCode.Created)
                    return new OkResult();
            }
            catch (Exception ex)
            {
                return new InternalServerErrorResult();
            }

            return new BadRequestResult();
        }
    }
}
