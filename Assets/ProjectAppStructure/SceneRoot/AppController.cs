using System;
using AppStructure.InputLocker;
using Cysharp.Threading.Tasks;
using DingoUnityExtensions.MonoBehaviours.Singletons;
using ProjectAppStructure.Core;
using ProjectAppStructure.Core.AppRootCore;
using ProjectAppStructure.Core.Model;
using UnityEngine;

namespace ProjectAppStructure.SceneRoot
{
    public class AppController : SingletonProtectedBehaviour<AppController>
    {
        [SerializeField] private AppInputLocker _appInputLocker;
        [SerializeField] private AppStateController _appCoreStateController;
        [SerializeField] private AppPopupStateController _appPopupStateController;
        [SerializeField] private ExternalDependencies _externalDependencies;
        [SerializeField] private ModelsRegistererManager _modelsRegistererManager;
        [SerializeField] private ExternalModelDependElements _externalModelDependElements;

        public static IAppStateController AppAppStateController => Instance._appCoreStateController;
        public static IAppPopupController AppPopupController => Instance._appPopupStateController;
        public static IAppInputLocker<AppInputLockMessage> AppInputLocker => Instance._appInputLocker;
        
        private AppModelRoot _appModel;

        public void PrepareController()
        {
            Debug.Log(nameof(PrepareController));
            _appInputLocker.Initialize();
            _appCoreStateController.AppViewRoot.PreInitialize();
            _appPopupStateController.AppPopupViewRoot.PreInitialize();
        }
        
        public async UniTask InitializeControllerAsync(AppCoreConfig config, AppViewModelConfig viewConfig, Action<bool> callback)
        {
            Debug.Log(nameof(InitializeControllerAsync));
            _appCoreStateController.Initialize(config);
            await _appCoreStateController.GoToBootstrap();
            
            _appModel = new AppModelRoot(config, viewConfig, _externalDependencies.CollectDependencies());
            await _modelsRegistererManager.RegisterModelsAsync(_appModel);
            await _appModel.PostInitializeAsync();
            await _externalModelDependElements.InitializeAsync();
            
            var initializeResult = await _appCoreStateController.AppViewRoot.InitializeAsync(config).AsUniTask();
            initializeResult |= await _appPopupStateController.AppPopupViewRoot.InitializeAsync(config).AsUniTask();
            callback?.Invoke(initializeResult);
        }

        public async UniTask BindAsync(Action<bool> callback)
        {
            Debug.Log(nameof(BindAsync));
            await _externalModelDependElements.BindAsync(_appModel);
            var result = await _appCoreStateController.AppViewRoot.BindAsync(_appModel).AsUniTask();
            result |= await _appPopupStateController.AppPopupViewRoot.BindAsync(_appModel).AsUniTask();
            callback?.Invoke(result);
        }

        public async UniTask FinalizeAsync(Action<bool> callback)
        {
            Debug.Log(nameof(FinalizeAsync));
            await _appModel.InitializeAppStaticDataAsync();
            await _externalModelDependElements.FinalizeAsync();
            var result = await _appCoreStateController.AppViewRoot.PostInitializeAsync().AsUniTask();
            result |= await _appPopupStateController.AppPopupViewRoot.PostInitializeAsync().AsUniTask();
 
            callback?.Invoke(result);
            await _appCoreStateController.GoToStart();
        }

        private void Dispose()
        {
            _appModel?.Dispose();
            if (_externalModelDependElements != null)
                _externalModelDependElements.Dispose();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
#if UNITY_EDITOR
            OnApplicationQuit();
#endif
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            Dispose();
        }
    }
}