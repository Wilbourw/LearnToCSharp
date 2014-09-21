using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
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
    /// Interaction logic for MainWindow.xaml
    /// Methods for all the UI controllers
    /// </summary>
    public partial class MainWindow : Window
    {
        Robot robot;

        public MainWindow()
        {
            InitializeComponent();
            robot = new Robot(canvasArea, imageRobot, 0, 0);
        }

        /// <summary>
        /// Drawing enabled or unenabled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonDrawOn_Checked(object sender, RoutedEventArgs e)
        {
            if (robot != null)
            {
                robot.SetDrawingOn();
            }

        }
        private void radioButtonDrawOff_Checked(object sender, RoutedEventArgs e)
        {
            if (robot != null)
            {
                robot.SetDrawingOff();
            }
        }

        /// <summary>
        /// Drawing color selection
        /// </summary>
        /// <param name="sender">In this method the sender is RadioButton.</param>
        /// <param name="e"></param>
        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {            
            RadioButton r = (RadioButton)sender;
            if (robot != null)
            {
                robot.SetDrawingColor(r);
            }
        }

        /// <summary>
        /// Draw thickness-slider method
        /// </summary>
        /// <param name="sender">In this method the sender is Slider</param>
        /// <param name="e"></param>
        private void sliderThickness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            if (robot != null)
            {
                robot.SetDrawingThickness(slider, textBoxSliderThicknessValue);
            }
        }

        /// <summary>
        /// Method for using the UI and executing user's commands with the Execute-button.
        /// The method is coded as an async method. This enables the waiting that robot does between the steps it takes.
        /// The waiting is done by command "await Task.Delay(1000);" that pauses the method for one second.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonExecute_Click(object sender, RoutedEventArgs e)
        {
            //These check the user's choices from the UI
            robot.SetRobotDirection(comboBoxMoveDirection);
            robot.SetLineStyle(comboBoxLineType);
            bool result = true;
            int steps = int.Parse(comboBoxMoveSteps.Text.ToString());
            
            //Mandatory for-loop: moves the robot and gives the error message if borders are crossed.
            for (int i = 0; i < steps; i++)
            {
                result = await robot.MoveTheRobot();
                if (result == false)
                {
                    MessageBox.Show("Don't cross the borders!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
        }

        /// <summary>
        /// Example method: how to use Robot-class for manually coding the commands for the robot.
        /// The method is coded as an async method. This enables the waiting that robot does between the steps it takes.
        /// The waiting is done by command "await Task.Delay(1000);" that pauses the method for one second.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonSquare_Click(object sender, RoutedEventArgs e)
        {
            //Manully coded commands for the robot.
            robot.SetDrawingOn();
            robot.SetRobotDirection(Direction.Up);
            robot.SetLineStyle(LineStyle.Solid);
            robot.SetDrawingColor(LineColor.Red);
            robot.SetDrawingThickness(4.0);
            bool result = true;
            int steps = 1;

            //Outer for-loop creates the square
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < steps; i++)
                {
                    result = await robot.MoveTheRobot();
                    if (result == false)
                    {
                        MessageBox.Show("Don't cross the borders!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                robot.SetRobotDirection(Direction.TurnLeft);                
            }
        }

        /// <summary>
        /// Example event method for a button Clear_Click to erase everything on the canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            canvasArea.Children.Clear();
            Image robotImage = new Image();
            canvasArea.Children.Add(robotImage);
            robot = new Robot(canvasArea, robotImage, 0, 0);
        }

    }
}
