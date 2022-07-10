# Release notes

## v1.0

### Done

-   Write high level documentation
-   Create demo page
    -   001 introduction to the demo format (description, layout in the background, create scene, next buttons)
    -   002 iterative generation ( In progress)
        - Add prev/next scene buttons & scene loader script
        - Add version number in the top
    -   003 color transformation
    -   004 cull transformation
    -   005 connectors transformation
-   Make all members start with _
-   Add seed option to LayoutGenerator so the generated map can be controlled
-   Add option to Transformations to indicate when to apply (after each iterating, afterCompletion).
-   Create Tlines connections to neighbors instead direct lines 
    - Draw Tlines 
    - Add Tline datastructure to layout so it can be used outside the gizmos
        rectA,
        rectB,
        Intersectionline,
        projectionPoint A,
        projectionPoint B,
    - Add lines in game view (see https://docs.unity3d.com/Manual/class-LineRenderer.html)

-   Create readme.md for github and setup git pages (rename old readme to technotes.md)

-   Add link in the demos to the github pages


### Dismissed / Later

-   find a pretty sky box (eg https://assetstore.unity.com/packages/3d/simple-sky-cartoon-assets-42373)
    - in the current demo you don't really see the sky box, doesn't justify including the assets at this point

-   Basic unit tests (Delayed until v2.0(1) and v2.0(2) )
