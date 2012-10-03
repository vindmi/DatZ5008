using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GooglePlus.Data.Contract
{
    public interface IObjectPersister<T, K>
    {
        void Write(T data);

        T Read(K key);
    }
}
