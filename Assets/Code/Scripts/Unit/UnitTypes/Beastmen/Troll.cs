public class Troll : LUnit
{
    private RegenerationSkill _regenerationSkill;

    #region Properties

    public RegenerationSkill RegenerationSkill => _regenerationSkill;

    #endregion
    
    public override void InitProperties()
    {
        base.InitProperties();
        _regenerationSkill = GetComponent<RegenerationSkill>();
    }

}