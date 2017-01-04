using System.ComponentModel.DataAnnotations;

namespace ShaverToolsShop.Conventions.Enums
{
    public enum SubscriptionType
    {
        [Display(Name = "Раз в два месяца")]
        OnceInTwoMonths = 10,
        [Display(Name = "Раз в месяц")]
        OnceInMonth = 20,
        [Display(Name = "Два раза в месяц")]
        TwiceInMonth = 30
    }
}