Imports System.Data.OleDb
Public Class actionBDD
    Dim con As OleDbConnection = New OleDbConnection
    Dim cmd As OleDbCommand = New OleDbCommand
    Dim idDernierTravail As String
    '        CONNEXION A LA BASE DES DONNNEES
    Private Function connexion() As Boolean
        Try
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=inscription.mdb"
            If con.State = 0 Then
                con.Open()
            End If
            If con.State = 0 Then
                Return False
            Else
                Return True
            End If
        Catch e As OleDbException
            MsgBox("Erreur lors de la connexion à la base ou il vient de se passer beaucoup des requêtes.")
        Catch e As InvalidOperationException
            'MsgBox("Echec d'envoi du message")
            'Dim x As New Design
            'x.afficherErreurDescriptive("Opération non effectuée avec succès.")
        End Try
        Return False
    End Function


    Public Function enregistrerEtudiant(matricule As String, nom As String, postNom As String, prenom As String, lieuNaissance As String, nationalite As String, dateNaissance As DateTimePicker, house As String, Optional typeEtudiant As Integer = 1) As Boolean
        If verifierExistanceMatricule(matricule) = False Then
            connexion()
            Dim cmd As New OleDbCommand
            cmd.Connection = con
            cmd.CommandText = "INSERT INTO Etudiants(numeroMatricule,nom,postNom,prenom,lieuNaissance,dateNaissance,typeEtudiant,house,nationalite) VALUES('" & matricule & "','" & nom & "','" & postNom & "','" & prenom & "','" & lieuNaissance & "','" & dateNaissance.Value & "','" & typeEtudiant & "','" & house & "','" & nationalite & "')"
            cmd.ExecuteNonQuery()
            deconnexion()
            Return True
        Else
            Return False
        End If
    End Function

    Public Function obtenirDernierId() As String
        connexion()
        Dim cmd As New OleDbCommand
        cmd.Connection = con
        cmd.CommandText = "SELECT numeroMatricule FROM Etudiants"
        Dim lect As OleDbDataReader
        lect = cmd.ExecuteReader
        Dim matricule As String = ""
        While lect.Read
            If lect("numeroMatricule") <> vbNull Then
                matricule = lect("numeroMatricule").ToString
            End If
        End While
        deconnexion()
        Return matricule
    End Function

    Public Function obtenirDernierMatricule() As String
        connexion()
        Dim cmd As New OleDbCommand
        cmd.Connection = con
        cmd.CommandText = "SELECT numeroMatricule FROM Etudiants"
        Dim lect As OleDbDataReader
        lect = cmd.ExecuteReader
        Dim matricule As String = "00107"
        While lect.Read
            matricule = obtenirGrandMatricule(matricule, lect("numeroMatricule").ToString.PadLeft(5, "0"))
        End While
        deconnexion()
        matricule = (CType(matricule(0) & matricule(1) & matricule(2), Integer) + 1).ToString.PadLeft(3, "0") & matricule(3) & matricule(4)
        Return matricule
    End Function

    Public Function chercherEtudiant(nom As String) As DataSet
        connexion()
        Dim dt As New OleDbDataAdapter("SELECT * FROM Etudiants WHERE nom LIKE '%" & nom & "%' OR postNom LIKE '%" & nom & "%' OR prenom LIKE '%" & nom & "%'", con)
        Dim ensDonnees As New DataSet()
        dt.Fill(ensDonnees, "Etudiants")
        'ensDonnees.Tables("EntiteAcademique").Rows(1)
        Return ensDonnees
        deconnexion()
    End Function

    Public Function chargerListeEtudiantsImprimer(type As Integer, Optional nom As String = "", Optional auditoire As String = "", Optional departement As String = "") As DataSet
        Dim ensDonnees As New DataSet()
        If type = 1 Then
            'Recherche des tous
            connexion()
            Dim dt As New OleDbDataAdapter("SELECT numeroMatricule AS Matricule,nom as Nom,postNom as Postnom,prenom AS Prenom,lieuNaissance AS 'Lieu de naissance',dateNaissance AS 'Date de naissance',nationalite AS Nationalite,house AS house FROM Etudiants ORDER BY id DESC", con)
            dt.Fill(ensDonnees, "Etudiants")
            deconnexion()
        ElseIf type = 2 Then
            'Recherche par les noms
            If nom <> "" Then
                connexion()
                Dim dt As New OleDbDataAdapter("SELECT * FROM Etudiants WHERE nom LIKE '%" & nom & "%' OR postNom LIKE '%" & nom & "%' OR prenom LIKE '%" & nom & "%' ORDER BY id DESC", con)
                dt.Fill(ensDonnees, "Etudiants")
                deconnexion()
            End If
        ElseIf type = 3 Then
            'Recherche par auditoire et departement ou faculte
            If auditoire <> "" And departement <> "" Then
                connexion()

                Dim dt As New OleDbDataAdapter("SELECT * FROM Etudiants WHERE nom LIKE '%" & nom & "%' OR postNom LIKE '%" & nom & "%' OR prenom LIKE '%" & nom & "%'", con)
                dt.Fill(ensDonnees, "Etudiants")
                deconnexion()
            Else

            End If
        End If

        Return ensDonnees
    End Function



    Private Function obtenirGrandMatricule(matricule1 As String, matricule2 As String) As String
        Dim part1 As String
        Dim part2 As String
        part1 = matricule1(0) & matricule1(1) & matricule1(2)
        part2 = matricule1(3) & matricule1(4)
        Dim ent1, ent2, ent3, ent4 As Integer
        ent1 = CType(part1, Integer)
        ent2 = CType(part2, Integer)
        part1 = matricule2(0) & matricule2(1) & matricule2(2)
        part2 = matricule2(3) & matricule2(4)
        ent3 = CType(part1, Integer)
        ent4 = CType(part2, Integer)

        If ent4 > ent2 Then
            'Le deuxieme est grand
            Return matricule2
        ElseIf ent4 < ent2 Then
            'Le deuxieme est petit
            Return matricule1
        ElseIf ent4 = ent2 Then
            If ent1 > ent3 Then
                'Le premier est grand
                Return matricule1
            Else
                'Le deuxieme est grand
                Return matricule2
            End If
        End If
        Return vbNull
    End Function

    Public Function enregistrerAffecterSalle(matricule As String, entite As Integer, anneeAcademique As Integer, provenance As String, pourcentage As Double) As Boolean
        If matricule <> "" Then
            If verifierDejaInscrit(matricule, entite, anneeAcademique) = False Then
                connexion()
                Dim cmd As New OleDbCommand
                Dim idEtudiant As Integer = obtenirIdEtudiant(matricule)
                cmd.Connection = con
                cmd.CommandText = "INSERT INTO sInscrit(idEtudiant,numeroMatricule,idEntiteAcademique,typeEntiteAcademique,pourcentage,provenance,anneeAcademique) VALUES('" & idEtudiant & "','" & matricule & "'," & entite & ",'" & obtenirTypeEntite(entite) & "'," & pourcentage & ",'" & provenance & "'," & anneeAcademique & ")"
                cmd.ExecuteNonQuery()
                deconnexion()
                Return True
            End If
        End If
        Return False
    End Function

    Public Function verifierDejaInscrit(matricule As String, idEntite As Integer, anneeAcademique As Integer) As Boolean
        If matricule <> "" Then
            connexion()
            Dim cmd As New OleDbCommand
            cmd.Connection = con
            cmd.CommandText = "SELECT idInscription FROM sInscrit WHERE numeroMatricule='" & matricule & "' AND idEntiteAcademique=" & idEntite & " AND anneeAcademique=" & anneeAcademique
            Dim lect = cmd.ExecuteReader
            While lect.Read
                deconnexion()
                Return True
            End While
            deconnexion()
        End If
        Return False
    End Function

    Public Function obtenirIdAuditoire(nomAuditoire As String, nomDepartement As String, nomFaculte As String) As Integer
        connexion()
        Dim cmd As New OleDbCommand
        cmd.Connection = con
        Dim idFaculte As Integer = 0
        Dim idDepartement As Integer = 0
        Dim idAuditoire As Integer = 0
        Dim lect As OleDbDataReader

        cmd.CommandText = "SELECT id FROM EntiteAcademique WHERE nom='" & nomFaculte & "'"
        lect = cmd.ExecuteReader
        While lect.Read
            idFaculte = CType(lect("id").ToString, Integer)
        End While

        If idFaculte <> 0 Then
            cmd = New OleDbCommand
            cmd.Connection = con
            cmd.CommandText = "SELECT id FROM EntiteAcademique WHERE nom='" & nomDepartement & "' AND idParent=" & idFaculte
            lect = cmd.ExecuteReader
            While lect.Read
                idDepartement = CType(lect("id").ToString, Integer)
            End While

            If idDepartement <> 0 Then
                cmd = New OleDbCommand
                cmd.Connection = con
                cmd.CommandText = "SELECT id FROM EntiteAcademique WHERE nom='" & nomAuditoire & "' AND (idParent=" & idDepartement & " OR idParent=" & idFaculte & ")"
                lect = cmd.ExecuteReader
                While lect.Read
                    idAuditoire = CType(lect("id").ToString, Integer)
                End While

                If idAuditoire <> 0 Then
                    deconnexion()
                    Return idAuditoire
                End If
            End If
        End If
        deconnexion()
        Return 0
    End Function



    Private Function impressionCarte() As Boolean
        Return False
    End Function

    Public Function obtenirIdEtudiant(matricule As String) As Integer
        Dim cmd As New OleDbCommand
        cmd.Connection = con
        cmd.CommandText = "SELECT id FROM Etudiants WHERE numeroMatricule='" & matricule & "'"
        Dim lect As OleDbDataReader
        lect = cmd.ExecuteReader
        While lect.Read
            ' deconnexion()
            Return CType(lect("id").ToString, Integer)
        End While
        Return 0
    End Function

    Public Function chercherEntite(nom As String) As DataSet
        'MsgBox("Recherche de l'entite")
        connexion()
        Dim dt As New OleDbDataAdapter("SELECT * FROM EntiteAcademique WHERE nom LIKE '%" & nom & "%'", con)
        Dim ensDonnees As New DataSet()
        dt.Fill(ensDonnees, "EntiteAcademique")
        'ensDonnees.Tables("EntiteAcademique").Rows(1)
        Return ensDonnees
        deconnexion()
    End Function

    Public Function obtenirListeSignataires() As DataSet
        connexion()
        Dim dt As New OleDbDataAdapter("SELECT nomEtTitreAcademique AS noms,titre FROM Signataire ORDER BY id DESC", con)
        Dim ensDonnees As New DataSet()
        dt.Fill(ensDonnees, "Signataire")
        deconnexion()
        Return ensDonnees
    End Function

    Public Function obtenirListeTousEtudiants() As DataSet
        connexion()
        Dim dt As New OleDbDataAdapter("SELECT * FROM Etudiants ORDER BY nom,postNom,prenom DESC", con)
        Dim ensDonnees As New DataSet()
        dt.Fill(ensDonnees, "Etudiants")
        deconnexion()
        Return ensDonnees
    End Function

    Public Function obtenirListeEntete() As DataSet
        connexion()
        Dim dt As New OleDbDataAdapter("SELECT * FROM EntiteAcademique ORDER BY id DESC", con)
        Dim ensDonnees As New DataSet()
        dt.Fill(ensDonnees, "EntiteAcademique")
        deconnexion()
        Return ensDonnees
    End Function

    Public Function obtenirIdSignataire(nom As String, titre As String) As Integer
        connexion()
        Dim cmd As New OleDbCommand
        cmd.Connection = con
        cmd.CommandText = "SELECT id FROM Signataire WHERE nomEtTitreAcademique='" & nom & "' AND titre='" & titre & "'"
        Dim lect As OleDbDataReader
        lect = cmd.ExecuteReader
        Dim idSignataire As String = ""
        While lect.Read
            idSignataire = lect("id")
        End While
        deconnexion()
        Return CType(idSignataire, Integer)
    End Function

    Public Function verifierExistanceMatricule(matricule As String) As Boolean
        connexion()
        Dim cmd As New OleDbCommand
        Dim lect As OleDbDataReader
        cmd.Connection = con
        cmd.CommandText = "SELECT * FROM Etudiants WHERE numeroMatricule='" & matricule & "'"
        lect = cmd.ExecuteReader
        While lect.Read
            deconnexion()
            Return True
            'MsgBox(lect("nom").ToString)
        End While

        deconnexion()
        Return False
    End Function

    Public Function obtenirNationalite() As String()
        connexion()
        Dim cmd, cmd1 As New OleDbCommand
        Dim lect As OleDbDataReader
        cmd1.Connection = con
        cmd1.CommandText = "SELECT DISTINCT(nationalite) FROM Etudiants"
        lect = cmd1.ExecuteReader
        Dim i As Integer = 0
        While lect.Read
            i += 1
        End While
        ' MsgBox(i)
        cmd.Connection = con
        cmd.CommandText = "SELECT DISTINCT(nationalite) FROM Etudiants"
        lect = cmd.ExecuteReader
        Dim tabAretourner(i) As String
        i = 0
        While lect.Read
            tabAretourner(i) = lect("Nationalite").ToString
            i += 1
        End While
        deconnexion()
        Return tabAretourner
    End Function

    Public Function obtenirTypeEntite(idEntite As Integer) As String
        connexion()
        Dim cmd As New OleDbCommand
        Dim lect As OleDbDataReader
        Dim typeEntite As String = ""
        cmd.Connection = con
        cmd.CommandText = "SELECT type FROM EntiteAcademique WHERE id=@idEntite"
        cmd.Parameters.Add("@idEntite", OleDbType.VarChar, 20, "id").Value = idEntite
        lect = cmd.ExecuteReader
        While lect.Read
            typeEntite = lect("type")
        End While
        'deconnexion()
        Return typeEntite
    End Function

    Public Function obtenirEntites() As String()
        connexion()
        Dim cmd, cmd1 As New OleDbCommand
        Dim lect As OleDbDataReader
        cmd1.Connection = con
        cmd1.CommandText = "SELECT id FROM EntiteAcademique"
        lect = cmd1.ExecuteReader
        Dim i As Integer = 0
        While lect.Read
            i += 1
        End While
        ' MsgBox(i)
        cmd.Connection = con
        cmd.CommandText = "SELECT nom,id,idparent FROM EntiteAcademique"
        lect = cmd.ExecuteReader
        Dim tabAretourner(i) As String
        i = 0
        While lect.Read
            tabAretourner(i) = lect("nom").ToString & " - " & lect("id").ToString
            i += 1
        End While
        deconnexion()
        Return tabAretourner
    End Function

    Public Function obtenirDonneesCarte(matricule As String, idSignateur As Integer) As DonneesCarte
        connexion()
        Dim cmd As New OleDbCommand
        Dim lect As OleDbDataReader
        Dim dc As New DonneesCarte
        Dim lienRelatifImage As String = "C:\Users\Athena\Documents\Visual Studio 2012\Projects\Student\Student\bin\Debug"

        dc.matricule = matricule
        dc.lienPhoto = lienRelatifImage & "photos\" & matricule & ".jpg"

        cmd.Connection = con
        cmd.CommandText = "SELECT nom,postNom,prenom,lieuNaissance,DAY(dateNaissance) AS leJour, MONTH(dateNaissance) AS leMois,YEAR(dateNaissance) AS annee,nationalite FROM Etudiants WHERE numeroMatricule='" & matricule & "'"
        lect = cmd.ExecuteReader

        While lect.Read
            dc.noms = lect("nom").ToString()
            dc.noms &= " " & lect("postNom").ToString()
            dc.noms &= " " & lect("prenom").ToString()
            dc.lieuNaissance = lect("lieuNaissance").ToString()
            dc.dateNaissance = lect("leJour").ToString() & "/" & lect("leMois").ToString() & "/" & lect("annee").ToString()
            'lect("typeEtudiant").ToString()
            'lect("house").ToString()
            dc.nationalite = lect("nationalite").ToString()
        End While

        Dim cmd2, cmd3 As New OleDbCommand
        cmd2.Connection = con
        cmd2.CommandText = "SELECT * FROM sInscrit WHERE numeroMatricule='" & matricule & "'"
        lect = cmd2.ExecuteReader

        Dim lect2 As OleDbDataReader
        Dim nomEntiteParent As String = ""
        While lect.Read

            lect("idEntiteAcademique").ToString()
            lect("anneeAcademique").ToString()
            'Obtenir le nom de l'entite et son type

            cmd3 = New OleDbCommand
            cmd3.Connection = con
            cmd3.CommandText = "SELECT * FROM EntiteAcademique WHERE id=" & lect("idEntiteAcademique").ToString()
            lect2 = cmd3.ExecuteReader

            'C'est le departement
            dc.departement = nomEntiteParent
            connexion()
            'Parce qu'il est probable que la fonction precedente aie fermee la connexion

            While lect2.Read
                dc.auditoire = lect2("nom").ToString()
                nomEntiteParent = obtenirNomParent(lect2("idParent").ToString())
                'C'est le nom de l'entite academique

            End While

            'lect("typeEntiteAcademique").ToString()
            'lect("").ToString()

        End While


        'Les donnees sur le signataire

        Dim cmd4 As New OleDbCommand
        cmd4.Connection = con
        cmd4.CommandText = "SELECT * FROM Signataire WHERE id=" & idSignateur
        lect = cmd4.ExecuteReader
        While lect.Read
            dc.lienSignature = lienRelatifImage & "signatures\" & idSignateur & ".jpg"
            dc.nomSignateur = lect("nomEtTitreAcademique").ToString()
            dc.titreSignateur = lect("titre").ToString()
        End While
        deconnexion()
        Return dc

    End Function

    Public Function obtenirEntitesEtParent() As String()
        connexion()
        Dim cmd, cmd1 As New OleDbCommand
        Dim lect As OleDbDataReader
        cmd1.Connection = con
        cmd1.CommandText = "SELECT id FROM EntiteAcademique"
        lect = cmd1.ExecuteReader
        Dim i As Integer = 0
        While lect.Read
            i += 1
        End While
        ' MsgBox(i)
        cmd.Connection = con
        cmd.CommandText = "SELECT nom,id,idParent FROM EntiteAcademique"
        lect = cmd.ExecuteReader
        Dim tabAretourner(i) As String
        i = 0
        Dim nomParent As String = ""
        Dim idParent As String = ""
        Dim nomElt As String = ""
        Dim idElt As String = ""
        While lect.Read
            idElt = lect("id").ToString
            nomElt = lect("nom").ToString
            idParent = lect("idParent").ToString
            nomParent = obtenirNomParent(idParent)
            

            'MsgBox(idParent & " et " & parent)
            If idParent = "0" Then
                tabAretourner(i) = nomElt & " -  - " & idElt
            Else
                tabAretourner(i) = nomElt & " - " & nomParent & " - " & idElt
            End If

            i += 1
        End While
        deconnexion()
        Return tabAretourner
    End Function

    Private Function obtenirNomParent(idParent As String) As String
        'connexion() //On n'ouvre pas la connexion parce qu'elle est deja ouverte
        Dim cmd As New OleDbCommand
        cmd.Connection = con
        cmd.CommandText = "SELECT nom FROM EntiteAcademique WHERE id=" & idParent
        Dim lect As OleDbDataReader
        lect = cmd.ExecuteReader
        Dim elt As String = " "
        While lect.Read
            elt = lect("nom").ToString()
            'deconnexion()
            Return elt
        End While
        'deconnexion()

        Return vbNullString
    End Function

    Public Function enregistrerSignataires(nom As String, titre As String, Optional signature As Boolean = True) As Boolean
        connexion()
        Dim cmd As New OleDbCommand
        cmd.Connection = con
        cmd.CommandText = "INSERT INTO Signataire(nomEtTitreAcademique,titre,signatureImportee) VALUES(@nom,@titre,@signatureImportee)"
        cmd.Parameters.Add("@nom", OleDbType.VarChar, 255, "nomEtTitreAcademique").Value = nom
        cmd.Parameters.Add("@titre", OleDbType.VarChar, 255, "titre").Value = titre
        cmd.Parameters.Add("@signatureImportee", OleDbType.Boolean, 20, "signatureImportee").Value = signature
        cmd.ExecuteNonQuery()
        deconnexion()
        Return True
    End Function

    Public Function obtenirAnneeAcademiques() As String()
        connexion()
        Dim cmd, cmd1 As New OleDbCommand
        Dim lect As OleDbDataReader
        Dim i As Integer = 0
        cmd1.Connection = con
        cmd1.CommandText = "SELECT DISTINCT(anneeAcademique) FROM sInscrit"
        lect = cmd1.ExecuteReader
        While lect.Read
            i += 1
        End While
        cmd.Connection = con
        cmd.CommandText = "SELECT DISTINCT(anneeAcademique) FROM sInscrit"

        lect = cmd.ExecuteReader
        Dim elts(i) As String
        i = 0
        While lect.Read
            elts(i) = lect("anneeAcademique")
        End While
        deconnexion()
        Return elts
    End Function


    Public Function obtenirSignataires() As String()
        connexion()
        Dim cmd, cmd1 As New OleDbCommand
        Dim lect As OleDbDataReader
        Dim i As Integer = 0
        cmd1.Connection = con
        cmd1.CommandText = "SELECT nomEtTitreAcademique,titre FROM Signataire"
        lect = cmd1.ExecuteReader
        While lect.Read
            i += 1
        End While

        cmd.Connection = con
        cmd.CommandText = "SELECT id,nomEtTitreAcademique,titre FROM Signataire"

        lect = cmd.ExecuteReader
        Dim elts(i) As String
        i = 0
        While lect.Read
            elts(i) = lect("nomEtTitreAcademique").ToString & " - " & lect("titre").ToString & " - " & lect("id").ToString
            'MsgBox(elts(i))
            i += 1
        End While
        deconnexion()
        Return elts
    End Function

    Public Function obtenirDerniereClassse(matricule As String) As String
        connexion()
        Dim cmd As New OleDbCommand
        Dim lect As OleDbDataReader
        cmd.Connection = con
        cmd.CommandText = "SELECT idEntiteAcademique,nom FROM sInscrit INNER JOIN EntiteAcademique ON sInscrit.idEntiteAcademique = EntiteAcademique.id WHERE numeroMatricule='" & matricule & "'"
        lect = cmd.ExecuteReader
        Dim entiteAcademique As String = ""
        While lect.Read
            entiteAcademique = lect("nom")
        End While
        deconnexion()
        Return entiteAcademique
    End Function

    Public Function obtenirSignataire(nom As String) As DataSet
        connexion()
        Dim dt As New OleDbDataAdapter("SELECT * FROM Signataire WHERE nomEtTitreAcademique LIKE '%" & nom & "%'", con)
        Dim ensDonnees As New DataSet()
        dt.Fill(ensDonnees, "Signataire")
        deconnexion()
        Return ensDonnees
    End Function

    Public Function modifierSignataire(idSignataire As Integer, nom As String, titre As String, Optional signature As Boolean = True) As Boolean
        connexion()
        Dim cmd As New OleDbCommand
        cmd.Connection = con
        cmd.CommandText = "UPDATE Signataire SET nomEtTitreAcademique=@nom,titre=@titre,signatureImportee=@signatureImportee WHERE id=@idSignataire"
        cmd.Parameters.Add("@nom", OleDbType.VarChar, 255, "nomEtTitreAcademique").Value = nom
        cmd.Parameters.Add("@titre", OleDbType.VarChar, 255, "titre").Value = titre
        cmd.Parameters.Add("@signatureImportee", OleDbType.Boolean, 20, "signatureImportee").Value = signature
        cmd.Parameters.Add("@idSignataire", OleDbType.Integer, 20, "id").Value = idSignataire
        cmd.ExecuteNonQuery()
        deconnexion()
        Return True
    End Function

    Public Function supprimerSignataire(idSignataire As Integer) As Boolean
        connexion()
        Dim cmd As New OleDbCommand
        cmd.Connection = con
        cmd.CommandText = "DELETE FROM Signataire WHERE id=@idSignataire"
        cmd.Parameters.Add("@idSignataire", OleDbType.Integer, 20, "id").Value = idSignataire
        cmd.ExecuteNonQuery()
        deconnexion()
        Return True
    End Function

    Public Function enregistrerEntite(nom As String, type As String, idParent As String, typeParent As String, Optional categorie As Integer = 1) As Boolean
        If verifierExistanceEntite(nom, idParent) = False Then
            connexion()
            Dim cmd As New OleDbCommand
            cmd.Connection = con
            cmd.CommandText = "INSERT INTO EntiteAcademique(nom,type,idParent,typeParent,categorie) VALUES(@nom,@type,@idParent,@typeParent,@categorie)"
            cmd.Parameters.Add(" @nom", OleDbType.VarChar, 20, "nom").Value = nom
            cmd.Parameters.Add("@type", OleDbType.VarChar, 20, "type").Value = type
            cmd.Parameters.Add("@idParent", OleDbType.Integer, 20, "idParent").Value = CType(idParent, Integer)
            cmd.Parameters.Add("@typeParent", OleDbType.VarChar, 20, "typeParent").Value = typeParent
            cmd.Parameters.Add("@categorie", OleDbType.VarChar, 30, "categorie").Value = categorie
            cmd.ExecuteNonQuery()
            deconnexion()
            Return True
        Else
            Return False
        End If
    End Function

    Public Function obtenirUneEntite() As String()
        connexion()
        Dim cmd, cmd1 As New OleDbCommand
        Dim lect As OleDbDataReader
        cmd1.Connection = con
        cmd1.CommandText = "SELECT DISTINCT(numeroMatricule) FROM Etudiants"
        lect = cmd1.ExecuteReader
        Dim i As Integer = 0
        While lect.Read
            i += 1
        End While
        'MsgBox(i)
        cmd.Connection = con
        cmd.CommandText = "SELECT DISTINCT(numeroMatricule) FROM Etudiants"
        lect = cmd.ExecuteReader
        Dim tabAretourner(i) As String
        i = 0
        While lect.Read
            tabAretourner(i) = lect("numeroMatricule").ToString
            i += 1
        End While
        deconnexion()
        Return tabAretourner

    End Function

    Public Function modifierEntite(idEntite As Integer, nom As String, type As String, idParent As String, typeParent As String, Optional categorie As Integer = 1) As Boolean
        If verifierExistanceEntite(nom, idParent) = True Then
            connexion()
            Dim cmd As New OleDbCommand
            cmd.Connection = con
            cmd.CommandText = "UPDATE EntiteAcademique SET nom=@nom,type=@type,idParent=@idParent,typeParent=@typeParent,categorie=@categorie WHERE id=@idEntite"
            cmd.Parameters.Add(" @nom", OleDbType.VarChar, 20, "nom").Value = nom
            cmd.Parameters.Add("@type", OleDbType.VarChar, 20, "type").Value = type
            cmd.Parameters.Add("@idParent", OleDbType.VarChar, 20, "idParent").Value = idParent
            cmd.Parameters.Add("@typeParent", OleDbType.VarChar, 20, "typeParent").Value = typeParent
            cmd.Parameters.Add("@categorie", OleDbType.VarChar, 30, "categorie").Value = categorie
            cmd.Parameters.Add("@idEntite", OleDbType.Integer, 30, "id").Value = idEntite
            cmd.ExecuteNonQuery()
            deconnexion()
            Return True
        Else
            Return False
        End If
    End Function

    Public Function obtenirIdentiteEtudiant(numeroMatricule As String) As String()
        Dim tabAretourner(9) As String
        If numeroMatricule <> "" Then
            connexion()
            Dim cmd As New OleDbCommand
            Dim lect As OleDbDataReader
            cmd.Connection = con
            cmd.CommandText = "SELECT * FROM Etudiants WHERE numeroMatricule='" & numeroMatricule & "'"
            lect = cmd.ExecuteReader

            Dim i As Integer = 0
            While lect.Read
                tabAretourner(0) = lect("id").ToString
                tabAretourner(1) = lect("numeroMatricule").ToString
                tabAretourner(2) = lect("nom").ToString
                tabAretourner(3) = lect("postNom").ToString
                tabAretourner(4) = lect("prenom").ToString
                tabAretourner(5) = lect("lieuNaissance").ToString
                tabAretourner(6) = lect("dateNaissance").ToString
                tabAretourner(7) = lect("house").ToString
                tabAretourner(8) = lect("nationalite").ToString
                i += 1
            End While
            deconnexion()
        End If
        Return tabAretourner
    End Function

    Public Function obtenirContactsEtudiant(numeroMatricule As String) As DataSet
        Dim ensDonnees As New DataSet()
        If numeroMatricule <> "" Then
            connexion()
            Dim dt As New OleDbDataAdapter("SELECT id AS Numero,type AS Type,valeur AS Valeur FROM Contact WHERE numeroMatricule='" & numeroMatricule & "' ORDER BY id DESC", con)
            'Dim dt As New OleDbDataAdapter("SELECT * FROM Contact", con)

            dt.Fill(ensDonnees, "Contact")
            'ensDonnees.Tables("EntiteAcademique").Rows(1)
           
        End If
        deconnexion()
        Return ensDonnees

    End Function

    Public Function obtenirDonneesContacts(idContact As String) As DataSet
        Dim ensDonnees As New DataSet()
        If idContact <> "" Then
            connexion()
            Dim dt As New OleDbDataAdapter("SELECT type,valeur FROM Contact WHERE id=" & idContact, con)

            dt.Fill(ensDonnees, "Contact")


        End If
        deconnexion()
        Return ensDonnees
    End Function

    Public Function ajouterContact(numeroMatricule As String, type As String, valeur As String) As Boolean
        If numeroMatricule <> "" And type <> "" And valeur <> "" Then
            connexion()
            Dim cmd As New OleDbCommand
            cmd.Connection = con
            cmd.CommandText = "INSERT INTO Contact(numeroMatricule,type,valeur) VALUES('" & numeroMatricule & "','" & type & "','" & valeur & "')"
            cmd.ExecuteNonQuery()
            deconnexion()
            Return True
        Else
            Return False
        End If
    End Function

    Public Function supprimerContact(idContact As String) As Boolean
        If idContact <> "" Then
            connexion()
            Dim cmd As New OleDbCommand
            cmd.Connection = con
            cmd.CommandText = "DELETE FROM Contact WHERE id=" & idContact & ""
            cmd.ExecuteNonQuery()
            deconnexion()
            Return True
        Else
            Return False
        End If
    End Function

    Public Function mettreAJourContact(idContact As String, numeroMatricule As String, type As String, valeur As String) As Boolean
        If idContact <> "" And numeroMatricule <> "" And type <> "" And valeur <> "" Then
            connexion()
            Dim cmd As New OleDbCommand
            cmd.Connection = con
            cmd.CommandText = "UPDATE Contact SET type='" & type & "',valeur='" & valeur & "',numeroMatricule='" & numeroMatricule & "' WHERE id=" & idContact
            ' MsgBox(cmd.CommandText)
            cmd.ExecuteNonQuery()
            deconnexion()
            Return True
        Else
            Return False
        End If
    End Function

    Public Function modifierEtudiant(idEtudiant As Integer, nom As String, postnom As String, prenom As String, type As String, idParent As String, typeParent As String, Optional categorie As Integer = 1) As Boolean
        If verifierExistanceEntite(nom, idParent) = True Then
            connexion()
            Dim cmd As New OleDbCommand
            cmd.Connection = con
            cmd.CommandText = "UPDATE EntiteAcademique SET nom=@nom,type=@type,idParent=@idParent,typeParent=@typeParent,categorie=@categorie WHERE id=@idEntite"
            cmd.Parameters.Add(" @nom", OleDbType.VarChar, 20, "nom").Value = nom
            cmd.Parameters.Add("@type", OleDbType.VarChar, 20, "type").Value = type
            cmd.Parameters.Add("@idParent", OleDbType.VarChar, 20, "idParent").Value = idParent
            cmd.Parameters.Add("@typeParent", OleDbType.VarChar, 20, "typeParent").Value = typeParent
            cmd.Parameters.Add("@categorie", OleDbType.VarChar, 30, "categorie").Value = categorie
            'cmd.Parameters.Add("@idEntite", OleDbType.Integer, 30, "id").Value = idEntite
            cmd.ExecuteNonQuery()
            deconnexion()
            Return True
        Else
            Return False
        End If
    End Function

    Public Function supprimerEtudiant(matricule As String) As Boolean
        If matricule <> "" Then
            connexion()
            Dim cmd As New OleDbCommand
            cmd.Connection = con
            cmd.CommandText = "DELETE FROM Etudiants WHERE numeroMatricule='" & matricule & "'"
            cmd.ExecuteNonQuery()
            deconnexion()
            Return True
        Else
            Return False
        End If
    End Function

    Public Function effacerEntite(idEntite As Integer) As Boolean
        connexion()
        Dim cmd As New OleDbCommand
        cmd.Connection = con
        cmd.CommandText = "DELETE FROM EntiteAcademique WHERE id=@idEntite"
        cmd.Parameters.Add("@idEntite", OleDbType.Integer, 30, "id").Value = idEntite
        cmd.ExecuteNonQuery()
        deconnexion()
        Return True
    End Function

    Public Function verifierExistanceEntite(nomEntite As String, idParent As Integer) As Boolean
        connexion()
        Dim cmd As New OleDbCommand
        Dim lect As OleDbDataReader
        cmd.Connection = con
        cmd.CommandText = "SELECT id FROM EntiteAcademique WHERE nom=@nom AND idParent=@idParent"
        cmd.Parameters.Add("@nom", OleDbType.VarChar, 20, "nom").Value = nomEntite
        cmd.Parameters.Add("@idParent", OleDbType.Integer, 20, "idParent").Value = idParent
        lect = cmd.ExecuteReader
        While lect.Read
            deconnexion()
            Return True
        End While
        deconnexion()
        Return False
    End Function

    Public Function obtenirMatricules() As String()
        connexion()
        Dim cmd, cmd1 As New OleDbCommand
        Dim lect As OleDbDataReader
        cmd1.Connection = con
        cmd1.CommandText = "SELECT DISTINCT(numeroMatricule) FROM Etudiants"
        lect = cmd1.ExecuteReader
        Dim i As Integer = 0
        While lect.Read
            i += 1
        End While
        ' MsgBox(i)
        cmd.Connection = con
        cmd.CommandText = "SELECT DISTINCT(numeroMatricule) FROM Etudiants"
        lect = cmd.ExecuteReader
        Dim tabAretourner(i) As String
        i = 0
        While lect.Read
            tabAretourner(i) = lect("numeroMatricule").ToString
            i += 1
        End While
        deconnexion()
        Return tabAretourner
    End Function

    Public Function obtenirNomsSignataire() As String()
        connexion()
        Dim cmd, cmd1 As New OleDbCommand
        Dim lect As OleDbDataReader
        cmd1.Connection = con
        cmd1.CommandText = "SELECT DISTINCT(nomEtTitreAcademique) FROM Signataire"
        lect = cmd1.ExecuteReader
        Dim i As Integer = 0
        While lect.Read
            i += 1
        End While
        ' MsgBox(i)
        cmd.Connection = con
        cmd.CommandText = "SELECT DISTINCT(nomEtTitreAcademique),id FROM Signataire"
        lect = cmd.ExecuteReader
        Dim tabAretourner(i) As String
        i = 0
        While lect.Read
            tabAretourner(i) = lect("nomEtTitreAcademique").ToString & " - " & lect("id").ToString
            i += 1
        End While
        deconnexion()
        Return tabAretourner
    End Function

    Private Function deconnexion() As Boolean
        con.Close()
        If con.State = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    '       FIN DE LA CONNEXION A LA BASE



    '       CREATION DES TABLES

    Public Function enregistrerDonneesDuDataGrid() As Boolean

        Return True
    End Function

    Public Function executerRequete(requette As String) As Boolean
        connexion()
        ' requette = "INSERT INTO Donnes_72(T_Nom,L_Age,T_Sexe,T_Methode) VALUES('Mathe','24','M','Uil')"
        Dim cmd1 As OleDbCommand = New OleDbCommand
        cmd1.Connection = con
        cmd1.CommandText = requette
        cmd1.ExecuteNonQuery()
        deconnexion()
        Return True
    End Function

    Public Function enleverEspace(chaineAtraiter As String) As String
        Dim nouvelleChaine As String = ""
        For i = 0 To chaineAtraiter.Length - 1
            If chaineAtraiter(i) = " " Then
                Continue For
            Else
                nouvelleChaine &= chaineAtraiter(i)
            End If
        Next
        Return nouvelleChaine
    End Function

    Public Function requeteStudent(degreDeLiberte As Double, seuil As Double) As Double
        connexion()
        Dim cmd, cmd2 As New OleDbCommand
        Dim lecteur, lecteur2 As OleDbDataReader

        Dim nomChampTrouver As String = ""
        Dim i As Integer

        cmd.Connection = con
        cmd.CommandText = "SELECT * FROM Student"
        'MsgBox(cmd.CommandText)
        lecteur = cmd.ExecuteReader
        lecteur.Read()
        For i = 2 To 10
            If (CType(lecteur("Champ" & i), Double) = seuil) Then
                nomChampTrouver = "Champ" & i

            End If
        Next

        cmd2.Connection = con
        cmd2.CommandText = "SELECT " & nomChampTrouver & " FROM Student"
        MsgBox(cmd2.CommandText)

        lecteur2 = cmd2.ExecuteReader
        'lecteur.Read()
        Dim j As Double = 0
        Dim tCritique As String = ""

        While lecteur2.Read
            tCritique = lecteur2(nomChampTrouver).ToString
            If j = degreDeLiberte Then
                Exit While
            End If

            j += 1
        End While

        'MsgBox(tCritique)

        connexion()
        Return tCritique
    End Function

    Public Function requetePearson(degreDeLiberte As Double, seuil As Double) As Double
        connexion()
        Dim cmd, cmd2 As New OleDbCommand
        Dim lecteur, lecteur2 As OleDbDataReader

        Dim nomChampTrouver As String = ""
        Dim i As Integer

        cmd.Connection = con
        cmd.CommandText = "SELECT * FROM Pearson"
        'MsgBox(cmd.CommandText)
        lecteur = cmd.ExecuteReader
        lecteur.Read()
        For i = 2 To 10
            If (CType(lecteur("Champ" & i), Double) = seuil) Then
                nomChampTrouver = "Champ" & i

            End If
        Next

        cmd2.Connection = con
        cmd2.CommandText = "SELECT " & nomChampTrouver & " FROM Pearson"

        lecteur2 = cmd2.ExecuteReader
        'lecteur.Read()
        Dim j As Double = 0
        Dim zCritique As String = ""

        While lecteur2.Read
            zCritique = lecteur2(nomChampTrouver).ToString
            If j = degreDeLiberte Then
                Exit While
            End If

            j += 1
        End While

        'MsgBox(tCritique)

        connexion()
        Return zCritique
    End Function



    Public Function ajouterClasses(tablesDesClasses() As String, id As String) As Boolean
        connexion()
        Dim cmd1 As New OleDbCommand
        Dim lecteur As OleDbDataReader
        Dim lesChamps As String = ""
        For i = 0 To tablesDesClasses.Length - 1
            If tablesDesClasses(i) <> "" Then
                lesChamps &= "," & tablesDesClasses(i)
            End If
            'MsgBox(lesChamps)
        Next

        cmd1.Connection = con
        cmd1.CommandText = "SELECT champsTravail FROM Travail WHERE id=" & id
        lecteur = cmd1.ExecuteReader
        lecteur.Read()
        ' MsgBox(lecteur("champsTravail").ToString())

        Dim cmd2 As New OleDbCommand
        cmd2.Connection = con
        cmd2.CommandText = "UPDATE Travail SET champsTravail='" & lecteur("champsTravail").ToString() & lesChamps & "' WHERE id=" & id
        'MsgBox(lecteur("champsTravail").ToString() & lesChamps)
        cmd2.ExecuteNonQuery()
        deconnexion()
        Return True
    End Function

    Public Function obtenirDonneesCompletes() As DataSet
        connexion()
        Dim dt As New OleDbDataAdapter("SELECT DISTINCT Etudiants.numeroMatricule,[Etudiants].[nom],Etudiants.postNom,Etudiants.prenom,sInscrit.idEntiteAcademique,EntiteAcademique.nom FROM (Etudiants INNER JOIN sInscrit ON Etudiants.numeroMatricule=sInscrit.numeroMatricule) INNER JOIN EntiteAcademique ON EntiteAcademique.id=sInscrit.idEntiteAcademique WHERE sInscrit.AnneeAcademique=2015", con)
        Dim ensDonnees As New DataSet()
        dt.Fill(ensDonnees)
        deconnexion()
        Return ensDonnees
    End Function

    Public Function obtenirDonneesSynchroniser() As String
        'Il faut trouver
        ' - NumeroMatricule
        ' - Nom
        ' - Postnom
        ' - Prenom
        ' - Sexe
        ' - FaculteEtudiant
        ' - Departement
        ' - Auditoire

        connexion()
        cmd = New OleDbCommand
        cmd.Connection = con
        cmd.CommandText = "SELECT numeroMatricule,nom,postNom,prenom FROM Etudiants"
        Dim lect As OleDbDataReader
        lect = cmd.ExecuteReader
        Dim chaine As String = ""
        Dim donneesInscription As String = ""
        While lect.Read
            donneesInscription = obtenirAuditoireEtudiantSynchr(lect("numeroMatricule").ToString)
            If donneesInscription <> "" Then
                chaine &= lect("numeroMatricule").ToString & "#"
                chaine &= lect("nom").ToString & "#"
                chaine &= lect("postNom").ToString & "#"
                chaine &= lect("prenom").ToString & "#"
                chaine &= obtenirAuditoireEtudiantSynchr(lect("numeroMatricule").ToString) & ";;"
                'chaine &= lect("sexe").ToString & ";;"
            End If
            donneesInscription = ""
        End While

        deconnexion()
        Return chaine
    End Function

    Private Function obtenirAuditoireEtudiantSynchr(matricule As String) As String
        'La connexion est ouverte ailleurs
        If matricule <> "" Then
            connexion()
            Dim cmd As New OleDbCommand
            cmd.Connection = con
            cmd.CommandText = "SELECT idEntiteAcademique FROM sInscrit WHERE numeroMatricule='" & matricule & "'"
            Dim lect As OleDbDataReader
            lect = cmd.ExecuteReader
            Dim idEntiteAcademique As String = ""
            While lect.Read
                idEntiteAcademique = lect("idEntiteAcademique").ToString
            End While
            If idEntiteAcademique <> "" Then
                cmd = New OleDbCommand
                cmd.Connection = con
                cmd.CommandText = "SELECT nom,type,idParent,typeParent FROM EntiteAcademique WHERE id=" & idEntiteAcademique
                lect = cmd.ExecuteReader
                Dim nomEntite As String = ""
                Dim idParent As String = ""
                Dim nomFaculte As String = ""
                Dim nomDepartement As String = ""

                Dim categorieParent As Integer 'Soit 1 -> Departement ou 2 -> Faculte

                While lect.Read
                    nomEntite = lect("nom").ToString()
                    idParent = lect("idParent").ToString
                    If lect("typeParent").ToString = "Département" Then
                        'Il y a un departement
                        categorieParent = 1
                    ElseIf lect("typeParent").ToString = "Faculté" Then
                        'Il n'y a qu'une faculte
                        categorieParent = 2
                    End If
                End While

                If idParent <> "" Then
                    If categorieParent = 1 Then
                        cmd = New OleDbCommand
                        cmd.Connection = con
                        cmd.CommandText = "SELECT nom,idParent FROM EntiteAcademique WHERE id=" & idParent
                        lect = cmd.ExecuteReader
                        Dim idFaculte As String = ""

                        While lect.Read
                            nomDepartement = lect("nom").ToString
                            idFaculte = lect("idParent").ToString
                        End While

                        If nomDepartement <> "" And idFaculte <> "" Then
                            cmd = New OleDbCommand
                            cmd.Connection = con
                            cmd.CommandText = "SELECT nom FROM EntiteAcademique WHERE id=" & idFaculte
                            lect = cmd.ExecuteReader

                            While lect.Read
                                nomFaculte = lect("nom").ToString
                            End While

                            If nomFaculte <> "" Then
                                Return "auditoire=" & nomEntite & "#departement=" & nomDepartement & "#faculte=" & nomFaculte
                            End If
                        End If
                    End If

                ElseIf categorieParent = 2 Then
                    cmd = New OleDbCommand
                    cmd.Connection = con
                    cmd.CommandText = "SELECT nom FROM EntiteAcademique WHERE id=" & idParent
                    lect = cmd.ExecuteReader
                    While lect.Read
                        nomFaculte = lect("nom").ToString
                    End While

                    If nomFaculte <> "" Then
                        Return "auditoire=" & nomEntite & "#departement=vide#faculte=" & nomFaculte
                    End If
                End If
            End If
        End If
        Return ""
    End Function

    Private Function creationTableDonnees(ByVal donnees As String(), ByVal idTravail As String) As Boolean
        Try
            Dim cmd1 As OleDbCommand = New OleDbCommand
            Dim req, req1, req2 As String
            Dim nbreElt As Integer
            nbreElt = donnees.Length
            Dim champs As String = ""
            Dim champ1 As String = ""
            Dim champDistribution As String = ""

            For i = 0 To nbreElt - 2
                champs &= donnees(i) & " CHAR(60), "
                champ1 &= donnees(i) & ","

            Next

            champs &= donnees(nbreElt - 1) & " CHAR(60)"
            champ1 &= donnees(nbreElt - 1)

            req = "CREATE TABLE Donnees_" & idTravail & " (" & champs & ")"
            req1 = "UPDATE Travail SET champsTravail=@champ, nbreChamps=@nbreChamp WHERE id=@idTravail"
            cmd1.Connection = con
            cmd1.CommandText = req
            cmd1.ExecuteNonQuery()
            cmd1.Connection = con
            cmd1.CommandText = req1
            cmd1.Parameters.Add("@champ", OleDbType.VarChar, 1000, "champsTravail").Value = champ1
            cmd1.Parameters.Add("@nbreChamp", OleDbType.Integer, 10, "nbreChamps").Value = nbreElt
            cmd1.Parameters.Add("@idTravail", OleDbType.Integer, 10, "id").Value = idTravail
            cmd1.ExecuteNonQuery()

            For i = 0 To nbreElt - 1
                If (donnees(i)(0) = "T") Then
                    req2 = "CREATE TABLE Distribution_" & donnees(i) & "_" & idTravail & "(" & donnees(i) & " VARCHAR(60),Ni FLOAT,Ni_croissant FLOAT,Ni_decroissant FLOAT,Fi FLOAT,Fi_croissant FLOAT,Fi_decroissant FLOAT)"
                    cmd1.Connection = con
                    cmd1.CommandText = req2
                    cmd1.ExecuteNonQuery()
                    'CREATION DES CLASSES DE CETTE DISTRIBUTION

                    req2 = "CREATE TABLE Distribution_Classes_" & donnees(i) & "_" & idTravail & "(Classes VARCHAR(60),CentreClasse FLOAT,Ni FLOAT,Ni_croissant FLOAT,Ni_decroissant FLOAT,Fi FLOAT,Fi_croissant FLOAT,Fi_decroissant FLOAT,amplitude FLOAT,eltMin FLOAT,eltMax FLOAT)"
                    cmd1.Connection = con
                    cmd1.CommandText = req2
                    cmd1.ExecuteNonQuery()

                ElseIf (donnees(i)(0) = "L") Then
                    req2 = "CREATE TABLE Distribution_" & donnees(i) & "_" & idTravail & "(" & donnees(i) & " VARCHAR(60),Ni FLOAT,Ni_croissant FLOAT,Ni_decroissant FLOAT,Fi FLOAT,Fi_croissant FLOAT,Fi_decroissant FLOAT)"
                    cmd1.Connection = con
                    cmd1.CommandText = req2
                    cmd1.ExecuteNonQuery()
                End If

            Next
        Catch e As OleDbException
            'Dim x As New Design
            'x.afficherErreurDescriptive("Une erreur fatale. Vérifiez que vous avez bien entré les noms des colonnes et que deux colonnes n'ont pas le même nom.")
        End Try
        MsgBox("Requete avec succes")
        Return True
    End Function


    '       FIN MODIFICATION DES TABLES
End Class

