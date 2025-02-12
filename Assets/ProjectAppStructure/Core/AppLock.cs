using System;
using System.Collections;
using System.Threading.Tasks;
using DingoUnityExtensions;
using ProjectAppStructure.SceneRoot;
using UnityEngine;

namespace ProjectAppStructure.Core
{
    public static class AppLock
    {
        public static void AppSyncLockTime(float time, Action callback = null, ushort lockFlag = 0, AppInputLockMessage lockMessage = default)
        {
            if (lockMessage.ConfigureFlags == AppInputLockConfigure.None)
                lockMessage = new AppInputLockMessage(AppInputLockConfigure.ShowPreloader);
            AppController.AppInputLocker.Enable(lockMessage, lockFlag);
            try
            {
                CoroutineParent.InvokeAfterSecondsWithCanceling(AppController.AppInputLocker, time, () =>
                {
                    AppController.AppInputLocker.Disable(lockFlag);
                    callback?.Invoke();
                });
            }
            catch (Exception e)
            {
                AppController.AppInputLocker.Disable(lockFlag);
                Debug.LogException(e);
            }
        }
        
        public static async void AppAsyncLockAction(Func<Task> awaitFunc, Action callback = null, ushort lockFlag = 0, AppInputLockMessage lockMessage = default)
        {
            if (lockMessage.ConfigureFlags == AppInputLockConfigure.None)
                lockMessage = new AppInputLockMessage(AppInputLockConfigure.ShowPreloader);
            AppController.AppInputLocker.Enable(lockMessage, lockFlag);
            try
            {
                await awaitFunc();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            AppController.AppInputLocker.Disable(lockFlag);
            callback?.Invoke();
        }
        
        public static IEnumerator AppAsyncLockActionCoroutine(Func<Task> awaitFunc, Action callback = null, ushort lockFlag = 0, AppInputLockMessage lockMessage = default)
        {
            if (lockMessage.ConfigureFlags == AppInputLockConfigure.None)
                lockMessage = new AppInputLockMessage(AppInputLockConfigure.ShowPreloader);
            AppController.AppInputLocker.Enable(lockMessage, lockFlag);
            Task task = null;
            try
            {
                task = awaitFunc();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            if (task != null)
            {
                while (!task.IsCompleted)
                {
                    yield return null;
                }
            }

            AppController.AppInputLocker.Disable(lockFlag);
            callback?.Invoke();
        }
    }
}