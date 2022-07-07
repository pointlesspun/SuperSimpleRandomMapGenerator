Super Simple Random Map Generator
=================================

The goal of this project is to create a number of demos which show how to randomly a somewhat interesting map in Unity.

Technical notes
---------------

At the core of the map generation is a simple algorithm which divides a given rectangle recursively into smaller rectangles. The generated
layout of random rectangles provides a easy to work with datastructure(s) such as a node graph where each node has a rectangle and 0 or
more neighbour nodes or a (axis aligned) BSP. 

After the layout is generated a number of transformations can be applied to cull, replace or populate the rectangles depending on the need of
the application. 

In the current application the division algorithm is implemented in the ```RectangleDivisionService.cs``` class. This class is exposed to the editor
via a MonoBehaviour called ```LayoutGenerator.cs```. To demonstrate the division algorithm in action another MonoBehaviour called  
```IterativeLayoutGenerator.cs``` controls the LayoutGenerator to incrementally (ie iteratively) generate the map. This way the user can 
see the map being generated in real-time.

