using CoreServer.Components;
using MessengerService.Requests;
using MessengerService.Responses;
using System;
using System.Net.Sockets;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server started!");
            CoreServer.MainProc.Instance.receivedRequestEvent += ProcessReceivedRequest;
            CoreServer.MainProc.Instance.Start();
            Console.ReadKey();
        }

        public static void ProcessReceivedRequest(Socket handler, string request)
        {
            try
            {
                ResponseBase response = null;
                var msg = request.Replace("<EOF>", string.Empty);

                var requestServerVersion = RequestServerVersion.Deserialize(msg);
                if (requestServerVersion != null)
                    response = ProcessTestRequest(requestServerVersion);

                //Send(handler, response.Serialize());
            }
            catch (Exception ex)
            {
                LoggingComponent.Log($"{ex.Message} - Stack: {ex.StackTrace}", LogType.Error);
            }
            finally
            {
                LoggingComponent.Log($"Request received from: {handler}", LogType.Succes);
            }
        }

        public static ResponseBase ProcessTestRequest(RequestServerVersion requestServerVersion)
        {
            ResponseServerVersion response = new ResponseServerVersion();
            response.RequestIdentifier = requestServerVersion.RequestIdentifier;

            try
            {
                response.ResponseStatus = ResponseStatus.Ok;
                response.Version = "v1.0.0";
                LoggingComponent.Log(response.Serialize(), LogType.Succes);
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseStatus = ResponseStatus.Error;
                response.Novelty = "Error processing the request.";

                LoggingComponent.Log(ex.Message, LogType.Error);
                return response;
            }
        }

    }
}
