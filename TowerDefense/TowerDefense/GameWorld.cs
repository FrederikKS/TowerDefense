using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Timers;
using IrrKlang;


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
        public int offsetX = 96 / 2;
        public int offsetY = 96 / 2;
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
        ISoundEngine engine = new ISoundEngine();

        #region Fields for wave
        // Fields for wave
        List<List<Enemy>> waveEnemy = new List<List<Enemy>>();
        private List<int> waveCount = new List<int>();
        public List<Enemy> currentWave = new List<Enemy>();
        List<bool> pathAvailable = new List<bool>();
        public List<List<PointF>> path = new List<List<PointF>>();
        private List<List<PointF>> tempPath;
        private int waveNumber = 0;
        private float chosenDif;
        private string waveName = "";
        private string waveNext = "";

        //Timer for spawning enemies
        public Timer spawner;
        private int TimerEventCounter = 0;


        #endregion
        // Fields for building phase

        public int gold = 100;
        bool afford = true;
        Timer tAfford;
        public float life;
        private bool slowTowerOwned = false;
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
        /// Frederik - Instantiating coordinate system & building world layout
        /// Tobias - tl and mouseRect
        /// Lucas - Buildwatch & Life
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
                    if (i % 3 == 0)
                        waveEnemy[i].Add(new EnemyNormal("EnemyNormal", 10 * chosenDif + (i * 5), 10, 0, 3, new Effect(@"Graphic/GrottoTop.png", new PointF(0, 0)), @"Graphic/heavyUp.png,Graphic/heavyLeft.png,Graphic/heavyDown.png,Graphic/heavyRight.png", new PointF(grottoX * tileSizeX, grottoY * tileSizeY), firstPoint));
                    if (i % 3 == 1)
                        waveEnemy[i].Add(new EnemyEvade("EnemyEvade", false, 10 * chosenDif + (i * 5), 10, 0, 4, new Effect(@"Graphic/GrottoTop.png", new PointF(0, 0)), @"Graphic/EvadeTop.png,Graphic/EvadeLeft.png,Graphic/EvadeBot.png,Graphic/EvadeRight.png,Graphic/EvadeTopEvading.png,Graphic/EvadeLeftEvading.png,Graphic/EvadeBotEvading.png,Graphic/EvadeRightEvading.png", new PointF(grottoX * tileSizeX, grottoY * tileSizeY), firstPoint));
                    if (i % 3 == 2)
                        waveEnemy[i].Add(new EnemySlow("EnemySlow", (float)0.50, tileSizeX * 3, 10 * chosenDif + (i * 5), 10, 0, 5, new Effect(@"Graphic/GrottoTop.png", new PointF(0, 0)), @"Graphic/slowUp.png,Graphic/slowLeft.png,Graphic/slowDown.png,Graphic/slowRight.png", new PointF(grottoX * tileSizeX, grottoY * tileSizeY), firstPoint));
                }
            }
            #endregion


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
            engine.Play2D("Blackmoor_Tides_Loop.wav", true);
           
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
        /// Tobias - Build towers
        /// Mikkel & Lucas - Sell towers
        /// All - Update lists
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
                                if (tower is TowerSlow)
                                    slowTowerOwned = false;

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
               bullets[i].Update(currentFPS);
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

            //Update wavename
            if(waveEnemy[waveNumber][0] is EnemyNormal)
                waveName = "Normal";
            if (waveEnemy[waveNumber][0] is EnemyEvade)
                waveName = "Evade";
            if (waveEnemy[waveNumber][0] is EnemySlow)
                waveName = "Slow";

            //Update nextWave
            if (waveNumber + 1 < waveEnemy.Count)
            {
                if (waveEnemy[waveNumber + 1][0] is EnemyNormal)
                    waveNext = "Normal";
                if (waveEnemy[waveNumber + 1][0] is EnemyEvade)
                    waveNext = "Evade";
                if (waveEnemy[waveNumber + 1][0] is EnemySlow)
                    waveNext = "Slow";
            }
            else
                waveNext = "Victory";

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
        /// Tobias - TowerButtons, GUI, Win/Loss GUI
        /// Lucas - Gold, Life, GameState
        /// Frederik - Current Wave, Next Wave
        /// Draws The World
        /// </summary>
        public void Draw()
        {
            dc.Clear(Color.White);
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
                            tl.Add(new TowerButton(new Size(75, 75), new Point(tmpX + 50, tmpY), "Land_Tower1", "Towers/LightCannons.png", 1));
                            tl.Add(new TowerButton(new Size(75, 75), new Point(tmpX - 120, tmpY), "Land_Tower2", "Towers/CannonDown.png", 2));
                            //tl.Add(new TowerButton(new Size(75, 75), new Point(tmpX - 30, tmpY - 120), "Land_Tower3", "Towers/w3.png", 3));
                        }
                    }
                    if (Form1.drawBuildGUI == 3)
                    {
                        if (tl.Count < 3)
                        {
                            tl.Add(new TowerButton(new Size(75, 75), new Point(tmpX + 50, tmpY), "Water_Tower1", "Towers/shipTower.png", 1));
                            tl.Add(new TowerButton(new Size(75, 75), new Point(tmpX - 120, tmpY), "Water_Tower2", "Towers/mermaid.png", 2));
                            tl.Add(new TowerButton(new Size(75, 75), new Point(tmpX - 30, tmpY - 120), "Water_Tower3", "Towers/Whirlpool.png", 3));
                        }
                    }
                    if (Form1.drawBuildGUI == 2)
                    {
                        if (tl.Count < 1)
                        {
                            tl.Add(new TowerButton(new Size(75, 75), new Point(tmpX - 30, tmpY - 120), "Sell_Tower", "Graphic/Sell.png", 3));
                        }
                    }
                }

            }

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

            // Drawing Phase
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

            //Draw Wave Number
            dc.DrawString(string.Format("Wave: {0} / {1}", waveNumber+1, waveEnemy.Count), w, q, 1152, 5);
            dc.DrawString(string.Format("Wave Type: {0}", waveName), w, q, 1152, 25);
            dc.DrawString(string.Format("Next wave: {0}", waveNext), w, q, 1152, 45);

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
            if (waveNumber == waveEnemy.Count)
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
        /// Frederik
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
        /// Frederik
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
        /// Frederik
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
        /// Frederik
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
        /// Frederik
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
        /// Frederik
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
        /// Frederik
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
        /// Frederik
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

        /// <summary>
        /// Frederik
        /// Checks nearby nodes to see if position is valid for pathing
        /// </summary>
        /// <param name="coordinateSystem"></param>
        /// <param name="mainNode"></param>
        /// <param name="openNodes"></param>
        /// <param name="closedNodes"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        public void CheckNearbyNodes(ref int[][] coordinateSystem, ref Node mainNode, ref List<Node> openNodes, ref List<Node> closedNodes, int startX, int startY, int endX, int endY)
        {
            //Node below main
            if (mainNode.LocationY < worldSizeY - 1)
            {
                if (coordinateSystem[mainNode.LocationX][mainNode.LocationY + 1] > 1 && coordinateSystem[mainNode.LocationX][mainNode.LocationY + 1] < 11 && coordinateSystem[mainNode.LocationX][mainNode.LocationY + 1] != 5)
                {
                    if (!CheckForDuplicates(ref openNodes, mainNode, 0, 1))
                        openNodes.Add(new Node(mainNode.LocationX, mainNode.LocationY + 1, endX, endY, mainNode.G));
                }
                if (coordinateSystem[mainNode.LocationX][mainNode.LocationY + 1] == 5 && endX == treasureX && endY == treasureY)
                    if (!CheckForDuplicates(ref openNodes, mainNode, 0, 1))
                    openNodes.Add(new Node(mainNode.LocationX, mainNode.LocationY + 1, endX, endY, mainNode.G));
            }

            //Node above main
            if (mainNode.LocationY > 0)
            {
                if (coordinateSystem[mainNode.LocationX][mainNode.LocationY - 1] > 1 && coordinateSystem[mainNode.LocationX][mainNode.LocationY - 1] < 11 && coordinateSystem[mainNode.LocationX][mainNode.LocationY - 1] != 5)
                {
                    if (!CheckForDuplicates(ref openNodes, mainNode, 0, -1))
                        openNodes.Add(new Node(mainNode.LocationX, mainNode.LocationY - 1, endX, endY, mainNode.G));
                }
                if (coordinateSystem[mainNode.LocationX][mainNode.LocationY - 1] == 5 && endX == treasureX && endY == treasureY)
                    if (!CheckForDuplicates(ref openNodes, mainNode, 0, -1))
                        openNodes.Add(new Node(mainNode.LocationX, mainNode.LocationY - 1, endX, endY, mainNode.G));
            }

            //Node left of main
            if (mainNode.LocationX > 0)
            {
                if (coordinateSystem[mainNode.LocationX - 1][mainNode.LocationY] > 1 && coordinateSystem[mainNode.LocationX - 1][mainNode.LocationY] < 11 && coordinateSystem[mainNode.LocationX - 1][mainNode.LocationY] != 5)
                {
                    if (!CheckForDuplicates(ref openNodes, mainNode, -1, 0))
                        openNodes.Add(new Node(mainNode.LocationX - 1, mainNode.LocationY, endX, endY, mainNode.G));
                }
                if (coordinateSystem[mainNode.LocationX - 1][mainNode.LocationY] == 5 && endX == treasureX && endY == treasureY)
                    if (!CheckForDuplicates(ref openNodes, mainNode, -1, 0))
                        openNodes.Add(new Node(mainNode.LocationX - 1, mainNode.LocationY, endX, endY, mainNode.G));
            }

            //Node right of main
            if (mainNode.LocationX < worldSizeX - 1)
            {
                if (coordinateSystem[mainNode.LocationX + 1][mainNode.LocationY] > 1 && coordinateSystem[mainNode.LocationX + 1][mainNode.LocationY] < 11 && coordinateSystem[mainNode.LocationX + 1][mainNode.LocationY] != 5)
                {
                    if (!CheckForDuplicates(ref openNodes, mainNode, 1, 0))
                        openNodes.Add(new Node(mainNode.LocationX + 1, mainNode.LocationY, endX, endY, mainNode.G));
                }
                if (coordinateSystem[mainNode.LocationX + 1][mainNode.LocationY] == 5 && endX == treasureX && endY == treasureY)
                    if (!CheckForDuplicates(ref openNodes, mainNode, 1, 0))
                        openNodes.Add(new Node(mainNode.LocationX + 1, mainNode.LocationY, endX, endY, mainNode.G));
            }
        }

        #endregion
        /// <summary>
        /// Frederik
        /// Assigning values to positions in the coordinate system, defining which environment object will be placed on that coordinate.
        /// Then it places that environment object on the tile and adds the object to gameworld's list of environment objects
        /// </summary>
        /// <param name="coordinateSystem"></param>
        /// <param name="environmentList"></param>
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
                        if (temp >= 15 && temp <= 100)
                            coordinateSystem[x][y] = 10;
                        //Island
                        if (temp >= 8 && temp < 15)
                            coordinateSystem[x][y] = 11;
                        //Lighthouse
                        if (temp >= 5 && temp < 8)
                            coordinateSystem[x][y] = 12;
                        //Rock
                        if (temp >= 0 && temp < 5)
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
                        if (y == 0)
                            environmentList.Add(new Grotto(@"Graphic/GrottoTop.png", new PointF(tempX, tempY)));
                        if (y == (worldSizeY*tileSizeY) - tileSizeY)
                            environmentList.Add(new Grotto(@"Graphic/GrottoBot.png", new PointF(tempX, tempY)));
                        if (x == (worldSizeX*tileSizeX) - tileSizeX)
                            environmentList.Add(new Grotto(@"Graphic/GrottoRight.png", new PointF(tempX, tempY)));
                        if (x == 0)
                            environmentList.Add(new Grotto(@"Graphic/GrottoLeft.png", new PointF(tempX, tempY)));
                    }

                    //Checkpoints
                    if (coordinateSystem[x][y] == 2)
                    {
                        environmentList.Add(new Checkpoint(@"Graphic/Checkpoint1.png", new PointF(tempX, tempY)));
                    }
                    if (coordinateSystem[x][y] == 3)
                    {
                        environmentList.Add(new Checkpoint(@"Graphic/Checkpoint2.png", new PointF(tempX, tempY)));
                    }
                    if (coordinateSystem[x][y] == 4)
                    {
                        environmentList.Add(new Checkpoint(@"Graphic/Checkpoint3.png", new PointF(tempX, tempY)));
                    }

                    //Placing treasure chest
                    if (coordinateSystem[x][y] == 5)
                    {
                        environmentList.Add(new Treasure(@"Graphic/TreasureChest.png", new PointF(tempX, tempY)));
                    }
                    //Placing Water
                    if (coordinateSystem[x][y] == 10)
                    {
                        environmentList.Add(new Water(@"Graphic/water.png", new PointF(tempX, tempY)));
                    }
                    //Placing Islands
                    if (coordinateSystem[x][y] == 11)
                    {
                        environmentList.Add(new Island(@"Graphic/land.png", new PointF(tempX, tempY)));
                    }
                    //Placing lighthouses
                    if (coordinateSystem[x][y] == 12)
                    {
                        environmentList.Add(new Lighthouse(@"Graphic/lighthouse.png", new PointF(tempX, tempY)));
                    }
                    //Placing rocks
                    if (coordinateSystem[x][y] == 13)
                    {
                        environmentList.Add(new Rock(@"Graphic/rocks.png", new PointF(tempX, tempY)));
                    }

                    //Placing Walls
                    //Left
                    if (coordinateSystem[x][y] == 101)
                    {
                        environmentList.Add(new Rock(@"Graphic/WallLeft.png", new PointF(tempX, tempY)));
                    }
                    //Right
                    if (coordinateSystem[x][y] == 102)
                    {
                        environmentList.Add(new Rock(@"Graphic/WallRight.png", new PointF(tempX, tempY)));
                    }
                    //Top
                    if (coordinateSystem[x][y] == 103)
                    {
                        environmentList.Add(new Rock(@"Graphic/WallTop.png", new PointF(tempX, tempY)));
                    }
                    //Bot
                    if (coordinateSystem[x][y] == 104)
                    {
                        environmentList.Add(new Rock(@"Graphic/WallBot.png", new PointF(tempX, tempY)));
                    }

                    //Placing Corners
                    //Top Left
                    if (coordinateSystem[x][y] == 105)
                    {
                        environmentList.Add(new Rock(@"Graphic/CornerTopLeft.png", new PointF(tempX, tempY)));
                    }
                    //Top Right
                    if (coordinateSystem[x][y] == 106)
                    {
                        environmentList.Add(new Rock(@"Graphic/CornerTopRight.png", new PointF(tempX, tempY)));
                    }
                    //Bot Left
                    if (coordinateSystem[x][y] == 107)
                    {
                        environmentList.Add(new Rock(@"Graphic/CornerBotLeft.png", new PointF(tempX, tempY)));
                    }
                    //Bot Right
                    if (coordinateSystem[x][y] == 108)
                    {
                        environmentList.Add(new Rock(@"Graphic/CornerBotRight.png", new PointF(tempX, tempY)));
                    }
                }
            }
        }

        /// <summary>
        /// Frederik
        /// Starts the current wave
        /// </summary>
        public void StartWave()
        {
            currentWave = waveEnemy[waveNumber];
            spawner = new Timer(2000);
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
                        waveNumber++;
                    }
                    break;


            }
        }
        /// <summary>
        /// Lucas & Mikkel - building towers
        /// Frederik - Reworking enemy path when placing tower
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
            if (towerNumb == 3 && slowTowerOwned)
                allowTower = false;

            if (allowTower)
                switch (towerNumb)
                {
                    #region Water
                    // Water
                    case 1:
                        if (gold >= 25)
                        {
                            towers.Add(new TowerNormal(15, 4000, 25, tileSizeX * 3, @"Towers/shipTower.png", position));
                            gold -= towers[0].Cost;
                        }
                        else
                        {
                            CantAfford();
                        }
                        break;

                    case 2:
                        if (gold >= 60)
                        {
                            towers.Add(new TowerBoost((float)0.3, 15, 1000, 60, tileSizeX * 3, @"Towers/mermaid.png", position));
                            gold -= towers[0].Cost;
                        }
                        else
                        {
                            CantAfford();
                        }
                        break;

                    case 3:
                        if (gold >= 80)
                        {
                            towers.Add(new TowerSlow((float)0.3, tileSizeX * 3, 1000, 80, tileSizeX * 3, @"Towers/Whirlpool.png", position));
                            gold -= towers[0].Cost;
                            slowTowerOwned = true;
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
                        if (gold >= 30)
                        {
                            towers.Add(new TowerNormal(10, 2000, 30, tileSizeX * 3, @"Towers/LightCannons.png", position));
                            gold -= towers[0].Cost;
                        }
                        else
                        {
                            CantAfford();
                        }

                        break;
                    case 5:
                        if (gold >= 50)
                        {
                            towers.Add(new TowerNormal(30, 5000, 50, tileSizeX * 3, @"Towers/CannonDown.png", position));
                            gold -= towers[0].Cost;
                        }
                        else
                        {
                            CantAfford();
                        }
                        break;
                    //Not used yet
                    //case 6:
                    //    if (gold >= 30)
                    //    {
                    //        towers.Add(new TowerStun(2, 5, 40, 7, @"Towers/L3.png", position, true));
                    //        gold -= towers[0].Cost;
                    //    }
                    //    else
                    //    {
                    //        CantAfford();
                    //    }
                    //    break;
                    #endregion
                }
        }
        /// <summary>
        /// Lucas & Mikkel - selling tower
        /// Frederik - Reworking enemy path if tower is sold
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
                    coordinateSystem[(int)t.Position.X / tileSizeX][(int)t.Position.Y / tileSizeY] = 10;
                    
                    //Perform path generation again

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

                    //Building enemy paths, depending on amount of checkpoints
                    for (int i = 0; i < endPoints.Count; i++)
                    {
                        RoadBuilder(ref coordinateSystem, (int)startPoints[i].X, (int)startPoints[i].Y, (int)endPoints[i].X, (int)endPoints[i].Y, i);
                    }

                    //Remove tower from list
                    towers.Remove(t);
                }
            }


        }
        /// <summary>
        /// Frederik
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
        /// Frederik
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

        
        /// <summary>
        /// Frederik
        /// Duplicates a 2D jagged array to prevent cross references
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Tobias
        /// Informs player that he cannot afford the tower he chose
        /// </summary>
        private void CantAfford()
        {
            tAfford = new Timer(3000);
            tAfford.Enabled = true;
            afford = false;
            tAfford.Elapsed += new ElapsedEventHandler(CantAffordTrue);

        }

        /// <summary>
        /// Tobias
        /// Stops the CantAfford() message from being shown
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void CantAffordTrue(object source, ElapsedEventArgs e)
        {
            afford = true;
            tAfford.Enabled = false;
        }
    }


}