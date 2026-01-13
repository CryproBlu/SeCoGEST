
CREATE PROCEDURE [SeCoGEST].[CreaDocXMLInterventi]
(
	@IDTESTA uniqueidentifier
)
AS
BEGIN

	DECLARE @TIPODOC varchar(3) = 'RAP'
	
    DECLARE
	   @XML XML,	   
	   @ESISTE AS VARCHAR

			SET @XML=(
					SELECT 
					TestaDocumento.NUMERODOC AS NUMERODOC, 
					TestaDocumento.DATADOC AS DATADOC,
					TestaDocumento.CODCLIFOR AS CODCLIFOR , 
					TestaDocumento.NUMRIFDOC AS NUMRIFDOC,
					TestaDocumento.DATARIFDOC AS DATARIFDOC,
					@TIPODOC as TIPODOC , 
					TestaDocumento.ORAINIZIO, 
					TestaDocumento.ORAFINE,
					TestaDocumento.ANNOTAZIONE,
					@IDTESTA AS IDRAPPORTINO,
					RIGADOCUMENTO.codart AS CODART, 
					RIGADOCUMENTO.QTAGEST as QTAGEST,
					RIGADOCUMENTO.UMGEST as UMGEST,
					RIGADOCUMENTO.RIFCONTRATTO AS RIFCONTRATTO,
					RIGADOCUMENTO.TipologiaArticolo AS TIPOADDEBITO,
					RIGADOCUMENTO.PROGRESSIVO AS IDTESTACONTRATTO,
					RIGADOCUMENTO.descrizioneart AS DESCRIZIONE,
					RIGADOCUMENTO.DaFatturare as PRELEVABILE,
					RIGADOCUMENTO.PREZZOmetodo as PREZZOUNITLORDOEURO 
					FROM 
					( 
						select ID, CodiceCliente AS CODCLIFOR, Numero AS NUMERODOC, NumeroCommessa as NUMRIFDOC, DataPrevistaIntervento AS DATARIFDOC, DataPrevistaIntervento AS DATADOC ,
							   null as ORAINIZIO, null as ORAFINE, Definizione AS ANNOTAZIONE
						from SeCoGEST.Intervento
						where ID = @IDTESTA
					)
					as TestaDocumento
					left outer join 
					(
						select a.IDIntervento as IDINTERVENTO, a.DaFatturare,
						isnull(a.CodiceContratto,'') as RIFCONTRATTO, ISNULL(a.CodiceArticolo,'') AS CODART, 
						--MODIFICA 
						--case when isnull(a.TipologiaArticolo,0)= 4 then 'NR' else 'ORE' end AS UMGEST,  
						--case when isnull(a.TipologiaArticolo,0)= 4 then isnull(quantita,0) else  Ore end AS QTAGEST
						case when isnull(quantita,0)>0 then 'NR' else 'ORA' end AS UMGEST,  
						case when isnull(quantita,0)>0 then isnull(quantita,0) else  Ore end AS QTAGEST
						 ,isnull(a.TipologiaArticolo,0) as TipologiaArticolo ,isnull(a.progressivo,0) as progressivo ,isnull(a.descrizione,'') as descrizioneart,
						case 
							when isnull(a.TipologiaArticolo,0)= 0 then 0
							when isnull(a.TipologiaArticolo,0)= 1 then 0
							when isnull(a.TipologiaArticolo,0)= 2 then 0
							when isnull(a.TipologiaArticolo,0)= 3 then isnull((select s.ImportoServizi  from dbo._T_ExtraServizi S where isnull(s.Contratto,'#')=isnull(a.CodiceContratto,'') and isnull(s.Progressivo,0)=isnull(a.Progressivo,-1) and isnull(s.codconto,'')=isnull(i.CodiceCliente,'')  ),0)
							when isnull(a.TipologiaArticolo,0)= 4 then isnull((select s.Costo   from dbo._T_ExtraAddebito  S where isnull(s.Contratto,'#')=isnull(a.CodiceContratto,'') and isnull(s.Progressivo,0)=isnull(a.Progressivo,-1) and isnull(s.codconto,'')=isnull(i.CodiceCliente,'')  ),0)
							end 
							 AS PREZZOmetodo
						 
						from 
						SeCoGEST.Intervento I
						inner join  SeCoGEST.Intervento_Articolo a on i.id=a.IDIntervento
						where i.ID=@IDTESTA
					
					
					

					)
					as RIGADOCUMENTO  on TESTADOCUMENTO.ID =RIGADOCUMENTO.IDIntervento   					

					WHERE 
					CAST(TESTADOCUMENTO.ID AS uniqueidentifier) = @IDTESTA

					FOR XML AUTO, ROOT ('Documenti'), ELEMENTS XSINIL 
			);

PRINT 'OK'
	IF  (select count(*) from @XML.nodes('(Documenti/TestaDocumento/RIGADOCUMENTO)') as ParamValues(TaskChainerTask))>0 BEGIN
		--MODIFICA PER PERMETTERE DI RIGENERARE RAP IN CASO IL DOCUMENTO VENGA CANCELLATO
		--SET @ESISTE=ISNULL((SELECT COUNT(*) FROM _T_GeneratoreDocumenti WHERE 	idTesta =@TIPODOC+cast(@IDTESTA as varchar(50))),0)
		
		
		SET @ESISTE=
		ISNULL((SELECT COUNT(*) FROM TESTEDOCUMENTI WHERE
		PROGRESSIVO IN 
		(SELECT ISNULL(IDTESTAGENERATO,0) FROM _T_GeneratoreDocumenti WHERE ISNULL(IDTESTAGENERATO,0)>0 AND	idTesta =@TIPODOC+cast(@IDTESTA as varchar(50)))
		AND TIPODOC='RAP'
		),0)


		IF @ESISTE=0 BEGIN
			print ''
			--INSERT INTO
			INSERT INTO _T_GeneratoreDocumenti
				([idTesta]
			   ,[TipoDoc]
			   ,[Documento]
			   ,[idTestaGenerato]
			   ,[Stato]
			   ,[UtenteModifica]
			   ,[DataModifica])
			   VALUES
			   (
				@TIPODOC+cast(@IDTESTA as varchar(50))
				,'RAPPORTINO'
				,@XML
				,0
				,0
				,USER_NAME() 
				,CAST(GETDATE() AS DATE)
			   );
		END
	END
 












    RETURN

END