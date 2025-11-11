using Framework.Interfaces;
using Framework.Models;
using Framework.Publishers;
using static Framework.Services.PrintService;

namespace Framework.Observers
{
    public class PrinterObserver : IObserver
    {
        private readonly Printer _printer;
        public PrinterObserver(Printer printer)
        {
            _printer = printer;
        }

        public async void Update(ISubject subject)
        {
            var printerPublisher = subject as PrinterPublisher;
            if (printerPublisher is not null)
            {
                var printerMessages = printerPublisher.PrintMessages;
                foreach (string msg in printerMessages)
                {
                    if (msg.Contains(_printer.Name))
                    {
                        await PrintOverTcp_Ip(_printer.Ip, int.Parse(_printer.Port), msg);
                    }
                }
            }
            else
            {
                Console.WriteLine("While updating oberver PrinterPublisher brought null");
            }
        }
    }
}