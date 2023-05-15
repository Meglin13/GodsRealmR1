using ObjectPooling;
using UnityEngine;

public class DistractorScript : MonoBehaviour, ITarget
{
    [SerializeField]
    private byte priority = 0;
    public byte Priority { get => priority; }
}