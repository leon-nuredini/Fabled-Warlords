public class PlayerAccount
{
    private int _wealth;
    private int _upkeep;
    private int _economyIncome;

    public int Wealth { get => _wealth; set => _wealth = value; }

    public int Upkeep { get => _upkeep; set => _upkeep = value; }

    public int EconomyIncome { get => _economyIncome; set => _economyIncome = value; }
}