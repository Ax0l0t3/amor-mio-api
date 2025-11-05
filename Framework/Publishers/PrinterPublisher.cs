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

        public async Task DetachAll()
        {
            observers.Clear();
        }

        public async Task Notify()
        {
            foreach (var thisOberver in observers)
            {
                await thisOberver.Update(this);
            }
        }

        public void SetPrinterService(string message)
        {
            var messegaByPrinters = message.Split('\n');
            PrintMessages = messegaByPrinters.ToList();
        }
    }
}