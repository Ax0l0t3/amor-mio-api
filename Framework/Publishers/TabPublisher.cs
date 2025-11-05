using Framework.Interfaces;

namespace Framework.Publishers
{
    public class TabPublisher : ISubject
    {
        public string NewPrinterName { get; set; } = string.Empty;
        public string PreviousPrinterName { get; set; } = string.Empty;
        private List<IObserver> observers = new List<IObserver>();

        public void Attach(IObserver currentObserver)
        {
            observers.Add(currentObserver);
        }

        public void Detach(IObserver currentObserver)
        {
            observers.Remove(currentObserver);
        }

        public void DetachAll()
        {
            observers.Clear();
        }

        public async Task Notify()
        {
            foreach (var thisOberver in observers)
            {
                thisOberver.Update(this);
            }
        }

        public void ResetStates()
        {
            NewPrinterName = string.Empty;
            PreviousPrinterName = string.Empty;
        }

        public void SetPrintersNames(string previousName, string currentName)
        {
            NewPrinterName = currentName;
            PreviousPrinterName = previousName;
        }
    }
}