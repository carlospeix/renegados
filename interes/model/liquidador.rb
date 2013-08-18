class Liquidador
  def initialize(tasas_de_interes_diario)
    @tasas_de_interes_diario = tasas_de_interes_diario
  end

  def interes(capital, fecha_vencimiento, fecha_liquidacion)
    return 0 if deuda_no_vencida(fecha_vencimiento, fecha_liquidacion)

    tasa_interes = @tasas_de_interes_diario[0][2]
    cantidad_de_dias = (fecha_liquidacion - fecha_vencimiento).to_i

    return capital * cantidad_de_dias * tasa_interes / 100
  end

private
  def deuda_no_vencida(fecha_vencimiento, fecha_liquidacion)
    return fecha_vencimiento > fecha_liquidacion
  end

end