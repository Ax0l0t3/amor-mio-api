namespace Framework.Models
{
    public class TicketClass
    {
        public string NowDate { get; set; } = string.Empty;
        public List<DishAdded> PrintedObjects { get; set; } = [];
    }

    public class DishAdded
    {
        public bool Favourite { get; set; }
        public List<string> Extras { get; set; } = [];
        public List<string> ExtrasToGo { get; set; } = [];
        public List<string> Ingredients { get; set; } = [];
        public List<string> IngsToGo { get; set; } = [];
        public string Comments { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Order { get; set; } = string.Empty;
        public string Printer { get; set; } = string.Empty;
        public string Tab { get; set; } = string.Empty;
    }

    public class PrintedTickets
    {
        public List<TicketClass> PrintedOrders { get; set; } = [];
    }
}