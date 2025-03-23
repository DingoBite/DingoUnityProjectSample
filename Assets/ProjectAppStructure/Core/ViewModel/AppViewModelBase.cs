using System;
using System.Collections.Generic;
using ProjectAppStructure.Core.AppRootCore;
using ProjectAppStructure.Core.Model;
using UnityEngine;

namespace ProjectAppStructure.Core.ViewModel
{
    public abstract class AppViewModelBase
    {
        protected readonly AppModelRoot AppModelRoot;

        public AppViewModelBase(AppModelRoot appModelRoot)
        {
            AppModelRoot = appModelRoot;
        }

        protected ExternalDependencies ExternalDependencies => AppModelRoot.ExternalDependencies;

        protected void Log(string message) => ExternalDependencies.LogDependencies.UnityLogWrap(() => Debug.Log(message));
        protected void LogError(string message) => ExternalDependencies.LogDependencies.UnityLogWrap(() => Debug.LogError(message));
        protected void LogException(Exception exception) => ExternalDependencies.LogDependencies.UnityLogWrap(() => Debug.LogException(exception));
    }
    
    public class AppViewModelRoot : AppModelBase
    {
        private readonly Dictionary<Type, AppViewModelBase> _appViewModelBases = new();
        
        public void RegisterModel<T>(T model) where T : AppViewModelBase
        {
            try
            {
                _appViewModelBases.Add(typeof(T), model);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public T Model<T>() where T : AppViewModelBase
        {
            if (!_appViewModelBases.TryGetValue(typeof(T), out var modelBase))
                return default;

            return modelBase as T;
        }
    }
}