namespace Pokemon3genRNGLibrary
{
    public class ForceEncounterDrawer : IEncounterDrawer
    {
        public bool DrawEncounter(ref uint seed) => true;

        private ForceEncounterDrawer() { }
        private static readonly ForceEncounterDrawer instance = new ForceEncounterDrawer();
        public static IEncounterDrawer Getinstance() => instance;
    }
}
