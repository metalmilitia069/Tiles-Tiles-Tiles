using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{


    public Tile tilePlaceholder;

    #region Singleton

    public static GridManager instance = null;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Theres More than One GridManager in the Scene!!!");
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

    public void GetCurrentTile(GameObject characterStandingOnTile)
    {
        RaycastHit hit;
        Debug.DrawRay(characterStandingOnTile.transform.position, Vector3.down);
        if (Physics.Raycast(characterStandingOnTile.transform.position, Vector3.down, out hit, 1))
        {
            tilePlaceholder = hit.collider.transform.GetComponent<Tile>();

            tilePlaceholder.isCurrent = true;
        }
    }

    public void CalculateAvailablePath(GameObject character)
    {
        GetCurrentTile(character);
    }
}
