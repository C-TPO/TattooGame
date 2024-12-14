using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TattooClient
{
    public string ClientName;
    public float PainSensitivity => painSensitivity;
    public float PainRecoveryRate => painRevoceryRate;

    private float painSensitivity = 1.0f;
    private float painRevoceryRate = -1.0f;

    public void Init(string _name)
    {
        ClientName = _name;
        painSensitivity = Random.Range(.5f, 2.5f);
        painRevoceryRate = Random.Range(-1.5f, -2.5f);
    }
}
