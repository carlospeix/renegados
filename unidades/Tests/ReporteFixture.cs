using System.Collections.Generic;
using NUnit.Framework;
using Unidades.Modelo;

namespace Unidades.Tests
{
	[TestFixture]
	public class ReporteFixture
	{
		private Producto _prod;
		private IList<dynamic> _reporte;

		[SetUp]
		public void SetUp()
		{
			_prod = new Producto();
			_prod.AgregarUnidad("Unidad", 1);
			_prod.AgregarUnidad("Caja", 10, "Unidad");
			_prod.AgregarUnidad("Cajón", 50, "Unidad");
			_prod.AgregarUnidad("Pallet", 125, "Caja");

			_reporte = new List<dynamic>
			           	{
			           		new {Producto = _prod, Cantidad = 90, Unidad = "Unidad"}, // 90
			           		new {Producto = _prod, Cantidad = 25, Unidad = "Caja"},   // 250
			           		new {Producto = _prod, Cantidad = 15, Unidad = "Cajón"},  // 750
			           		new {Producto = _prod, Cantidad = 10, Unidad = "Pallet"}  // 12500
			           	};
		}

		[Test]
		public void ReporteDeberiaSumar()
		{
			var unidades = 0;

			Assert.AreEqual(90 + 250 + 750 + 12500, unidades);
		}
	}
}