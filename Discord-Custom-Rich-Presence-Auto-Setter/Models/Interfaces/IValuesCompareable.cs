using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces
{
    public  interface IValuesComparable<T>:IValuesComparable where T:IValuesComparable<T> {
		public  bool ValuesEqual(T other);
	}

	public interface IValuesComparable {
	

	}
}
