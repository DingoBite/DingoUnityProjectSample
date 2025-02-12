using AppStructure.StateMachines;

namespace ProjectAppStructure.Core
{
    public class AppPopupStateMachine : OpenCloseStateMachine<AppPopup>
    {
        protected override AppPopup NoneState => AppPopup.None;
    }
}