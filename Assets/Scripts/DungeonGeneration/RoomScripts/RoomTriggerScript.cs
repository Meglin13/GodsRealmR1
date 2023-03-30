using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    private void OnCollisionEnter(Collision collision)
    {
        Room.OnRoomEnterTrigger();
    }

    private void OnCollisionExit(Collision collision)
    {
        Room.OnRoomExitTrigger();
    }
}