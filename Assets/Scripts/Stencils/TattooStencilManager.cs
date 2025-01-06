using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class TattooStencilManager : MonoBehaviour
{
    public static TattooStencilManager instance {get; private set;}

    [SerializeField] private TattooStencilScriptableObject[] stencilList;

    #region Unity Messages

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("FOUND MULTIPLE TATTOO STENCIL MANAGERS IN SCENE! DESTROYING NEWEST INSTANCE.");
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region Public API

    public TattooStencilScriptableObject GetStencilByIndex(int index)
    {
        if(index >= stencilList.Length)
            return stencilList[0];
        
        return stencilList[index];
    }
    
    public int GetRandomStencilIndex()
    {
        return Random.Range(0, stencilList.Length-1);
    }

    #endregion
}