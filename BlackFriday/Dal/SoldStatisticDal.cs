using System.Collections.Generic;
using BlackFriday.Interfaces;
using BlackFriday.Models;
using System.Linq;

namespace BlackFriday.Dal
{
    public class SoldStatisticDal : ISoldStatisticDal
    {
        private static Dictionary<int, Sell> _sells = new Dictionary<int, Sell>();

        public int AddSell(ISell sell)
        {
            int key = 0;
            if(_sells.Count == 0)
            {
                key = 1;
            }
            else
            {
                key = _sells.Keys.Max() + 1;
            }
            
            _sells.Add(key, (Sell)sell);
            return key;
        }

        public int DeleteSell(int id)
        {
            if (_sells.ContainsKey(id))
            {
                _sells.Remove(id);
                return id;
            }
            throw new KeyNotFoundException();
        }

        public ISell GetSellById(int id)
        {
            if (_sells.ContainsKey(id))
            {
                return _sells[id];
            }
            throw new KeyNotFoundException();
        }

        public IEnumerable<ISell> GetSells()
        {
            return _sells.Values.ToList();
        }
    }
}
