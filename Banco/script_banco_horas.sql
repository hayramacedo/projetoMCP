--drop FUNCTION calcula_saldo_banco_horas

CREATE OR REPLACE FUNCTION calcula_saldo_banco_horas(id_func INT)
RETURNS INT AS $$
DECLARE
    saldo INT;
    hora_jornada INT;
    dt_inic_bancoh DATE;
    entrada TIME WITH TIME ZONE;
    pausa TIME WITH TIME ZONE;
    retorno TIME WITH TIME ZONE;
    saida TIME WITH TIME ZONE;
    total_horas INT;
    tolerancia INT := 10 * 60; -- 15 minutos em segundos
    tempo_trabalhado INT;
    entrada_seconds DOUBLE PRECISION;
    pausa_seconds DOUBLE PRECISION;
    retorno_seconds DOUBLE PRECISION;
    saida_seconds DOUBLE PRECISION;
BEGIN
    -- Obter a data de início do banco de horas e a jornada de trabalho diária do funcionário
    SELECT f.dt_inic_bancoh, c.hora_jornada
    INTO dt_inic_bancoh, hora_jornada
    FROM "FUNCIONARIO" f
    JOIN "CARGO" c ON f.id_cargo = c.id
    WHERE f.id = id_func;

    -- Calcular o saldo de horas desde a data de início do banco de horas até o dia atual
    saldo := 0;
    FOR entrada, pausa, retorno, saida IN
        SELECT f.entrada, f.pausa, f.retorno, f.saida
        FROM "FOLHA" f
        WHERE f.id_funcionario = id_func AND f.data_folha >= dt_inic_bancoh AND f.data_folha <= CURRENT_DATE 
		AND f.id_st_jornada <> 3 --Folga
		AND f.id_st_jornada <> 4 --Dispensa
    LOOP
        -- Converter os tempos para segundos
        entrada_seconds := EXTRACT(EPOCH FROM entrada);
        pausa_seconds := EXTRACT(EPOCH FROM pausa);
        retorno_seconds := EXTRACT(EPOCH FROM retorno);
        saida_seconds := EXTRACT(EPOCH FROM saida);

        -- Exibir os valores das variáveis
        RAISE NOTICE 'Entrada_seconds: %', entrada_seconds;
        RAISE NOTICE 'Pausa_seconds: %', pausa_seconds;
        RAISE NOTICE 'Retorno_seconds: %', retorno_seconds;
        RAISE NOTICE 'Saida_seconds: %', saida_seconds;
		
        -- Calcular o tempo total trabalhado no dia         
        tempo_trabalhado := saida_seconds - entrada_seconds - (retorno_seconds - pausa_seconds);
        		
		-- Descontar a tolerância de 15 minutos apenas se o tempo trabalhado for maior que a jornada total
        total_horas := hora_jornada * 3600; -- Convertendo horas para segundos
        IF tempo_trabalhado > total_horas + tolerancia THEN
            tempo_trabalhado := tempo_trabalhado - tolerancia;
	    ELSE
		  tempo_trabalhado := total_horas;
        END IF;
		
       -- Calcular o saldo de horas do dia
        saldo := saldo + (tempo_trabalhado - total_horas) / 60; -- Convertendo segundos para minutos
		
    END LOOP;
    
    RETURN saldo;
END;
$$ LANGUAGE plpgsql;



Select * from "BANCO_HORAS"

Select * from "FUNCIONARIO" WHERE id = 1

Select * from "FOLHA" where
id_funcionario = 1 
and data_folha < '2025-03-01'
order by data_folha


Select calcula_saldo_banco_horas(1)

SELECT f.entrada, f.pausa, f.retorno, f.saida, f.data_folha
FROM "FOLHA" f
WHERE f.id_funcionario = 1 
AND f.data_folha >= '2025-02-17' 
AND f.data_folha <= CURRENT_DATE 
AND f.id_st_jornada <> 3


