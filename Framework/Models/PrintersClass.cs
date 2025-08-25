namespace Framework.Models
{
    public class PrintersClass
    {
        public List<Printer> Printers { get; set; } = [];
    }

    public class Printer
    {
        public string Name { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
    }
}