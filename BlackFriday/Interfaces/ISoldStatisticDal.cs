using System.Collections.Generic;

namespace BlackFriday.Interfaces
{
    public interface ISoldStatisticDal
    {
        int AddSell(ISell sell);
        IEnumerable<ISell> GetSells();
        ISell GetSellById(int id);
        int DeleteSell(int id);
    }
}
