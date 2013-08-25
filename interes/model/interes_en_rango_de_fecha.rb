class InteresEnRangoDeFecha
  def initialize(rango,interes)
    @rango = rango
    @interes = interes
  end

  def interes_para(rango)
    @rango.interseccion(rango).cantidad_de_dias * @interes
  end
end