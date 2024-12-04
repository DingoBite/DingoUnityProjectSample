using System;
using System.Threading.Tasks;
using ProjectAppStructure.Core.Model;
using UnityEngine;

namespace ProjectAppStructure.SceneRoot
{
    public class ExternalModelDependElements : MonoBehaviour, IDisposable
    {
        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public Task BindAsync(AppModelRoot appModelRoot)
        {
            return Task.CompletedTask;
        }

        public Task FinalizeAsync()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}