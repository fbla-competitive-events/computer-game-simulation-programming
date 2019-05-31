using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;

/// <summary>
/// The state where the classmate travels from classroom to classroom
/// </summary>
public class ClassState : IState
{
    /// <summary>
    /// The classmate that is in this state
    /// </summary>
    private Classmate parent;

    /// <summary>
    /// Basically the map of where the classmate needs to go
    /// </summary>
    private List<Tile> tiles;

    /// <summary>
    /// The next tile that the classmate needs to travel to. The next destination in the "map"
    /// </summary>
    private Tile destinationTile;
    
    /// <summary>
    /// A reference to the tilemap that the classmate needs to travel on. Should be the ground tilemap
    /// </summary>            
    private Tilemap tilemap;

    /// <summary>
    /// Used when the classmate is at its destination
    /// </summary>
    private ObjectPool classmatePool; 

    /// <summary>
    /// Returns the tile that is right between the feet of the classmate
    /// </summary>
    private Vector3Int parentPosAbsolute
    {
        get
        {            
            Vector3 pos = parent.transform.position;

            //Returns the precise coordinates of right between the feet
            return tilemap.WorldToCell(new Vector3(pos.x - .31f, pos.y - .57f));           
        }
    }

    /// <summary>
    /// The position right in the middle of the classmates head
    /// </summary>
    private Vector3Int parentPos
    {
        get
        {
            return tilemap.WorldToCell(new Vector3(parent.transform.position.x - .2f, parent.transform.position.y - .8f));
        }
    }

    /// <summary>
    /// An enum version of this state
    /// </summary>
    public Classmate.State State
    {
        get
        {
            return Classmate.State.Class;
        }
    }

    /// <summary>
    /// Initializes the data and chooses a destination
    /// </summary>
    /// <param name="parent">The classmate that is in this state</param>
    public void Enter(Classmate parent)
    {
        //Sets values and initializes
        this.parent = parent;
        tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        classmatePool = GameObject.Find("ClassmatePool").GetComponent<ObjectPool>();
        System.Random rnd = new System.Random();
        tiles = new List<Tile>();

        //Gets a destination that is not right next to the classmate
        do
        {
            parent.Spawner = GameObject.FindObjectsOfType<Door>()[rnd.Next(0, GameObject.FindObjectsOfType<Door>().Length - 1)].gameObject;
        } while ((tilemap.WorldToCell(parent.Spawner.transform.position) - parentPosAbsolute).magnitude == 1);                        
        
        //Creates a map to the destination created above
        getDirections();               
    }

    /// <summary>
    /// Creates a map of where the classmate needs to go to get to the specified spawner
    /// </summary>
    private void getDirections()
    {
        //The tile that the destination is on
        Vector3Int targetPos = tilemap.WorldToCell(parent.Spawner.transform.position);

        //Possible paths to choose from
        List<Tile> openList = new List<Tile>();

        //Only tiles that are for sure going to be apart of the map
        List<Tile> closedList = new List<Tile>();

        //Where the classmate is standing now is a possible path, so add it
        openList.Add(new Tile(parentPos));
        do
        {
            //Sorts the possible paths based on the lowest F value, which is the best path to take
            openList.Sort();

            //The next square in the path 
            Tile currentSquare;
            
            //If one of the possible paths is the destination, add it to our map            
            if (openList.Exists(x => x.Position == targetPos))
            {
                currentSquare = openList.Find(x => x.Position == targetPos);
            }
            //else just add the path that has the lowest F value, which is the best path to take
            else
            {
                currentSquare = openList[0];
            }
            //The current square needs to be added to our map and taken out of the possible paths
            closedList.Add(currentSquare);
            openList.Remove(currentSquare);

            //If our map has gone all the way to the destination, we are done
            if (closedList.Exists(x => x.Position == targetPos))
            {
                break;
            }

            //Gets all of the walkable, adjacent tiles to the newly found path
            //Basically from now on we are trying to find our next path
            List<Tile> adjacentTiles = getAdjacentTile(currentSquare, targetPos);   
            
            //Loop through each adjacent tile and calculate its G and F values         
            foreach (Tile tile in adjacentTiles)
            {
                //This determines how likely we are to walk on the tile. If, for example, the tile was mud, we would be less likely to 
                //walk on it, so the movement cost would be higher
                int moveCost = movementCost(tile.Parent, tile);

                //We only want to deal with tiles that are not already part of our map
                if (closedList.Contains(tile))
                {
                    continue;
                }

                //If the adjacent tile is not in our list of possible tiles, calculate the G (movement cost + parent's movement cost + grandparent's movement cost etc.)
                //and add it to the list of possible paths
                if (!openList.Exists(x => x.Position == tile.Position))
                {
                    tile.G = tile.Parent.G + moveCost;
                    openList.Add(tile);
                }

                //If the adjacent tile is part of the possible paths, see if its movement cost would be lower than the movement cost of the tile in the list
                //If it is, replace it
                else
                {
                    Tile _tile = openList.Find(x => x.Position == tile.Position);
                    if ((currentSquare.G + moveCost) < _tile.G)
                    {
                        _tile.G = currentSquare.G + moveCost;
                    }
                }
            }
         //Keep looping until there are no more possible paths
        } while (openList.Count != 0);
        
        /* 
         * Now that we have our map, we need to work backwards to have it start where the classmate currently is
         */

        //Have the current tile start out as the target
        Tile currentTile = closedList.Find(x => x.Position == targetPos);

        
        //No matter what we want this tile added to the list.
        tiles.Add(currentTile);   
        
        //Keep looping until we are back at the tile that the classmate is on     
        while (currentTile != null)
        {
            if (currentTile.Parent != null)
            {
                tiles.Add(currentTile);
            }                        
            currentTile = currentTile.Parent;
        }        

        //Set the next tile of where the classmate needs to go
        destinationTile = tiles[tiles.Count - 1];

        try
        {
            //Have the classmate move in the direction that he needs to go
            parent.MoveVector = destinationTile.Position - parentPos;
        }
        catch(NullReferenceException ex)
        {
            return;
        }
    }

    /// <summary>
    /// Calculates the cost of the tile. Just returns one for now, until there are more tiles that require different movement costs
    /// </summary>
    /// <param name="parent">The parent of the tile</param>
    /// <param name="child">The child of the tile</param>
    /// <returns></returns>
    private int movementCost(Tile parent, Tile child)
    {        
        return 1;
    }

    /// <summary>
    /// Gets the walkable tiles adjacent to the given tile
    /// </summary>
    /// <param name="currentTile">The tile wanting adjacent members</param>
    /// <param name="targetPos">The position of the destination</param>
    /// <returns></returns>
    private List<Tile> getAdjacentTile(Tile currentTile, Vector3Int targetPos)
    {
        List<Tile> tiles = new List<Tile>();

        //Loops through all of the tiles adjacent to the currentTile
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //We only want the tile directly to the right, left, up, and down (the ^ thing means xor)
                if (x == 0 ^ y==0)
                {
                    //Gets the position of the adjacent tile
                    Vector3Int pos = new Vector3Int(currentTile.Position.x + x, currentTile.Position.y + y, 0);
                    
                    //Gets the tile using the initialized tilemap                    
                    TileBase tile = tilemap.GetTile(pos);

                    //If the tile is a hallway tile or if the tile is the destination tile, it is a good, walkable tile
                    if (tile != null || pos == targetPos)
                    {
                        tiles.Add(new Tile(pos, targetPos, currentTile));
                    }
                }
            }
        }
        return tiles;
    }

    /// <summary>
    /// Called when switched to a new state
    /// </summary>
    public void Exit()
    {
         
    }

    /// <summary>
    /// Update the map of where the classmate needs to go. Called by the parent classmate
    /// </summary>
    public void Update()
   {        
        //If we are at our final destination
        if (tiles.Count == 0)
        {
            AtDestination();            
            return;
        }

        //If the destination tile is null, it messed things up, so just exit the update
        if (destinationTile == null)
        {
            return;
        }
            
        //If we are on the destination tile, remove it from the list
        Vector3Int displacement = tiles[0].Position - parentPosAbsolute;
        if (destinationTile.Position == parentPosAbsolute || displacement.magnitude == 1)
        {
            tiles.Remove(destinationTile);            
        }
        
        //If we are on the spawner
        if (tiles.Count == 0)
        {
            AtDestination();                
            return;
        }    

        //If we need to update the tile
        if (!destinationTile.Equals(tiles[tiles.Count -1]))
        {
            //The last tile is the tile that the classmate needs to move towards
            destinationTile = tiles[tiles.Count - 1];
            parent.MoveVector = destinationTile.Position - parentPosAbsolute;            
        }
    }

    /// <summary>
    /// Called when at the final destination. ClassmatePool determines what is done next
    /// </summary>
    private void AtDestination()
    {
        classmatePool.SpawnClassmate(parent);
    }

    /// <summary>
    /// A class used to represent a tile on the map
    /// </summary>
    class Tile : IComparable
    {
        ///<summary>
        ///Number of closed tiles away from the classmate
        ///</summary>
        public int G { get; set; }

        ///<summary>
        ///Number of tiles from this Tile to the spawner (sposition)
        ///</summary>
        public int H { get; private set; }

        ///<summary>
        ///Overall how good the tile is. A low F means a good tile
        ///</summary>
        public int F
        {
            get
            {
                return G + H;
            }
        }

        /// <summary>
        /// The position of the tile
        /// </summary>
        public Vector3Int Position { get; private set; }

        /// <summary>
        /// The tile before this tile in the map
        /// </summary>
        public Tile Parent { get; set; }

        /// <summary>
        /// Initializes a new tilie
        /// </summary>
        /// <param name="tposition">The Tile Position</param>
        /// <param name="parent">The Parent Tile</param>        
        /// <param name="sposition">The spawner position</param>
        public Tile(Vector3Int tposition, Vector3Int sposition, Tile parent)
        {            
            Position = tposition;
            Parent = parent;

            //the number of tiles from the tile and the spawner
            H = Math.Abs((tposition - sposition).x) + Math.Abs((tposition - sposition).y);            
        }

        /// <summary>
        /// The first tile on the map
        /// </summary>
        /// <param name="parent"></param>
        public Tile(Vector3Int parent)
        {
            Parent = null;
            Position = parent;
            H = 0;
            G = 0;
        }

        /// <summary>
        /// Determines if two tiles are equal
        /// </summary>
        /// <param name="obj">The tile to compare to </param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return (this == (Tile)obj);
        }

        /// <summary>
        /// Determines how to sort the tile. Sorts based on the F value
        /// </summary>
        /// <param name="obj">The tile to compare to </param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            Tile o = (Tile)obj;

            //this goes before obj
            if (o.F > F)
            {
                return -1;
            }
            //this goes after obj
            else if (o.F < F)
            {
                return 1;
            }
            //The the two F's equal, then put the one with the shortest distance to the target before
            if (o.H > H)
            {
                return -1;
            }
            else if (o.H < H)
            {
                return 1;
            }
            return 0;
        }
    }    
}