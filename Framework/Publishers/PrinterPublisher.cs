using Framework.Interfaces;

namespace Framework.Publishers
{
    public class PrinterPublisher : ISubject
    {
        public List<string> PrintMessages { get; set; } = [];
        private List<IObserver> observers = new List<IObserver>();

        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
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