using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Language
{
	public enum LanguagesNames{
		Fa = 0,
		En
	}

	public LanguagesNames languageName;
	public string languageUIFontName;
    public float languageFontSize;    
	public string languageSkinPath;
	public string languageSubtitlePath;
	public string languageSubtitleFontName;
    public float languageSubtitleFontSize;

    public Language(LanguagesNames langName, string langUIFontName, float langFontSize, string langSkinPath, string langSubtitlePath, string langSubtitleFontName, float langSubtitleFontSize)
    {
		languageName = langName;
		languageUIFontName = langUIFontName;
		languageFontSize = langFontSize;
		languageSkinPath = langSkinPath;
		languageSubtitlePath = langSubtitlePath;
		languageSubtitleFontName = langSubtitleFontName;
        languageSubtitleFontSize = langSubtitleFontSize;
	}
}