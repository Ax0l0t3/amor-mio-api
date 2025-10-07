using Framework.Interfaces;
using Framework.Models;
using Framework.Publishers;

namespace Framework.Observers
{
    public class OptionTabObserver : IObserver
    {
        private readonly OptionTab _tab;
        public OptionTabObserver(OptionTab tab)
        {
            _tab = tab;
        }

        public void Update(ISubject subject)
        {
            var previousState = (subject as TabPublisher).PreviousPrinterName;
            var currentState = (subject as TabPublisher).NewPrinterName;
            if (_tab.Printer == previousState)
            {
                _tab.Printer = currentState;
            }
        }
    }
}