namespace Pokemon3genRNGLibrary
{
    /// <summary>
    /// ヒンバススロットの実装. Singleton.
    /// </summary>
    public class FeebasSlotGenerator : MassOutBreakSlotGenerator
    {
        private static readonly FeebasSlotGenerator instance = new FeebasSlotGenerator();
        public static ITryGeneratable<GBASlot> GetInstance() => instance;
        private FeebasSlotGenerator() : base("ヒンバス", 20, 6) { }
    }
}
