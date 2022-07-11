# To do (in no particular order)

## v2.0: Refactoring & Bugfixing

## In progress
-   Create AABSP
-   Add unit tests.
-   Add a flag to the AABSP signalling whether or not any size constraint violation should stop
    node development. IE now a node can be developed if node's width OR height is below the threshold.
    And options should be added that a node can only be developed it the node's width AND height is below the
    threshold.
-   Add a data field to the BSP nodes.

### Planned

-   Add point lookup to the AABSP.
-   Add rect lookup to the AABSP.
-   Add circle lookup to the AABSP.
-   Add factory creating a layout based on the AABSP
-   Create a version of the layout generator which just takes transformations (named eg TransformationStream)
-   Add Serializable to the AABSP

-   Add warning if no prefab is defined in the layout generator.
-   Gizmos are not drawn correctly when the object containing the layout generator is moved.
-   (1) Refactor LayoutGeneration so each step is a transformation, starting with a rectangle.
-   (2) Add AABSP transformation as a start.
-   Should be able to set the starting division (as opposed to random)

- Add demo for AABSP / AARectGraph.

### Done

-   Should be able to set the number of divisions to any number (not necessarily even). To do so track the 'depth' in the AABSP.

## v3.0: City

-   Cull away from center
-   Add blocks as if it was a city
-   Add park area
-   Add roads
-   Get some nice props
-   Add height

## v4.0: Dungeon

### Planned

-   Walls
-   Doors
-   Cull halways to create pathways

## v4.0: Pathfinding

### Planned

## v5.0: Race track ?

...
