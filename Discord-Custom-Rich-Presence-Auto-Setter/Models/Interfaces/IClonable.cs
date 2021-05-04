﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Custom_Rich_Presence_Auto_Setter.Models
{

	public interface ICloneable<out T> :ICloneable where T:ICloneable<T> {
		protected  T Clone();
		public new static T1 Clone<T1>(T1 original) where T1 : ICloneable<T1> => original.Clone();
	}
	public interface ICloneable
	{
		public static T1 Clone<T1>(T1 original) where T1 : ICloneable<T1> => ICloneable<T1>.Clone(original);
	}
}
