using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Copyright (c) 2018 AThousandLittleIdeas.com. All Rights Reserved.
/// </summary>
namespace Zodiac340
{
    public class Candidate
    {
        private Random r;

        private string _cipherKey, _resultString;
        private double? _fitness;
        //  private char[] _matchedLetters;
        // The 340 cipher with numbers 0-61 representing the 62 symbols
        private int[] cipher = {
            0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,
            17,4,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,
            19,33,34,35,36,18,37,38,14,25,20,32,12,21,39,0,40,
            41,4,4,42,6,5,43,29,7,44,4,22,18,18,2,30,15,
            45,46,36,18,39,47,48,16,10,49,50,8,18,8,51,9,52,
            4,43,2,6,50,5,22,53,29,16,54,9,50,3,15,24,20,
            21,49,18,30,55,23,56,15,37,35,57,14,7,27,39,12,10,
            20,14,15,40,31,48,21,22,18,45,17,26,39,18,58,12,46,
            16,28,36,18,59,18,38,2,15,50,19,35,33,60,61,51,30,
            53,39,5,37,7,18,6,40,18,22,4,42,28,50,19,33,53,
            37,18,2,52,49,47,1,10,24,26,19,4,59,13,36,30,22,
            15,28,35,5,2,40,10,29,49,13,49,36,27,18,8,19,50,
            39,61,46,41,33,21,18,17,10,49,50,19,35,20,56,43,2,
            5,14,50,17,6,31,49,15,49,59,27,35,7,49,47,18,18,
            33,19,57,11,29,34,51,46,54,1,3,7,37,38,49,53,18,
            10,35,27,44,39,19,30,20,22,4,6,27,31,36,55,14,15,
            2,35,13,18,12,49,15,54,28,18,50,5,25,19,10,32,12,
            18,18,32,25,54,39,25,35,8,22,41,0,13,52,20,32,4,
            10,50,9,16,25,28,42,47,19,45,26,22,19,29,53,54,35,
            3,36,24,0,17,4,9,41,39,38,22,43,60,10,30,56,18
         };
        private List<CharacterType> _characterTypes;
        #region Properties


        /// <summary>
        ///  A list of CharacterType enums found by matching letters to words in the dictionary
        /// </summary>
        public List<CharacterType> CharacterTypes
        {
            set
            {
                _characterTypes = value;
            }
            get
            {
                if (_characterTypes == null)
                {
                    _characterTypes = new List<CharacterType>();
                    int i = 0;
                    while (i < cipher.Length)
                    {
                        int maxWrdLength = getAvailableLength(i) > EnglishDictionary.LongestWordLength ?
                            EnglishDictionary.LongestWordLength : getAvailableLength(i);
                        bool matched = false;
                        // Match the longest word possible
                        for (int j = maxWrdLength; j > 1; j--)
                        {
                            string wrd = string.Empty;
                            for (int w = i; w < (i + j); w++)
                                wrd += CipherKey[cipher[w]];

                            // If matched
                            if (EnglishDictionary.Words.Contains(wrd))
                            {
                                for (int k = i; k < (wrd.Length + i); k++)
                                {
                                    CharacterType ct = CharacterType.MatchMiddle;
                                    if (k == i)
                                        ct = CharacterType.MatchStart;
                                    else if (k == (i + wrd.Length - 1))
                                        ct = CharacterType.MatchEnd;
                                    _characterTypes.Add(ct);
                                }
                                i += (wrd.Length);
                                matched = true;
                                break;
                            }
                        }
                        if (!matched)
                        {
                            _characterTypes.Add(CharacterType.NoMatch);
                            i++;
                        }
                    }
                }
                return _characterTypes;
            }
        }

        /// <summary>
        /// Fitness= Matched Characters/Total Characters
        /// </summary>
        public double? Fitness
        {
            set
            {
                _fitness = value;
            }
            get
            {
                return ((double)CharacterTypes.Where(ct=>ct!=CharacterType.NoMatch).Count()) / ((double)CharacterTypes.Count());
            }
        }


        public string CipherKey {
            get
            {
                if (_cipherKey == null)
                {
                    for (int i = 0; i < 62; i++)
                        _cipherKey += RandomLetterGenerator.GetWeightedRandomChar();
                }
                return _cipherKey;
            }
            set
            {
                _cipherKey = value;
            }

        }

        /// <summary>
        /// A string representing the result of translating the cipher with the key.  Found words are bracketed
        /// </summary>
        public string ResultString
        {
            get
            {
                if (_resultString == null)
                {
                    _resultString = string.Empty;

                    for (int i = 0; i < cipher.Length; i++)
                    {
                        if (CharacterTypes[i] == CharacterType.MatchStart)
                            _resultString += " [ ";
                        _resultString += CipherKey[cipher[i]]; 
                        if (CharacterTypes[i] == CharacterType.MatchEnd)
                            _resultString += " ] ";
                    }

                }
                return _resultString;
            }
                
             
        }
    
        
        
        /// <summary>
        /// Crossover between two candidates.  The crossover point is a random point in the cipher key.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="mutationProbability"></param>
        /// <returns></returns>
        public Candidate MateWith(Candidate c, double mutationProbability)
        {
            string newCipher = string.Empty;
            if (r == null)
                r = new Random();
            // Randomize a mutation
            if (r.NextDouble() <= mutationProbability)
            {
                char[] charArray = CipherKey.ToCharArray();
                charArray[r.Next(0, CipherKey.Length - 1)] = RandomLetterGenerator.GetWeightedRandomChar();
                CipherKey = new string(charArray);                               
            }
            // Randomize the crossover point
            int crossoverPt = r.Next(0, CipherKey.Length-1);
            for (int i=0;i<crossoverPt;i++)
            {
                newCipher += CipherKey[i];
            }
            for(int i=crossoverPt;i<c.CipherKey.Length;i++)
            {
                newCipher += c.CipherKey[i];
            }
            return new Candidate() { CipherKey = newCipher,Fitness=null };
        }
        #endregion
        /// <summary>
        /// Gets the characters left in the cipher
        /// </summary>
        /// <param name="startIndex"></param>
        /// <returns>Integer of the characters left in the cipher</returns>
        private int getAvailableLength(int startIndex)
        {
            return this.cipher.Length - startIndex;
        }
    }
}
