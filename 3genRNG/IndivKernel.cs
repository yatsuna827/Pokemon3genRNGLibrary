using System;
using System.Collections.Generic;
using System.Text;

namespace _3genRNG
{
    public class IndivKernel
    {
        public uint Lv;
        public uint PID;
        public uint[] IVs;
        internal IndivKernel(uint PID, uint[] IVs, uint Lv = 50) { this.Lv = Lv; this.PID = PID; this.IVs = IVs; }
    }
}
