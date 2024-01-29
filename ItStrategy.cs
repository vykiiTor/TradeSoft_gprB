namespace TradeSoft_gprB;

public interface ItStrategy
{
    int ApplyStrategy(Portfolio portfolio);
}


//class to me moved 

public class TestStrategy : ItStrategy
{
    public int ApplyStrategy(Portfolio portfolio)
    {
        if (portfolio.GetCash() >= 0)
            return -1;
        return 0;
    }
}