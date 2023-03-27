// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;
using System.Xml;
using System.Text.RegularExpressions;

Console.WriteLine("Process TMX");

// Load the TMX file
if (args.Length > 0)
{
    ProcessTMX(args[0]);
    return 0;
}
else return 1;

static void ProcessTMX(string fileName)
{

    XmlDocument xmlDoc = new();
    xmlDoc.Load(fileName);

    StreamWriter outfile = new(fileName + ".cleanonly.tsv");

    // Select all TU elements
    XmlNodeList tuNodes = xmlDoc.SelectNodes("//tu");

    // Loop through each TU element and apply the method
    foreach (XmlNode tuNode in tuNodes)
    {
        // Apply your method to the TU element here
        // For example, you could extract the source and target segments:
        XmlNode segSource = tuNode.SelectSingleNode("./tuv[@xml:lang='EN-US']/seg");
        string sourceText = segSource.InnerText;

        XmlNode segTarget = tuNode.SelectSingleNode("./tuv[@xml:lang='ES-ES']/seg");
        string targetText = segTarget.InnerText;

        string cleanSourceText = RemoveMarkup(sourceText);
        string cleanTargetText = RemoveMarkup(targetText);

        if ((cleanSourceText != sourceText) || (cleanTargetText != targetText))
        {
            outfile.WriteLine($"{cleanSourceText}\t{cleanTargetText}");
        }
    }

}

static string RemoveMarkup(string tuv)
{
    // regular expression pattern to match XML tags
    string pattern = @"<[^>]+>";

    // remove all XML tags from the string
    string plainText = Regex.Replace(tuv, pattern, "");

    // output plain text string
    return plainText;

}