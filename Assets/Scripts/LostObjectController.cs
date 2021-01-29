﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostObjectController : MonoBehaviour
{
    Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public void Highlight ()
    {
        outline.enabled = true;
    }

    public void DisableHighlight()
    {
        outline.enabled = false;
    }

    public void ObjectFound ()
    {
        Destroy(gameObject);
    }
}
