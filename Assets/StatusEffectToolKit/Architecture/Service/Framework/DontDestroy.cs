using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private string objectID;

    private void Awake()
    {
        objectID = name + transform.position.ToString() + transform.eulerAngles.ToString();
    }

    private void Start()
    {
        for (int i = 0; i < Object.FindObjectsByType<DontDestroy>(FindObjectsSortMode.None).Length; i++)
        {
            if (Object.FindObjectsByType<DontDestroy>(FindObjectsSortMode.None)[i] != this)
            {
                if (Object.FindObjectsByType<DontDestroy>(FindObjectsSortMode.None)[i].objectID == objectID)
                {
                    Destroy(gameObject);
                }
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
