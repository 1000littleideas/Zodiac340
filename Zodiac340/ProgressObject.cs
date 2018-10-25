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
    /// <summary>
    /// An object to facilitate reporting progress from a background worker
    /// </summary>
    public class ProgressObject
    {
        private Candidate _topCandidate;
        public Candidate TopCandidate {
            get
            {
                if(_topCandidate==null)
                {
                    _topCandidate = new Candidate();
                }
                return _topCandidate;
            }
            set
            {
                _topCandidate = value;
            }
        }
        public int Generation { get; set; }
        public int Candidates { get; set; }
        
    }
}
