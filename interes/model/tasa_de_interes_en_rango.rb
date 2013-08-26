class TasaDeInteresEnRango
  def initialize(rango,tasa)
    @rango = rango
    @tasa = tasa
  end

  def interes_para(rango_de_deuda,day_count_basis)
    day_count_basis.calcular_dias(@rango.interseccion(rango_de_deuda)) * @tasa
  end
end
