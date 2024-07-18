public class Paladin : LUnit, ISwordInfantry
{
    private VictorsSmiteSkill _victorsSmiteSkill;
    
    #region Properties

    public VictorsSmiteSkill VictorsSmiteSkill => _victorsSmiteSkill;

    #endregion

    public override void InitProperties()
    {
        base.InitProperties();
        _victorsSmiteSkill = GetComponent<VictorsSmiteSkill>();
    }
}
