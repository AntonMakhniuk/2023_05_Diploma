using UnityEngine;

namespace Building.Buildings.Base_Classes
{
    public abstract class BuildingParent : MonoBehaviour
    {
        public BuildingType buildingType;
        public GameObject blueprintObject;
        [HideInInspector] public BaseBlueprint blueprintComponent;
        public GameObject buildingObject;
        [HideInInspector] public BaseBuilding buildingComponent;
        [HideInInspector] public BuildingState currentState;

        private void Start()
        {
            blueprintComponent = blueprintObject.GetComponent<BaseBlueprint>();
            buildingComponent = buildingObject.GetComponent<BaseBuilding>();
            
            SetState(BuildingState.Blueprint);
        }

        // If true, enables the built structure, otherwise enables the blueprint
        public void SetState(BuildingState state)
        {
            switch (state)
            {
                case BuildingState.Blueprint:
                {
                    blueprintObject.SetActive(true);
                    buildingObject.SetActive(false);
                    
                    break;
                }
                case BuildingState.Constructed:
                {
                    blueprintObject.SetActive(false);
                    buildingObject.SetActive(true);

                    break;
                }
            }

            currentState = state;
        }
    }

    public enum BuildingType
    {
        Teleporter, Accelerator, RefillStation
    }

    public enum BuildingState
    {
        Blueprint, Constructed
    }
}