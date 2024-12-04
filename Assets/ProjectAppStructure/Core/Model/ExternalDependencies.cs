using ProjectAppStructure.Core.GeneralUtils;

namespace ProjectAppStructure.Core.Model
{
    public class ExternalDependencies
    {
        public readonly RuntimeDependencies RuntimeDependencies;
        public readonly LogDependencies LogDependencies;
        
        public ExternalDependencies(RuntimeDependencies runtimeDependencies, LogDependencies logDependencies)
        {
            RuntimeDependencies = runtimeDependencies;
            LogDependencies = logDependencies;
        }
    }
}