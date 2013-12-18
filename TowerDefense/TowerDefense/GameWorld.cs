using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace TowerDefense
{
    class GameWorld
    {
        // Fields

        private Random rnd = new Random();
        private DateTime lastFrameStartIt = new DateTime();
        private float currentFPS;
        private Graphics dc;
        private BufferedGraphics buffer;
        private Stopwatch stopWatch = new Stopwatch();

        // Fields for world creation

        private float worldSizeX;
        private float worldSizeY;
        private int[][] coordinateSystem;
        private int grottoX;
        private int grottoY;
        private int treasureX;
        private int treasureY;
        private int minDistStart;
        private List<Environment> environment = new List<Environment>();
        private List<Tower> towers = new List<Tower>();

        private bool validLocation;

        // Fields for wave

        private List<int> waveCount = new List<int>();
        private List<Enemy> currentWave = new List<Enemy>();
        private int waveNumber;
        private int checkPoint;
        private float chosenDif;

        // Fields for building phase

        private int gold;
        private int chest;


        // Constructors

        public GameWorld(Graphics dc, Rectangle displayRectangle, float worldSizeX, float worldSizeY, float dif)
        {
            this.dc = buffer.Graphics;
            this.worldSizeX = worldSizeX;
            this.worldSizeY = worldSizeY;
            this.chosenDif = dif;
        }

        // Functions

        public void SetupWorld()
        {

        }

        public void GameLoop()
        {

        }

        public void Update()
        {

        }

        public void Draw()
        {

        }

        public bool CheckLocation(int startX, int startY, int endX, int endY)
        {
            
        }

        public bool CheckLocation(int startX, int startY, int endX, int endY, int minDistStart)
        {

        }

        public void RoadBuilder(float worldSizeX, float worldSizeY, int startX, int startY, int endX, int endY, int[][] coordinateSystem)
        {

        }

        public void GameState()
        {

        }

        public void Build()
        {

        }

        public void Sell()
        {

        }

        public void OnCheckpoint()
        {

        }
    }
}
