
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsomePattern.Core.Interfaces.Repositories
{
    public interface IUnitOfWork: IDisposable
    {
        #region Repositories

        #endregion

        void Save();
        Task SaveAsync();
    }
}

