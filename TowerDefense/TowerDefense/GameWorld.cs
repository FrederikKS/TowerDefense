using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Timers;


namespace TowerDefense
{
    public enum State { build, wave }

    public class GameWorld
    {
        TowerButton tb;
        public RectangleF mouseRect;
        public RectangleF guiRect = new RectangleF();
        public List<TowerButton> tl;
        GameObject go;
        // Fields
        private Random rnd = new Random();
        private DateTime lastFrameStarted = new DateTime();
        private float currentFPS;
        private Graphics dc;
        private string phase = "Build Phase";
        private BufferedGraphics buffer;
        private State currentState = State.build;
        private Stopwatch stopWatch = new Stopwatch();
        private Stopwatch buildWatch = new Stopwatch();
        private Stopwatch enemyWatch = new Stopwatch();
        public List<Projectile> bullets = new List<Projectile>();
        private bool enemySent = false;
        private bool enemyDead = false;
        private int enemydisabled = 0;
        // Fields for world creation
        private float worldSizeX;
        private float worldSizeY;
        public int tileSizeX = 96;
        public int tileSizeY = 96;
        private int offsetX = 96 / 2;
        private int offsetY = 96 / 2;
        private int[][] coordinateSystem;
        private int grottoX;
        private int grottoY;
        private int treasureX;
        private int treasureY;
        private int minDistStart = 3;
        public List<Environment> environment = new List<Environment>();
        public List<Tower> towers = new List<Tower>();
        public List<Tower> tmpTowers = new List<Tower>();
        private List<PointF> checkpointList = new List<PointF>();
        public List<PointF> endPoints = new List<PointF>();
        private List<PointF> startPoints = new List<PointF>();
        private bool validLocation;

        #region Fields for wave
        // Fields for wave
        List<List<Enemy>> waveEnemy = new List<List<Enemy>>();
        private List<int> waveCount = new List<int>();
        public List<Enemy> currentWave = new List<Enemy>();
        List<bool> pathAvailable = new List<bool>();
        public List<List<PointF>> path = new List<List<PointF>>();
        private int waveNumber = 0;
        private int listNumb;
        private int checkPoint;
        private float chosenDif;

        //Timer for spawning enemies
        public Timer spawner = new Timer(60000);
        private int TimerEventCounter = 0;


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
            int numberOfCheckpoints = rnd.Next(2, 4);
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
                    checkpointList[i] = new PointF(rnd.Next(1, (int)worldSizeX - 1), rnd.Next(1, (int)worldSizeY - 1));
                    //Making sure first checkpoint is not too close to start
                    bool tempPosCheck = CheckLocation(grottoX, grottoY, (int)checkpointList[i].X, (int)checkpointList[i].Y, 2);
                    while (!tempPosCheck)
                    {
                        checkpointList[i] = new PointF(rnd.Next(1, (int)worldSizeX - 1), rnd.Next(1, (int)worldSizeY - 1));
                        tempPosCheck = CheckLocation(grottoX, grottoY, (int)checkpointList[i].X, (int)checkpointList[i].Y, 2);
                    }
                    //Adding position of checkpoint to list of endpoints used for saving paths
                    endPoints.Add(new PointF(checkpointList[i].X, checkpointList[i].Y));
                    //Adding position of checkpoint to list of startpoints used for saving paths
                    startPoints.Add(new PointF(checkpointList[i].X, checkpointList[i].Y));
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

            //First point enemy should move to, in case first path is empty
            PointF firstPoint;
            if (path[0].Count > 0)
                firstPoint = path[0].Last();
            else
                firstPoint = new PointF(endPoints[0].X * tileSizeX, endPoints[0].Y * tileSizeY);

            //Instantiating and adding enemies to waves list
            for (int i = 0; i < 20; i++)
            {
                waveEnemy.Add(new List<Enemy>());

                for (int enemyNumber = 0; enemyNumber < 10; enemyNumber++)
                {
                    if (enemyNumber % 3 == 0)
                        waveEnemy[i].Add(new EnemyNormal("TestEnemyNormal", 100 * chosenDif, 3, 0, 10, new Effect(@"Graphic/GrottoPlaceHolder.png", new PointF(0, 0), false), @"Graphic/96.jpg", new PointF(grottoX * tileSizeX, grottoY * tileSizeY), firstPoint, false));
                    if (enemyNumber % 3 == 1)
                        waveEnemy[i].Add(new EnemyEvade("TestEnemyEvade", false, 100 * chosenDif, 3, 0, 10, new Effect(@"Graphic/GrottoPlaceHolder.png", new PointF(0, 0), false), @"Graphic/Resized/EvadeResized.png", new PointF(grottoX * tileSizeX, grottoY * tileSizeY), firstPoint, false));
                    if (enemyNumber % 3 == 2)
                        waveEnemy[i].Add(new EnemySlow("TestEnemySlow", 10, 10, 100 * chosenDif, 3, 0, 10, new Effect(@"Graphic/GrottoPlaceHolder.png", new PointF(0, 0), false), @"Graphic/Resized/SlowResized.png,Graphic/Resized/SlowResizedDown.png,Graphic/Resized/SlowResizedLeft.png,Graphic/Resized/SlowResizedRight.png", new PointF(grottoX * tileSizeX, grottoY * tileSizeY), firstPoint, false));
                }
            }

            //Starting buildwatch for the first time
            buildWatch.Start();
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

        }
        /// <summary>
        /// Updates the gameworld on every frame
        /// </summary>
        public void Update()
        {
            if (enemySent)
            {
                for (int i = 0; i < currentWave.Count; i++)
                {
                    if (currentWave[i].Enabled == false)
                    {
                        enemydisabled++;
                    }
                }
                if (enemydisabled == 10)
                {
                    enemyDead = true;
                }
            }
            //Update mouse rectangle pos
            mouseRect.Location = Form1.localMousePos;
            foreach (TowerButton tb in tl)
            {
                tmpTowers = towers.ToList();
                if (mouseRect.IntersectsWith(tb.CollisionRect))
                {
                    int x = (((int)Math.Floor((decimal)Form1.guiPos.X / tileSizeX)) * tileSizeX);
                    int y = (((int)Math.Floor((decimal)Form1.guiPos.Y / tileSizeY)) * tileSizeY);
                    if (!towers.Any(t => t.Position.X == x && t.Position.Y == y))
                    {
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
                        #region Sell
                        foreach (Tower tower in tmpTowers)
                        {
                            if (guiRect.IntersectsWith(tower.CollisionRect))
                            {
                                if (Form1.drawBuildGUI == 2)
                                {
                                    Sell(tower);
                                }
                            }
                        }
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
                if (enemy.Enabled)
                {
                    enemy.Update(currentFPS);
                    UpdatePath(enemy, ref endPoints, ref path);
                }
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

                int tmpX = Form1.gui.ellipse.X + tileSizeX + (tileSizeX / 2);
                int tmpY = Form1.gui.ellipse.Y + tileSizeY + (tileSizeY / 2);
                guiRect = new RectangleF(tmpX, tmpY, 10, 10);
                if (Form1.drawBuildGUI == 4)
                {
                    if (tl.Count < 3)
                    {
                        tl.Add(new TowerButton(new Size(75, 75), new Point(tmpX + 50, tmpY), "Land_Tower1", "Towers/w1.png", 1));
                        tl.Add(new TowerButton(new Size(75, 75), new Point(tmpX - 120, tmpY), "Land_Tower2", "Towers/w2.png", 2));
                        tl.Add(new TowerButton(new Size(75, 75), new Point(tmpX - 30, tmpY - 120), "Land_Tower3", "Towers/w3.png", 3));
                    }
                }
                if (Form1.drawBuildGUI == 3)
                {
                    if (tl.Count < 3)
                    {
                        tl.Add(new TowerButton(new Size(75, 75), new Point(tmpX + 50, tmpY), "Water_Tower1", "Towers/w1.png", 1));
                        tl.Add(new TowerButton(new Size(75, 75), new Point(tmpX - 120, tmpY), "Water_Tower2", "Towers/w2.png", 2));
                        tl.Add(new TowerButton(new Size(75, 75), new Point(tmpX - 30, tmpY - 120), "Water_Tower3", "Towers/w3.png", 3));
                    }
                }
                if (Form1.drawBuildGUI == 2)
                {
                    if (tl.Count < 1)
                    {
                        tl.Add(new TowerButton(new Size(75, 75), new Point(tmpX - 30, tmpY - 120), "Sell_Tower", "Towers/st.png", 3));
                    }
                }
            }
            else
            {
                guiRect = new RectangleF(0, 0, 10, 10);
            }
            dc.Clear(Color.White);

            //Drawing environment
            for (int i = 0; i < environment.Count; i++)
            {
                environment[i].Draw(dc);
            }
            //Drawing towers
            for (int i = 0; i < towers.Count; i++)
            {
                towers[i].Draw(dc);
            }
            //Drawing enemies
            for (int i = 0; i < currentWave.Count; i++)
            {
                if (currentWave[i].Enabled)
                    currentWave[i].Draw(dc);
            }
            // Drawing Bullets
            //for (int i = 0; i < bullets.Count; i++)
            //{
            //    bullets[i].Draw(dc);
            //}

            Font w = new Font("Arial", 14);
            Brush q = new SolidBrush(Color.White);
            dc.DrawString(string.Format("Phase: {0}", phase), w, q, 30, 5);

            //Draw timer if build phase is on
            if (Enum.IsDefined(typeof(State), State.build))
            {
                dc.DrawString(string.Format("Time: {0}", buildWatch.Elapsed.Seconds), w, q, 30, 25);
            }

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
                    // Left
                    if (node.LocationX == tempNode.LocationX - 1 && node.LocationY == tempNode.LocationY && node.G == steps - counter)
                    {
                        path[pathNumber].Add(new PointF(node.LocationX * tileSizeX, node.LocationY * tileSizeY));
                        counter++;
                        tempNode = node;
                        if (currentWave.Count >= 1)
                        {
                            go.sprite = Image.FromFile(@"Graphic/Resized/SlowResizedLeft.png");
                        }

                        break;
                    }
                    // Right
                    if (node.LocationX == tempNode.LocationX + 1 && node.LocationY == tempNode.LocationY && node.G == steps - counter)
                    {
                        path[pathNumber].Add(new PointF(node.LocationX * tileSizeX, node.LocationY * tileSizeY));
                        counter++;
                        tempNode = node;
                        if (currentWave.Count >= 1)
                        {
                            go.sprite = Image.FromFile(@"Graphic/Resized/SlowResizedRight.png");
                        }

                        break;
                    }
                    // Down
                    if (node.LocationX == tempNode.LocationX && node.LocationY == tempNode.LocationY - 1 && node.G == steps - counter)
                    {
                        path[pathNumber].Add(new PointF(node.LocationX * tileSizeX, node.LocationY * tileSizeY));
                        counter++;
                        tempNode = node;
                        if (currentWave.Count >= 1)
                        {
                            go.sprite = Image.FromFile(@"Graphic/Resized/SlowResizedDown.png");
                        }

                        break;
                    }
                    // Up
                    if (node.LocationX == tempNode.LocationX && node.LocationY == tempNode.LocationY + 1 && node.G == steps - counter)
                    {
                        path[pathNumber].Add(new PointF(node.LocationX * tileSizeX, node.LocationY * tileSizeY));
                        counter++;
                        tempNode = node;
                        if (currentWave.Count >= 1)
                        {
                            go.sprite = Image.FromFile(@"Graphic/Resized/SlowResized.png");
                        }

                        break;
                    }
                }
            }
            if (path[pathNumber].Count > 0)
                path[pathNumber].Remove(path[pathNumber].Last());
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

                    //Giving value to wall tiles
                    //Left Wall
                    if (x == 0 && y > 0 && y < worldSizeY - 1 && coordinateSystem[x][y] != 1)
                        coordinateSystem[x][y] = 101;

                    //Right Wall
                    if (x == worldSizeX - 1 && y > 0 && y < worldSizeY - 1 && coordinateSystem[x][y] != 1)
                        coordinateSystem[x][y] = 102;

                    //Top Wall
                    if (x > 0 && y == 0 && x < worldSizeX - 1 && coordinateSystem[x][y] != 1)
                        coordinateSystem[x][y] = 103;

                    //Bot Wall
                    if (x > 0 && y == worldSizeY - 1 && x < worldSizeX - 1 && coordinateSystem[x][y] != 1)
                        coordinateSystem[x][y] = 104;

                    //Giving value to corner tiles
                    //Top left
                    if (x == 0 && y == 0)
                        coordinateSystem[x][y] = 105;
                    //Top right
                    if (x == worldSizeX - 1 && y == 0)
                        coordinateSystem[x][y] = 106;
                    //Bottom left
                    if (x == 0 && y == worldSizeY - 1)
                        coordinateSystem[x][y] = 107;
                    //Bottom right
                    if (x == worldSizeX - 1 && y == worldSizeY - 1)
                        coordinateSystem[x][y] = 108;

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
                        environmentList.Add(new Checkpoint(@"Graphic/Resized/Checkpoint1Resized.png", new PointF(tempX, tempY), false));
                    }
                    if (coordinateSystem[x][y] == 3)
                    {
                        environmentList.Add(new Checkpoint(@"Graphic/Resized/Checkpoint2Resized.png", new PointF(tempX, tempY), false));
                    }
                    if (coordinateSystem[x][y] == 4)
                    {
                        environmentList.Add(new Checkpoint(@"Graphic/Resized/Checkpoint3Resized.png", new PointF(tempX, tempY), false));
                    }

                    //Placing treasure chest
                    if (coordinateSystem[x][y] == 5)
                    {
                        environmentList.Add(new Treasure(@"Graphic/Resized/TreasureResized.png", new PointF(tempX, tempY), false));
                    }
                    //Placing Water
                    if (coordinateSystem[x][y] == 10)
                    {
                        environmentList.Add(new Water(@"Graphic/Resized/WaterResized.png", new PointF(tempX, tempY), true));
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
                        environmentList.Add(new Rock(@"Graphic/Rock.png", new PointF(tempX, tempY), false));
                    }

                    //Placing Walls
                    //Left
                    if (coordinateSystem[x][y] == 101)
                    {
                        environmentList.Add(new Rock(@"Graphic/Resized/WallLeft.png", new PointF(tempX, tempY), false));
                    }
                    //Right
                    if (coordinateSystem[x][y] == 102)
                    {
                        environmentList.Add(new Rock(@"Graphic/Resized/WallRight.png", new PointF(tempX, tempY), false));
                    }
                    //Top
                    if (coordinateSystem[x][y] == 103)
                    {
                        environmentList.Add(new Rock(@"Graphic/Resized/WallTop.png", new PointF(tempX, tempY), false));
                    }
                    //Bot
                    if (coordinateSystem[x][y] == 104)
                    {
                        environmentList.Add(new Rock(@"Graphic/Resized/WallBot.png", new PointF(tempX, tempY), false));
                    }

                    //Placing Corners
                    //Top Left
                    if (coordinateSystem[x][y] == 105)
                    {
                        environmentList.Add(new Rock(@"Graphic/Resized/CornerTopLeft.png", new PointF(tempX, tempY), false));
                    }
                    //Top Right
                    if (coordinateSystem[x][y] == 106)
                    {
                        environmentList.Add(new Rock(@"Graphic/Resized/CornerTopRight.png", new PointF(tempX, tempY), false));
                    }
                    //Bot Left
                    if (coordinateSystem[x][y] == 107)
                    {
                        environmentList.Add(new Rock(@"Graphic/Resized/CornerBotLeft.png", new PointF(tempX, tempY), false));
                    }
                    //Bot Right
                    if (coordinateSystem[x][y] == 108)
                    {
                        environmentList.Add(new Rock(@"Graphic/Resized/CornerBotRight.png", new PointF(tempX, tempY), false));
                    }
                }
            }
        }

        /// <summary>
        /// Starts the current wave
        /// </summary>
        public void StartWave()
        {
            currentWave = waveEnemy[waveNumber];
            spawner.Enabled = true;
            spawner.Interval = 3000;
            spawner.Elapsed += new ElapsedEventHandler(SpawnWave);
        }

        public void SpawnWave(object source, ElapsedEventArgs e)
        {
            if (TimerEventCounter == waveEnemy[waveNumber].Count)
            {
                spawner.Enabled = false;
                TimerEventCounter = 0;
                enemySent = true;
            }

            if (spawner.Enabled)
            {
                currentWave[TimerEventCounter].Enabled = true;
                TimerEventCounter++;
            }
        }

        /// <summary>
        /// Lucas
        /// Checks What phase The Player Is On
        /// </summary>
        public void GameState()
        {
            switch (currentState)
            {
                case State.build:

                    if (buildWatch.Elapsed.Seconds > 2)
                    {
                        buildWatch.Stop();
                        buildWatch.Reset();
                        phase = "Wave";
                        currentState = State.wave;
                        StartWave();
                        waveNumber++;
                    }
                    break;

                case State.wave:

                    if (enemyDead == true)
                    {
                        if (!buildWatch.IsRunning)
                            buildWatch.Start();

                        if (buildWatch.Elapsed.Seconds > 5)
                        {
                            phase = "Build phase";
                            buildWatch.Reset();
                            currentState = State.build;
                        }

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
                    towers.Add(new TowerSlow(2, tileSizeX * 3, 5, 25, tileSizeX * 3, @"Towers/w1.png", position, true));
                    towers[0].Bullet = new Projectile(100, 100, @"Towers/w2.png", position, false, towers[0]);
                    gold -= cost;
                    break;

                case 2:
                    towers.Add(new TowerBoost(2, 3, 5, 35, 6, @"Towers/w2.png", position, true));
                    towers[0].Bullet = new Projectile(10, 100, @"Towers/w2.png", position, false, towers[0]);
                    gold -= cost;
                    break;
                case 3:
                    towers.Add(new TowerStun(2, 5, 40, 7, @"Towers/w3.png", position, true));
                    towers[0].Bullet = new Projectile(10, 5, @"Towers/w2.png", position, false, towers[0]);
                    gold -= cost;
                    break;
                #endregion
                #region Land
                // Land
                case 4:
                    towers.Add(new TowerBoost(2, 3, 5, 35, 6, @"Towers/L1.png", position, true));
                    towers[0].Bullet = new Projectile(10, 5, @"Towers/w2.png", position, false, towers[0]);
                    gold -= cost;
                    break;
                case 5:
                    towers.Add(new TowerSlow(2, 5, 5, 25, 6, @"Towers/L2.png", position, true));
                    towers[0].Bullet = new Projectile(10, 5, @"Towers/w2.png", position, false, towers[0]);
                    gold -= cost;
                    break;
                case 6:
                    towers.Add(new TowerStun(2, 5, 40, 7, @"Towers/L3.png", position, true));
                    towers[0].Bullet = new Projectile(10, 5, @"Towers/w2.png", position, false, towers[0]);
                    gold -= cost;
                    break;
                #endregion
            }
        }
        /// <summary>
        /// The Sell Function
        /// </summary>
        public void Sell(Tower tower)
        {
            tmpTowers = towers.ToList();

            foreach (Tower t in tmpTowers)
            {
                if (t == tower)
                {
                    gold += t.Cost;
                    towers.Remove(t);
                }
            }


        }
        /// <summary>
        /// Updating the enemies' endposition, the point they will try to reach
        /// </summary>
        public void UpdatePath(Enemy enemy, ref List<PointF> endPoints, ref List<List<PointF>> path)
        {
            if (enemy.Enabled)
            {
                //Check if enemy is positioned on top of his current endposition
                if (enemy.Position == enemy.EndPosition)
                {
                    if (endPoints.Count == enemy.ReachedEndCounter)
                    {
                        enemy.Enabled = false;
                    }

                    //Check if enemy has been on all the points between his starting position and his end position
                    if (enemy.Enabled)
                    {
                        if (enemy.ReachedPointCounter != path[enemy.ReachedEndCounter].Count)
                        {
                            enemy.ReachedPointCounter++;
                            enemy.EndPosition = path[enemy.ReachedEndCounter][path[enemy.ReachedEndCounter].Count - enemy.ReachedPointCounter];

                        }

                        //If enemy has been on all points between start and end, set endposition to a point from endPoints list and reset reachedPointCounter.
                        if (enemy.ReachedPointCounter == path[enemy.ReachedEndCounter].Count)
                        {
                            enemy.ReachedPointCounter = 0;
                            enemy.EndPosition = new PointF(endPoints[enemy.ReachedEndCounter].X * tileSizeX, endPoints[enemy.ReachedEndCounter].Y * tileSizeY);
                            enemy.ReachedEndCounter++;
                        }
                    }
                }
            }
        }
    }
}
