using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

public static class RemoteManager {
    private struct userAttributes { }
    private struct appAttributes { }
    private static async Task InitializeRemoteConfigAsync() {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn) {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static async Task Initialize() {
        Debug.Log("A");
        
        if (Utilities.CheckForInternetConnection()) {
            await InitializeRemoteConfigAsync();
        }

        RemoteConfigService.Instance.FetchCompleted += response => Debug.Log("B"); ;
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
    }


    public static int GetInt(string key) {
        return RemoteConfigService.Instance.appConfig.GetInt(key);
    }
    
    public static float GetFloat(string key) {
        return RemoteConfigService.Instance.appConfig.GetFloat(key);
    }
    
    public static bool GetBool(string key) {
        return RemoteConfigService.Instance.appConfig.GetBool(key);
    }
}