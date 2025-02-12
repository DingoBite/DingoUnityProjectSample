using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DingoUnityExtensions.Serialization;
using ProjectAppStructure.Core.AppRootCore;
using UnityEngine;
using UnityEngine.Scripting;

namespace ProjectAppStructure.SceneRoot
{
    [Serializable, Preserve]
    public class AppBootstrapConfig
    {
        public bool ExternalAppCoreConfig;
    }
    
    public class AppBoostrap : AppStructure.AppBootstrap
    {
        [SerializeField] private AppBootstrapConfig _appBootstrapConfig;
        [SerializeField] private AppCoreConfig _appCoreConfig;
        [SerializeField] private AppViewModelConfig _appViewConfig;
        [SerializeField] private AppController _appController;

        public AppCoreConfig AppCoreConfig => _appCoreConfig;
        
        protected IEnumerator LoadConfigExternal(Action<AppCoreConfig> callback)
        {
            callback?.Invoke(JsonClone.CloneSerializable(_appCoreConfig));
            yield break;
        }
        
        protected override IEnumerator PrepareBootstrapProcess(Action<bool> callback)
        {
            if (_appBootstrapConfig.ExternalAppCoreConfig)
                yield return LoadConfigExternal(c => _appCoreConfig = c);
            _appController.PrepareController();
            yield return _appController.InitializeControllerAsync(_appCoreConfig, _appViewConfig, callback).ToCoroutine();
        }

        protected override IEnumerator StartBootstrapProcess(Action<bool> callback)
        {
            yield return _appController.BindAsync(callback).ToCoroutine();
        }

        protected override IEnumerator FinalizeBootstrapProcess(Action<bool> callback)
        {
            yield return _appController.FinalizeAsync(callback).ToCoroutine();
        }
    }
}