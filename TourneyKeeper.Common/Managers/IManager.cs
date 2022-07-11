using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourneyKeeper.Common.Managers
{
    public interface IManager<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        void Update(TEntity entity, string token);
    }
}
