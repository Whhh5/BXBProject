using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Computer Asset", menuName = "System/Computer Asset")]
public class ComputerSettingAsset : ScriptableObject
{
    public string ip;
    public int port;
}
