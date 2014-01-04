using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;


namespace TowerDefense
{
    public enum State { build, wave }

    class GameWorld
    {
        TowerButton tb;
        public RectangleF mouseRect;
        public List<TowerButton> tl;

        // Fields
        private Random rnd = new Random();
        private DateTime lastFrameStarted = new DateTime();
        private float currentFPS;
        private Graphics dc;
        private string phase;
        private BufferedGraphics buffer;
        private State currentState = State.build;
        private Stopwatch stopWatch = new Stopwatch();
        private Stopwatch buildWatch = new Stopwatch();
        private Stopwatch enemyWatch = new Stopwatch();
        
        // Fields for world creation
        private float worldSizeX;
        private float worldSizeY;
        public int tileSizeX = 96;
        public int tileSizeY = 96;
        private int[][] coordinateSystem;
        private int grottoX;
        private int grottoY;
        private int treasureX;
        private int treasureY;
        private int minDistStart = 3;
        public List<Environment> environment = new List<Environment>();
        public List<Tower> towers = new List<Tower>();
        private List<PointF> checkpointList = new List<PointF>();
        private List<PointF> endPoints = new List<PointF>();
        private List<PointF> startPoints = new List<PointF>();
        private bool validLocation;

        #region Field for wave
        // Fields for wave
        List<List<Enemy>> waveEnemy = new List<List<Enemy>>();
        private List<int> waveCount = new List<int>();
        private List<Enemy> currentWave = new List<Enemy>();
        List<bool> pathAvailable = new List<bool>();
        private List<List<PointF>> path = new List<List<PointF>>();
        private int waveNumber;
        private int listNumb;
        private int checkPoint;
        private float chosenDif;
        #endregion
        // Fields for building phase

        private int gold;
        private int cost;
        private int chest;


        // Constructors

        public GameWorld(Graphics dc, Rectangle displayRectangle, float worldSizeX, float worldSizeY, float dif)
        {
            // Creates and allocates a buffer in memory with the size of the display
            buffer = BufferedGraphicsManager.Current.Allocate(dc, displayRectangle);

            this.dc = buffer.Graphics;
            this.worldSizeX = worldSizeX;
            this.worldSizeY = worldSizeY;
            this.chosenDif = dif;

            SetupWorld();
        }

        // Functions

        /// <summary>
        /// Creates the world layout
        /// </summary>
        public void SetupWorld()
        {
            mouseRect = new Rectangle(Form1.MousePosition, new Size(1, 1));
            tl = new List<TowerButton>();
            tl.Add(new TowerButton(new Size(100, 100), new Point(150, 150), "Test", "hej", 1));
            tb = new TowerButton(new Size(100, 100), new Point(150, 150), "Test", "hej", 1);
            //}

            //Starting FPS timert
            lastFrameStarted = DateTime.Now;

            #region Instantiating coordinate system and grotto/checkpoints/treasure location
            // Instantiating coordinate system
            coordinateSystem = new int[(int)worldSizeX + 1][];

            for (int i = 0; i <= worldSizeX; i++)
            {
                coordinateSystem[i] = new int[(int)worldSizeY + 1];
            }

            //Instantiating coordinate system
            for (int x = 0; x < worldSizeX + 1; x++)
            {
                for (int y = 0; y < worldSizeY + 1; y++)
                {
                    coordinateSystem[x][y] = 0;
                }
            }

            //Randomizing number of checkpoints in the map
            int numberOfCheckpoints = rnd.Next(0, 3);
            if (numberOfCheckpoints > 0)
            {
                for (int i = 0; i < numberOfCheckpoints; i++)
                {
                    checkpointList.Add(new PointF());
                }
            }

            //Initializing enemy path lists, depending on amount of checkpoints
            for (int i = 0; i < checkpointList.Count + 1; i++)
            {
                path.Add(new List<PointF>());
                pathAvailable.Add(new bool());
            }

            //Location for treasure
            treasureX = rnd.Next(1, (int)worldSizeX - 1);
            treasureY = rnd.Next(1, (int)worldSizeY - 2);

            //Location for grotto
            #region Defining which border grotto should spawn on
            //Randoming one out of 4 numbers, each representing a side
            int locationOfGrotto = rnd.Next(0, 3);
            //left
            if (locationOfGrotto == 0)
            {
                grottoX = 0;
                grottoY = rnd.Next(1, (int)worldSizeY - 2);
            }
            //right
            if (locationOfGrotto == 1)
            {
                grottoX = (int)worldSizeX - 1;
                grottoY = rnd.Next(1, (int)worldSizeY - 2);
            }
            //top
            if (locationOfGrotto == 2)
            {
                grottoX = rnd.Next(1, (int)worldSizeY - 2);
                grottoY = 0;
            }
            //bot
            if (locationOfGrotto == 3)
            {
                grottoX = rnd.Next(1, (int)worldSizeY - 2);
                grottoY = (int)worldSizeY - 1;
            }
            #endregion

            //Adding grotto position to list of startpoints used for saving paths. Saved as first position in list
            startPoints.Add(new PointF(grottoX, grottoY));

            //Location for checkpoints
            if (numberOfCheckpoints > 0)
                for (int i = 0; i < numberOfCheckpoints; i++)
                {
                    checkpointList[i] = new PointF(rnd.Next(0, (int)worldSizeX), rnd.Next(0, (int)worldSizeY));
                    //Adding position of checkpoint to list of endpoints used for saving paths
                    endPoints.Add(checkpointList[i]);
                    //Adding position of checkpoint to list of startpoints used for saving paths
                    startPoints.Add(checkpointList[i]);
                }

            //Adding treasure position to list of endpoints used for saving paths. Added as last position in list
            endPoints.Add(new PointF(treasureX, treasureY));

            // Bool for checking validity of tile location
            validLocation = CheckLocation(grottoX, grottoY, treasureX, treasureY, minDistStart);

            //If location is not valid (if start is too close to finish), find a new location
            while (!validLocation)
            {
                treasureX = rnd.Next(0, (int)worldSizeX);
                treasureY = rnd.Next(0, (int)worldSizeY);

                validLocation = CheckLocation(grottoX, grottoY, treasureX, treasureY, minDistStart);
            }

            #endregion

            #region Building the world layout

            //Assign values to positions in the coordinate system and place environment tiles accordingly while adding them to environment list
            Randomizer(coordinateSystem, environment);

            //Building enemy paths, depending on amount of checkpoints
            for (int i = 0; i < endPoints.Count; i++)
            {
                RoadBuilder((int)startPoints[i].X, (int)startPoints[i].Y, (int)endPoints[i].X, (int)endPoints[i].Y, i);
            }

            //If a path cannot be found, perform generation again
            while (!CheckIfValidPath(pathAvailable, numberOfCheckpoints))
            {
                //Perform world generation again
                Randomizer(coordinateSystem, environment);
                for (int i = 0; i < endPoints.Count; i++)
                {
                    RoadBuilder((int)startPoints[i].X, (int)startPoints[i].Y, (int)endPoints[i].X, (int)endPoints[i].Y, i);
                }
            }

            #endregion

        }
        /// <summary>
        /// Makes sure all the update functions is called every frames
        /// </summary>
        public void GameLoop()
        {
            //Time spent since last loop
            TimeSpan deltaTime = DateTime.Now - lastFrameStarted;
            //Convert deltaTime to milliseconds, 1ms minimum
            int milliSeconds = deltaTime.Milliseconds > 0 ? deltaTime.Milliseconds : 1;
            currentFPS = 1000 / milliSeconds;

            //Set new frame start
            lastFrameStarted = DateTime.Now;

            Update();
            UpdateAnimation();
            Draw();
            GameState();
            if (currentState == State.wave)
            {
                Wave();
            }

        }
        /// <summary>
        /// Updates the gameworld on every frame
        /// </summary>
        public void Update()
        {
            //Update mouse rectangle pos
            mouseRect.Location = Form1.localMousePos;

            //if (mouseRect.IntersectsWith())
            //{

            //}

            foreach (TowerButton tb in tl)
            {
                if (mouseRect.IntersectsWith(tb.CollisionRect))
                {
                    float x = Form1.guiPos.X - 50;
                    float y = Form1.guiPos.Y - 50;
                    #region Water Towers
                    // Water Towers
                    //Build Water Tower 1
                    if (tb.Name == "Water_Tower1")
                    {
                        Build(1, new PointF(x, y));
                    }
                    // Build Water Tower 2
                    if (tb.Name == "Water_Tower2")
                    {
                        Build(2, new PointF(x, y));
                    }
                    // Build Water Tower 3
                    if (tb.Name == "Water_Tower3")
                    {
                        Build(3, new PointF(x, y));
                    }
                    #endregion
                    #region Land Towers
                    // Land Towers
                    // Build Land Tower 1
                    if (tb.Name == "Land_Tower1")
                    {
                        Build(4, new PointF(x, y));
                    }
                    // Build Land Tower 2
                    if (tb.Name == "Land_Tower2")
                    {
                        Build(5, new PointF(x, y));
                    }
                    // Build Land Tower 3
                    if (tb.Name == "Land_Tower3")
                    {
                        Build(6, new PointF(x, y));
                    }
                    #endregion
                }
            }
            //Update all tower objects
            foreach (Tower tower in towers)
            {
                tower.Update(currentFPS);
            }

            //Update all environment objects
            foreach (Environment env in environment)
            {
                env.Update(currentFPS);
            }

            //Update all enemy objects
            foreach (Enemy enemy in currentWave)
            {
                enemy.Update(currentFPS);
            }
        }

        /// <summary>
        /// Updates the animations of the game objects in the world
        /// </summary>
        public void UpdateAnimation()
        {
            //Update all tower animations
            foreach (Tower tower in towers)
            {
                tower.UpdateAnimation(currentFPS);
            }

            //Update all environment animations
            foreach (Environment env in environment)
            {
                env.UpdateAnimation(currentFPS);
            }

            //Update all enemy animations
            foreach (Enemy enemy in currentWave)
            {
                enemy.UpdateAnimation(currentFPS);
            }
        }

        /// <summary>
        /// Draws The World
        /// </summary>
        public void Draw()
        {
            if (Form1.guiIsClicked)
            {
                if (Form1.drawBuildGUI == 4)
                {
                    tl.Add(new TowerButton(new Size(75, 75), new Point(Form1.guiPos.X + 50, Form1.guiPos.Y), "Land_Tower1", "Towers/w1.png", 1));
                    tl.Add(new TowerButton(new Size(75, 75), new Point(Form1.guiPos.X - 120, Form1.guiPos.Y), "Land_Tower2", "Towers/w2.png", 2));
                    tl.Add(new TowerButton(new Size(75, 75), new Point(Form1.guiPos.X - 30, Form1.guiPos.Y - 120), "Land_Tower3", "Towers/w3.png", 3));
                }
                if (Form1.drawBuildGUI == 3)
                {
                    tl.Add(new TowerButton(new Size(75, 75), new Point(Form1.guiPos.X + 50, Form1.guiPos.Y), "Water_Tower1", "Towers/w1.png", 1));
                    tl.Add(new TowerButton(new Size(75, 75), new Point(Form1.guiPos.X - 120, Form1.guiPos.Y), "Water_Tower2", "Towers/w2.png", 2));
                    tl.Add(new TowerButton(new Size(75, 75), new Point(Form1.guiPos.X - 30, Form1.guiPos.Y - 120), "Water_Tower3", "Towers/w3.png", 3));
                }
                if (Form1.drawBuildGUI == 2)
                {
                    tl.Add(new TowerButton(new Size(75, 75), new Point(Form1.guiPos.X - 30, Form1.guiPos.Y - 120), "Sell_Tower", "Towers/st.png", 3));
                }
            }
            dc.Clear(Color.White);

            //Drawing environment
            foreach (Environment env in environment)
            {
                env.Draw(dc);
            }

            //Drawing towers
            foreach (Tower tow in towers)
            {
                tow.Draw(dc);
            }

            //Drawing enemies
            foreach (Enemy enemy in currentWave)
            {
                enemy.Draw(dc);
            }

            Font w = new Font("Arial", 14);
            Brush q = new SolidBrush(Color.White);
            dc.DrawString(string.Format("Phase: {0}", phase), w, q, 30, 5);
            if (Form1.guiIsClicked)
            {
                foreach (TowerButton tb in tl)
                {
                    tb.DrawMe(dc);
                }
            }

            buffer.Render();

        }
        /// <summary>
        /// Checking If The Location Of The Treasure Island Is On A Valid Location
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <returns></returns>
        public bool CheckForDuplicates(ref List<Node> openNodes, Node mainNode, int xModifier, int yModifier)
        {
            return openNodes.Any(p => p.LocationX == mainNode.LocationX + xModifier && p.LocationY == mainNode.LocationY + yModifier);
        }
        /// <summary>
        /// Checking If The Location Of The Treasure Island Is On A Valid Location And Is Minimum Px From Start
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <returns></returns>
        public bool CheckLocation(int startX, int startY, int endX, int endY, int minDistStart)
        {
            Random rnd = new Random();
            bool invalidLocation = false;

            for (int x = 0; x < minDistStart + 1; x++)
            {
                for (int y = 0; y < minDistStart + 1; y++)
                {
                    if (endX + x == startX && endY + y == startY)
                    {
                        invalidLocation = true;
                    }

                    if (endX + x == startX && endY - y == startY)
                    {
                        invalidLocation = true;
                    }

                    if (endX - x == startX && endY + y == startY)
                    {
                        invalidLocation = true;
                    }

                    if (endX - x == startX && endY - y == startY)
                    {
                        invalidLocation = true;
                    }
                }
            }

            if (invalidLocation == true)
                return false;
            else
                return true;
        }
        /// <summary>
        /// Builds The Road The Enemies Will Follow
        /// </summary>
        /// <param name="worldSizeX"></param>
        /// <param name="worldSizeY"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <param name="coordinateSystem"></param>

        #region Pathfinding A* algorithm

        public void RoadBuilder(int startX, int startY, int endX, int endY, int pathNumber)
        {
            //Creating a list of nodes
            Node mainNode;
            List<Node> openNodes = new List<Node>();
            List<Node> closedNodes = new List<Node>();
            mainNode = new Node(startX, startY, endX, endY, -1);
            openNodes.Add(mainNode);

            //Check nearby nodes
            CheckNearbyNodes(ref mainNode, ref openNodes, ref closedNodes, startX, startY, endX, endY);
            mainNode.WasChecked = true;
            RemoveClosedNodesFromOpenNodesList(ref closedNodes, ref openNodes);

            //Setting new main node
            while (mainNode.LocationX != endX || mainNode.LocationY != endY)
            {
                if (!NodeListContains(ref closedNodes, ref mainNode))
                {
                    closedNodes.Add(mainNode);
                }
                openNodes.Remove(mainNode);
                int counter = 0;
                foreach (Node node in openNodes)
                {
                    counter++;
                    //If node's F value is less than current mainNode, set that node to mainNode instead
                    if (node.F < mainNode.F)
                    {
                        mainNode = node;
                        break;
                    }

                    //If no node's F value is less than current mainNode, choose the first node from the openNodes list instead
                    if (counter == openNodes.Count)
                    {
                        mainNode = openNodes.First(n => node.WasChecked == false);
                    }
                }

                //Perform nearby node checks again
                CheckNearbyNodes(ref mainNode, ref openNodes, ref closedNodes, startX, startY, endX, endY);
                RemoveClosedNodesFromOpenNodesList(ref closedNodes, ref openNodes);
                mainNode.WasChecked = true;

                //If no more open nodes are available, label path as unavailable
                if (openNodes.Count < 1)
                {
                    pathAvailable[pathNumber] = false;
                    break;
                }
            }

            //If mainNode's location is the same as end location, path is valid and set to true
            if (mainNode.LocationX == endX && mainNode.LocationY == endY)
                pathAvailable[pathNumber] = true;
            else
                pathAvailable[pathNumber] = false;

            //Draw this valid path and save it in a list of paths
            if (pathAvailable[pathNumber])
            {
                BuildEnemyPath(ref closedNodes, mainNode, mainNode.G - 1, pathNumber);
            }
        }

        /// <summary>
        /// Building enemy path and storing it in a list of PointF values
        /// </summary>
        /// <param name="closedNodes"></param>
        /// <param name="mainNode"></param>
        /// <param name="steps"></param>
        /// <param name="pathNumber"></param>
        public void BuildEnemyPath(ref List<Node> closedNodes, Node mainNode, int steps, int pathNumber)
        {
            Node tempNode = mainNode;
            int counter = 0;
            while (tempNode.G != 0)
            {
                foreach (Node node in closedNodes)
                {
                    if (node.LocationX == tempNode.LocationX - 1 && node.LocationY == tempNode.LocationY && node.G == steps - counter)
                    {
                        path[pathNumber].Add(new PointF(node.LocationX * tileSizeX + (tileSizeX / 2), node.LocationY * tileSizeY + (tileSizeY / 2)));
                        counter++;
                        tempNode = node;
                        break;
                    }
                    if (node.LocationX == tempNode.LocationX + 1 && node.LocationY == tempNode.LocationY && node.G == steps - counter)
                    {
                        path[pathNumber].Add(new PointF(node.LocationX * tileSizeX + (tileSizeX / 2), node.LocationY * tileSizeY + (tileSizeY / 2)));
                        counter++;
                        tempNode = node;
                        break;
                    }
                    if (node.LocationX == tempNode.LocationX && node.LocationY == tempNode.LocationY - 1 && node.G == steps - counter)
                    {
                        path[pathNumber].Add(new PointF(node.LocationX * tileSizeX + (tileSizeX / 2), node.LocationY * tileSizeY + (tileSizeY / 2)));
                        counter++;
                        tempNode = node;
                        break;
                    }
                    if (node.LocationX == tempNode.LocationX && node.LocationY == tempNode.LocationY + 1 && node.G == steps - counter)
                    {
                        path[pathNumber].Add(new PointF(node.LocationX * tileSizeX + (tileSizeX / 2), node.LocationY * tileSizeY + (tileSizeY / 2)));
                        counter++;
                        tempNode = node;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if a valid path is available or the map should be remade
        /// </summary>
        /// <param name="pathAvailable"></param>
        /// <param name="numberOfCheckpoints"></param>
        /// <returns></returns>
        public bool CheckIfValidPath(List<bool> pathAvailable, int numberOfCheckpoints)
        {
            for (int i = 0; i <= numberOfCheckpoints; i++)
            {
                if (pathAvailable[i] == false)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Shortcut for checking if node is out of bounds
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool OutOfBounds(int x, int y)
        {
            return x >= 0 && x < worldSizeX
                && y >= 0 && y < worldSizeY;
        }

        /// <summary>
        /// Checks if list already contains specific node
        /// </summary>
        /// <param name="nodeList"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool NodeListContains(ref List<Node> nodeList, ref Node node)
        {
            foreach (Node currentNode in nodeList)
            {
                if (currentNode.LocationX == node.LocationX && currentNode.LocationY == node.LocationY)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Removes duplicated notes in list of open nodes
        /// </summary>
        /// <param name="closedNodes"></param>
        /// <param name="openNodes"></param>
        public void RemoveClosedNodesFromOpenNodesList(ref List<Node> closedNodes, ref List<Node> openNodes)
        {
            for (int x = 0; x < closedNodes.Count; x++)
            {
                for (int y = 0; y < openNodes.Count; y++)
                {
                    if (closedNodes[x].LocationX == openNodes[y].LocationX && closedNodes[x].LocationY == openNodes[y].LocationY)
                        openNodes.Remove(openNodes[y]);
                }
            }
        }

        //Saving path to PointF array
        //path.Add(new PointF(mainNode.LocationX, mainNode.LocationY));
        public void CheckNearbyNodes(ref Node mainNode, ref List<Node> openNodes, ref List<Node> closedNodes, int startX, int startY, int endX, int endY)
        {
            //Node below main
            if (mainNode.LocationY < worldSizeY - 1)
                if (coordinateSystem[mainNode.LocationX][mainNode.LocationY + 1] > 1 && coordinateSystem[mainNode.LocationX][mainNode.LocationY + 1] < 11)
                {
                    if (!CheckForDuplicates(ref openNodes, mainNode, 0, 1))
                        openNodes.Add(new Node(mainNode.LocationX, mainNode.LocationY + 1, endX, endY, mainNode.G));
                }

            //Node above main
            if (mainNode.LocationY > 0)
                if (coordinateSystem[mainNode.LocationX][mainNode.LocationY - 1] > 1 && coordinateSystem[mainNode.LocationX][mainNode.LocationY - 1] < 11)
                {
                    if (!CheckForDuplicates(ref openNodes, mainNode, 0, -1))
                        openNodes.Add(new Node(mainNode.LocationX, mainNode.LocationY - 1, endX, endY, mainNode.G));
                }

            //Node left of main
            if (mainNode.LocationX > 0)
                if (coordinateSystem[mainNode.LocationX - 1][mainNode.LocationY] > 1 && coordinateSystem[mainNode.LocationX - 1][mainNode.LocationY] < 11)
                {
                    if (!CheckForDuplicates(ref openNodes, mainNode, -1, 0))
                        openNodes.Add(new Node(mainNode.LocationX - 1, mainNode.LocationY, endX, endY, mainNode.G));
                }

            //Node right of main
            if (mainNode.LocationX < worldSizeX - 1)
                if (coordinateSystem[mainNode.LocationX + 1][mainNode.LocationY] > 1 && coordinateSystem[mainNode.LocationX + 1][mainNode.LocationY] < 11)
                {
                    if (!CheckForDuplicates(ref openNodes, mainNode, 1, 0))
                        openNodes.Add(new Node(mainNode.LocationX + 1, mainNode.LocationY, endX, endY, mainNode.G));
                }
        }

        #endregion
        /// <summary>
        /// Assigning values to positions in the coordinate system, defining which environment object will be placed on that coordinate.
        /// Then it places that environment object on the tile and adds the object to gameworld's list of environment objects
        /// </summary>
        /// <param name="coordinateSystem"></param>
        public void Randomizer(int[][] coordinateSystem, List<Environment> environmentList)
        {
            if (environmentList.Count == 0)
                environmentList.Clear();

            for (int x = 0; x <= worldSizeX; x++)
            {
                for (int y = 0; y <= worldSizeY; y++)
                {
                    //Giving grotto location a value of 1
                    if (x == grottoX && y == grottoY)
                    {
                        coordinateSystem[x][y] = 1;
                    }
                    //giving treasure location a value of 5
                    else if (x == treasureX && y == treasureY)
                    {
                        coordinateSystem[x][y] = 5;
                    }

                    //Giving value to all other tiles, randomizing if they will be water, rock, island or lighthouse
                    else
                    {
                        int temp = rnd.Next(1, 100);

                        //Water
                        if (temp >= 27 && temp <= 100)
                            coordinateSystem[x][y] = 10;
                        //Island
                        if (temp >= 10 && temp < 25)
                            coordinateSystem[x][y] = 11;
                        //Lighthouse
                        if (temp >= 25 && temp < 27)
                            coordinateSystem[x][y] = 12;
                        //Rock
                        if (temp >= 0 && temp < 10)
                            coordinateSystem[x][y] = 13;
                    }

                    //Giving checkpoints locations a value of 2 + checkpoint number, reaching a maximum value of 4 when there is 3 checkpoints in the map
                    if (checkpointList.Count() > 0)
                        for (int i = 0; i < checkpointList.Count; i++)
                        {
                            if (x == (int)checkpointList[i].X && y == (int)checkpointList[i].Y)
                            {
                                coordinateSystem[x][y] = 2 + i;
                            }
                        }
                }
            }

            //Placing environment
            for (int x = 0; x < worldSizeX; x++)
            {
                for (int y = 0; y < worldSizeY; y++)
                {
                    //Temp locations to make the code cleaner
                    int tempX = x * tileSizeX;
                    int tempY = y * tileSizeY;

                    //Placing grotto
                    if (coordinateSystem[x][y] == 1)
                    {
                        environmentList.Add(new Grotto(@"Graphic/GrottoPlaceHolder.png", new PointF(tempX, tempY), false));
                    }

                    //Checkpoints
                    if (coordinateSystem[x][y] == 2)
                    {
                        environmentList.Add(new Checkpoint(@"Graphic/Checkpoint1PlaceHolder.png", new PointF(tempX, tempY), false));
                    }
                    if (coordinateSystem[x][y] == 3)
                    {
                        environmentList.Add(new Checkpoint(@"Graphic/Checkpoint2PlaceHolder.png", new PointF(tempX, tempY), false));
                    }
                    if (coordinateSystem[x][y] == 4)
                    {
                        environmentList.Add(new Checkpoint(@"Graphic/Checkpoint3PlaceHolder.png", new PointF(tempX, tempY), false));
                    }

                    //Placing treasure chest
                    if (coordinateSystem[x][y] == 5)
                    {
                        environmentList.Add(new Treasure(@"Graphic/TreasureChest.png", new PointF(tempX, tempY), false));
                    }
                    //Placing Water
                    if (coordinateSystem[x][y] == 10)
                    {
                        environmentList.Add(new Water(@"Graphic/WaterPlaceHolder.png", new PointF(tempX, tempY), true));
                    }
                    //Placing Islands
                    if (coordinateSystem[x][y] == 11)
                    {
                        environmentList.Add(new Island(@"Graphic/LandResized.png", new PointF(tempX, tempY), true));
                    }
                    //Placing lighthouses
                    if (coordinateSystem[x][y] == 12)
                    {
                        environmentList.Add(new Lighthouse(@"Graphic/LighthousePlaceHolder.jpg", new PointF(tempX, tempY), false));
                    }
                    //Placing rocks
                    if (coordinateSystem[x][y] == 13)
                    {
                        environmentList.Add(new Rock(@"Graphic/RockPlaceHolder.jpg", new PointF(tempX, tempY), false));
                    }
                }
            }
        }
        /// <summary>
        /// start the the enemy wave 
        /// </summary>
        public void Wave()
        {
            foreach (List<Enemy> wave in waveEnemy)
            {
                listNumb++;
                if (listNumb == waveNumber)
                {
                    enemyWatch.Start();
                    foreach (Enemy enemy in wave)
                    {
                        while (enemyWatch.Elapsed.Milliseconds < 2500)
                        {
                            if (enemyWatch.Elapsed.Milliseconds > 2000)
                            {
                                currentWave.Add(enemy);
                                enemyWatch.Restart();
                                break;
                            }
                        }


                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Checks What phase The Player Is On
        /// </summary>
        public void GameState()
        {


            switch (currentState)
            {
                case State.build:

                    if (buildWatch.Elapsed.Milliseconds > 30000)
                    {
                        buildWatch.Stop();
                        buildWatch.Reset();
                        phase = "Wave";
                        currentState = State.wave;
                    }
                    break;
                case State.wave:

                    if (currentWave.Count == 0)
                    {
                        phase = "build phase";
                        buildWatch.Start();
                        currentState = State.build;

                    }
                    break;

            }
        }
        /// <summary>
        /// The Build Function
        /// </summary>
        public void Build(int towerNumb, PointF position)
        {
            switch (towerNumb)
            {
                #region Water
                // Water
                case 1:
                    towers.Add(new TowerSlow(20, 20, 20, 20, 20, @"Towers/w1.png", position, true));
                    gold -= cost;
                    break;

                case 2:
                    towers.Add(new TowerBoost(20, 20, 20, 20, 20, 20, @"Towers/w2.png", position, true));
                    gold -= cost;
                    break;
                case 3:
                    towers.Add(new TowerStun(20, 20, 20, 20, 20, @"Towers/w3.png", position, true));
                    gold -= cost;
                    break;
                #endregion
                #region Land
                // Land
                case 4:
                    towers.Add(new TowerBoost(20, 20, 20, 20, 20, 20, @"Towers/L1.png", position, true));
                    gold -= cost;
                    break;
                case 5:
                    towers.Add(new TowerBoost(20, 20, 20, 20, 20, 20, @"Towers/L2.png", position, true));
                    gold -= cost;
                    break;
                case 6:
                    towers.Add(new TowerBoost(20, 20, 20, 20, 20, 20, @"Towers/L3.png", position, true));
                    gold -= cost;
                    break;
                #endregion
            }
        }
        /// <summary>
        /// The Sell Function
        /// </summary>
        public void Sell(int towerSell, Tower tower)
        {
            switch (towerSell)
            {
                case 1:
                    towers.Remove(tower);
                    gold += cost;
                    break;
                case 2:
                    towers.Remove(tower);
                    gold += cost;
                    break;
                case 3:
                    towers.Remove(tower);
                    gold += cost;
                    break;
                case 4:
                    towers.Remove(tower);
                    gold += cost;
                    break;
                case 5:
                    towers.Remove(tower);
                    gold += cost;
                    break;
                case 6:
                    towers.Remove(tower);
                    break;
            }

        }
        /// <summary>
        /// Checking If The Enemies Are On A Checkpoing
        /// </summary>
        public void OnCheckpoint()
        {

        }
    }
}
