using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

static public class Textf {

	
	static public string GangsterWrap(string longstring,int[] lineLengths){
		int currentLine = 0;
    	int charCount = 0;
		char[] delimiterChars = { ' ' };
		String[] words = longstring.Split(delimiterChars);
    	String result = "";
		
 	    for (int index = 0; index < words.Length; index++) {
        	string word = words[index];

        	if (index == 0) {
            	result = word;
				charCount = word.Length;
            }
			else if (index > 0 ) {
            	charCount += word.Length + 1; //+1 because we assume that there will be a space after every word
            	if (charCount <= lineLengths[currentLine]) {
					result += " " + word;
				}
				else {
                	charCount = word.Length + 1;
	                result += Environment.NewLine + word;
					currentLine ++;
            	}

            }
			if (currentLine == lineLengths.Length){
				return "String doesn't fit!";
			}
        }
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
	public static String[] ParseScript (TextAsset script, String[] characters){
		String[] delimitors = new String[] { Environment.NewLine };
		string wholeScript = script.text;
		
		String[] lines = wholeScript.Split(delimitors, StringSplitOptions.RemoveEmptyEntries);
		
		return lines;
	}
	
	public static String SplitIntoComponents (String fullText, int maxLength){
		if (fullText.Length <= maxLength){
			return fullText;
		}
		
		List<String> components = new List<String>();
		String remainingText = fullText;
		String modifiedText = "";
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
		
		modifiedText = String.Concat(components.ToArray());
		return modifiedText;
	}
}
