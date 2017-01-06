using System.ComponentModel.DataAnnotations;

namespace ShaverToolsShop.Conventions.Enums
{
    public enum SubscriptionStatus
    {
        [Display(Name = "Подписка начата")]
        Started = 10,
        [Display(Name = "Подписка остановлена")]
        Stopped = 20
    }
}