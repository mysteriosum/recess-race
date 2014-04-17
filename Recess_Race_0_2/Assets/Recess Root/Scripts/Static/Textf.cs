using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

static public class Textf {

	
	static public string GangsterWrap(string longstring,int[] lineLengths, out int lineAmount, out int longestLine){
		int currentLine = 0;
    	int charCount = 0;
		char[] delimiterChars = { ' ' };
		string[] words = longstring.Split(delimiterChars);
    	string result = "";
		int longest = 0;
		
 	    for (int index = 0; index < words.Length; index++) {
        	string word = words[index];

        	if (index == 0) {
            	result = word;
				charCount = word.Length;
            }
			else if (index > 0 ) {
            	charCount += word.Length + 1; //+1 because we assume that there will be a space after every word
            	if (charCount <= lineLengths[Mathf.Min (lineLengths.Length - 1, currentLine)]) {
					result += " " + word;
					if (charCount > longest){
						longest = charCount;
					}
				}
				else {
                	charCount = word.Length + 1;
	                result += Environment.NewLine + word;
					currentLine ++;
            	}

            }
			if (charCount > longest){
				longest = charCount;
			}
//			if (currentLine == lineLengths.Length){
//				return "string doesn't fit!";
//			}
        }
		longestLine = longest;
		lineAmount = currentLine + 1;
		return result;
    }
	/// <summary>
	/// Parses a script.
	/// </summary>
	/// <returns>
	/// The script in 
	/// </returns>
	/// <param name='script'>
	/// Script.
	/// </param>
	/// <param name='characters'>
	/// Characters.
	/// </param>
	public static string[] ParseScript (TextAsset script){
		string[] delimitors = new string[] { Environment.NewLine, "\n" };
		string wholeScript = script.text;
		
		string[] lines = wholeScript.Split(delimitors, StringSplitOptions.RemoveEmptyEntries);
		Debug.Log("First line is " + lines[0]);
		return lines;
	}
	
	public static string SplitIntoComponents (string fullText, int maxLength){
		if (fullText.Length <= maxLength){
			return fullText;
		}
		
		List<string> components = new List<string>();
		string remainingText = fullText;
		string modifiedText = "";
		int insuranceButton = 0;
		while (remainingText.Length > maxLength){
			insuranceButton ++;
			int splitAt = maxLength;
			while (remainingText[splitAt] != ' '){
				splitAt --;
				insuranceButton ++;
				if (insuranceButton > 10000){
					Debug.Log ("Too much!");
					break;
				}
			}
			components.Add (remainingText.Substring(0, splitAt));
			remainingText = remainingText.Remove (0, splitAt);
			//Debug.Log (remainingText);
			if (insuranceButton > 10000){
				Debug.Log ("Too much even!");
			}
		}
		
		components.Add (remainingText);
		
		for (int i = 0; i < components.Count -1; i ++){
			components[i] = components[i].Insert (components[i].Length, "...*");
		}
		
		modifiedText = string.Concat(components.ToArray());
		return modifiedText;
	}
	
	public static string ConvertTimeToString(float timeValue){
		if (timeValue < 0){
			return "00:00:00";
		}
		int seconds = (int) Mathf.Floor(timeValue % 60);
		float centiSeconds = (int) Mathf.Floor((timeValue - (int) timeValue) * 100);
		return Mathf.Floor(timeValue/60).ToString() + (seconds < 10? ":0" : ":") + seconds.ToString() + (centiSeconds < 10? ":0" : ":") + centiSeconds.ToString();
	}
}
