// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

Console.WriteLine("Process TMX");

// Load the TMX file
if (args.Length > 0)
{
    ProcessTMX(args[0]);
    Console.WriteLine("Run completed.");
    return 0;
}
else return 1;

static void ProcessTMX(string fileName)
{

    XmlDocument xmlDoc = new();
    xmlDoc.Load(fileName);
    XPathNavigator xPathNavigator = xmlDoc.CreateNavigator();
    XmlNamespaceManager xmlNamespaceManager = new(xPathNavigator.NameTable);

    StreamWriter outfile = new(fileName + ".cleanonly.tsv");

    outfile.WriteLine("EN\tES");

    // Select all TU elements
    XmlNodeList tuNodes = xmlDoc.SelectNodes("//tu");

    // Loop through each TU element and apply the method
    foreach (XmlNode tuNode in tuNodes)
    {
        // Apply your method to the TU element here
        // For example, you could extract the source and target segments:
        XmlNode segSource = tuNode.SelectSingleNode("./tuv[@xml:lang='EN-US']/seg", xmlNamespaceManager);
        string sourceText = segSource.InnerText;

        XmlNode segTarget = tuNode.SelectSingleNode("./tuv[@xml:lang='ES-ES']/seg", xmlNamespaceManager);
        string targetText = segTarget.InnerText;

        string cleanSourceText = RemoveMarkup(sourceText);
        string cleanTargetText = RemoveMarkup(targetText);

        if ((string.IsNullOrEmpty(cleanSourceText)) || (string.IsNullOrEmpty(cleanTargetText))) continue;
        if (cleanSourceText.StartsWith("Ramon")) continue;
        if (cleanSourceText.StartsWith("Diana")) continue;
        if (cleanSourceText.StartsWith("Edgar")) continue;

        if ((cleanSourceText != sourceText) || (cleanTargetText != targetText))
        {
            outfile.WriteLine($"{cleanSourceText}\t{cleanTargetText}");
        }
    }
    outfile.Close();
}

static string RemoveMarkup(string tuv)
{
    // regular expression pattern to match XML tags
    string pattern = @"<[^>]+>";

    // remove all XML tags from the string
    string plainText = Regex.Replace(tuv, pattern, "");

    //other cleanup

    plainText = plainText.Replace("\t", " ");
    plainText = plainText.Replace("• ", "");
    plainText = plainText.StartsWith("-") ? plainText[1..] : plainText;
    plainText = plainText.StartsWith("■") ? plainText[1..] : plainText;
    plainText = plainText.StartsWith("\"") ? "\"" + plainText : plainText;
    plainText = Regex.Replace(plainText, @"\.+", ".");


    // output plain text string
    return plainText.Trim();

}