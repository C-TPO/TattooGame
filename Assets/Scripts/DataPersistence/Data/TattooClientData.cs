using UnityEngine;

[System.Serializable]
public class TattooClientData
{
    private readonly float painSensitivity = 1.0f;
    private readonly float painRevoceryRate = -1.0f;

    #region Public API

    public string ClientName;
    public float PainSensitivity => painSensitivity;
    public float PainRecoveryRate => painRevoceryRate;

    public TattooClientData(string _name)
    {
        ClientName = _name;
        painSensitivity = Random.Range(.5f, 2.5f);
        painRevoceryRate = Random.Range(-1.5f, -2.5f);
    }

    #endregion
}