using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;
using azure_function.Repositories;

namespace azure_function
{
    public static class ClientDetailsFunction
    {
        [FunctionName("ClientDetailsFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Inject] IClientFakeRepository _clientFakeRepository)
        {
            //Example getting by url
            string clientId = req.Query["clientId"];

            //Example getting by the body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            clientId ??= data?.clientId;

            var client = _clientFakeRepository.GetClientDetails(Convert.ToInt32(clientId));

            if (client == null) return new NotFoundResult();

            return new OkObjectResult(client);
        }
    }
}
