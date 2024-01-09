using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInventory : MonoBehaviour
{
    public List<GameObject> instruments; // List of instruments in the ship's inventory
    [SerializeField]private int currentInstrumentIndex = 0; // Index of the currently selected instrument

    void Update()
    {
        // Toggle instruments with number keys (1 to 9)
        for (int i = 0; i <= instruments.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SwitchInstrument(i);
            }
        }
    }

    void SwitchInstrument(int newIndex)
    {
        // Deactivate the current instrument
        if (currentInstrumentIndex < instruments.Count)
        {
            instruments[currentInstrumentIndex].SetActive(false);
        }

        // Activate the new instrument
        if (newIndex < instruments.Count)
        {
            instruments[newIndex].SetActive(true);
            currentInstrumentIndex = newIndex;
        }
    }
}
