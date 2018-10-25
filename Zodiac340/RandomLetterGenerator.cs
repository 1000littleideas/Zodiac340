﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Copyright (c) 2018 AThousandLittleIdeas.com. All Rights Reserved.
/// </summary>
namespace Zodiac340
{
    static public class RandomLetterGenerator
    {
        /// <summary>
        /// Dictionary of letters and their probability of occuring in the English language
        /// </summary>
        static private Dictionary<char, double> dictLetterProbabilitys = new Dictionary<char, double>()
        {
            {'e',.1116},
        };
        static public Random r = new Random();
        /// <summary>
        /// Gets a random character a-z taking into account the probability of a letter occurring in a word.
        /// </summary>
        /// <returns>A character</returns>
        static public char GetWeightedRandomChar()
        {
            double next = r.NextDouble();
            double sum = 0;
            // Keep adding probabilities until the sum is greater than the random double.  The current letter is then the one returned.
            foreach(KeyValuePair<char,double> kvp in dictLetterProbabilitys)
            {
                sum += kvp.Value;
                if (sum >= next)
                {
                    return kvp.Key;
                }
            }
            return 'e';
        }

    }
}