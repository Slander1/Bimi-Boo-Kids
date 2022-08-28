using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
[RequireComponent(typeof(DragHandler))]

public class Item : MonoBehaviour
{
    public int groupID;
}
