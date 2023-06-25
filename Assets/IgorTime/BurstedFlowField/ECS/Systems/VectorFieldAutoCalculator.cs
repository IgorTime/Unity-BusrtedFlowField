using IgorTime.BurstedFlowField.ECS.Data;
using Unity.Entities;

namespace IgorTime.BurstedFlowField.ECS.Systems
{
    public partial struct VectorFieldAutoCalculator : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (destinationCell, vectorField) in SystemAPI.Query<
                         RefRO<DestinationCell>, 
                         RefRW<VectorFieldData>>())
            {
                if(vectorField.ValueRO.destinationCell.Equals(destinationCell.ValueRO.cellCoordinates))
                {
                    continue;
                }
                
                
            }
        }
    }
}