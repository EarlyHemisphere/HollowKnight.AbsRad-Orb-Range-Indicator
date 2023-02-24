using System.Diagnostics;
using System.Reflection;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

public class OrbRangeIndicators : Mod, ITogglableMod {
    public static OrbRangeIndicators instance;
    private GameObject absRad = null;

    public OrbRangeIndicators() : base("Orb Range Indicators") { }

    public override void Initialize() {
        Log("Initalizing.");

        instance = this;
        USceneManager.activeSceneChanged += InitiateRadiancePolling;
        if (USceneManager.GetActiveScene().name == "GG_Radiance") {
            ModHooks.HeroUpdateHook += FindRadiance;
        }

        Log("Initialized");
    }

    public override string GetVersion(){
        return FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(OrbRangeIndicators)).Location).FileVersion;
    }

    private void InitiateRadiancePolling(Scene _, Scene to) {
        if (to.name == "GG_Radiance") {
            ModHooks.HeroUpdateHook += FindRadiance;
        } else {
            if (absRad != null) {
                Indicators indicators = absRad.GetComponent<Indicators>();
                if (indicators != null) {
                    indicators.Unload();
                    GameObject.DestroyImmediate(indicators);
                }
            }
            this.absRad = null;
        }
    }

    private void FindRadiance() {
        GameObject absRad = GameObject.Find("Absolute Radiance");
        if (absRad != null) {
            absRad.AddComponent<Indicators>();
            this.absRad = absRad;
            ModHooks.HeroUpdateHook -= FindRadiance;
        }
    }

    public void Unload() {
        if (absRad != null) {
            Indicators indicators = absRad.GetComponent<Indicators>();
            if (indicators != null) {
                indicators.Unload();
                GameObject.DestroyImmediate(indicators);
            }
        }
        ModHooks.HeroUpdateHook -= FindRadiance;
        USceneManager.activeSceneChanged -= InitiateRadiancePolling;
    }
}
