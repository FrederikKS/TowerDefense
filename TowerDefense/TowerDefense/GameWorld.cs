﻿using System;
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

        /// <summary>
        /// Draws The World
        /// </summary>
        public void SetupWorld()
        {

        }
        /// <summary>
        /// Makes sure all the update functions is called every frames
        /// </summary>
        public void GameLoop()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {

        }
        /// <summary>
        /// Draws The World
        /// </summary>
        public void Draw()
        {

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
        /// Checks What Wave The Player Is On
        /// </summary>
        public void GameState()
        {

        }
        /// <summary>
        /// The Build Function
        /// </summary>
        public void Build()
        {

        }
        /// <summary>
        /// The Sell Function
        /// </summary>
        public void Sell()
        {

        }
        /// <summary>
        /// Checking If The Enemies Are On A Checkpoing
        /// </summary>
        public void OnCheckpoint()
        {

        }
    }
}
