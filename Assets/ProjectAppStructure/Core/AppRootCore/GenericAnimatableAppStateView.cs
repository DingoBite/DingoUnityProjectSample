using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStructure;
using DingoUnityExtensions.Tweens;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectAppStructure.Core.AppRootCore
{
    public class GenericAnimatableAppStateView<TState, TAppModel, TAppConfig> : AppStateView<TState, TAppModel, TAppConfig> where TState : Enum
    {
        [Foldout("Events")]
        [SerializeField] private UnityEvent _startEnable;
        [Foldout("Events")]
        [SerializeField] private UnityEvent _fullEnable;
        [Foldout("Events")]
        [SerializeField] private UnityEvent _startDisable;
        [Foldout("Events")]
        [SerializeField] private UnityEvent _fullDisable;
        
        [SerializeField] private List<AnimatableBehaviour> _animatableBehaviours;
            
        private Canvas _canvas;

        protected Canvas Canvas => _canvas ??= GetComponent<Canvas>();

        private List<AnimatableBehaviour> AnimatableBehaviours => _animatableBehaviours;

        protected override void StartEnable()
        {
            _startEnable?.Invoke();
            if (Canvas != null)
                Canvas.enabled = true;
            base.StartEnable();
        }

        public override Task DisableOnTransferAsync(TransferInfo<TState> transferInfo)
        {
            _startDisable?.Invoke();
            if (AnimatableBehaviours.Count == 0)
                DisableCompletely();
            else
                AnimatableBehaviours.ForEach(b => b.Disable(onComplete: DisableAllHandle));
            return base.DisableOnTransferAsync(transferInfo);
        }

        protected override void DisableCompletely()
        {
            base.DisableCompletely();
            if (Canvas != null)
                Canvas.enabled = false;
            SetDefaultValues();
            _fullDisable?.Invoke();
        }

        protected virtual void EnableCompletely()
        {
            _fullEnable?.Invoke();
        }
        
        public override Task EnableOnTransferAsync(TransferInfo<TState> transferInfo)
        {
            StartEnable();
            AnimatableBehaviours.ForEach(b => b.Enable(onComplete:EnableFirstHandle));
            return base.EnableOnTransferAsync(transferInfo);
        }

        private void DisableAllHandle()
        {
            if (AnimatableBehaviours.Any(animatableBehaviour => !animatableBehaviour.IsFullyDisabled))
                return;

            DisableCompletely();
        }

        private void EnableFirstHandle()
        {
            if (AnimatableBehaviours.Any(animatableBehaviour => !animatableBehaviour.IsFullyEnabled))
                return;
            EnableCompletely();
        }
        
        protected override void SetDefaultValues()
        {
            IsActive = false;
            
            if (Canvas != null)
                Canvas.enabled = false;

            gameObject.SetActive(false);
            AnimatableBehaviours.ForEach(b => b.DisableImmediately());
        }
    }
}