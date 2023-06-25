using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace IgorTime.BurstedFlowField.Jobs
{
    [BurstCompile]
    public struct ResetIntegrationFieldJob : IJobParallelFor
    {
        [WriteOnly] 
        public NativeArray<ushort> integrationField;

        public void Execute(int index)
        {
            integrationField[index] = ushort.MaxValue;
        }
    }
}