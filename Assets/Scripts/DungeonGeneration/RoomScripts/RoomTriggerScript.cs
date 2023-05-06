using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RoomTriggerScript : MonoBehaviour
{
    private RoomScript Room;
    private BoxCollider Trigger;

    private void Awake()
    {
        Room = GetComponentInParent<RoomScript>();

        Trigger = GetComponent<BoxCollider>();
        Trigger.isTrigger = true;

        Trigger.size = new Vector3(Room.Size.x - 1, 0, Room.Size.y - 1);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Character")
        {
            Room.OnRoomEnterTrigger();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Character")
        {
            Room.OnRoomExitTrigger();
        }
    }
}