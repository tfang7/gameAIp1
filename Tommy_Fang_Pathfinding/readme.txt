CONTROLS
Left Click set starting TILE position
Right Click set destination TILE position
CTRL + CLICK toggle tile as obstacle, you have to press ctrl + click at the same time, a console message shows up.

I used the exact A* algorithm found on the lecture slides. you can find the code in Astar.cs
It uses the distance from start/distance to goal as heuristics. I also add a tile to the next
iteration of A*'s search space. If the solution is not reached within an iteration, it adds all the other closer
nodes to the goal to the search space and the A* algorithm will search through them until the goal is reached or
the algorithm times out.

As the algorithm searches, it sets the cost of the adjacent tiles.

The board is composed of 2x2 squares. The file reads through checking the character to instantiate a gameobject.
Obstacles are considered blocked if all the characters in the 2x2 tile are trees or obstacles. 
There are additional debug draw lines in the scene for each board representation.
I tested the left clicking and right clicking feature, it works on both maps.


CHANGING MAPS
click on the board gameobject with the boardgenerator script. there is a field for file name
change arena2.map to arena1.map. I renamed hrn2012 to arena1

MAIN FUNCTIONS
BoardGenerator.cs
Tileizer(), Waypointizer, chunkObstacles
these loop through the file and determine if the tile should be an obstacle or path.

mouse.cs
located on every waypoint center, 

Astar.cs
There are two different find path functions, they are poorly named.
findPathWaypoints is A* for waypoints
fnNode is A* for tiles.
*** IMPORTANT ***
RESETTING SIMULATION
Console messages appear for each interaction.
To reset the pathfinder after right click and left clicking a new tile, click on the RESET bool
located on the A* game object in the inspector. This will clear the previous path and start finding the new one.
