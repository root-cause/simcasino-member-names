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

        [HarmonyPostfix]
        [HarmonyPatch(typeof(RepeatCustomer), MethodType.Constructor, new[] { typeof(Customer) })]
        public static void RenameCustomer(RepeatCustomer __instance, Customer template)
        {
            __instance.Name = NameUtility.GetName(template.gender);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(RepeatCustomer), "Return")]
        public static void RenameExistingCustomer(RepeatCustomer __instance, Customer customer)
        {
            if (__instance.Name == "Bob Default")
            {
                __instance.Name = NameUtility.GetName(customer.gender);
            }
        }
    }
}
