using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectAppStructure.Core.AppRootCore;
using UnityEngine;

namespace ProjectAppStructure.Core.Model
{
    public abstract class AppModelBase { }

    public abstract class HardLinkAppModelBase : AppModelBase
    {
        public virtual Task PostInitialize(AppCoreConfig appCoreConfig, ExternalDependencies externalDependencies)
        {
            return Task.CompletedTask;
        }

        public virtual Task AfterAppStaticDataInitializeAsync(AppStaticData appStaticData)
        {
            return Task.CompletedTask;
        }
    }
    
    public class AppModelRoot : IDisposable
    {
        public readonly AppStaticData AppStaticData;

        public readonly ExternalDependencies ExternalDependencies;
        public readonly AppCoreConfig AppCoreConfig;
        public readonly AppViewModelConfig AppViewModelConfig;

        private readonly Dictionary<Type, AppModelBase> _appModelBases = new();

        public AppModelRoot(AppCoreConfig appCoreConfig, AppViewModelConfig appViewModelConfig, ExternalDependencies externalDependencies)
        {
            AppCoreConfig = appCoreConfig;
            AppViewModelConfig = appViewModelConfig;
            AppStaticData = new AppStaticData(appCoreConfig);
            ExternalDependencies = externalDependencies;
        }

        public void RegisterModel<T>(T model) where T : AppModelBase
        {
            try
            {
                _appModelBases.Add(typeof(T), model);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public T Model<T>() where T : AppModelBase
        {
            if (!_appModelBases.TryGetValue(typeof(T), out var modelBase))
                return default;

            return modelBase as T;
        }

        public async Task PostInitializeAsync()
        {
            foreach (var (type, appModelBase) in _appModelBases)
            {
                try
                {
                    if (appModelBase is HardLinkAppModelBase hardLinkAppModelBase)
                        await hardLinkAppModelBase.PostInitialize(AppCoreConfig, ExternalDependencies);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Cannot post initialize {type}");
                    Debug.LogException(e);
                }
            }
        }

        public async Task InitializeAppStaticDataAsync()
        {
            try
            {
                await AppStaticData.InitializeAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }


            foreach (var (type, appModelBase) in _appModelBases)
            {
                try
                {
                    if (appModelBase is HardLinkAppModelBase hardLinkAppModelBase)
                        await hardLinkAppModelBase.AfterAppStaticDataInitializeAsync(AppStaticData);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Cannot static data initialize {type}");
                    Debug.LogException(e);
                }
            }
        }

        public void Dispose()
        {
            AppStaticData.Dispose();
        }
    }
}