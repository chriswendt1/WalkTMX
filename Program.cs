// See https://aka.ms/new-console-template for more information
using System.Xml;

Console.WriteLine("Hello, World!");

// Load the TMX file
XmlDocument xmlDoc = new();
xmlDoc.Load("path/to/tmx/file");

// Select all TU elements
XmlNodeList tuNodes = xmlDoc.SelectNodes("//tu");

// Loop through each TU element and apply the method
foreach (XmlNode tuNode in tuNodes)
{
    // Apply your method to the TU element here
    // For example, you could extract the source and target segments:
    XmlNode segSource = tuNode.SelectSingleNode("./tuv[@xml:lang='en-US']/seg");
    string sourceText = segSource.InnerText;

    XmlNode segTarget = tuNode.SelectSingleNode("./tuv[@xml:lang='es-ES']/seg");
    string targetText = segTarget.InnerText;

    // Do something with the source and target segments
    Console.WriteLine("Source: " + sourceText);
    Console.WriteLine("Target: " + targetText);
}
