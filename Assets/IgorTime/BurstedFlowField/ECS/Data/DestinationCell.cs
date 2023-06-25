using Unity.Entities;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.Data
{
    public struct DestinationCell : IComponentData
    {
        public bool isSet;
        public int2 cellCoordinates;
    }
}