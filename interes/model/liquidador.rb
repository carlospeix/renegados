class Liquidador
  def initialize(interes_diario = 0)
    @interes_diario = interes_diario
  end

  def interes(capital, fecha_vencimiento, fecha_liquidacion)
    return 0 if deuda_no_vencida(fecha_vencimiento, fecha_liquidacion)

    return capital * (fecha_liquidacion - fecha_vencimiento).to_i / 100
  end

private
  def deuda_no_vencida(fecha_vencimiento, fecha_liquidacion)
    return fecha_vencimiento >= fecha_liquidacion
  end
  
end