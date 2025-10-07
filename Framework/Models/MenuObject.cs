namespace Framework.Models
{
    public class MenuObject
    {
        public List<OptionTab> Tabs { get; set; } = [];
    }

    public class OptionTab
    {
        public List<Ingredients> Extras { get; set; } = [];
        public List<Ingredients> Ingredients { get; set; } = [];
        public List<Dish> Options { get; set; } = [];
        public string Title { get; set; } = string.Empty;
        public string Printer { get; set; } = string.Empty;
        public bool Selected { get; set; }
    }

    public class Ingredients
    {
        public string Category { get; set; } = string.Empty;
        public List<string> Options { get; set; } = [];
    }

    public class Dish
    {
        public string Comments { get; set; } = string.Empty;
        public List<string> Extras { get; set; } = [];
        public bool Favourite { get; set; }
        public List<string> Ingredients { get; set; } = [];
        public string Name { get; set; } = string.Empty;
    }
}