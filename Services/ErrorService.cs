using Models;

namespace Services
{
    public class ErrorService
    {
        private const string DateTimeValue = "dd/MM/yyyy HH:mm:ss";

        public static Object CatchService2(string method, string error, object? result, DateTime startTime)
        {
            ErrorModel errorModel = new ErrorModel
            {
                message = error,
                method = method
            };
            string timestamp = DateTime.UtcNow.ToString(DateTimeValue);
            string duration = (DateTime.UtcNow - startTime).TotalSeconds.ToString("0.00") + "ms";
            string level = "ERROR";
            string context = $"Detalle del error: {error}";

            string errorMessage = $"{timestamp} | {method} | {level} | {duration} | {result} | {level} | {context}";
            Console.WriteLine(errorMessage);
            return errorModel;
        }

        public static Object CatchService2Async(string searchId, string method, string usuario, string error, DateTime startTime)
        {
            ErrorModel errorModel = new ErrorModel();
            errorModel.message = error;
            errorModel.method = method;
            PrintError(error, searchId, usuario, method, startTime);
            return errorModel;
        }

        public static void PrintError(string error, string searchId, string usuario, string method, DateTime startTime)
        {
            string timestamp = DateTime.UtcNow.ToString(DateTimeValue);
            string duration = (DateTime.UtcNow - startTime).TotalSeconds.ToString("0.00") + "seg";

            string logmessage = $"{timestamp} | ERROR | Id: {searchId} | Usuario: {usuario} | Finaliza Log: {method} | Duracion: {duration} | Detalle del error: {error}";

            Console.WriteLine(logmessage);
        }

        public static void PrintLogStartRequest(string searchId, string method, string url, string usuario, object request)
        {
            string timestamp = DateTime.Now.ToString(DateTimeValue);

            string logmessage = $"{timestamp} | INFO | Id: {searchId} | Usuario: {usuario} | Inicia log: {method} | url: {url} | Request: {request}";
            Console.WriteLine(logmessage);
        }

        public static void PrintLogEndRequest(string searchId, string method, DateTime startTime, string usuario, string response)
        {
            string timestamp = DateTime.UtcNow.ToString(DateTimeValue);
            string duration = (DateTime.UtcNow - startTime).TotalSeconds.ToString("0.00") + "seg";
            string type = "INFO";

            string logmessage = $"{timestamp} | {type} | Id: {searchId} | Usuario: {usuario} | Finaliza log: {method} | Duracion: {duration} | Response: {response}";
            Console.WriteLine(logmessage);
        }
    }
}
