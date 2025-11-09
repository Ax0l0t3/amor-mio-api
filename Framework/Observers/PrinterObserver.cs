using System.Net.Sockets;
using Framework.Interfaces;
using Framework.Models;
using Framework.Publishers;
using static Framework.Services.PrintService;

namespace Framework.Observers
{
    public class PrinterObserver : IObserver
    {
        private readonly Printer _printer;
        private TcpClient _client;
        public PrinterObserver(Printer printer)
        {
            _printer = printer;
        }

        public async Task Update(ISubject subject)
        {
            var printerPublisher = subject as PrinterPublisher;
            if (printerPublisher is not null)
            {
                var printerMessages = printerPublisher.PrintMessages;
                foreach (string msg in printerMessages)
                {
                    if (msg.Contains(_printer.Name))
                    {
                        await PrintOverTcp_Ip(_client, _printer.Ip, int.Parse(_printer.Port), msg);
                    }
                }
            }
            else
            {
                Console.WriteLine("While updating oberver PrinterPublisher brought null");
            }
        }

        public async void SetNewClient()
        {
            _client = new TcpClient();
            await _client.ConnectAsync(printerIp, printerPort);
        }
    }
}