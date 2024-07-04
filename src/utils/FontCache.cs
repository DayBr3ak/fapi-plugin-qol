
using System.Collections.Generic;
using TMPro;

public static class FontCache
{
    public static Dictionary<string, TMP_FontAsset> _fontCache = new Dictionary<string, TMP_FontAsset>();
    public static TMP_FontAsset GetTMP_FontAsset(string path)
    {
        if (_fontCache.ContainsKey(path))
        {
            return _fontCache[path];
        }
        var fonts = UnityEngine.Object.FindObjectsOfType<TMP_FontAsset>();
        TMP_FontAsset targetFont = null;
        foreach (TMP_FontAsset font in fonts)
        {
            if (font.name == path)
            {
                targetFont = font;
                break;
            }
        }

        if (targetFont != null)
        {
            _fontCache.Add(path, targetFont);
        }
        return targetFont;
    }
}