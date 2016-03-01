--Script Coletor Mobile 07/12/2015


--##########################################################################################################################
--#	Tabela para gerenciar as propostas que serão trabalhadas no coletor mobile                                             #
--# Sebastião																											   #		
--# 07/12/2015																											   #	
--##########################################################################################################################

	GO
		DROP TABLE [dbo].[tb1651_Picking_Mobile]
	GO


	SET ANSI_NULLS ON
	GO

	SET QUOTED_IDENTIFIER ON
	GO

	CREATE TABLE [dbo].[tb1651_Picking_Mobile](
		[codigoPICKINGMOBILE] [int] IDENTITY(1,1) NOT NULL,
		[propostaPICKINGMOBILE] [int] NOT NULL,
		[usuarioPICKINGMOBILE] [int] NOT NULL,
		[statusPICKINGMOBILE] [smallint] NOT NULL CONSTRAINT [dfPickingmobile_status]  DEFAULT ((0)),
		[horainicioPICKINGMOBILE] [datetime] NULL,
		[horafimPICKINGMOBILE] [datetime] NULL,
		[pesototalprodutosPICKINGMOBILE] [numeric](18, 4) NULL,
		[pesototalembalagensPICKINGMOBILE] [numeric](18, 4) NULL,
		[pesototalPICKINGMOBILE] [numeric](18, 4) NULL,
		[isinterrompidoPICKINGMOBILE] [tinyint] NULL CONSTRAINT [dfPickingmobile_interrompido]  DEFAULT ((0)),
	 CONSTRAINT [PKpickingMobileID] PRIMARY KEY CLUSTERED 
	(
		[codigoPICKINGMOBILE] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	GO

	ALTER TABLE [dbo].[tb1651_Picking_Mobile]  WITH CHECK ADD  CONSTRAINT [FKpropostaPicking] FOREIGN KEY([propostaPICKINGMOBILE])
	REFERENCES [dbo].[tb1601_Propostas] ([codigoPROPOSTA])
	GO

	ALTER TABLE [dbo].[tb1651_Picking_Mobile] CHECK CONSTRAINT [FKpropostaPicking]
	GO



--##########################################################################################################################
--#	Tabela para gerenciaras embalagens utilizadas para a separação dos itens da propostas (VOLUMES)                        #
--# Sebastião																											   #		
--# 07/12/2015																											   #	
--##########################################################################################################################

GO
	DROP TABLE [dbo].[tb1653_Picking_Embalagem_Separacoes]
GO

	SET ANSI_NULLS ON
GO

	SET QUOTED_IDENTIFIER ON
GO

	CREATE TABLE [dbo].[tb1653_Picking_Embalagem_Separacoes](
		[codigoEMBALAGEMSEPARACOES] [int] IDENTITY(1,1) NOT NULL,
		[pickingmobileEMBALAGEMSEPARACOES] [int] NOT NULL,
		[embalagemEMBALAGEMSEPARACOES] [int] NOT NULL,
		[quantidadeEMBALAGEMSEPARACOES] [int] NULL,
	 CONSTRAINT [PK_EmbalagemSeparacoes] PRIMARY KEY CLUSTERED 
	(
		[codigoEMBALAGEMSEPARACOES] ASC,
		[pickingmobileEMBALAGEMSEPARACOES] ASC,
		[embalagemEMBALAGEMSEPARACOES] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

GO

	DROP TABLE [dbo].[tb1652_Picking_Prioridade]
	GO

	SET ANSI_NULLS ON
	GO

	SET QUOTED_IDENTIFIER ON
	GO

	CREATE TABLE [dbo].[tb1652_Picking_Prioridade](
		[propostaPRIORIDADE] [int] NOT NULL,
		[valorPRIORIDADE] [int] NOT NULL,
		[usuarioPRIORIDADE] [int] NOT NULL,
		[dataPRIORIDADE] [datetime] NOT NULL
	) ON [PRIMARY]

GO


--##########################################################################################################################
--# Esta coluna irá armazenar informações geradas pelo coletor mobile para cada item que foi liberado.                     #
--# Sebastião																											   #		
--# 07/12/2015																											   #	
--##########################################################################################################################

    IF NOT EXISTS(SELECT C.name FROM SYS.COLUMNS C WHERE C.name = 'xmlsequenciaITEMPROPOSTA')
		ALTER TABLE tb1602_Itens_Proposta ADD xmlsequenciaITEMPROPOSTA NTEXT NULL

GO


--##########################################################################################################################
--#	Recupera informações para os propostas a serem liberadas retirando as propostas que estão em liberação.                #
--# Sebastiao																											   #		
--# 07/10/2015																											   #	
--##########################################################################################################################


	IF EXISTS(	SELECT O.name FROM SYSOBJECTS O WHERE O.name = 'vwMobile_tb1601_Proposta')
		DROP VIEW vwMobile_tb1601_Proposta
	GO


	CREATE VIEW vwMobile_tb1601_Proposta


		AS
			 
			SELECT COALESCE(codigoPICKINGMOBILE,0) AS codigoPICKINGMOBILE,
			codigoPROPOSTA,numeroPROPOSTA, CONVERT(nvarchar, P.dataLIBERACAOPROPOSTA,103)  +' '+ CONVERT(nvarchar, P.dataLIBERACAOPROPOSTA,108) as dataLIBERACAOPROPOSTA,clientePROPOSTA AS clientePROPOSTA, razaoEMPRESA ,
			COALESCE(ordemseparacaoimpressaPROPOSTA,0) AS ordemseparacaoimpressaPROPOSTA,volumesPROPOSTA as volumesPROPOSTA,COALESCE(valorPRIORIDADE,(SELECT MAX(valorPRIORIDADE) FROM tb1652_Picking_Prioridade)+1) as Prioridade,COALESCE(isinterrompidoPICKINGMOBILE,0) AS isinterrompidoPICKINGMOBILE
			FROM tb1601_Propostas (NOLOCK) 
			INNER JOIN tb1611_Liberacoes_Proposta P (NOLOCK) ON P.propostaLIBERACAOPROPOSTA = codigoPROPOSTA 
			LEFT JOIN tb1611_Liberacoes_Proposta C (NOLOCK) ON C.propostaLIBERACAOPROPOSTA = codigoPROPOSTA 
			LEFT JOIN tb0301_Empresas (NOLOCK) ON clientePROPOSTA = codigoEMPRESA
			LEFT JOIN tb1651_Picking_Mobile ON propostaPICKINGMOBILE = codigoPROPOSTA AND statusPICKINGMOBILE =0
			LEFT JOIN tb1652_Picking_Prioridade ON propostaPRIORIDADE = codigoPROPOSTA 
			WHERE statusPROPOSTA = 1 
			AND P.liberacaoLIBERACAOPROPOSTA = 1 
			AND C.liberacaoLIBERACAOPROPOSTA = 2 
			AND P.liberadoLIBERACAOPROPOSTA = 1  
			AND C.liberadoLIBERACAOPROPOSTA = 0
			AND (codigoPROPOSTA NOT IN (
											SELECT propostaPICKINGMOBILE	
											FROM tb1651_Picking_Mobile 
											WHERE statusPICKINGMOBILE > 0
										))

  GO

--##########################################################################################################################
--#	Informações dois produtos de uma propostas.                                                                            #
--# Sebastiao																											   #		
--# 07/12/2015																											   #	
--##########################################################################################################################


	IF EXISTS(	SELECT O.name FROM SYSOBJECTS O WHERE O.name = 'fn0003_informacoesProdutos')
		DROP FUNCTION fn0003_informacoesProdutos
	GO

	CREATE FUNCTION fn0003_informacoesProdutos ( @codigoProposta int )

				RETURNS @InformationTable TABLE
				   (
						codigoPRODUTO				INT,
						partnumberPRODUTO			NVARCHAR(50),
						nomePRODUTO					NVARCHAR(150),
						ean13PRODUTO				NVARCHAR(15),
						pesoLiquidoPRODUTO          NUMERIC(18,4),
						pesobrutoPRODUTO            NUMERIC(18,4),
						codigolotePRODUTO			INT,
						identificacaolotePRODUTO	NVARCHAR(50),
						codigolocalPRODUTO			INT,
						nomelocalPRODUTO			NVARCHAR(20)
				   )
				AS
				BEGIN
				   INSERT @InformationTable
					   SELECT codigoPRODUTO,partnumberPRODUTO,nomePRODUTO,ean13PRODUTO,pesoliquidoPRODUTO,pesobrutoPRODUTO,codigoLOTE, identificacaoLOTE,codigoLOCAL,nomeLOCAL
								FROM tb1205_Lotes
								INNER JOIN tb0501_Produtos ON produtoLOTE = codigoPRODUTO
								INNER JOIN tb1212_Lotes_Locais (NOLOCK) ON codigoLOTE = loteLOTELOCAL
																		   AND  codigoLOTE IN  
																							(
																								SELECT loteRESERVA
																								FROM tb1206_Reservas (NOLOCK) 
																								INNER JOIN tb1602_Itens_Proposta (NOLOCK) ON codigoITEMPROPOSTA = docRESERVA 
																								INNER JOIN tb1212_Lotes_Locais (NOLOCK) ON loteRESERVA = loteLOTELOCAL
																								WHERE propostaITEMPROPOSTA = @codigoProposta
																								AND tipodocRESERVA = 1602 
																								AND statusITEMPROPOSTA = 3 
																								AND separadoITEMPROPOSTA = 0
																							)
								INNER JOIN tb1211_Locais ON codigoLOCAL = localLOTELOCAL 
								WHERE codigoPRODUTO IN (   
														SELECT produtoRESERVA AS codigoPRODUTO
														FROM tb1206_Reservas (NOLOCK) 
														INNER JOIN tb1602_Itens_Proposta (NOLOCK) ON codigoITEMPROPOSTA = docRESERVA 
														WHERE propostaITEMPROPOSTA = @codigoProposta
														AND tipodocRESERVA = 1602 
														AND statusITEMPROPOSTA = 3 
				  										AND separadoITEMPROPOSTA = 0  
													)
						   RETURN
				END

    GO

--##########################################################################################################################
--#	Realiza o split em uma string e retorna uma tabela com as partes separadas                                             #
--# Sebastiao																											   #		
--# 07/12/2015																											   #	
--##########################################################################################################################


	IF EXISTS(	SELECT O.name FROM SYSOBJECTS O WHERE O.name = 'fn0003_SplitTitanium')
		DROP FUNCTION fn0003_SplitTitanium
	GO

	CREATE FUNCTION fn0003_SplitTitanium( @InputString VARCHAR(8000), @Delimiter VARCHAR(50))

		RETURNS @Items TABLE (Item VARCHAR(8000))

		AS
		BEGIN
			  IF @Delimiter = ' '
			  BEGIN
					SET @Delimiter = ','
					SET @InputString = REPLACE(@InputString, ' ', @Delimiter)
			  END

			  IF (@Delimiter IS NULL OR @Delimiter = '')
					SET @Delimiter = ','

			  DECLARE @Item           VARCHAR(8000)
			  DECLARE @ItemList       VARCHAR(8000)
			  DECLARE @DelimIndex     INT

			  SET @ItemList = @InputString
			  SET @DelimIndex = CHARINDEX(@Delimiter, @ItemList, 0)
			  WHILE (@DelimIndex != 0)
			  BEGIN
					SET @Item = SUBSTRING(@ItemList, 0, @DelimIndex)
					INSERT INTO @Items VALUES (@Item)

					-- Set @ItemList = @ItemList minus one less item
					SET @ItemList = SUBSTRING(@ItemList, @DelimIndex+1, LEN(@ItemList)-@DelimIndex)
					SET @DelimIndex = CHARINDEX(@Delimiter, @ItemList, 0)
			  END -- End WHILE

			  IF @Item IS NOT NULL -- At least one delimiter was encountered in @InputString
			  BEGIN
					SET @Item = @ItemList
					INSERT INTO @Items VALUES (@Item)
			  END

			  -- No delimiters were encountered in @InputString, so just return @InputString
			  ELSE INSERT INTO @Items VALUES (@InputString)

			  RETURN

	END

    GO
	
--##########################################################################################################################
--#	Recupera informações do lote de reserva de um item da proposta                                                         #
--# Sebastiao																											   #		
--# 07/12/2015																											   #	
--##########################################################################################################################

	IF EXISTS(	SELECT O.name FROM SYSOBJECTS O WHERE O.name = 'fn1211_LotesReservaProduto')
		DROP FUNCTION fn1211_LotesReservaProduto
	GO


	CREATE FUNCTION fn1211_LotesReservaProduto(@codigoPRODUTO int,@propostaReserva int)

		RETURNS NVARCHAR(20)

		BEGIN
    
			DECLARE @NumLote NVARCHAR(20)
			DECLARE @NumsLotes NVARCHAR(20)

			SET @NumsLotes = ' '

			DECLARE Local_Cursor CURSOR FOR 


				SELECT loteRESERVA
				FROM tb1206_Reservas (NOLOCK)
				INNER JOIN tb1602_Itens_Proposta (NOLOCK) ON codigoITEMPROPOSTA = docRESERVA
				INNER JOIN tb0501_Produtos (NOLOCK) ON produtoITEMPROPOSTA = codigoPRODUTO 
				WHERE produtoRESERVA = @codigoPRODUTO
				AND propostaITEMPROPOSTA = @propostaReserva
				AND tipodocRESERVA = 1602 
				AND statusITEMPROPOSTA = 3
				AND separadoITEMPROPOSTA = 0  
				ORDER BY produtoRESERVA ASC


			OPEN Local_Cursor

			FETCH NEXT FROM Local_Cursor INTO @NumLote

			WHILE @@FETCH_STATUS = 0
			BEGIN


				SET @NumsLotes =  @NumsLotes + ',' + @NumLote


				FETCH NEXT FROM Local_Cursor INTO @NumLote

			END

			CLOSE Local_Cursor
			DEALLOCATE Local_Cursor

		RETURN SUBSTRING(LTRIM(RTRIM(@NumsLotes)),2,LEN(LTRIM(RTRIM(@NumsLotes)))-1)    

		END 
 
	GO

--##########################################################################################################################
--#	Recupera informações dos locais de armazenamento de cada produto                                                       #
--# Sebastiao																											   #		
--# 07/12/2015																											   #	
--##########################################################################################################################

	IF EXISTS(	SELECT O.name FROM SYSOBJECTS O WHERE O.name = 'fn1211_LocaisLoteProduto')
		DROP FUNCTION fn1211_LocaisLoteProduto
	GO


    CREATE FUNCTION fn1211_LocaisLoteProduto(@codigoPRODUTO INT,@lotePRODUTO NVARCHAR(10))

		RETURNS NVARCHAR(20)

		BEGIN
    
			DECLARE @Local NVARCHAR(20)
			DECLARE @LocalNames NVARCHAR(20)

			SET @LocalNames = ' '

			DECLARE Local_Cursor CURSOR FOR 

			SELECT nomeLOCAL 
			FROM tb1205_Lotes
			INNER JOIN tb1212_Lotes_Locais ON codigoLOTE = loteLOTELOCAL
			INNER JOIN tb1211_Locais ON codigoLOCAL = localLOTELOCAL
			WHERE produtoLOTE = @codigoPRODUTO AND codigoLOTE IN (SELECT * FROM  dbo.fn0003_SplitTitanium(@lotePRODUTO,',') )
			ORDER BY nomeLOCAL DESC

			OPEN Local_Cursor

			FETCH NEXT FROM Local_Cursor INTO @Local

			WHILE @@FETCH_STATUS = 0
			BEGIN


				SET @LocalNames =  @Local  + ',' + @LocalNames


				FETCH NEXT FROM Local_Cursor INTO @Local

			END

			CLOSE Local_Cursor
			DEALLOCATE Local_Cursor

		RETURN SUBSTRING(LTRIM(RTRIM(@LocalNames)),1,LEN(LTRIM(RTRIM(@LocalNames)))-1)    

	END 

GO


--##########################################################################################################################
--#	Recupera informações dos locais de armazenamento de cada produto                                                       #
--# Sebastiao																											   #		
--# 07/12/2015																											   #	
--##########################################################################################################################


	IF EXISTS(	SELECT O.name FROM SYSOBJECTS O WHERE O.name = 'sps1601_manipulaPRIORIDADEPICKING')
		DROP PROCEDURE sps1601_manipulaPRIORIDADEPICKING
	GO

	CREATE PROCEDURE sps1601_manipulaPRIORIDADEPICKING(@TIPO_PROCEDIMENTO INT,@PROPOSTA INT,@USER INT )
		   -----------------
		   -- PARAMETRO @TIPO_PROCEDIMENTO
		   -- || VALOR  || ACAO 
		   --      1    || REALIZA O INSERT 
		   --      2    || REALIZA O DELETE 
		   --      3    || DECREMENTA A PRIORIDADE DO ITEM
		   --      4    || INCREMENTA A PRIORIDADE DO ITEM
		   --

		AS 

		BEGIN

			DECLARE @VALOR_ATUAL_PRIORIDADE AS INT
			DECLARE @PROPOSTA_PRIORIDADE_ANTERIOR AS INT 
			DECLARE @PROPOSTA_PRIORIDADE_POSTERIOR AS INT 

			 --REALIZA PROCEDIMENTOS PARA INSERIR A PROPOSTA NA TABELA DE PRIORIDADES
			IF(@TIPO_PROCEDIMENTO)=1
				BEGIN 
	    						-- VERIFICA SE A PROPOSTA A SER INSERIDA EXISTE NA TABELA
					IF( SELECT COUNT(*) FROM tb1652_Picking_Prioridade WHERE propostaPRIORIDADE = @PROPOSTA ) > 0
					PRINT 'passei aqui'
						BEGIN 
						--RECUPERA O VALOR ATUAL DA DA PRIORIDADE
						SET  @VALOR_ATUAL_PRIORIDADE = (SELECT valorPRIORIDADE FROM tb1652_Picking_Prioridade WHERE propostaPRIORIDADE = @PROPOSTA)
		    
						--DELETA A PROPOSTA DA TABELA
						DELETE 
						FROM tb1652_Picking_Prioridade 
						WHERE propostaPRIORIDADE = @PROPOSTA

						--ATUALIZA O VALOR DOS ITENS COM PRIORIDADE ACIMA DA PROPOSTA DELETADA
						UPDATE tb1652_Picking_Prioridade
						SET valorPRIORIDADE = valorPRIORIDADE-1
						WHERE valorPRIORIDADE >=  @VALOR_ATUAL_PRIORIDADE

					END

					INSERT INTO tb1652_Picking_Prioridade
					(propostaPRIORIDADE,dataPRIORIDADE,usuarioPRIORIDADE,valorPRIORIDADE)
					VALUES 
					(@PROPOSTA,GETDATE(),@USER, (SELECT COALESCE(MAX(valorPRIORIDADE),0)FROM tb1652_Picking_Prioridade) + 1 )

					SELECT * 
					FROM tb1652_Picking_Prioridade
					ORDER BY valorPRIORIDADE ASC

			END
			--REALIZA PROCEDIMENTOS PARA DELETAR A PROPOSTA NA TABELA DE PRIORIDADES
			IF(@TIPO_PROCEDIMENTO)=2
				  BEGIN 

					IF( SELECT COUNT(*) FROM tb1652_Picking_Prioridade WHERE propostaPRIORIDADE = @PROPOSTA ) > 0 
						--RECUPERA O VALOR ATUAL DA PRIORIDADE
						SET  @VALOR_ATUAL_PRIORIDADE = (SELECT valorPRIORIDADE FROM tb1652_Picking_Prioridade WHERE propostaPRIORIDADE = @PROPOSTA)
		    
						--DELETA A PROPOSTA DA TABELA
						DELETE 
						FROM tb1652_Picking_Prioridade 
						WHERE propostaPRIORIDADE = @PROPOSTA

						--ATUALIZA O VALOR DOS ITENS COM PRIORIDADE ACIMA DA PROPOSTA DELETADA
						UPDATE tb1652_Picking_Prioridade
						SET valorPRIORIDADE = valorPRIORIDADE-1
						WHERE valorPRIORIDADE >=  @VALOR_ATUAL_PRIORIDADE

						SELECT * 
						FROM tb1652_Picking_Prioridade
						ORDER BY valorPRIORIDADE ASC
				
					END
			--REALIZA DECREMENTO NA PRIORIDADE DA PROPOSTA TRATADA 
			IF(@TIPO_PROCEDIMENTO)=3
				BEGIN 

					IF( SELECT COUNT(*) FROM tb1652_Picking_Prioridade WHERE propostaPRIORIDADE = @PROPOSTA ) > 0
							--RECUPERA O VALOR PRIORIDADE DA PROPOSTA A SER TRABALHADA
							SET  @VALOR_ATUAL_PRIORIDADE = (SELECT valorPRIORIDADE FROM tb1652_Picking_Prioridade WHERE propostaPRIORIDADE = @PROPOSTA)
							print 'proposta a ser tratada ' + convert(varchar(10),@VALOR_ATUAL_PRIORIDADE)

							--CASO A PROPOSTA TENHA A PRIORIDADE MAIOR QUE UM REALIZA O PROCEDIMENTO
							IF(@VALOR_ATUAL_PRIORIDADE)>1
								BEGIN
									--RECUPERA A PROPOSTA COM PRIORIDADE ANTERIOR
									SET @PROPOSTA_PRIORIDADE_ANTERIOR = (SELECT propostaPRIORIDADE FROM tb1652_Picking_Prioridade WHERE valorPRIORIDADE = (@VALOR_ATUAL_PRIORIDADE -1)  )
                            
									--INCREMENTA A PROPOSTA ANTERIOR
									UPDATE tb1652_Picking_Prioridade
									SET  valorPRIORIDADE = valorPRIORIDADE+1
										 ,dataPRIORIDADE = GETDATE()
										 ,usuarioPRIORIDADE = @USER
									WHERE propostaPRIORIDADE = @PROPOSTA_PRIORIDADE_ANTERIOR;

									--DECREMENTA A PROPOSTA TRATADA
									UPDATE tb1652_Picking_Prioridade
									SET valorPRIORIDADE = valorPRIORIDADE-1
										,dataPRIORIDADE = GETDATE()
										,usuarioPRIORIDADE = @USER
									WHERE propostaPRIORIDADE = @PROPOSTA;

								END

						SELECT * 
						FROM tb1652_Picking_Prioridade
						ORDER BY valorPRIORIDADE ASC

				END
			--REALIZA INCREMENTO NA PRIORIDADE DA PROPOSTA TRATADA 
			IF(@TIPO_PROCEDIMENTO)=4
				  BEGIN 

						IF( SELECT COUNT(*) FROM tb1652_Picking_Prioridade WHERE propostaPRIORIDADE = @PROPOSTA ) > 0
							--RECUPERA O VALOR PRIORIDADE DA PROPOSTA A SER TRABALHADA
							SET  @VALOR_ATUAL_PRIORIDADE = (SELECT valorPRIORIDADE FROM tb1652_Picking_Prioridade WHERE propostaPRIORIDADE = @PROPOSTA)
							print 'proposta a ser tratada ' + convert(varchar(10),@VALOR_ATUAL_PRIORIDADE)

							--CASO A PROPOSTA TENHA A PRIORIDADE MAIOR QUE UM REALIZA O PROCEDIMENTO
							IF(@VALOR_ATUAL_PRIORIDADE)>=1

								IF( @VALOR_ATUAL_PRIORIDADE < (SELECT MAX(valorPRIORIDADE)FROM tb1652_Picking_Prioridade ))
									BEGIN
										--RECUPERA A PROPOSTA COM PRIORIDADE ANTERIOR
										SET @PROPOSTA_PRIORIDADE_POSTERIOR = (SELECT propostaPRIORIDADE FROM tb1652_Picking_Prioridade WHERE valorPRIORIDADE = (@VALOR_ATUAL_PRIORIDADE + 1)  )
                            
										--DECREMENTA A PROPOSTA POSTERIOR
										UPDATE tb1652_Picking_Prioridade
										SET  valorPRIORIDADE = valorPRIORIDADE-1
											 ,dataPRIORIDADE = GETDATE()
											 ,usuarioPRIORIDADE = @USER
										WHERE propostaPRIORIDADE = @PROPOSTA_PRIORIDADE_POSTERIOR;

										--INCREMENTA A PROPOSTA TRATADA
										UPDATE tb1652_Picking_Prioridade
										SET valorPRIORIDADE = valorPRIORIDADE+1
											,dataPRIORIDADE = GETDATE()
											,usuarioPRIORIDADE = @USER
										WHERE propostaPRIORIDADE = @PROPOSTA;

									END

						SELECT * 
						FROM tb1652_Picking_Prioridade
						ORDER BY valorPRIORIDADE ASC

              END
	     END

GO

--Permissoes necessárias para trabalhar com o coletor mobile

	IF NOT EXISTS (
		SELECT metodoMETODO
		FROM tb0034_Metodos
		WHERE METODOMETODO IN ('Guarda Volumes Mobile')
	)
	BEGIN
		INSERT INTO tb0034_Metodos VALUES (5,'Guarda Volumes Mobile')
	END
GO
	IF NOT EXISTS (
		SELECT metodoMETODO
		FROM tb0034_Metodos
		WHERE METODOMETODO IN ('Liberacao Vendas Mobile')
	)
	BEGIN
		INSERT INTO tb0034_Metodos VALUES (5,'Liberacao Vendas Mobile')
	END



