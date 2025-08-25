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

        public void Update(ISubject subject)
        {
            var printerMessages = (subject as PrinterPublisher).PrintMessages;
            foreach (string msg in printerMessages)
            {
                if (msg.Contains(_printer.Name))
                {
                    PrintOverTcp_Ip(_printer.Ip, int.Parse(_printer.Port), msg);
                }
            }
        }
    }
}