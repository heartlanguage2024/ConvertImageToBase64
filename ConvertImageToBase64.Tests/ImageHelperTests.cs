using System;
using System.IO;
using ConvertImageToBase64;
using Xunit;

namespace ConvertImageToBase64.Tests
{
    public class ImageHelperTests
    {
        // Test ConvertPngToBase64 with a valid file path
        [Fact]
        public void ConvertPngToBase64_ValidFilePath_ReturnsBase64String()
        {
            // Arrange
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "TestImage.png"); // Use relative path for testing

            // Act
            string result = ImageHelper.ConvertPngToBase64(filePath);

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.False(string.IsNullOrEmpty(result)); // Ensure the result is a non-empty base64 string
        }

        // Test ConvertPngToBase64 when file path is empty
        [Fact]
        public void ConvertPngToBase64_EmptyFilePath_ReturnsNull()
        {
            // Act
            string result = ImageHelper.ConvertPngToBase64(string.Empty);

            // Assert
            Assert.Null(result);
        }

        // Test ConvertPngToBase64 when the file does not exist
        [Fact]
        public void ConvertPngToBase64_FileNotFound_ReturnsNull()
        {
            // Arrange
            string filePath = @"C:\NonExistentPath\Image.png"; // Specify a non-existing file path

            // Act
            string result = ImageHelper.ConvertPngToBase64(filePath);

            // Assert
            Assert.Null(result);
        }

        // Test ReadFile with a valid file path
        [Fact]
        public void ReadFile_ValidPath_ReturnsByteArray()
        {
            // Arrange
            // Get the relative path of the image in the "Images" folder within the test project
            string relativeFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "image.png");

            // Verify that the file exists before proceeding with the test
            Assert.True(File.Exists(relativeFilePath), "Test image file does not exist at the specified path.");

            // Read the expected bytes from the file
            byte[] expectedBytes = File.ReadAllBytes(relativeFilePath);

            // Act
            byte[] result = ImageHelper.ReadFile(relativeFilePath);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedBytes, result);
        }

        // Test ReadFile when file is not found
        [Fact]
        public void ReadFile_FileNotFound_ReturnsNull()
        {
            // Arrange
            string filePath = @"C:\NonExistentPath\Image.png";

            // Act
            byte[] result = ImageHelper.ReadFile(filePath);

            // Assert
            Assert.Null(result);
        }
    }
}
