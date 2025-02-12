using System;
using AppStructure.InputLocker;
using DingoUnityExtensions.UnityViewProviders.Core;
using UnityEngine;

namespace ProjectAppStructure.Core
{
    [Flags]
    public enum AppInputLockConfigure
    {
        None = 0,
        ShowPreloader = 1 << 0,
    }
    
    public struct AppInputLockMessage
    {
        public readonly AppInputLockConfigure ConfigureFlags;
        public AppInputLockMessage(AppInputLockConfigure configureFlags)
        {
            ConfigureFlags = configureFlags;
        }
    }
    
    public class AppInputLocker : AppInputLocker<AppInputLockMessage>
    {
        [SerializeField] private ValueContainer<AppInputLockMessage> _view;

        public override void Initialize()
        {
            if (_view != null)
                _view.SetDefaultView();
            base.Initialize();
        }

        protected override void OnLockEnable(AppInputLockMessage lockMessage)
        {
            if (_view != null)
                _view.UpdateValueWithoutNotify(lockMessage);
        }

        protected override void OnLockDisable()
        {
            if (_view != null)
                _view.UpdateValueWithoutNotify(new AppInputLockMessage(AppInputLockConfigure.None));
        }
    }
}