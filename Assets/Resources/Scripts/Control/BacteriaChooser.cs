using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FunctionsController;

public class BacteriaChooser : MonoBehaviour
{
    public float radiusOfSelecting;
    public GameObject inspectionPanel;
    public GameObject previewPanel;
    public GameObject bacteriaPrefab;

    public static bool inspectionPanelActive = false;
    public static GameObject choosedItem;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Physics2D.OverlapCircle(GetMouseWorldPosition(), radiusOfSelecting))
        {
            choosedItem = Physics2D.OverlapCircle(GetMouseWorldPosition(), radiusOfSelecting).gameObject;
            TryToUpdate();
        }

        if(Input.GetMouseButtonDown(1) && Physics2D.OverlapCircle(GetMouseWorldPosition(), radiusOfSelecting) == null)
        {
            Vector3 spawnPosition = GetMouseWorldPosition();
            spawnPosition.z = 0f;
            Instantiate(bacteriaPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void TryToUpdate()
    {
        if (inspectionPanelActive)
        {
            UpdateInpectionPanel();
        }
    }

    void UpdateInpectionPanel()
    {
        if (choosedItem)
        {
            foreach (Transform toTrash in inspectionPanel.transform)
            {
                Destroy(toTrash.gameObject);
            }

            previewPanel.GetComponent<ItemPreview>().UpdateSpriteRenderers();
            GetVariablesToPanel(choosedItem, inspectionPanel, previewPanel);
        }
    }

    //Delete after tests
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(GetMouseWorldPosition(), radiusOfSelecting);
    }
}
