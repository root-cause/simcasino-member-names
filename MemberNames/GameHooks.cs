using Agent;
using HarmonyLib;

namespace MemberNames
{
    [HarmonyPatch]
    public class GameHooks
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Customer), "Hover_Heading")]
        public static bool GetCustomerName(ref string __result, Customer __instance)
        {
            if (__instance.IsVIP || !__instance.IsMember || __instance.RepeatCustomerData.Name == "Bob Default")
            {
                return true;
            }

            __result = $"{__instance.RepeatCustomerData.Name} ({I18n.Get($"UI.VisitorTypes.{__instance.visitorType}")}): {I18n.GetFormat("UI.GuestExperience.Averages.Hours", "", (GameTimer.Minute - __instance.MinuteArrived) / 60)}";
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Customer), "ISelectable_SelectedEnter")]
        public static void AddRenameButton(Customer __instance, UITab tab)
        {
            if (__instance.IsVIP || !__instance.IsMember)
            {
                return;
            }

            tab.AddButton("Rename", () =>
            {
                __instance.RepeatCustomerData.RandomizeName(__instance.gender);
            }, tabCategory: UITab.UITabCategories.Guests);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(RepeatCustomer), MethodType.Constructor, new[] { typeof(Customer) })]
        public static void RenameCustomer(RepeatCustomer __instance, Customer template)
        {
            __instance.RandomizeName(template.gender);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(RepeatCustomer), "Return")]
        public static void RenameExistingCustomer(RepeatCustomer __instance, Customer customer)
        {
            if (__instance.Name == "Bob Default")
            {
                __instance.RandomizeName(customer.gender);
            }
        }
    }
}
