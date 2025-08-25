using Framework.Interfaces;

namespace Framework.Publishers
{
    public class PrinterPublisher : ISubject
    {
        public List<string> PrintMessages { get; set; } = [];
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

        public void Notify()
        {
            foreach (var thisOberver in observers)
            {
                thisOberver.Update(this);
            }
        }

        public void SetPrinterService(string message)
        {
            var messegaByPrinters = message.Split('\n');
            PrintMessages = messegaByPrinters.ToList();
        }
    }
}