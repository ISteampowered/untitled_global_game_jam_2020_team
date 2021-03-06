﻿using System.Collections;
using System.Collections.Generic;
using Logic;
using UnityEngine;

public class SnappingPoint : MonoBehaviour
{
    public bool IsTypeRestricted = false;
    public List<PartType> AcceptedTypes; 
    public PartType Preferred { get; set; }
    public GameObject AssignedPart;
    public bool IsBody = false;
    
    // Start is called before the first frame update
    void Start()
    {
        tag = "snap";
        //if (transform.parent != null)
        //    transform.localScale = transform.parent.localScale;
    }


    public SnappingPoint()
    {
        AcceptedTypes = new List<PartType>();

        // Preferred = PartType.HEAD;
    }


    public bool CanSnap(GameObject part)
    {
        var bodypartVisualComp = part.GetComponent<BodyPartVisual>();
        if (bodypartVisualComp == null)
            return false;

        bool ContainsRestrictedType = IsTypeRestricted && AcceptedTypes.Contains(bodypartVisualComp.AssignedPart.Type);
        return AssignedPart == null && (!IsTypeRestricted || ContainsRestrictedType);
    }

    public void AssignGameObject(GameObject part)
    {
        AssignedPart = part;
    }

    public void UnSnap(bool destroy = false)
    {

        AssignedPart = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
