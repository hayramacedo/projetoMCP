
Select * from "FOLHA" 
where id_funcionario = 1
  and data_folha = '2025-02-27'
  fetch first 1 rows only


Select * from "SOLICITACAO_AJUSTE"
delete from "SOLICITACAO_AJUSTE" where id = 6

SELECT last_value + 1 FROM sq_id_solicitacao_ajuste;


Select * from "USUARIO"

Select * from "SITUACAO_JORNADA"
Select * from "FUNCIONARIO"
Select * from "BANCO_HORAS"
Select * from "BANCOH_X_FOLHA"
Select * from "ATESTADO"
delete from "ATESTADO" where id = 5

Select * from "ESCALA" order by id
escala_jornada
5x2
6x1
5x1
12x36

insert into "ESCALA" (escala) values ('4x2')
SELECT now() - INTERVAL '1 day';



SELECT f.*, s.descricao 
FROM "FOLHA" f,
     "SITUACAO_JORNADA" s
WHERE s.id = f.id_st_jornada
--AND id = 
--AND id_funcionario = 
AND data_folha BETWEEN '2025-02-23' AND '2025-02-26'


Select * from "FOLHA" order by data_folha

delete from "FOLHA" where id > 1

Insert into "FOLHA" (data_folha,entrada,pausa,retorno,
saida,id_st_jornada,id_funcionario) values
(now(),'00:00:00-03','00:00:00-03','00:00:00-03','00:00:00-03',1,1)

SELECT f.* FROM "FUNCIONARIO" f WHERE f.id = 1--id_funcionario

SELECT 1 FROM FOLHA WHERE data_folha = dia

DROP FUNCTION atualizar_folha(integer)

CREATE OR REPLACE FUNCTION atualizar_folha(id_func integer) RETURNS void AS $$
DECLARE
    hoje DATE := CURRENT_DATE;--'01/03/2025';
    primeiro_dia DATE := date_trunc('month', hoje);
    ultimo_dia DATE := (date_trunc('month', hoje) + INTERVAL '1 month' - INTERVAL '1 day')::date;
    dia DATE;
	dt_admissao DATE;
	id_escala integer;
    trabalho integer;
    folga integer;
    ultimo_dia_folga DATE;
    id_situacao integer;
	contador_trabalho integer := 0;
    contador_folga integer := 0;
    modo char := 'T'; 
BEGIN
    -- Busca a data de admissão do funcionário
    SELECT f.dt_admissao INTO dt_admissao FROM "FUNCIONARIO" f WHERE f.id = id_func;

    -- Busca a escala do funcionário
    SELECT e.trabalho, e.folga INTO trabalho, folga
    FROM "ESCALA" e
    JOIN "FUNCIONARIO" f ON e.id = f.id_escala
    WHERE f.id = id_func;

    -- Verifica se a data de admissão é no mês atual
    IF dt_admissao >= primeiro_dia AND dt_admissao <= ultimo_dia THEN
        primeiro_dia := dt_admissao;
    END IF;

	-- Busca o último dia de folga
    SELECT ult_dia_folga INTO ultimo_dia_folga FROM "FOLHA" WHERE id_funcionario = id_func ORDER BY data_folha DESC LIMIT 1;

    -- Se o funcionário não tem um último dia de folga registrado, usa a data de admissão
    IF ultimo_dia_folga IS NULL THEN
        ultimo_dia_folga := dt_admissao - folga;
    END IF;

	-- Define o modo inicial (trabalho ou folga) com base no último dia de folga
    IF (hoje - ultimo_dia_folga) <= trabalho THEN
        modo := 'T';
        --contador_trabalho := (hoje - ultimo_dia_folga)::integer;
    ELSE
        modo := 'F';
        --contador_folga := (hoje - (ultimo_dia_folga + trabalho))::integer;
    END IF;
	
    FOR dia IN SELECT generate_series(primeiro_dia, ultimo_dia, '1 day'::interval)::date
    LOOP
	    IF modo = 'T' THEN
            id_situacao := (SELECT id FROM "SITUACAO_JORNADA" WHERE descricao = 'TRABALHO');
            contador_trabalho := contador_trabalho + 1;
            IF contador_trabalho = trabalho THEN
                contador_trabalho := 0;
                modo := 'F';
            END IF;
        ELSE
            id_situacao := (SELECT id FROM "SITUACAO_JORNADA" WHERE descricao = 'FOLGA');
            contador_folga := contador_folga + 1;
            ultimo_dia_folga := dia;
            IF contador_folga = folga THEN
                contador_folga := 0;
                modo := 'T';
            END IF;
        END IF;
	
        IF NOT EXISTS (SELECT 1 FROM "FOLHA" f WHERE f.data_folha = dia AND f.id_funcionario = id_func) THEN
		    Insert into "FOLHA" (data_folha,entrada,pausa,retorno,
				saida,id_st_jornada,id_funcionario,ult_dia_folga) values
				(dia,'00:00:00-03:00','00:00:00-03:00','00:00:00-03:00','00:00:00-03:00',id_situacao,id_func,ultimo_dia_folga);
        END IF;
    END LOOP;
END;
$$ LANGUAGE plpgsql;

drop procedure verificar_folha()

CREATE OR REPLACE PROCEDURE verificar_folha(id_funcionario integer) AS $$
BEGIN
  PERFORM atualizar_folha(id_funcionario);
END;
$$ LANGUAGE plpgsql;


CALL verificar_folha(1);
