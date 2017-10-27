public class ChangePackage
{
    public Town.ImprovementTags Tag;
    public int Level;
    public int Population;
    public int Esteem;

    public ChangePackage(Town.ImprovementTags _tag, int _level, int _population, int _esteem)
    {
        Tag = _tag;
        Level = _level;
        Population = _population;
        Esteem = _esteem;
    }
}
