using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AutoDebugger.Plugins
{
    internal class SendHttpGetRequestPlugin : DiagnosticPlugin
    {
        private readonly HttpClient _client = new();

        /// <summary>
        /// Wait a given amount of seconds
        /// </summary>
        [KernelFunction, Description("Send http request to specific endpoint")]
        public async Task<string> SendGetRequest([Description("The URI for the http get request")] string requestUri)
        {
            // string url = "http://127.0.0.1:port/controller/method/?var=12345";
            try
            {
                var response = await _client.GetAsync(requestUri);
                var result = $"Status code: {response.StatusCode}{Environment.NewLine}Content: {await response.Content.ReadAsStringAsync()}";

                CaptureCall(nameof(SendGetRequest),
                    result,
                    new Dictionary<string, object?>
                    {
                        {nameof(requestUri), requestUri}
                    });

                return result;
            }
            catch (Exception e)
            {
                var errorReturn = $"There was error when trying to send http get request. Error: {e.Message}";
                CaptureCall(nameof(SendGetRequest),
                    errorReturn,
                    new Dictionary<string, object?>
                    {
                        {nameof(requestUri), requestUri}
                    });

                return errorReturn;
            }
        }
    }
}
