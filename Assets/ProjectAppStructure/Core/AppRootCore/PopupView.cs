using System.Collections.Generic;
using System.Threading.Tasks;
using AppStructure;
using AppStructure.BaseElements;
using DingoUnityExtensions.Extensions;
using DingoUnityExtensions.UnityViewProviders.Core;
using ProjectAppStructure.Core.Model;
using ProjectAppStructure.SceneRoot;
using UnityEngine;

namespace ProjectAppStructure.Core.AppRootCore
{
    public abstract class PopupView : GenericAnimatableAppStateView<AppPopup, AppModelRoot, AppCoreConfig>
    {
        [SerializeField] private List<EventContainer> _closeButtons;

        public override void PreInitialize()
        {
            _closeButtons.ForEach(e => e.SafeSubscribe(CloseLast));
            SetDefaultValues();
        }
        protected void CloseLast() => AppController.AppPopupController.CloseAsync();
    }

    public abstract class PopupStateElementBehaviour : StateViewElement<AppPopup, AppModelRoot, AppCoreConfig>
    {
        public override Task EnableElementAsync(TransferInfo<AppPopup> transferInfo) => Task.CompletedTask;
        public override void OnCompletelyDisable() { }
    }
}