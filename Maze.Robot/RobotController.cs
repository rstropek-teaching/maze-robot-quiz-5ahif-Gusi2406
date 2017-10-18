using Maze.Library;
using System;
using System.Collections.Generic;

namespace Maze.Solver
{
    /// <summary>
    /// Moves a robot from its current position towards the exit of the maze
    /// </summary>
    public class RobotController
    {
        private IRobot robot;
        private List<(int x, int y)> visitedPositions = new List<(int x, int y)>();
        bool reachedEnd = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotController"/> class
        /// </summary>
        /// <param name="robot">Robot that is controlled</param>
        public RobotController(IRobot robot)
        {
            // Store robot for later use
            this.robot = robot;
        }

        /// <summary>
        /// Moves the robot to the exit
        /// </summary>
        /// <remarks>
        /// This function uses methods of the robot that was passed into this class'
        /// constructor. It has to move the robot until the robot's event
        /// <see cref="IRobot.ReachedExit"/> is fired. If the algorithm finds out that
        /// the exit is not reachable, it has to call <see cref="IRobot.HaltAndCatchFire"/>
        /// and exit.
        /// Die Funktion verwendet die Methoden des Roboters. Roboter bekommt man oben im Konstrukor.
        /// Damit man den Roboter bewegen kann, verwendet man die Methodes des Roboters
        /// </remarks>
        public void MoveRobotToExit()
        {
            // Here you have to add your code

            // Trivial sample algorithm that can just move right

            robot.ReachedExit += Robot_ReachedExit;  //Listenerzuweis, += wird in C# für Listenerzuweise verwendet

            TryToMoveInAllDirections();  //Methode aufrufen, die versucht in alle Richtungen nach der Reihe zu gehen

            if (!reachedEnd)
            {
                robot.HaltAndCatchFire();
            }
        }

        private void Robot_ReachedExit(object sender, EventArgs e)
        {
            this.reachedEnd = true;  //falls das Ende erreicht wurde
        }

        private void TryToMoveInAllDirections(int x=0, int y=0)  //x=0, x ist ohne übergabe 0
        {
            //4 mal wird der 1. Schritt versucht
            MoveRobotIntoOneDirection(Direction.Left, x, y - 1);
            MoveRobotIntoOneDirection(Direction.Right, x, y + 1);
            MoveRobotIntoOneDirection(Direction.Up, x - 1, y);
            MoveRobotIntoOneDirection(Direction.Down, x + 1, y);
        }

        private void MoveRobotIntoOneDirection(Direction direction, int x, int y)
        {
            if (!reachedEnd && !visitedPositions.Contains((x, y)))
            {
                //wenn das Ende noch nicht erreicht wurde und die Position noch nicht besucht wurde,
                //wird versucht in die übergebene Richtung zu gehen
                if (robot.TryMove(direction))
                {
                    visitedPositions.Add((x, y));  //Es wird gespeichert in welche Richtung gegangen wurde
                    TryToMoveInAllDirections(x, y);
                    if (!reachedEnd)
                    {
                        robot.Move(InvertDirection(direction));  //Wieder dort zurückgehen, wo man zuvor war
                    }
                }
            }
        }

        private Direction InvertDirection(Direction direction)  //Kehrt die übergebene Richtung um
        {
            switch (direction)
            {
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                default:
                    throw new Exception("Unknown Direction");
            }
        }
    }
}