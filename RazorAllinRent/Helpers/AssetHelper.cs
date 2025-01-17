namespace RazorAllinRent.Helpers
{
    public static class AssetHelper
    {
        // Base folder for assets
        private static string AssetsFolder = "~/assets/";

        // CSS loading helper
        public static string LoadCss(string fileName)
        {
            return $"{AssetsFolder}css/{fileName}";
        }

        // JavaScript loading helper
        public static string LoadJs(string fileName)
        {
            return $"{AssetsFolder}js/{fileName}";
        }

        // Image loading helper
        public static string LoadImage(string fileName)
        {
            return $"{AssetsFolder}img/{fileName}";
        }

        // Font loading helper
        public static string LoadFont(string fileName)
        {
            return $"{AssetsFolder}fonts/{fileName}";
        }

        // Example of loading Bootstrap CSS
        public static string LoadBootstrapCss(string fileName)
        {
            return $"{AssetsFolder}bootstrap/dist/css/{fileName}";
        }

        // Example of loading Bootstrap JS
        public static string LoadBootstrapJs(string fileName)
        {
            return $"{AssetsFolder}bootstrap/dist/js/{fileName}";
        }

        // Example of loading plugin JS
        public static string LoadPluginsJs(string fileName)
        {
            return $"{AssetsFolder}plugins/{fileName}";
        }
    }
}
