class Liquidador
  def initialize(tasas_de_interes_diario)
    @tasas_de_interes_diario = tasas_de_interes_diario
  end

  def interes(capital, fecha_vencimiento, fecha_liquidacion)
    return 0 if deuda_no_vencida(fecha_vencimiento, fecha_liquidacion)

    interes = 0
    (fecha_vencimiento + 1..fecha_liquidacion).each { |dia|
      tasa_interes = tasas_de_interes_para(dia)
      interes = interes + capital * tasa_interes / 100
    }

    return interes
  end

private
  def deuda_no_vencida(fecha_vencimiento, fecha_liquidacion)
    return fecha_vencimiento > fecha_liquidacion
  end

  def tasas_de_interes_para(dia)
    @tasas_de_interes_diario.each { |tasa|
      return tasa[2] if (tasa[0]..tasa[1])===dia
    }
    return 0
  end

end