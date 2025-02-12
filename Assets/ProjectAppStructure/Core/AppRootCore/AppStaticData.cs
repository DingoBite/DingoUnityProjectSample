using System;
using System.Threading.Tasks;

namespace ProjectAppStructure.Core.AppRootCore
{
    public class AppStaticData : IDisposable
    {
        private readonly AppCoreConfig _config;

        public AppStaticData(AppCoreConfig config)
        {
            _config = config;
        }
        
        public void Dispose()
        {
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}