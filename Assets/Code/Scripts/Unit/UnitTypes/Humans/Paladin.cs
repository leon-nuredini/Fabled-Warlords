public class Paladin : LUnit
{
    private VictorsSmite _victorsSmite;
    
    #region Properties

    public VictorsSmite VictorsSmite => _victorsSmite;

    #endregion

    public override void InitProperties()
    {
        base.InitProperties();
        _victorsSmite = GetComponent<VictorsSmite>();
    }
}
