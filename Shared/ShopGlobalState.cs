namespace RickGuitars.SmellyApi.Shared;

public static class ShopGlobalState
{
    // Smell: global mutable state.
    // Any service can change these values and affect the whole system.

    public static bool FreeShippingEnabled { get; set; } = false;

    public static decimal GlobalDiscountRate { get; set; } = 0.00m;

    public static string ActiveCampaignName { get; set; } = "Spring Guitar Deals";

    public static bool MaintenanceMode { get; set; } = false;
}