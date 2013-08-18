# encoding: utf-8

require File.join(File.dirname(__FILE__),'../model/liquidador')

describe Liquidador do

  it 'debería calcular cero interés con capital no vencido' do
    liquidador = Liquidador.new
    liquidador.interes(100, Date.today, Date.today).should == 0
  end

end