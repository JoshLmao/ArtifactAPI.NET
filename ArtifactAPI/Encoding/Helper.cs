namespace ArtifactAPI.Encoding
{
    internal class Helper
    {
        public static string StripTags(string s)
        {
            System.Text.RegularExpressions.Regex regHtml = new System.Text.RegularExpressions.Regex("<[^>]*>");
            return regHtml.Replace(s, "");
        }
    }
}
