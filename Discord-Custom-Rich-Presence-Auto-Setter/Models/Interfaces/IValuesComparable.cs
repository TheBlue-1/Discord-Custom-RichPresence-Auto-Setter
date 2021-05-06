namespace Discord_Custom_Rich_Presence_Auto_Setter.Models.Interfaces {
	public interface IValuesComparable<in T> : IValuesComparable
		where T : IValuesComparable<T> {
		protected bool ValuesCompare(T other);

		public new static bool ValuesCompare<T1>(T1 original, T1 other)
			where T1 : IValuesComparable<T1> => other == null ? original == null : original.ValuesCompare(other);
	}

	public interface IValuesComparable {
		public static bool ValuesCompare<T1>(T1 original, T1 other)
			where T1 : IValuesComparable<T1> => IValuesComparable<T1>.ValuesCompare(original, other);
	}
}
