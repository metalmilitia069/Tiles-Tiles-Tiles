using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCalculatorManager : MonoBehaviour
{

    #region Singleton

    public static CombatCalculatorManager instance = null;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Theres More than One CombatCalculatorManager in the Scene!!!");
            Destroy(this.gameObject);
        }

        instance = this;

        //DontDestroyOnLoad(this.gameObject);        
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
