namespace Kitchen.Models;

public class KitchenOptions
{
    public const string Kitchen = "Kitchen";
    public CookingAppliances CookingAppliances { get; set; }
    public int Waiters { get; set; }
    public List<Cook> Cooks { get; set; }
    public int TimeUnit { get; set; }
    public List<MenuItem> Menu { get; set; }
}
