using System.Threading.Tasks;
using AppStructure;
using AppStructure.BaseElements;
using ProjectAppStructure.Core.Model;

namespace ProjectAppStructure.Core.AppRootCore
{
    public abstract class AppStateBehaviour : ImmediateAppStateView<AppCoreState, AppModelRoot, AppCoreConfig> { }
    
    public abstract class AppStateElementBehaviour : StateViewElement<AppCoreState, AppModelRoot, AppCoreConfig>
    {
        public override Task EnableElementAsync(TransferInfo<AppCoreState> transferInfo) => Task.CompletedTask;
        public override void OnStartScreenEnable() { }
        public override Task DisableElementAsync(TransferInfo<AppCoreState> transferInfo) => Task.CompletedTask;
        public override void OnCompletelyDisable() { }
    }

    public abstract class AppGeneralElementBehaviour : GeneralViewElement<AppModelRoot, AppCoreConfig> { }

    public abstract class AppStateStaticElementBehaviour : StaticStateViewElement<AppCoreState, AppModelRoot, AppCoreConfig>
    {
        public override void Transfer(TransferInfo<AppCoreState> transferInfo)
        {
        }
    }
}