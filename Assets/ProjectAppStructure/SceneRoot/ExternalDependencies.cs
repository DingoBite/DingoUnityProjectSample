using System;
using ProjectAppStructure.Core.GeneralUtils;
using UnityEngine;

namespace ProjectAppStructure.SceneRoot
{
    public class ExternalDependencies : MonoBehaviour
    {
        public Core.Model.ExternalDependencies CollectDependencies()
        {
            return new Core.Model.ExternalDependencies(UpdateAndCoroutineUtils.MakeRuntimeDependencies(), new LogDependencies(UnityLogInserted));
        }

        private void UnityLogInserted(Action logAction)
        {
            try
            {
                logAction();
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }
    }
}