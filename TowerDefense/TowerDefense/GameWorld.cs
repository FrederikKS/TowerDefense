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
        public RectangleF mouseRect;
        public RectangleF guiRect = new RectangleF();
        public List<TowerButton> tl;
        Font w = new Font("Arial", 14);
        Brush q = new SolidBrush(Color.White);
        private int waveComplete;
        // Fields
        private Random rnd = new Random();
        private DateTime lastFrameStarted = new DateTime();
        private float currentFPS;
        private Graphics dc;
        private string phase = "Build Phase";
        private BufferedGraphics buffer;
        public State currentState = State.build;
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
        private int[][] tempCoordinateSystem;
        private int grottoX;
        private int grottoY;
        private int treasureX;
        private int treasureY;
        private int minDistStart = 3;
        private int numberOfCheckpoints;
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
        private List<List<PointF>> tempPath;
        private int waveNumber = 0;
        private int listNumb;
        private int checkPoint;
        private float chosenDif;

        //Timer for spawning enemies
        public Timer spawner;
        private int TimerEventCounter = 0;


        #endregion
        // Fields for building phase

        public int gold = 10000;
        private int cost;
        private int chest;
        bool afford = true;
        Timer tAfford;
        public float life;
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
            numberOfCheckpoints = rnd.Next(2, 4);
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


            // Bool for checking validity of tile location
            validLocation = CheckLocation(grottoX, grottoY, treasureX, treasureY, minDistStart);

            //If location is not valid (if start is too close to finish), find a new location
            while (!validLocation)
            {
                treasureX = rnd.Next(1, (int)worldSizeX - 1);
                treasureY = rnd.Next(1, (int)worldSizeY - 2);

                validLocation = CheckLocation(grottoX, grottoY, treasureX, treasureY, minDistStart);
            }

            //Location for checkpoints
            if (numberOfCheckpoints > 0)
                for (int i = 0; i < numberOfCheckpoints; i++)
                {
                    checkpointList[i] = new PointF(rnd.Next(1, (int)worldSizeX - 1), rnd.Next(1, (int)worldSizeY - 1));
                    //Making sure first checkpoint is not too close to start or treasurechest
                    bool grottoCheck = CheckLocation(grottoX, grottoY, (int)checkpointList[i].X, (int)checkpointList[i].Y, 2);
                    bool treasureCheck = CheckLocation(treasureX, treasureY, (int)checkpointList[i].X, (int)checkpointList[i].Y, 1);

                    //Making sure this checkpoints is not positioned on another checkpoint
                    bool otherCheckpointsCheck = true;
                    for (int x = 0; x < i; x++)
                    {
                        otherCheckpointsCheck = CheckLocation((int)checkpointList[i].X, (int)checkpointList[i].Y, (int)checkpointList[x].X, (int)checkpointList[x].Y, 1);
                    }

                    //Keep rolling new checkpoints until they aren't on top or close to grotto, treasure or other checkpoints
                    while (!grottoCheck || !treasureCheck || !otherCheckpointsCheck)
                    {
                        checkpointList[i] = new PointF(rnd.Next(1, (int)worldSizeX - 1), rnd.Next(1, (int)worldSizeY - 1));
                        grottoCheck = CheckLocation(grottoX, grottoY, (int)checkpointList[i].X, (int)checkpointList[i].Y, 2);
                        treasureCheck = CheckLocation(treasureX, treasureY, (int)checkpointList[i].X, (int)checkpointList[i].Y, 2);
                        for (int x = 0; x < i; x++)
                        {
                            otherCheckpointsCheck = CheckLocation((int)checkpointList[i].X, (int)checkpointList[i].Y, (int)checkpointList[x].X, (int)checkpointList[x].Y, 1);
                        }
                    }
                    //Adding position of checkpoint to list of endpoints used for saving paths
                    endPoints.Add(new PointF(checkpointList[i].X, checkpointList[i].Y));
                    //Adding position of checkpoint to list of startpoints used for saving paths
                    startPoints.Add(new PointF(checkpointList[i].X, checkpointList[i].Y));
                }


            //Adding treasure position to list of endpoints used for saving paths. Added as last position in list
            endPoints.Add(new PointF(treasureX, treasureY));

            #endregion

            #region Building the world layout

            //Assign values to positions in the coordinate system and place environment tiles accordingly while adding them to environment list
            Randomizer(coordinateSystem, environment);

            //Building enemy paths, depending on amount of checkpoints
            for (int i = 0; i < endPoints.Count; i++)
            {
                RoadBuilder(ref coordinateSystem, (int)startPoints[i].X, (int)startPoints[i].Y, (int)endPoints[i].X, (int)endPoints[i].Y, i);
            }

            //If a path cannot be found, perform generation again
            while (!CheckIfValidPath(pathAvailable, numberOfCheckpoints))
            {
                //Clearing list of path in case it's already been used before
                foreach (List<PointF> previousPath in path)
                {
                    previousPath.Clear();
                }
                //Clearing previous environment
                environment.Clear();

                //Perform world generation again
                Randomizer(coordinateSystem, environment);
                for (int i = 0; i < endPoints.Count; i++)
                {
                    RoadBuilder(ref coordinateSystem, (int)startPoints[i].X, (int)startPoints[i].Y, (int)endPoints[i].X, (int)endPoints[i].Y, i);
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
                        waveEnemy[i].Add(new EnemyNormal("TestEnemyNormal", 100 * chosenDif, 9, 0, 10, new Effect(@"Graphic/GrottoPlaceHolder.png", new PointF(0, 0), false), @"Graphic/heavyUp.png,Graphic/heavyLeft.png,Graphic/heavyDown.png,Graphic/heavyRight.png", new PointF(grottoX * tileSizeX, grottoY * tileSizeY), firstPoint, false));
                    if (enemyNumber % 3 == 1)
                        waveEnemy[i].Add(new EnemyEvade("TestEnemyEvade", false, 100 * chosenDif, 9, 0, 10, new Effect(@"Graphic/GrottoPlaceHolder.png", new PointF(0, 0), false), @"Graphic/EvadeResized.png,Graphic/EvadeResizedLeft.png,Graphic/EvadeResizedDown.png,Graphic/EvadeResizedRight.png", new PointF(grottoX * tileSizeX, grottoY * tileSizeY), firstPoint, false));
                    if (enemyNumber % 3 == 2)
                        waveEnemy[i].Add(new EnemySlow("TestEnemySlow", 10, 10, 100 * chosenDif, 9, 0, 10, new Effect(@"Graphic/GrottoPlaceHolder.png", new PointF(0, 0), false), @"Graphic/slowUp.png,Graphic/slowLeft.png,Graphic/slowDown.png,Graphic/slowRight.png", new PointF(grottoX * tileSizeX, grottoY * tileSizeY), firstPoint, false));
                }
            }

            //Starting buildwatch for the first time
            buildWatch.Start();
            switch (Form1.difc)
            {
                case 1:
                    life = 50;
                    break;
                case 2:
                    life = 30;
                    break;
                case 3:
                    life = 10;
                    break;
            }
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

                    }
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
            //Update all bullet objects
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].Target.Enabled)
                    bullets[i].Update(currentFPS);
                else
                {
                    bullets[i].Position = bullets[i].Tw.Position;
                    bullets.Remove(bullets[i]);
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
            UpdatePath(ref currentWave, ref endPoints, ref path);
            foreach (Enemy enemy in currentWave)
            {
                if (enemy.Enabled)
                {
                    enemy.Update(currentFPS);
                }
            }
            // Control when enemy wave is dead
            if (enemySent)
            {
                for (int i = 0; i < currentWave.Count; i++)
                {
                    if (currentWave[i].Enabled == false && currentWave[i].Check == false)
                    {
                        enemydisabled++;
                        currentWave[i].Check = true;
                    }
                }
                if (enemydisabled == currentWave.Count)
                {
                    enemyDead = true;
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
                if (currentState == State.build)
                {
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
            if (bullets.Count > 0)
                for (int i = 0; i < bullets.Count; i++)
                {
                    bullets[i].Draw(dc);
                }

            // Drawing Phase count


            dc.DrawString(string.Format("Phase: {0}", phase), w, q, 30, 5);

            // Draw Gold count

            dc.DrawString(string.Format("Gold: {0}", gold), w, q, 30, 45);
            // Draw life count

            dc.DrawString(string.Format("Life: {0}", life), w, q, 30, 65);
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
            if (afford == false)
            {
                dc.DrawString(string.Format("You cant afford this tower!"), w, q, 100, 100);
            }
            // Lost
            if (life <= 0)
            {
                environment.Clear();
                towers.Clear();
                tmpTowers.Clear();
                checkpointList.Clear();
                endPoints.Clear();
                startPoints.Clear();
                SolidBrush sb = new SolidBrush(Color.Black);
                dc.FillRectangle(sb, 0, 0, Form1.ActiveForm.Size.Width, Form1.ActiveForm.Size.Height);
                dc.DrawString(string.Format("You are dead"), w, q, Form1.ActiveForm.Size.Width / 2, Form1.ActiveForm.Size.Height / 2);
                dc.DrawString(string.Format("Press ESC to exit the game!"), w, q, Form1.ActiveForm.Size.Width / 2, Form1.ActiveForm.Size.Height / 2 + 30);
            }
            // Won
            if (Form1.difc * 10 == waveComplete)
            {
                environment.Clear();
                towers.Clear();
                tmpTowers.Clear();
                checkpointList.Clear();
                endPoints.Clear();
                startPoints.Clear();
                SolidBrush sb = new SolidBrush(Color.Black);
                dc.FillRectangle(sb, 0, 0, Form1.ActiveForm.Size.Width, Form1.ActiveForm.Size.Height);
                dc.DrawString(string.Format("You Won!"), w, q, Form1.ActiveForm.Size.Width / 2, Form1.ActiveForm.Size.Height / 2);
                dc.DrawString(string.Format("Press ESC to exit the game!"), w, q, Form1.ActiveForm.Size.Width / 2, Form1.ActiveForm.Size.Height / 2 + 30);

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

        public void RoadBuilder(ref int[][] coordinateSystem, int startX, int startY, int endX, int endY, int pathNumber)
        {
            //Creating a list of nodes
            Node mainNode;
            List<Node> openNodes = new List<Node>();
            List<Node> closedNodes = new List<Node>();
            mainNode = new Node(startX, startY, endX, endY, -1);
            openNodes.Add(mainNode);

            //Check nearby nodes
            CheckNearbyNodes(ref coordinateSystem, ref mainNode, ref openNodes, ref closedNodes, startX, startY, endX, endY);
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
                CheckNearbyNodes(ref coordinateSystem, ref mainNode, ref openNodes, ref closedNodes, startX, startY, endX, endY);
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

                        break;
                    }
                    // Right
                    if (node.LocationX == tempNode.LocationX + 1 && node.LocationY == tempNode.LocationY && node.G == steps - counter)
                    {
                        path[pathNumber].Add(new PointF(node.LocationX * tileSizeX, node.LocationY * tileSizeY));
                        counter++;
                        tempNode = node;

                        break;
                    }
                    // Down
                    if (node.LocationX == tempNode.LocationX && node.LocationY == tempNode.LocationY - 1 && node.G == steps - counter)
                    {
                        path[pathNumber].Add(new PointF(node.LocationX * tileSizeX, node.LocationY * tileSizeY));
                        counter++;
                        tempNode = node;

                        break;
                    }
                    // Up
                    if (node.LocationX == tempNode.LocationX && node.LocationY == tempNode.LocationY + 1 && node.G == steps - counter)
                    {
                        path[pathNumber].Add(new PointF(node.LocationX * tileSizeX, node.LocationY * tileSizeY));
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
        public void CheckNearbyNodes(ref int[][] coordinateSystem, ref Node mainNode, ref List<Node> openNodes, ref List<Node> closedNodes, int startX, int startY, int endX, int endY)
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
                        environmentList.Add(new Checkpoint(@"Graphic/Checkpoint1Resized.png", new PointF(tempX, tempY), false));
                    }
                    if (coordinateSystem[x][y] == 3)
                    {
                        environmentList.Add(new Checkpoint(@"Graphic/Checkpoint2Resized.png", new PointF(tempX, tempY), false));
                    }
                    if (coordinateSystem[x][y] == 4)
                    {
                        environmentList.Add(new Checkpoint(@"Graphic/Checkpoint3Resized.png", new PointF(tempX, tempY), false));
                    }

                    //Placing treasure chest
                    if (coordinateSystem[x][y] == 5)
                    {
                        environmentList.Add(new Treasure(@"Graphic/TreasureResized.png", new PointF(tempX, tempY), false));
                    }
                    //Placing Water
                    if (coordinateSystem[x][y] == 10)
                    {
                        environmentList.Add(new Water(@"Graphic/water.png", new PointF(tempX, tempY), true));
                    }
                    //Placing Islands
                    if (coordinateSystem[x][y] == 11)
                    {
                        environmentList.Add(new Island(@"Graphic/land.png", new PointF(tempX, tempY), true));
                    }
                    //Placing lighthouses
                    if (coordinateSystem[x][y] == 12)
                    {
                        environmentList.Add(new Lighthouse(@"Graphic/lighthouse.png", new PointF(tempX, tempY), false));
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
                        environmentList.Add(new Rock(@"Graphic/WallLeft.png", new PointF(tempX, tempY), false));
                    }
                    //Right
                    if (coordinateSystem[x][y] == 102)
                    {
                        environmentList.Add(new Rock(@"Graphic/WallRight.png", new PointF(tempX, tempY), false));
                    }
                    //Top
                    if (coordinateSystem[x][y] == 103)
                    {
                        environmentList.Add(new Rock(@"Graphic/WallTop.png", new PointF(tempX, tempY), false));
                    }
                    //Bot
                    if (coordinateSystem[x][y] == 104)
                    {
                        environmentList.Add(new Rock(@"Graphic/WallBot.png", new PointF(tempX, tempY), false));
                    }

                    //Placing Corners
                    //Top Left
                    if (coordinateSystem[x][y] == 105)
                    {
                        environmentList.Add(new Rock(@"Graphic/CornerTopLeft.png", new PointF(tempX, tempY), false));
                    }
                    //Top Right
                    if (coordinateSystem[x][y] == 106)
                    {
                        environmentList.Add(new Rock(@"Graphic/CornerTopRight.png", new PointF(tempX, tempY), false));
                    }
                    //Bot Left
                    if (coordinateSystem[x][y] == 107)
                    {
                        environmentList.Add(new Rock(@"Graphic/CornerBotLeft.png", new PointF(tempX, tempY), false));
                    }
                    //Bot Right
                    if (coordinateSystem[x][y] == 108)
                    {
                        environmentList.Add(new Rock(@"Graphic/CornerBotRight.png", new PointF(tempX, tempY), false));
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
            spawner = new Timer(3000);
            spawner.Enabled = true;
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
                    if (!buildWatch.IsRunning)
                    {
                        buildWatch.Start();
                    }

                    if (buildWatch.Elapsed.Seconds > 15)
                    {
                        buildWatch.Stop();
                        buildWatch.Reset();
                        phase = "Wave";
                        waveNumber++;
                        StartWave();
                        currentState = State.wave;
                    }
                    break;

                case State.wave:

                    if (enemyDead == true)
                    {
                        phase = "Build phase";
                        enemyDead = false;
                        enemySent = false;
                        enemydisabled = 0;
                        currentState = State.build;
                        waveComplete++;
                    }
                    break;


            }
        }
        /// <summary>
        /// The Build Function
        /// </summary>
        public void Build(int towerNumb, PointF position)
        {
            bool allowTower = true;
            int tmpX = Form1.gui.ellipse.X + tileSizeX + (tileSizeX / 2);
            int tmpY = Form1.gui.ellipse.Y + tileSizeY + (tileSizeY / 2);
            //Saving current path
            tempPath = DuplicateList(path);

            //Clearing current path
            foreach (List<PointF> previousPath in path)
            {
                previousPath.Clear();
            }

            //Clearing previous validPath bools
            for (int i = 0; i < checkpointList.Count + 1; i++)
            {
                pathAvailable[i] = true;
            }

            //Resetting tempCoordinate System to prevent blocking towers from having a value
            if (tempCoordinateSystem != null)
                tempCoordinateSystem = null;

            //taking a temporary copy of the coordinate system to check if a valid path is available after a tower has been built
            tempCoordinateSystem = DuplicateCoordinateSystem(coordinateSystem);
            tempCoordinateSystem[(int)position.X / tileSizeX][(int)position.Y / tileSizeY] = 110;

            //Building enemy paths, depending on amount of checkpoints
            for (int i = 0; i < endPoints.Count; i++)
            {
                RoadBuilder(ref tempCoordinateSystem, (int)startPoints[i].X, (int)startPoints[i].Y, (int)endPoints[i].X, (int)endPoints[i].Y, i);
            }

            //If path isn't available, use the old path instead and do not allow player to place a tower
            if (!CheckIfValidPath(pathAvailable, numberOfCheckpoints))
            {
                path = DuplicateList(tempPath);

                //Clearing current path
                foreach (List<PointF> oldTempPath in tempPath)
                {
                    oldTempPath.Clear();
                }
                allowTower = false;
            }
            else
            {
                coordinateSystem[(int)position.X / tileSizeX][(int)position.Y / tileSizeY] = 110;
                allowTower = true;
            }

            if (allowTower)
                switch (towerNumb)
                {
                    #region Water
                    // Water
                    case 1:
                        if (gold >= 25)
                        {
                            towers.Add(new TowerSlow(2, tileSizeX * 3, 1000, 25, tileSizeX * 3, @"Towers/shipTower.png", position, true));
                            gold -= towers[0].Cost;
                        }
                        else
                        {
                            CantAfford();
                        }
                        break;

                    case 2:
                        if (gold >= 40)
                        {
                            towers.Add(new TowerBoost(10, 3, 1000, 35, 6, @"Towers/WhirlPool.png", position, true));
                            gold -= towers[0].Cost;
                        }
                        else
                        {
                            CantAfford();
                        }
                        break;

                    case 3:
                        if (gold >= 30)
                        {
                            towers.Add(new TowerStun(2, 1000, 40, 7, @"Towers/w3.png", position, true));
                            gold -= towers[0].Cost;
                        }
                        else
                        {
                            CantAfford();
                        }
                        break;
                    #endregion
                    #region Land
                    // Land
                    case 4:
                        if (gold >= 25)
                        {
                            towers.Add(new TowerBoost(2, 3, 5, 35, 6, @"Towers/ArrowTower.png", position, true));
                            gold -= towers[0].Cost;
                        }
                        else
                        {
                            CantAfford();
                        }
                        break;
                    case 5:
                        if (gold >= 40)
                        {
                            towers.Add(new TowerSlow(2, 5, 5, 25, 6, @"Towers/L2.png", position, true));
                            gold -= towers[0].Cost;
                        }
                        else
                        {
                            CantAfford();
                        }
                        break;
                    case 6:
                        if (gold >= 30)
                        {
                            towers.Add(new TowerStun(2, 5, 40, 7, @"Towers/L3.png", position, true));
                            gold -= towers[0].Cost;
                        }
                        else
                        {
                            CantAfford();
                        }
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
                    gold += t.Cost / 2;
                    towers.Remove(t);
                }
            }


        }
        /// <summary>
        /// Updating the enemies' endposition, the point they will try to reach
        /// </summary>
        public void UpdatePath(ref List<Enemy> currentWave, ref List<PointF> endPoints, ref List<List<PointF>> path)
        {
            foreach (Enemy enemy in currentWave)
            {
                if (enemy.Enabled)
                {
                    //Check if enemy is positioned on top of his current endposition
                    if (enemy.Position == enemy.EndPosition)
                    {
                        enemy.ReachedPointCounter++;

                        if (endPoints.Count == enemy.ReachedEndCounter)
                        {
                            enemy.Enabled = false;
                            life--;
                        }

                        //Check if enemy has been on all the points between his starting position and his end position
                        if (enemy.Enabled)
                        {
                            if (enemy.ReachedPointCounter < path[enemy.ReachedEndCounter].Count + 1)
                            {
                                enemy.EndPosition = path[enemy.ReachedEndCounter][path[enemy.ReachedEndCounter].Count - enemy.ReachedPointCounter];
                            }

                            //If enemy has been on all points between start and end, set endposition to a point from endPoints list and reset reachedPointCounter.
                            if (enemy.ReachedPointCounter == path[enemy.ReachedEndCounter].Count + 1)
                            {
                                enemy.EndPosition = new PointF(endPoints[enemy.ReachedEndCounter].X * tileSizeX, endPoints[enemy.ReachedEndCounter].Y * tileSizeY);
                                enemy.ReachedEndCounter++;
                                enemy.ReachedPointCounter = 0;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Making a deep copy of a list
        /// </summary>
        /// <param name="listToDuplicate"></param>
        /// <returns></returns>
        private List<List<PointF>> DuplicateList(List<List<PointF>> listToDuplicate)
        {
            List<List<PointF>> newList = new List<List<PointF>>();
            for (int i = 0; i < listToDuplicate.Count; i++)
            {
                newList.Add(new List<PointF>());
                for (int x = 0; x < listToDuplicate[i].Count; x++)
                {
                    newList[i].Add(new PointF(listToDuplicate[i][x].X, listToDuplicate[i][x].Y));
                }
            }

            return newList;
        }

        //Duplicates a 2D jagged array to prevent cross references
        private int[][] DuplicateCoordinateSystem(int[][] target)
        {
            int[][] newCoordinateSystem = new int[target.Count()][];
            for (int i = 0; i < target.Count(); i++)
            {
                newCoordinateSystem[i] = new int[target[i].Count()];
                for (int x = 0; x < target[i].Count(); x++)
                {
                    newCoordinateSystem[i][x] = target[i][x];
                }
            }
            return newCoordinateSystem;
        }
        private void CantAfford()
        {
            tAfford = new Timer(3000);
            tAfford.Enabled = true;
            afford = false;
            tAfford.Elapsed += new ElapsedEventHandler(CantAffordTrue);

        }
        private void CantAffordTrue(object source, ElapsedEventArgs e)
        {
            afford = true;
            tAfford.Enabled = false;
        }
    }


}