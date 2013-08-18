# encoding: utf-8

require File.join(File.dirname(__FILE__),'../model/liquidador')

describe Liquidador do

  it 'debería calcular cero interés con capital no vencido' do
    liquidador = Liquidador.new
    liquidador.interes(100, Date.today, Date.today).should == 0
  end

  it 'debería calcular 2 pesos de interés para un día despues del vencimiento' do
    liquidador = Liquidador.new(1)
    liquidador.interes(200, Date.today-1, Date.today).should == 2
  end

end