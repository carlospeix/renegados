using System;

namespace Unidades.Modelo
{
	public class Unidad
	{
		private readonly string _nombre;
		private readonly int _cantidad;

		public Unidad(string nombre, int cantidad)
		{
			_nombre = nombre;
			_cantidad = cantidad;
		}

		public string Nombre
		{
			get { return _nombre; }
		}

		public int Cantidad
		{
			get { return _cantidad; }
		}
	}
}
