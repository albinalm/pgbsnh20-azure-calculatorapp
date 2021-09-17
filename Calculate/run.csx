#r "Newtonsoft.Json"
using System.Data;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
//Updated from VSCode to GitHub to verify CI Pipeline works.
public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    log.LogInformation("C# HTTP trigger function processed a request.");

    string input = req.Query["input"];
    bool correct_syntax = false;
    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    dynamic data = JsonConvert.DeserializeObject(requestBody);
    string responseMessage = "";
    double result = 0;
    input = input ?? data?.input;
    try {
        result = Convert.ToDouble(new DataTable().Compute(input, null)); 
        correct_syntax = true;
    }
    catch {
        correct_syntax = false;
    }
   
    if(!string.IsNullOrEmpty(input)) {
        if(correct_syntax) {
            responseMessage = $"{result}";
        }
        else {
            responseMessage = "Du har angett matematisk syntax som inte stöds för denna kalkylator:\n+ (addition)\n- (subtraktion)\n* (multiplikation)\n/ (division)\n% (modul)";
        }
    }
    else {
        responseMessage = "Vänligen ange en input-parameter i följande syntax:\nEx: \"Input:\" \"5+5\"";
    }
            return new OkObjectResult(responseMessage);
}
