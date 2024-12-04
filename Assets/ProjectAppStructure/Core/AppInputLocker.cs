using AppStructure.InputLocker;

namespace ProjectAppStructure.Core
{
    public struct AppInputLockMessage
    {
    }
    
    public class AppInputLocker : AppInputLocker<AppInputLockMessage>
    {
        protected override void OnLockEnable(AppInputLockMessage lockMessage)
        {
        }

        protected override void OnLockDisable()
        {
        }
    }
}