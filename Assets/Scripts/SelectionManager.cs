using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [Header("Reference")] 
    [SerializeField] private Camera cam;

    [Header("Material")] [SerializeField] 
    private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;
    
    [Header("Others")]
    [SerializeField] private string selectableTag = "Grabbable";
    private Transform _selection;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_selection != null)
        {
            Renderer selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            _selection = null;
        }
        
        
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        _selection = null;

        if (Physics.Raycast(ray, out hit))
        {
            Transform selection = hit.transform;

            if (selection.CompareTag(selectableTag))
            {
                Renderer selectionRenderer = selection.GetComponent<Renderer>();

                if (selectionRenderer != null)
                {
                    selectionRenderer.material = highlightMaterial;
                }

                _selection = selection;
            }
            
        }
    }
}
