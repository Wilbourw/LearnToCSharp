using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

/*
 The MIT License (MIT)

Copyright (c) <2014> <Ville Viitala>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

namespace LearnToCSharp
{
    /// <summary>
    /// Robot shall move to the direction
    /// </summary>
    public enum Direction
    {
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4,
        TurnLeft = 5,
        TurnRight = 6
    }

    /// <summary>
    /// Line style to draw
    /// </summary>
    public enum LineStyle
    { 
        Solid = 1,
        Dashed = 2,
        Dotted = 3
    }

    /// <summary>
    /// Line color
    /// </summary>
    public enum LineColor
    { 
        Black = 1,
        Red = 2,
        Blue = 3,
        Green = 4,
        Yellow = 5,
        Orange = 6,
        Gray = 7
    }

    class Robot
    {
        /// <summary>
        /// Constant value for one movement step of the robot.
        /// </summary>
        private const double MOVE_MEASURE = 50;

        /// <summary>
        /// Constant value for half step of the robot. 
        /// </summary>
        private const double MOVE_MEASURE_HALF = 25;
        
        /// <summary>
        /// Variable for getting robot's movement direction .
        /// </summary>
        private String moveDirection;

        /// <summary>
        /// Variables to get robot's coordinates on the canvas.
        /// </summary>
        private double robotCoordinateX;
        private double robotCoordinateY;

        /// <summary>
        /// Variable for altering does the robot draw or not.
        /// </summary>
        private bool draw = true;

        /// <summary>
        /// Value for changing robot's drawing line's thickness.
        /// </summary>
        private double drawThickness = 2;

        /// <summary>
        /// Variable for choosing robot's drawing color.
        /// </summary>
        private String lineColor = "black";

        /// <summary>
        /// Variable for choosing robot's drawing line's type.
        /// </summary>
        private int lineType = 0;

        /// <summary>
        /// Objects for canvas and creating robot's image.
        /// </summary>
        private Canvas canvas;
        private Image imageRobot = new Image();
        private BitmapImage bitmapImageRobot = new BitmapImage();

        /// <summary>
        /// Create the robot and set it on the canvas.
        /// </summary>
        /// <param name="canvasArea">Name of the canvas object.</param>
        /// <param name="robotImage">Name of the robot object</param>
        /// <param name="left">Left coordinate of the robot, 1 - 750, 0 = center</param>
        /// <param name="top">Top coordinate of the robot, 1 - 700, 0 = center</param>
        public Robot(Canvas canvasArea, Image robotImage, double left, double top)
        {
            canvas = canvasArea;

            //Getting the image file of the robot and setting it for the canvas.
            imageRobot = robotImage;
            var uri = new Uri("pack://application:,,,/img/Robot.png");
            var bitmap = new BitmapImage(uri);
            imageRobot.Source = bitmap;
                        
            imageRobot.SetValue(Panel.ZIndexProperty, 1);

            if (left > 0 && left <= 750)
            {
                imageRobot.SetValue(Canvas.LeftProperty, left);
            }
            else
            {
                imageRobot.SetValue(Canvas.LeftProperty, 350d);
            }

            if (top > 0 && top <= 700)
            {
                imageRobot.SetValue(Canvas.TopProperty, top);
            }
            else
            {
                imageRobot.SetValue(Canvas.TopProperty, 350d);
            }

            robotCoordinateX = Canvas.GetLeft(imageRobot);
            robotCoordinateY = Canvas.GetTop(imageRobot);
        }

        /// <summary>
        /// Set robot to draw a line.
        /// </summary>
        public void SetDrawingOn()
        {
            draw = true;
        }

        /// <summary>
        /// Set robot not to draw a line.
        /// </summary>
        public void SetDrawingOff()
        {
            draw = false;
        }

        /// <summary>
        /// Set drawing color for the robot with UI.
        /// </summary>
        /// <param name="r">Parameter for RadioButton object to catch the lineColor for drawing.</param>
        public void SetDrawingColor(RadioButton r)
        {
            lineColor = r.Background.ToString(); //color as hexcode       
        }

        /// <summary>
        /// Set drawing color for the robot with code.
        /// </summary>
        /// <param name="color">Parameter for LineColor object to save the color as hexcode.</param>
        public void SetDrawingColor(LineColor color)
        {
            switch (color)
                {
                case LineColor.Black:
                    lineColor = "#FF000000";
                    break;
                case LineColor.Red:
                    lineColor = "#FFFF0000";
                    break;
                case LineColor.Blue:
                    lineColor = "#FFFF0000";
                    break;
                case LineColor.Green:
                    lineColor = "#FF008000";
                    break;
                case LineColor.Yellow:
                    lineColor = "#FFFFFF00";
                    break;
                case LineColor.Orange:
                    lineColor = "#FFFFA500";
                    break;
                default: // Gray
                    lineColor = "#FF808080";
                    break;
            }                
        }
        
        /// <summary>
        /// Set thickness of the robot's line with UI.
        /// </summary>
        /// <param name="slider">Parameter for Slider object to alter drawing thickness in the UI.</param>
        /// <param name="textBoxSliderThicknessValue">Parameter for TextBox object that shows the value of drawing thickness.</param>
        public void SetDrawingThickness(Slider slider, TextBox textBoxSliderThicknessValue)
        {
            drawThickness = slider.Value;

            if (textBoxSliderThicknessValue != null)
            {
                textBoxSliderThicknessValue.Text = drawThickness.ToString();
            }
        }

        /// <summary>
        /// Set thickness of the robot's line with code.
        /// </summary>
        /// <param name="thicknessValue">Variables that holds the drawing thickness value.</param>
        public void SetDrawingThickness(double thicknessValue)
        {
            drawThickness = thicknessValue;
        }

        /// <summary>
        /// Set the Robot's movement on the canvas with UI.
        /// </summary>
        /// <param name="comboBoxMoveDirection">Parameter for comboBox object that lets user to choose Robots movement direcion in the UI.</param>
        public void SetRobotDirection(ComboBox comboBoxMoveDirection)
        {
            //Robot movement direction
            moveDirection = comboBoxMoveDirection.Text;            
        }

        /// <summary>
        /// Set the Robot's movement on the canvas with code.
        /// </summary>
        /// <param name="direction">Parameter for Direction object that enables choosing the robot's movement direction.</param>
        public void SetRobotDirection(Direction direction)
        {
            if (direction == Direction.Up)
            {
                moveDirection = "Up";
            }
            else if (direction == Direction.Down)
            {
                moveDirection = "Down";
            }
            else if (direction == Direction.Left)
            {
                moveDirection = "Left";
            }
            else if (direction == Direction.Right)
            {
                moveDirection = "Right";
            }
            else if (direction == Direction.TurnLeft) // turn left
            {
                if (moveDirection == "Up")
                {
                    moveDirection = "Left";
                }
                else if (moveDirection == "Left")
                {
                    moveDirection = "Down";
                }
                else if (moveDirection == "Down")
                {
                    moveDirection = "Right";
                }
                else
                {
                    moveDirection = "Up";
                }
            }
            else // turn right
            {               
                if (moveDirection == "Up")
                {
                    moveDirection = "Right";
                }
                else if (moveDirection == "Left")
                {
                    moveDirection = "Up";
                }
                else if (moveDirection == "Down")
                {
                    moveDirection = "Left";
                }
                else
                {
                    moveDirection = "Down";
                }

            }
        }//SetRobotDirection()

        /// <summary>
        /// Method for choosing the line style for drawing.
        /// </summary>
        /// <param name="comboBoxLineType">Parameter for comboBox object that lets user to choose the drawing line type with UI.</param>
        public void SetLineStyle(ComboBox comboBoxLineType)
        {
            if (comboBoxLineType.Text == "Solid")
            {
                lineType = (int)LineStyle.Solid;
            }
            else if (comboBoxLineType.Text == "Dashed")
            {
                lineType = (int)LineStyle.Dashed;
            }
            else
            {
                lineType = (int)LineStyle.Dotted;
            }
        }
        
        /// <summary>
        /// Method for user to manually change lineStyle with code.
        /// </summary>
        /// <param name="lineStyle">Parameter for LineStyle object that lets user to code drawing line style.</param>
        public void SetLineStyle(LineStyle lineStyle)
        {
            lineType = (int)lineStyle;
        }        

        /// <summary>
        /// Method that moves the robot and checks that the robot doesn't cross canvas borders.
        /// The method is coded as an async method. This enables the waiting that robot does between the steps it takes.
        /// The waiting is done by command "await Task.Delay(1000);" that pauses the method for one second.
        /// </summary>
        /// <returns></returns>
        public async Task<Boolean> MoveTheRobot()
        {

            if (moveDirection == "Left" || moveDirection == "Right")
            {
                robotCoordinateX = Canvas.GetLeft(imageRobot);

                if (moveDirection == "Left")
                {
                    if (robotCoordinateX < 50)
                    {
                        return false;  // reached border
                    }
                    else
                    {
                        Canvas.SetLeft(imageRobot, robotCoordinateX - MOVE_MEASURE);
                        if (draw == true)
                        {
                            DrawRobotsLine();
                        }
                    }
                }
                else if (moveDirection == "Right")
                {
                    if (robotCoordinateX > 700)
                    {
                        return false;
                    }
                    else
                    {
                        Canvas.SetLeft(imageRobot, robotCoordinateX + MOVE_MEASURE);
                        if (draw == true)
                        {
                            DrawRobotsLine();
                        }
                    }
                }
            }
            else
            {
                robotCoordinateY = Canvas.GetTop(imageRobot);

                if (moveDirection == "Up")
                {
                    if (robotCoordinateY < 50)
                    {
                        return false;
                    }
                    else
                    {
                        Canvas.SetTop(imageRobot, robotCoordinateY - MOVE_MEASURE);
                        if (draw == true)
                        {
                            DrawRobotsLine();
                        }
                    }
                }
                else if (moveDirection == "Down")
                {
                    if (robotCoordinateY > 650)
                    {
                        return false;
                    }
                    else
                    {
                        Canvas.SetTop(imageRobot, robotCoordinateY + MOVE_MEASURE);
                        if (draw == true)
                        {
                            DrawRobotsLine();
                        }
                    }
                }
            }
            await Task.Delay(1000);
            return true;

        } // MoveTheRobot

        /// <summary>
        /// Method that draws Robot's movement line if drawing is enabled.
        /// Also creates the line types according the user's choice.
        /// </summary>
        public void DrawRobotsLine()
        {
            Line robotDrawLine = new Line();
            SolidColorBrush colorToUse = (SolidColorBrush)new BrushConverter().ConvertFrom(lineColor);
            robotDrawLine.Stroke = colorToUse;
            robotDrawLine.StrokeThickness = drawThickness;
            DoubleCollection dashes = new DoubleCollection();

            //Line type
            if (lineType == (int) LineStyle.Solid)  
            {
                dashes.Add(0);
                dashes.Add(0);
                robotDrawLine.StrokeDashArray = dashes;
                robotDrawLine.StrokeDashCap = PenLineCap.Square;
            }
            else if (lineType == (int) LineStyle.Dashed) 
            {
                dashes.Add(2);
                dashes.Add(4);
                robotDrawLine.StrokeDashArray = dashes;
                robotDrawLine.StrokeDashCap = PenLineCap.Round;
                robotDrawLine.StrokeStartLineCap = PenLineCap.Round;
            }
            else if (lineType == (int) LineStyle.Dotted)  
            {
                dashes.Add(1);
                dashes.Add(0.5);
                robotDrawLine.StrokeDashArray = dashes;
                robotDrawLine.StrokeDashCap = PenLineCap.Flat;
            }

            //Movement
            if (moveDirection == "Left")
            {
                robotDrawLine.X1 = Canvas.GetLeft(imageRobot) + MOVE_MEASURE_HALF;
                robotDrawLine.Y1 = Canvas.GetTop(imageRobot) + MOVE_MEASURE_HALF;
                robotDrawLine.X2 = Canvas.GetLeft(imageRobot) + MOVE_MEASURE + MOVE_MEASURE_HALF;
                robotDrawLine.Y2 = Canvas.GetTop(imageRobot) + MOVE_MEASURE_HALF;
            }
            else if (moveDirection == "Right")
            {
                robotDrawLine.X1 = Canvas.GetLeft(imageRobot) + MOVE_MEASURE_HALF;
                robotDrawLine.Y1 = Canvas.GetTop(imageRobot) + MOVE_MEASURE_HALF;
                robotDrawLine.X2 = Canvas.GetLeft(imageRobot) - MOVE_MEASURE_HALF;
                robotDrawLine.Y2 = Canvas.GetTop(imageRobot) + MOVE_MEASURE_HALF;
            }
            else if (moveDirection == "Up")
            {
                robotDrawLine.X1 = Canvas.GetLeft(imageRobot) + MOVE_MEASURE_HALF;
                robotDrawLine.Y1 = Canvas.GetTop(imageRobot) + MOVE_MEASURE_HALF;
                robotDrawLine.X2 = Canvas.GetLeft(imageRobot) + MOVE_MEASURE_HALF;
                robotDrawLine.Y2 = Canvas.GetTop(imageRobot) + MOVE_MEASURE + MOVE_MEASURE_HALF;
            }
            else if (moveDirection == "Down")
            {
                robotDrawLine.X1 = Canvas.GetLeft(imageRobot) + MOVE_MEASURE_HALF;
                robotDrawLine.Y1 = Canvas.GetTop(imageRobot) + MOVE_MEASURE_HALF;
                robotDrawLine.X2 = Canvas.GetLeft(imageRobot) + MOVE_MEASURE_HALF;
                robotDrawLine.Y2 = Canvas.GetTop(imageRobot) - MOVE_MEASURE_HALF;
            }

            //Adds drawing to the canvas
            canvas.Children.Add(robotDrawLine);
        } //DrawRobotsLine()
    }
}
