using System.Windows;
using System.IO;

namespace ConvertImageToBase64
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string filePath = @"C:\Projects\Image.png"; // Specify the path to your PNG file
            string? base64String = ImageHelper.ConvertPngToBase64(filePath);

            if (!string.IsNullOrEmpty(base64String))
            {
                Console.WriteLine("Base64 String: ");
                Console.WriteLine(base64String);
            }
            else
            {
                Console.WriteLine("Error occurred while converting PNG to Base64.");
            }
        }

    }

    public static class ImageHelper
    {
        public static string? ConvertPngToBase64(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("File path is null or empty.");
                return null;
            }

            try
            {
                byte[]? imageBytes = ReadFile(filePath);
                return imageBytes != null ? ConvertToBase64(imageBytes) : null;
            }
            catch (Exception ex)
            {
                HandleError(ex);
                return null;
            }
        }

        public static byte[]? ReadFile(string filePath)
        {
            try
            {
                return File.Exists(filePath) ? File.ReadAllBytes(filePath) : null;
            }
            catch (Exception ex)
            {
                HandleError(ex);
                return null;
            }
        }


        public static string ConvertToBase64(byte[] imageBytes)
        {
            return Convert.ToBase64String(imageBytes);
        }

        private static void HandleError(Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

}