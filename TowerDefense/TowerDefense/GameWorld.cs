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

    public class GameWorld
    {
        TowerButton tb;
        public RectangleF mouseRect;
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
        private int tileSizeX = 64;
        private int tileSizeY = 64;
        private int[][] coordinateSystem;
        private int grottoX;
        private int grottoY;
        private int treasureX;
        private int treasureY;
        private int minDistStart = 3;
        public List<Environment> environment = new List<Environment>();
        public List<Tower> towers = new List<Tower>();
        private List<PointF> checkpointList = new List<PointF>();

        private bool validLocation;

        // Fields for wave
        List<List<Enemy>> waveEnemy = new List<List<Enemy>>();
        private List<int> waveCount = new List<int>();
        private List<Enemy> currentWave = new List<Enemy>();
        private int waveNumber;
        private int listNumb;
        private int checkPoint;
        private float chosenDif;

        // Fields for building phase

        private int gold;
        private int cost;
        private int chest;


        // Constructors

        public GameWorld(Graphics dc, Rectangle displayRectangle, float worldSizeX, float worldSizeY, float dif)
        {
            /// Creates and allocates a buffer in memory with the size of the display
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
            //Mousedown
            //if (Form1.guiIsClicked)
            //{
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

            //Location for checkpoints
            if (numberOfCheckpoints > 0)
                for (int i = 0; i < numberOfCheckpoints; i++)
                {
                    checkpointList[i] = new PointF(rnd.Next(0, (int)worldSizeX), rnd.Next(0, (int)worldSizeY));
                }

            // Bool for checking validity of tile location
            validLocation = CheckLocation(grottoX, grottoY, treasureX, treasureY, minDistStart);

            //If location is not valid (start too close to finish), find a new location
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

            //Building road from A to B, then save that path
            //Building road from B to C, then save that path
            //Continue depending on amount of checkpoints



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

            //if (Form1.guiIsClicked)
            //{
                if (mouseRect.IntersectsWith(tb.CollisionRect))
                {
                    if (tb.Name == "Test")
                    {
                       // Build A Test Tower
                    }
               // }
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
            //if (Form1.guiIsClicked)
            //{
                tb.DrawMe(dc);
           // }

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
        public bool CheckLocation(int startX, int startY, int endX, int endY)
        {
            return true;
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
        public void RoadBuilder(float worldSizeX, float worldSizeY, int startX, int startY, int endX, int endY, int[][] coordinateSystem)
        {

        }

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
            int offsetX = tileSizeX / 2;
            int offsetY = tileSizeY / 2;

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
                        environmentList.Add(new Treasure(@"Graphic/TreasureChestResized.png", new PointF(tempX, tempY), false));
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
            switch(towerNumb)
            {
                case 1:
                        towers.Add(new TowerSlow(20, 20, 20, 20, 20, @"Sprites/LighthousePlaceHolder.jpg", position, true));
                        gold -= cost;
                    break;

                case 2:
                        towers.Add(new TowerBoost(20, 20, 20, 20, 20, 20, @"Sprites/LighthousePlaceHolder.jpg", position, true));
                        gold -= cost;                   
                    break;
                case 3:
                        towers.Add(new TowerStun(20, 20, 20, 20, 20, @"Sprites/LighthousePlaceHolder.jpg", position, true));
                        gold -= cost;
                    break;
                case 4:
                        towers.Add(new TowerBoost(20, 20, 20, 20, 20, 20, @"Sprites/LighthousePlaceHolder.jpg", position, true));
                        gold -= cost;
                    break;
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
