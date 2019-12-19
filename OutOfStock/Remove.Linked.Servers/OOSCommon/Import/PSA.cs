using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon.Import
{

    /// <summary>
    /// Parse WIMP records using Finite State Automata
    /// http://en.wikipedia.org/wiki/Finite-state_machine
    /// The class name is short to make the state table initialization more readable
    /// </summary>
    public class PSA
    {
        public pSt lineState { get; set; }
        public pAct lineAction { get; set; }

        /// The enum names are short to make the state table initialization more readable
        public enum pSt : int { Init = 0, SkipLine = 1, BeginField = 2, Collecting = 3, Done = 4, Size = 5 };
        public enum pAct : int { None, AddChar, PushField };
        public enum pIn : int { Space = 0, Sep = 1, Alpha = 2, Other = 3, EOL = 4, Size = 5 };

        /*
         * Implement the following state machine
         * 	            <space>	            '|'	                    <alpha>	            <other>	            EOL/EOF
         * 	Init	    Init,	            BeginField,	            SkipLine,	        SkipLine,	        ,Done
         * 	SkipLine	SkipLine,	        SkipLine,	            SkipLine,	        SkipLine,	        ,Done
         * 	BeginField	BeginField,	        BeginField,PushField	Collecting,AddChar	Collecting,AddChar	,Done
         * 	Collecting	Collecting,AddChar	BeginField,PushField	Collecting,AddChar	Collecting,AddChar	,Done
         */
        public static PSA[,] stateTransitions
        {
            get
            {
                if (_stateTransitions == null)
                {
                    _stateTransitions = new PSA[,]
                    {
{new PSA(pSt.Init),	new PSA(pSt.BeginField),	new PSA(pSt.SkipLine),	new PSA(pSt.SkipLine),	new PSA(pSt.Done)},
{new PSA(pSt.SkipLine),	new PSA(pSt.SkipLine),	new PSA(pSt.SkipLine),	new PSA(pSt.SkipLine),	new PSA(pSt.Done)},
{new PSA(pSt.BeginField),	new PSA(pSt.BeginField,pAct.PushField),	new PSA(pSt.Collecting,pAct.AddChar),	new PSA(pSt.Collecting,pAct.AddChar),	new PSA(pSt.Done,pAct.PushField)},
{new PSA(pSt.Collecting,pAct.AddChar),	new PSA(pSt.BeginField,pAct.PushField),	new PSA(pSt.Collecting,pAct.AddChar),	new PSA(pSt.Collecting,pAct.AddChar),	new PSA(pSt.Done,pAct.PushField)}
                    };
                }
                return _stateTransitions;
            }
            set { _stateTransitions = value; }
        } static PSA[,] _stateTransitions = null;

        public PSA(pSt lineState, pAct lineAction = pAct.None)
        {
            this.lineState = lineState;
            this.lineAction = lineAction;
        }

        /// <summary>
        /// Parse a line of input into an array of tokens
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static string[] ImportNextLine(string line)
        {
            List<string> result = new List<string>();
            pSt pStCurrent = pSt.Init;
            string fieldCurrent = string.Empty;

            for (int ix = 0; pStCurrent != pSt.Done && ix <= line.Length; ++ix)
            {
                pIn pInCurrent = pIn.Other;
                // Characterize input
                char ch = '\0';
                if (ix >= line.Length)
                    pInCurrent = pIn.EOL;
                else
                {
                    ch = line[ix];
                    if (ch == '|')
                        pInCurrent = pIn.Sep;
                    else if (char.IsWhiteSpace(ch))
                        pInCurrent = pIn.Space;
                    else if (char.IsLetter(ch))
                        pInCurrent = pIn.Alpha;
                }
                PSA psa = stateTransitions[(int)pStCurrent, (int)pInCurrent];
                switch (psa.lineAction)
                {
                    case pAct.AddChar:
                        fieldCurrent += ch;
                        break;
                    case pAct.PushField:
                        result.Add(fieldCurrent.Trim());
                        fieldCurrent = string.Empty;
                        break;
                }
                pStCurrent = psa.lineState;
            }
            return result.ToArray();
        }

    }
}
