using System.Threading.Tasks;
using AppStructure.BaseElements;
using ProjectAppStructure.Core.AppRootCore;
using ProjectAppStructure.Core.Model;
using UnityEngine;

namespace ProjectAppStructure.Core
{
    public interface IAppStateController
    {
        public Task ForceGoToStateAsync(AppCoreState appState);
    }
    
    public class AppStateController : MonoBehaviour, IAppStateController
    {
        [SerializeField] private AppStateMachine _appCoreStateMachine;
        [SerializeField] private AppStateElementsRoot _appStateElementsRoot;

        [SerializeField] private AppCoreState _bootstrapState = AppCoreState.Bootstrap;
        [SerializeField] private AppCoreState _startState = AppCoreState.Start;
        [SerializeField] private bool _autoGoToStart = true;
            
        public IAppStructurePart<AppModelRoot, AppCoreConfig> AppViewRoot => _appStateElementsRoot;

        public void Initialize(AppCoreConfig config)
        {
            _startState = config.StartState;
        } 
        
        public async Task GoToBootstrap()
        {
            var t = _appCoreStateMachine.GoToState(_bootstrapState);
            await _appStateElementsRoot.ApplyTransferAsync(t);
        }

        public async Task GoToStart()
        {
            if (!_autoGoToStart)
                return;
            var t = _appCoreStateMachine.GoToState(_startState);
            await _appStateElementsRoot.ApplyTransferAsync(t);
        }

        public async Task ForceGoToStateAsync(AppCoreState appState)
        {
            var t = _appCoreStateMachine.GoToState(appState);
            await _appStateElementsRoot.ApplyTransferAsync(t);
        }
    }
}