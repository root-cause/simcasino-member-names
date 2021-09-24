using System.Reflection;
using HarmonyLib;
using SimCasino.Modding;

namespace MemberNames
{
    public class Main : BaseMod
    {
        public override string InternalName => "root.MemberNames";

        public override void OnLoad(GameEnvironment gameState)
        {
            Harmony harmony = new Harmony(InternalName);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
