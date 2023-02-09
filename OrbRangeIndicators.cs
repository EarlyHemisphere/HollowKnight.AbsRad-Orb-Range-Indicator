using System.Diagnostics;
using System.Reflection;
using Modding;
using UnityEngine;

public class OrbRangeIndicators : Mod, ITogglableMod {
    public static OrbRangeIndicators instance;
    private static bool assigned = false;

    public OrbRangeIndicators() : base("Orb Range Indicators") { }

    public override void Initialize() {
        instance = this;

        Log("Initalizing.");
        ModHooks.HeroUpdateHook += FindRadiance;
    }


    public override string GetVersion(){
        return FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(OrbRangeIndicators)).Location).FileVersion;
    }

    private static void FindRadiance() {
        GameObject absRad = GameObject.Find("Absolute Radiance");
        if (!assigned && absRad != null) {
            absRad.AddComponent<Indicators>();
            assigned = true;
        }
    }

    public void Unload() {
        ModHooks.HeroUpdateHook -= FindRadiance;
        GameObject absRad = GameObject.Find("Absolute Radiance");
        Indicators indicators = (instance != null ? absRad != null ? absRad.GetComponent<Indicators>() : null : null);
        if (!(indicators == null))
        {
            Object.Destroy(indicators);
        }
        assigned = false;
    }
}
