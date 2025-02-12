using Cysharp.Threading.Tasks;
using ProjectAppStructure.Core.Model;
using ProjectAppStructure.Core.ViewModel;
using UnityEngine;

namespace ProjectAppStructure.SceneRoot
{
    public class ModelsRegistererManager : MonoBehaviour
    {
        public async UniTask RegisterModelsAsync(AppModelRoot appModelRoot)
        {
            appModelRoot.RegisterModel(new AppPopupMessageModel());

            await RegisterViewModelAsync(appModelRoot);
        }

        public UniTask RegisterViewModelAsync(AppModelRoot appModelRoot)
        {
            var appViewModelRoot = new AppViewModelRoot();
            appModelRoot.RegisterModel(appViewModelRoot);
            
            return UniTask.CompletedTask;
        }
    }
}