using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using openCV;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Image_Processing
{
    public partial class Form2 : Form
    {
        public Form1 frm1; // Reference to Form1

        // Constructor
        public Form2()
        {
            InitializeComponent();
        }

        // Event handler for button1 click
        private void button1_Click(object sender, EventArgs e)
        {
            // This method calculates the RGB histograms from an image and displays them using chart1.
            Bitmap bmp; // Declares a Bitmap variable
            bmp = (Bitmap)Form1.image1; // Assigns the image1 property from Form1 to the bmp variable

            int width = bmp.Width; // Gets the width of the image
            int height = bmp.Height; // Gets the height of the image

            // Arrays to store histogram information for red, green, and blue channels
            int[] ni_red = new int[256];
            int[] ni_green = new int[256];
            int[] ni_blue = new int[256];

            // Loop through each pixel in the image
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Get the color of the pixel at (x, y)
                    Color pixelColor = bmp.GetPixel(x, y);

                    // Increment the corresponding histogram array based on the pixel's RGB values
                    ni_red[pixelColor.R]++;
                    ni_green[pixelColor.G]++;
                    ni_blue[pixelColor.B]++;
                }
            }

            // Set colors for each series in chart1 (Red, Green, Blue)
            chart1.Series["Red"].Color = Color.FromArgb(200, 255, 0, 0);
            chart1.Series["Green"].Color = Color.FromArgb(200, 0, 255, 0);
            chart1.Series["Blue"].Color = Color.FromArgb(200, 0, 0, 255);

            // Add histogram data to each series in chart1
            for (int i = 0; i < 256; i++)
            {
                chart1.Series["Red"].Points.AddY(ni_red[i]);
                chart1.Series["Green"].Points.AddY(ni_green[i]);
                chart1.Series["Blue"].Points.AddY(ni_blue[i]);
            }
        }

        // Event handler for button2 click
        public void button2_Click(object sender, EventArgs e)
        {
            // This method performs histogram equalization on an image and displays the histograms using chart2.

            // Get the image from Form1
            Bitmap bmpImg = (Bitmap)Form1.image1;
            Bitmap newImage = bmpImg;
            int width = bmpImg.Width;
            int height = bmpImg.Height;

            // Arrays to store histogram information for red, green, and blue channels
            int[] ni_Red = new int[256];
            int[] ni_Green = new int[256];
            int[] ni_Blue = new int[256];

            // Calculate frequency of occurrence for each intensity level in the image
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color pixelColor = bmpImg.GetPixel(i, j);

                    ni_Red[pixelColor.R]++;
                    ni_Green[pixelColor.G]++;
                    ni_Blue[pixelColor.B]++;
                }
            }

            // Calculate probability of occurrence for each intensity level in the image
            decimal[] prob_ni_Red = new decimal[256];
            decimal[] prob_ni_Green = new decimal[256];
            decimal[] prob_ni_Blue = new decimal[256];

            for (int i = 0; i < 256; i++)
            {
                prob_ni_Red[i] = (decimal)ni_Red[i] / (decimal)(width * height);
                prob_ni_Green[i] = (decimal)ni_Green[i] / (decimal)(width * height);
                prob_ni_Blue[i] = (decimal)ni_Blue[i] / (decimal)(width * height);
            }

            // Calculate Cumulative Distribution Function (CDF)
            decimal[] cdf_Red = new decimal[256];
            decimal[] cdf_Green = new decimal[256];
            decimal[] cdf_Blue = new decimal[256];

            cdf_Red[0] = prob_ni_Red[0];
            cdf_Green[0] = prob_ni_Green[0];
            cdf_Blue[0] = prob_ni_Blue[0];

            for (int i = 1; i < 256; i++)
            {
                cdf_Red[i] = prob_ni_Red[i] + cdf_Red[i - 1];
                cdf_Green[i] = prob_ni_Green[i] + cdf_Green[i - 1];
                cdf_Blue[i] = prob_ni_Blue[i] + cdf_Blue[i - 1];
            }

            // Equalize the image using the calculated CDF values
            int red, green, blue;
            int constant = 255;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color pixelColor = bmpImg.GetPixel(i, j);

                    red = (int)Math.Round(cdf_Red[pixelColor.R] * constant);
                    green = (int)Math.Round(cdf_Green[pixelColor.G] * constant);
                    blue = (int)Math.Round(cdf_Blue[pixelColor.B] * constant);

                    Color newColor = Color.FromArgb(red, green, blue);
                    newImage.SetPixel(i, j, newColor);
                }
            }

            // Calculate histograms for the equalized image
            int n_width = newImage.Width;
            int n_height = newImage.Height;

            int[] ni_red = new int[256];
            int[] ni_green = new int[256];
            int[] ni_blue = new int[256];

            for (int y = 0; y < n_height; y++)
            {
                for (int x = 0; x < n_width; x++)
                {
                    Color pixelColor = newImage.GetPixel(x, y);

                    ni_red[pixelColor.R]++;
                    ni_green[pixelColor.G]++;
                    ni_blue[pixelColor.B]++;
                }
            }

            // Set colors for each series in chart2 (Red, Green, Blue)
            chart2.Series["Red"].Color = Color.FromArgb(200, 255, 0, 0);
            chart2.Series["Green"].Color = Color.FromArgb(200, 0, 255, 0);
            chart2.Series["Blue"].Color = Color.FromArgb(200, 0, 0, 255);

            // Add histogram data to each series in chart2
            for (int i = 0; i < 256; i++)
            {
                chart2.Series["Red"].Points.AddY(ni_red[i]);
                chart2.Series["Green"].Points.AddY(ni_green[i]);
                chart2.Series["Blue"].Points.AddY(ni_blue[i]);
            }

            // Update pictureBox2 in Form1 with the equalized image
            frm1.pictureBox2.Image = (Image)newImage;
            frm1.pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
