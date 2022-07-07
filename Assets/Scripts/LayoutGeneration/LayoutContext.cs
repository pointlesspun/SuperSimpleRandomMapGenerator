﻿using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collection of mutable data forming the context under which the given layout was constructed
/// </summary>
public class LayoutContext
{
    /// <summary>
    /// The gameobject holding all the objects with RectangleNode2DBehaviour
    /// </summary>
    public GameObject layoutContainer;

    /// <summary>
    /// Current state of the (randomly generated) layout
    /// </summary>
    public List<RectangleNode2DBehaviour> layout;

    /// <summary>
    /// Bounds of the layout
    /// </summary>
    public RectInt bounds;
    
    /// <summary>
    /// Configuration used to produce the given layout
    /// </summary>
    public LayoutConfiguration config;

    /// <summary>
    /// Shared store for any other property a transformation may add or modify.
    /// </summary>
    public Dictionary<string, object> store = new Dictionary<string, object>();
}

