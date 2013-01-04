using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Inhuman
{
    public class Block
    {
        string _Text = "";
        public string Text
        {
            get { return _Text; }
            set
            {
                _Text = value;
                Update();
            }
        }
        public List<Character> Characters = new List<Character>();

        public bool UseCaseSensitive = false;
        public bool UseStart = true;
        public bool UseEnd = false;
        public bool UseSpaces = false;
 
        //================================================================================================================================================================//
        public Block() { }
        public Block(string text)
        {
            Text = text;
            Update();
        }

        //================================================================================================================================================================//
        public void Print()
        {
            // Print Character //
            Debug.WriteLine("==================================================================================");
            Debug.WriteLine(Text);
            Debug.WriteLine("==================================================================================");

            for (int i = 0; i < Characters.Count; i++)
            {
                // Print Character //
                //Debug.WriteLine("=========================================");
                Debug.WriteLine("\"" + Characters[i].Value + "\"");
               // Debug.WriteLine("=========================================");

                // Print Before //
                //Debug.WriteLine("Before");
                foreach (KeyValuePair<String, int> entry in Characters[i].Before)
                {
                    Debug.WriteLine(" < " + entry.Key + ": " + entry.Value);
                } 

                // Print After //
                //Debug.WriteLine("After");
                foreach (KeyValuePair<String, int> entry in Characters[i].After)
                {
                    Debug.WriteLine(" > " + entry.Key + ": " + entry.Value);
                } 
            }
        }

        //================================================================================================================================================================//              
        public void Update()
        {
            Characters.Clear();

            if (!UseCaseSensitive) _Text = Text.ToLower();
            if (!UseSpaces) _Text = Text.Replace(" ", "");

            for (int i = 0; i < Text.Length; i++)
            {
                Character character = new Character(Text.Substring(i, 1));               

                if (!Characters.Contains(character))
                {
                    Characters.Add(character);
                }
                else
                {
                    int pos = Characters.IndexOf(character);
                    character = Characters[Characters.IndexOf(character)];                    
                }
                
                // Add Before //
                int before = i - 1;

                if (before >= 0)
                {
                    string beforeChar = Text.Substring(before, 1);

                    // Update Char //
                    if (character.Before.ContainsKey(beforeChar))
	                {
                        character.Before[beforeChar]++;
	                }
                    else
	                {
                        character.Before.Add(beforeChar, 1);
	                }         
                }
                else if (UseStart)
                {
                    // Add "Start" //
                    if (character.Before.ContainsKey(" "))
	                {
		                character.Before[" "]++;
	                }
                    else
	                {
                        character.Before.Add(" ", 1);
	                }        
                }

                // Add After //
                int after = i + 1;
                if (after < _Text.Length)
                {
                    string afterChar = _Text.Substring(after, 1);

                    // Update Char //
                    if (character.After.ContainsKey(afterChar))
                    {
                        character.After[afterChar]++;
                    }
                    else
                    {
                        character.After.Add(afterChar, 1);
                    }         
                }
                else if (UseEnd)
                {
                    // Add "End" //
                    if (character.After.ContainsKey("End"))
                    {
                        character.After["End"]++;
                    }
                    else
                    {
                        character.After.Add("End", 1);
                    }      
                    
                }
            }
        }

        //================================================================================================================================================================//
        public static float Compare(Block block1, Block block2)
        {
            int totalCount = 0;
            int totalChecks = 0;
            int totalMatches = 0;
            int totalFails = 0;
            int totalOffset = 0;
            float similarity = 0;

            // Get All Characters //
            List<Character> AllCharacters = new List<Character>();
            foreach (Character c in block1.Characters)
            {
                if (!AllCharacters.Contains(c))
                    AllCharacters.Add(c);
            }

            foreach (Character c in block2.Characters)
            {
                if (!AllCharacters.Contains(c))
                    AllCharacters.Add(c);
            }

            // Foreach character check each before and after ===============================//
            foreach (Character c in AllCharacters)
            {
                int charCount = 0;
                int charChecks = 0;
                int charMatches = 0;
                int charFails = 0;
                int charOffset = 0;
                float charSimilarity = 0;

                // Get Full Before List ===============================//
                List<string> AllBefore = new List<string>();

                if (block1.Characters.Contains(c))
                {
                    int index = block1.Characters.IndexOf(c);
                    Character block1Char = block1.Characters[index];

                    foreach (KeyValuePair<String, int> entry in block1Char.Before)
                    {
                        if (!AllBefore.Contains(entry.Key))
                            AllBefore.Add(entry.Key);
                    }
                }

                if (block2.Characters.Contains(c))
                {
                    int index = block2.Characters.IndexOf(c);
                    Character block2Char = block2.Characters[index];

                    foreach (KeyValuePair<String, int> entry in block2Char.Before)
                    {
                        if (!AllBefore.Contains(entry.Key))
                            AllBefore.Add(entry.Key);
                    }
                }

                // Get Full After List ===============================//
                List<string> AllAfter = new List<string>();

                if (block1.Characters.Contains(c))
                {
                    int index = block1.Characters.IndexOf(c);
                    Character block1Char = block1.Characters[index];

                    foreach (KeyValuePair<String, int> entry in block1Char.After)
                    {
                        if (!AllAfter.Contains(entry.Key))
                            AllAfter.Add(entry.Key);
                    }
                }

                if (block2.Characters.Contains(c))
                {
                    int index = block2.Characters.IndexOf(c);
                    Character block2Char = block2.Characters[index];

                    foreach (KeyValuePair<String, int> entry in block2Char.After)
                    {
                        if (!AllAfter.Contains(entry.Key))
                            AllAfter.Add(entry.Key);
                    }
                }

                // Compare Every Before ===============================//
                foreach (string text in AllBefore)
                {
                    Character block1Char = null;
                    Character block2Char = null;
                    int block1Count = 0;
                    int block2Count = 0;
                    
                    if (block1.Characters.Contains(c))
                    {
                        int index = block1.Characters.IndexOf(c);
                        block1Char = block1.Characters[index];

                        if (block1Char != null && block1Char.Before.ContainsKey(text))
                        {
                            block1Count = block1Char.Before[text];
                            totalChecks++;
                            charChecks++;
                        }
                    }

                    if (block2.Characters.Contains(c))
                    {
                        int index = block2.Characters.IndexOf(c);
                        block2Char = block2.Characters[index];

                        if (block2Char != null && block2Char.Before.ContainsKey(text))
                        {
                            block2Count = block2Char.Before[text];
                            totalChecks++;
                            charChecks++;
                        }
                    }

                    // Totals //
                    totalCount++;
                    charCount++;
                    if (block1Count == block2Count)
                    {
                        totalMatches++;
                        charMatches++;
                    }
                    else
                    {
                        totalFails++;
                        charFails++;
                    }

                    totalOffset += Math.Abs(block1Count - block2Count);
                    charOffset += Math.Abs(block1Count - block2Count);
                }

                // Compare Every After ===============================//
                foreach (string text in AllAfter)
                {
                    Character block1Char = null;
                    Character block2Char = null;
                    int block1Count = 0;
                    int block2Count = 0;

                    if (block1.Characters.Contains(c))
                    {
                        int index = block1.Characters.IndexOf(c);
                        block1Char = block1.Characters[index];

                        if (block1Char != null && block1Char.After.ContainsKey(text))
                        {
                            block1Count = block1Char.After[text];
                            totalChecks++;
                            charChecks++;
                        }
                    }

                    if (block2.Characters.Contains(c))
                    {
                        int index = block2.Characters.IndexOf(c);
                        block2Char = block2.Characters[index];

                        if (block2Char != null && block2Char.After.ContainsKey(text))
                        {
                            block2Count = block2Char.After[text];
                            totalChecks++;
                            charChecks++;
                        }
                    }

                    // Totals //
                    charCount++;
                    totalCount++;
                    if (block1Count == block2Count)
                    {
                        totalMatches++;
                        charMatches++;
                    }
                    else
                    {
                        totalFails++;
                        charFails++;
                    }

                    totalOffset += Math.Abs(block1Count - block2Count);
                    charOffset += Math.Abs(block1Count - block2Count);
                }

                charSimilarity = (float)(charChecks - charOffset) / charChecks;
                charSimilarity = Math.Max(0, charSimilarity);
                //Debug.Write("\"" + c.Value + "\" Similarity: " + (charSimilarity * 100f).ToString("N0") + "%");
                /*Debug.Write("\"" + c.Value + "\" ");
                for (int x = 0; x < charSimilarity * 20; x++)
                {
                    Debug.Write("|");
                }
                Debug.Write("\n");*/

                if (block1.Characters.Contains(c))
                {
                    int index = block1.Characters.IndexOf(c);
                    Character block1Char = block1.Characters[index];

                    block1Char.Similarity = charSimilarity;
                }

                if (block2.Characters.Contains(c))
                {
                    int index = block2.Characters.IndexOf(c);
                    Character block2Char = block2.Characters[index];

                    block2Char.Similarity = charSimilarity;
                }


                similarity += charSimilarity;
                //Debug.WriteLine("Char Checks: " + charChecks);
                //Debug.WriteLine("Char Offset: " + charOffset);
                //Debug.WriteLine("Char Matches: " + charMatches);
                //Debug.WriteLine("Char Fails: " + charFails);
                
            }
            //Debug.WriteLine("Total Checks: " + totalChecks);
            //Debug.WriteLine("Total Offset: " + totalOffset);
            //Debug.WriteLine("Total Matches: " + totalMatches);
            //Debug.WriteLine("Total Fails: " + totalFails);

            int numChars = block1.Text.Length + block2.Text.Length;
            float sizeRatio = (float)AllCharacters.Count / numChars;
            //Debug.WriteLine(sizeRatio);

            //similarity = (float)(totalChecks - totalOffset) / totalChecks;
            //similarity = Math.Max(0, similarity);
            similarity = similarity / AllCharacters.Count;

            //Debug.WriteLine("Similarity: " + (similarity * 100f).ToString("N0") + "%");

            return similarity;
        }
    }
}
