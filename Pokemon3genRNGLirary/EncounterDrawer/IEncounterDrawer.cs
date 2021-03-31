using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon3genRNGLibrary
{
    public interface IEncounterDrawer
    {
        bool DrawEncounter(ref uint seed);
    }
}
