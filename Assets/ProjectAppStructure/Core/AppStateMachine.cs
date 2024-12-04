using System.Collections.Generic;
using System.Threading.Tasks;
using AppStructure;
using AppStructure.StateMachines;

namespace ProjectAppStructure.Core
{
    public class AppStateMachine : GoBackSupportStateMachine<AppCoreState>
    {
        protected override AppCoreState NoneState => AppCoreState.None;

        protected override bool IsValidBack(IReadOnlyList<TransferInfo<AppCoreState>> history, out TransferInfo<AppCoreState> firstBack)
        {
            firstBack = default;
            return false;
        }
    }
}