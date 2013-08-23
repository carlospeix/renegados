class Liquidador
  def initialize(tasas_de_interes_diario)
    @tasas_de_interes_diario = tasas_de_interes_diario
  end

  def interes(capital, fecha_vencimiento, fecha_liquidacion)
    interes = 0

    totalizador = Proc.new do |dias, tasa|
      interes = interes + capital * dias * tasa / 100
    end

    tasa_de_interes_segmentada(fecha_vencimiento, fecha_liquidacion, totalizador)

    return interes
  end

private
  def tasa_de_interes_segmentada(fecha_vencimiento, fecha_liquidacion, block)
    desde = fecha_vencimiento
    @tasas_de_interes_diario.each { |periodo_tasa|
      if desde <= fecha_liquidacion
        if desde <= periodo_tasa[1] # Si caigo en este periodo
          if fecha_liquidacion <= periodo_tasa[1]
            dias = (fecha_liquidacion - desde).to_i
            block.call(dias, periodo_tasa[2])
            desde = fecha_liquidacion + 1
          else
            dias = (periodo_tasa[1] - desde).to_i
            block.call(dias, periodo_tasa[2])
            desde = periodo_tasa[1]
          end
        end
      end
    }
  end

end