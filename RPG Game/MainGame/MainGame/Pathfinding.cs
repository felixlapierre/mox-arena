using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MainGame
{
    public class Pathfinding
    {
        #region Properties
        const bool isDebugging = false;
        int tileSize;
        int TilesHigh;
        int TilesWide;
        public int MapTilesHigh
        {
            get { return TilesHigh; }
            set { TilesHigh = value; }
        }
        public int MapTilesWide
        {
            get { return TilesWide; }
            set { TilesWide = value; }
        }

        PathfinderType type;
        public PathfinderType Type
        {
            get { return type; }
            set { type = value; }
        }

        List<GridEntity> pathDisplay = new List<GridEntity>();
        Texture2D pathDisplaySprite;
        public int TileSize
        {
            get { return tileSize; }
        }
        #endregion

        #region Constructors
        public Pathfinding(int TilesWide, int TilesHigh, int TileSize, Texture2D pathDisplaySprite, PathfinderType type)
        {
            this.tileSize = TileSize;
            this.TilesHigh = TilesHigh;
            this.TilesWide = TilesWide;
            this.pathDisplaySprite = pathDisplaySprite;
            this.type = type;
        }
        #endregion

        public Path CreatePath(Vector2 userLocation, Vector2 targetLocation, List<GridEntity> obstacles)
        {
            if (isDebugging)
                pathDisplay.Clear();
            List<Path> successfulPaths = new List<Path>();
            Vector2 start = ConvertToGrid(userLocation);
            Vector2 target = ConvertToGrid(targetLocation);

            //Create the initial path
            Path startingPath = new Path(target);
            startingPath.AddLocation(start);

            if (startingPath.LastLocation() == target)
                return startingPath;

            //If the player is standing on any obstacles, ignore them for the purpose of pathfinding
            List<GridEntity> obstaclesToBeChecked = new List<GridEntity>();
            foreach (GridEntity obstacle in obstacles)
            {
                if (!obstacle.CollisionRectangle.Contains(targetLocation))
                    obstaclesToBeChecked.Add(obstacle);
            }

            ListFindPathAlgorithm(startingPath, target, obstaclesToBeChecked, successfulPaths);

            //If no successful path was found, return an empty path
            if (successfulPaths.Count == 0)
                return new Path(target);

            //If there are successful paths, then return the shortest path
            Path shortestPath = successfulPaths.ElementAt(0);
            foreach (Path path in successfulPaths)
            {
                if (path.Length < shortestPath.Length)
                    shortestPath = path;
            }
            DeleteExtraLocations(shortestPath, obstacles);
            if (isDebugging)
            {
                foreach (Vector2 location in shortestPath.Locations)
                {
                    pathDisplay.Add(new GridEntity("PathShower", (int)location.X, (int)location.Y, 0, new Vector2(location.X * tileSize + tileSize / 2, location.Y * tileSize + tileSize / 2), tileSize, pathDisplaySprite));
                }
            }
            return shortestPath;
        }

        /// <summary>
        /// Adds to the successfulPaths list all the valid paths leading to the target provided
        /// starting at the currentPath provided
        /// </summary>
        /// <param name="currentPath">The starting point of the algorithm</param>
        /// <param name="target">The target that must be reached by the path</param>
        /// <param name="TilesWide">The width of the game board, in tiles</param>
        /// <param name="TilesHigh">The height of the game board, in tiles</param>
        /// <param name="obstacles">A list containing all obstacles on the game board</param>
        /// <param name="successfulPaths">A list in which all successful paths will be stored</param>
        public void RecursiveFindPathAlgorithm(Path currentPath, Vector2 target, int TilesWide, int TilesHigh, List<GridEntity> obstacles, List<Path> successfulPaths)
        {
            //Do not execute if a successful path has been found (try avoiding stack overflow)
            if (successfulPaths.Count < 1)
            {
                List<Path> openPaths = new List<Path>(); //To store paths that are currently being checked
                float shortestPathLength = -1;
                if (successfulPaths.Count > 0)
                    shortestPathLength = successfulPaths.ElementAt(0).Length;

                //Create one new path in each direction, and add them to openPaths
                ExpandOutwards(openPaths, currentPath);

                //Delete paths that loop back on themselves, that hit an obstacle, that leave the bounds or are too long
                DeleteBadPaths(openPaths, TilesHigh, TilesWide, obstacles, target, shortestPathLength);

                //Store any path that has successfully found the target in successfulPaths
                List<Path> toBeDeleted = new List<Path>();
                foreach (Path path in openPaths)
                {
                    if (path.LastLocation() == target)
                    {
                        if (path.Length < shortestPathLength)
                            successfulPaths.Insert(0, path);
                        else
                            successfulPaths.Add(path);
                        toBeDeleted.Add(path);
                    }
                }
                //Delete the successful paths from the openPaths to be checked
                foreach (Path path in toBeDeleted)
                    openPaths.Remove(path);
                //Sort the open paths so that the estimated shortest one is started with
                //openPaths = sortPaths(openPaths, target);
                openPaths.Sort();
                //Repeat this function for any path that hasn't arrived at the target yet
                foreach (Path path in openPaths)
                {
                    DeleteExtraLocations(path, obstacles);
                    RecursiveFindPathAlgorithm(path, target, TilesWide, TilesHigh, obstacles, successfulPaths);
                }
            }
        }

        /// <summary>
        /// Finds a path that leads to the target using a list rather than a recursive function 
        /// </summary>
        /// <param name="firstPath">The path containing only the location of the enemy</param>
        /// <param name="target">The location of the player</param>
        /// <param name="tilesWide"></param>
        /// <param name="tilesHigh"></param>
        /// <param name="obstacles">The list containing all obstacles</param>
        /// <param name="successfulPaths">The list in which the successful path is to be stored</param>
        public void ListFindPathAlgorithm(Path firstPath, Vector2 target, List<GridEntity> obstacles, List<Path> successfulPaths)
        {
            List<Path> allPaths = new List<Path>();
            allPaths.Add(firstPath);
            List<Path> openPaths = new List<Path>();
            bool pathNotFound = true;

            bool alreadyLineOfSight = false;
            if (LineOfSight(ConvertFromGrid(firstPath.LastLocation()), ConvertFromGrid(target), obstacles, 40))
                alreadyLineOfSight = true;

            //Main loop
            while (pathNotFound)
            {
                if (allPaths.Count <= 0)
                    return;
                if (allPaths.Count > 50)
                {
                    successfulPaths.Add(allPaths.ElementAt(0));
                    pathNotFound = false;
                }
                //if (allPaths.Count > 50)
                //{
                //    for (int i = 49; i < 25; i--)
                //        allPaths.RemoveAt(i);
                //}
                //if (allPaths.Count > 200)
                //{
                //    successfulPaths.Add(allPaths.ElementAt(0));
                //    pathNotFound = false;
                //}
                ExpandOutwards(openPaths, allPaths.ElementAt(0));
                allPaths.RemoveAt(0);
                DeleteBadPaths(openPaths, TilesHigh, TilesWide, obstacles, target, -1);
                openPaths.Sort();
                foreach (Path path in openPaths)
                {
                    if (type == PathfinderType.BasicPathfinder)
                        CheckPathToPlayer(path, successfulPaths, target, pathNotFound);
                    else if (type == PathfinderType.BasicRanged)
                    {
                        if (!alreadyLineOfSight)
                        {
                            if (LineOfSight(ConvertFromGrid(path.LastLocation()), ConvertFromGrid(target), obstacles, 40)
                                && DistanceBetween(path.LastLocation(), target) < 8)
                            {
                                successfulPaths.Add(path);
                                pathNotFound = false;
                            }
                        }
                        else if (DistanceBetween(path.LastLocation(), target) > DistanceBetween(firstPath.LastLocation(), target)
                            && LineOfSight(ConvertFromGrid(path.LastLocation()), ConvertFromGrid(target), obstacles, 40)
                            && DistanceBetween(path.LastLocation(), target) < 8) 
                        {
                            successfulPaths.Add(path);
                            pathNotFound = false;
                        }
                    }
                    allPaths.Insert(0, path);
                }
                allPaths.Sort();
                openPaths.Clear();
            }

        }

        public void CheckPathToPlayer(Path path, List<Path> successfulPaths, Vector2 target, bool pathNotFound)
        {
            if (path.LastLocation() == target)
            {
                successfulPaths.Add(path);
                pathNotFound = false;
            }
        }

        public void CheckPathRanged(Path path, List<Path> successfulPaths, Vector2 target, List<GridEntity> obstacles, bool pathNotFound)
        {
            if (LineOfSight(ConvertFromGrid(path.LastLocation()), ConvertFromGrid(target), obstacles, 10) )                
            {
                successfulPaths.Add(path);
                pathNotFound = false;
            }
        }

        public void CheckPathRangedDodge(Path path, List<Path> successfulPaths, Vector2 target, List<GridEntity> obstacles, bool pathNotFound)
        {
            if (LineOfSight(path.LastLocation(), target, obstacles, 20)
                && DistanceBetween(path.LastLocation(), target) > DistanceBetween(path.Locations.ElementAt(0), target))
            {
                successfulPaths.Add(path);
                pathNotFound = false;
            }
        }

        public double DistanceBetween(Vector2 point1, Vector2 point2)
        {
            //Pythagoras
            float deltaX = point2.X - point1.X;
            float deltaY = point2.Y - point1.Y;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        public Vector2 ConvertToGrid(Vector2 operand)
        {
            int newLocationX = (int)Math.Floor(operand.X / tileSize);
            int newLocationY = (int)Math.Floor(operand.Y / tileSize);
            return new Vector2(newLocationX, newLocationY);
        }

        public Vector2 ConvertFromGrid(Vector2 operand)
        {
            int newLocationX = ((int)operand.X * tileSize + tileSize / 2);
            int newLocationY = ((int)operand.Y * tileSize + tileSize / 2);
            return new Vector2(newLocationX, newLocationY);
        }

        public void ExpandOutwards(List<Path> openPaths, Path currentPath)
        {
            Vector2 currentLocation = currentPath.LastLocation();

            Path pathUp = currentPath.Copy();
            Path pathDown = currentPath.Copy();
            Path pathLeft = currentPath.Copy();
            Path pathRight = currentPath.Copy();

            pathUp.AddLocation(currentLocation + new Vector2(0, 1));
            openPaths.Add(pathUp);

            pathDown.AddLocation(currentLocation + new Vector2(0, -1));
            openPaths.Add(pathDown);

            pathLeft.AddLocation(currentLocation + new Vector2(-1, 0));
            openPaths.Add(pathLeft);

            pathRight.AddLocation(currentLocation + new Vector2(1, 0));
            openPaths.Add(pathRight);
        }

        public void DeleteBadPaths(List<Path> openPaths, int tilesHigh, int tilesWide, List<GridEntity> obstacles, Vector2 target, float shortestPathLength)
        {
            List<Path> toBeDeleted = new List<Path>();

            foreach (Path path in openPaths)
            {
                Vector2 currentLocation = path.LastLocation();
                //The following is executed only if the given list contains duplicates
                List<Vector2> allLocations = new List<Vector2>();
                foreach (Vector2 location in path.Locations)
                    allLocations.Add(location);
                foreach (Vector2 location in path.DeletedLocations)
                    allLocations.Add(location);
                if (allLocations.Count != allLocations.Distinct().Count())
                    toBeDeleted.Add(path);

                //The following is executed if the path goes out of bounds
                else if (currentLocation.X < 0)
                    toBeDeleted.Add(path);
                else if (currentLocation.X > (TilesWide - 1))
                    toBeDeleted.Add(path);
                else if (currentLocation.Y < 0)
                    toBeDeleted.Add(path);
                else if (currentLocation.Y > (TilesHigh - 1))
                    toBeDeleted.Add(path);

                //The following is executed if the path encounters an obstacle
                else
                {
                    foreach (GridEntity obstacle in obstacles)
                    {
                        if (currentLocation == new Vector2(obstacle.XPosition, obstacle.YPosition))
                            toBeDeleted.Add(path);
                    }
                }

                //The following flags the path for deletion if the path is too long
                if (shortestPathLength > 0 && path.Length > shortestPathLength)
                {
                    toBeDeleted.Add(path);
                }

            }
            foreach (Path path in toBeDeleted)
                openPaths.Remove(path);
        }

        public List<Path> sortPaths(List<Path> paths, Vector2 target)
        {
            List<Path> sortedList = new List<Path>();

            while (paths.Count > 0)
            {
                Path shortestPath = paths.ElementAt(0);
                foreach (Path path in paths)
                {
                    if (path.Length < shortestPath.Length)
                        shortestPath = path;
                }
                sortedList.Add(shortestPath.Copy());
                paths.Remove(shortestPath);
            }
            paths = sortedList;
            return paths;
        }

        public bool LineOfSight (Vector2 location, Vector2 target, List<GridEntity> obstacles, float pathWidth)
        {
            bool lineOfSight = true;
            Vector2 line1Start = location;
            Vector2 line1End = target;

            if (pathWidth <= 0)
            {
                foreach (GridEntity obstacle in obstacles)
                {
                    if (RectangleLineIntersection(line1Start, line1End, obstacle.CollisionRectangle))
                        lineOfSight = false;
                }
            }
            else
            {
                float halfPathWidth = pathWidth / 2;
                float lineAngle = (float)Math.Atan2(target.Y - location.Y, target.X - location.X);
                Vector2 secondaryLineDistance = new Vector2(halfPathWidth * (-1) * (float)Math.Sin(lineAngle), halfPathWidth * (float)Math.Cos(lineAngle));
                Vector2 line2Start = line1Start + secondaryLineDistance;
                Vector2 line2End = line1End + secondaryLineDistance;
                Vector2 line3Start = line1Start - secondaryLineDistance;
                Vector2 line3End = line1End - secondaryLineDistance;
                foreach (GridEntity obstacle in obstacles)
                {
                    if (RectangleLineIntersection(line1Start, line1End, obstacle.CollisionRectangle))
                        lineOfSight = false;
                    if (RectangleLineIntersection(line2Start, line2End, obstacle.CollisionRectangle))
                        lineOfSight = false;
                    if (RectangleLineIntersection(line3Start, line3End, obstacle.CollisionRectangle))
                        lineOfSight = false;
                }
            }
            if (lineOfSight == true)
                line1End = new Vector2(0, 0);
            return lineOfSight;
        }

        public bool RectangleLineIntersection(Vector2 lineStart, Vector2 lineEnd, Rectangle rectangle)
        {
            Vector2 topLeftCorner = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 topRightCorner = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 bottomLeftCorner = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 bottomRightCorner = new Vector2(rectangle.Right, rectangle.Bottom);

            if (LineLineIntersection(lineStart, lineEnd, topLeftCorner, topRightCorner) ||
                LineLineIntersection(lineStart, lineEnd, topRightCorner, bottomRightCorner) ||
                LineLineIntersection(lineStart, lineEnd, bottomRightCorner, bottomLeftCorner) ||
                LineLineIntersection(lineStart, lineEnd, bottomLeftCorner, topLeftCorner))
                return true;
            else
                return false;
        }

        public bool LineLineIntersection(Vector2 line1Start, Vector2 line1End, Vector2 line2Start, Vector2 line2End)
        {
            float x11 = line1Start.X;
            float y11 = line1Start.Y;
            float x21 = line2Start.X;
            float y21 = line2Start.Y;

            float dx1 = line1End.X - x11;
            float dy1 = line1End.Y - y11;
            float dx2 = line2End.X - x21;
            float dy2 = line2End.Y - y21;

            if (dy1 * dx2 - dx1 * dy2 == 0)
                //Lines are parallel, thus it matters not (for our purposes) if they intersect infinitely or not at all)
                return false;
            float t1 = ((x11 - x21) * dy2 + (y21 - y11) * dx2) / (dy1 * dx2 - dx1 * dy2);
            float t2 = ((x21 - x11) * dy1 + (y11 - y21) * dx1) / (dy2 * dx1 - dx2 * dy1);
            if (0 <= t1 && t1 <= 1 && 0 <= t2 && t2 <= 1)
                return true;

            else
                return false;
        }

        public void DeleteExtraLocations(Path path, List<GridEntity> obstacles)
        {
            int i = 0; //Incrementer
            int j = 2;

            while (i + 2 < path.Locations.Count)
            {
                Vector2 currentGridLocation = path.Locations.ElementAt(i);
                Vector2 observedGridLocation = path.Locations.ElementAt(j);
                Vector2 currentLocation = new Vector2(currentGridLocation.X * tileSize, currentGridLocation.Y * tileSize);
                Vector2 observedLocation = new Vector2(observedGridLocation.X * tileSize, observedGridLocation.Y * tileSize);
                if (currentGridLocation.X - observedGridLocation.X != 0 &&
                    currentGridLocation.Y - observedGridLocation.Y != 0)
                {
                    Vector2 line1Start = new Vector2(currentLocation.X + tileSize, currentLocation.Y + tileSize);
                    Vector2 line1End = new Vector2(observedLocation.X + tileSize, observedLocation.Y + tileSize);
                    Vector2 line2Start = new Vector2(currentLocation.X, currentLocation.Y + tileSize);
                    Vector2 line2End = new Vector2(observedLocation.X, observedLocation.Y + tileSize);
                    Vector2 line3Start = new Vector2(currentLocation.X + tileSize, currentLocation.Y);
                    Vector2 line3End = new Vector2(observedLocation.X + tileSize, observedLocation.Y);
                    Vector2 line4Start = new Vector2(currentLocation.X, currentLocation.Y);
                    Vector2 line4End = new Vector2(observedLocation.X, observedLocation.Y);

                    bool pathCanBeShortened = true;
                    foreach (GridEntity obstacle in obstacles)
                    {
                        if (RectangleLineIntersection(line1Start, line1End, obstacle.CollisionRectangle) ||
                            RectangleLineIntersection(line2Start, line2End, obstacle.CollisionRectangle) ||
                            RectangleLineIntersection(line3Start, line3End, obstacle.CollisionRectangle) ||
                            RectangleLineIntersection(line4Start, line4End, obstacle.CollisionRectangle))
                            pathCanBeShortened = false;
                    }
                    if (pathCanBeShortened)
                    {
                        for (int k = j - i; k > 1; k--)
                        {
                            path.DeletedLocations.Add(path.Locations.ElementAt(i + 1));     
                            path.Locations.RemoveAt(i + 1);
                        }
                        if (j > path.Locations.Count - 1)
                        {
                            i++;
                            j = i + 2;
                        }
                    }
                    else
                    {
                        i++;
                        j = i + 2;
                    }
                }
                else
                {
                    j++;
                    if (j > path.Locations.Count - 1)
                    {
                        i++;
                        j = i + 2;
                    }
                }
                        
            } 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isDebugging)
            {
                foreach (GridEntity entity in pathDisplay)
                {
                    entity.Draw(spriteBatch);
                }
            }
        }

        public Pathfinding Copy()
        {
            return new Pathfinding(TilesWide, TilesHigh, TileSize, pathDisplaySprite, type);
        }
    }

    public class Path : IComparable<Path>
    {
        #region Properties

        List<Vector2> locations = new List<Vector2>();
        public List<Vector2> Locations
        {
            get { return locations; }
        }
        List<Vector2> deletedLocations = new List<Vector2>();
        public List<Vector2>DeletedLocations
        {
            get { return deletedLocations; }
        }
        Vector2 target;
        public Vector2 Target
        {
            get { return target; }
            set { target = value; }
        }
        float length;
        public float Length
        {
            get { return length; }
            set { length = value; }
        }

        #endregion

        #region Constructors

        public Path(Vector2 target)
        {
            this.target = target;
        }
        public Path(Vector2 target, List<Vector2> locations)
        {
            this.target = target;
            foreach (Vector2 location in locations)
            {
                this.locations.Add(new Vector2(location.X, location.Y));
            }
            UpdateLength();
        }

        #endregion

        public void AddLocation(Vector2 location)
        {
            locations.Add(location);
            UpdateLength();
        }

        public Vector2 LastLocation()
        {
            return locations.ElementAt(locations.Count - 1);
        }

        public void UpdateLength()
        {
            float newLength = 0;
            foreach (Vector2 location in locations)
                if (locations.IndexOf(location) != locations.Count - 1)
                    newLength += DistanceBetweenPoints(location,locations.ElementAt(locations.IndexOf(location) + 1));

            float minimumDistanceToTarget = DistanceBetweenPoints(target, LastLocation());
            newLength += minimumDistanceToTarget;
            length = newLength;
        }

        public float DistanceBetweenPoints(Vector2 location1, Vector2 location2)
        {
            return (float)Math.Sqrt(Math.Pow((location1.Y - location2.Y), 2) + Math.Pow((location1.X - location2.X), 2));
        }
        
        public Vector2 nextLocationCoordinates(int tileSize)
        {
            float xLocation = (locations.ElementAt(0).X + 0.5f) * tileSize;
            float yLocation = (locations.ElementAt(0).Y + 0.5f) * tileSize;
            return new Vector2(xLocation, yLocation);
        }

        public Path Copy()
        {
            List<Vector2> copyLocations = new List<Vector2>();
            foreach (Vector2 location in locations)
                copyLocations.Add(location);
            Path copy = new Path(target, copyLocations);
            return copy;
        }

        public int CompareTo(Path T)
        {
            if (Length < T.Length)
                //This object precedes the object specified by the CompareTo method in the sort order.
                return -1;
            else if (Length > T.Length)
                //This current instance follows the object specified by the CompareTo method argument in the sort order.
                return 1;
            else
                //This current instance occurs in the same position in the sort order as the object specified by the CompareTo method argument.
                return 0;

        }

    }
}
