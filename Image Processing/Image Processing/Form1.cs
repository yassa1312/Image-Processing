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
using System.Drawing.Imaging;

namespace Image_Processing
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
        }
        public static IplImage image1;
        public static IplImage img;
        public static Bitmap bmp;

       // Event handler for clicking the "openFileToolStripMenuItem" menu item
private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
{
    // Setting up the OpenFileDialog properties
    openFileDialog1.FileName = " "; // Setting initial FileName to an empty string
    openFileDialog1.Filter = "JPEG|*JPG|Bitmap|*.bmp|All|*.*-11"; // Filter for allowed file types
    
    // Opening the OpenFileDialog and checking if the user selected a file
    if (openFileDialog1.ShowDialog() == DialogResult.OK)
    {
        try
        {
            // Loading the selected image using a library (presumably OpenCV)
            image1 = cvlib.CvLoadImage(openFileDialog1.FileName, cvlib.CV_LOAD_IMAGE_COLOR);

            // Creating a size object to match the dimensions of pictureBox1
            CvSize size = new CvSize(pictureBox1.Width, pictureBox1.Height);

            // Creating a new image with the size of pictureBox1
            IplImage resized_image = cvlib.CvCreateImage(size, image1.depth, image1.nChannels);

            // Resizing the loaded image to fit the pictureBox1 using bilinear interpolation
            cvlib.CvResize(ref image1, ref resized_image, cvlib.CV_INTER_LINEAR);

            // Setting the BackgroundImage of pictureBox1 to display the resized image
            pictureBox1.BackgroundImage = (Image)resized_image;
        }
        catch (Exception ex)
        {
            // Handling any exceptions that might occur during the process
            MessageBox.Show(ex.Message); // Displaying an error message if an exception occurs
        }
    }
}


        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Creating a new image 'img' using CvCreateImage function with the dimensions and properties of 'image1'
            img = cvlib.CvCreateImage(new CvSize(image1.width, image1.height), image1.depth, image1.nChannels);

            // Getting the memory address of the image data of 'image1' and 'img'
            int srcAdd = image1.imageData.ToInt32();
            int dstAdd = img.imageData.ToInt32();

            // Starting an unsafe context to work with pointers
            unsafe
            {
                int srcIndex, dstIndex;
                // Looping through each row (r) and column (c) of the 'img' image
                for (int r = 0; r < img.height; r++)
                {
                    for (int c = 0; c < img.width; c++)
                    {
                        // Calculating the index for the source and destination pixels
                        srcIndex = dstIndex = (img.width * r * img.nChannels) + (c * img.nChannels);

                        // Setting the Red channel to 0 (eliminating red color) and copying the original blue and green channels
                        *(byte*)(dstAdd + dstIndex + 0) = 0;
                        *(byte*)(dstAdd + dstIndex + 1) = 0;
                        *(byte*)(dstAdd + dstIndex + 2) = *(byte*)(srcAdd + srcIndex + 2);
                    }
                }
            }

            // Creating a new image 'resized_image' with the dimensions of 'pictureBox2'
            CvSize size = new CvSize(pictureBox2.Width, pictureBox2.Height);
            IplImage resized_image = cvlib.CvCreateImage(size, img.depth, img.nChannels);

            // Resizing 'img' to fit 'resized_image' using linear interpolation
            cvlib.CvResize(ref img, ref resized_image, cvlib.CV_INTER_LINEAR);

            // Setting the image in 'pictureBox2' to be the resized image
            pictureBox2.Image = (Image)resized_image;
        }


        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            img = cvlib.CvCreateImage(new CvSize(image1.width, image1.height), image1.depth, image1.nChannels);
            int srcAdd = image1.imageData.ToInt32();
            int dstAdd = img.imageData.ToInt32();

            unsafe
            {
                int srcIndex, dstIndex;
                for (int r = 0; r < img.height; r++)
                {
                    for (int c = 0; c < img.width; c++)
                    {
                        srcIndex = dstIndex = (img.width * r * img.nChannels) + (c * img.nChannels);
                        *(byte*)(dstAdd + dstIndex + 0) = 0;
                        *(byte*)(dstAdd + dstIndex + 1) = *(byte*)(srcAdd + srcIndex + 1);
                        *(byte*)(dstAdd + dstIndex + 2) = 0;
                    }
                }
            }
            CvSize size = new CvSize(pictureBox2.Width, pictureBox2.Height);
            IplImage resized_image = cvlib.CvCreateImage(size, img.depth, img.nChannels);
            cvlib.CvResize(ref img, ref resized_image, cvlib.CV_INTER_LINEAR);
            pictureBox2.Image = (Image)resized_image;
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            img = cvlib.CvCreateImage(new CvSize(image1.width, image1.height), image1.depth, image1.nChannels);
            int srcAdd = image1.imageData.ToInt32();
            int dstAdd = img.imageData.ToInt32();

            unsafe
            {
                int srcIndex, dstIndex;
                for (int r = 0; r < img.height; r++)
                {
                    for (int c = 0; c < img.width; c++)
                    {
                        srcIndex = dstIndex = (img.width * r * img.nChannels) + (c * img.nChannels);
                        *(byte*)(dstAdd + dstIndex + 0) = *(byte*)(srcAdd + srcIndex + 0);
                        *(byte*)(dstAdd + dstIndex + 1) = 0;
                        *(byte*)(dstAdd + dstIndex + 2) = 0;
                    }
                }
            }
            CvSize size = new CvSize(pictureBox2.Width, pictureBox2.Height);
            IplImage resized_image = cvlib.CvCreateImage(size, img.depth, img.nChannels);
            cvlib.CvResize(ref img, ref resized_image, cvlib.CV_INTER_LINEAR);
            pictureBox2.Image = (Image)resized_image;
        }

        private void convertToGrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Casting 'image1' to a Bitmap named 'bmp'
            bmp = (Bitmap)image1;

            // Getting the width and height of the 'bmp' Bitmap
            int width = bmp.Width;
            int height = bmp.Height;

            Color p;
            // Looping through each pixel of the image
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Getting the color of the pixel at (x, y)
                    p = bmp.GetPixel(x, y);

                    // Extracting individual color components (alpha, red, green, blue)
                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    // Calculating the average of RGB values to convert to grayscale
                    int avg = (r + g + b) / 3;

                    // Creating a new grayscale color and setting it to the pixel at (x, y)
                    bmp.SetPixel(x, y, Color.FromArgb(a, avg, avg, avg));

                    // Setting the PictureBox to display the updated image
                    pictureBox2.Image = (Image)bmp;
                    pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }


        public void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void histogramToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.frm1 = this;
            frm2.Show();
        }

        private void removeNoiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Assuming 'image1' is an object containing an image that can be cast to a Bitmap
            Bitmap originalImage = (Bitmap)image1;

            // Creating a new Bitmap to store the denoised image with the same dimensions as the original image
            Bitmap denoisedImage = new Bitmap(originalImage.Width, originalImage.Height);

            // Size of the kernel used for the median filter
            int kernelSize = 3;

            // Locking the bits of both the original and denoised images for direct access to pixel data
            BitmapData originalData = originalImage.LockBits(new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                                                            ImageLockMode.ReadOnly,
                                                            PixelFormat.Format24bppRgb);
            BitmapData denoisedData = denoisedImage.LockBits(new Rectangle(0, 0, denoisedImage.Width, denoisedImage.Height),
                                                             ImageLockMode.WriteOnly,
                                                             PixelFormat.Format24bppRgb);

            unsafe
            {
                // Obtaining pointers to the start of the image data for direct manipulation
                byte* origScan0 = (byte*)originalData.Scan0;
                byte* denoisedScan0 = (byte*)denoisedData.Scan0;

                int bytesPerPixel = 3; // 24 bits per pixel (RGB)

                // Looping through each pixel in the original image
                for (int y = 0; y < originalImage.Height; y++)
                {
                    for (int x = 0; x < originalImage.Width; x++)
                    {
                        // Arrays to store RGB values for the kernel
                        byte[] redValues = new byte[kernelSize * kernelSize];
                        byte[] greenValues = new byte[kernelSize * kernelSize];
                        byte[] blueValues = new byte[kernelSize * kernelSize];

                        int index = 0;

                        // Iterating through the kernel around the current pixel
                        for (int ky = -kernelSize / 2; ky <= kernelSize / 2; ky++)
                        {
                            for (int kx = -kernelSize / 2; kx <= kernelSize / 2; kx++)
                            {
                                int offsetX = x + kx;
                                int offsetY = y + ky;

                                // Checking if the kernel falls within the image bounds
                                if (offsetX >= 0 && offsetX < originalImage.Width &&
                                    offsetY >= 0 && offsetY < originalImage.Height)
                                {
                                    // Accessing the pixel at the offset coordinates
                                    byte* pixel = origScan0 + (offsetY * originalData.Stride) + (offsetX * bytesPerPixel);
                                    redValues[index] = pixel[2];   // Red
                                    greenValues[index] = pixel[1]; // Green
                                    blueValues[index] = pixel[0];  // Blue
                                    index++;
                                }
                            }
                        }

                        // Sorting the RGB arrays to find the median values
                        Array.Sort(redValues);
                        Array.Sort(greenValues);
                        Array.Sort(blueValues);

                        // Setting the pixel in the denoised image to the median RGB values
                        byte* denoisedPixel = denoisedScan0 + (y * denoisedData.Stride) + (x * bytesPerPixel);
                        denoisedPixel[2] = redValues[index / 2];    // Median Red
                        denoisedPixel[1] = greenValues[index / 2];  // Median Green
                        denoisedPixel[0] = blueValues[index / 2];   // Median Blue
                    }
                }
            }

            // Unlocking the bits of both images after manipulation
            originalImage.UnlockBits(originalData);
            denoisedImage.UnlockBits(denoisedData);

            // Displaying the denoised image in pictureBox2 with stretching mode
            pictureBox2.Image = (Image)denoisedImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }



        private void smoothingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Assuming 'image1' is an object containing an image that can be cast to a Bitmap
            Bitmap originalImage = (Bitmap)image1;

            // Creating a new Bitmap to store the smoothed image with the same dimensions as the original image
            Bitmap smoothedImage = new Bitmap(originalImage.Width, originalImage.Height);

            // Size of the kernel used for the smoothing operation (adjust for different blur amounts)
            int kernelSize = 3;

            // Locking the bits of both the original and smoothed images for direct access to pixel data
            BitmapData originalData = originalImage.LockBits(new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                                                            ImageLockMode.ReadOnly,
                                                            PixelFormat.Format24bppRgb);
            BitmapData smoothedData = smoothedImage.LockBits(new Rectangle(0, 0, smoothedImage.Width, smoothedImage.Height),
                                                             ImageLockMode.WriteOnly,
                                                             PixelFormat.Format24bppRgb);

            unsafe
            {
                // Obtaining pointers to the start of the image data for direct manipulation
                byte* origScan0 = (byte*)originalData.Scan0;
                byte* smoothScan0 = (byte*)smoothedData.Scan0;

                int bytesPerPixel = 3; // 24 bits per pixel (RGB)

                // Looping through each pixel in the original image
                for (int y = 0; y < originalImage.Height; y++)
                {
                    for (int x = 0; x < originalImage.Width; x++)
                    {
                        int avgR = 0, avgG = 0, avgB = 0;
                        int count = 0;

                        // Iterating through the kernel around the current pixel
                        for (int ky = -kernelSize / 2; ky <= kernelSize / 2; ky++)
                        {
                            for (int kx = -kernelSize / 2; kx <= kernelSize / 2; kx++)
                            {
                                int offsetX = x + kx;
                                int offsetY = y + ky;

                                // Checking if the kernel falls within the image bounds
                                if (offsetX >= 0 && offsetX < originalImage.Width &&
                                    offsetY >= 0 && offsetY < originalImage.Height)
                                {
                                    // Accessing the pixel at the offset coordinates
                                    byte* pixel = origScan0 + (offsetY * originalData.Stride) + (offsetX * bytesPerPixel);
                                    avgR += pixel[2]; // Red
                                    avgG += pixel[1]; // Green
                                    avgB += pixel[0]; // Blue
                                    count++;
                                }
                            }
                        }

                        byte* smoothPixel = smoothScan0 + (y * smoothedData.Stride) + (x * bytesPerPixel);

                        if (count > 0)
                        {
                            // Calculating the average RGB values for the current pixel within the kernel
                            smoothPixel[2] = (byte)(avgR / count); // Smoothed Red
                            smoothPixel[1] = (byte)(avgG / count); // Smoothed Green
                            smoothPixel[0] = (byte)(avgB / count); // Smoothed Blue
                        }
                    }
                }
            }

            // Unlocking the bits of both images after manipulation
            originalImage.UnlockBits(originalData);
            smoothedImage.UnlockBits(smoothedData);

            // Displaying the smoothed image in pictureBox2 with stretching mode
            pictureBox2.Image = (Image)smoothedImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }



        private void averagingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Assuming 'image1' is an object containing an image that can be cast to a Bitmap
            Bitmap originalImage = (Bitmap)image1;

            // Creating a new Bitmap to store the averaged image with the same dimensions as the original image
            Bitmap averagedImage = new Bitmap(originalImage.Width, originalImage.Height);

            // Define the size of the kernel for the averaging operation
            int kernelSize = 3;

            // Calculate the divisor for averaging
            int divisor = kernelSize * kernelSize;

            // Locking the bits of both the original and averaged images for direct access to pixel data
            BitmapData originalData = originalImage.LockBits(new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                                                            ImageLockMode.ReadOnly,
                                                            PixelFormat.Format24bppRgb);
            BitmapData averagedData = averagedImage.LockBits(new Rectangle(0, 0, averagedImage.Width, averagedImage.Height),
                                                             ImageLockMode.WriteOnly,
                                                             PixelFormat.Format24bppRgb);

            unsafe
            {
                // Obtaining pointers to the start of the image data for direct manipulation
                byte* origScan0 = (byte*)originalData.Scan0;
                byte* avgScan0 = (byte*)averagedData.Scan0;

                int bytesPerPixel = 3; // 24 bits per pixel (RGB)

                // Looping through each pixel in the original image, excluding the border pixels
                for (int y = 1; y < originalImage.Height - 1; y++)
                {
                    byte* origRow = origScan0 + (y * originalData.Stride);
                    byte* avgRow = avgScan0 + (y * averagedData.Stride);

                    for (int x = 1; x < originalImage.Width - 1; x++)
                    {
                        int avgR = 0, avgG = 0, avgB = 0;

                        byte* origPixel = origRow + (x * bytesPerPixel);

                        // Iterating through the 3x3 kernel around the current pixel
                        for (int ky = -1; ky <= 1; ky++)
                        {
                            byte* neighborRow = origPixel + (ky * originalData.Stride);

                            for (int kx = -1; kx <= 1; kx++)
                            {
                                byte* neighborPixel = neighborRow + (kx * bytesPerPixel);
                                avgR += neighborPixel[2]; // Red
                                avgG += neighborPixel[1]; // Green
                                avgB += neighborPixel[0]; // Blue
                            }
                        }

                        byte* avgPixel = avgRow + (x * bytesPerPixel);
                        avgPixel[2] = (byte)(avgR / divisor); // Averaged Red
                        avgPixel[1] = (byte)(avgG / divisor); // Averaged Green
                        avgPixel[0] = (byte)(avgB / divisor); // Averaged Blue
                    }
                }
            }

            // Unlocking the bits of both images after manipulation
            originalImage.UnlockBits(originalData);
            averagedImage.UnlockBits(averagedData);

            // Displaying the averaged image in pictureBox2 with stretching mode
            pictureBox2.Image = (Image)averagedImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }



        private void mirrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Assuming image1 is a Bitmap object or something castable to Bitmap
            Bitmap bm1 = (Bitmap)image1; // Assuming image1 is your original image

            // Create a new bitmap to hold the mirrored image
            Bitmap mirroredImage = new Bitmap(bm1.Width, bm1.Height);

            // Perform the mirroring operation
            for (int y = 0; y < bm1.Height; y++)
            {
                for (int x = 0; x < bm1.Width; x++)
                {
                    // Mirroring horizontally by reversing the x-coordinate
                    mirroredImage.SetPixel(bm1.Width - 1 - x, y, bm1.GetPixel(x, y));
                }
            }

            // Display the mirrored image in pictureBox2
            pictureBox2.Image = (Image)mirroredImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void invertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Creating a new image 'img' using CvCreateImage function with the dimensions and properties of 'image1'
            img = cvlib.CvCreateImage(new CvSize(image1.width, image1.height), image1.depth, image1.nChannels);

            // Getting the memory address of the image data of 'image1' and 'img'
            int srcAdd = image1.imageData.ToInt32();
            int dstAdd = img.imageData.ToInt32();

            unsafe
            {
                int srcIndex, dstIndex;
                // Looping through each row and column of the 'img' image
                for (int r = 0; r < img.height; r++)
                {
                    for (int c = 0; c < img.width; c++)
                    {
                        srcIndex = dstIndex = (img.width * r * img.nChannels) + (c * img.nChannels);
                        // Inverting the color channels (RGB) of each pixel
                        *(byte*)(dstAdd + dstIndex + 0) = (byte)(255 - *(byte*)(srcAdd + srcIndex + 0)); // Invert Blue channel
                        *(byte*)(dstAdd + dstIndex + 1) = (byte)(255 - *(byte*)(srcAdd + srcIndex + 1)); // Invert Green channel
                        *(byte*)(dstAdd + dstIndex + 2) = (byte)(255 - *(byte*)(srcAdd + srcIndex + 2)); // Invert Red channel
                    }
                }
            }

            // Creating a new image 'resized_image' with the dimensions of 'pictureBox2'
            CvSize size = new CvSize(pictureBox2.Width, pictureBox2.Height);
            IplImage resized_image = cvlib.CvCreateImage(size, img.depth, img.nChannels);

            // Resizing 'img' to fit 'resized_image' using linear interpolation
            cvlib.CvResize(ref img, ref resized_image, cvlib.CV_INTER_LINEAR);

            // Setting the image in 'pictureBox2' to be the resized image
            pictureBox2.Image = (Image)resized_image;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }


    }
}
