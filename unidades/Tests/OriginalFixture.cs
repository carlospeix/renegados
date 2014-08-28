using NUnit.Framework;
using Unidades.Modelo;

namespace Unidades.Tests
{
	[TestFixture]
	public class OriginalFixture
	{
		private Producto _prod;

		[SetUp]
		public void SetUp()
		{
			_prod = new Producto();
			_prod.AgregarUnidad("Unidad", 1);
			_prod.AgregarUnidad("Caja", 10);
		}

		[Test]
		public void UnidadesPor1UnidadDeberiaSer1()
		{
			Assert.AreEqual(1, _prod.UnidadesPorOld("Unidad"));
		}

		[Test]
		public void UnidadesPor1CajasDeberiaSer10()
		{
			Assert.AreEqual(10, _prod.UnidadesPorOld("Caja"));
		}
	}
}